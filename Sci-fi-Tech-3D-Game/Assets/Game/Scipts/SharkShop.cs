using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkShop : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            Player player = other.GetComponent<Player>();
            UI_Manager uiManager = GameObject.Find("Canvas").GetComponent<UI_Manager>();
            AudioSource audioSource = GetComponent<AudioSource>();
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (player.hasCoin)
                {
                    player.hasCoin = false;
                    uiManager.NoteCollectedCoin();
                    audioSource.Play();
                    player.EnableWeapons();
                }
                else
                {
                    Debug.Log("Get out of here!");
                }
            }

        }
    }
}
