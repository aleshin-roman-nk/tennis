using UnityEngine;

public class LookAtCameraX : MonoBehaviour
{
    private void Start()
    {
        Camera mainCamera = Camera.main;

        if (mainCamera != null)
        {
            Vector3 directionToCamera = mainCamera.transform.position - transform.position;

            Quaternion targetRotation = Quaternion.LookRotation(directionToCamera);

            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, targetRotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        }
    }
}
