using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{

    Rigidbody rb;
    public float speed; 

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        
        Vector3 pos = rb.position;
        //rb.position -= transform.right * -1 * speed * Time.deltaTime;
        rb.position -= transform.forward * speed * Time.deltaTime;
        rb.MovePosition(pos);

        


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
