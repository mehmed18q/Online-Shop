using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;


namespace Repository
{
    public class DapperService<TParam, TRes> : IDapperService<TParam, TRes> where TParam : class where TRes : class
    {
        private readonly string _connectionString;
        private readonly IConfiguration _configuration;
        public DapperService(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<bool> Procedure(string procedure, TParam param)
        {
            int x = -1;
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var result = await connection.ExecuteAsync(procedure, param, commandType: System.Data.CommandType.StoredProcedure);
            return x >= 1;
        }

        public async Task<IEnumerable<TRestM>> Query<TRestM>(string query, TParam param)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var result = await connection.QueryAsync<TRestM>(query, param, commandType: System.Data.CommandType.StoredProcedure);
            return result;
        }

        public async Task<TRestM?> FirstResultQuery<TRestM>(string command, TParam param)
            => (await Query<TRestM>(command, param)).FirstOrDefault();

        public async Task<(IEnumerable<T1>, IEnumerable<T2>)> MultiQuery<T1, T2>(string query, TParam param)
        {
            var connection = new SqlConnection(_connectionString);

            await connection.OpenAsync();
            var result = await connection.QueryMultipleAsync(query, param, commandType: System.Data.CommandType.StoredProcedure);
            var vt1 = result.Read<T1>();
            var vt2 = result.Read<T2>();

            return (vt1, vt2);
        }

        public async Task<(IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>)> MultiQuery<T1, T2, T3>(string query, TParam param)
        {
            var connection = new SqlConnection(_connectionString);

            await connection.OpenAsync();
            var result = await connection.QueryMultipleAsync(query, param, commandType: System.Data.CommandType.StoredProcedure);
            var vt1 = result.Read<T1>();
            var vt2 = result.Read<T2>();
            var vt3 = result.Read<T3>();

            return (vt1, vt2, vt3);
        }

        public async Task<(IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>, IEnumerable<T4>)> MultiQuery<T1, T2, T3, T4>(string query, TParam param)
        {
            var connection = new SqlConnection(_connectionString);

            await connection.OpenAsync();
            var result = await connection.QueryMultipleAsync(query, param, commandType: System.Data.CommandType.StoredProcedure);
            var vt1 = result.Read<T1>();
            var vt2 = result.Read<T2>();
            var vt3 = result.Read<T3>();
            var vt4 = result.Read<T4>();

            return (vt1, vt2, vt3, vt4);
        }

        public async Task<(IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>, IEnumerable<T4>, IEnumerable<T5>, IEnumerable<T6>, IEnumerable<T7>)> MultiQuery<T1, T2, T3, T4, T5, T6, T7>(string query, TParam param)
        {
            var connection = new SqlConnection(_connectionString);

            await connection.OpenAsync();
            var result = await connection.QueryMultipleAsync(query, param, commandType: System.Data.CommandType.StoredProcedure);
            var vt1 = result.Read<T1>();
            var vt2 = result.Read<T2>();
            var vt3 = result.Read<T3>();
            var vt4 = result.Read<T4>();
            var vt5 = result.Read<T5>();
            var vt6 = result.Read<T6>();
            var vt7 = result.Read<T7>();

            return (vt1, vt2, vt3, vt4, vt5, vt6, vt7);
        }
    }
}