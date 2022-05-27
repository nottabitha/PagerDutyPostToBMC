using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PagerDutyFunctionApp
{
    
    public class Rootobject
    {
        public Message[] messages { get; set; }
    }

    public class Message
    {
        public string _event { get; set; }
        public Log_Entries[] log_entries { get; set; }
        public Webhook webhook { get; set; }
        public IncidentInfo incident { get; set; }
        public string id { get; set; }
        public DateTime created_on { get; set; }
        public Account_Features account_features { get; set; }
        public string account_id { get; set; }
    }

    public class Webhook
    {
        public string endpoint_url { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public Webhook_Object webhook_object { get; set; }
        public Config config { get; set; }
        public Outbound_Integration outbound_integration { get; set; }
        public Accounts_Addon accounts_addon { get; set; }
        public string id { get; set; }
        public string type { get; set; }
        public string summary { get; set; }
        public string self { get; set; }
        public object html_url { get; set; }
    }

    public class Webhook_Object
    {
        public string id { get; set; }
        public string type { get; set; }
        public string summary { get; set; }
        public string self { get; set; }
        public string html_url { get; set; }
    }

    public class Config
    {
        public string referer { get; set; }
    }

    public class Outbound_Integration
    {
        public string id { get; set; }
        public string type { get; set; }
        public string summary { get; set; }
        public string self { get; set; }
        public object html_url { get; set; }
    }

    public class Accounts_Addon
    {
        public string id { get; set; }
        public string addon_id { get; set; }
        public string name { get; set; }
        public string version { get; set; }
        public string location { get; set; }
        public string key { get; set; }
        public string url { get; set; }
        public string description { get; set; }
        public string help_url { get; set; }
        public Config1 config { get; set; }
        public string type { get; set; }
        public string summary { get; set; }
        public string self { get; set; }
        public string html_url { get; set; }
    }

    public class Config1
    {
    }

    public class IncidentInfo
    {
        public int incident_number { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public DateTime created_at { get; set; }
        public string status { get; set; }
        public object incident_key { get; set; }
        public Service service { get; set; }
        public Assignment[] assignments { get; set; }
        public string assigned_via { get; set; }
        public DateTime last_status_change_at { get; set; }
        public First_Trigger_Log_Entry first_trigger_log_entry { get; set; }
        public Alert_Counts alert_counts { get; set; }
        public bool is_mergeable { get; set; }
        public Escalation_Policy1 escalation_policy { get; set; }
        public Team1[] teams { get; set; }
        public Impacted_Services[] impacted_services { get; set; }
        public object[] pending_actions { get; set; }
        public Acknowledgement[] acknowledgements { get; set; }
        public object basic_alert_grouping { get; set; }
        public object alert_grouping { get; set; }
        public Last_Status_Change_By last_status_change_by { get; set; }
        public Metadata1 metadata { get; set; }
        public object[] external_references { get; set; }
        public object priority { get; set; }
        public object[] incidents_responders { get; set; }
        public object[] responder_requests { get; set; }
        public object[] subscriber_requests { get; set; }
        public string urgency { get; set; }
        public string id { get; set; }
        public string type { get; set; }
        public string summary { get; set; }
        public string self { get; set; }
        public string html_url { get; set; }
        public Alert[] alerts { get; set; }
    }

    public class Service
    {
        public string id { get; set; }
        public string name { get; set; }
        public object description { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public string status { get; set; }
        public Team[] teams { get; set; }
        public string alert_creation { get; set; }
        public object[] addons { get; set; }
        public object[] scheduled_actions { get; set; }
        public object support_hours { get; set; }
        public DateTime last_incident_timestamp { get; set; }
        public Escalation_Policy escalation_policy { get; set; }
        public Incident_Urgency_Rule incident_urgency_rule { get; set; }
        public object acknowledgement_timeout { get; set; }
        public object auto_resolve_timeout { get; set; }
        public object alert_grouping { get; set; }
        public object alert_grouping_timeout { get; set; }
        public Alert_Grouping_Parameters alert_grouping_parameters { get; set; }
        public Integration[] integrations { get; set; }
        public Metadata metadata { get; set; }
        public object response_play { get; set; }
        public string type { get; set; }
        public string summary { get; set; }
        public string self { get; set; }
        public string html_url { get; set; }
    }

    public class Escalation_Policy
    {
        public string id { get; set; }
        public string type { get; set; }
        public string summary { get; set; }
        public string self { get; set; }
        public string html_url { get; set; }
    }

    public class Incident_Urgency_Rule
    {
        public string type { get; set; }
        public string urgency { get; set; }
    }

    public class Alert_Grouping_Parameters
    {
        public object type { get; set; }
        public object config { get; set; }
    }

    public class Metadata
    {
    }

    public class Team
    {
        public string id { get; set; }
        public string type { get; set; }
        public string summary { get; set; }
        public string self { get; set; }
        public string html_url { get; set; }
    }

    public class Integration
    {
        public string id { get; set; }
        public string type { get; set; }
        public string summary { get; set; }
        public string self { get; set; }
        public string html_url { get; set; }
    }

    public class First_Trigger_Log_Entry
    {
        public string id { get; set; }
        public string type { get; set; }
        public string summary { get; set; }
        public string self { get; set; }
        public string html_url { get; set; }
    }

    public class Alert_Counts
    {
        public int all { get; set; }
        public int triggered { get; set; }
        public int resolved { get; set; }
    }

    public class Escalation_Policy1
    {
        public string id { get; set; }
        public string type { get; set; }
        public string summary { get; set; }
        public string self { get; set; }
        public string html_url { get; set; }
    }

    public class Last_Status_Change_By
    {
        public string id { get; set; }
        public string type { get; set; }
        public string summary { get; set; }
        public string self { get; set; }
        public string html_url { get; set; }
    }

    public class Metadata1
    {
    }

    public class Assignment
    {
        public DateTime at { get; set; }
        public Assignee assignee { get; set; }
    }

    public class Assignee
    {
        public string id { get; set; }
        public string type { get; set; }
        public string summary { get; set; }
        public string self { get; set; }
        public string html_url { get; set; }
    }

    public class Team1
    {
        public string id { get; set; }
        public string type { get; set; }
        public string summary { get; set; }
        public string self { get; set; }
        public string html_url { get; set; }
    }

    public class Impacted_Services
    {
        public string id { get; set; }
        public string type { get; set; }
        public string summary { get; set; }
        public string self { get; set; }
        public string html_url { get; set; }
    }

    public class Acknowledgement
    {
        public DateTime at { get; set; }
        public Acknowledger acknowledger { get; set; }
    }

    public class Acknowledger
    {
        public string id { get; set; }
        public string type { get; set; }
        public string summary { get; set; }
        public string self { get; set; }
        public string html_url { get; set; }
    }

    public class Alert
    {
        public string alert_key { get; set; }
    }

    public class Account_Features
    {
        public bool response_automation { get; set; }
    }

    public class Log_Entries
    {
        public string id { get; set; }
        public string type { get; set; }
        public string summary { get; set; }
        public string self { get; set; }
        public object html_url { get; set; }
        public DateTime created_at { get; set; }
        public Agent agent { get; set; }
        public Channel channel { get; set; }
        public Service1 service { get; set; }
        public Incident1 incident { get; set; }
        public Team2[] teams { get; set; }
        public object[] contexts { get; set; }
        public Event_Details event_details { get; set; }
    }

    public class Agent
    {
        public string id { get; set; }
        public string type { get; set; }
        public string summary { get; set; }
        public string self { get; set; }
        public string html_url { get; set; }
    }

    public class Channel
    {
        public string type { get; set; }
        public string name { get; set; }
        public string addon_id { get; set; }
        public string accounts_addon_id { get; set; }
        public bool details_omitted { get; set; }
        public bool body_omitted { get; set; }
    }

    public class Service1
    {
        public string id { get; set; }
        public string type { get; set; }
        public string summary { get; set; }
        public string self { get; set; }
        public string html_url { get; set; }
    }

    public class Incident1
    {
        public string id { get; set; }
        public string type { get; set; }
        public string summary { get; set; }
        public string self { get; set; }
        public string html_url { get; set; }
    }

    public class Event_Details
    {
    }

    public class Team2
    {
        public string id { get; set; }
        public string type { get; set; }
        public string summary { get; set; }
        public string self { get; set; }
        public string html_url { get; set; }
    }

}
