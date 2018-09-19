using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneLoad : MonoBehaviour {
    //重新开始游戏---------------------
    public void Retry()
    {
        SceneManager.LoadScene("Mine");
    }
    //返回主菜单------------------------
    public void Return()
    {
        SceneManager.LoadScene("Menu");
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
