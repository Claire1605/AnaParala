using UnityEngine;
using System.Collections;
using System;

public class Recurse : MonoBehaviour {

    public LineRenderer line;
    public AnimationCurve curve;

    public GameObject player;

    public int minBranches;
    public int maxBranches;
    public int minLength;
    public int maxLength;
    public float rate = 0.1f;

    private Vector3 newVector = new Vector3(0, 0, 0);
    private Vector3 startVector = new Vector3(0, 0, 0);

    private int branches;
    public int lineLength;
    private System.Random r;

    private Vector3 relativePos;
    private bool stop = false;

    void Start () {
     //   gameObject.tag = "InactivePath";
        r = new System.Random();
        line = GetComponent<LineRenderer>();
        lineLength = r.Next(minLength, maxLength+1);
        branches = r.Next(minBranches, maxBranches+1);

        GetComponent<BoxCollider>().center = new Vector3(0, lineLength / 2, 0);
        GetComponent<BoxCollider>().size = new Vector3(0, lineLength, 0);
    }

    void Update()
    {
        if (gameObject.tag == "ActivePath")
            ActiveGrow();
        else if (gameObject.tag == "InactivePath")
            InactiveGrow();      
    }

    void InactiveGrow()
    {
       
       // if (line.GetPosition(1).y < 0.3f)
       //     line.SetPosition(1, new Vector3(0, 0.3f - (Vector3.Distance(player.transform.position, line.transform.position))));
    }

    void ActiveGrow()
    {
        relativePos = player.transform.position + new Vector3(0, 0.03f, 0);
        line.SetPosition(0, startVector);
        line.SetPosition(1, relativePos);
        Debug.Log(Vector3.Distance(relativePos, startVector));

        if (Vector3.Distance(relativePos, startVector) >= lineLength - 0.3f && !stop)
        {
            stop = true;
            NewLines();
        }
    }

    void NewLines()
    {
        if (branches % 2 == 0) //even number
        {
            for (int i = -Mathf.FloorToInt(branches / 2); i <= Mathf.FloorToInt(branches / 2); i++)
            {
                if (i != 0)
                {
                    var copy = Instantiate(gameObject);
                    var newRecursion = copy.GetComponent<Recurse>();
                    newRecursion.stop = false;
                    copy.tag = "InactivePath";
                    copy.transform.position += newRecursion.transform.up * lineLength;
                    copy.GetComponent<LineRenderer>().SetPosition(0, new Vector3(0, 0.15f, 0));
                    //copy.GetComponent<LineRenderer>().SetPosition(1, copy.transform.position);
                    StartCoroutine(GrowLine(copy.GetComponent<LineRenderer>()));

                    // copy.transform.rotation *= Quaternion.Euler(0, 0, ((r.Next(120,240) / branches) * i) + (r.Next(120, 240) / (branches * 2)));
                    copy.transform.rotation *= Quaternion.Euler(0, 0, (r.Next(30, 90) * i));
                }
               
            }
        }
        else if (branches % 2 != 0) //odd number  ...  && generations > 0
        {
            for (int i = -Mathf.FloorToInt(branches / 2); i <= Mathf.FloorToInt(branches / 2); i++)
            {
                var copy = Instantiate(gameObject);
                var newRecursion = copy.GetComponent<Recurse>();
                copy.tag = "InactivePath";
                copy.transform.position += newRecursion.transform.up * lineLength;
                copy.GetComponent<LineRenderer>().SetPosition(0, new Vector3(0, 0.15f, 0));
                //copy.GetComponent<LineRenderer>().SetPosition(1, copy.transform.position);
                StartCoroutine(GrowLine(copy.GetComponent<LineRenderer>()));

                copy.transform.rotation *= Quaternion.Euler(0, 0, r.Next(30,80) * i);
            }
        }
    }

    IEnumerator GrowLine(LineRenderer newLine)
    {
        float i = newLine.GetPosition(0).y;
        float rate = 5;
        while (i < 0.4)
        {
            i += Time.deltaTime / rate;

            newLine.SetPosition(1, new Vector3(0,i, 0));
            yield return new WaitForEndOfFrame();
        }
       
    }
}
