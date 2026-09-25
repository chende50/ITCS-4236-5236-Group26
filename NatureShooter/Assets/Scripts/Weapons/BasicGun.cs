using UnityEngine;

public class BasicGun : MonoBehaviour
{
    
    [SerializeField] private WeaponSO weaponSO;
    [SerializeField] private Transform bulletPoint;

    public WeaponSO GetWeaponSO()
    {
        return weaponSO;
    }

    public Transform GetBulletPoint()
    {
        return bulletPoint;
    }

}
