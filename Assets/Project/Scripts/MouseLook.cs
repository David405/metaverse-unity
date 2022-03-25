using System;
using UnityEngine;
public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public float xRotation = 0f;
    public float yRotation = 0f;
    public Transform playerBody;
    private Vector2 joystickPosition;
    private bool isMobile;

    // Start is called before the first frame update
    void Start()
    {
        playerBody = transform.parent;
        Cursor.lockState = CursorLockMode.Confined;

#if UNITY_ANDROID || UNITY_IOS
        isMobile = true;
#endif
    }

    // Update is called once per frame
    void Update()
    {
        Rotate();
    }

    public void JoystickInput(Vector2 vector2)
    {
        joystickPosition = vector2;

        if (Math.Abs(joystickPosition.normalized.x) < 0.4f)
        {
            joystickPosition = Vector2.zero;
        }
    }

    private void Rotate()
    {
        float mouseX = 0f;

        if (isMobile)
        {
            mouseX = joystickPosition.normalized.x * mouseSensitivity * Time.deltaTime;
        }
        else
        {
            mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        }

        playerBody.Rotate(Vector3.up * mouseX);
    }
}
