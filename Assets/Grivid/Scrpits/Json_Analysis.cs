using Assets.Imvisd.Scrpits;
using Microsoft.MixedReality.Toolkit.UI;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
/*
 * Using for Analysis JSON Data & Get All Portotype
 */
public class Json_Analysis : MonoBehaviour
{
    //用于处理单个Imvisd
    GrividClass imvisd = new GrividClass();
    //用于记录单个criteria
    CriteriaClass criteria = new CriteriaClass();
    // 用于记录单个criteriaFilter
    CriteriaFilterClass criteriaFilter = new CriteriaFilterClass();
    //用于记录单个Interaction
    InteractionClass interaction = new InteractionClass();
    //用于记录Param声明
    ParamClass param = new ParamClass();
    //用于记录CriteriaArry声明
    CriteriaArryClass criteriaArry = new CriteriaArryClass();
    //用于记录单个paramCriteria
    CriteriaClass paramCriteria = new CriteriaClass();
    // 用于记录单个paramCriteriaFilter
    CriteriaFilterClass paramCriteriaFilter = new CriteriaFilterClass();
    //用于记录param中所声明的变量
    Dictionary<string, List<GameObject>> paramDitionary = new Dictionary<string, List<GameObject>>();


    [SerializeField]
    private List<string> fileNameList;
    //public DXR_ElementObtain DXR = new DXR_ElementObtain();
    public void Start() {
        foreach(string fileName in fileNameList)
            JsonPrototypeRead(GetJsonFromFile(fileName));
    }

