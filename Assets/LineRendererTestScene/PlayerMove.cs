using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{

    public float speed = 0.1f;
    public float distanceToEnd;

    private float h;
    private float v;
    private RaycastHit ray;
    private GameObject activeLine;

    void Update()
    {
        activeLine = GameObject.FindGameObjectWithTag("ActivePath");
        //distanceToEnd = Vector3.Distance(transform.localPosition, activeLine.GetComponent<LineRenderer>().GetPosition(1));
        //Debug.Log(distanceToEnd);

        v = Input.GetAxis("Vertical");
        h = Input.GetAxis("Horizontal");

       if (Physics.Raycast(transform.position + new Vector3(0, v / Mathf.Abs(v) * 1.5f, 0) * speed, transform.forward, out ray))
        {
            if (v > 0 && ray.collider.gameObject.tag == "ActivePath") //if moving it won't take it off the line
            {
                transform.position += new Vector3(0, v, 0) * speed;
            }
        }

        
    }
}
