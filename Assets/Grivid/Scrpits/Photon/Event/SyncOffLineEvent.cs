using ExitGames.Client.Photon;
using System.Collections.Generic;
using Common;
using System.Xml.Serialization;
using System.IO;
using UnityEngine;
public class SyncOffLineEvent : BaseEvent
{

    private void Start() {
        this.EventCode = Common.EventCode.SyncOffLine;
        PhotonClient.Instance.AddEvent(this);
    }
    public override void OnEvent(EventData eventData)
    {
    }
}
