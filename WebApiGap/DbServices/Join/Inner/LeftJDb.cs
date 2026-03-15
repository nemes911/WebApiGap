using API_GAI.DbServices.SRC.Models;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using WebApiGap.DbServices.SRC.Models;

namespace WebApiGap.DbServices.Join.Inner
{
    public partial class JDb
    {
        //левое соеденение + запрос на запросеф
        public List<Incident> GetFullIncidentByOfficer(Officer oficer)
        {
            var constring = _context.Database.GetConnectionString();

            using (var conn = new NpgsqlConnection(constring))
            {
                conn.Open();

                var cmd = new NpgsqlCommand(@"
                select 
                    i.id as incident_id,
                    i.incident_date,
                    o.first_name,
                    o.last_name,
                    r.rank_name
                    from gai.incident_officers io 
                    inner join gai.incidents i 
                        on i.id = io.incidents_id
                    inner join gai.officers o
                        on o.id = io.officer_id
                    left join gai.ranks r
                        on r.id = o.rank_id
                    where o.id = @officer_id", conn);

                cmd.Parameters.AddWithValue("officer_id", oficer.Id);

                var list = new List<Incident>();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Incident
                        {

                            Id = reader.GetGuid(0),
                            IncidentDate = reader.GetFieldValue<DateOnly>(1)
                        });
                    }
                }
                return list;
            }
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
        /// Левое внешнее соединение
        /// </summary>
        public List<IncidentOfficerDto> GetLeftJoinIncidentsWithOfficers() 
        {
            var constring = _context.Database.GetConnectionString();

            using (var conn = new NpgsqlConnection(constring))
            {
                conn.Open();

                var cmd = new NpgsqlCommand(@"
                select
                        i.id,
                        i.incident_date,
                        o.first_name || '' || o.last_name as officer_name,
                        r.rank_name
                from gai.incidents i
                left join gai.incident_officers io on i.id = io.incident_id
                left join gai.officers o on io.officer_id = o.id
                left join gai.ranks r on o.rank_id = r.id
                ", conn);

                var list = new List<IncidentOfficerDto>();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new IncidentOfficerDto
                        {
                            IncidentId = reader.GetGuid(0),
                            IncidentDate = reader.GetFieldValue<DateOnly>(1),
                            OfficerName = reader.IsDBNull(2) ? null : reader.GetString(2),
                            RankName = reader.IsDBNull(3) ? null : reader.GetString(3),
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
            using (var conn = new NpgsqlConnection(constring))
            {
                var cmd = new NpgsqlCommand(@"
                select
                        i.id,
                        i.incident_date,
                        (
                        select o.first_name || '' || o.last_name
                        from gai.officers o
                        where o.id = io.officer_id
                        ) as officer_name
                        from gai.incidents i 
                        left join gai.incident_officers io on i.id = io.incident_id
                        where io.officer_id = @officer_id", conn);

                cmd.Parameters.AddWithValue("officer_id", officer.Id);

                var list = new List<IncidentOfficerDto>();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new IncidentOfficerDto
                        {
                            IncidentId = reader.GetGuid(0),
                            IncidentDate = reader.GetFieldValue<DateOnly>(1),
                            OfficerName = reader.GetString(2)
                        });
                    }
                }

                return list;
            }
        }
    }
}
