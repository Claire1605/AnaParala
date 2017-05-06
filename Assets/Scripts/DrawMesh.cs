using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class DrawMesh : MonoBehaviour {

    private BezierSpline bSpline;
    public AnimationCurve anim;

    public float width = 1;
    [Range(1, 40)]
    private int YSegments = 30;
    [Range(1, 4)]
    private int XSegments = 2;
    Vector3 v;
    Vector3 xIncrement;
    Vector3 P3;
    Vector3 P4;
    Vector3[,] points;

    Vector3[] vertices;
    Vector2[] uv;
    int[] triangles;
    Mesh mesh;
    MeshFilter meshFilter;

	void Start () {
        bSpline = GetComponentInParent<BezierSpline>();
        //not sure how to do this if length needs to change at runtime? maybe keep as is, and just change width through shader?
        mesh = new Mesh();
        meshFilter = GetComponent<MeshFilter>();
        GetComponent<MeshFilter>().mesh = mesh;
        DrawQuad();
    }
	
	void Update () {
       DrawQuad();
    }

    void DrawQuad()
    {
        points = new Vector3[XSegments + 1, YSegments + 1];
        vertices = new Vector3[(XSegments + 1) * (YSegments + 1)];
        triangles = new int[6 * XSegments * YSegments];
        uv = new Vector2[vertices.Length];

        for (int i = 0; i <= YSegments; i++)
        {
            v = bSpline.GetDirection((float)i / YSegments); //get tangent of point to spline
            P3 = new Vector3(-v.y, v.x) / Mathf.Sqrt(Mathf.Pow(v.x, 2) + Mathf.Pow(v.y, 2)) * width; //get recipricol

            for (int j = 0; j <= XSegments; j++)
            {
                xIncrement = P3 / XSegments * 2; //(YSegments / 2 >= i ? Mathf.Sqrt(i) : Mathf.Sqrt(YSegments - i))
                points[j, i] = transform.InverseTransformPoint(bSpline.GetPoint((float)i / YSegments) + (P3) - (xIncrement * j)); //i * P3 and i on the line above give the change in width along y
                points[j, i].z = 0.1f;

                //Tapered
                //float a = YSegments / (0.05f * ((i - YSegments / 2)* (i - YSegments / 2)) + 10f);
                //xIncrement = new Vector3(-v.y, v.x) / Mathf.Sqrt(Mathf.Pow(v.x, 2) + Mathf.Pow(v.y, 2)) * (a * width / XSegments) * 2; //(YSegments / 2 >= i ? Mathf.Sqrt(i) : Mathf.Sqrt(YSegments - i))
                //points[j, i] = transform.InverseTransformPoint(bSpline.GetPoint((float)i / YSegments) + (a * P3) - (xIncrement * j)); //i * P3 and i on the line above give the change in width along y
                //points[j, i].z = 0.5f;
            }
        }
        
        for (int y = 0, a = 0; y <= YSegments; y++)
        {
            for (int x = 0; x <= XSegments; x++, a++)
            {
                vertices[a] = points[x, y];
                uv[a] = new Vector2((float)x / XSegments, (float)y / YSegments);
            }
        }
        mesh.vertices = vertices;
        mesh.uv = uv;

        for (int g = 0, f = 0, y = 0; y < YSegments; y++, f++)
        {
            for (int x = 0; x < XSegments; x++, g += 6, f++)
            {
                triangles[g] = f;
                triangles[g + 3] = triangles[g + 2] = f + 1;
                triangles[g + 4] = triangles[g + 1] = f + XSegments + 1;
                triangles[g + 5] = f + XSegments + 2;
            }
        }

    
        mesh.triangles = triangles;


        mesh.RecalculateBounds();
        mesh.RecalculateNormals();

        if (meshFilter != null)
        {
            meshFilter.sharedMesh = mesh;
        }
    }

    public IEnumerator changeWidth(float startWidth, float endWidth)
    {
        float i = 0;
        float rate = 0.5f;
        while (i <1)
        {
            i += Time.fixedDeltaTime * rate;
            width = Mathf.Lerp(startWidth, endWidth, anim.Evaluate(i));
            DrawQuad();
            yield return new WaitForFixedUpdate();
        }
    }

    //void OnDrawGizmos()
    //{
    //    points = new Vector3[XSegments + 1, YSegments + 1];
    //    for (int i = 0; i <= YSegments; i++)
    //    {
    //        v = (bSpline.GetPoint((float)i / YSegments) + bSpline.GetDirection((float)i / YSegments)) - bSpline.GetPoint((float)i / YSegments); //get tangent of point to spline

    //        P3 = new Vector3(-v.y, v.x) / Mathf.Sqrt(Mathf.Pow(v.x, 2) + Mathf.Pow(v.y, 2)) * width;
    //        P4 = new Vector3(-v.y, v.x) / Mathf.Sqrt(Mathf.Pow(v.x, 2) + Mathf.Pow(v.y, 2)) * -width;
    //        Gizmos.DrawLine(bSpline.GetPoint((float)i / YSegments) + P3, bSpline.GetPoint((float)i / YSegments) + P4); //draw Tangent

    //        for (int j = 0; j <= XSegments; j++)
    //        {
    //            xIncrement = new Vector3(-v.y, v.x) / Mathf.Sqrt(Mathf.Pow(v.x, 2) + Mathf.Pow(v.y, 2)) * (width / XSegments) * 2;
    //            points[j, i] = bSpline.GetPoint((float)i / YSegments) + P3 - (xIncrement * j);
    //        }
    //    }

    //    for (int x = 0; x <= XSegments; x++)
    //    {
    //        for (int y = 0; y <= YSegments; y++)
    //        {
    //            Gizmos.DrawSphere(points[x, y], 0.05f);
    //        }
    //    }
    //}
}

