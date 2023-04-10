using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleeveMachineScript : MonoBehaviour
{
    public float slideSpeed = 1f;
    public float breakSlidePointY = 0.1f;
    GameManager gM;
    GameObject slidingSleeve;
    Vector3 slideStartPoint;
    Vector3 slideEndPoint;
    void Start()
    {
        slideStartPoint = transform.GetChild(5).position;
        slideEndPoint = transform.GetChild(4).position;
        gM = FindObjectOfType<GameManager>();
        slidingSleeve = transform.GetChild(3).gameObject;
        Transform particlePoint = transform.GetChild(0);
        Instantiate(gM.sleeveMachineParticle, particlePoint.position + Vector3.down, Quaternion.Euler(-90, 0, 0) );
    }

    private void Update()
    {
        SlideTheLid();
    }

    private void SlideTheLid()
    {
        if(slidingSleeve.transform.position.y < slideEndPoint.y - breakSlidePointY)
        {
            slidingSleeve.transform.position = Vector3.Lerp(slidingSleeve.transform.position, slideEndPoint, slideSpeed * Time.deltaTime);
        }
        else
        {
            slidingSleeve.transform.position = slideStartPoint;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CollectedCup"))
        {
            gM.PutSleeveToCup(other.gameObject);
            transform.GetChild(transform.childCount - 1).GetComponent<MachineCanvasSc>().TrigMachineCanvas(gM.sleevePrice);
        }
    }
}
