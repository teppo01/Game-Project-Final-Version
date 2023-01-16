using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public Transform playerbody;
    public static float mouseSensitivity = 100f;
    private float xRotation = 0f;
    private float xMouse;
    private float yMouse;

    void Start()
    {
        // GamePause.cs pitää huolen cursorin lockstatesta pausen mukaisesti
        // Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        xMouse = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        yMouse = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        
        //Pitää kääntää x akseli luku koska negatiivinen katsoo ylös ja positiivinen katsoo alas. Ja hiiri toiminto on mitä on.
        xRotation -= yMouse;
        //Mathf.Clamp tarkistaa et meidän luku ei ole alle -90 tai yli 90. Et me ei voida katsoo selän taakse X akselissa.
        xRotation = Mathf.Clamp(xRotation, -90f, 90);

        playerbody.Rotate(Vector3.up * xMouse);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}
