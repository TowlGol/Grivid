using ExitGames.Client.Photon;
using UnityEngine;

class SyncOffLineRequest : Request
{
    public static SyncOffLineRequest Instance;

    public SyncOffLineRequest()
    {
        Instance = this;
    }
    private void Start() {
        this.OpCode = Common.OperationCode.SyncOffLine;
        PhotonClient.Instance.AddRequest(this);
    }
    public override void DefaultRequest(GameObject go)
    {
            PhotonClient.Peer.OpCustom((byte)OpCode, null, true);
    }

    public override void OnOperationResponse(OperationResponse response)
    {
    }
}

