using DxR;
using Microsoft.MixedReality.Toolkit.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactChartByLine : MonoBehaviour
{
    public static ContactChartByLine Instance;
    private GameObject Lines;
    List<GameObject> Lios;
    Dictionary<GameObject, Color> ColorDic;
    private void Awake() {
        if(Instance == null) {
            Instance = this;
        }
        
    }
    public void ContactCharts(object objectList) {
        Instance.Lios = (List<GameObject>)objectList;

       foreach(GameObject gameObject in Instance.Lios) {
            List<GameObject> markList = gameObject.GetComponent<Vis>().markInstances;
            foreach (GameObject mark in markList) {
                var pointerHandler = mark.GetComponent<PointerHandler>();
                if (mark.GetComponent<PointerHandler>() == null) {
                    pointerHandler = mark.AddComponent<PointerHandler>();
                }
                CreatContactLine(markList.IndexOf(mark), gameObject.GetComponent<Vis>().markInstances.Count, mark);
                if(!mark.GetComponent<HighLight>())
                    mark.AddComponent<HighLight>();
            }
       }
    }
    //index 指明节点位置 cou指明数据数量
    private void CreatContactLine(int index, int cou,GameObject gameObject) {
        if(Lines == null)
            Lines = new GameObject("Lines");

        Instance.ColorDic = new Dictionary<GameObject, Color>();
        foreach (GameObject Lio in Instance.Lios) {
            if (!Lio.GetComponent<Vis>().markInstances.Contains(gameObject)
                && cou ==Lio.GetComponent<Vis>().markInstances.Count) {
                List<GameObject> markList = Lio.GetComponent<Vis>().markInstances;
                GameObject Line = GameObject.Instantiate(GrividPrefab.Instance.Line);
                Debug.LogError("LINE");
                Line.transform.SetParent(Lines.transform);
                LineRenderer LineRender =  Line.GetComponent<LineRenderer>();
                LineRender.SetPosition(0, gameObject.transform.position);
                LineRender.SetPosition(1, markList[index].transform.position);
            }
        }
    }
}
