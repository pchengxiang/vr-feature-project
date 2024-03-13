using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using QuickType;
using System;

[Serializable]
public class Deck:IResource
{
    [SerializeField]
    [JsonProperty("name")]
    public string Name { get; set; }

    [SerializeField]
    [JsonProperty("cards")]
    public string[] Cards { get; set; }
}
