using App.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Text.Json;
namespace App.Controllers
{
    [AllowAnonymous]
    public class LoginController : Controller
    {
        HttpClient httpClient;
        public LoginController(IHttpClientFactory httpClientFactory)
        {
            httpClient= httpClientFactory.CreateClient();
        }
        public IActionResult Index()
        {
            UserInformation objUserInformation = new UserInformation();
            return View(objUserInformation);
        }
        [HttpPost]
        public async Task<IActionResult> Index(UserInformation user)
        {
            var jsonRequest = JsonSerializer.Serialize(user);
            StringContent oDataToPost = new StringContent(jsonRequest,System.Text.Encoding.UTF8,"application/json");
            var ApiResponse = await httpClient.PostAsync("https://localhost:44300/api/Login/Autheticate", oDataToPost);
            string result = await ApiResponse.Content.ReadAsStringAsync();

            UserInformationFromAPI objUserInformationFromAPI = JsonSerializer.Deserialize<UserInformationFromAPI>(result);

            if (objUserInformationFromAPI.ResponseMessage.ToUpper().Contains("SUCCESS"))
            {
                List<Claim> lstClaims = new List<Claim>();

                lstClaims.Add(new Claim(ClaimTypes.Name, objUserInformationFromAPI.UserName));
                lstClaims.Add(new Claim(ClaimTypes.NameIdentifier, objUserInformationFromAPI.UserName));
               
                foreach (string role in objUserInformationFromAPI.Roles.Split(','))
                {
                    lstClaims.Add(new Claim(ClaimTypes.Role, role));
                }

                IIdentity identity = new ClaimsIdentity(lstClaims, CookieAuthenticationDefaults.AuthenticationScheme);
                IPrincipal principal = new ClaimsPrincipal(identity);
                ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(principal);
                await HttpContext.SignOutAsync();
                await HttpContext.SignInAsync(claimsPrincipal);
                return LocalRedirect("~/Registration/Index");
            }
            else
                return View(user);
           
        }
    }
}
