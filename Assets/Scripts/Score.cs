using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour {

    public ColourLibrary colourLibrary;
    public Text greenScore;
    public Text blueScore;
    public Text redScore;
    public Image green;
    public Image blue;
    public Image red;

    public int greenCount = 0;
    public int blueCount = 0;
    public int redCount = 0;

    public bool randomColour = false;

    void Start () {
        greenScore.text = greenCount.ToString();
        blueScore.text = blueCount.ToString();
        redScore.text = redCount.ToString();
    }

    public void Visible()
    {
        StartCoroutine(textFade(greenScore));
        StartCoroutine(textFade(blueScore));
        StartCoroutine(textFade(redScore));
        StartCoroutine(imageFade(green));
        StartCoroutine(imageFade(blue));
        StartCoroutine(imageFade(red));
    }

    public void UpdateScore(MeshRenderer currentLine)
    {
        Color activeColor = currentLine.GetComponentInParent<Intersection>().chosenLineColour;
        if (activeColor == colourLibrary.green)
        {
            greenCount += 1;
            greenScore.text = greenCount.ToString();
        }
        else if (activeColor == colourLibrary.blue)
        {
            blueCount += 1;
            blueScore.text = blueCount.ToString();
        }       
        else if (activeColor == colourLibrary.red)
        {
            redCount += 1;
            redScore.text = redCount.ToString();
        }
    }

    IEnumerator textFade(Text t)
    {
        float i = 0;
        while (i<1)
        {
            i += Time.deltaTime;
            t.color = Color.Lerp(new Color(t.color.r, t.color.g, t.color.b, 0), new Color(t.color.r, t.color.g, t.color.b, 1), i);
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator imageFade(Image t)
    {
        float i = 0;
        while (i < 1)
        {
            i += Time.deltaTime;
            t.color = Color.Lerp(new Color(t.color.r, t.color.g, t.color.b, 0), new Color(t.color.r, t.color.g, t.color.b, 1), i);
            yield return new WaitForEndOfFrame();
        }
    }
}
