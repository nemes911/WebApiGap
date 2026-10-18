using API_GAI.DbServices.SRC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using WebApiGap.DbServices.SRC.DTOModels;

namespace MVCGAI.Controllers
{
    public class HeadStationManagerController : Controller
    {

        private readonly ILogger<HeadStationManagerController> _logger;

        private readonly IHttpClientFactory _httpClientFactory;

        public HeadStationManagerController(ILogger<HeadStationManagerController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }



        public async Task<IActionResult> IncidentsByDistrict(int id)
        {
            var sessionId = HttpContext.Session.GetString("session_ID");
            ViewBag.Username = HttpContext.Session.GetString("UserName");
            ViewBag.Role = HttpContext.Session.GetString("role");

            var client = _httpClientFactory.CreateClient("ApiClient");
            client.DefaultRequestHeaders.Add("X-Session-Id", sessionId);

            var response = await client.GetAsync($"api/District/GetStatsByDistrict/{id}");

            if (response.IsSuccessStatusCode)
            {
                ViewBag.IncidentsByDistrict = await response.Content
                    .ReadFromJsonAsync<List<IncidentsByIdDistrict>>();
            }
            else
            {
                var errorBody = await response.Content.ReadAsStringAsync();
                ViewBag.Error = $"Status: {(int)response.StatusCode} {response.StatusCode}\n{errorBody}";
            }

            return View();
        }

        public async Task<IActionResult> Index()
        {
            if(TempData["Error"] != null)
            {
                ViewBag.Error = TempData["Error"];
            }

            var sessionId = HttpContext.Session.GetString("session_ID");
            ViewBag.Username = HttpContext.Session.GetString("UserName");
            ViewBag.Role = HttpContext.Session.GetString("role");

            var client = _httpClientFactory.CreateClient("ApiClient");
            client.DefaultRequestHeaders.Add("X-Session-Id", sessionId);

            try
            {
                var response = await client.GetAsync("api/District/get-all-district");
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<List<District>>();
                    ViewBag.Districts = result;
                       
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    ViewBag.Error = error;
                }

            }catch(Exception ex)
            {

            }
           

             return View();
        }
    }
}
