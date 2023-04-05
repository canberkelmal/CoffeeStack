using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LidInMachineScript : MonoBehaviour
{
    Vector3 startPos;
    Vector3 midPosA;
    Vector3 midPosB;
    Vector3 endPos;

    Quaternion startRotation;
    Quaternion endRotation;

    bool passedMidA = false;
    bool passedMidB = false;

    // public float time = 1;

    public float moveSens = 1f;
    public float rotateSens = 1f;
    void Awake()
    {
        startPos = transform.position;
        midPosA = transform.parent.GetChild(3).position;
        midPosB = transform.parent.GetChild(4).position;
        endPos = transform.parent.GetChild(1).position;

        startRotation = transform.rotation;
        endRotation = transform.parent.GetChild(1).rotation;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Time.timeScale = time;
        
        if(transform.position.x != midPosA.x && !passedMidA)
        {
            transform.position = Vector3.MoveTowards(transform.position, midPosA, moveSens * Time.deltaTime); 
            transform.rotation = Quaternion.RotateTowards(transform.rotation, endRotation, rotateSens * Time.deltaTime);
        }
        else if(transform.position.x == midPosA.x && !passedMidA)
        {
            passedMidA = true;
        }
        else if(transform.position.x != midPosB.x && passedMidA && !passedMidB)
        {
            transform.position = Vector3.MoveTowards(transform.position, midPosB, moveSens * Time.deltaTime);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, endRotation, rotateSens * Time.deltaTime);
        }
        else if (transform.position.x == midPosB.x && passedMidA && !passedMidB)
        {
            passedMidB = true;
        }
        else if (transform.position.x != endPos.x && passedMidA && passedMidB)
        {
            transform.position = Vector3.MoveTowards(transform.position, endPos, moveSens * Time.deltaTime);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, endRotation, rotateSens * Time.deltaTime);
        }
        else if(transform.position.x == endPos.x && passedMidA && passedMidB)
        {
            transform.SetPositionAndRotation(startPos, startRotation);
            passedMidA = false;
            passedMidB = false;
        }

        else
        {
            print("Some error occured on moving lid on the lid machine. Check the machine values!");
        }
    }
}
