using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LidMachineScript : MonoBehaviour
{
    GameManager gM;
    GameObject movingPart;

    public float upDownSpeed = 1f;
    public float downLimit = 0.96f;

    Vector3 movingPartUpPos;
    Vector3 movingPartDownPos;
    Vector3 targetPos;

    bool moveDown = true;

    // Start is called before the first frame update
    void Awake()
    {
        gM = FindObjectOfType<GameManager>();
        movingPart = transform.GetChild(2).gameObject;

        movingPartUpPos = movingPart.transform.position;
        movingPartDownPos = movingPart.transform.position - Vector3.up * downLimit;
    }

    // Update is called once per frame
    void Update()
    {
        MoveMovingPart();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CollectedCup"))
        {
            gM.PutLidToCup(other.gameObject);
        }
    }



    void MoveMovingPart()
    {
        if (moveDown)
        {
            targetPos = movingPartDownPos;
        }
        else
        {
            targetPos = movingPartUpPos;
        }

        movingPart.transform.position = Vector3.MoveTowards(movingPart.transform.position, targetPos, upDownSpeed * Time.deltaTime);

        if (movingPart.transform.position.y == movingPartUpPos.y)
        {
            moveDown = true;
        }
        else if (movingPart.transform.position.y == movingPartDownPos.y)
        {
            moveDown = false;
        }
    }
}
