using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StairSc : MonoBehaviour
{
    public Material mat1;
    public Material mat2;
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < transform.childCount; i++) 
        {

            transform.GetChild(i).GetChild(0).GetChild(0).GetComponent<Text>().text = (transform.GetChild(i).localPosition.y * 2).ToString();
            transform.GetChild(i).GetChild(0).GetChild(1).GetComponent<Text>().text = (Convert.ToInt32(transform.GetChild(i).localPosition.y * 4 / 50 ) + 2).ToString();

            if (i%2 == 0)
            {
            transform.GetChild(i).GetComponent<Renderer>().material = mat1;
            }
            else
            {
            transform.GetChild(i).GetComponent<Renderer>().material = mat2;
            }
        }
    }
}
