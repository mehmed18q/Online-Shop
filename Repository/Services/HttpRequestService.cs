using Framework;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Net;
using System.Text;

namespace Repository
{
    public class HttpRequestService : IHttpRequest
    {
        private readonly IHttpContextAccessor _context;
        private readonly IConfiguration _configuration;
        private readonly IMemoryCache _cache;
        private readonly HttpClient _client;

        public HttpRequestService(HttpClient httpClient,
            IHttpContextAccessor context,
            IConfiguration configuration,
            IMemoryCache cache
            )
        {
            _context = context;
            _client = httpClient;
            _configuration = configuration;
            _cache = cache;
        }

        private void SetToken(string? token = null)
        {
            if (token == null)
            {
                token = "04386FD8-AA96-46BC-AF7A-FE0CE5CE37E1";
            }
            if (_client.DefaultRequestHeaders.TryGetValues("xUnit", out IEnumerable<string>? values))
            {
                token = values.First();
            }

            if (!string.IsNullOrEmpty(token))
            {
                if (_client.DefaultRequestHeaders.Contains("Authorization"))
                {
                    _client.DefaultRequestHeaders.Remove("Authorization");
                }
                _client.DefaultRequestHeaders.Add("Authorization", token);
            }

            if (_client.DefaultRequestHeaders.Contains("PageCode"))
            {
                _client.DefaultRequestHeaders.Remove("PageCode");
            }
            var client = _context?.HttpContext?.Request.Path;
            if (client != null)
            {
                var splitedUrl = client?.ToString().Split("/")[1];
                _client.DefaultRequestHeaders.Add("PageCode", splitedUrl);
            }
        }

        private void Cache(string url, string type, object? data, string result, string timer = "")
        {
            if (_configuration["IsDebug"] == "1")
            {
                var list = _cache.Get<List<ApiCacheVM>>("apiCache");
                list ??= new List<ApiCacheVM>();
                if (!list.ToAnyTry() || (list != null && list.Count > 100))
                {
                    list = new List<ApiCacheVM>();
                }
                var model = new ApiCacheVM
                {
                    Api = url,
                    Type = type,
                    Timer = timer,
                    Result = result,
                    Json = JsonConvert.SerializeObject(data),
                };

                list?.Add(model);
                _cache.Set("apiCache", list, DateTimeOffset.UtcNow.AddHours(1));
            }
        }

        public async Task<ApiResult> ReturnResult(HttpResponseMessage httpresponse)
        {
            var responseStream = await httpresponse.Content.ReadAsStringAsync();

            if (httpresponse.StatusCode == HttpStatusCode.InternalServerError)
                return new ApiResult(false, "Internal Server Error");

            var jsonObj = JObject.Parse(responseStream.ToString());
            var message = GetMessage(jsonObj);

            if (httpresponse.IsSuccessStatusCode && jsonObj != null)
            {
                var errors = jsonObj["errors"];
                if (errors != null)
                {
                    var error = !errors.HasValues ? string.Empty : errors.ToString();
                    return new ApiResult(true, message, error);
                }
                return new ApiResult(false, message);
            }
            else
                return new ApiResult(false, message);
        }

        private static string GetMessage(JObject data)
        {
            if (data["status"] != null && data["status"]?.ToString() != "2")
            {
                var result = string.Empty;
                try
                {
                    if (data["errors"] != null)
                    {
                        var json = JsonConvert.DeserializeObject<Dictionary<string, object>>(data["errors"]?.ToString() ?? string.Empty);
                        if (json == null)
                            return Convert.ToString(data["message"]) ?? string.Empty;

                        foreach (var item in json)
                        {
                            var child = JsonConvert.DeserializeObject<Dictionary<string, string>>(item.Value.ToString() ?? string.Empty);
                            if (child != null)
                                foreach (var itemchild in child)
                                    result += $"<p>{itemchild.Value}</p>";
                        }

                        return result;
                    }
                }
                catch
                {
                    return data["errors"]?.ToString() ?? string.Empty;
                }
            }

            return Convert.ToString(data["message"]) ?? string.Empty;
        }

