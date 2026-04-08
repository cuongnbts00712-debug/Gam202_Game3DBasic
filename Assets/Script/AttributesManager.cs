using UnityEngine;
using UnityEngine.UI;

public class AttributesManager : MonoBehaviour
{
    public int health; // Máu hiện tại của nhân vật


    public int attack;// Sát thương tấn công cơ bản
    public bool isDead = false; // Biến đánh dấu trạng thái chết của nhân vật

    // ====== ĐÒN CHÍ MẠNG (CRITICAL HIT) ======


    // Hệ số sát thương khi đánh chí mạng
    //  1.5 = damage tăng 150%
    public float critDamage = 1.5f;


    // Tỉ lệ đánh chí mạng
    // 0.5 = 50% khả năng ra đòn chí mạng
    public float critChance = 0.5f;

    // ====== NHẬN SÁT THƯƠNG ======


    // Hàm nhận sát thương
    // amount: lượng damage mà nhân vật bị trừ
    public void TakeDamage(int amount)
    {
        // Trừ máu theo lượng sát thương nhận vào
        health -= amount;

        if (gameObject.CompareTag("Enemy")) // nếu là kẻ thù
        {
            Slider slider = gameObject.transform
                .GetChild(1).transform   // lấy tới canvas
                .GetChild(0).transform   // lấy tới HealthBar
                .GetComponent<Slider>(); // lấy component Slider


            slider.value = health; // cập nhật giá trị slider


            if (health <= 0) // khi máu bằng 0
            {
                EnemyDie();
            }
        }


        // Kiểm tra nếu máu <= 0 thì nhân vật chết
        if (health <= 0)
        {
            // In ra tên GameObject để biết ai đã chết
            EnemyDie();
        }
    }
    void EnemyDie()
    {
        // Nếu enemy đã chết rồi thì thoát hàm ngay, tránh chạy lại nhiều lần
        if (isDead) return;


        // Đánh dấu enemy đã chết
        isDead = true;


        // In log ra Console để debug: tên enemy + trạng thái Dead
        Debug.Log(gameObject.name + " Dead");


        // =======================
        // 0. TẮT AI (RẤT QUAN TRỌNG)
        // =======================


        // Lấy component EnemyController (script điều khiển AI)
        EnemyController enemyController = GetComponent<EnemyController>();


        // Nếu enemy có EnemyController thì tắt script này
        // → Ngăn AI tiếp tục di chuyển / tấn công sau khi chết
        if (enemyController != null)
            enemyController.enabled = false;


        // =======================
        // 1. TẮT NAVMESH AGENT
        // =======================


        // Lấy NavMeshAgent để dừng hệ thống pathfinding
        UnityEngine.AI.NavMeshAgent agent = GetComponent<UnityEngine.AI.NavMeshAgent>();


        // Kiểm tra enemy có dùng NavMeshAgent không
        if (agent != null)
        {
            // Dừng ngay việc di chuyển
            agent.isStopped = true;


            // Tắt hoàn toàn NavMeshAgent để tránh bug giật / trượt
            agent.enabled = false;
        }


        // =======================
        // 2. TẮT CHARACTER CONTROLLER
        // =======================


        // Lấy CharacterController (dùng cho va chạm và di chuyển)
        CharacterController cc = GetComponent<CharacterController>();


        // Nếu có thì tắt để enemy không còn tương tác vật lý
        if (cc != null)
            cc.enabled = false;


        // =======================
        // 3. TẮT COLLIDER
        // =======================


        // Lấy Collider chính của enemy
        Collider col = GetComponent<Collider>();


        // Tắt collider để:
        // - Không bị đánh thêm
        // - Không cản đường player
        if (col != null)
            col.enabled = false;


        // =======================
        // 4. BẬT ANIMATION CHẾT
        // =======================


        // Lấy Animator trong enemy hoặc trong các object con
        Animator animator = GetComponentInChildren<Animator>();


        // Kiểm tra enemy có Animator không
        if (animator != null)
        {
            // Tắt root motion để animation không kéo nhân vật di chuyển
            animator.applyRootMotion = false;


            // Set biến isDead = true để kích hoạt animation chết
            animator.SetBool("isDead", true);
        }


        // =======================
        // 6. HỦY ENEMY
        // =======================


        // Hủy enemy sau 4 giây
        // → Đủ thời gian cho animation chết chạy xong
        Destroy(gameObject, 4f);

    }


    // ====== GÂY SÁT THƯƠNG ======


    // Hàm gây sát thương cho đối tượng khác
    // target: GameObject bị tấn công
    public void DealDamage(GameObject target)
    {
        // Lấy component AttributesManager của target
        // Dùng để truy cập máu và các chỉ số của target
        AttributesManager atm = target.GetComponent<AttributesManager>();


        // Nếu target không có AttributesManager
        // thì không thể gây damage → thoát hàm
        if (atm == null) return;


        // Khởi tạo damage ban đầu bằng attack cơ bản
        float totalDamage = attack;


        // Kiểm tra có ra đòn chí mạng hay không
        // Random.Range(0f, 1f) tạo số ngẫu nhiên từ 0 đến 1
        if (Random.Range(0f, 1f) < critChance)
        {
            // Nếu crit thì nhân damage lên theo hệ số crit
            totalDamage *= critDamage;


            // In log để biết đòn đánh là chí mạng
            Debug.Log("Critical Hit!");
        }


        // Truyền damage cuối cùng (đã tính crit nếu có)
        // Ép kiểu float → int trước khi trừ máu
        atm.TakeDamage((int)totalDamage);
    }

}
