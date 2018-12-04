using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TryAgainButton : MonoBehaviour {

	public void OnTryAgain()
    {
        gameObject.GetComponent<GameSceneManager>().LoadSceneByIndex(2);
    }
}
