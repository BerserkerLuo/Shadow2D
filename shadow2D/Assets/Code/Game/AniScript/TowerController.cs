using System.Collections;
using UnityEngine;

public class TowerController : RoleController
{
    public Transform WeaponTrans;

    public void Awake(){
        Init();
    }
    public override void OnShowAtkAnimation(Vector2 dire){
        animator.SetBool("IsMeleeAtk", true);

        float zAngle = Mathf.Atan2(dire.y, dire.x) * (180 / Mathf.PI);
        WeaponTrans.eulerAngles = new Vector3(0, 0, zAngle);
    }

}
