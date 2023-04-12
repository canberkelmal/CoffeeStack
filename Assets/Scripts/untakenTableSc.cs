using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class untakenTableSc : MonoBehaviour
{
    GameManagerIdle gM;
    // Start is called before the first frame update
    void Awake()
    {
        gM = FindObjectOfType<GameManagerIdle>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CoffeeBox"))
        {
            gM.CoffeeToUntaken(other.gameObject);
        }

      


    }

   
    

   
}
