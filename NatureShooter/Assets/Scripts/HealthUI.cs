using UnityEngine;

public class HealthUI : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private float MaxHealth; 
    public float Width, Height;

    [SerializeField]
    private RectTransform healthBar;

    public void SetMaxHealth(float maxHealth)
    {
        MaxHealth = maxHealth;
    }

    public void SetHealth(float health)
    {
        float healthpercentage = health / MaxHealth;
        healthBar.localScale = new Vector3(healthpercentage, 1f, 1);
    }
}
