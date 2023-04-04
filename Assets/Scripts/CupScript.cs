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

    public bool resizing = false;

    float startY;

    // Start is called before the first frame update
    void Awake()
    {
        gM = FindObjectOfType<GameManager>();

        cupName= cup.cupName;

        price = cup.price;

        GetComponent<Renderer>().material = cup.mat; 
        
        defScale = transform.localScale;

        startY = transform.localPosition.y;
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

    public IEnumerator ScaleObject(float scalingSens, float maxScaleConstant)
    {

        if(!resizing)
        {
            resizing = true;
            Vector3 targetScale = defScale * maxScaleConstant; // Hedef scale deðerleri

            SetSizeDefault();
            // Büyütme iþlemi
            while (transform.localScale.x < targetScale.x)
            {
                /*if (gM.stopMoving)
                {
                    SetSizeDefault();
                    break;
                }*/
                transform.localScale = Vector3.Lerp(transform.localScale, targetScale * 1.2f, scalingSens * Time.fixedDeltaTime);
                yield return new WaitForSeconds(0.01f);
            }

            if (transform.localScale.x >= targetScale.x)
            {
                transform.localScale = targetScale;
            }

            yield return new WaitForSeconds(0.01f);

            // Küçültme iþlemi
            while (transform.localScale.x > defScale.x)
            {
                /*if (gM.stopMoving)
                {
                    SetSizeDefault();
                    break;
                }*/
                transform.localScale = Vector3.Lerp(transform.localScale, defScale / 1.2f, scalingSens * Time.fixedDeltaTime);
                yield return new WaitForSeconds(0.01f);
            }
            SetSizeDefault();
            resizing = false;
        }
        
    }

    public void SetSizeDefault()
    {
        transform.localScale = defScale;
    }


}
