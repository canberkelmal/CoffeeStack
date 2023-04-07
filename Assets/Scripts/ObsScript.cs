using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86.Avx;

public class ObsScript : MonoBehaviour
{
    GameManager gM;
    SkinnedMeshRenderer sMR;
    public bool isHit = false;
    public Vector3 hitPoint;
    public bool hitBack;
    public float startTime = 0;
    public float timeDiff = 0.5f;
    public float repeatRate = 0.04f;
    public float sizeChange = 0.1f;
    public float resizeMax = 0;
    public float resizeMin = 1;

    bool obs1Growing = false;
    bool obs2Growing = false;
    bool obs3Growing = false;
    bool obs4Growing = false;

    float obs1Size = 0;
    float obs2Size = 0;
    float obs3Size = 0;
    float obs4Size = 0;

    // Start is called before the first frame update
    void Awake()
    {
        gM = FindObjectOfType<GameManager>();
    }

    void Start()
    {
        sMR = transform.GetChild(0).GetComponent<SkinnedMeshRenderer>();
        InvokeRepeating("ResizingObs1", startTime, repeatRate);
        InvokeRepeating("ResizingObs2", startTime + timeDiff, repeatRate);
        InvokeRepeating("ResizingObs3", startTime + timeDiff * 2, repeatRate);
        InvokeRepeating("ResizingObs4", startTime + timeDiff * 3, repeatRate);
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CollectedCup") && other.GetComponent<CupScript>().collected)
        {
            hitPoint = other.transform.position;
            gM.HitCup(other.gameObject, gameObject.GetComponent<ObsScript>());
        }
    }

    void ResizingObs1()
    {
        if (sMR.GetBlendShapeWeight(0) > resizeMax && obs1Growing)
        {
            obs1Size -= sizeChange;
            sMR.SetBlendShapeWeight(0, obs1Size);
        }
        else if (sMR.GetBlendShapeWeight(0) <= resizeMax && obs1Growing)
        {
            obs1Growing = false;
            obs1Size = 1;
            sMR.SetBlendShapeWeight(0, obs1Size);
        }
        else if (sMR.GetBlendShapeWeight(0) < resizeMin && !obs1Growing)
        {
            obs1Size += sizeChange;
            sMR.SetBlendShapeWeight(0, obs1Size);
        }
        else if (sMR.GetBlendShapeWeight(0) >= resizeMin && !obs1Growing)
        {
            obs1Growing = true;
            obs1Size = 99;
            sMR.SetBlendShapeWeight(0, obs1Size);
        }
    }

    void ResizingObs2()
    {
        if (sMR.GetBlendShapeWeight(1) > resizeMax && obs2Growing)
        {
            obs2Size -= sizeChange;
            sMR.SetBlendShapeWeight(1, obs2Size);
        }
        else if (sMR.GetBlendShapeWeight(1) <= resizeMax && obs2Growing)
        {
            obs2Growing = false;
            obs2Size = 1;
            sMR.SetBlendShapeWeight(1, obs2Size);
        }
        else if (sMR.GetBlendShapeWeight(1) < resizeMin && !obs2Growing)
        {
            obs2Size += sizeChange;
            sMR.SetBlendShapeWeight(1, obs2Size);
        }
        else if (sMR.GetBlendShapeWeight(1) >= resizeMin && !obs2Growing)
        {
            obs2Growing = true;
            obs2Size = 99;
            sMR.SetBlendShapeWeight(1, obs2Size);
        }
    }

    void ResizingObs3()
    {
        if (sMR.GetBlendShapeWeight(2) > resizeMax && obs3Growing)
        {
            obs3Size -= sizeChange;
            sMR.SetBlendShapeWeight(2, obs3Size);
        }
        else if (sMR.GetBlendShapeWeight(2) <= resizeMax && obs3Growing)
        {
            obs3Growing = false;
            obs3Size = 1;
            sMR.SetBlendShapeWeight(2, obs3Size);
        }
        else if (sMR.GetBlendShapeWeight(2) < resizeMin && !obs3Growing)
        {
            obs3Size += sizeChange;
            sMR.SetBlendShapeWeight(2, obs3Size);
        }
        else if (sMR.GetBlendShapeWeight(2) >= resizeMin && !obs3Growing)
        {
            obs3Growing = true;
            obs3Size = 99;
            sMR.SetBlendShapeWeight(2, obs3Size);
        }
    }

    void ResizingObs4()
    {
        if (sMR.GetBlendShapeWeight(3) > resizeMax && obs4Growing)
        {
            obs4Size -= sizeChange;
            sMR.SetBlendShapeWeight(3, obs4Size);
        }
        else if (sMR.GetBlendShapeWeight(3) <= resizeMax && obs4Growing)
        {
            obs4Growing = false;
            obs4Size = 1;
            sMR.SetBlendShapeWeight(3, obs4Size);
        }
        else if (sMR.GetBlendShapeWeight(3) < resizeMin && !obs4Growing)
        {
            obs4Size += sizeChange;
            sMR.SetBlendShapeWeight(3, obs4Size);
        }
        else if (sMR.GetBlendShapeWeight(3) >= resizeMin && !obs4Growing)
        {
            obs4Growing = true;
            obs4Size = 99;
            sMR.SetBlendShapeWeight(3, obs4Size);
        }
    }
}
