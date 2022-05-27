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
using System.Collections.Generic;

namespace PagerDutyFunctionApp
{ 
    public class GetHttpTrigger  
    {
        private string token { get; set; }

        public static string GetAccessTokenRun(ILogger log)
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

        public static string PostDataToBmcRun(string token, string id, string name, string description, string urgency,
            string incidentNo, ILogger log)
        {
            log.LogInformation("C# post data to bmc function processed a request.");

            var request = new RestRequest("/api/arsys/v1/entry/HPD:IncidentInterface_Create/", Method.Post);
            var client = new RestClient("https://agl-qa-restapi.onbmc.com");
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Authorization", token);
            Incident incident = new Incident();
            incident.LoginID = "svc_pagerduty";
            incident.FirstName = "";
            incident.LastName = "";
            incident.ServiceType = "User Service Restoration";
            incident.Urgency = urgency;
            incident.Impact = "4-Minor/Localized";
            incident.Description = $"Incident {incidentNo}";
            incident.DetailedDescription = description;
            incident.ReportedSource = "Phone";
            incident.Assignee = name;
            incident.AssignedSupportCompany = "AGL";
            incident.AssignedSupportOrganization = "AGL";
            incident.AssignedGroup = "APP_INTEGRATED_SRE";
            //incident.Notes = notes;

            request.AddStringBody(incident.ToJson(), DataFormat.Json);
            
            var response = client.ExecutePostAsync(request).Result;
            log.LogInformation(response.Content);
            if (response.IsSuccessful)
            {
                log.LogInformation(response.Content.ToString());
                log.LogInformation("Data posted to BMC Helix");
            }

            return response.Content;
        }
        public static void LogOutRun(string token, ILogger log)
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

        [FunctionName("SendRequest")]
        public static async Task<IActionResult> SentRequestRun(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");


            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<Rootobject>(requestBody);
            string id = data.messages[0].incident.assignments[0].assignee.id;
            string name = data.messages[0].incident.assignments[0].assignee.summary;
            string description = data.messages[0].incident.description;
            string urgency = data.messages[0].incident.urgency;
            string incidentNo = data.messages[0].incident.incident_number.ToString();

            var callerGetAccessToken = GetAccessTokenRun(log).ToString();
            var callerPostDataToBmc = PostDataToBmcRun(callerGetAccessToken, id, name, description, urgency, incidentNo, log);
            LogOutRun(callerGetAccessToken, log);

            
                
            string responseMessage = string.IsNullOrEmpty(name)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {name}. This HTTP triggered function executed successfully. {name} {urgency} {description} {id}";

            return new OkObjectResult(responseMessage);
        }
    }
}
