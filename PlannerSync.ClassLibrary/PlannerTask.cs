using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using System.Text.Json.Serialization;

namespace PlannerSync.ClassLibrary
{
    public class PlannerTask
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("title")]
        public string Title { get; set; }
        [JsonPropertyName("percentComplete")]
        public int PercentComplete { get; set; }
        [JsonPropertyName("startDateTime")]
        public string StartDateTime { get; set; }
        [JsonPropertyName("createdDateTime")]
        public string CreatedDateTime { get; set; }
        [JsonPropertyName("dueDateTime")]
        public string DueDateTime { get; set; }
        [JsonPropertyName("planId")]
        public string PlanId { get; set; }

    }
}
