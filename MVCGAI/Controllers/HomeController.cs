using Microsoft.AspNetCore.Mvc;
using MVCGAI.Models;
using System.Diagnostics;
using MVCGAI.Models;
using API_GAI.DbServices.SRC.Models;
using System.Threading.Tasks;
using System.Text.Json;

namespace MVCGAI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IHttpClientFactory _httpClientFactory;

        public HomeController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }


        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Users model)
        {
            if (!ModelState.IsValid)
            {
                // Явно возвращаем страницу Index при ошибке валидации
                return View("Index", model);
            }

            try
            {
                var client = _httpClientFactory.CreateClient("ApiClient");
                var response = await client.PostAsJsonAsync("api/User/test-auth", model);

                if (response.IsSuccessStatusCode)
                {
                    string rawResponse = await response.Content.ReadAsStringAsync();

                    // Парсим JSON без создания моделей
                    using (JsonDocument doc = JsonDocument.Parse(rawResponse))
                    {
                        JsonElement root = doc.RootElement;

                        // ВЫТАСКИВАЕМ 2 ОТДЕЛЬНЫЕ СТРОКИ
                        // Внимание: имена свойств ("id", "role") чувствительны к регистру! 
                        // Посмотри в Swagger, как они пишутся: с большой буквы или с маленькой.
                        string sessionId = root.GetProperty("session_ID").GetString() ?? string.Empty;
                        string role = root.GetProperty("role").GetString() ?? "User";

                        // Теперь работаем с ними по отдельности как с обычными строками!
                        HttpContext.Session.SetString("session_ID", sessionId);
                        HttpContext.Session.SetString("role", role);
                        HttpContext.Session.SetString("UserName", model.Name);
                    }

                    return RedirectToAction("Index", "StationManager");
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    // 2. Используем ViewBag для совместимости с вашей версткой
                    ViewBag.Error = "Ошибка авторизации: " + error;
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Не удалось подключиться к API: " + ex.Message;
            }

            // 3. Явно возвращаем Index, так как мы находимся на этой странице
            return View("Index", model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
