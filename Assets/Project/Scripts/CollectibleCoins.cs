using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleCoins : MonoBehaviour
{
    int coins = 0;
  
    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Coin"))
        {
            Debug.Log("Coin collected" + coins);
            coins = coins + 1;
            // Col.gameObject.SetActive(false);
            Destroy(other.gameObject);
        }
    }


}
