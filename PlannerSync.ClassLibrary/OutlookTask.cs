using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace PlannerSync.ClassLibrary
{
    public class OutlookTask
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("status")]
        public string Status { get; set; }
        [JsonPropertyName("subject")]
        public string Subject { get; set; }
        [JsonPropertyName("dueDateTime")]
        public OutlookDateTime DueDateTime { get; set; }
        [JsonPropertyName("startDateTime")]
        public OutlookDateTime StartDateTime { get; set; }
        [JsonPropertyName("body")]
        public OutlookTaskBody Body { get; set; }
    }

    public class OutlookDateTime
    {
        [JsonPropertyName("dateTime")]
        public string DateTime { get; set; }
        [JsonPropertyName("timeZone")]
        public string Timezone { get; set; }
    }

    public class OutlookTaskBody
    {
        [JsonPropertyName("contentType")]
        public string ContentType { get; set; }
        [JsonPropertyName("content")]
        public string Content { get; set; }
    }
}
