using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PagerDutyFunctionApp
{
    internal class Incident
    {
        Dictionary<string, string> dict;
        Dictionary<string, Dictionary<string,string>> test;
        public Incident(string LoginID, string FirstName, string LastName, string ServiceType, 
            string Urgency, string Impact, string Description, string DetailedDescription, string ReportedSource, 
            string AssignedSupportCompany, string AssignedSupportOrganization, string AssignedGroup, string Assignee, string Notes) {

            dict = new Dictionary<string, string>();
            this.LoginID = LoginID;
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.ServiceType = ServiceType;
            this.Urgency = Urgency;
            this.Impact = Impact;
            this.Description = Description;
            this.DetailedDescription = DetailedDescription;
            this.ReportedSource = ReportedSource;
            this.AssignedSupportCompany = AssignedSupportCompany;
            this.AssignedSupportOrganization = AssignedSupportOrganization;
            this.AssignedGroup = AssignedGroup;
            this.Assignee = Assignee;
            this.Notes = Notes; 


        }

        public Incident()
        {
            dict = new Dictionary<string, string>();
            test = new Dictionary<string, Dictionary<string, string>>();
        }

        public string LoginID
        {
            set
            {
                dict.Add("Login_ID", value);
            }
        }

        public string FirstName
        {
            set
            {
                dict.Add("First_Name", value);
            }
        }

        public string LastName
        {
            set
            {
                dict.Add("Last_Name", value);
            }
        }

        public string ServiceType
        {
            set
            {
                dict.Add("Service_Type", value);
            }
        }

        public string Urgency
        {
            set
            {
                dict.Add("Urgency", value);
            }
        }

        public string Impact
        {
            set
            {
                dict.Add("Impact", value);
            }
        }

        public string Description
        {
            set
            {
                dict.Add("Description", value);
            }
        }

        public string DetailedDescription
        {
            set
            {
                dict.Add("Detailed_Decription", value);
            }
        }

        public string ReportedSource
        {
            set
            {
                dict.Add("Reported Source", value);
            }
        }

        public string AssignedSupportCompany
        {
            set
            {
                dict.Add("Assigned Support Company", value);
            }
        }

        public string AssignedSupportOrganization
        {
            set
            {
                dict.Add("Assigned Support Organization", value);
            }
        }

        public string AssignedGroup
        {
            set
            {
                dict.Add("Assigned Group", value);
            }
        }

        public string Assignee
        {
            set
            {
                dict.Add("Assignee", value);
            }
        }

        public string Notes
        {
            set
            {
                dict.Add("Notes", value);
            }
        }

        public string ToJson()
        {
            test.Add("values", dict);
            return JsonConvert.SerializeObject(test);
        }
    }
}
