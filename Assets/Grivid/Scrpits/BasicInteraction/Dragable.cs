using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragable : MonoBehaviour
{
    private GameObject[] visList;
    private void Awake() {
        visList = GameObject.FindGameObjectsWithTag("DxRVis");
        foreach (GameObject vis in visList) {
            if (!gameObject.GetComponent<BoxCollider>()) {
                vis.AddComponent<ConstraintManager>();
                vis.AddComponent<BoxCollider>();
                vis.AddComponent<ObjectManipulator>().enabled = false;
                vis.GetComponent<BoxCollider>().enabled = false;
                vis.GetComponent<BoxCollider>().center = new Vector3(0.25f, 0.25f, 0.25f);
                vis.GetComponent<BoxCollider>().size = new Vector3(0.5f, 0.5f, 0.5f);
            }
        }
    }
    public void Start() {
        if(GameObject.Find("ToggleSwitch(Clone)") == null) {
            Instantiate(GrividPrefab.Instance.ToggleSwitchPrefab);
        }
    }
    public void DragableSwitch() {
        TextMesh labelText =  transform.GetChild(0).transform.GetChild(0).GetComponent<TextMesh>();
        if(labelText.text == "Manipulation Off") {
            labelText.text = "Manipulation On";
        }
        else {
            labelText.text = "Manipulation Off";
        }

        foreach(GameObject vis in visList) {
            vis.TryGetComponent<BoxCollider>(out BoxCollider boxCollider);
            vis.TryGetComponent<ObjectManipulator>(out ObjectManipulator objectManipulator);
            
            if (boxCollider) {
                boxCollider.enabled = !boxCollider.enabled;
            }
            if (objectManipulator) {
                objectManipulator.enabled = !objectManipulator.enabled;
            }
        }
    }
}
