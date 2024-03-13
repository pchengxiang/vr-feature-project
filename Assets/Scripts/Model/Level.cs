namespace QuickType
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using UnityEngine;

    [Serializable]
    public partial class Level:IResource
    {
        [SerializeField]
        [JsonProperty("name")]
        public string Name { get; set; }
        [SerializeField]
        [JsonProperty("width")]
        public long Width { get; set; }
        [SerializeField]
        [JsonProperty("height")]
        public long Height { get; set; }
        [SerializeField]
        [JsonProperty("generate_rule")]
        public object[] GenerateRule { get; set; }
    }
}
