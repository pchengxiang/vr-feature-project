namespace QuickType
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using UnityEngine;
    
    public interface IResource
    {
        string Name { get; set; }
    }
    [Serializable]
    public class Card : IResource
    {
        [SerializeField]
        //[JsonProperty("name")]  
        public string Name { get; set; }
        [SerializeField]
        //[JsonProperty("description")]
        public string Description { get; set; }
        [SerializeField]
        //[JsonProperty("cost_time")]
        public long CostTime { get; set; }
        [SerializeField]
        //[JsonProperty("commands")]
        public Command[] Commands { get; set; }
    }
    [Serializable]
    public class Command
    {
        [SerializeField]
        //[JsonProperty("item")]
        public string Item { get; set; }
        [SerializeField]
        //[JsonProperty("prefab")]
        public string Prefab { get; set; }
        [SerializeField]
        //[JsonProperty("timing")]
        public EffectTiming Timing { get; set; }
        [SerializeField]
        //[JsonProperty("utility")]
        public Utility Utility { get; set; }
        [SerializeField]
        //[JsonProperty("durability")]
        public int Durability { get; set; }

        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }
    }
    [Serializable]
    public struct Utility
    {
        [SerializeField]
        public long? Integer;
        [SerializeField]
        public string String;

        public static implicit operator Utility(long Integer) => new Utility { Integer = Integer };
        public static implicit operator Utility(string String) => new Utility { String = String };
    }

    public enum EffectTiming
    {
        Start,
        Executing,
        Terminate
    }
}