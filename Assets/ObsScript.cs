using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86.Avx;

public class ObsScript : MonoBehaviour
{
    GameManager gM;

    public Obstacle obs;
    public bool isHit = false;
    string obsName;

    // Start is called before the first frame update
    void Start()
    {
        gM = FindObjectOfType<GameManager>();

        GetComponent<Renderer>().material = obs.mat;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CollectedCup") && other.GetComponent<CupScript>().collected && !isHit)
        {
            isHit = true;
            gM.HitCup(other.gameObject);
        }
    }
}
