using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PlannerSync.ClassLibrary
{
    public class SyncedTask
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }
        [JsonPropertyName("planner-id")]
        public string PlannerId { get; set; }
        [JsonPropertyName("outlook-id")]
        public string OutlookId { get; set; }
        [JsonPropertyName("duedate")]
        public DateTime DueDate { get; set; }
    }
}
