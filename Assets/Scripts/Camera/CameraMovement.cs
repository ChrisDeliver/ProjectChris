using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField, Range(0f, 1f)] private float smooth = 0.4f;

    [Header("Player")]
    [SerializeField] private GameObject player;

    private Vector3 velocity = Vector3.zero;

    private void LateUpdate()
    {
        Transform camera = GetComponent<Transform>();

        Vector3 playerPosition = player.transform.position;
        Vector3 newCameraPosition = new(playerPosition.x, playerPosition.y, -10);

        transform.position = Vector3.SmoothDamp(newCameraPosition, camera.position, ref velocity, smooth);
    }
}