        private static async Task<ApiResult<TResult>> ReturnResult<TResult>(HttpResponseMessage httpResponse)
        {
            var responseStream = await httpResponse.Content.ReadAsStringAsync();

            if (httpResponse.StatusCode == HttpStatusCode.InternalServerError || string.IsNullOrEmpty(responseStream.ToString()))
                return new ApiResult<TResult>(false, "Internal Server Error.", string.Empty, httpResponse.StatusCode, 0, Activator.CreateInstance<TResult>());

            var jsonObj = JObject.Parse(responseStream.ToString());
            var message = GetMessage(jsonObj);

            if (httpResponse.IsSuccessStatusCode)
            {
                var error = jsonObj["errors"]?.ToString();
                bool v = int.TryParse(jsonObj["count"]?.ToString(), out int count);

                var data = jsonObj["data"]?.ToString();

                var result = ((string.IsNullOrEmpty(jsonObj.ToString()) || string.IsNullOrEmpty(data)) ? Activator.CreateInstance<TResult>() : JsonConvert.DeserializeObject<TResult>(data));
                return new ApiResult<TResult>(true, message, error ?? string.Empty, httpResponse.StatusCode, v ? count : 0, result);
            }
            else
                return new ApiResult<TResult>(false, message, string.Empty, httpResponse.StatusCode, 0, Activator.CreateInstance<TResult>());
        }

        private static async Task<List<TResult>> ListReturnResult<TResult>(HttpResponseMessage httpResponse)
        {
            var responseStream = await httpResponse.Content.ReadAsStringAsync();

            var jsonObj = JObject.Parse(responseStream.ToString());

            string data = jsonObj["data"]?.ToString() ?? string.Empty;
            var result = JsonConvert.DeserializeObject<List<TResult>>(data);
            result ??= new List<TResult>();
            return result;
        }

        private static HttpContent Serialize(object data) => new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");

        public async Task<ApiResult> DeleteAsync(string apiUrl)
        {
            SetToken();
            var response = await _client.DeleteAsync(apiUrl);
            return await ReturnResult(response);
        }

        public async Task<ApiResult> DeleteByIdAsync(string apiUrl, long id)
        {
            SetToken();
            var data = new
            {
                Id = id,
                PageSize = 1,
                PageNumber = 1
            };

            using var Content = Serialize(data);
            using var httpReq = new HttpRequestMessage(HttpMethod.Delete, apiUrl) { Content = Content };
            var response = await _client.SendAsync(httpReq);
            var responseStream = await response.Content.ReadAsStringAsync();
            Cache(apiUrl, "DeleteById", data, responseStream.ToString());

            return await ReturnResult(response);
        }

        public async Task<ApiResult> DeleteByIdAsync(string apiUrl, object data)
        {
            SetToken();
            using var Content = Serialize(data);
            using var httpReq = new HttpRequestMessage(HttpMethod.Delete, apiUrl) { Content = Content };
            var response = await _client.SendAsync(httpReq);

            return await ReturnResult(response);
        }

        public async Task<ApiResult<TResult>> DeleteAsync<TResult>(string apiUrl)
        {
            SetToken();
            var response = await _client.DeleteAsync(apiUrl);

            return await ReturnResult<TResult>(response);
        }

        public async Task<ApiResult<TResult>> PutAsync<TResult>(string apiUrl, object data)
        {
            SetToken();

            var _data = Serialize(data);
            var response = await _client.PutAsync(apiUrl, _data);

            var responseStream = await response.Content.ReadAsStringAsync();
            Cache(apiUrl, "Put", data, responseStream.ToString());

            if (response.StatusCode == HttpStatusCode.Unauthorized)
                throw new UnauthorizedAccessException();

            return await ReturnResult<TResult>(response);
        }

        public async Task<ApiResult> PutAsync(string apiUrl, object data)
        {
            SetToken();

            var _data = Serialize(data);
            var response = await _client.PutAsync(apiUrl, _data);

            var responseStream = await response.Content.ReadAsStringAsync();
            Cache(apiUrl, "Put", data, responseStream.ToString());

            if (response.StatusCode == HttpStatusCode.Unauthorized)
                throw new UnauthorizedAccessException(responseStream.ToString());

            return await ReturnResult(response);
        }

