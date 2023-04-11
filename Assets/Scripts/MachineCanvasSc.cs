using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MachineCanvasSc : MonoBehaviour
{
    public GameObject mainText;
    public GameObject subText;
    GameManager gM;
    float timer = 0f;
    int price = 0;


    public float movementSens = 1;
    public float alphaSens = 1;
    void Start()
    {
        gM = FindObjectOfType<GameManager>();
        Vector3 mainPos = mainText.transform.position;
        Vector3 subPos = subText.transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(timer > 0f)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            mainText.GetComponent<Text>().text = " ";
        }
    }

    public void TrigMachineCanvas(int addedPrice)
    {
        StartCoroutine(TextAnim(Instantiate(subText, subText.transform.position, Quaternion.identity, transform), addedPrice));
        // AnimateText(Instantiate(subText, subText.transform.position, Quaternion.identity, transform), addedPrice);
        mainText.transform.SetAsLastSibling();
    }

    IEnumerator TextAnim(GameObject tempTx, int prc)
    {
        print("fill canvas anim");
        timer = 1f;
        price += prc;
        mainText.GetComponent<Text>().text = "$" + price.ToString();
        while (tempTx.transform.position.y < mainText.transform.position.y - 0.01f)
        {
            tempTx.GetComponent<Text>().text = "$" + prc.ToString();
            tempTx.transform.position = Vector3.Lerp(tempTx.transform.position, mainText.transform.position - Vector3.back * 0.1f, movementSens *  Time.deltaTime);
            tempTx.GetComponent<Text>().color += new Color(0,0,0, Mathf.Lerp(tempTx.GetComponent<Text>().color.a, 255, alphaSens * Time.deltaTime));
            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }
        Destroy(tempTx);
    }

    /*
    void AnimateText(GameObject tempTx, int prc)
    {
        timer = 1f;
        if (tempTx.transform.position.y < mainText.transform.position.y - 0.01f)
        {
            tempTx.GetComponent<Text>().text = "$" + prc.ToString();
            tempTx.transform.position = Vector3.Lerp(tempTx.transform.position, mainText.transform.position - Vector3.back * 0.1f, movementSens * Time.deltaTime);
            tempTx.GetComponent<Text>().color += new Color(0, 0, 0, Mathf.Lerp(tempTx.GetComponent<Text>().color.a, 255, alphaSens * Time.deltaTime));
        }
        else
        {
            Destroy(tempTx);
        }
    }*/
}
