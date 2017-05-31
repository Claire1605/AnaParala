using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageConnect : MonoBehaviour {

    public List<GameObject> stageChildren = new List<GameObject>();
    private Score score;
    private ColourLibrary colourLibrary;

    void Start()
    {
        colourLibrary = GameObject.FindGameObjectWithTag("Player").GetComponent<ColourLibrary>();
        score = GameObject.FindGameObjectWithTag("Player").GetComponent<Score>();
    }

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
        connector.GetComponent<Intersection>().childLines.Clear();
        foreach (var item in connector.GetComponent<SegmentSetup>().Children)
        {
            if (item)
            {
                connector.GetComponent<Intersection>().childLines.Add(item.GetComponent<BezierSpline>().gameObject);
                connector.GetComponent<Intersection>().envyLines.Add(item.GetComponent<BezierSpline>().gameObject);
            }
               
        }
        transform.position = connector.transform.position;

        if (connector.tag == "Stage2" && connector.GetComponent<SegmentSetup>().nextStage.gameObject.name == "BRANCH 2" && !score.randomColour)
            score.randomColour = true;

        //SECOND STAGE 2 - main colour + one other
        if (connector.tag == "Stage2" && connector.GetComponent<SegmentSetup>().nextStage.gameObject.name == "BRANCH 2") 
        {
            int r = Random.Range(0, 2);

            if (score.redCount == 1)
            {
                stageChildren[0].GetComponent<Intersection>().initialColour = Intersection.initialLineColourENUM.LightRed;
                stageChildren[0].GetComponent<Intersection>().chosenColour = Intersection.chosenLineColourENUM.Red;
                if (r == 0)
                {
                    stageChildren[1].GetComponent<Intersection>().initialColour = Intersection.initialLineColourENUM.LightBlue;
                    stageChildren[1].GetComponent<Intersection>().chosenColour = Intersection.chosenLineColourENUM.Blue;
                }
                else
                {
                    stageChildren[1].GetComponent<Intersection>().initialColour = Intersection.initialLineColourENUM.LightGreen;
                    stageChildren[1].GetComponent<Intersection>().chosenColour = Intersection.chosenLineColourENUM.Green;
                }
            }
            else if (score.blueCount == 1)
            {
                stageChildren[0].GetComponent<Intersection>().initialColour = Intersection.initialLineColourENUM.LightBlue;
                stageChildren[0].GetComponent<Intersection>().chosenColour = Intersection.chosenLineColourENUM.Blue;
                if (r == 0)
                {
                    stageChildren[1].GetComponent<Intersection>().initialColour = Intersection.initialLineColourENUM.LightRed;
                    stageChildren[1].GetComponent<Intersection>().chosenColour = Intersection.chosenLineColourENUM.Red;
                }
                else
                {
                    stageChildren[1].GetComponent<Intersection>().initialColour = Intersection.initialLineColourENUM.LightGreen;
                    stageChildren[1].GetComponent<Intersection>().chosenColour = Intersection.chosenLineColourENUM.Green;
                }
            }
            else if (score.greenCount == 1)
            {
                stageChildren[0].GetComponent<Intersection>().initialColour = Intersection.initialLineColourENUM.LightGreen;
                stageChildren[0].GetComponent<Intersection>().chosenColour = Intersection.chosenLineColourENUM.Green;
                if (r == 0)
                {
                    stageChildren[1].GetComponent<Intersection>().initialColour = Intersection.initialLineColourENUM.LightRed;
                    stageChildren[1].GetComponent<Intersection>().chosenColour = Intersection.chosenLineColourENUM.Red;
                }
                else
                {
                    stageChildren[1].GetComponent<Intersection>().initialColour = Intersection.initialLineColourENUM.LightBlue;
                    stageChildren[1].GetComponent<Intersection>().chosenColour = Intersection.chosenLineColourENUM.Blue;
                }
            }
            stageChildren[0].GetComponent<Intersection>().initialLineColour = stageChildren[0].GetComponent<Intersection>().ColourAssign(stageChildren[0].GetComponent<Intersection>().initialColour);
            stageChildren[0].GetComponent<Intersection>().chosenLineColour = stageChildren[0].GetComponent<Intersection>().ColourAssign(stageChildren[0].GetComponent<Intersection>().chosenColour);
            stageChildren[1].GetComponent<Intersection>().initialLineColour = stageChildren[1].GetComponent<Intersection>().ColourAssign(stageChildren[1].GetComponent<Intersection>().initialColour);
            stageChildren[1].GetComponent<Intersection>().chosenLineColour = stageChildren[1].GetComponent<Intersection>().ColourAssign(stageChildren[1].GetComponent<Intersection>().chosenColour);

            stageChildren[0].GetComponentInChildren<MeshRenderer>().material.SetColor("_Color", stageChildren[0].GetComponent<Intersection>().initialLineColour);
            stageChildren[1].GetComponentInChildren<MeshRenderer>().material.SetColor("_Color", stageChildren[1].GetComponent<Intersection>().initialLineColour);
        }
        //MAIN RANDOM COLOUR PICKER
        else if (connector.tag == "Stage2" && score.randomColour)
        {
            foreach (var item in stageChildren)
            {
                if (item)
                {
                    Intersection intersection = item.GetComponent<Intersection>();
                    float r = Random.Range(0.00f, 1.00f);

                    float redRange = score.redCount + 0.5f; //+ 0.5f to make sure it has a chance of being chosen
                    float blueRange = score.blueCount + 0.5f;
                    float greenRange = score.greenCount + 0.5f;

                    float total = redRange + blueRange + greenRange;
                    redRange /= total; //these three summed = 1
                    blueRange /= total;
                    greenRange /= total;

                    if (r < redRange)
                    {
                        intersection.initialColour = Intersection.initialLineColourENUM.LightRed;
                        intersection.chosenColour = Intersection.chosenLineColourENUM.Red;
                    }
                    else if (r >= redRange && r < (redRange + blueRange))
                    {
                        intersection.initialColour = Intersection.initialLineColourENUM.LightBlue;
                        intersection.chosenColour = Intersection.chosenLineColourENUM.Blue;
                    }
                    else if (r >= redRange + blueRange)
                    {
                        intersection.initialColour = Intersection.initialLineColourENUM.LightGreen;
                        intersection.chosenColour = Intersection.chosenLineColourENUM.Green;
                    }

                    intersection.initialLineColour = intersection.ColourAssign(intersection.initialColour);
                    intersection.chosenLineColour = intersection.ColourAssign(intersection.chosenColour);
                    item.GetComponentInChildren<MeshRenderer>().material.SetColor("_Color", intersection.initialLineColour);
                }
            }
        }
    }
}
