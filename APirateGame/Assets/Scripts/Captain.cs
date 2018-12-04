using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.EventSystems;

public class Captain : MonoBehaviour, IPointerClickHandler, IPointerDownHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        GameManager.Instance.UiController.OnCaptainSelected();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        GameManager.Instance.UiController.OnCaptainSelected();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
