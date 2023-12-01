using Common;
using UnityEngine;
using ExitGames.Client.Photon;

public abstract class Request : MonoBehaviour
{
    public OperationCode OpCode;

    public abstract void DefaultRequest(GameObject go);
    public abstract void OnOperationResponse(OperationResponse response);

    public virtual void Start()
    {
        PhotonClient.Instance.AddRequest(this);
    }

    public void OnDestroy()
    {
        PhotonClient.Instance.RemoveRequest(this);
    }
}