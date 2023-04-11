using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CupScript : MonoBehaviour
{
    GameManager gM;

    public Cup cup;
    public string cupName;
    public int price;

    public Vector3 defScale;

    public bool collected = false;

    public bool isLidded = false;

    public bool isFilled = false;

    public bool isSleeved = false;

    public bool resizing = false;
    bool growing = false;
    bool smalling = false;
    float scaleSpeed = 0;
    float scaleCons = 0;
    bool signaled = false;
    Vector3 targetScale = Vector3.zero;

    // Start is called before the first frame update
    void Awake()
    {
        gM = FindObjectOfType<GameManager>();

        cupName= cup.cupName;

        price = cup.price;

        GetComponent<Renderer>().material = cup.mat; 
        
        defScale = transform.localScale;
    }

    private void FixedUpdate()
    {
        if (resizing)
        {
            Anim();
        }

    }

    void OnTriggerEnter(Collider other)
    {
        if (gameObject.CompareTag("CollectableCup") && other.gameObject.CompareTag("CollectedCup") && !collected && !gM.stopMoving)
        {
            gM.CollectCup(gameObject);
        }

        if (other.gameObject.CompareTag("Ground") && gameObject.CompareTag("DroppedCup"))
        {
            GetComponent<Rigidbody>().isKinematic = true;
            Vector3 targetPos = transform.position;
            //targetPos.y = 0;
            transform.position = targetPos;
            gameObject.tag = "CollectableCup";
        }
    }

    public void PutLid(int lidsPrice)
    {
        transform.GetChild(0).gameObject.SetActive(true);
        isLidded= true;
        price += lidsPrice;
    }

    public void PutSleeve(int sleevesPrice)
    {
        print("sleeved");
        transform.GetChild(2).gameObject.SetActive(true);
        isSleeved = true;
        price += sleevesPrice;
    }

    public void FillTheCup(int fillPrice)
    {
        transform.GetChild(1).GetComponent<Renderer>().material.SetFloat("_Fill", 0.5f);
        // Put filled method/function here
        isFilled = true;
        price += fillPrice;
    }

    public void Anim()
    {
        if (growing)
        {
            print("growing");
            //SetSizeDefault();
            // Büyütme iþlemi
            if (transform.localScale.x < targetScale.x)
            {
                transform.localScale = Vector3.Lerp(transform.localScale, targetScale * 1.2f, scaleSpeed * Time.deltaTime);
            }

            else
            {
                transform.localScale = targetScale;
                growing = false;
                smalling = true;
            }

            if(!signaled && transform.localScale.x > targetScale.x / 15*13)
            {
                signaled = true;
                int siblingIndex = transform.GetSiblingIndex();
                if (siblingIndex != 0)
                {
                    transform.parent.GetChild(siblingIndex - 1).GetComponent<CupScript>().Scale(scaleSpeed, scaleCons);
                }
            }
        }        


        if(smalling)
        {
            print("smalling");
            if (transform.localScale.x > defScale.x)
            {
                transform.localScale = Vector3.Lerp(transform.localScale, defScale / 1.2f, scaleSpeed * Time.deltaTime);
            }
            else
            {
                SetSizeDefault();
                resizing = false;
                smalling = false;
                signaled = false;
            }
        }
    }

    public void Scale(float scalingSens, float maxScaleConstant)
    {
        resizing = false;
        growing = false;
        smalling = false;
        scaleSpeed = scalingSens;
        scaleCons = maxScaleConstant;
        targetScale = defScale * scaleCons;
        growing = true;
        resizing = true;
    }

    public void SetSizeDefault()
    {
        transform.localScale = defScale;
    }

    /*public IEnumerator ScaleObject(float scalingSens, float maxScaleConstant)
    {
        if(!resizing)
        {
            resizing = true;
            Vector3 targetScale = defScale * maxScaleConstant;

            SetSizeDefault();
            // Büyütme iþlemi
            while (transform.localScale.x < targetScale.x)
            {
                transform.localScale = Vector3.Lerp(transform.localScale, targetScale * 1.2f, scalingSens * Time.deltaTime);
                yield return new WaitForSeconds(Time.fixedDeltaTime);
            }

            if (transform.localScale.x >= targetScale.x)
            {
                transform.localScale = targetScale;
            }

            int siblingIndex = transform.GetSiblingIndex();
            print("index: " + siblingIndex);
            if (siblingIndex != 0)
            {
                StartCoroutine(transform.parent.GetChild(siblingIndex - 1).GetComponent<CupScript>().ScaleObject(scalingSens, maxScaleConstant));
            }

            yield return new WaitForSeconds(Time.fixedDeltaTime);

            // Küçültme iþlemi
            while (transform.localScale.x > defScale.x)
            {
                transform.localScale = Vector3.Lerp(transform.localScale, defScale / 1.2f, scalingSens * Time.deltaTime);
                yield return new WaitForSeconds(Time.fixedDeltaTime);
            }
            SetSizeDefault();
            resizing = false;
        }
        *//*
        SetSizeDefault();
        Vector3 targetScale = defScale * maxScaleConstant; // Hedef scale deðerleri

        // Büyütme iþlemi
        while (transform.localScale.x < targetScale.x)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale * 1.2f, scalingSens * Time.deltaTime);
            yield return new WaitForSeconds(0.001f);
        }

        if (transform.localScale.x >= targetScale.x)
        {
            transform.localScale = targetScale;
        }
        int siblingIndex = transform.GetSiblingIndex();
        print("index: " + siblingIndex);
        if (siblingIndex != 0)
        {
            StartCoroutine(transform.parent.GetChild(siblingIndex - 1).GetComponent<CupScript>().ScaleObject(scalingSens, maxScaleConstant));
        }

        yield return new WaitForSeconds(0.001f);

        // Küçültme iþlemi
        while (transform.localScale.x > defScale.x)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, defScale / 1.2f, scalingSens * Time.deltaTime);
            yield return new WaitForSeconds(0.001f);
        }
        SetSizeDefault();
        *//*
    }*/


}
