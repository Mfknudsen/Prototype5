using UnityEngine;

namespace UI
{
    public class FaceCamera : MonoBehaviour
    {
        [HideInInspector] public new Camera camera;
        
        private void Update()
        {
            if (camera)
                transform.LookAt(camera.transform, Vector3.up);
        }
    }
}
