using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intersection : MonoBehaviour {

    private ColourLibrary colourLibrary;
    public enum initialLineColourENUM
    {
        LightGrey, LightRed, LightBlue, LightGreen
    };
    public enum chosenLineColourENUM
    {
        Grey, Red, Blue, Green
    };
    public initialLineColourENUM initialColour;
    public chosenLineColourENUM chosenColour;

    [HideInInspector]
    public Color initialLineColour = Color.grey;
    [HideInInspector]
    public Color chosenLineColour = Color.grey;
    public bool envy = false;
    [Range(0, 0.8f)]
    public float envyLineWidth = 0.8f;
    [Range(0, 1)]
    public float envyLineSpeed = 1;
    public float envyEndZ = 0;
    public Color envyColour = Color.grey;
    public AnimationCurve envyCurve;
    public bool confidence = false;
    [Range(0, 0.8f)]
    public float confLineWidth = 0.8f;
    [Range(0, 1)]
    public float confLineSpeed = 1;
    public AnimationCurve confCurve;
    public List<GameObject> childLines = new List<GameObject>();
    public List<GameObject> envyLines = new List<GameObject>();

    private SegmentSetup segmentSetup;
    private Movement playerMovement;
    private int activeLine = 0;
    private bool chosen = true;
    private bool canMoveUp = false;
    private bool firstChoice = true;
    private MeshRenderer lineMesh;
    private int lineInt;
    private Color activeColour;

    //Working out what's going on...
    public MeshRenderer thisLine;
    private SpriteRenderer thisSprite;

    void Awake()
    {
        colourLibrary = GameObject.FindGameObjectWithTag("Player").GetComponent<ColourLibrary>();
    }

    void Start () {

        thisLine = GetComponentInChildren<MeshRenderer>();
        initialLineColour = ColourAssign(initialColour);
        chosenLineColour = ColourAssign(chosenColour);
        thisLine.material.SetColor("_Color", initialLineColour); //Set initial line colour
        playerMovement = GameObject.Find("Player").GetComponent<Movement>();
        if (GetComponent<SpriteRenderer>())
        {
            Color c = GetComponent<SpriteRenderer>().color;
            GetComponent<SpriteRenderer>().color = new Color(c.r, c.g, c.b, 0);
        }
        childLines.Clear();
        envyLines.Clear();
        segmentSetup = GetComponentInParent<SegmentSetup>();
        if (segmentSetup.Children.Count >0)
        {
            foreach (var item in segmentSetup.Children)
            {
                if (item)
                {
                    childLines.Add(item.GetComponent<BezierSpline>().gameObject);
                    envyLines.Add(item.GetComponent<BezierSpline>().gameObject);
                }    
            }
        }
	}
	
    private Color ColourAssign(initialLineColourENUM iColour)
    {
        switch(iColour)
        {
            case initialLineColourENUM.LightGrey:
                {
                    return colourLibrary.lightGrey;
                }
            case initialLineColourENUM.LightRed:
                {
                    return colourLibrary.lightRed;
                }
            case initialLineColourENUM.LightBlue:
                {
                    return colourLibrary.lightBlue;
                }
            case initialLineColourENUM.LightGreen:
                {
                    return colourLibrary.lightGreen;
                }
            default:
                {
                    return colourLibrary.lightGrey;
                }
        }
    }

    private Color ColourAssign(chosenLineColourENUM iColour)
    {
        switch (iColour)
        {
            case chosenLineColourENUM.Grey:
                {
                    return colourLibrary.grey;
                }
            case chosenLineColourENUM.Red:
                {
                    return colourLibrary.red;
                }
            case chosenLineColourENUM.Blue:
                {
                    return colourLibrary.blue;
                }
            case chosenLineColourENUM.Green:
                {
                    return colourLibrary.green;
                }
            default:
                {
                    return colourLibrary.grey;
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
                Renderer rend = item.GetComponent<Renderer>();
                MeshRenderer meshRend = item.GetComponentInChildren<MeshRenderer>();
                Color c = rend.material.GetColor("_Color");
                rend.material.SetColor("_Color", new Color(c.r, c.g, c.b, 1));
                StartCoroutine(LineFadeIn(meshRend, meshRend.material.GetColor("_Color")));
            }
        }  
    }

	void Update () {
        if (playerMovement.showIntersection && !playerMovement.rendDone) //Fade in the intersection sprite
        {
            thisSprite = playerMovement.line.GetComponentInParent<SpriteRenderer>();
            Color c = thisSprite.color;
            c.a = 0;
            Color d = new Color(c.r, c.g, c.b, 1);
            StartCoroutine(ColourLerpSpriteRend(thisSprite, c, d));
        }
        if (!chosen) //Choose the new line
        {
            playerMovement.envyLines.Clear();
            if (canMoveUp && (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))) //Selecting complete, move to new line
            {
                MoveToNewLine();
                Color c = childLines[activeLine].GetComponentInParent<Intersection>().chosenLineColour;
                StartCoroutine(ColourLerp(activeLine, thisLine.material.color, c));
                canMoveUp = false;
            }
            //Selecting in progress
            else if (segmentSetup.Children.Count % 2 == 1 && (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)))
            {
                lineInt = Mathf.FloorToInt(segmentSetup.Children.Count / 2);
                SelectLine(lineInt);
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A) && !chosen)
            {
                lineInt = -1;
                SelectLine(lineInt);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D) && !chosen)
            {
                lineInt = 1;
                SelectLine(lineInt);
            }
        }
    }

    void SelectLine(int move)
    {
        canMoveUp = true;

        //Fade old line from active to inactive
        lineMesh = childLines[activeLine].GetComponentInChildren<MeshRenderer>();
        activeColour = childLines[activeLine].GetComponentInParent<Intersection>().initialLineColour;
        StartCoroutine(ColourLerp(activeLine, lineMesh.material.color, activeColour));

        //Select new line
        if (firstChoice)
        {
            float c = childLines.Count;
            c = (c - 1) / 2;
            activeLine = (move > 0) ? Mathf.CeilToInt(c) : Mathf.FloorToInt(c);
            firstChoice = false;
        }
        else
        {
            activeLine += move;
            if (activeLine > childLines.Count - 1)
                activeLine = 0;
            else if (activeLine < 0)
                activeLine = childLines.Count - 1;
        }

        //Fade new line from inactive to active
        lineMesh = childLines[activeLine].GetComponentInChildren<MeshRenderer>();
        activeColour = childLines[activeLine].GetComponentInParent<Intersection>().chosenLineColour;
        StartCoroutine(ColourLerp(activeLine, lineMesh.material.color, activeColour));
    }

    void MoveToNewLine()
    {
        chosen = true;
        playerMovement.StartCoroutine(playerMovement.MoveToLineEnd(playerMovement.progress, childLines[activeLine]));
        if (childLines[activeLine].GetComponent<SegmentSetup>().stageConnector)
        {
            childLines[activeLine].GetComponent<SegmentSetup>().nextStage.GetComponent<StageConnect>().Connect(childLines[activeLine]);
        }
        if (envy)
        {
            envyLines.Remove(childLines[activeLine]);
            foreach (var item in envyLines)
            {
                DrawMesh drawMesh = item.GetComponentInChildren<DrawMesh>();
                MeshRenderer rend = item.GetComponentInChildren<MeshRenderer>();
                SpriteRenderer sprite = item.GetComponent<SpriteRenderer>();
                StartCoroutine(ColourLerpRend(rend, rend.material.GetColor("_Color"), rend.GetComponentInParent<Intersection>().envyColour));
                rend.material.SetFloat("_AlphaHeight", 0);
                playerMovement.envyLines.Add(rend);
                StartCoroutine(ColourLerpRend(sprite, sprite.color, new Color(0,0,0,0)));
               // drawMesh.StartCoroutine(drawMesh.growLine(rend, envyLineSpeed, 0.7f, envyCurve));
                drawMesh.StartCoroutine(drawMesh.changeWidth(rend, drawMesh.width, envyLineWidth, envyCurve));
                StartCoroutine(MoveLineZ(item.gameObject.transform, envyEndZ));
            }
        }
        if (confidence)
        {
            DrawMesh drawMesh = childLines[activeLine].GetComponentInChildren<DrawMesh>();
            MeshRenderer rend = childLines[activeLine].GetComponentInChildren<MeshRenderer>();
            drawMesh.StartCoroutine(drawMesh.growLine(rend, confLineSpeed, 1, confCurve));
            drawMesh.StartCoroutine(drawMesh.changeWidth(rend, drawMesh.width, confLineWidth, confCurve));
        }
    }

    IEnumerator ColourLerp(int lineIndex, Color start, Color end)
    {
        float i = 0;
        float rate = 2;
        MeshRenderer rend = childLines[lineIndex].GetComponentInChildren<MeshRenderer>();

        while (i<1)
        {
            i += Time.deltaTime * rate;
            rend.material.color = Color.Lerp(start, end, i);
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator ColourLerpRend(Renderer rend, Color start, Color end)
    {
        float i = 0;
        float rate = 2f;

        while (i < 1)
        {
            i += Time.deltaTime * rate; 
            rend.material.color = Color.Lerp(start, end, i);
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator ColourLerpSpriteRend(SpriteRenderer rend, Color start, Color end)
    {
        playerMovement.rendDone = true;
        float i = 0;
        float rate = 1f;

        while (i < 1)
        {
            i += Time.deltaTime * rate;
            rend.color = Color.Lerp(start, end, i);
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator LineFadeIn(Renderer line, Color colour)
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

    IEnumerator MoveLineZ(Transform t, float Zend)
    {
        float i = 0;
        float rate = (0.5f/Mathf.Abs(Zend));

        while (i < 1)
        {
            i += Time.deltaTime * rate;
            t.position = Vector3.Lerp(new Vector3(t.position.x, t.position.y, t.position.z), new Vector3(t.position.x, t.position.y, Zend), i);
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
