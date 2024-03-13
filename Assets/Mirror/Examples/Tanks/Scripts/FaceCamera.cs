// Useful for Text Meshes that should face the camera.
using UnityEngine;

namespace Mirror.Examples.Tanks
{
    public class FaceCamera : MonoBehaviour
    {
        public Camera camera;
        // LateUpdate so that all camera updates are finished.
        void LateUpdate()
        {
            transform.LookAt(camera.transform);
        }
    }
}
