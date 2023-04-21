using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField]
    private AudioClip _coinPickupSound;

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Player player = other.GetComponent<Player>();
                if (player != null)
                {
                    player.hasCoin = true;
                    AudioSource.PlayClipAtPoint(_coinPickupSound, transform.position, 1f);

                    UI_Manager uIManager = GameObject.Find("Canvas").GetComponent<UI_Manager>();
                    if(uIManager != null)
                    {
                        uIManager.CollectedCoin();
                    }

                    Destroy(this.gameObject);
                }
            }
        }
    }
}
