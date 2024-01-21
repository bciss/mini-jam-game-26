using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCameraController : MonoBehaviour
{
    public float lookSpeed = 2.0f;

    private Vector2 lookInput;

    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    void Update()
    {
        // Adjust the rotation based on the mouse delta
        Vector3 newRotation = transform.eulerAngles;
        newRotation.x -= lookInput.y * lookSpeed;
        newRotation.y += lookInput.x * lookSpeed;

        // Clamp the vertical rotation to avoid over-rotation
        newRotation.x = Mathf.Clamp(newRotation.x, -90f, 90f);

        // Apply the new rotation to the camera
        transform.eulerAngles = newRotation;
    }
}