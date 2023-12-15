using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour
{
    private GameObject canmera;
    private void Start() {
        canmera = Camera.main.gameObject;
        if (!GameObject.Find("GrividMove(Clone)"))
            Instantiate(GrividPrefab.Instance.MovePrefab).transform.position = new Vector3(-0.2f,-0.1f,0.5f);
    }
    public void Forward() {
        canmera.transform.Translate(Vector3.forward);
    }
    public void Left() {
        canmera.transform.Translate(Vector3.left);
    }
    public void Right() {
        canmera.transform.Translate(Vector3.right);
    }
    public void Back() {
        canmera.transform.Translate(-Vector3.forward);
    }
    public void Reset() {
        canmera.transform.position = Vector3.zero;
    }

}
