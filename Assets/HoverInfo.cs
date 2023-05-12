using UnityEngine;

public class HoverInfo : MonoBehaviour
{
    public string objectInfo;
    
    void Start()
    {
        objectInfo = "" + gameObject.name;
    }
}