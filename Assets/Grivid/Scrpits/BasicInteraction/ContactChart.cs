using DxR;
using Microsoft.MixedReality.Toolkit.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactChart : MonoBehaviour
{
    public static ContactChart Instance;
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
                pointerHandler.OnPointerDown.AddListener((e) => { HighLight(markList.IndexOf(mark), gameObject.GetComponent<Vis>().markInstances.Count, mark); });
                pointerHandler.OnPointerUp.AddListener((e) => { OffLight(markList.IndexOf(mark), gameObject.GetComponent<Vis>().markInstances.Count, mark); });
                if(!mark.GetComponent<HighLight>())
                    mark.AddComponent<HighLight>();
            }
       }
    }
    //index 指明节点位置 cou指明数据数量
    private void HighLight(int index, int cou,GameObject gameObject) {
        Instance.ColorDic = new Dictionary<GameObject, Color>();
        foreach (GameObject Lio in Instance.Lios) {
            if (!Lio.GetComponent<Vis>().markInstances.Contains(gameObject)
                && cou ==Lio.GetComponent<Vis>().markInstances.Count) {
                List<GameObject> markList = Lio.GetComponent<Vis>().markInstances;
                Instance.ColorDic.Add(markList[index], markList[index].GetComponent<Renderer>().material.color);
                markList[index].GetComponent<Renderer>().material.color = Color.green;
            }
        }
    }
    private void OffLight(int index,int cou, GameObject gameObject) {
        foreach (GameObject Lio in Instance.Lios) {
            if (!Lio.GetComponent<Vis>().markInstances.Contains(gameObject) &&
                cou == Lio.GetComponent<Vis>().markInstances.Count) {
                List<GameObject> markList = Lio.GetComponent<Vis>().markInstances;
                if (Instance.ColorDic.ContainsKey(markList[index])) {
                    markList[index].GetComponent<Renderer>().material.color = Instance.ColorDic[markList[index]];
                }
            }
        }
    }
}
