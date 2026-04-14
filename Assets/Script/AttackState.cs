using UnityEngine;

public class AttackState : StateMachineBehaviour
{
    // Lưu Transform của Player để kiểm tra khoảng cách tấn công
    Transform player;


    // Khoảng cách tối đa để enemy có thể tấn
    // công
    public float attackDistance = 2.5f;
    public float distance;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Tìm Player theo tag và lấy Transform
        player = GameObject.FindGameObjectWithTag("Player").transform;

        animator.SetBool("isChasing", false);
    }


    // Cập nhật mỗi frame
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Kiểm tra player đã bị destroy chưa
        if (player == null) return;       // biến null
        if (!player) return;              // Unity Object bị Destroy (MissingReference)


        // Tính khoảng cách
        float distance = Vector3.Distance(animator.transform.position, player.position);


        // Nếu Player ở xa, dừng tấn công
        if (distance > attackDistance)
        {
            animator.SetBool("isAttacking", false);
        }
        else
        {
            // Nếu gần, bật tấn công
            animator.SetBool("isAttacking", true);
        }
    }


    // Nếu trạng thái kết thúc
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Reset trigger/flag tấn công để tránh lỗi animation
        animator.SetBool("isAttacking", false);
    }

}
