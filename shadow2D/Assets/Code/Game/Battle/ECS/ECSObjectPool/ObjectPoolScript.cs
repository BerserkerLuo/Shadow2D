using UnityEngine;

//对象池统计脚本
public class ObjectPoolScript : MonoBehaviour
{
    public int TotalObjectCount = 0;
    public int UsedObjectCount = 0;
    public int GetCount = 0;
    public int ReturnCount = 0;
    public int ClearCount = 0;


    public void OnReturn() {
        ReturnCount ++;
        UsedObjectCount --; ;
    }

    public void OnGet() {
        UsedObjectCount ++;
        GetCount ++;
    }

    public void OnAdd() {
        TotalObjectCount ++;
    }
     
    public void Clear()
    {
        ClearCount++;
    }
}


