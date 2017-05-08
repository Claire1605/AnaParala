using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

    public BezierSpline spline;
    [HideInInspector]
    public MeshRenderer line;
    [HideInInspector]
    public float progress;
    [HideInInspector]
    private float v;
    [HideInInspector]
    public float speed;
    public float speedRatio;
   // public bool freeMovement = false;

    void Start()
    {
        line = spline.GetComponentInChildren<MeshRenderer>();
    }
    void Update () {
        //if (transform.position.y > 90)
        //    freeMovement = true;

        //if (freeMovement)
        //    v = Input.GetAxis("Vertical");
        //else
        //    v = 0.7f;
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
        line.material.SetFloat("_AlphaHeight", progress);
    }

    public IEnumerator MoveToLineEnd(float currentProgress, GameObject newLine)
    {
        float i = 0;
        float rate = 2;
        bool done = false;
        while (i < 1 && !done)
        {
            if (progress >= 1)
                done = true;
            i += Time.deltaTime * rate;
            if (!done)
                transform.localPosition = Vector3.Lerp(spline.GetPoint(currentProgress), spline.GetPoint(1), i);
            yield return new WaitForEndOfFrame();
        }
        spline = newLine.GetComponent<BezierSpline>();
        line = newLine.GetComponentInChildren<MeshRenderer>();
        progress = 0;
        speed = 10 * Vector3.Distance(spline.GetPoint(1), spline.GetPoint(0));
    }
}
