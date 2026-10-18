using API_GAI.Settings;
using Microsoft.Extensions.Options;
using Npgsql;
using WebApiGap.DbServices.DefaultCommand.Implements;
using WebApiGap.DbServices.DefaultCommand.Interface;
using WebApiGap.DbServices.SRC.DTOModels;
using WebApiGap.Session.Service;


namespace WebApiGap.DbServices.SRC.Queries
{
    public class QueryList
    {
        private readonly ICurrentSession _session;


        private readonly string _connection_string;


        public QueryList(ICurrentSession session, IOptions<AppiSettings> options)
        {
            _session = session;
            var setting = options.Value;

            _connection_string = setting.PostgresBase;
        }

        public async Task<string> CreateUser(string p_username)
        {
            try
            {
                var connectionString = _connection_string +
                    "Username=" + _session.Session.name +
                    ";Password=" + _session.Session.password + ";";



                await using (var conn = new NpgsqlConnection(connectionString))
                {
                    await conn.OpenAsync();

                    const string sql = "SELECT * FROM gai.create_or_update_user_with_district(@p_username)";

                    await using var cmd = new NpgsqlCommand(sql, conn);

                    cmd.Parameters.AddWithValue("p_username", p_username);

                    var result = await cmd.ExecuteScalarAsync();

                    return result?.ToString() ?? string.Empty;
                }



            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }


        //ДОСТУПА К ТАБЛИЦЕ ИНЦЕНДЕНТЫ НЕТ У РОЛИ 
        public async Task<List<IncidentsByIdDistrict>> JoinDistrictIncidents(int id)
        {
            var list = new List<IncidentsByIdDistrict>();

            var connectionString = _connection_string +
                "Username=" + _session.Session.name +
                ";Password=" + _session.Session.password + ";";

            await using var conn = new NpgsqlConnection(connectionString);
            await conn.OpenAsync();

            const string sql = "SELECT * FROM gai.get_incidents_by_district(@p_district_id)";

            await using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("p_district_id", id);

            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                list.Add(new IncidentsByIdDistrict
                {
                    Id = reader.GetGuid(0),
                    Incident_class_Id = reader.GetInt32(1),
                    Incident_Date = reader.GetDateTime(2),
                    Description = reader.GetString(3),
                    Repair_cost = reader.GetDecimal(4),
                    TimeStamp = reader.GetDateTime(5),
                    Location = reader.GetString(6),
                    PoliceStation_Id = reader.GetInt32(7)
                });
            }

            return list;
        }
    


    //сравнение дтп за месяцы 
    public async Task<Dictionary<DateTime, Dictionary<int, double>>> EquelseDTPPerMothAsync(DateTime start, DateTime end)
        {
            var result = new Dictionary<DateTime, Dictionary<int, double>>();


            var list = new List<IncidentsByIdDistrict>();

            var connectionString = _connection_string +
                "Username=" + _session.Session.name +
                ";Password=" + _session.Session.password + ";";

            await using var conn = new NpgsqlConnection(connectionString);

            await conn.OpenAsync();

            const string sql = "select * from incidents_equels(@date_start, @date_end)";

            await using var cmd = new NpgsqlCommand(sql, conn);

            var reader = await cmd.ExecuteReaderAsync();

            while (reader.Read())
            {
                var date = reader.GetDateTime(0);
                var countIncidents = reader.GetInt32(2);
                var procent = reader.GetDouble(3);

                if(!result.TryGetValue(date, out var dictionarhy))
                {
                    dictionarhy = new Dictionary<int, double>();
                    result[date] = dictionarhy;
                 }

                dictionarhy[countIncidents] = procent;
            }

            return result;
        }
    }

}
