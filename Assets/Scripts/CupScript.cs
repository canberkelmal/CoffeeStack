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

    Vector3 defScale;

    [DoNotSerialize]
    public bool collected = false;
    // Start is called before the first frame update
    void Start()
    {
        gM = FindObjectOfType<GameManager>();

        cupName= cup.cupName;

        price = cup.price;

        GetComponent<Renderer>().material = cup.mat; 
        
        defScale = transform.localScale;
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
            gameObject.tag = "CollectableCup";
        }
    }

    public IEnumerator ScaleObject(float scalingSens, float maxScaleConstant)
    {
        Vector3 startScale = defScale; // Baþlangýç scale deðerleri
        Vector3 targetScale = startScale * maxScaleConstant; // Hedef scale deðerleri

        // Büyütme iþlemi
        while (transform.localScale.x < targetScale.x)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale * 1.1f, scalingSens * Time.deltaTime);
            yield return new WaitForSeconds(0.01f);
        }

        // Küçültme iþlemi
        while (transform.localScale.x > startScale.x)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, startScale / 1.1f, scalingSens * Time.deltaTime);
            yield return new WaitForSeconds(0.01f);
        }
    }


}
