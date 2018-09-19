using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//控制摄像机移动--------------------------
public class MoveButton : Button
{
    public Camera mycamera;
    public Camerastate direction;
    public enum Camerastate
    {
        moveright,moveleft,stop
    }
    public Camerastate camerastate;
 
    protected override void DoStateTransition(SelectionState state, bool instant)
    {
        base.DoStateTransition(state, instant);
        switch (state)
        {
            case SelectionState.Disabled:
                break;
            case SelectionState.Highlighted:
                camerastate = direction;
                break;
            case SelectionState.Normal:
                camerastate = Camerastate.stop;
                break;
            case SelectionState.Pressed:
                break;
            default:
                break;
        }
    }
    private void Update()
    {
        if(camerastate==Camerastate.moveleft&&mycamera.transform.position.x>4)
        {
            mycamera.transform.Translate(-15.0F * Time.deltaTime, 0, 0);
        }
        else if(camerastate == Camerastate.moveright&&mycamera.transform.position.x<76)
        {
            mycamera.transform.Translate(15.0F * Time.deltaTime, 0, 0);
        }
    }
}
