using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class EndHand : MonoBehaviour
{
    public int multiplier;
    public float animSens = 1f;
    public int price = 1;
    GameManager gM;
    bool cupTaken = false;
    Vector3 targetPos = Vector3.zero;
    void Start()
    {
        gM = FindObjectOfType<GameManager>();
        targetPos += transform.localPosition.x > 0 ? new Vector3(7, 7, 0) : new Vector3(-7, 7, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CollectedCup") && !cupTaken)
        {
            cupTaken = true;
            gM.LeftCupOnFinish(other.gameObject, gameObject, price);
        }
    }

    public void TakeAnimStarter()
    {
        StartCoroutine(TakeAnim());
    }

    IEnumerator TakeAnim()
    {

        while (transform.rotation.x < 90)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0,90f,0), animSens * Time.deltaTime);
            transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, animSens * Time.deltaTime);
            yield return new WaitForSeconds(0.01f);
        }
    }
}
