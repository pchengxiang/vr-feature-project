using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpotLight : MonoBehaviour
{
    public Light SpotLight;
    public Transform Player;
    PlatformOperationGenerator generator;
    // Start is called before the first frame update
    void Start()
    {
        generator = PlatformOperationGenerator.instance;
        generator.InitializedEvent.AddListener(() =>
        {
            Player = generator.Player.transform;
            transform.SetParent(Player);
        }
            
        ); ;
    }
}
