
using Npgsql;
using API_GAI.DbServices.SRC.Models;
using WebApiGap.DbServices.PostgresFactory;
using Microsoft.EntityFrameworkCore;
using WebApiGap.DbServices.SRC.Models;
using System.Data;

namespace WebApiGap.DbServices.Join.Inner
{
    public partial class JDb
    {
        private readonly PostgresContext _context;

        public JDb(PostgresContextFactory factory) => _context = factory.Create();



        //view к инцендентам
        public List<ViewIncidents> GetIncidents(ViewIncidents incident)
        {
            var constring = _context.Database.GetConnectionString(); 
            using (var con = new NpgsqlConnection(constring))
            {
                con.Open();
                var cmd = new NpgsqlCommand("select * from gai.incident_full_view WHERE incident_date = @incident_date", con);

                cmd.Parameters.AddWithValue("incident_date", incident.incident_date);

                var listreturn = new List<ViewIncidents>();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var viewincident = new ViewIncidents
                        {
                            incident_id = reader.GetGuid(0),
                            incident_class_id = reader.GetInt32(1),
                            incident_date = reader.GetFieldValue<DateOnly>(2),
                            description = reader.GetString(3),
                            repair_cost = reader.GetDecimal(4),
                            vehicle_id = reader.GetGuid(5),
                            serial_number = reader.GetInt32(6),
                            color = reader.GetString(7),
                            owner_id = reader.GetGuid(8),
                            car_brand = reader.GetString(9),
                            insurance_company = reader.GetString(10),
                            vin = reader.GetString(11)
                        };
                        listreturn.Add(viewincident);
                    }
                }
                return listreturn;
            }
        }

        //инцендент + класификация и полицейское отделение 
        public List<Incident> GetFullInfoIncident(PoliceStation police)
        {
            var constring = _context.Database.GetConnectionString();

            using (var con = new NpgsqlConnection(constring))
            {
                con.Open();

                var cmd = new NpgsqlCommand(@"
                select
                i.id,
                i.incident_id,
                i.description,
                i.repair_cost,
                i.timestamp,
                i.location
                i.police_station_id,

                ic.id as class_id,
                ic.classification,

                ps.id as station_id,
                ps.address
                from gai.incidents i
                inner join gai.incident_classification ic
                    on ic.id = i.incident_class_id
                inner join gai.police_station_id
                    on ps.id = i.police_station_id
                where ps.id = @police_station_id", con);

                cmd.Parameters.AddWithValue("police_station_id", police.Id);

                var list = new List<Incident>();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Incident
                        {
                            Id = reader.GetGuid(0),
                            IncidentClassId = reader.GetInt32(1),
                            IncidentDate = reader.GetFieldValue<DateOnly>(2),
                            Description = reader.GetString(3),
                            RepairCost = reader.GetDecimal(4),
                            Timestamp = reader.GetDateTime(5),
                            Location = reader.GetString(6),
                            PoliceStationId = reader.GetInt32(7),
                            IncidentClass =     new IncidentClassification
                            {
                                Id = reader.GetInt32(8),
                                ClassificationName = reader.GetString(9),
                            },
                            PoliceStation = new PoliceStation
                            {
                                Id = reader.GetInt32(10),
                                Address = reader.GetString(11)
                            }
                                                
                        });
                    }
                    
                }
                return list;
            }
        }

        //инценденты по районам 
        /*public Incident GetIncidentGroupDistrict(District district)
        {
            var constring = _context.Database.GetConnectionString();

            using (var con = new NpgsqlConnection(constring))
            {
                con.Open();

                var cmd = new NpgsqlCommand(@"select 
                d.name as district,
                count(i.id) as total_incidents
                from gai.district d
                ", con);
            }
        }*/
    }
}
//incident_da//"serial_number, color, car_brand, insurance_company, vin