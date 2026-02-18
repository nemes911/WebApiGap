using API_GAI.DbServices.SRC.Models;
using API_GAI.Settings;
using Microsoft.Extensions.Options;
using Npgsql;
using Npgsql.EntityFrameworkCore.PostgreSQL.Query.ExpressionTranslators.Internal;
using WebApiGap.DbServices.DefaultCommand.Interface;

namespace API_GAI.DbServices.SRC.Data.Auth
{
    public class Authzorization
    {
        private readonly string _connection_string;

        public Authzorization(IOptions<AppiSettings> options)
        {
            var setting = options.Value;

            _connection_string = setting.PostgresBase;
        }
        //установка роли 
        //public async Task<string?> Setrole(Users user)
        //{
          //  
        //}
       



        //вход 
        public async Task<string?> AuthAsync(string name, string password)
        {
            var _connection = _connection_string + "Username=" + name + ";Password=" + password + ";";

            Console.WriteLine(_connection);

            await using (var connection = new NpgsqlConnection(_connection))
            {
                     await connection.OpenAsync();


                const string sql = "SELECT authenticated, role_name FROM gai.check_user(@p_username, @p_password)";

                await using var cmd = new NpgsqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("p_username", name);
                cmd.Parameters.AddWithValue("p_password", password);

                await using var reader = await cmd.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    bool authenticated = reader.GetBoolean(0);
                    string? roleName = reader.IsDBNull(1) ? null : reader.GetString(1);

                    if (authenticated && roleName != null)
                    {
                        return roleName;
                    }
                }
            }
            return null;  
        }
    }
}
