using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickPlayerExample : MonoBehaviour
{
    public float speed;
    public float maxSpeed;
    // public VariableJoystick variableJoystick;
    public Joystick js;
    public Rigidbody rb;

    public void Update()
    {
        if (Input.GetMouseButton(0))
        {
            PlayerMovement();
        }
        else
        {
            rb.velocity = Vector3.zero;
        }
    }

    void PlayerMovement()
    {
        //Vector3 direction = (Vector3.right * js.Vertical + Vector3.forward * js.Vertical) + (Vector3.right * js.Horizontal + Vector3.forward * js.Horizontal);
        Vector3 direction = new Vector3(js.Horizontal, 0f, js.Vertical);
        direction = Camera.main.transform.TransformDirection(direction);
        rb.AddForce(direction * speed * Time.deltaTime, ForceMode.VelocityChange);
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
    }
}