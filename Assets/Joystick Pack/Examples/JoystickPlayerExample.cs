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
    public GameObject unservedCoffees;
    public GameObject cam;
    public Animator playerAnimator;

    bool takingCoffee = false;

    GameManagerIdle gM;
    float timer = 1;
    void Start()
    {
        gM = FindObjectOfType<GameManagerIdle>();
    }
    public void Update()
    {
        if (Input.GetMouseButton(0))
        {
            PlayerMovement();
            playerAnimator.SetBool("Walking", true);
        }
        else
        {
            rb.velocity = Vector3.zero;
            playerAnimator.SetBool("Walking", false);
        }

        if (takingCoffee && unservedCoffees.transform.childCount>0)
        {
            timer -= Time.deltaTime;
            TransferToPlayer();
        }
    }

    public void FixedUpdate()
    {
        CamareMovement();
    }

    void PlayerMovement()
    {
        //Vector3 direction = (Vector3.right * js.Vertical + Vector3.forward * js.Vertical) + (Vector3.right * js.Horizontal + Vector3.forward * js.Horizontal);
        Vector3 direction = new Vector3(js.Horizontal, 0, js.Vertical);
        direction = Camera.main.transform.TransformDirection(direction);
        rb.AddForce(direction * speed * Time.deltaTime, ForceMode.VelocityChange);
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
        transform.LookAt(transform.position + direction - Vector3.up * direction.y);
    }

    void CamareMovement()
    {
        cam.transform.LookAt(transform.position);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("PlayerTakeCoffe") && !takingCoffee)
        {
            gM.WorkerLeftObject(transform.GetChild(0).gameObject, transform.GetChild(0).gameObject, unservedCoffees.transform.GetChild(unservedCoffees.transform.childCount - 1).gameObject);
            takingCoffee = true;
            timer = 1;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PlayerTakeCoffe") && takingCoffee)
        {
            takingCoffee = false;
        }
    }

    void TransferToPlayer()
    {
        if(timer < 0.3)
        {
            print("coffe to player");
            gM.WorkerLeftObject(transform.GetChild(0).gameObject, transform.GetChild(0).gameObject, unservedCoffees.transform.GetChild(unservedCoffees.transform.childCount - 1).gameObject);
            timer = 1;
        }
    }
}