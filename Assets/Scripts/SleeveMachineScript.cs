using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleeveMachineScript : MonoBehaviour
{
    GameManager gM;
    void Start()
    {
        gM = FindObjectOfType<GameManager>();
        Transform particlePoint = transform.GetChild(0);
        Instantiate(gM.sleeveMachineParticle, particlePoint.position + Vector3.down, Quaternion.Euler(-90, 0, 0) );
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CollectedCup"))
        {
            gM.PutSleeveToCup(other.gameObject);
        }
    }
}
