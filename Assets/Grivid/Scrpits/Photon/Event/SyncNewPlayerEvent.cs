using ExitGames.Client.Photon;
using Common;
using UnityEngine;
using System.IO;
using System.Xml.Serialization;
class SyncNewPlayerEvent : BaseEvent
{
    private void Start() {
        this.EventCode = Common.EventCode.SyncNewPlayer;
        PhotonClient.Instance.AddEvent(this);
    }
    public override void OnEvent(EventData eventData)
    {
    }
}

