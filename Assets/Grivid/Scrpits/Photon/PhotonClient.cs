using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;
using Common;
using System.Threading;
#region 脚本用途
/// <summary>
/// SyncOffLineRequest
/// @ 创建人：刘亚鹏
/// @ 创建时间：2022/6/14 8:42:15
/// @ 作用:
///     管理、创建和服务器的链接
///     
///     
///     
/// </summary>
#endregion
public class PhotonClient : MonoBehaviour, IPhotonPeerListener
{
    public static PhotonClient Instance;

    private static PhotonPeer peer;
   
    private Dictionary<OperationCode, Request> RequestDict = new Dictionary<OperationCode, Request>();
    
    private Dictionary<EventCode, BaseEvent> EventDict = new Dictionary<EventCode, BaseEvent>();

    public string Address;


    public static PhotonPeer Peer
    {
        get
        {
            return peer;
        }
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
    }

    void Start()
    {
        peer = new PhotonPeer(this, ConnectionProtocol.Udp);
        peer.Connect(Address+":5055", "SceneServer");
        Application.runInBackground = true;
    }

    void Update()
    {

        if(peer.PeerState != PeerStateValue.Connected)
            Debug.Log("状态 "+peer.PeerState);
        peer.Service();
    }


    public void DebugReturn(DebugLevel level, string message){}

    public void OnEvent(EventData eventData)
    {
         //Debug.Log(eventData); 
         EventCode code = (EventCode)eventData.Code;
        BaseEvent e = DictTool.GetValue(EventDict, code);
        try {
            e.OnEvent(eventData);
        }
        catch {
            Debug.LogError("Event "+ eventData.Code+" Not Exits");
        }
        
    }

    public void OnOperationResponse(OperationResponse operationResponse)
    {
        OperationCode opCode = (OperationCode)operationResponse.OperationCode;
        Request request = DictTool.GetValue(RequestDict, opCode);
        request.OnOperationResponse(operationResponse);
        
    }
    public void OnStatusChanged(StatusCode statusCode)
    {
        Debug.Log(statusCode);
        if(statusCode == StatusCode.Connect)
        {
            Debug.Log("Conneted");
                //SyncNewClientRequest.Instance.DefaultRequest(null);
                //SyncNewPlayerRequest.Instance.DefaultRequest(null);
        }
        else if(statusCode == StatusCode.Disconnect)
        {
            while(peer.PeerState == PeerStateValue.Disconnected)
            {
                peer.Connect(Address + ":5055", "SceneServer");
                //Thread.Sleep(1000);
            }
                
        }
    }
    public void OnPlayerDisconnected()
    {
        SyncOffLineRequest.Instance.DefaultRequest(null);
    }

    public void AddRequest(Request request)
    {
        RequestDict.Add(request.OpCode, request);
    }

    public void RemoveRequest(Request request)
    {
        RequestDict.Remove(request.OpCode);
    }

    public void AddEvent(BaseEvent e)
    {
        EventDict.Add(e.EventCode, e);
    }

    public void RemoveEvent(BaseEvent e)
    {
        EventDict.Remove(e.EventCode);
    }

    public void InitAddress(string address) {
        PhotonClient.Instance.Address = address;
    }
}