    //获取相应文件名称
    private string GetJsonFromFile(string fileName) {
        string ret = "";
        string line;
        try {
            //Pass the file path and file name to the StreamReader constructor
            StreamReader sr = new StreamReader("Assets\\Grivid\\Interaction_Design\\" + fileName);
            //Read the first line of text
            line = sr.ReadLine();
            //Continue to read until you reach end of file
            while (line != null) {
                //write the line to console window
                //Read the next line
                ret = ret + line;
                line = sr.ReadLine();
            }
            //close the file
            sr.Close();
        }
        catch (Exception e) {
            Debug.Log("Exception: " + e.Message);
        }
        finally {
            Debug.Log("Executing finally block.");
        }
        return ret;
    }
    private void JsonPrototypeRead(string jsonString) {
        JObject IMVISD;

        //用于处理单个Imvisd
        imvisd = new GrividClass();
        //用于记录单个criteria
        criteria = new CriteriaClass();
        // 用于记录单个criteriaFilter
        criteriaFilter = new CriteriaFilterClass();
        //用于记录单个Interaction
        interaction = new InteractionClass();
        //用于记录Param声明
        param = new ParamClass();
        //用于记录CriteriaArry声明
        criteriaArry = new CriteriaArryClass();
        //用于记录单个paramCriteria
        paramCriteria = new CriteriaClass();
        // 用于记录单个paramCriteriaFilter
        paramCriteriaFilter = new CriteriaFilterClass();
        //用于记录param中所声明的变量
        paramDitionary = new Dictionary<string, List<GameObject>>();




        IMVISD = JObject.Parse(jsonString);
        //获取第一个设计声明
        imvisd.imvidItem = IMVISD.SelectToken("Grammar[0]");
        JToken imvisdItem = imvisd.imvidItem;
        //List<GameObject> paramList = new List<GameObject>();
        //List<GameObject> criteriaList = new List<GameObject>();



        //获取筛选条件
        while (imvisdItem != null) {
            List<GameObject> paramList = new List<GameObject>();
            List<GameObject> criteriaList = new List<GameObject>();
            //获取Target
            imvisd.target = (Target)Enum.Parse(typeof(Target), (string)imvisdItem.SelectToken("Target"));
            

            //获取Param声明
            imvisd.param = imvisdItem.SelectToken("Param[0]");
            if (imvisd.param != null) {

                param.name = (string)imvisd.param.SelectToken("name");
                param.CriteriaArry = imvisd.param.SelectToken("CriteriaArry[0]");
                while (param.CriteriaArry != null) {
                    //清除上次记录
                    paramDitionary.Clear();
                    //获取Target
                    criteriaArry.target = param.CriteriaArry.SelectToken("target") == null ?
                    Target.NULL : (Target)Enum.Parse(typeof(Target), (string)param.CriteriaArry.SelectToken("target"));
                    //获取筛选条件
                    criteriaArry.criteria = param.CriteriaArry.SelectToken("criteria[0]");
                    //筛选条件中的ID以及筛选因素
                    paramCriteria.id = (string)criteriaArry.criteria.SelectToken("id");
                    paramCriteria.criteriaFilter = criteriaArry.criteria.SelectToken("criteriaFilter[0]");

                    //如果目标是图表
                    if (criteriaArry.target == Target.Chart) {
                        if (paramCriteria.id == "ALL")
                            paramList.AddRange(GameObject.FindGameObjectsWithTag("DxRVis"));
                        else
                            paramList.Add(GameObject.Find(paramCriteria.id));
                    }
                    //如果目标是场景
                    else if(criteriaArry.target == Target.Scene) {
                        paramList.Add(GameObject.Find("GrividPrefab"));
                    }
                    //如果目标是用户
                    else if(criteriaArry.target == Target.User) {
                        paramList.Add(Camera.main.gameObject);
                    }
                    //目标是图表中的元素
                    else {
                        //获取当前筛选条件
                        if (paramCriteria.criteriaFilter != null) {
                            paramCriteriaFilter = GetFilterProptype(paramCriteria.criteriaFilter);
                            /*根据结果筛选 */
                            paramList = DXR_ElementObtain.Instance.GetElelmentByCriteria(paramCriteriaFilter, paramCriteria.id);
                        }
                        else {
                            paramList = DXR_ElementObtain.Instance.GetAllDxrGameobject(paramCriteria.id);
                        }
                    }

                    
                    if(paramDitionary.ContainsKey(param.name)) {
                        paramDitionary[param.name].AddRange(paramList);
                    }
                    else {
                        paramDitionary.Add(param.name, paramList);
                    }

                    param.CriteriaArry = param.CriteriaArry.Next;
                }
            }
            
            
            //当对图表或者元素进行操作时，需要进行筛选 获取筛选条件
            if (imvisd.target == Target.Chart|| imvisd.target == Target.Item) {
                //获取筛选条件
                imvisd.criteria = imvisdItem.SelectToken("Criteria[0]");
                //标识当前目标可视化图表
                criteria.id = (string)imvisd.criteria.SelectToken("id");

                //如果当前的目标是图表
                if(imvisd.target == Target.Chart) {
                    if (criteria.id == "ALL")
                        criteriaList.AddRange(GameObject.FindGameObjectsWithTag("DxRVis"));
                    else
                        criteriaList.Add(GameObject.Find(criteria.id));
                }
                else {
                    //获取当前筛选条件
                    criteria.criteriaFilter = imvisd.criteria.SelectToken("criteriaFilter[0]");

                    //如果需要进行筛选
                    if (criteria.criteriaFilter != null) {
                        criteriaFilter = GetFilterProptype(criteria.criteriaFilter);
                        /*
                         * 应该在这里将所有的结果进行传递，或者根据获得的结果筛选出剩余的结果
                         */
                        criteriaList = DXR_ElementObtain.Instance.GetElelmentByCriteria(criteriaFilter, criteria.id, criteriaList);
                    }
                    else {
                        criteriaList = DXR_ElementObtain.Instance.GetAllDxrGameobject(criteria.id);
                    }
                    //resultList = new List<GameObject>();
                }
            }
            //如果Target是User/Scene 无需筛选
            else { 
                //
                if(imvisd.target == Target.User) {
                    criteriaList.Add(Camera.main.gameObject);
                }
                else {
                    criteriaList.Add(GameObject.Find("GrividPrefab"));
                }
            }
            //获取交互方式
            imvisd.interaction = imvisdItem.SelectToken("Interaction[0]");
            if (imvisd.interaction == null) {
                Debug.LogError("INTERACTION DESIGN LOST!");
            }

            while (imvisd.interaction != null) {
                //获取以及设计名称与触发条件
                interaction.name = (string)imvisd.interaction.SelectToken("name");
                interaction.trigger = imvisd.interaction.SelectToken("trigger") == null ?
                    Trigger.NULL : (Trigger)Enum.Parse(typeof(Trigger), (string)imvisd.interaction.SelectToken("trigger"));

                JToken values = imvisd.interaction.SelectToken("attribute[0]");
                if (interaction.attribute.Count != 0)
                    interaction.attribute.Clear();
                while (values != null) {
                    interaction.attribute.Add((string)values);
                    values = values.Next;
                }

                interaction.function = (string)imvisd.interaction.SelectToken("function");
                //开始设计交互
                RealizeInteraction(criteriaList, interaction, paramDitionary);

                imvisd.interaction = imvisd.interaction.Next;
            }

            //进行下个条件的筛选
            imvisdItem = imvisdItem.Next;
        }
    }

