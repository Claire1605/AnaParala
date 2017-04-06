using UnityEngine;
using System.Collections;

public class DrawLine : MonoBehaviour {

    public LineRenderer line;
    public Vector3 newVector;
    public Vector3 startVector = new Vector3(0,0,0);
    public GameObject pivot;
    public GameObject lineObject;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        line = GetComponent<LineRenderer>();
        line.SetPosition(1, startVector);
        line.SetPosition(0, newVector);
        pivot.transform.position = newVector;
        if (newVector.magnitude >=2)
        {
            Instantiate(lineObject, newVector, this.transform.rotation);   
        }
	}
}
