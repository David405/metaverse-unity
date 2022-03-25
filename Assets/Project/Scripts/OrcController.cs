using UnityEngine;
public class OrcController : MonoBehaviour
{
    public HealthBar healthBar;
    public int maxHealth = 25;
    public CharacterController controller;
    public Transform groundCheck;
    public LayerMask groundMask;

    private Animator anim;
    private int currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        controller = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
        healthBar.SetMaxHealth(maxHealth);

    }

    public void InflictDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth < 0) currentHealth = 0;
        // Adjust healthbar.
        healthBar.SetHealth(currentHealth);
         if (currentHealth <= 0)
        {
            Die();
        }
    }

   
    private void Die()
    {
        anim.SetTrigger("DIE");
    }
}
