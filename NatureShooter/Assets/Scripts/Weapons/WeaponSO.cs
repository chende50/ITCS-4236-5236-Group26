using UnityEngine;

[CreateAssetMenu()]
public class WeaponSO : ScriptableObject
{
    [Header("Basic Information")]
    [SerializeField] private string weaponName;
    [SerializeField] private string weaponDesc;
    [SerializeField] private GameObject weaponPrefab;
    [SerializeField] private GameObject bulletPrefab;
    // [SerializeField] private bool isMelee; maybe if we have time for fun

    [Header("Statistics")]
    [SerializeField] private float damage;
    [SerializeField] private float fireRate;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private float range;

    public GameObject GetBulletPrefab()
    {
        return bulletPrefab;
    }

    public float GetDamage()
    {
        return damage;
    }

    public float GetProjectileSpeed()
    {
        return projectileSpeed;
    }

    public float GetRange()
    {
        return range;
    }
}
