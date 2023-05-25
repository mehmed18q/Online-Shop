using Framework;

namespace Repository
{
    public interface IHttpRequest
    {
        Task<ApiResult> GetAsync(string apiUrl);
        Task<bool> CheckPictureAsync(string apiUrl);
        Task<ApiResult> DeleteAsync(string apiUrl);
        Task<ApiResult<byte[]>> GetFileAsync(string apiUrl);
        Task<ApiResult<object>> GetDataAsync(string apiUrl);
        Task<ApiResult> PutAsync(string apiUrl, object data);
        Task<ApiResult> PostAsync(string apiUrl, object data);
        Task<ApiResult> DeleteByIdAsync(string apiUrl, long id);
        Task<ApiResult> DeleteAsync(string apiUrl, object data);
        Task<ApiResult<TResult>> GetAsync<TResult>(string apiUrl);
        Task<ApiResult<TResult>> DeleteAsync<TResult>(string apiUrl);
        Task<ApiResult> ReturnResult(HttpResponseMessage httpresponse);
        Task<ApiResult<byte[]>> GetApiResultAsync<TResult>(string apiUrl);
        Task<ApiResult<TResult>> PutAsync<TResult>(string apiUrl, object data);
        Task<ApiResult<TResult>> PostAsync<TResult>(string apiUrl, object data, string? token = null);
        Task<List<TResult>> ListPostAsync<TResult>(string apiUrl, object data, string? token = null);
        Task<HttpResponseMessage> PostAsJObject(string apiUrl, object data);
        Task<ApiResult<object>> PostFileAsync(string apiUrl, HttpContent data);
        Task<HttpResponseMessage> PostFiles(string apiUrl, MultipartFormDataContent data);
        Task<ApiResult<TResult>> PostWithCustomTokenAsync<TResult>(string apiUrl, object data, string token);
        Task<ApiResult<TResult>> GetWithCustomTokenAsync<TResult>(string apiUrl, string token);
    }
}