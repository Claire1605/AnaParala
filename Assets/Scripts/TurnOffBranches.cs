using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOffBranches : MonoBehaviour {

	void Start () {
        foreach (var item in GameObject.FindGameObjectsWithTag("Branch"))
        {
            if (item.GetComponent<Renderer>())
            {
                Color c = item.GetComponent<Renderer>().material.GetColor("_Color");
                item.GetComponent<Renderer>().material.SetColor("_Color", new Color(c.r, c.g, c.b, 0));
            }
        }   
	}
}
