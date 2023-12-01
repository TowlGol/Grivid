using Assets.Imvisd.Scrpits;
using DxR;
using Microsoft.MixedReality.Toolkit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Filter : MonoBehaviour {
    
    [SerializeField] string filterFieldName;

    private List<GameObject> itemList;
    private GameObject filterAimObject;
    private Data data;
    private Vis vis;
    private List<Color> colorList;

    private void Start() {
        if (filterFieldName != null) {
            filterAimObject = gameObject;
            filter(filterAimObject, filterFieldName);
        }
    }

    public void filter(GameObject gameObject,string fieldName) {
        Init(gameObject);
        Dictionary<string, List<GameObject>> fieldDic = new Dictionary<string, List<GameObject>>();
        for(int i = 0; i < itemList.Count; i++) {
            if(data.values[i].TryGetValue(fieldName,out string outFiledName)) {
                if(!fieldDic.ContainsKey(outFiledName)) {
                    fieldDic[outFiledName] = new List<GameObject>();
                }
                fieldDic[outFiledName].Add(itemList[i]);
            }
        }

        float middleCount;
        float minCount;
        float maxCount;
        int colorIndex = 0;
        float Times;
        Vector3 scale;
        GameObject tag = null;

        tag = InitTitle(fieldName,gameObject);
        colorIndex++;
        if (fieldDic.Keys.Count > colorList.Count) {
            scale = gameObject.GetComponent<Vis>().markInstances[0].transform.localScale;
            middleCount = GetCount(1, fieldDic);
            minCount = GetCount(0, fieldDic);
            maxCount = GetCount(2, fieldDic);
            CreatColorLine(colorList[colorList.Count / 2], colorList[0],  new Vector3(-0.25f, 0.45f, 0),gameObject, middleCount, maxCount);
            CreatColorLine(colorList[colorList.Count - 1], colorList[colorList.Count/2],  new Vector3(-0.35f, 0.45f, 0), gameObject, minCount, middleCount);
            CreatSilder(minCount,maxCount,gameObject,fieldName, scale);
            CreatSilder(minCount, maxCount, gameObject, fieldName, scale, 1);
        }
        
        foreach (string key in fieldDic.Keys) {
            Times = (float)colorIndex / fieldDic.Keys.Count * 2;

            if (fieldDic.Keys.Count <= colorList.Count) {
               tag =  InitTag(key, colorIndex,gameObject);
            }

            foreach (GameObject item in fieldDic[key]) {
                if(fieldDic.Keys.Count >  colorList.Count)
                    Times = (float)2.0 - GetIndex(float.Parse(key), fieldDic) / fieldDic.Keys.Count * 2;
                if (Times <= 1) {
                    item.GetComponent<Renderer>().material.color = Color.Lerp(colorList[0], colorList[colorList.Count/2], Times);
                    }
                else {
                    item.GetComponent<Renderer>().material.color = Color.Lerp(colorList[colorList.Count/2], colorList[colorList.Count-1], Times-1);
                }
                if (fieldDic.Keys.Count <= 8)
                    tag.GetComponent<Renderer>().material.color = item.GetComponent<Renderer>().material.color;
            }
            colorIndex++;
        }
    }

    private void CreatSilder(float Min, float Max, GameObject gameObject, string fieldName, Vector3 scale, float SilderValue = 0) {
        GameObject silder = Instantiate(GrividPrefab.Instance.PinchSilder);
        silder.transform.SetParent(gameObject.transform);
        silder.transform.localPosition = new Vector3(-0.25f,0.42f,0);
        silder.transform.localScale = new Vector3(0.8f, 1, 1);
        silder.transform.localEulerAngles = Vector3.zero;
        silder.GetComponent<PinchSlider>().SliderValue = SilderValue;

        silder.GetComponent<PinchSlider>().OnInteractionEnded.AddListener((e) => {
            SliderEvent(Min, Max, gameObject, fieldName, scale);
        });
    }
    private void SliderEvent(float Min,float Max, GameObject gameObject,string fieldName, Vector3 scale) {
        float minSilder = -1;
        float maxSilder = -1;
        float distance = Max - Min;
        List<GameObject> Item = gameObject.GetComponent<Vis>().markInstances;
        foreach(GameObject item in Item) {
            item.transform.localScale = Vector3.zero;
        }
        //获取筛选范围
        foreach (PinchSlider pinchSlider in gameObject.GetComponentsInChildren<PinchSlider>()) {
            if(maxSilder == -1) {
                maxSilder = pinchSlider.SliderValue;
            }
            else 
            {
                minSilder = pinchSlider.SliderValue;
            }
        }
        if(maxSilder < minSilder) {
            float tmp;
            tmp = maxSilder;
            maxSilder = minSilder;
            minSilder = tmp;
        }
        maxSilder = Min + distance * maxSilder;
        minSilder = Min + distance * minSilder;

        CriteriaFilterClass criteriaFilter = new CriteriaFilterClass();
        List<List<string>> valueArry = new List<List<string>>();
        List<string> value = new List<string>();
        value.Add(minSilder.ToString()); value.Add(maxSilder.ToString());
        valueArry.Add(value);
        criteriaFilter.contain = true;
        criteriaFilter.type = DataType.Number;
        criteriaFilter.value2Arry = valueArry;
        criteriaFilter.field = fieldName;

        List<GameObject> appearList = DXR_ElementObtain.Instance.GetElelmentByCriteria(criteriaFilter, gameObject.name);
        foreach(GameObject item in appearList) {
            item.transform.localScale = scale;
        }

    }

    //初始化
    private void Init(GameObject gameObject) {
        vis = gameObject.GetComponent<Vis>();
        itemList = vis.markInstances;
        data = vis.data;
        colorList = new List<Color>();
        colorInit(colorList);

    }
    private void CreatColorLine(Color start, Color end ,Vector3 position,GameObject gameObject,float minCount,float maxCount) {
        GameObject colorLine = Instantiate(GrividPrefab.Instance.ColorLine);
        colorLine.transform.SetParent(gameObject.transform);
        colorLine.transform.localPosition = position;
        colorLine.transform.localScale =new Vector3(0.5f,1,1);
        colorLine.transform.localEulerAngles = Vector3.zero;
        colorLine.GetComponent<LineRenderer>().startColor = start;
        colorLine.GetComponent<LineRenderer>().endColor = end;
        colorLine.GetComponentsInChildren<TextMesh>()[0].text = minCount.ToString();
        colorLine.GetComponentsInChildren<TextMesh>()[1].text = maxCount.ToString();
    }
    private float GetCount(int index, Dictionary<string, List<GameObject>> fieldDic) {
        List<float> countList = new List<float>();
        foreach (string key in fieldDic.Keys) {
            countList.Add(float.Parse(key));
        }
        countList.Sort();
        if (index == 0)
            return countList[0];
        else if (index == 1)
            return countList[countList.Count / 2];
        else
            return countList[countList.Count - 1];
    }
    private float GetIndex(float cou,Dictionary<string,List<GameObject>> fieldDic) {
        List<float> countList = new List<float>();
        foreach (string key in fieldDic.Keys) {
            countList.Add(float.Parse(key));
        }
        countList.Sort();
        return countList.IndexOf(cou);
    }
    private void colorInit(List<Color> colorList) {
        //System.Random random = new System.Random();
        //if(random.Next(0,3) == 0) {
            colorList.Add(HexRGB2Color("#b94b1f"));
            colorList.Add(HexRGB2Color("#dc9230"));
            colorList.Add(HexRGB2Color("#f1c363"));
            colorList.Add(HexRGB2Color("#f0e3b4"));
            colorList.Add(HexRGB2Color("#eaf3de"));
            colorList.Add(HexRGB2Color("#c1e8c7"));
            colorList.Add(HexRGB2Color("#6ace9e"));
            colorList.Add(HexRGB2Color("#26a280"));
            colorList.Add(HexRGB2Color("#18676d"));
        //}
        //else if (random.Next(0, 3) == 1) {
        //    colorList.Add(HexRGB2Color("#1b7537"));
        //    colorList.Add(HexRGB2Color("#05b550"));
        //    colorList.Add(HexRGB2Color("#7ad089"));
        //    colorList.Add(HexRGB2Color("#bfe9c3"));
        //    colorList.Add(HexRGB2Color("#eff2dd"));
        //    colorList.Add(HexRGB2Color("#ffe9b8"));
        //    colorList.Add(HexRGB2Color("#ffd370"));
        //    colorList.Add(HexRGB2Color("#edb50a"));
        //    colorList.Add(HexRGB2Color("#a27b16"));
        //}
        //else {
        //    colorList.Add(HexRGB2Color("#eaf3de"));
        //    colorList.Add(HexRGB2Color("#f0e3b4"));
        //    colorList.Add(HexRGB2Color("#f1c363"));
        //    colorList.Add(HexRGB2Color("#dc9230"));
        //    colorList.Add(HexRGB2Color("#b94b1f"));
        //}




    }
    //将HEX数值转化为RGB
    private Color HexRGB2Color(string hexRGB) {
        Color color;
        ColorUtility.TryParseHtmlString(hexRGB, out color);
        return color;
    }
    //初始化Title
    private GameObject InitTitle(string fieldName,GameObject parent) {
        GameObject tag = Instantiate(GrividPrefab.Instance.TextMeshPrefab);
        tag.transform.SetParent(parent.transform);
        tag.transform.localEulerAngles = Vector3.zero;
        tag.transform.localPosition = new Vector3(-0.35f, 0.5f, 0);
        tag.GetComponentInChildren<TextMesh>().text = fieldName;
        return tag;
    }
    //创建Tag
    private GameObject InitTag(string key,int colorIndex,GameObject parent) {
        GameObject tag = Instantiate(GrividPrefab.Instance.TagPrefab);
        tag.transform.SetParent(parent.transform);
        tag.transform.localPosition = new Vector3(-0.25f, 0.5f - colorIndex * 0.05f, 0);
        tag.transform.localEulerAngles = Vector3.zero;
        tag.GetComponentInChildren<TextMesh>().text = key;
        return tag;
    }
}