using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerLoad : MonoBehaviour {
    public int LoadTimeOut;

	// Use this for initialization
	void Start () {
        Invoke("LoadNext", LoadTimeOut);
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            LoadNext();
        }
	}

    void LoadNext()
    {
        gameObject.GetComponent<GameSceneManager>().NextScene();
    }
}
