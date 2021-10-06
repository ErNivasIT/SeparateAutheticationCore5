using AppSingleSignOn.Models;
using Microsoft.AspNetCore.Mvc;
using Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AppSingleSignOn.Controllers
{
    public class RegistrationController : Controller
    {
        public IActionResult Index()
        {
            ParameterBase objParameterBase = new ParameterBase();
            return View(objParameterBase);
        }
        [HttpPost]
        public async Task<IActionResult> Index(ParameterBase objParameterBase)
        {
            List<ParameterPerson> lstPersons = new List<ParameterPerson>();
            string record_id = string.Empty;
            for (int i = 0; i < 3; i++)
            {
                record_id = Guid.NewGuid().ToString();
                lstPersons.Add(new ParameterPerson()
                {
                    Record_ID = record_id,
                    Profile_Photo=await ImageToByteArray(),
                    First_Name = "Person : " + i.ToString(),
                    Middle_Name = i % 3 == 0 ? null : "M " + i.ToString(),
                    Last_Name = "Last " + i.ToString(),
                    DOB = DateTime.Now.AddYears(-15),
                    Category_ID = Convert.ToByte(1 + (i % 3)),
                    Gender_ID = Convert.ToByte(1 + (i % 2)),
                    Father_Name = "Father of Person " + i.ToString(),
                    Person_Address = new List<Person_Address>() {
                      new Person_Address(){House_No="House No "+i.ToString(),Record_ID=record_id,Mohalla=" Mohalla "+i.ToString(), City="City "+i.ToString(), Street="Street "+i.ToString(), State="State "+i.ToString(), PIN="436254", Person_Address_Type=1 },
                      new Person_Address(){ House_No="House No "+i.ToString(),Record_ID=record_id,Mohalla=" Mohalla "+i.ToString(), City="City "+i.ToString(), Street="Street "+i.ToString(), State="State "+i.ToString(), PIN="436254",Person_Address_Type=2 },
                    }
                });
            }
            objParameterBase.People = lstPersons;

            HttpClient _httpClient = new HttpClient();
            var json = JsonConvert.SerializeObject(objParameterBase);
            var oData = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
            var ApiResponse = await _httpClient.PostAsync($"https://localhost:44300/api/Registration/Save", oData);
            string result = await ApiResponse.Content.ReadAsStringAsync();
            return View(objParameterBase);
        }
        public async Task<byte[]> ImageToByteArray()
        {
                return System.IO.File.ReadAllBytes(Directory.GetCurrentDirectory() + "/Images/Profile.jpg");
        }
        public async Task<IActionResult> Bulk() {
            RequestData requestData = new RequestData();
            return View(requestData);
        }
        [HttpPost]
        public async Task<IActionResult> Bulk([FromForm] RequestData requestData,CancellationToken cancellationToken)
        {
            HttpClient _httpClient = new HttpClient();
            var json = JsonConvert.SerializeObject(requestData);
           // var oOnlyData = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
            var oData = new MultipartFormDataContent();
            
            
            for (int i = 0; i < requestData.oFiles.Count(); i++)
            {
                oData.Add(new StreamContent(requestData.oFiles[i].OpenReadStream()) { }, requestData.oFiles[i].Name, requestData.oFiles[i].FileName); ;
            }
            
            oData.Add(new StringContent(requestData.Data.First_Name), "Data.First_Name");
            oData.Add(new StringContent(requestData.Data.Last_Name), "Data.Last_Name");
            oData.Add(new StringContent(requestData.Data.Father_Name), "Data.Father_Name");
            oData.Add(new StringContent(requestData.Data.DOB.ToString()), "Data.DOB");
            
            if (requestData.Data.Tags.Count() > 0)
            {
                foreach (var tag in requestData.Data.Tags[0].Split(','))
                {
                    oData.Add(new StringContent(tag), "Data.Tags");
                }
            }
            oData.Add(new StringContent(requestData.Data.Address.Full_Address), "Data.Address.Full_Address");

            //using var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:44300/api/Registration/Bulk");
            //request.Content = oData;
            //var ApiResponse = await _httpClient.SendAsync(request);

            var ApiResponse = await _httpClient.PostAsync("https://localhost:44300/api/Registration/Bulk", oData, cancellationToken);
            string result = await ApiResponse.Content.ReadAsStringAsync();
            return View(requestData);
        }
    }
}
