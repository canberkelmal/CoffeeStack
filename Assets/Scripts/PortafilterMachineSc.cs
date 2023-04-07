using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortafilterMachineSc : MonoBehaviour
{
    GameManager gM;
    void Start()
    {
        gM = FindObjectOfType<GameManager>();
        Transform particlePoint = transform.GetChild(1).GetChild(0);
        Instantiate(gM.flowCoffeParticle, particlePoint.position, Quaternion.identity, particlePoint);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CollectedCup"))
        {
            gM.FillTheCup(other.gameObject);
        }
    }
}
