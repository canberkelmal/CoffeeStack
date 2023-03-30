using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CupScript : MonoBehaviour
{
    GameManager gM;

    public Cup cup;
    string cupName;
    int price;
    [DoNotSerialize]
    public bool collected = false;
    // Start is called before the first frame update
    void Start()
    {
        gM = FindObjectOfType<GameManager>();

        cupName= cup.cupName;

        price = cup.price;

        GetComponent<Renderer>().material = cup.mat;        
    }

    void OnTriggerEnter(Collider other)
    {
        if (!gameObject.CompareTag("CollectedCup") && other.gameObject.CompareTag("CollectedCup") && !collected)
        {
            gM.CollectCup(gameObject);
        }
    }
}
