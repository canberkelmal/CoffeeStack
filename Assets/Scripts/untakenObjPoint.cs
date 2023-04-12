using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class untakenObjPoint : MonoBehaviour
{
    GameManagerIdle gM;
    // Start is called before the first frame update
    void Awake()
    {
        gM = FindObjectOfType<GameManagerIdle>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("coffeeWorker"))
        {
            gM.WorkerTakeObject(other.gameObject, transform.GetChild(transform.childCount-1).gameObject);
        }
    }
}
