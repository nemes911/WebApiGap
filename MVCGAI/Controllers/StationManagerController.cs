using API_GAI.DbServices.SRC.Models;
using Microsoft.AspNetCore.Mvc;
using MVCGAI.DTOModels;
using MVCGAI.Models;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MVCGAI.Controllers
{
    public class StationManagerController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public StationManagerController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpPost]
        public async Task<IActionResult> Stats(DateTime date_start, DateTime date_end)
        {
            var sessionId = HttpContext.Session.GetString("session_ID");
            ViewBag.Username = HttpContext.Session.GetString("UserName");
            ViewBag.Role = HttpContext.Session.GetString("role");

            var client = _httpClientFactory.CreateClient("ApiClient");
            client.DefaultRequestHeaders.Add("X-Session-Id", sessionId);

            var dataresultStatsByDate = ViewBag.DataStats as Dictionary<DateTime, Dictionary<int, double>>;


            try
            {
                var response = await client.PostAsJsonAsync("api/Incidents/GetStatByDate", new {date_start, date_end });
                if (response.IsSuccessStatusCode)
                {
                    dataresultStatsByDate = await response.Content.ReadFromJsonAsync<Dictionary<DateTime, Dictionary<int, double>>>();
                }
                ViewBag.DataStats = dataresultStatsByDate;
            }
            catch(Exception ex)
            {

            }
            return View();
        }



        // 1. Главная страница
        public async Task<IActionResult> Index()
        {
            if (TempData["Error"] != null)
                ViewBag.Error = TempData["Error"];

            var sessionId = HttpContext.Session.GetString("session_ID");
            ViewBag.Username = HttpContext.Session.GetString("UserName");
            ViewBag.Role = HttpContext.Session.GetString("role");

            var client = _httpClientFactory.CreateClient("ApiClient");
            client.DefaultRequestHeaders.Add("X-Session-Id", sessionId);

            // Загружаем инциденты
            try
            {
                var response = await client.GetAsync("api/Incidents/AllIncidents");
                if (response.IsSuccessStatusCode)
                {
                    ViewBag.Incidents = await response.Content.ReadFromJsonAsync<List<Incident>>();
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    ViewBag.Error = FormatApiError(errorContent, (int)response.StatusCode, response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = FormatException(ex, "Загрузка инцидентов");
            }

            // Загружаем участки и классификации
            try
            {
                var responseStations = await client.GetFromJsonAsync<List<PoliceStation>>("api/PoliceStation/get-all-policestation");
                var respclass = await client.GetFromJsonAsync<List<IncidentClassification>>("api/IncidentClassification/Get-Classification");

                ViewBag.PoliceStations = responseStations ?? new List<PoliceStation>();
                ViewBag.IncidentClassification = respclass ?? new List<IncidentClassification>();
            }
            catch (Exception ex)
            {
                ViewBag.PoliceStations = new List<PoliceStation>();
                ViewBag.IncidentClassification = new List<IncidentClassification>();
                ViewBag.Error = (ViewBag.Error ?? "") + "\n" + FormatException(ex, "Загрузка справочников");
            }

            return View();
        }

        // 2. Редактирование
        [HttpPost]
        public async Task<IActionResult> EditIncident(Incident model)
        {
            var sessionId = HttpContext.Session.GetString("session_ID");
            var client = _httpClientFactory.CreateClient("ApiClient");
            client.DefaultRequestHeaders.Add("X-Session-Id", sessionId);

            if(model.PoliceStation == null)
            {
                var psresp = await client.GetFromJsonAsync<List<PoliceStation>>("api/PoliceStation/get-all-policestation");
                foreach(var inc in psresp)
                {
                    if(inc.Id == model.PoliceStationId)
                    {
                        model.PoliceStation = inc;
                        break;
                    }
                }
            }

            try
            {
                var response = await client.PutAsJsonAsync("api/Incidents/UpdateIncident", model);

                if (response.IsSuccessStatusCode)
                    return RedirectToAction("Index");

                var errorContent = await response.Content.ReadAsStringAsync();
                TempData["Error"] = FormatApiError(errorContent, (int)response.StatusCode, response.ReasonPhrase);
            }
            catch (Exception ex)
            {
                TempData["Error"] = FormatException(ex, "Редактирование инцидента");
            }

            return RedirectToAction("Index");
        }

        // 3. Добавление
        [HttpPost]
        public async Task<IActionResult> CreateIncident(Incident incident)
        {
            var sessionId = HttpContext.Session.GetString("session_ID");
            incident.Id = null;

            var client = _httpClientFactory.CreateClient("ApiClient");
            client.DefaultRequestHeaders.Add("X-Session-Id", sessionId);

            try
            {
                // === ПРИВЯЗКА ВИРТУАЛЬНЫХ СВОЙСТВ ===

                // 1. Полицейский участок
                if (incident.PoliceStationId > 0)
                {
                    // Подгружаем все участки и находим нужный
                    var stations = await client.GetFromJsonAsync<List<PoliceStation>>("api/PoliceStation/get-all-policestation");
                    incident.PoliceStation = stations?.FirstOrDefault(s => s.Id == incident.PoliceStationId);

                    // Если вдруг не нашли — создаём минимальный объект (чтобы не было null)
                    if (incident.PoliceStation == null)
                    {
                        incident.PoliceStation = new PoliceStation { Id = incident.PoliceStationId };
                    }
                }

                // 2. Классификация
                if (incident.IncidentClassId.HasValue && incident.IncidentClassId > 0)
                {
                    var classifications = await client.GetFromJsonAsync<List<IncidentClassification>>("api/IncidentClassification/Get-Classification");
                    incident.IncidentClass = classifications?.FirstOrDefault(c => c.Id == incident.IncidentClassId.Value);

                    if (incident.IncidentClass == null)
                    {
                        incident.IncidentClass = new IncidentClassification { Id = incident.IncidentClassId.Value };
                    }
                }

                // ====================================

                // Для отладки
                var jsonToSend = JsonSerializer.Serialize(incident, new JsonSerializerOptions { WriteIndented = true });
                Console.WriteLine("===== ОТПРАВЛЯЕМ НА API =====");
                Console.WriteLine(jsonToSend);
                Console.WriteLine("=============================");

                var response = await client.PostAsJsonAsync("api/Incidents/SetIncident", incident);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    TempData["Error"] = FormatApiError(errorContent, (int)response.StatusCode, response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = FormatException(ex, "Добавление инцидента");
            }

            return RedirectToAction("Index");
        }


        [HttpGet]
        public async Task<IActionResult> PravaAndHumans()
        {
            var sessionId = HttpContext.Session.GetString("session_ID");

            var client = _httpClientFactory.CreateClient("ApiClient");
            client.DefaultRequestHeaders.Add("X-Session-Id", sessionId);



            try
            {
                var response = await client.GetFromJsonAsync<List<HumansVies>>("api/Person/ViewsHumanForSelect");

                ViewBag.PeopleAndHuman = response ?? new List<HumansVies>();

            }
            catch (Exception ex)
            {

            }

            return View();
        }

    

       //search 
        [HttpPost]
        public async Task<IActionResult> SearchPerson(int? passportnumber,int? passportseries, string? pravaseries, int? pravanumber, string? kodPodradeleniya) 
        {
           var people = ViewBag.PeopleAndHuman as List<HumansVies> ?? new List<HumansVies>();

            if (passportseries.HasValue)
            {
                people = people
                        .Where(x => x.PassportSeries == passportseries.Value)
                        .ToList();
            }
            if (passportseries.HasValue)
            {
                people = people
                        .Where(x => x.PassportNumber == passportnumber.Value)
                        .ToList();
            }
            if (!string.IsNullOrEmpty(pravaseries))
            {
                people = people
                        .Where(x => x.PravaSeries != null && 
                                x.PravaSeries.Contains(
                                        pravaseries,
                                        StringComparison.OrdinalIgnoreCase))
                        .ToList();
            }
            if (pravanumber.HasValue)
            {
                people = people
                        .Where(x => x.PravaNumber == pravanumber.Value)
                        .ToList();
            }
            if(!string.IsNullOrEmpty(kodPodradeleniya))
            {
                people = people
                        .Where(x => x.KodPodrazdeleniya != null && 
                                    x.KodPodrazdeleniya.Contains(
                                                kodPodradeleniya, StringComparison.OrdinalIgnoreCase))
                        .ToList();
            }

            return View("PravaAndHumans", people);

        }
       
        [HttpPost]
        public async Task<IActionResult> NewPerson(Person person)
        {
            var sessionId = HttpContext.Session.GetString("session_ID");

            var client = _httpClientFactory.CreateClient("ApiClient");
            client.DefaultRequestHeaders.Add("X-Session-Id", sessionId);



            try
            {
                var response = await client.PostAsJsonAsync<Person>("api/Person/Set-Peson", person);
            }catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
            return RedirectToAction("PravaAndHumans");
        }


        //CreapePrava
        [HttpPost]
        public async Task CreatePrava(Prava prava)
        {
            ViewBag.PravaIdCreated = Guid.NewGuid();

            var sessionId = HttpContext.Session.GetString("session_ID");

            var pra = prava;


            var client = _httpClientFactory.CreateClient("ApiClient");
            client.DefaultRequestHeaders.Add("X-Session-Id", sessionId);

            try
            {
                var response = await client.PostAsJsonAsync<Prava>("api/Prava/NewPrava", prava);

                var content = await response.Content.ReadAsStringAsync();

                ViewBag.PravaIdCreated = new Guid(content);

            }catch(Exception ex)
            {
                 Console.WriteLine(ex.Message);
            }
           
        }
        //Присвоить права уже существующему юзеру
        [HttpPost]
        public async Task<IActionResult> SetPravaPesorn(Person person)
        {
            var sessionId = HttpContext.Session.GetString("session_ID");

            var client = _httpClientFactory.CreateClient("ApiClient");
            client.DefaultRequestHeaders.Add("X-Session-Id", sessionId);

            person.Id_prav = ViewBag.PravaIdCreated;

            try
            {
                var response = await client.PutAsJsonAsync<Person>("api/Person/Update-Person", person);
            }catch(Exception ex)
            {

            }
            return View();
        }



        [HttpPost]
        public async Task NewPrava(Prava prava)
        {
            var sessionId = HttpContext.Session.GetString("session_ID");

            var client = _httpClientFactory.CreateClient("ApiClient");
            client.DefaultRequestHeaders.Add("X-Session-Id", sessionId);

            try
            {
                var response = await client.PostAsJsonAsync<Prava>("api/Prava/NewPrava", prava);
            }catch(Exception EX)
            {

            }
        }

        //Create Prava And Person


        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Home");
        }

        // ====================== МЕТОДЫ ФОРМАТИРОВАНИЯ ОШИБОК ======================

        private string FormatApiError(string rawContent, int statusCode, string? reasonPhrase)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"🔴 Ошибка API");
            sb.AppendLine($"Статус: {statusCode} ({reasonPhrase})");
            sb.AppendLine();

            if (string.IsNullOrWhiteSpace(rawContent))
            {
                sb.AppendLine("Тело ответа пустое.");
                return sb.ToString();
            }

            sb.AppendLine("Сырой ответ сервера:");
            sb.AppendLine(rawContent);
            sb.AppendLine();

            // Пытаемся красиво разобрать JSON
            try
            {
                using var doc = JsonDocument.Parse(rawContent);
                var root = doc.RootElement;

                sb.AppendLine("--- Разобранный JSON ---");

                // Стандартный ProblemDetails / ValidationProblemDetails
                if (root.TryGetProperty("title", out var title))
                    sb.AppendLine($"Title: {title.GetString()}");

                if (root.TryGetProperty("detail", out var detail))
                    sb.AppendLine($"Detail: {detail.GetString()}");

                if (root.TryGetProperty("type", out var type))
                    sb.AppendLine($"Type: {type.GetString()}");

                if (root.TryGetProperty("status", out var status))
                    sb.AppendLine($"Status: {status}");

                if (root.TryGetProperty("instance", out var instance))
                    sb.AppendLine($"Instance: {instance.GetString()}");

                // Ошибки валидации (самый частый случай при 400)
                if (root.TryGetProperty("errors", out var errors) && errors.ValueKind == JsonValueKind.Object)
                {
                    sb.AppendLine();
                    sb.AppendLine("Ошибки валидации:");
                    foreach (var prop in errors.EnumerateObject())
                    {
                        sb.AppendLine($"  • {prop.Name}:");
                        foreach (var msg in prop.Value.EnumerateArray())
                        {
                            sb.AppendLine($"      - {msg.GetString()}");
                        }
                    }
                }

                // Иногда ошибка лежит просто в message
                if (root.TryGetProperty("message", out var message))
                    sb.AppendLine($"Message: {message.GetString()}");

                // Иногда в exceptionMessage / MessageText (Npgsql)
                if (root.TryGetProperty("exceptionMessage", out var exMsg))
                    sb.AppendLine($"Exception: {exMsg.GetString()}");

                if (root.TryGetProperty("MessageText", out var msgText))
                    sb.AppendLine($"MessageText: {msgText.GetString()}");
            }
            catch
            {
                sb.AppendLine("(не удалось распарсить как JSON)");
            }

            var result = sb.ToString();
            Console.WriteLine(result); // дублируем в консоль
            return result;
        }

        private string FormatException(Exception ex, string actionName)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"🚨 Исключение при: {actionName}");
            sb.AppendLine($"Тип: {ex.GetType().FullName}");
            sb.AppendLine($"Сообщение: {ex.Message}");
            sb.AppendLine();
            sb.AppendLine("StackTrace:");
            sb.AppendLine(ex.StackTrace);

            if (ex.InnerException != null)
            {
                sb.AppendLine();
                sb.AppendLine("--- Inner Exception ---");
                sb.AppendLine($"Тип: {ex.InnerException.GetType().FullName}");
                sb.AppendLine($"Сообщение: {ex.InnerException.Message}");
                sb.AppendLine(ex.InnerException.StackTrace);
            }

            var result = sb.ToString();
            Console.WriteLine(result);
            return result;
        }
    }
}