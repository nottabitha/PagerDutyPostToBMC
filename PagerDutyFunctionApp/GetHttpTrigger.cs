using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Configuration;

namespace PagerDutyFunctionApp
{ 
    public class GetHttpTrigger  
    {
        private string token { get; set; }

        [FunctionName("GetAccessToken")]
        public static string GetAccessTokenRun([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]
        HttpRequest req, ILogger log)
        {
            log.LogInformation(Environment.GetEnvironmentVariable("bmcQaUsername"));
            
            var secretUser = Environment.GetEnvironmentVariable("bmcQaUsername");
            var secretPass = Environment.GetEnvironmentVariable("bmcQaPassword");

            log.LogInformation("C# get access token function processed a request.");

            var request = new RestRequest("/api/jwt/login", Method.Post);
            var client = new RestClient("https://agl-qa-restapi.onbmc.com");
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddHeader("Accept", "*/*");
            request.AddParameter("username", secretUser);
            request.AddParameter("password", secretPass);

            var response = client.ExecutePostAsync(request).Result;
            log.LogInformation(response.StatusCode.ToString());
            if (response.IsSuccessful)
            {
                var token = response.Content.ToString();
                return token;
            }
            else
            {
                return null;
            }
        }

        [FunctionName("PostDataToBmc")]
        public static string PostDataToBmcRun([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]
        string token,HttpRequest req, ILogger log)
        {
            string responseMessage = "Hi this worked";
            log.LogInformation("C# post data to bmc function processed a request.");

            var json =
                @"{
                    " + "\n" +
                @"""values"": {
                    " + "\n" +
                @"    ""Login_ID"": ""svc_ignio"",
                    " + "\n" +
                @"    ""First_Name"": ""svc"",
                    " + "\n" +
                @"    ""Last_Name"": ""ignio"",
                    " + "\n" +
                @"    ""Service_Type"": ""User Service Restoration"",
                    " + "\n" +
                @"    ""Urgency"": ""4-Low"",
                    " + "\n" +
                @"    ""Impact"": ""4-Minor/Localized"",
                    " + "\n" +
                @"    ""Description"": ""Incident Creation Test API"",
                    " + "\n" +
                @"    ""Detailed_Decription"": ""Test Test Test "",
                    " + "\n" +
                @"    ""Reported Source"": ""Phone"",
                    " + "\n" +
                @"    ""Assigned Support Company"": ""AGL"",
                    " + "\n" +
                @"    ""Assigned Support Organization"": ""AGL"",
                    " + "\n" +
                @"    ""Assigned Group"": ""APP_INTEGRATED_SRE""
                    " + "\n" +
                @"     }
                    " + "\n" +
                @"}";

            var request = new RestRequest("/api/arsys/v1/entry/HPD:IncidentInterface_Create/", Method.Post);
            var client = new RestClient("https://agl-qa-restapi.onbmc.com");
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Authorization", token); 
            request.AddStringBody(json, DataFormat.Json);

            var response = client.ExecutePostAsync(request).Result;
            log.LogInformation(response.StatusCode.ToString());
            if (response.IsSuccessful)
            {
                log.LogInformation(response.Content.ToString());
                log.LogInformation("Data posted to BMC Helix");
            }

            return response.Content;
        }
        [FunctionName("LogOut")]
        public static void LogOutRun([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]
        string token, HttpRequest req, ILogger log)
        {
            log.LogInformation("Log Out Function.");

            var request = new RestRequest("/api/jwt/logout", Method.Post);
            var client = new RestClient("https://agl-qa-restapi.onbmc.com");
            request.AddHeader("Authorization", token);

            var response = client.ExecutePostAsync(request).Result;
            if (response.IsSuccessful)
            {
                log.LogInformation("Successfully logged out!");
            }
        }
        [FunctionName("GetHttpTrigger")]
        public static async Task<IActionResult> GetHttpTriggerRun(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

           

            var callerGetAccessToken = GetAccessTokenRun(req, log).ToString();
            var callerPostDataToBmc = PostDataToBmcRun(callerGetAccessToken, req, log);
            LogOutRun(callerGetAccessToken, req, log);

            string name = req.Query["name"];
            string id = req.Query["id"];
            string coordGroup = "APP_INTEGRATED SRE";
            string title = req.Query["title"];
            string category = req.Query["category"]; //
            string urgency = req.Query["urgency"];
            string status = req.Query["status"];
            string manGroup = req.Query["manGroup"]; //
            

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;
            id = id ?? data?.id;
            coordGroup = coordGroup ?? data?.coordGroup;
            title = title ?? data?.title;
            category = category ?? data?.category;
            urgency = urgency ?? data?.urgency;
            status = status ?? data?.status;
            manGroup = manGroup ?? data?.manGroup;
                
            string responseMessage = string.IsNullOrEmpty(name)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {callerPostDataToBmc}. This HTTP triggered function executed successfully. {id}{coordGroup}{title}{category}{urgency}{status}{manGroup}";

            return new OkObjectResult(responseMessage);
        }
    }
}
