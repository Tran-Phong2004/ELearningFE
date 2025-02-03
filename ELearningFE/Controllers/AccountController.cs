using ELearningFE.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace ELearningFE.Controllers
{
    public class AccountController : Controller
    {

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username,string password)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var payload = new { username = username, password = password };
                HttpContent content = new StringContent(System.Text.Json.JsonSerializer.Serialize(payload), System.Text.Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync("https://localhost:7242/api/Auth/Login", content);
                
                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    var authResponse = JsonConvert.DeserializeObject<ApiResponse<LoginData>>(result);
                    if (authResponse.success)
                    {
                        HttpContext.Session.SetString("Username", authResponse.data.UserName);
                        HttpContext.Session.SetString("AccessToken", authResponse.data.AccessToken);
                    }
                    else
                    {
                        TempData["ErrLogin"] = authResponse.message;
                        return View();
                    }
                    return RedirectToAction("Index","Home");
                }
                else
                {
                    return Content("Error: " + response.StatusCode);
                }
            }
        }

        public async Task<IActionResult> Logout()
        {
          
            using (HttpClient client = new HttpClient())
            {
                string url = "https://localhost:7242/api/Auth/Logout";

               
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);

                // Lấy access token từ session
                string accessToken = HttpContext.Session.GetString("AccessToken");

                if (!string.IsNullOrEmpty(accessToken))
                {
                    // Thêm header Authorization
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                }

                // Gửi request và nhận response
                HttpResponseMessage response = await client.SendAsync(request);

                // Kiểm tra kết quả
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("Response body:");
                    Console.WriteLine(responseBody);
                    HttpContext.Session.Clear();
                }
                else
                {
                    Console.WriteLine($"Request failed with status code: {response.StatusCode}");
                }
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string username, string password)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var payload = new { username = username, password = password };
                HttpContent content = new StringContent(System.Text.Json.JsonSerializer.Serialize(payload), System.Text.Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync("https://localhost:7242/api/Auth/Register", content);

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    var resgisterResponse = System.Text.Json.JsonSerializer.Deserialize<ApiResponse<string>>(result);
                    if (!resgisterResponse.success)
                    {
                        TempData["ErrRegister"] = resgisterResponse.message;
                        return View();
                    }
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    return Content("Error: " + response.StatusCode);
                }
            }
        }
    }
}
