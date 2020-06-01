using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace PlannerSync.ClassLibrary
{
    public class SyncTask
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("dueDateTime")]
        public DateTime DueDateTime { get; set; }
        [JsonPropertyName("title")]
        public string Title { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }
    }
}
