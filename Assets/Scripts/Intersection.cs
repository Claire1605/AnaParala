﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intersection : MonoBehaviour {

    private SegmentSetup segmentSetup;
    public List<GameObject> lines = new List<GameObject>();
   // public bool freeChoice = true;
    public bool flashRed = false;
    private GameObject player;
    private int activeLine = 0;
    private bool chosen = true;
    private bool canMoveUp = false;
    private bool firstChoice = true;

    void Start () {
        player = GameObject.Find("Player");
        lines.Clear();
        segmentSetup = GetComponentInParent<SegmentSetup>();
        if (segmentSetup.Children.Count >0)
        {
            foreach (var item in segmentSetup.Children)
            {
                if (item)
                    lines.Add(item.GetComponent<BezierSpline>().gameObject);
            }
        }
	}
	
    void OnTriggerEnter(Collider col)
    {
        chosen = false;
        if (GetComponent<SegmentSetup>().stageConnector)
            GetComponent<SegmentSetup>().nextStage.gameObject.SetActive(true);
        if (flashRed)
        {
            for (int i = 0; i < lines.Count; i++)
            {
                StartCoroutine(ColourLerp(i, Color.grey, Color.red));
                StartCoroutine(ColourLerp(i, Color.red, Color.grey));
            }
        }
    }

	void Update () {
        if (!chosen)
        {
            //if (!freeChoice)
            //{
            //    ChooseLine(0);
            //    MoveToNewLine();
            //    StartCoroutine(ColourLerp(activeLine, lines[activeLine].GetComponentInChildren<MeshRenderer>().material.color, Color.black));
            //}
            if (canMoveUp && (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)))
            {
                MoveToNewLine();
                StartCoroutine(ColourLerp(activeLine, lines[activeLine].GetComponentInChildren<MeshRenderer>().material.color, Color.black));
                canMoveUp = false;
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A) && !chosen)
            {
                canMoveUp = true;
                StartCoroutine(ColourLerp(activeLine, lines[activeLine].GetComponentInChildren<MeshRenderer>().material.color, Color.grey));
                ChooseLine(-1);
                StartCoroutine(ColourLerp(activeLine, Color.grey, Color.red));

            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D) && !chosen)
            {
                canMoveUp = true;
                StartCoroutine(ColourLerp(activeLine, lines[activeLine].GetComponentInChildren<MeshRenderer>().material.color, Color.grey));
                ChooseLine(1);
                StartCoroutine(ColourLerp(activeLine, Color.grey, Color.red));
            }
        }
    }

    void ChooseLine(int move)
    {
        if (firstChoice)
        {
            float c = lines.Count;
            c = (c - 1) / 2;
            activeLine = (move > 0) ? Mathf.CeilToInt(c) : Mathf.FloorToInt(c);
            firstChoice = false;
        }
        else
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
    }

    void MoveToNewLine()
    {
        chosen = true;
        player.GetComponent<Movement>().StartCoroutine(player.GetComponent<Movement>().MoveToLineEnd(player.GetComponent<Movement>().progress, lines[activeLine]));
        if (lines[activeLine].GetComponent<SegmentSetup>().stageConnector)
        {
            lines[activeLine].GetComponent<SegmentSetup>().nextStage.GetComponent<StageConnect>().Connect(lines[activeLine]);
        }
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

    IEnumerator LineFadeIn(int lineIndex)
    {
        float i = 0;
        float rate = 2;

        while (i < 1)
        {
            i += Time.deltaTime * rate;
            lines[lineIndex].GetComponent<Renderer>().material.color = Color.Lerp(new Color(0.5f, 0.5f, 0.5f, 0), new Color(0.5f, 0.5f, 0.5f, 1), i);
            lines[lineIndex].GetComponentInChildren<MeshRenderer>().material.color = Color.Lerp(new Color(0.5f, 0.5f, 0.5f, 0), new Color(0.5f, 0.5f, 0.5f, 1), i);
            yield return new WaitForEndOfFrame();
        }
    }

    void OnDrawGizmos()
    {
        segmentSetup = GetComponent<SegmentSetup>();
        if (segmentSetup.Children.Count>0)
        {
            foreach (GameObject item in segmentSetup.Children)
            {
                if (item)
                    Gizmos.DrawLine(transform.position, item.transform.position);
            }
        }
    }
}
