using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrividPrefab : MonoBehaviour
{
    public GameObject MovePrefab;
    public GameObject ToggleSwitchPrefab;
    public GameObject TagPrefab;
    public GameObject ColorLine;
    public GameObject TextMeshPrefab;
    public GameObject PinchSilder;
    public GameObject BackGrundPlant;
    public GameObject Line;
    public static GrividPrefab Instance;
    private void Awake() {
        if(Instance == null) {
            Instance = this;
        }
    }

}
