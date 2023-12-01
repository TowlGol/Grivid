using DxR;
using UnityEngine;
using System.Collections.Generic;
using System;


#region 项目概述
/// <summary>
/// DXR_ElementObtai
/// @ 创建人：刘亚鹏
/// @ 创建时间：2023/8/19 9:31:03
/// @ 作用:
///     
///     
///     
///     
/// </summary>
#endregion
namespace Assets.Imvisd.Scrpits {

    public class DXR_ElementObtain {
        //存储当前的DXR中的数据
        Data data;
        public static DXR_ElementObtain Instance = new DXR_ElementObtain();
        int[] criteriaFlag;
        int flagMaxCount = 0;
        int flagExNum = 0;

        //字符串判定
        private void FilterElementByCriteria(CriteriaFilterClass CriteriaFilter, string id) {
            GameObject chart = GameObject.Find(id);
            data = chart.GetComponent<Vis>().data;
            if (CriteriaFilter.contain) {
                flagMaxCount++;
            }
            if (criteriaFlag == null)
                criteriaFlag = new int[data.values.Count];

            for (int i = 0; i < data.values.Count; i++) {
                if (data.values[i].TryGetValue(CriteriaFilter.field, out string value)) {
                    if (CriteriaFilter.valueArry.Contains(value)) {
                        if (CriteriaFilter.contain)
                            criteriaFlag[i]++;
                        else
                            criteriaFlag[i]--;
                    }
                }
            }
        }

        //根据ID获取当前所有的图表中的元素。
        public List<GameObject> GetAllDxrGameobject(string id) {
            return GameObject.Find(id).GetComponent<Vis>().markInstances;
        }

        //数字、日期判定
        private void FilterElementByCriteria2Arry( CriteriaFilterClass CriteriaFilter,string id) {

            flagExNum++;
            GameObject chart = GameObject.Find(id);
            data = chart.GetComponent<Vis>().data;
            List<List<string>> value2Arry = CriteriaFilter.value2Arry;
            List<string> valueArry;
            if (criteriaFlag == null)
                criteriaFlag = new int[data.values.Count];

            for(int j = 0;j< value2Arry.Count; j++) {
                valueArry = value2Arry[j];
                for (int i = 0; i < data.values.Count; i++) {
                    if (data.values[i].TryGetValue(CriteriaFilter.field, out string value)) {
                        if(CriteriaFilter.contain == true) {
                            try {
                                if (Convert.ToDateTime(valueArry[1]) >= Convert.ToDateTime(value) && Convert.ToDateTime(valueArry[0]) <= Convert.ToDateTime(value)) {
                                    if (criteriaFlag[i] == 0) {
                                        criteriaFlag[i] = 1;
                                    }
                                    else if (flagMaxCount == 1 && criteriaFlag[i] == 1) {
                                        criteriaFlag[i] = 2;
                                    }
                                }
                            }
                            catch {
                                if (double.Parse(valueArry[1]) >= double.Parse(value) && double.Parse(valueArry[0]) <= double.Parse(value)) {
                                    if (criteriaFlag[i] == 0) {
                                        criteriaFlag[i] = 1;
                                    }
                                    else if (flagMaxCount == 1 && criteriaFlag[i] == 1) {
                                        criteriaFlag[i] = 2;
                                    }
                                }
                            }
                            
                        }
                        else {
                            try {
                                if (Convert.ToDateTime(valueArry[1]) <= Convert.ToDateTime(value) || Convert.ToDateTime(valueArry[0]) >= Convert.ToDateTime(value)) {
                                    if (criteriaFlag[i] == 0) {
                                        criteriaFlag[i] = 1;
                                    }
                                    else if (flagMaxCount == 1 && criteriaFlag[i] == 1) {
                                        criteriaFlag[i] = 2;
                                    }
                                }
                            }
                            catch {
                                if (double.Parse(valueArry[1]) <= double.Parse(value) || double.Parse(valueArry[0]) >= double.Parse(value)) {
                                    if (criteriaFlag[i] == 0) {
                                        criteriaFlag[i] = 1;
                                    }
                                    else if (flagMaxCount == 1 && criteriaFlag[i] == 1) {
                                        criteriaFlag[i] = 2;
                                    }
                                }
                            }
                        }

                    }
                }
            }
        }

        //根据前面所述的条件，获取满足所有条件的元素
        public List<GameObject> GetElelmentByCriteria(CriteriaFilterClass CriteriaFilter, string id) {
            if (CriteriaFilter.type == DataType.String)
                FilterElementByCriteria(CriteriaFilter, id);
            else if(CriteriaFilter.type == DataType.Time || CriteriaFilter.type == DataType.Number)
                FilterElementByCriteria2Arry(CriteriaFilter, id);

            List<GameObject> gameObjects = GameObject.Find(id).GetComponent<Vis>().markInstances;
            List<GameObject> resList = new List<GameObject>();

            for (int i = 0; i < criteriaFlag.Length; i++) {
                if (criteriaFlag[i] == flagMaxCount + flagExNum) {
                    resList.Add(gameObjects[i]);
                }
            }
            clear();
            return resList;
        }

        //Input:一个数组与筛选条件，OutPut:根据筛选条件所获得的元素
        public List<GameObject> GetElelmentByCriteria(CriteriaFilterClass CriteriaFilter, string id,List<GameObject> resulatList) {
            if (CriteriaFilter.type == DataType.String)
                FilterElementByCriteria(CriteriaFilter, id);
            else if (CriteriaFilter.type == DataType.Time || CriteriaFilter.type == DataType.Number)
                FilterElementByCriteria2Arry(CriteriaFilter, id);

            List<GameObject> gameObjects = GameObject.Find(id).GetComponent<Vis>().markInstances;
            List<GameObject> resList = new List<GameObject>();

            for (int i = 0; i < criteriaFlag.Length; i++) {
                if (criteriaFlag[i] == flagMaxCount + flagExNum && (resulatList.Count == 0 || resulatList.Contains(gameObjects[i]))) {
                    resList.Add(gameObjects[i]);
                }
            }
            clear();
            return resList;
        }

        //清空筛选记录
        public void clear() {
            criteriaFlag = null;
            flagMaxCount = 0;
            flagExNum = 0;
        }
    }
    
}
