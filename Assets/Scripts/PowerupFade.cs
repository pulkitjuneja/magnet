using UnityEngine;
using System.Collections;

public class PowerupFade : StateMachineBehaviour {

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.IsName("Fade"))
        {
            GameObject.Destroy(animator.gameObject);
        }
    }
}
