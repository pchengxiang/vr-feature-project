using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilityUI : MonoBehaviour
{
    Canvas UI;
    [SerializeField] Vector3 deltaPosition;
    [SerializeField] Vector2 resize;
    [SerializeField] Vector3 Rotation;
    [SerializeField] bool moveWithView;
    [SerializeField] Transform StartPosition;

    public void Awake()
    {
        UI = GetComponent<Canvas>();
        if (UI == null)
            Debug.LogError("[PortableUI]:No canvas component in this object.");
       
    }
    // Start is called before the first frame update
    void Start()
    {
        if(PlatformOperationGenerator.instance.Mode == PlatformOperationGenerator.OperationMode.VR)
        {
            TransformIntoPortable();
        }
    }

    public void TransformIntoPortable()
    {
        UI.renderMode = RenderMode.WorldSpace;
        Vector3 scale = transform.localScale;
        scale.x *= resize.x;
        scale.y *= resize.y;
        scale.z = 0;
        transform.localScale = scale;
        transform.rotation = Quaternion.Euler(Rotation.x, Rotation.y, Rotation.z);
        transform.position = StartPosition.position;
        transform.rotation = StartPosition.rotation;
    }

    public void Freedom()
    {
        UI.renderMode = RenderMode.ScreenSpaceOverlay;
    }
}
