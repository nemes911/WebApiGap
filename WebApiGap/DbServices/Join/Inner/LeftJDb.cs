using API_GAI.DbServices.SRC.Models;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using WebApiGap.DbServices.SRC.Models;

namespace WebApiGap.DbServices.Join.Inner
{
    public partial class JDb
    {
        /// <summary>
        /// Левое внешнее соеденение 
        /// </summary>
        /// <returns></returns>
        public List<IncidentOfficerDto> GetLeftJoinIncidentsWithOfficers()
        {
            
            var constring = _context.Database.GetConnectionString();
            using var conn = new NpgsqlConnection(constring);
            conn.Open();
            var cmd = new NpgsqlCommand(@"
                SELECT i.id, i.incident_date,
                       o.first_name || ' ' || o.last_name AS officer_name,
                       r.rank_name
                FROM gai.incidents i
                LEFT JOIN gai.incident_officers io ON i.id = io.incident_id
                LEFT JOIN gai.officers o ON io.officer_id = o.id
                LEFT JOIN gai.ranks r ON o.rank_id = r.id", conn);

            var list = new List<IncidentOfficerDto>();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new IncidentOfficerDto
                {
                    IncidentId = reader.GetGuid(0),
                    IncidentDate = reader.GetFieldValue<DateOnly>(1),
                    OfficerName = reader.IsDBNull(2) ? null : reader.GetString(2),
                    RankName = reader.IsDBNull(3) ? null : reader.GetString(3)
                });
            }
            return list;
        }

        //инценденты по районам 
        public List<DistricStat> GetIncidentGroupDistrict()
        {
            var constring = _context.Database.GetConnectionString();

            using (var con = new NpgsqlConnection(constring))
            {
                con.Open();

                var cmd = new NpgsqlCommand(@"select 
                d.name as district,
                count(i.id) as total_incidents
                from gai.district d
                left join gai.police_station ps
                    on ps.district_id = d.id
                left join gai.incidents i
                    on i.police_station_id = ps.id
                group by d.name
                order by total_incidents desc;", con);

                var list = new List<DistricStat>();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new DistricStat
                        {
                            District = reader.GetString(0),
                            TotalCountIncidents = reader.GetInt32(1)
                        });
                    }
                }

                return list;
            }
        }

        /// <summary>
        /// Запрос на запросе по принципу левого соединения
        /// </summary>
        /// <param name="officer"></param>
        /// <returns></returns>
        public List<IncidentOfficerDto> GetSubqueryLeftJoinStyle(Officer officer)
        {
            var constring = _context.Database.GetConnectionString();
            using var conn = new NpgsqlConnection(constring);
            conn.Open();
            var cmd = new NpgsqlCommand(@"
        SELECT 
            sub.id AS incident_id,
            sub.incident_date,
            o.first_name || ' ' || o.last_name AS officer_name,
            r.rank_name
        FROM (
            SELECT i.id, i.incident_date, io.officer_id
            FROM gai.incidents i
            LEFT JOIN gai.incident_officers io ON i.id = io.incident_id
        ) AS sub
        LEFT JOIN gai.officers o ON sub.officer_id = o.id
        LEFT JOIN gai.ranks r ON o.rank_id = r.id
        WHERE sub.officer_id = @officer_id", conn);

            cmd.Parameters.AddWithValue("officer_id", officer.Id);

            var list = new List<IncidentOfficerDto>();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new IncidentOfficerDto
                {
                    IncidentId = reader.GetGuid(0),
                    IncidentDate = reader.GetFieldValue<DateOnly>(1),
                    OfficerName = reader.IsDBNull(2) ? null : reader.GetString(2),
                    RankName = reader.IsDBNull(3) ? null : reader.GetString(3)
                });
            }
            return list;
        }
    }
}
