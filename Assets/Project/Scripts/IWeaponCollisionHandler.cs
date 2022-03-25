using UnityEngine;
public interface IWeaponCollisionHandler
{
    void OnWeaponCollision(GameObject weapon, Collision collision);
}
