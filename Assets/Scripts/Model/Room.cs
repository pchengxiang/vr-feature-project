namespace QuickType
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using UnityEngine;

    [Serializable]
    public partial class Room:IResource
    {
        [SerializeField]
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        public RoomEventType Type { get; set; }
        [SerializeField]
        [JsonProperty("generate")]
        public Generate[] Generate { get; set; }
        [SerializeField]
        [JsonProperty("rarity")]
        public string Rarity { get; set; }
    }

    public partial class Generate
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("cardinal_amount")]
        public long CardinalAmount { get; set; }
    }
}
public enum RoomEventType
{
    Battle,
    Shop,
    Timer
}