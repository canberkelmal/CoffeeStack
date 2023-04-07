using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleeveMachineScript : MonoBehaviour
{
    GameManager gM;
    public GameObject sleeveMachineParticle;
    void Start()
    {
        gM = FindObjectOfType<GameManager>();
        Transform particlePoint = transform.GetChild(1).GetChild(0);
        Instantiate(gM.sleeveMachineParticle, particlePoint.position, Quaternion.identity, particlePoint);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CollectedCup"))
        {
            gM.PutSleeveToCup(other.gameObject);
        }
    }
}
