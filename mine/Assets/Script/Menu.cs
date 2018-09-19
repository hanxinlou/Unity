using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Menu : MonoBehaviour {

    

	// Use this for initialization
	void Start () {
		
	}
    //根据选的模式决定难度并启动游戏界面-------------------------------
	public void PressButton(int difficulty)
    {
        PlayData.difficulty = difficulty;
        SceneManager.LoadScene("Mine");
    }
	// Update is called once per frame
	void Update () {
		
	}
}
public static class PlayData
{
    public static int difficulty;

}