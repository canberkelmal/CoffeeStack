using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86.Avx;

public class ObsScript : MonoBehaviour
{
    GameManager gM;
    public bool isHit = false;
    public Vector3 hitPoint;
    public bool hitBack;

    // Start is called before the first frame update
    void Awake()
    {
        gM = FindObjectOfType<GameManager>();
    }

    void FixedUpdate()
    {
        
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CollectedCup") && other.GetComponent<CupScript>().collected)
        {
            hitPoint = other.transform.position;
            gM.HitCup(other.gameObject, gameObject.GetComponent<ObsScript>());
        }
    }
}
