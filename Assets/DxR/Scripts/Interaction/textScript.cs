using Microsoft.MixedReality.Toolkit.Input;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class textScript : MonoBehaviour
{

    private void Start()
    {
        //    gameObject.AddComponent<Renderer>();
        //    gameObject.GetComponent<Renderer>().material.color = Color.red;

        // Add and configure the touchable
        var touchable = gameObject.AddComponent<NearInteractionTouchableVolume>();
        touchable.EventsToReceive = TouchableEventType.Pointer;

        
        var material = gameObject.GetComponent<Renderer>().material;
        Color initColor = material.color;
        // Change color on pointer down and up
        var pointerHandler = gameObject.AddComponent<PointerHandler>();
        pointerHandler.OnPointerDown.AddListener((e) => material.color = Color.green);
        pointerHandler.OnPointerUp.AddListener((e) => material.color = initColor);
    }
    

}