using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//控制摄像机移动--------------------------
public class MoveButtonData : MonoBehaviour {
    public Camera mycamera;
    public MoveButton.Camerastate direction;
    // Use this for initialization
    void Start () {
        var button = GetComponent<MoveButton>();
        button.mycamera = mycamera;
        button.direction = direction;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
