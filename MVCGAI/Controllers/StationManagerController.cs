using API_GAI.DbServices.SRC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
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

        // 1. Главная страница: загружаем и инциденты, и участки для модалки
        public async Task<IActionResult> Index()
        {
            if (TempData["Error"] != null) ViewBag.Error = TempData["Error"];

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
            }
            catch { ViewBag.Error = "Не удалось загрузить данные инцидентов"; }

            // Загружаем участки (они нужны тут, так как модалка рендерится на этой же странице!)
            try
            {
                var responseStations = await client.GetFromJsonAsync<List<PoliceStation>>("api/PoliceStation/get-all-policestation");
                var respclass = await client.GetFromJsonAsync<List<IncidentClassification>>("api/IncidentClassification/Get-Classification");
                ViewBag.PoliceStations = responseStations ?? new List<PoliceStation>();
                ViewBag.IncidentClassification = respclass ?? new List<IncidentClassification>();
            }
            catch { ViewBag.PoliceStations = new List<PoliceStation>(); ViewBag.IncidentClassification = new List<IncidentClassification>(); }

            Console.WriteLine(ViewBag.PoliceStations);
            Console.WriteLine(ViewBag.PoliceStations);
            return View();
        }

        // 2. Редактирование (шлем модель сразу в API, без выкачивания всей базы)
        [HttpPost]
        public async Task<IActionResult> EditIncident(Incident model)
        {
            var sessionId = HttpContext.Session.GetString("session_ID");
            var client = _httpClientFactory.CreateClient("ApiClient");
            client.DefaultRequestHeaders.Add("X-Session-Id", sessionId);

            try
            {
                // Шлем обновление напрямую. Если API требует сначала подгрузить оригинал,
                // то в API должен быть эндпоинт вроде "api/Incidents/GetIncident/" + model.Id
                var response = await client.PutAsJsonAsync("api/Incidents/UpdateIncident", model);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }

                var errorContent = await response.Content.ReadAsStringAsync();
                TempData["Error"] = FormatApiError(errorContent, response.StatusCode.ToString());
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"🚨 Локальное исключение: {ex.Message}";
            }

            return RedirectToAction("Index");
        }

        // 3. Добавление (Имя совпадает с asp-action="CreateIncident" в представлении)
        [HttpPost]
        public async Task<IActionResult> CreateIncident(Incident incident)
        {
            var sessionId = HttpContext.Session.GetString("session_ID");
            var client = _httpClientFactory.CreateClient("ApiClient");
            client.DefaultRequestHeaders.Add("X-Session-Id", sessionId);

            try
            {
                var response = await client.PostAsJsonAsync("api/Incidents/SetIncident", incident);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    TempData["Error"] = FormatApiError(errorContent, response.StatusCode.ToString());
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"🚨 Ошибка отправки: {ex.Message}";
            }

            return RedirectToAction("Index");
        }

        private string FormatApiError(string rawJson, string statusCode)
        {
            // Твой рабочий метод парсинга оставляем без изменений...
            try { /* ... */ return "ошибка"; } catch { return rawJson; }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Home");
        }
    }
}