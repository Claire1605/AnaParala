using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SegmentSetup : MonoBehaviour {

    [SerializeField]
    private GameObject parent;
    public GameObject Parent { get { return parent; } }
    [SerializeField]
    public List<GameObject> Children = new List<GameObject>();
    public bool stageConnector;
    public GameObject nextStage;
}
