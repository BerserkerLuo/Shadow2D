using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using ECS;
using System;

public class BookController : MonoBehaviour
{
    public Animator _animator;

    public Animator Animator{
        get{
            if (_animator == null)
                _animator = transform.GetComponent<Animator>();

            return _animator;
        }
    }

    public Action FlipLefEndCallback;
    public void OnFlipLeft() {Animator.SetBool("Left",true);}
    public void FlipLeftEnd() {

        Debug.Log("FlipLeftEnd");

        Animator.SetBool("Left", false);
        FlipLefEndCallback?.Invoke();
    }


    public Action FlipRightEndCallback;
    public void OnFlipRight() { Animator.SetBool("Right", true); }
    public void FlipRightEnd()
    {
        Animator.SetBool("Right", false);
        FlipRightEndCallback?.Invoke();
    }



    public Action OpenEndCallback;
    public void OnOpen() { Animator.SetBool("Open", true); }
    public void OpenEnd()
    {
        Animator.SetBool("Open", false);
        OpenEndCallback?.Invoke();
    }


    public Action CloseEndCallback;
    public void OnClose() { Animator.SetBool("Close", true); }
    public void CloseEnd()
    {
        Animator.SetBool("Close", false);
        CloseEndCallback?.Invoke();
    }
}
