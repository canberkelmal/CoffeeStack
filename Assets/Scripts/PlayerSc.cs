using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSc : MonoBehaviour
{
    GameManager gM;
    void Start()
    {
        gM = FindObjectOfType<GameManager>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("CollectableCup") && gM.GetCupCount() == 0)
        {
            gM.CollectCup(other.gameObject);
        }
    }
}
