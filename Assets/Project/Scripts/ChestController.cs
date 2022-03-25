using UnityEngine;
public class ChestController : MonoBehaviour
{
    public HealthBar healthBar;
    public int maxHealth = 3;
    public GameObject[] treasures;
    public float raiseTreasure = 0.15f;
    private int currentHealth;
    private Animator anim;
    private bool open = false;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        anim = GetComponentInChildren<Animator>();
    }

    public void InflictDamage(int damage)
    {
         if (open) return;
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
        if (currentHealth <= 0)
        {
            Open();
        }
    }

    private void Open()
    {
       anim.SetTrigger("OPEN");
         open = true;

       foreach (GameObject t in treasures)
        {
            AwardableController ac = null;

            t.TryGetComponent<AwardableController>(out ac);

            if (ac != null)
            {
                ac.Display(new Vector3(0, raiseTreasure, 0));
                ac.SetCanBeClaimed();
            }
        }
    }
}
