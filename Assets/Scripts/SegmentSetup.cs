using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SegmentSetup : MonoBehaviour {

    public GameObject Parent;
    public List<GameObject> Children = new List<GameObject>();
    public bool stageConnector;
    public GameObject nextStage;
}
