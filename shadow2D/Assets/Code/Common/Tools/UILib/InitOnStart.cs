using UnityEngine;
using System.Collections;
using UnityEngine.UI;


using UILib;

[RequireComponent(typeof(XUIAdaptList))]
[DisallowMultipleComponent]
public class InitOnStart : MonoBehaviour
{
    public int totalCount = -1;
    public int offset = 0;
    void Start()
    {
        var ls = GetComponent<XUIAdaptList>();
        ls.Init(totalCount, null, offset);
    }
}
