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
    void Awake()
    {
        gM = FindObjectOfType<GameManager>();

        obsName = obs.name;

        GetComponent<Renderer>().material = obs.mat;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CollectedCup") && other.GetComponent<CupScript>().collected && !isHit)
        {
            gM.HitCup(other.gameObject, gameObject.GetComponent<ObsScript>());
        }
    }
}
