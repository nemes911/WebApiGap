using API_GAI.DbServices.SRC.Models;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Data;
using WebApiGap.DbServices.PostgresFactory;
using WebApiGap.DbServices.SRC.Models;

namespace WebApiGap.DbServices.Join.Inner
{
    public partial class JDb
    {
        private readonly PostgresContext _context;
        public JDb(PostgresContextFactory factory) => _context = factory.Create();

        /// <summary>
        /// Симметричное внутреннее соединение с условием (по дате) — использование VIEW
        /// </summary>
        /// <param name="incident"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Симметричное внутреннее соединение с условием (по внешнему ключу — полицейский участок)
        ///    Инцидент + классификация + полицейское отделени
        /// </summary>
        /// <param name="police"></param>
        /// <returns></returns>
        public List<Incident> GetFullInfoIncident(PoliceStation police)
        {
            var constring = _context.Database.GetConnectionString();
            using (var con = new NpgsqlConnection(constring)){
                con.Open();
                var cmd = new NpgsqlCommand(@"
                SELECT
                    i.id, i.incident_class_id, i.incident_date, i.description,
                    i.repair_cost, i.timestamp, i.location, i.police_station_id,
                    ic.id AS class_id, ic.classification_name,
                    ps.id AS station_id, ps.address
                FROM gai.incidents i
                INNER JOIN gai.incident_classification ic ON ic.id = i.incident_class_id
                INNER JOIN gai.police_station ps ON ps.id = i.police_station_id
                WHERE ps.id = @police_station_id", con);

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
                            IncidentClass = new IncidentClassification { Id = reader.GetInt32(8), ClassificationName = reader.GetString(9) },
                            PoliceStation = new PoliceStation { Id = reader.GetInt32(10), Address = reader.GetString(11) }
                        });
                    }
                }
                return list;
            }
        }

        /// <summary>
        /// Симметричное внутреннее соединение с условием (по внешнему ключу — звание)
        ///    oficer by ranks
        /// </summary>
        /// <param name="rank"></param>
        /// <returns></returns>
        public List<Officer> GetOfficerByRanks(Rank rank)
        {
            var constring = _context.Database.GetConnectionString();
            using (var con = new NpgsqlConnection(constring))
            {
                con.Open();
                var cmd = new NpgsqlCommand(@"select
                    o.id, o.first_name, o.last_name, o.middle_name, o.rank_id,
                    o.birth_date, o.passport_number, o.passport_series
                    from gai.officers o
                    inner join gai.ranks r on r.id = o.rank_id
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

        /// <summary>
        /// Симметричное внутреннее соединение без условия — всех с просроченными правами
        /// </summary>
        /// <returns></returns>
        public List<JoinPrava> GetOnPrava()
        {
            var constring = _context.Database.GetConnectionString();
            var list = new List<JoinPrava>();
            using (var conn = new NpgsqlConnection(constring))
            {
                conn.Open();
                var cmd = new NpgsqlCommand(@"
                    select
                        p.date, p.series, p.number, p.kod_podrazdeleniya, p.type, p.status,
                        per.first_name, per.last_name, per.middle_name,
                        per.passport_number, per.passport_series, per.social_status_id,
                        v.serial_number, v.color, v.car_brand, v.insurance_company, v.vin
                    from gai.prava p
                    inner join gai.people per on per.id_prav = p.id
                    inner join gai.vehicles v on v.owner_id = per.id
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

        /// <summary>
        /// Симметричное внутреннее соединение с условием (два запроса с условием отбора по внешнему ключу) — по офицеру
        /// </summary>
        /// <param name="officer"></param>
        /// <returns></returns>
        public List<IncidentOfficerDto> GetIncidentsByOfficer(Officer officer)
        {
            var constring = _context.Database.GetConnectionString();
            using (var conn = new NpgsqlConnection(constring))
            {
                conn.Open();
                var cmd = new NpgsqlCommand(@"
                    select
                        i.id, i.incident_date,
                        o.first_name || ' ' || o.last_name as officer_name,
                        r.rank_name
                    from gai.incidents i
                    inner join gai.incident_officers io on i.id = io.incident_id
                    inner join gai.officers o on io.officer_id = o.id
                    inner join gai.ranks r on o.rank_id = r.id
                    where o.id = @officer_id", conn);

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
                            OfficerName = reader.GetString(2),
                            RankName = reader.GetString(3)
                        });
                    }
                }
                return list;
            }
        }

        /// <summary>
        /// Симметричное внутреннее соединение с условием (два запроса с условием отбора по внешнему ключу) — по автомобилю
        /// </summary>
        /// <param name="vehicle"></param>
        /// <returns></returns>
        public List<JoinPrava> GetIncidentByVehicle(Vehicle vehicle)
        {
            var constring = _context.Database.GetConnectionString();
            using (var conn = new NpgsqlConnection(constring))
            {
                conn.Open();
                var cmd = new NpgsqlCommand(@"
                    select
                        p.date, p.series, p.number, p.kod_podrazdeleniya, p.type, p.status,
                        per.first_name, per.last_name, per.middle_name,
                        per.passport_number, per.passport_series,
                        v.serial_number, v.color, v.car_brand, v.insurance_company, v.vin
                    from gai.incidents i
                    inner join gai.incident_vehicles iv on i.id = iv.incident_id
                    inner join gai.vehicles v on iv.vehicle_id = v.id
                    inner join gai.people per on v.owner_id = per.id
                    inner join gai.prava p on per.id_prav = p.id
                    where v.id = @vehicle_id", conn);

                cmd.Parameters.AddWithValue("vehicle_id", vehicle.Id);
                var list = new List<JoinPrava>();
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
                                status = reader.GetBoolean(5)
                            },
                            Person = new Person
                            {
                                FirstName = reader.GetString(6),
                                LastName = reader.GetString(7),
                                MiddleName = reader.GetString(8),
                                PassportNumber = reader.GetInt32(9),
                                PassportSeries = reader.GetInt32(10)
                            },
                            Vehicle = new Vehicle
                            {
                                SerialNumber = reader.GetInt32(11),
                                Color = reader.GetString(12),
                                CarBrand = reader.GetString(13),
                                Insurance_company = reader.GetString(14),
                                Vin = reader.GetString(15)
                            }
                        });
                    }
                }
                return list;
            }
        }

        /// <summary>
        /// Симметричное внутреннее соединение с условием (два запроса с условием отбора по датам) — первый
        /// </summary>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <returns></returns>
        public List<ViewIncidents> GetIncidentsByDateRange(DateOnly dateFrom, DateOnly dateTo)
        {
            var constring = _context.Database.GetConnectionString();
            using (var conn = new NpgsqlConnection(constring))
            {
                conn.Open();
                var cmd = new NpgsqlCommand("select * from gai.incident_full_view where incident_date BETWEEN @dateFrom AND @dateTo", conn);
                cmd.Parameters.AddWithValue("dateFrom", dateFrom);
                cmd.Parameters.AddWithValue("dateTo", dateTo);
                var list = new List<ViewIncidents>();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new ViewIncidents
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
                        });
                    }
                }
                return list;
            }
        }

        /// <summary>
        /// Симметричное внутреннее соединение без условия (три запроса) — все офицеры + станция + ранг
        /// </summary>
        /// <returns></returns>
        public List<Officer> GetAllOfficerWithStationsAndRanks()
        {
            var constring = _context.Database.GetConnectionString();
            using var conn = new NpgsqlConnection(constring);
            conn.Open();
            var cmd = new NpgsqlCommand(@"
                select o.id, o.first_name, o.last_name, o.middle_name, o.rank_id, o.birth_date,
                       o.passport_number, o.passport_series, o.police_station_id,
                       ps.address, r.rank_name
                from gai.officers o
                inner join gai.police_station ps on o.police_station_id = ps.id
                inner join gai.ranks r on o.rank_id = r.id", conn);

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
                    PassportSeries = reader.GetInt32(7),
                    
                });
            }
            return list;
        }

        /// <summary>
        ///  Симметричное внутреннее соединение без условия (три запроса) — все инциденты + классификация + ТС
        /// </summary>
        /// <returns></returns>
        public List<Incident> GetAllIncidentsWithClassificationAndVehicle()
        {
            var constring = _context.Database.GetConnectionString();
            using var conn = new NpgsqlConnection(constring);
            conn.Open();
            var cmd = new NpgsqlCommand(@"
                select i.id, i.incident_class_id, i.incident_date, i.description, i.repair_cost,
                       ic.classification_name, v.serial_number, v.car_brand
                from gai.incidents i
                inner join gai.incident_classification ic on i.incident_class_id = ic.id
                inner join gai.incident_vehicles iv on i.id = iv.incident_id
                inner join gai.vehicles v on iv.vehicle_id = v.id", conn);

            var list = new List<Incident>();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new Incident
                {
                    Id = reader.GetGuid(0),
                    IncidentClassId = reader.GetInt32(1),
                    IncidentDate = reader.GetFieldValue<DateOnly>(2),
                    Description = reader.GetString(3),
                    RepairCost = reader.IsDBNull(2) ? 0 : reader.GetDecimal(4)
                    // можно добавить вложенные объекты
                });
            }
            return list;
        }

     
        /// <summary>
        /// Итоговый запрос без условия
        /// </summary>
        public List<DistrictCount> GetAggregateNoCondition()
        {
            var constring = _context.Database.GetConnectionString();

            using (var conn = new NpgsqlConnection( constring))
            {
                conn.Open();

                var cmd = new NpgsqlCommand(@"
                select
                        ps.district_id,
                        count(i.id) as incident_count,
                        sum(i.repair_cost) as total_damage
                from gai.incidents i
                inner join gai.police_station ps on i.police_station_id = ps.id
                group by ps.district_id", conn);

                var list = new List<DistrictCount>();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new DistrictCount
                        {
                         DistrictId = reader.GetInt32(0),
                         IncidentCount = reader.GetInt32(1),
                         TotalDamage = reader.IsDBNull(2) ? 0 : reader.GetDecimal(2)
                        });
                    }
                }
                return list;
            }
        }

        /// <summary>
        /// Итоговый запрос с условием на данные
        /// </summary>
        /// <param name="dateFrom"></param>
        /// <returns></returns>
        public List<DistrictCount> GetAggregateWithDataCondition(DateOnly dateFrom)
        {
            var constring = _context.Database.GetConnectionString();

            using (var conn = new NpgsqlConnection(constring))
            {
                conn.Open();

                var cmd = new NpgsqlCommand(@"
                select
                    ps.district_id,
                    count(i.id),
                    sum(i.repair_cost)
                from gai.incidents i
                inner join gai.police_station ps on i.police_station_id = ps.id
                where i.incident_date >= @dateFrom
                group by ps.district_id
                ",conn);

                cmd.Parameters.AddWithValue("dateFrom", dateFrom);

                var list = new List<DistrictCount>();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new DistrictCount
                        {
                            DistrictId = reader.GetInt32(0),
                            IncidentCount = reader.GetInt32(1),
                            TotalDamage = reader.IsDBNull(2) ? 0 : reader.GetDecimal(2)
                        });
                    }
                }
                return list;
            }
        }

        /// <summary>
        /// Итоговый запрос с условием на группы
        /// </summary>
        /// <param name="minIncidents"></param>
        /// <returns></returns>
        public List<DistrictCount> GetAggregateWithGroupCondition(int minIncidents)
        {
            var constring = _context.Database.GetConnectionString();

            using (var conn = new NpgsqlConnection(constring))
            {
                conn.Open();

                var cmd = new NpgsqlCommand(@"
                select 
                        ps.district_id,
                        count(i.id),
                        sum(i.repair_cost)
                from gai.incidents i
                inner join gai.police_station ps on i.police_station_id = ps.id
                group by ps.district_id
                having count(i.id) >= @minIncidents", conn);

                cmd.Parameters.AddWithValue("minIncidents", minIncidents);

                var list = new List<DistrictCount>();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new DistrictCount
                        {
                            DistrictId = reader.GetInt32(0),
                            IncidentCount = reader.GetInt32(1),
                            TotalDamage = reader.IsDBNull(2) ? 0 : reader.GetDecimal(2)
                        });
                    }
                }
                return list;
            }
        }

        /// <summary>
        /// Итоговый запрос с условием на данные и на группы
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="minDamage"></param>
        /// <returns></returns>
        public List<DistrictCount> GetAggregateWithBothConditions(DateOnly from, DateOnly to, decimal minDamage)
        {
            var constring = _context.Database.GetConnectionString();
            using (var conn = new NpgsqlConnection(constring))
            {
                conn.Open();

                var cmd = new NpgsqlCommand(@"
                select
                        ps.district_id,
                        count(i.id),
                        sum(i.repair_cost)
                from gai.incidents i
                inner join gai.police_station ps on i.police_station_id = ps.id
                where i.incident_date between @from and @to
                group by ps.district_id 
                having sum(i.repair_cost) >= @minDamage", conn);

                cmd.Parameters.AddWithValue("from", from);
                cmd.Parameters.AddWithValue("to", to);
                cmd.Parameters.AddWithValue("minDamage", minDamage);

                var list = new List<DistrictCount>();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new DistrictCount
                        {
                            DistrictId = reader.GetInt32(0),
                            IncidentCount = reader.GetInt32(1),
                            TotalDamage = reader.IsDBNull(2) ? 0 : reader.GetDecimal(2)
                        });
                    }
                }
                return list;
            }
        }

        /// <summary>
        /// Запрос на запросе по принципу итогового запроса
        /// </summary>
        /// <returns></returns>
        public List<DistrictCount> GetSubqueryAggregate()
        {
            var constring = _context.Database.GetConnectionString();

            using (var conn = new NpgsqlConnection(constring))
            {
                conn.Open();

                var cmd = new NpgsqlCommand(@"
                select *
                from (
                        select 
                            ps.district_id,
                            count(i.id) as incidents_count,
                            sum(i.repair_cost) as total_damage
                        from gai.incidents i
                        inner join gai.police_station ps on i.police_station_id = ps.id
                        group by ps.district_id
                ) t
                order by total_damage desc", conn);

                var list = new List<DistrictCount>();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new DistrictCount
                        {
                            DistrictId = reader.GetInt32(0),
                            IncidentCount = reader.GetInt32(1),
                            TotalDamage = reader.IsDBNull(2) ? 0 : reader.GetDecimal(2)
                        });
                    }
                }
                return list;
            }
        }

        /// <summary>
        /// Запрос с подзапросом
        /// </summary>
        /// <returns></returns>
        public List<Incident> GetAggregateWithSubquery()
        {
                var constring = _context.Database.GetConnectionString();

            using var conn = new NpgsqlConnection(constring);
            conn.Open();

            var cmd = new NpgsqlCommand(@"
        select
            id,
            incident_class_id,
            incident_date,
            description,
            repair_cost
        from gai.incidents
        where repair_cost >
        (
            select avg(repair_cost)
            from gai.incidents
        )
        ", conn);

            var list = new List<Incident>();

            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                list.Add(new Incident
                {
                    Id = reader.GetGuid(0),
                    IncidentClassId = reader.GetInt32(1),
                    IncidentDate = reader.GetFieldValue<DateOnly>(2),
                    Description = reader.GetString(3),
                    RepairCost = reader.IsDBNull(2) ? 0 : reader.GetDecimal(4)
                });
            }

            return list;
        }
    }
}