using UnityEngine;
using UnityEditor;
using Assets.Scripts;
using System;
using System.Collections.Generic;
using System.Xml;
using System.Linq;
using UnityEngine.EventSystems;

public class Barf : MonoBehaviour
{
    private GameObject BarfGO;
    private SpriteMask Mask;
    private float Y_offset = 0.0f;
    private float yReset;
    private float yStart;
    private Vector3 OriginalSize;
    public float BarfingSpeed = 0.008f;
    public bool IsBarfing = false;


    public Barf(Transform t)
    {
        BarfGO = Instantiate(Resources.Load<GameObject>("Prefabs/Barf"), t);
        BarfGO.transform.position = t.position;
        Mask = BarfGO.transform.Find("BarfMask").gameObject.GetComponent<SpriteMask>();

        Vector3 originalPosition = Mask.transform.localPosition; 
        OriginalSize = BarfGO.GetComponent<Renderer>().bounds.size;

        yStart = originalPosition.y + 0.8f*OriginalSize.y;
        yReset = yStart - 2.0f * OriginalSize.y;

        originalPosition.y = yStart;
        Mask.transform.localPosition = originalPosition;

    }

    // Update is called once per frame
    public void UpdateBarf()
    {
        Vector3 v3 = Mask.transform.localPosition;

        Y_offset += (IsBarfing)?BarfingSpeed:0.0f;
        v3.y = yStart - Y_offset;

        if (v3.y < yReset)
        {
            Y_offset = 0.0f;
            IsBarfing = false;
        }

        Mask.transform.localPosition = v3;
    }

    public void StartBarfing()
    {
        BarfGO.GetComponent<AudioSource>().Play();
        IsBarfing = true;
    }
}