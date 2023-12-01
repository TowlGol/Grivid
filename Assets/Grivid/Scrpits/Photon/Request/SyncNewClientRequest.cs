using ExitGames.Client.Photon;
using UnityEngine;
using Common;
using System.Collections.Generic;
using TMPro;
public class SyncNewClientRequest : Request
{
    
    public static SyncNewClientRequest Instance;
    public static int count = 0;
    public SyncNewClientRequest()
    {
        Instance = this;
    }
    private void Start() {
        this.OpCode = Common.OperationCode.SyncNewClient;
        PhotonClient.Instance.AddRequest(this);
    }
    public override void DefaultRequest(GameObject go)
    {
        Dictionary<byte, object> data = new Dictionary<byte, object>();
        data.Add((byte)ParameterCode.UserName, "User1");
        data.Add((byte)ParameterCode.Password, "12345678");
        PhotonClient.Peer.OpCustom((byte)OpCode, data, false);
        data.Clear();
    }

    public override void OnOperationResponse(OperationResponse response)
    {
        count = (int)DictTool.GetValue(response.Parameters, (byte)ParameterCode.ClientCount);
        if (count != 0)
        {
            Debug.Log("count = "+count);
            SyncNewPlayerRequest.Instance.DefaultRequest(null);
        }
    }

}

