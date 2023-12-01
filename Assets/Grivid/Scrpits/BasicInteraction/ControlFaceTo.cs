using Microsoft.MixedReality.Toolkit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class ControlFaceTo : MonoBehaviour {
    private void Start() {
        CreatSilder();
    }
    private void CreatSilder() {
        GameObject silder = Instantiate(GrividPrefab.Instance.PinchSilder);
        silder.transform.SetParent(gameObject.transform);
        silder.transform.localPosition = new Vector3(0.5f, -0.37f, -0.74f);
        silder.transform.localScale = new Vector3(0.8f, 1, 1);
        silder.transform.localEulerAngles = Vector3.zero;
        silder.GetComponent<PinchSlider>().SliderValue = 0;

        silder.GetComponent<PinchSlider>().OnInteractionEnded.AddListener((e) => {
            SliderEvent(silder);
        });
        GameObject backGround = Instantiate(GrividPrefab.Instance.BackGrundPlant);
        backGround.transform.localPosition = new Vector3(0.5f, -0.37f, -0.73f);
        backGround.transform.localScale = new Vector3(0.3f, 0.1f, 0.1f);
        backGround.transform.localEulerAngles = Vector3.zero;

    }

    private void SliderEvent(GameObject silder) {
        GameObject[] visList = GameObject.FindGameObjectsWithTag("DxRVis");
        float value = silder.GetComponent<PinchSlider>().SliderValue;
        foreach(GameObject vis in visList) {
            vis.GetComponent<faceTo>().toward = 1 - value * 2;
        }
    }

}