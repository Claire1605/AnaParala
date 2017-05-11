using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intersection : MonoBehaviour {

   // public bool flashRed = false;
    public bool envy = false;
    [Range(0, 0.8f)]
    public float envyLineWidth = 0.8f;
    [Range(0, 1)]
    public float envyLineSpeed = 1;
    public AnimationCurve envyCurve;
    public bool confidence = false;
    [Range(0, 0.8f)]
    public float confLineWidth = 0.8f;
    [Range(0, 1)]
    public float confLineSpeed = 1;
    public AnimationCurve confCurve;
    public List<GameObject> lines = new List<GameObject>();
    public List<GameObject> envyLines = new List<GameObject>();

    // public bool freeChoice = true;
    private SegmentSetup segmentSetup;
    private Movement playerMovement;
    private int activeLine = 0;
    private bool chosen = true;
    private bool canMoveUp = false;
    private bool firstChoice = true;
    private MeshRenderer lineMesh;

    void Start () {
        playerMovement = GameObject.Find("Player").GetComponent<Movement>();
        lines.Clear();
        envyLines.Clear();
        segmentSetup = GetComponentInParent<SegmentSetup>();
        if (segmentSetup.Children.Count >0)
        {
            foreach (var item in segmentSetup.Children)
            {
                if (item)
                {
                    lines.Add(item.GetComponent<BezierSpline>().gameObject);
                    envyLines.Add(item.GetComponent<BezierSpline>().gameObject);
                }    
            }
        }
	}
	
    void OnTriggerEnter(Collider col)
    {
        chosen = false;
        if (segmentSetup.stageConnector)
        {
            segmentSetup.nextStage.gameObject.SetActive(true);
            foreach (var item in segmentSetup.nextStage.GetComponent<StageConnect>().stageChildren)
            {
                Color c = item.GetComponent<Renderer>().material.GetColor("_Color");
                item.GetComponent<Renderer>().material.SetColor("_Color", new Color(c.r, c.g, c.b, 1));
                StartCoroutine(LineFadeIn(item.GetComponentInChildren<MeshRenderer>(), item.GetComponentInChildren<MeshRenderer>().material.GetColor("_Color")));
            }
        }  
        //if (flashRed)
        //{
        //    for (int i = 0; i < lines.Count; i++)
        //    {
        //        StartCoroutine(ColourLerp(i, Color.grey, Color.red));
        //        StartCoroutine(ColourLerp(i, Color.red, Color.grey));
        //    }
        //}
    }

	void Update () {
        if (!chosen)
        {
            lineMesh = lines[activeLine].GetComponentInChildren<MeshRenderer>();
            //if (!freeChoice)
            //{
            //    ChooseLine(0);
            //    MoveToNewLine();
            //    StartCoroutine(ColourLerp(activeLine, lines[activeLine].GetComponentInChildren<MeshRenderer>().material.color, Color.black));
            //}
            if (canMoveUp && (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)))
            {
                MoveToNewLine();
                StartCoroutine(ColourLerp(activeLine, lineMesh.material.color, Color.black));
                canMoveUp = false;
            }
            else if (segmentSetup.Children.Count == 1 && (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)))
            {
                canMoveUp = true;
                StartCoroutine(ColourLerp(activeLine, lineMesh.material.color, Color.grey));
                ChooseLine(1);
                StartCoroutine(ColourLerp(activeLine, Color.grey, Color.red));
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A) && !chosen)
            {
                canMoveUp = true;
                StartCoroutine(ColourLerp(activeLine, lineMesh.material.color, Color.grey));
                ChooseLine(-1);
                StartCoroutine(ColourLerp(activeLine, Color.grey, Color.red));

            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D) && !chosen)
            {
                canMoveUp = true;
                StartCoroutine(ColourLerp(activeLine, lineMesh.material.color, Color.grey));
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
                activeLine = 0;
            else if (activeLine < 0)
                activeLine = lines.Count - 1;
        } 
    }

    void MoveToNewLine()
    {
        chosen = true;
        if (envy)
        {
            envyLines.Remove(lines[activeLine]);
            foreach (var item in envyLines)
            {
                DrawMesh drawMesh = item.GetComponentInChildren<DrawMesh>();
                MeshRenderer rend = item.GetComponentInChildren<MeshRenderer>();
                drawMesh.StartCoroutine(drawMesh.growLine(rend, envyLineSpeed, envyCurve));
                drawMesh.StartCoroutine(drawMesh.changeWidth(rend, drawMesh.width, envyLineWidth, envyCurve));
            }
        }
        if (confidence)
        {
            DrawMesh drawMesh = lines[activeLine].GetComponentInChildren<DrawMesh>();
            MeshRenderer rend = lines[activeLine].GetComponentInChildren<MeshRenderer>();
            drawMesh.StartCoroutine(drawMesh.growLine(rend, confLineSpeed, confCurve));
            drawMesh.StartCoroutine(drawMesh.changeWidth(rend, drawMesh.width, confLineWidth, confCurve));
        }
        playerMovement.StartCoroutine(playerMovement.MoveToLineEnd(playerMovement.progress, lines[activeLine]));
        if (lines[activeLine].GetComponent<SegmentSetup>().stageConnector)
        {
            lines[activeLine].GetComponent<SegmentSetup>().nextStage.GetComponent<StageConnect>().Connect(lines[activeLine]);
        }
    }

    IEnumerator ColourLerp(int lineIndex, Color start, Color end)
    {
        float i = 0;
        float rate = 2;
        MeshRenderer rend = lines[lineIndex].GetComponentInChildren<MeshRenderer>();

        while (i<1)
        {
            i += Time.deltaTime * rate;
            rend.material.color = Color.Lerp(start, end, i);
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator LineFadeIn(MeshRenderer line, Color colour)
    {
        float i = 0;
        float rate = 2;

        while (i < 1)
        {
            i += Time.deltaTime * rate;
            //line.material.color = Color.Lerp(new Color(colour.r, colour.g, colour.b, 0), new Color(colour.r, colour.g, colour.b, 1), i);
            line.material.SetFloat("_AlphaHeight", Mathf.Lerp(-0.25f, 0, i));
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
