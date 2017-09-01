using UnityEngine;


namespace RPG.Characters
{
    public class FaceCamera : MonoBehaviour
    {

        

        Camera cameraToLookAt;

        // Use this for initialization 
        void Start()
        {
            cameraToLookAt = Camera.main;
            
        }

       
        void LateUpdate()
        {
            transform.LookAt(cameraToLookAt.transform);
        }
    }
}