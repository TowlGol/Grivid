using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class faceTo : MonoBehaviour {
    private enum Mode {
        LookAt,
        LookAtInverted,
        CameraForward,
        CameraBackWard
    }
    [SerializeField] private Mode mode = Mode.CameraForward;
    public float toward = 1f;
    //public GameObject tmp;
    private void LateUpdate() {
        switch (mode) {
            case Mode.LookAt:
                transform.LookAt(Camera.main.transform);
                transform.localEulerAngles *= toward;
                break;
            case Mode.LookAtInverted:
                Quaternion q = Quaternion.identity;
                q.SetLookRotation(Camera.main.transform.position  , this.transform.up);
                transform.localEulerAngles = Quaternion.ToEulerAngles(q) * toward;
                break;
            case Mode.CameraForward:
                transform.forward = (transform.position  - Camera.main.transform.position)*(1-toward) +(Camera.main.transform.forward)*toward;
                //transform.forward = (transform.position  - tmp.transform.position)*(1-toward) +(tmp.transform.forward)*toward;
                break;
            case Mode.CameraBackWard:
                transform.forward = -Camera.main.transform.forward * toward;
                break;
        }
    }

}
