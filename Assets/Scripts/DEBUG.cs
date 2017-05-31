using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DEBUG : MonoBehaviour {

    public Movement movement;
    public BezierSpline STAGE1;
    public BezierSpline STAGE2;
    public BezierSpline STAGE3;
    public BezierSpline STAGE4;
    public BezierSpline STAGE5;

    void Update () {
		if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            movement.spline = STAGE1;
            movement.progress = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            movement.spline = STAGE2;
            movement.progress = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            movement.spline = STAGE3;
            movement.progress = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            movement.spline = STAGE4;
            movement.progress = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Keypad5))
        {
            movement.spline = STAGE5;
            movement.progress = 0;
        }
    }
}