        public async Task<ApiResult<TResult>> PostAsync<TResult>(string apiUrl, object data, string? token = null)
        {
            SetToken(token);
            var timer = new Stopwatch();
            timer.Start();

            var _data = Serialize(data);
            var response = await _client.PostAsync(apiUrl, _data);

            var responseStream = await response.Content.ReadAsStringAsync();


            if (response.StatusCode == HttpStatusCode.Unauthorized) throw new UnauthorizedAccessException(responseStream);

            var result = await ReturnResult<TResult>(response);
            timer.Stop();

            return result;
        }

        public async Task<List<TResult>> ListPostAsync<TResult>(string apiUrl, object data, string? token)
        {
            SetToken(token);
            var timer = new Stopwatch();
            timer.Start();

            var _data = Serialize(data);
            var response = await _client.PostAsync(apiUrl, _data);

            var responseStream = await response.Content.ReadAsStringAsync();


            if (response.StatusCode == HttpStatusCode.Unauthorized) throw new UnauthorizedAccessException(responseStream);

            var result = await ListReturnResult<TResult>(response);
            timer.Stop();

            return result;
        }

        public async Task<ApiResult<TResult>> GetWithCustomTokenAsync<TResult>(string apiUrl, string token)
        {
            if (_client.DefaultRequestHeaders.Contains("Authorization")) _client.DefaultRequestHeaders.Remove("Authorization");

            _client.DefaultRequestHeaders.Add("Authorization", token);

            var request = new HttpRequestMessage(HttpMethod.Get, apiUrl);
            var response = await _client.SendAsync(request);

            return await ReturnResult<TResult>(response);
        }

        public async Task<ApiResult<TResult>> PostWithCustomTokenAsync<TResult>(string apiUrl, object data, string token)
        {
            if (_client.DefaultRequestHeaders.Contains("Authorization")) _client.DefaultRequestHeaders.Remove("Authorization");

            _client.DefaultRequestHeaders.Add("Authorization", token);
            var _data = Serialize(data);
            var response = await _client.PostAsync(apiUrl, _data);

            var responseStream = await response.Content.ReadAsStringAsync();

            Cache(apiUrl, "PostWithToken", $"{_client.BaseAddress} - {token}", responseStream);
            if (response.StatusCode == HttpStatusCode.Unauthorized)
                throw new UnauthorizedAccessException(responseStream.ToString());

            return await ReturnResult<TResult>(response);
        }

        public async Task<ApiResult<object>> PostFileAsync(string apiUrl, HttpContent data)
        {
            SetToken();
            var response = await _client.PostAsync(apiUrl, data);

            var responseStream = await response.Content.ReadAsStringAsync();
            Cache(apiUrl, "PostFileAsync", string.Empty, responseStream);

            var jsonObj = JObject.Parse(responseStream.ToString());
            var message = GetMessage(jsonObj);

            if (response.IsSuccessStatusCode)
            {
                var errors = jsonObj["errors"];
                if (errors != null)
                {
                    var error = !errors.HasValues ? string.Empty : errors?.ToString();
                    return new ApiResult<object>(true, message, error ?? string.Empty, jsonObj["data"]?.ToString() ?? string.Empty);
                }
                return new ApiResult<object>(false, message, null);
            }
            else
                return new ApiResult<object>(false, message, null);
        }

        public async Task<ApiResult> PostAsync(string apiUrl, object data)
        {
            SetToken();
            var timer = new Stopwatch();
            timer.Start();

            var _data = Serialize(data);
            var response = await _client.PostAsync(apiUrl, _data);
            var responseStream = await response.Content.ReadAsStringAsync();

            if (response.StatusCode == HttpStatusCode.Unauthorized)
                throw new UnauthorizedAccessException();

            var result = await response.Content.ReadAsStringAsync();

            timer.Stop();
            Cache(apiUrl, "Post", data, responseStream, $"{timer.Elapsed.Seconds}:{timer.Elapsed.Milliseconds}");

            return new ApiResult<object>(true, result, result);
        }

