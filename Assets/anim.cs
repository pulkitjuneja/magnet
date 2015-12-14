using UnityEngine;
using System.Collections;

public class anim : StateMachineBehaviour {

	public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (stateInfo.IsName ("absorb")) {
			animator.SetInteger("state",0);
		}
	}
}
