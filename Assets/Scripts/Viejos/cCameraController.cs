using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class cCameraController : MonoBehaviour
{
    public float speed = 5.0f;
    public float sensitivity = 30.0f;

    public PlayerInput py;

    float MouseX;
    float MouseY;

    // Start is called before the first frame update
    void Start()
    {
        py = GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void Update()
    {
        /*
        float RotationX = sensitivity * MouseX * Time.deltaTime;
        float RotationY = sensitivity * MouseY * Time.deltaTime;

        Vector3 CameraRotation = transform.rotation.eulerAngles;

        CameraRotation.x -= py.actions["RotateX"].ReadValue
        CameraRotation.y += RotationX;

        transform.rotation = Quaternion.Euler(CameraRotation);
         Vector2 cameraMoveInput = py.actions["Move"].ReadValue<Vector2>();

         // Move the camera forward, backward, left, and right
         transform.position += transform.forward * cameraMoveInput.y * speed * Time.deltaTime;
         transform.position += transform.right * cameraMoveInput.x * speed * Time.deltaTime;
         transform.position += transform.up * py.actions["Strafe"].ReadValue<int>() * speed * Time.deltaTime;

         // Rotate the camera based on the mouse movement
         Vector2 cameraRotation = py.actions["Rotate"].ReadValue<Vector2>();
         /*float mouseX = Input.GetAxis("Mouse X");
         float mouseY = Input.GetAxis("Mouse Y");*/
        /*transform.eulerAngles += new Vector3(-cameraRotation.y * sensitivity, cameraRotation.x * sensitivity, 0);*/

    }

    /*public void OnRotationX(InputAction.CallbackContext Context)
    {
        MouseX = Context.ReadValue<float>();
    }

    public void OnRotationY(InputAction.CallbackContext Context)
    {
        MouseY = Context.ReadValue<float>();
    }*/
}
