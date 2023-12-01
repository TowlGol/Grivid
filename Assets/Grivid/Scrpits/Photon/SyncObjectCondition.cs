using UnityEngine;


public class SyncObjectCondition : MonoBehaviour
{
    private Vector3 initPosition;
    private Vector3 initRotation;
    private Vector3 initScale;
    private Color initColor;
    private bool initActive;
    public bool flag = false;
    private int TimeS = 0;

    void Start()
    {
        initPosition = transform.localPosition;
        initRotation = transform.localEulerAngles;
        initScale = transform.localScale;
        initActive = true;
    }

    void Update()
    {
        if ((transform.localPosition != initPosition
            || transform.localEulerAngles != initRotation
            || transform.localScale != initScale
            || initActive != isActiveAndEnabled) && flag)
        {
            initPosition = transform.localPosition;
            initRotation = transform.localEulerAngles;
            initScale = transform.localScale;
            if (flag && TimeS++ % 10 == 0 || initActive != isActiveAndEnabled)
            {
                try
                {
                    SyncTransformRequest.Instance.DefaultRequest(GameObject.Find(this.name));
                }
                catch
                {
                    SyncTransformRequest.Instance.DefaultRequest(GameObject.Find("assemblyPart").transform.Find(this.name).gameObject);
                }
                TimeS = 0;
            }
            else
                flag = true;
            initActive = isActiveAndEnabled;
        }
        else
        {
            flag = true;
            initPosition = transform.localPosition;
            initRotation = transform.localEulerAngles;
            initScale = transform.localScale;
        }

    }
    public void OnDisable()
    {

    }
}
