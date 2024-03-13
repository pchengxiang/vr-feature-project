using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    bool following = false;
    [SerializeField]
    GameObject target;
    public GameObject Target
    {
        get
        {
            return target;
        }
        set
        {
            target = value;
            following= value != null;
        }
    }
    [SerializeField]
    GameObject targetCamera;
    public GameObject TargetCamera
    {
        get { return targetCamera; }
        set { targetCamera = value; }
    }

    public bool LookTarget;
    Vector3 prePos= Vector3.zero;
    public int distance = 20;
    public float speed = 0.1f;

    private void Update()
    {
        if(Vector3.Distance(transform.position, Target.transform.position) < distance)
        {
            LookTransform(TargetCamera.gameObject.transform);
        }
        else
        {
            LookTransform(targetCamera.gameObject.transform); 
            transform.Translate(new Vector3(0,0,speed * Time.deltaTime));
        }
    }

    public void LookTransform(Transform Mtransform)
    {
        Vector3 targetPos = Mtransform.position;
        Vector3 vector = targetPos - transform.position;
        Quaternion targetRot = Quaternion.LookRotation(vector);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, speed*Time.deltaTime);
    }
}