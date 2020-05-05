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
        [JsonPropertyName("createdDateTime")]
        public string CreatedDatetime { get; set; }
        [JsonPropertyName("status")]
        public string Status { get; set; }
        [JsonPropertyName("importance")]
        public string Importance { get; set; }
        [JsonPropertyName("sensitivity")]
        public string Sensitivity { get; set; }
        [JsonPropertyName("subject")]
        public string Subject { get; set; }
        [JsonPropertyName("completedDateTime")]
        public string CompletedDateTime { get; set; }
        [JsonPropertyName("dueDateTime")]
        public string DueDateTime { get; set; }
        [JsonPropertyName("startDateTime")]
        public string StartDateTime { get; set; }
    }
}
