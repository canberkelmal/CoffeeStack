using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class EndHand : MonoBehaviour
{
    public float animSens = 1f;
    public int price = 1;
    GameManager gM;
    bool cupTaken = false;
    bool posSet = false;
    Vector3 targetPos = Vector3.zero;
    GameObject frontArm;
    void Start()
    {
        frontArm = transform.GetChild(0).GetChild(0).GetChild(0).gameObject;
        gM = FindObjectOfType<GameManager>();
        targetPos = transform.localPosition;
        targetPos += transform.localPosition.x > 0 ? new Vector3(7, 7, 0) : new Vector3(-7, 7, 0);
    }

    private void Update()
    {
        if (cupTaken && !posSet)
        {
            TakeAnim();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CollectedCup") && !cupTaken)
        {
            cupTaken = true;
            gM.LeftCupOnFinish(other.gameObject, gameObject, price);
        }
    }

    void TakeAnim()
    {
        if (frontArm.transform.rotation.x > -85)
        {
            frontArm.transform.rotation = Quaternion.Lerp(frontArm.transform.rotation, Quaternion.Euler(0, 180, 0), animSens * Time.deltaTime);
            transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, animSens * Time.deltaTime);
        }
        else
        {
            posSet = true;
        }

    }

    IEnumerator TakeAnim2()
    {
        while (transform.rotation.x < 90)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0,90f,0), animSens * Time.deltaTime);
            transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, animSens * Time.deltaTime);
            yield return new WaitForSeconds(0.01f);
        }
    }
}
