using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleeveMachineScript : MonoBehaviour
{
    public float slideSpeed = 1f;
    public float rotateSpeed = 1f;
    public float breakSlidePointY = 0.1f;
    public MachineCanvasSc macCanvSc;
    GameManager gM;
    GameObject slidingSleeve;
    Vector3 slideStartPoint;
    Vector3 slideEndPoint;
    bool rotating = false;
    bool rotatingFin = false;
    void Start()
    {
        slideStartPoint = transform.GetChild(5).localPosition;
        slideEndPoint = transform.GetChild(4).localPosition;
        gM = FindObjectOfType<GameManager>();
        slidingSleeve = transform.GetChild(3).gameObject;
        Transform particlePoint = transform.GetChild(0);
        Instantiate(gM.sleeveMachineParticle, particlePoint.position + Vector3.down, Quaternion.Euler(-90, 0, 0) );
    }

    private void Update()
    {
        SlideTheLid();
        if(rotating && !rotatingFin)
        {
            RotatePoint();
        }
    }

    private void SlideTheLid()
    {
        if(slidingSleeve.transform.localPosition.y < slideEndPoint.y - breakSlidePointY)
        {
            slidingSleeve.transform.localPosition = Vector3.Lerp(slidingSleeve.transform.localPosition, slideEndPoint, slideSpeed * Time.deltaTime);
        }
        else
        {
            slidingSleeve.transform.localPosition = slideStartPoint;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CollectedCup"))
        {
            gM.PutSleeveToCup(other.gameObject);
            macCanvSc.TrigMachineCanvas(gM.sleevePrice);
            rotating = true;
        }
    }

    void RotatePoint()
    {
        if (transform.rotation.y < 75)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0,80,0), rotateSpeed * Time.deltaTime);
        }
        else
        {
            rotatingFin = true;
        }
    }
}
