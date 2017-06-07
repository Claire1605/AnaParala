using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColour : MonoBehaviour {

    public Score score;
    public int maxScore;
    private Color currentColor = Color.white;
    private float red = 0;
    private float blue = 0;
    private float green = 0;
    private float total = 0;

    void Update () {
        GetComponent<SpriteRenderer>().color = currentColor;
        total = score.redCount + score.blueCount + score.greenCount;
        if (total>0)
        {
            red = (float)score.redCount / 2;
            blue = (float)score.blueCount / 2;
            green = (float)score.greenCount / 2;
           
        }
        currentColor = new Color((maxScore-green-blue)/maxScore, (maxScore - red - blue)/maxScore, (maxScore - green - red)/maxScore);
    }
}
