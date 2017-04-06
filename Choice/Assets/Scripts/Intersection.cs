using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intersection : MonoBehaviour {

    private SegmentSetup segmentSetup;
    private List<GameObject> lines = new List<GameObject>();
    private GameObject player;
    private int activeLine = 0;
    private bool chosen = true;

    void Start () {
        player = GameObject.Find("Player");
        lines.Clear();
        segmentSetup = GetComponentInParent<SegmentSetup>();
        if (segmentSetup.Children.Count >0)
        {
            foreach (var item in segmentSetup.Children)
            {
                lines.Add(item.GetComponent<BezierSpline>().gameObject);
            }
        }
        
	}
	
    void OnTriggerEnter(Collider col)
    {
        chosen = false;
        StartCoroutine(ColourLerp(activeLine, Color.grey, Color.red));
    }

	void Update () {
        if (!chosen)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            {
                MoveToNewLine();
                StartCoroutine(ColourLerp(activeLine, Color.red, Color.black));
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A) && !chosen)
            {
                StartCoroutine(ColourLerp(activeLine, Color.red, Color.grey));
                ChooseLine(-1);
                StartCoroutine(ColourLerp(activeLine, Color.grey, Color.red));
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D) && !chosen)
            {
                StartCoroutine(ColourLerp(activeLine, Color.red, Color.grey));
                ChooseLine(1);
                StartCoroutine(ColourLerp(activeLine, Color.grey, Color.red));
            }
        }
	}

    void ChooseLine(int move)
    {
        activeLine += move;
        if (activeLine > lines.Count - 1)
        {
            activeLine = 0;
        }
        if (activeLine < 0)
        {
            activeLine = lines.Count - 1;
        }           
    }

    void MoveToNewLine()
    {
        chosen = true;
        player.GetComponent<Movement>().spline = lines[activeLine].GetComponent<BezierSpline>();
        player.GetComponent<Movement>().progress = 0;
        player.GetComponent<Movement>().speed = 10 * Vector3.Distance(player.GetComponent<Movement>().spline.GetPoint(1), player.GetComponent<Movement>().spline.GetPoint(0));
    }

    IEnumerator ColourLerp(int lineIndex, Color start, Color end)
    {
        float i = 0;
        float rate = 2;

        while (i<1)
        {
            i += Time.deltaTime * rate;
            lines[lineIndex].GetComponentInChildren<MeshRenderer>().material.color = Color.Lerp(start, end, i);

            yield return new WaitForEndOfFrame();
        }
    }

    void OnDrawGizmos()
    {
        segmentSetup = GetComponent<SegmentSetup>();
        if (segmentSetup.Children.Count>0)
        {
            foreach (var item in segmentSetup.Children)
            {
                Gizmos.DrawLine(transform.position, item.transform.position);
            }
        }
    }
}
