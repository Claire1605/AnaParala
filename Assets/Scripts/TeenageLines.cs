using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeenageLines : MonoBehaviour {

    private Score score;
	void Start () {
        score = GameObject.FindGameObjectWithTag("Player").GetComponent<Score>();
	}
	
	void Update () {
		
	}
}
