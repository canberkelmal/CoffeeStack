using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTriggerSc2 : MonoBehaviour
{
    GameManager gM;
    void Start()
    {
        gM = FindObjectOfType<GameManager>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CollectedCup"))
        {
            gM.EnterToFinish2();
        }
    }
}
