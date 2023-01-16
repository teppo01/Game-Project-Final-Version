using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    [Header("Sway")]
    [SerializeField] private float swayAmount = 3f;
    [SerializeField] private float swaySmooth = 3f;

    private float mouseX;
    private float mouseY;

    void Update()
    {
        mouseX = Input.GetAxisRaw("Mouse X") * swayAmount;
        mouseY = Input.GetAxisRaw("Mouse Y") * swayAmount;

        AimingSway();        
    }


    private void AimingSway()
    {
        // X and Y -rotations as quaternions
        Quaternion rotationX = Quaternion.AngleAxis(-mouseY, Vector3.right);
        Quaternion rotationY = Quaternion.AngleAxis(mouseX, Vector3.up);

        transform.localRotation = Quaternion.Slerp(transform.localRotation, rotationX*rotationY, swaySmooth * Time.deltaTime);
    }
}
