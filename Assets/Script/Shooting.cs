using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public GameObject target;
    public GameObject effect;
    public float effectSpeed;
    // Start is called before the first frame update
    void Start()
    {
        effect.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            
        }
        
    }

    void stopEffect()
    {
        effect.SetActive(false);
    }
    public void Fire()
    {
        Instantiate(target,transform.position,transform.rotation);
        effect.SetActive(true);
        Invoke("stopEffect", effectSpeed);
    }
}
