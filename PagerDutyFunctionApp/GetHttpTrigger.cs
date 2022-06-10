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
        //logs in and returns the token for authorization
        public static string GetAccessTokenRun(ILogger log)
        {
            //getting credentials from function environment variables which are assigned in azure and linked to key vault
            var secretUser = Environment.GetEnvironmentVariable("pagerduty-service-account");
            var secretPass = Environment.GetEnvironmentVariable("pagerduty-service-account-password");

            log.LogInformation("C# get access token function processed a request.");

            var request = new RestRequest("/api/jwt/login", Method.Post);
            var client = new RestClient("https://agl-qa-restapi.onbmc.com");
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddHeader("Accept", "*/*");
            request.AddParameter("username", secretUser);
            request.AddParameter("password", secretPass);

            var response = client.ExecutePostAsync(request).Result;
            if (response.IsSuccessful)
            {
                log.LogInformation("Login was successful.");
                var token = response.Content.ToString();
                return token;
            }
            else
            {
                log.LogInformation("Login was unsuccessful.");
                return null;
            }
        }

        //uses token for authorization and posts the recieved pagerduty data to bmc
        public static void PostDataToBmcRun(string token, string id, string name, string description, string urgency,
            string incidentNo, ILogger log)
        {
            log.LogInformation("C# post data to bmc function processed a request.");

            var request = new RestRequest("/api/arsys/v1/entry/HPD:IncidentInterface_Create/", Method.Post);
            var client = new RestClient("https://agl-qa-restapi.onbmc.com");
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Authorization", token);

            //assigning variables to incident that will be created in bmc
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

            request.AddStringBody(incident.ToJson(), DataFormat.Json);
            
            var response = client.ExecutePostAsync(request).Result;
            if (response.IsSuccessful)
            {
                log.LogInformation("Post was successful.");
            }
            else
            {
                log.LogInformation("Post was unsuccessful.");
            }
        }

        //logs out user
        public static void LogOutRun(string token, ILogger log)
        {
            log.LogInformation("Log Out Function.");

            var request = new RestRequest("/api/jwt/logout", Method.Post);
            var client = new RestClient("https://agl-qa-restapi.onbmc.com");
            request.AddHeader("Authorization", token);

            var response = client.ExecutePostAsync(request).Result;
            if (response.IsSuccessful)
            {
                log.LogInformation("Successfully logged out.");
            }
            else
            {
                log.LogInformation("Logout unsuccessful.");
            }
        }

        [FunctionName("SendRequest")]
        public static async Task<IActionResult> SentRequestRun(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            //reads the quest being sent from pagerduty and deseralizes
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<Rootobject>(requestBody);
            //grabs values from deserialized pagerduty json and assigns relevant data to variables to be used in incident creation
            string id = data.messages[0].incident.assignments[0].assignee.id;
            string name = data.messages[0].incident.assignments[0].assignee.summary;
            string description = data.messages[0].incident.description;
            string urgency = data.messages[0].incident.urgency;
            string incidentNo = data.messages[0].incident.incident_number.ToString();

            //function calls
            var callerGetAccessToken = GetAccessTokenRun(log).ToString();
            PostDataToBmcRun(callerGetAccessToken, id, name, description, urgency, incidentNo, log);
            LogOutRun(callerGetAccessToken, log);

            return new OkObjectResult("SendRequest function executed successfully.");
        }
    }
}
