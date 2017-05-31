using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

    public Score score;
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
    [HideInInspector]
    public List<MeshRenderer> envyLines = new List<MeshRenderer>();
    [HideInInspector]
    public bool showIntersection;
    [HideInInspector]
    public bool rendDone;
    private bool stage2reached = false;
    private bool movingInProgress = false;

    void Start()
    {
        line = spline.GetComponentInChildren<MeshRenderer>();
    }
    void Update () {
       // Debug.Log(line.gameObject.GetComponentInParent<Intersection>().gameObject.name.ToString());
        v = Input.GetAxis("Vertical");
        if (v > 0)
            progress += v / speed * speedRatio;
        if (progress > 1f)
            progress = 1f;
        
        if (progress < 0f)
            progress = 0f;

        if (progress > 0.6f)
            showIntersection = true;

        Vector3 position = spline.GetPoint(progress);
        position.z -= 0.2f;
        transform.localPosition = position;
        if (!movingInProgress)
            line.material.SetFloat("_AlphaHeight", progress);
        if (envyLines.Count>0 && progress <0.8f)
        {
            foreach (var item in envyLines)
            {
                if (item.material.GetFloat("_AlphaHeight") <= 0.7f)
                    item.material.SetFloat("_AlphaHeight", progress * 2);
            }
        }
    }

    public IEnumerator MoveToLineEnd(float currentProgress, GameObject newLine)
    {
        movingInProgress = true;
        line = newLine.GetComponentInChildren<MeshRenderer>();
        score.UpdateScore(line);
        float i = 0;
        float rate = 2;
        bool done = false;
        while (i < 1 && !done)
        {
            if (progress >= 1)
            {
                done = true;
            }
               
            i += Time.deltaTime * rate;
            if (!done)
                transform.localPosition = Vector3.Lerp(spline.GetPoint(currentProgress), spline.GetPoint(1), i);
            yield return new WaitForEndOfFrame();
        }
        spline = newLine.GetComponent<BezierSpline>();
        progress = 0;
        speed = 10 * Vector3.Distance(spline.GetPoint(1), spline.GetPoint(0));
        showIntersection = false;
        rendDone = false;
        if (line.GetComponentInParent<Intersection>().gameObject.tag == "Stage2" && !stage2reached)
        {
            stage2reached = true;
            score.Visible();
        }
        movingInProgress = false;
    }
}
