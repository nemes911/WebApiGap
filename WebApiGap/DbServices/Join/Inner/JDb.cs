
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

        //oficer by ranks
        public List<Officer> GetOfficerByRanks(Rank rank)
        {
            var constring = _context.Database.GetConnectionString();
            using (var con = new NpgsqlConnection(constring))
            {
                con.Open();
                var cmd = new NpgsqlCommand(@"select
                o.id,
                o.first_name,
                o.last_name,
                o.middle_name,
                o.rank_id,
                o.birth_date,
                o.passport_number,
                o.passport_series
                from gai.officers o
                inner join gai.ranks r
                    on r.id = o.rank_id
                where r.id = @rank_id", con);

                cmd.Parameters.AddWithValue("rank_id", rank.Id);

                var list = new List<Officer>();
                using (var reader = cmd.ExecuteReader())
                {
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
                            PassportSeries = reader.GetInt32(7),
                        });
                    }
                }
                return list;
            }
        }

        //всех с просроченными правами 
        public  List<JoinPrava> GetOnPrava()
        {
            var constring = _context.Database.GetConnectionString();
            var list = new List<JoinPrava>();
            using (var conn = new NpgsqlConnection(constring))
            {
                conn.Open();

                var cmd = new NpgsqlCommand(@"
                select
                p.date,
                p.series,
                p.number,
                p.kod_podrazdeleniya,
                p.type,
                p.status,
                per.first_name,
                per.last_name,
                per.middle_name,
                per.passport_number,
                per.passport_series,
                per.social_status_id,
                v.serial_number,
                v.color,
                v.car_brand,
                v.insurance_company,
                v.vin
                from gai.prava p
                inner join gai.people per 
                    on per.id_prav = p.id   
                inner join gai.vehicles v 
                    on v.owner_id = per.id
                ", conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new JoinPrava
                        {
                            prava = new Prava
                            {
                                date = reader.GetDateTime(0),
                                series = reader.GetString(1),
                                number = reader.GetInt32(2),
                                kod_podrazdeleniya = reader.GetString(3),
                                type = reader.GetFieldValue<string[]>(4),
                                status = reader.GetBoolean(5),
                            },
                            Person = new Person
                            {
                                FirstName = reader.GetString(6),
                                LastName = reader.GetString(7),
                                MiddleName = reader.GetString(8),
                                PassportNumber = reader.GetInt32(9),
                                PassportSeries = reader.GetInt32(10),
                                SocialStatusId = reader.GetInt32(11),
                            },
                            Vehicle = new Vehicle
                            {
                                SerialNumber = reader.GetInt32(12),
                                Color = reader.GetString(13),
                                CarBrand = reader.GetString(14),
                                Insurance_company = reader.GetString(15),
                                Vin = reader.GetString(16)
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