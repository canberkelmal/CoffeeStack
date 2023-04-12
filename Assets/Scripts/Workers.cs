using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Workers : MonoBehaviour
{
    GameManagerIdle gM;

    public float workerSpeed = 1f;

    public Transform untakenCups;
    public Transform leftedCups;
    public Animator baristaAnimator;

    public bool handled = false;




    // Start is called before the first frame update

    private void Awake()
    {
        gM = FindObjectOfType<GameManagerIdle>();
    }

    void FixedUpdate()
    {
        if ( (gameObject.CompareTag("coffeeWorker") && gM.untakenCoffees.transform.childCount > 0) || handled)
        {
            WorkerMovement();
            baristaAnimator.SetBool("Walking", true);
        }
        else
        {
            baristaAnimator.SetBool("Walking", false);
        }
    }

    public void SetHandled(bool value)
    {
        handled = value;
    }

    public bool GetHandled()
    {
        return handled;
    }

    private void WorkerMovement()
    {

        //Move to coffee and donut
        if (!handled)
        {
            //Look to coffee
            transform.LookAt(untakenCups.position);
            transform.position = Vector3.MoveTowards(transform.position, untakenCups.position, Time.deltaTime * workerSpeed);
        }
        else
        {
            transform.LookAt(leftedCups.position);
            transform.position = Vector3.MoveTowards(transform.position, leftedCups.position, Time.deltaTime * workerSpeed);
        }        
    }



}


    
