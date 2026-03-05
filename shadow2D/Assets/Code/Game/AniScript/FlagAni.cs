using System.Collections;
using UnityEngine;
using DG.Tweening;

public class FlagAni : MonoBehaviour
{
    void OnEnable()
    {
        transform.localPosition = Vector3.up;
       

        Vector3 originalScale = transform.localScale;
        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOLocalMoveY(0, 0.2f).SetEase(Ease.OutQuad));
        seq.Append(transform.DOScale(originalScale * 1.2f, 0.1f).SetEase(Ease.OutQuad));
        seq.Append(transform.DOScale(originalScale, 0.1f).SetEase(Ease.InQuad));
    }

}
