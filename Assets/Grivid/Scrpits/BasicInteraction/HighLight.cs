using DxR;
using Microsoft.MixedReality.Toolkit.Input;
using System.Collections.Generic;
using UnityEngine;

public class HighLight : MonoBehaviour
{
    private List<GameObject> ColorChange;
    private List<Color> ColorArry;
    private void Start() {
        if (gameObject.GetComponent<Renderer>()) {
            var touchable = this.gameObject.AddComponent<NearInteractionTouchableVolume>();
            touchable.EventsToReceive = TouchableEventType.Pointer;
            var material = this.gameObject.GetComponent<Renderer>().material;
            Color initColor = material.color;
            // Change color on pointer down and up
            var pointerHandler = this.gameObject.AddComponent<PointerHandler>();
            pointerHandler.OnPointerDown.AddListener((e) => {
                material.color = Color.green;
                Appear(); 
            });
            pointerHandler.OnPointerUp.AddListener((e) => {
                material.color = initColor;
                DestoryInfo(); 
            });
        }
    }

    private void DestoryInfo() {
        Destroy(GameObject.Find("Imvisd Info Text"));
        for (int i = 0; i < ColorChange.Count; i++) {
            ColorChange[i].GetComponent<Renderer>().material.color = ColorArry[i];
        }
        Debug.LogError("消失了");
    }
    private void Appear() {
        GameObject bar = gameObject.transform.parent.transform.parent.transform.parent.gameObject;
        Data data = bar.GetComponent<Vis>().data;
        List<GameObject> gameObjects = bar.GetComponent<Vis>().markInstances;
        int index;
        for (index = 0; index < gameObjects.Count; index++) {
            if (gameObjects[index] == gameObject) {
                break;
            }
        }
        GameObject text = new GameObject("Imvisd Info Text");
        text.transform.position = gameObject.transform.position + Camera.main.transform.right * 0.02f;
        text.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        text.transform.forward = Camera.main.transform.forward;
        TextMesh textMesh = text.AddComponent<TextMesh>();
        Debug.LogError("出现了");
        for (int i = 0; i < data.fieldNames.Count; i++) {
            textMesh.text += data.fieldNames[i] + ":" + data.values[index][data.fieldNames[i]] + "\n";
        }
        ColorChange = new List<GameObject>();
        ColorArry = new List<Color>();
    }
}
