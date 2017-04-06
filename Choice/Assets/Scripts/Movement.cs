using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

    public BezierSpline spline;
    [HideInInspector]
    public float progress;
    [HideInInspector]
    private float v;
    [HideInInspector]
    public float speed;
    public float speedRatio;

    void Update () {
        v = Input.GetAxis("Vertical");

        if (v > 0)
            progress += v / speed * speedRatio;
        if (progress > 1f)
            progress = 1f;
        
        if (progress < 0f)
            progress = 0f;

        Vector3 position = spline.GetPoint(progress);
        position.z -= 0.2f;
        transform.localPosition = position;

    }
}
