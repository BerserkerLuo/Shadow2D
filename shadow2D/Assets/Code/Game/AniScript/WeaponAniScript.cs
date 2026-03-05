using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAniScript : StateMachineBehaviour
{
    static Dictionary<int, string> StateMap = new Dictionary<int, string>();
    static WeaponAniScript() {
        StateMap.Add(Animator.StringToHash("AtkLayer.Cut"), "Cut");
        StateMap.Add(Animator.StringToHash("AtkLayer.Cut2"), "Cut2");
        StateMap.Add(Animator.StringToHash("AtkLayer.Poke"), "Poke");
        StateMap.Add(Animator.StringToHash("AtkLayer.Shoot"), "Shoot");
    }

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    //public AtkLayer


    //OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        string paramName = StateMap.GetValueOrDefault(stateInfo.fullPathHash,"");
        if(paramName != "") animator.SetBool(paramName, false);
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