    private void RealizeInteraction(List<GameObject> criteriaList, InteractionClass interaction, Dictionary<string, List<GameObject>> paramDitionary) {
        foreach (GameObject element in criteriaList) {
            if (interaction.trigger == Trigger.NULL ) {
                if (interaction.attribute.Count == 0)
                    element.AddComponentExt(interaction.name);
                else {
                    attachMethod(element, interaction);
                }
            }
            else if (interaction.trigger == Trigger.Onclick) {
                element.AddComponent<Interactable>();
                element.GetComponent<Interactable>().OnClick.AddListener(()=>{
                    attachMethod(element, interaction);
                });
            }
            else {
                if(element.GetComponent<ConstraintManager>() == null)
                    element.AddComponent<ConstraintManager>();
                if(element.GetComponent<ObjectManipulator>() == null)
                    element.AddComponent<ObjectManipulator>();
                if (element.GetComponent<BoxCollider>() == null)
                    element.AddComponent<BoxCollider>();
                if (interaction.trigger == Trigger.Ondrag) {
                    element.GetComponent<ObjectManipulator>().OnManipulationStarted.AddListener((ObjectManipulator) => {
                        attachMethod(element, interaction);
                    });
                }
                else if (interaction.trigger == Trigger.Onhover) {
                    element.GetComponent<ObjectManipulator>().OnHoverEntered.AddListener((ObjectManipulator) => {
                        attachMethod(element, interaction);
                    });
                }
                else if (interaction.trigger == Trigger.DragExit) {
                    element.GetComponent<ObjectManipulator>().OnManipulationEnded.AddListener((ObjectManipulator) => {
                        attachMethod(element, interaction);
                    });
                }
                else if (interaction.trigger == Trigger.HoverExit) {
                    element.GetComponent<ObjectManipulator>().OnHoverExited.AddListener((ObjectManipulator) => {
                        attachMethod(element, interaction);
                    });
                }
            }
        }
    }
    //调用相应的方法
    private void attachMethod(GameObject element, InteractionClass interaction) {
        var t = element.AddComponentExt(interaction.name).GetType();
        MethodInfo[] info = t.GetMethods();
        for (int i = 0; i < info.Length; i++) {
            var md = info[i];
            //方法名
            string mothodName = md.Name;
            //参数集合
            ParameterInfo[] paramInfos = md.GetParameters();
            object[] objList = new object[interaction.attribute.Count];
            //objList[interaction.attribute.Count] = element;
            //方法名相同且参数个数一样
            if (mothodName == interaction.function && paramInfos.Length == interaction.attribute.Count) {

                int index = 0;
                foreach (string e in interaction.attribute) {
                    if (paramDitionary.ContainsKey(e))
                        objList[index++] = paramDitionary[e];
                    else if(e == "THIS") {
                        objList[index++] = element;
                    }
                    else
                        objList[index++] = e;
                }
                md.Invoke(Activator.CreateInstance(t), objList);
            }
        }
    }
    //获取筛选条件
    private CriteriaFilterClass GetFilterProptype(JToken criteriaFilterToken) {
        CriteriaFilterClass criteriaFilter = new CriteriaFilterClass();

        criteriaFilter.field = criteriaFilterToken.SelectToken("field") == null
                        ? null : (string)criteriaFilterToken.SelectToken("field");
        //对包含属性进行筛选，默认情况下使用包含
        criteriaFilter.contain = criteriaFilterToken.SelectToken("contain") == null
            ? true : (bool)criteriaFilterToken.SelectToken("contain");
        //对数据类型进行获取，分为Number、Time、String
        criteriaFilter.type = (string)criteriaFilterToken.SelectToken("type") == null
            ? DataType.NULL : (DataType)Enum.Parse(typeof(DataType), (string)criteriaFilterToken.SelectToken("type"));

        //当数据类型为数字或者时间时，需要创建队列将所有限制因素导入
        if (criteriaFilter.type == DataType.Number || criteriaFilter.type == DataType.Time) {
            JToken values = criteriaFilterToken.SelectToken("value[0]");
            List<string> tmpList;
            while (values != null) {
                tmpList = new List<string>();
                tmpList.Add((string)values.First);
                tmpList.Add((string)values.Last);
                criteriaFilter.value2Arry.Add(tmpList);
                values = values.Next;
            }
        }

        //当数据类型为字符串类型时，使用一维数组进行存储操作
        if (criteriaFilter.type == DataType.String) {
            JToken values = criteriaFilterToken.SelectToken("value").First;
            while (values != null) {
                criteriaFilter.valueArry.Add((string)values);
                values = values.Next;
            }
            //当默认情况下采取全匹配的形式
            criteriaFilter.strict = criteriaFilterToken.SelectToken("strict") == null
                ? true : (bool)criteriaFilterToken.SelectToken("strict");
        }

        return criteriaFilter;
    }
}
