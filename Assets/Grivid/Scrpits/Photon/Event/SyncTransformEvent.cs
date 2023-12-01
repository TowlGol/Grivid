using ExitGames.Client.Photon;
using System.Collections.Generic;
using Common;
using System.Xml.Serialization;
using System.IO;
using UnityEngine;
public class SyncTransformEvent : BaseEvent
{
    private void Start() {
        this.EventCode = Common.EventCode.SyncTransform;
        PhotonClient.Instance.AddEvent(this);
    }

    public override void OnEvent(EventData eventData)
    {
        GameObject present;
        string gameObjectDataString = (string)DictTool.GetValue(eventData.Parameters, (byte)ParameterCode.GameObject);

        GameObjectData data;
        using (StringReader reader = new StringReader(gameObjectDataString))
        {
            XmlSerializer serializer = new XmlSerializer(typeof(GameObjectData));
            data = (GameObjectData)serializer.Deserialize(reader);
        }
        Debug.Log(data.name);
        present = GameObject.Find(data.name);
        if (present == null) {
            present = new GameObject(data.name);
            present.AddComponent<SyncObjectCondition>();
        }
            

        present.transform.localPosition = new Vector3() { x = data.px, y = data.py, z = data.pz };
        present.transform.localEulerAngles = new Vector3() { x = data.rx, y = data.ry, z = data.rz };
        present.transform.localScale = new Vector3() { x = data.sx, y = data.sy, z = data.sz };
        if (data.r != 0 && data.g != 0 && data.b != 0)
            present.transform.GetComponent<MeshRenderer>().materials[0].color = new Color() { r = data.r, g = data.g, b = data.b, a = data.a };
        present.SetActive(data.active);
        present.GetComponent<SyncObjectCondition>().flag = false;


    }
}