        public async Task<bool> CheckPictureAsync(string apiUrl)
        {
            SetToken();
            var request = new HttpRequestMessage(HttpMethod.Get, apiUrl);
            var response = await _client.SendAsync(request);

            return response.StatusCode == HttpStatusCode.OK;
        }

        public async Task<ApiResult> GetAsync(string apiUrl)
        {
            SetToken();
            var request = new HttpRequestMessage(HttpMethod.Get, apiUrl);
            var response = await _client.SendAsync(request);

            return await ReturnResult(response);
        }

        public async Task<ApiResult<TResult>> GetAsync<TResult>(string apiUrl)
        {
            SetToken();

            var request = new HttpRequestMessage(HttpMethod.Get, apiUrl);
            var response = await _client.SendAsync(request);
            var responseStream = await response.Content.ReadAsStringAsync();
            Cache(apiUrl, "Get", null, responseStream.ToString());

            return await ReturnResult<TResult>(response);
        }

        public async Task<ApiResult<byte[]>> GetApiResultAsync<TResult>(string apiUrl)
        {
            SetToken();
            var request = new HttpRequestMessage(HttpMethod.Get, apiUrl);
            var response = await _client.SendAsync(request);
            if (!response.IsSuccessStatusCode)
                return new ApiResult<byte[]>(false, response?.ReasonPhrase ?? string.Empty, default);

            var responseStream = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<GetApiResultVM>(responseStream);

            return new ApiResult<byte[]>(result?.IsSuccess ?? false, result?.Message ?? string.Empty, result?.Data);
        }

        public async Task<ApiResult<object>> GetDataAsync(string apiUrl)
        {
            SetToken();
            var request = new HttpRequestMessage(HttpMethod.Get, apiUrl);
            var response = await _client.SendAsync(request);

            var responseStream = await response.Content.ReadAsStringAsync();

            var jsonObj = JObject.Parse(responseStream.ToString());
            if (response.IsSuccessStatusCode)
            {
                var errors = jsonObj["errors"];
                if (errors != null)
                {
                    var error = !errors.HasValues ? string.Empty : errors.ToString();
                    var message = Convert.ToString(jsonObj["message"]);
                    if (jsonObj["status"]?.ToString() == "1")
                        message = Convert.ToString(jsonObj["errors"]);

                    var data = jsonObj["data"]?.ToString();
                    return new ApiResult<object>(true, message ?? string.Empty, error, data ?? string.Empty);
                }

                return new ApiResult<object>(false, jsonObj["message"]?.ToString() ?? string.Empty, null);
            }
            else
                return new ApiResult<object>(false, jsonObj["message"]?.ToString() ?? string.Empty, null);
        }

        public async Task<HttpResponseMessage> PostAsJObject(string apiUrl, object data)
        {
            SetToken();

            var _data = Serialize(data);
            var response = await _client.PostAsync(apiUrl, _data);
            var responseStream = await response.Content.ReadAsStringAsync();
            Cache(apiUrl, "PostAsJObject", data, responseStream.ToString());

            return response;
        }

        public async Task<HttpResponseMessage> PostFiles(string apiUrl, MultipartFormDataContent data)
        {
            SetToken();
            var response = await _client.PostAsync(apiUrl, data);
            return response;
        }

        public async Task<ApiResult> DeleteAsync(string apiUrl, object data)
        {
            SetToken();

            using var Content = Serialize(data);
            using var httpReq = new HttpRequestMessage(HttpMethod.Delete, apiUrl) { Content = Content };
            var response = await _client.SendAsync(httpReq);
            var responseStream = await response.Content.ReadAsStringAsync();
            Cache(apiUrl, "Delete", data, responseStream.ToString());

            return await ReturnResult(response);
        }

        public async Task<ApiResult<byte[]>> GetFileAsync(string apiUrl)
        {
            SetToken();
            var response = await _client.GetAsync(apiUrl);

            if (!response.IsSuccessStatusCode)
                return new ApiResult<byte[]>(false, response.ReasonPhrase ?? string.Empty, null);

            var result = await response.Content.ReadAsByteArrayAsync();

            return new ApiResult<byte[]>(true, response.ReasonPhrase ?? string.Empty, result);
        }
    }
}