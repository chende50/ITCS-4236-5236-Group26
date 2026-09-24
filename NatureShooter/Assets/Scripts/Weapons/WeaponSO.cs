using UnityEngine;

[CreateAssetMenu()]
public class WeaponSO : ScriptableObject
{
    [Header("Basic Information")]
    [SerializeField] private string weaponName;
    [SerializeField] private string weaponDesc;
    [SerializeField] private GameObject weaponPrefab;
    // [SerializeField] private bool isMelee; maybe if we have time for fun

    [Header("Statistics")]
    [SerializeField] private float damage;
    [SerializeField] private float fireRate;
    [SerializeField] private float projectileSpeed;
}
