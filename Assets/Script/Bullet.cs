using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public float speed;
    public GameObject target;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward*speed);
        Destroy(target,5);
    }

    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.tag == "Monster"){
            other.transform.GetComponentInParent<MonsterEntity>().TakenDamage(2);
        }
    }
}
