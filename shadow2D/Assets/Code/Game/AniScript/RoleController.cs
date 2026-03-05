
using ECS;
using System.Collections;
using UnityEngine;
using DG.Tweening;

public class RoleController : MonoBehaviour
{
    #region
    static Material nomal = null;
    static Material hit = null;
 
    static void TryLoad() {
        if (nomal != null)
            return;
        nomal = Resources.Load<Material>("Prefabs/Common/SpriteNomal");
        hit = Resources.Load<Material>("Prefabs/Common/SpriteHit2");
    }
    #endregion

    protected Animator animator;
    protected SpriteRenderer sp;

    void Awake()
    {
        Init();
    }

    public void Init() {

        TryLoad();

        animator = transform.GetComponent<Animator>();
        sp = transform.GetComponent<SpriteRenderer>();
    }

    public void OnBron() { 
        animator.SetBool("IsDead", false);
    }
    public void OnShowMoveAnimation(float speed)
    {
        animator.SetFloat("Speed", speed);
    }

    public void OnShowDeadAnimation() {
        animator.SetBool("IsDead", true);
    }

    public void SetTargetDire(Vector2 dire) {
        animator.SetFloat("TarDX", dire.x);
        animator.SetFloat("TarDY", dire.y);
    }

    public void SetMoveDire(Vector2 dire) {
        animator.SetFloat("MoveDX", dire.x);
        animator.SetFloat("MoveDY", dire.y);
    }

    //=============================================================================
    public virtual void OnShowAtkAnimation(Vector2 dire){
        Sequence seq1 = DOTween.Sequence();
        seq1.Append(transform.DOLocalMove(dire.normalized * 0.5f, 0.1f).SetEase(Ease.OutExpo).OnComplete(() => {
            OnAtkFrame();
        }));
        seq1.Append(transform.DOLocalMove(Vector2.zero, 0.3f).SetEase(Ease.OutBack));
    }

    public void OnAtkFrame()
    {
        OwnerScript script = transform.parent.GetComponent<OwnerScript>();
        if (script == null)
            return;

        SkillUtils.OnAtkFrame(script.Owner);
    }

    //=============================================================================
    public float StartTime = 0;
    public void OnHit() {

        if (Time.time - StartTime < 0.15f)
            return;

        StartTime = Time.time;
        StartCoroutine(DoHit());
    }

    private IEnumerator DoHit() 
    {
        sp.material = hit;
        yield return new WaitForSeconds(0.1f);
        sp.material = nomal;
    }

    //=============================================================================

}
