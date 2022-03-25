using UnityEngine;
public class WeaponController : MonoBehaviour
{

    private void OnCollisionEnter(Collision collision)
    {
       if (collision.collider.tag.Equals("CanBeAttacked"))
        {
            Debug.Log($"Sword hit {collision.collider.name}");

            collision.gameObject.GetComponent<Attackable>().OnInflictDamage(1);

        }
    }
}
