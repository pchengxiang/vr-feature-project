using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraView : MonoBehaviour
{
    new Camera camera;
    public float distance = 10f;

    private void OnValidate()
    {
        camera = GetComponent<Camera>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    Vector3[] GetCorners(float distance)
    {
        Vector3[] corner = new Vector3[4];

        float halfFOV = camera.fieldOfView * 0.5f * Mathf.Deg2Rad;
        float aspect = camera.aspect;

        float height = distance * Mathf.Tan(halfFOV);
        float width = height * aspect;

        //Upper Left
        corner[0] = new Vector3(
            transform.position.x - width,
            transform.position.y + height,
            transform.position.z + distance);
        //Upper Right
        corner[1] = new Vector3(
            transform.position.x + width,
            transform.position.y + height,
            transform.position.z + distance);
        //Lower Left
        corner[2] = new Vector3(
            transform.position.x - width,
            transform.position.y - height,
            transform.position.z + distance);
        //Lower Right
        corner[3] = new Vector3(
            transform.position.x + width,
            transform.position.y - height,
            transform.position.z + distance);

        //corner[0] = transform.position - (Vector3.right * width);
        //corner[0] += Vector3.up * height;
        //corner[0] += Vector3.forward * distance;


        return corner;
    }

    void FindCorner()
    {
        Vector3[] corners = GetCorners(distance);
        Debug.Log($"{corners[0]}, {corners[1]}, {corners[2]}, {corners[3]}  ");
        Debug.DrawLine(corners[0], corners[1], Color.yellow);
        Debug.DrawLine(corners[1], corners[3], Color.yellow);
        Debug.DrawLine(corners[3], corners[2], Color.yellow);
        Debug.DrawLine(corners[2], corners[0], Color.yellow);
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3[] corners = GetCorners(distance);
        Gizmos.DrawLine(corners[0], corners[1]);
        Gizmos.DrawLine(corners[1], corners[3]);
        Gizmos.DrawLine(corners[3], corners[2]);
        Gizmos.DrawLine(corners[2], corners[0]);
    }
}
