using ExitGames.Client.Photon;
using System;
using System.Collections.Generic;
using UnityEngine;
using Common;
using System.IO;
using System.Xml.Serialization;
class SyncNewPlayerRequest : Request
{

    public static SyncNewPlayerRequest Instance;

    public SyncNewPlayerRequest()
    {
        Instance = this;
    }
    public void Start()
    {
        base.Start();
        this.OpCode = Common.OperationCode.SyncNewPlayer;
        PhotonClient.Instance.AddRequest(this);
    }
    public override void DefaultRequest(GameObject go)
    {
        Dictionary<byte, object> data = new Dictionary<byte, object>();
        Vector3 localPosition = Vector3.zero;
        Vector3 cubePosition = Vector3.zero;
        Vector3 res = cubePosition - localPosition;
        Vector3 rotate = GameObject.Find("Main Camera").transform.localEulerAngles;
        PlayerData playerData = new PlayerData(res.x, res.y, res.z, rotate.x, rotate.y, rotate.z);
        StringWriter sw = new StringWriter();
        XmlSerializer serializer = new XmlSerializer(typeof(PlayerData));
        serializer.Serialize(sw, playerData);
        sw.Close();
        string playerDataString = sw.ToString();
        data.Add((byte)ParameterCode.Player, playerDataString);
        PhotonClient.Peer.OpCustom((byte)OpCode, data, true);
    }

    public override void OnOperationResponse(OperationResponse response)
    {
    }
}

