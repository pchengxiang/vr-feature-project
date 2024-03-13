using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using QuickType;
using UnityEngine;

[Serializable]
public partial class Monster: IResource
{
    [SerializeField]
    [JsonProperty("name")]
    public string Name { get; set; }
    [SerializeField]
    [JsonProperty("hp")]
    public float Hp { get; set; }
    [SerializeField]
    [JsonProperty("baseDamage")]
    public float BaseDamage { get; set; }
}
