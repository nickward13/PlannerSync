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
        public string PrimaryTaskId { get; set; }
        [JsonPropertyName("outlook-id")]
        public string SecondaryTaskId { get; set; }
        [JsonPropertyName("duedate")]
        public DateTime DueDateTime { get; set; }
    }
}
