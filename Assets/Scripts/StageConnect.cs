using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageConnect : MonoBehaviour {

    public List<GameObject> stageChildren = new List<GameObject>();

    public void Connect(GameObject connector)
    {
        connector.GetComponent<SegmentSetup>().Children.Clear();
        foreach (var item in stageChildren)
        {
            if (item)
            {
                connector.GetComponent<SegmentSetup>().Children.Add(item);
                item.GetComponent<SegmentSetup>().Parent = connector;
            }
        }
        foreach (var item in connector.GetComponent<SegmentSetup>().Children)
        {
            if (item)
                connector.GetComponent<Intersection>().lines.Add(item.GetComponent<BezierSpline>().gameObject);
        }
        transform.position = connector.transform.position;
    }
}
