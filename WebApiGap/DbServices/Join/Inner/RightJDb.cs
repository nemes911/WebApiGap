using API_GAI.Controllers;
using API_GAI.DbServices.SRC.Models;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using WebApiGap.DbServices.SRC.Models;

namespace WebApiGap.DbServices.Join.Inner
{
    public partial class JDb
    {
        /// <summary>
        /// Правое внешнее соеденение 
        /// </summary>
        /// <returns></returns>
        public List<Officer> GetRightJoinOfficersWithIncidents()
        {
            var constring = _context.Database.GetConnectionString();
            using var conn = new NpgsqlConnection(constring);
            conn.Open();
            var cmd = new NpgsqlCommand(@"
                SELECT o.id, o.first_name, o.last_name, o.middle_name, o.rank_id,
                       o.birth_date, o.passport_number, o.passport_series
                FROM gai.incidents i
                RIGHT JOIN gai.incident_officers io ON i.id = io.incident_id
                RIGHT JOIN gai.officers o ON io.officer_id = o.id", conn);

            var list = new List<Officer>();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new Officer
                {
                    Id = reader.GetGuid(0),
                    FirstName = reader.GetString(1),
                    LastName = reader.GetString(2),
                    MiddleName = reader.GetString(3),
                    RankId = reader.GetInt32(4),
                    BirthDate = reader.GetFieldValue<DateOnly>(5),
                    PassportNumber = reader.GetInt32(6),
                    PassportSeries = reader.GetInt32(7)
                });
            }
            return list;
        }

    }
    }
}
