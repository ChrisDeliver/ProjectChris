using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HPSystem : MonoBehaviour
{
    [SerializeField, Range(1, 100)] private sbyte maxHealth = 100;
    [SerializeField] private GameObject healthTextGameObject;

    private TextMeshProUGUI healthText;
    private sbyte currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
        healthText = healthTextGameObject.GetComponent<TextMeshProUGUI>();
    }

    public void TakeDamage(sbyte damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        SceneManager.LoadScene(0);
    }

    private void Update()
    {
        healthText.text = $"{currentHealth}%";
    }

    public void Heal(sbyte amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }
}