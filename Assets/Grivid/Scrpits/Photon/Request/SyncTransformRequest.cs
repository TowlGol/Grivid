using ExitGames.Client.Photon;
using System.Collections.Generic;
using UnityEngine;
using Common;
using System.IO;
using System.Xml.Serialization;
public class SyncTransformRequest : Request
{
    public static SyncTransformRequest Instance;
    public SyncTransformRequest()
    {
        Instance = this;
    }
    private void Start() {
        this.OpCode = Common.OperationCode.SyncTransform;
        PhotonClient.Instance.AddRequest(this);
    }
    public override void DefaultRequest(GameObject go)
    {
        GameObject tmp = go;
        string name = tmp.transform.name;
        Vector3 position = tmp.transform.localPosition;
        Vector3 rotation = tmp.transform.localEulerAngles;
        Vector3 scale = tmp.transform.localScale;
        bool active = tmp.activeSelf;

        GameObjectData gameObjectData = new GameObjectData(name, position.x, position.y, position.z, rotation.x, rotation.y, rotation.z, scale.x, scale.y, scale.z, 0, 0, 0, 0, active);
        StringWriter sw = new StringWriter();
        XmlSerializer serializer = new XmlSerializer(typeof(GameObjectData));
        serializer.Serialize(sw, gameObjectData);
        sw.Close();
        string gameObjectDataString = sw.ToString();
        Dictionary<byte, object> data = new Dictionary<byte, object>();
        data.Add((byte)ParameterCode.Name, name);
        data.Add((byte)ParameterCode.Collider, 0);
        data.Add((byte)ParameterCode.GameObject, gameObjectDataString);

        PhotonClient.Peer.OpCustom((byte)OpCode, data, true);
        if (go.layer == 31)
            Destroy(go);


    }
    public override void OnOperationResponse(OperationResponse response)
    {
    }
}