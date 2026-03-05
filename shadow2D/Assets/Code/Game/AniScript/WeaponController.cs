using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using ECS;

public class WeaponController : MonoBehaviour
{
    public bool IsMelee = true;
    public float AniSpeed = 1;
    public Animator animator;

    public Transform targetTran;

    Transform rootTrans;

    Vector2 TargetDire = Vector2.zero;
    Vector2 Forward = Vector2.left;

    Entity owner;
    Entity target;
    bool IsMove = false;

    SpriteRenderer sp;

    void Start()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
        if (sp == null)
            sp = GetComponentInChildren<SpriteRenderer>();
        rootTrans = transform.Find("Root");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //TargetDire = targetTran.position - transform.position;
        //SetDireByTargetDire(TargetDire);
        //return;
        //Forward = targetTran.position - transform.position;

        if (target != null && !IsMove){
            TargetDire = LogicUtils.GetPos(target) - LogicUtils.GetPos(owner);
            SetDireByTargetDire(TargetDire);
        }
        else
            SetDireForwardDire(Forward);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            SetTargetDire(targetTran.position - transform.position);
            OnAtk();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            SetTargetDire(Vector2.zero);
        }
    }

    private void SetDireByTargetDire(Vector2 targetDire) {
        float xAngle = 0-Mathf.Atan2(-targetDire.y, -targetDire.x) * (180 / Mathf.PI);

        float yAngle = 180; 
        if (xAngle > 90) {
            yAngle = 0;
            xAngle = 180 - xAngle;
        }
        if (xAngle < -90) {
            yAngle = 0;
            xAngle = -180 - xAngle;
        }

        //if (xAngle > 63 && xAngle < 116)
        //    sp.sortingOrder = 0;
        //else
        //    sp.sortingOrder = 1;

        rootTrans.eulerAngles = new Vector3(0, yAngle, xAngle);
    }

    private void SetDireForwardDire(Vector2 dire) {
        float xAngle = 0 - Mathf.Atan2(-dire.y, -dire.x) * (180 / Mathf.PI);

        //ÉÏ >63 <=116
        //×ó > -63 <=63
        //ÏÂ >-135 <= -45

        if (xAngle > 63 && xAngle < 116) {
            //sp.sortingOrder = 1;
            rootTrans.eulerAngles = new Vector3(0, 180, 0);
            return;
        }

        //sp.sortingOrder = 0;

        if (xAngle > -63 && xAngle <= 63){
            rootTrans.eulerAngles = new Vector3(0, 180, 0);
            return;
        }

        rootTrans.eulerAngles = new Vector3(0, 0, 0);
    }

    public void SetTargetDire(Vector2 dire) {
        animator.SetBool("IsBattle", dire != Vector2.zero);
        TargetDire = dire;
    }

    public void SetTarget(Entity tar) {
        animator.SetBool("IsBattle", tar != null);
        target = tar;
    }

    public void SetMoveDire(Vector2 dire) {
        Forward = dire;
    }

    public void SetIsMove(bool flag){
        IsMove = flag;
        animator.SetBool("IsMove", flag);
    }

    //====================================================================

    float lastAtkTime = 0;
    public void OnAtk() {
        if (IsMelee) OnMeleeAtk();
        else animator.SetBool("Shoot", true);
    }

    int lastAniId = 0;

    void OnMeleeAtk() {
        if (Time.time - lastAtkTime > 1f || lastAniId >= 3)
            lastAniId = 0;

        lastAniId += 1;
        lastAtkTime = Time.time;

        switch (lastAniId) {
            case 1: animator.SetBool("Cut", true); break;
            case 2: animator.SetBool("Cut2", true); break;
            case 3: animator.SetBool("Poke", true); break;
        }
    }

    //====================================================================
    public void SetOwner(Entity e) {
        owner = e;
    }
    public void OnAtkFrame()
    {
        //Debug.Log("WeaponController OnAtkFrame");

        if(owner != null) SkillUtils.OnAtkFrame(owner);
    }
}
