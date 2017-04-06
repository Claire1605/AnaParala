using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineDynamic : MonoBehaviour {

    private DrawMesh drawMesh;
    public float Width = 1f;
	void Start () {
        drawMesh = GetComponent<DrawMesh>();
	}
	
	void Update () {
       if (Input.GetKeyDown(KeyCode.Space))
        {
            drawMesh.StartCoroutine(drawMesh.changeWidth(0.2f, 0.8f));
        }
       if (Input.GetKeyDown(KeyCode.Return))
        {
            drawMesh.StartCoroutine(drawMesh.changeWidth(0.8f, 0.2f));
        }
	}
}
