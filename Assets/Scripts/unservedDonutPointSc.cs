using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class unservedDonutPointSc : MonoBehaviour
{
    GameManagerIdle gM;
    // Start is called before the first frame update
    void Awake()
    {
        gM = FindObjectOfType<GameManagerIdle>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DonutWorker"))
        {
            gM.WorkerLeftObject(gameObject, other.gameObject, other.transform.GetChild(0).GetChild(0).gameObject);
        }
    }
}
