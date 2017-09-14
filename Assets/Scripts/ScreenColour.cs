using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenColour : MonoBehaviour {

    public AnimationCurve cameraZoomCurve;
    public Color beginning;
    public Color baby;
    public Color toddler;
    public Color child;
    public Color teenage;
    public Color twenties;
    public Color twentiesEnd;
    public Color present;

    public float babyHeight;
    public float toddlerHeight;
    public float childHeight;
    public float teenageHeight;
    public float twentiesHeight;
    public float twentiesEndHeight;
    public float presentHeight;

    private bool beginningDone = false;
    private bool babyDone = false;
    private bool toddlerDone = false;
    private bool childDone = false;
    private bool teenageDone = false;
    private bool twentiesDone = false;
    private bool twentiesEndDone = false;
    private bool presentDone = false;


    public GameObject player;
    public Camera gameCamera;

	void Update () {
        if (player.transform.position.y >= 0 && !beginningDone)
            StartCoroutine(FadeIn(beginning, baby));
        if (player.transform.position.y > babyHeight && !babyDone)
        {
            babyDone = true;
            StartCoroutine(ChangeScreenColour(baby, toddler, babyHeight, toddlerHeight));
        }  
        if (player.transform.position.y > toddlerHeight && !toddlerDone)
        {
            toddlerDone = true;
            StartCoroutine(ChangeScreenColour(toddler, child, toddlerHeight, childHeight));
        }
        if (player.transform.position.y > childHeight && !childDone)
        {
            childDone = true;
            StartCoroutine(ChangeScreenColour(child, teenage, childHeight, teenageHeight));
        }
        if (player.transform.position.y > teenageHeight && !teenageDone)
        {
            teenageDone = true;
            StartCoroutine(ChangeScreenColour(teenage, twenties, teenageHeight, twentiesHeight));
        }
        if (player.transform.position.y > twentiesHeight && !twentiesDone)
        {
            twentiesDone = true;
            StartCoroutine(ChangeScreenColour(twenties, twentiesEnd, twentiesHeight, twentiesEndHeight));
        }
        if (player.transform.position.y > twentiesEndHeight && !twentiesEndDone)
        {
            twentiesEndDone = true;
            StartCoroutine(ChangeScreenColour(twentiesEnd, present, twentiesEndHeight, presentHeight));
        }

        //Camera Zoom
        //  gameCamera.transform.position = new Vector3(gameCamera.transform.position.x, gameCamera.transform.position.y, -15 - ((cameraZoonCurve.Evaluate(player.transform.position.y/100))*45)); //change 100 to whatever max Y is in game
    }

    public IEnumerator FadeIn(Color start, Color end)
    {
        beginningDone = true;
        float i = 0;
        while (i < 1)
        {
            i += Time.deltaTime * 1f;
            gameCamera.backgroundColor = Color.Lerp(start, end, i);
            yield return new WaitForEndOfFrame();
        }
    }

    public IEnumerator ChangeScreenColour(Color start, Color end, float startY, float endY)
    {
        float i = 0;
        while (i <1)
        {  
            i = 1-((endY - player.transform.position.y) / (endY-startY));
            gameCamera.backgroundColor = Color.Lerp(start, end, i);
            yield return new WaitForEndOfFrame();
        }
    }
}
