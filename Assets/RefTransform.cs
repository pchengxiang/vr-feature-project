using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefTransform : MonoBehaviour
{
    [SerializeField]
    bool minusRotationX;
    [SerializeField]
    bool minusRotationY;
    [SerializeField]
    bool minusRotationZ;
    [SerializeField]
    Transform target;
    [SerializeField]
    Vector3 biaseRotation;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void LateUpdate()
    {
        CalculateBiased();
    }



    private void CalculateBiased()
    {
        
        //if(minusRotationX)
        //    biaseRotation.x = -2 * target.rotation.x;
        //if(minusRotationY)
        //    biaseRotation.y = -2 * target.rotation.y;
        //if(minusRotationZ)
        //    biaseRotation.z = -2 * target.rotation.z;
        
        //var rotationX = 0f;
        //var rotationY = 0f;
        //var rotationZ = 0f;
        //if (transform.rotation.x - target.rotation.x- biaseRotation.x > 1.0f)
        //    rotationX = 0.5f;
        //else if(transform.rotation.x - target.rotation.x - biaseRotation.x < -1.0f)
        //    rotationX= -0.5f;
        //if (transform.rotation.y - target.rotation.y + biaseRotation.y > 1.0f)
        //    rotationY = 0.5f;
        //else if (transform.rotation.y - target.rotation.y +180f + biaseRotation.y < -1.0f)
        //    rotationY = -0.5f;
        //if (transform.rotation.z - target.rotation.z - biaseRotation.z > 1.0f)
        //    rotationZ = 0.5f;
        //else if (transform.rotation.z - target.rotation.z  - biaseRotation.z < -1.0f)
        //    rotationZ = -0.5f;
        
        var rotationY = transform.rotation.y - biaseRotation.y;
        if(rotationY != 0)
            biaseRotation.y = transform.rotation.y;

        //transform.localRotation = Quaternion.Euler(target.transform.rotation.x + biaseRotation.x,
        //                                        target.transform.rotation.y + biaseRotation.y,
        //                                        target.transform.rotation.z + biaseRotation.z);
        transform.Rotate(0, -rotationY, 0);
    }
}
