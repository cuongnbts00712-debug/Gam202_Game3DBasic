using UnityEditor.Rendering;
using UnityEngine;

public class WalkState : StateMachineBehaviour
{
    float time;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        time = 0;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        time += Time.deltaTime;

        if (time > 5)
            animator.SetBool("IsPatrolling", false);
    }
}
