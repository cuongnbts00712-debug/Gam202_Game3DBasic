using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public GameObject weapon;

    public void EnableWeaponCollider(int isEnable)
    {
        if (weapon != null)
        {
            Collider col = weapon.GetComponent<Collider>();
            
            if (col != null)
            {
                col.enabled = isEnable == 1; // Bật hoặc tắt collider dựa trên giá trị isEnable
            }
        }
    }
}
