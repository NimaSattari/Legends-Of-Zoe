using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedPU : MonoBehaviour
{
    GameObject player;
    PlayerController playerController;
    void Start()
    {
        player = GameManager.instance.Player;
        playerController = player.GetComponent<PlayerController>();
        GameManager.instance.RegisterPowerUp();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == player)
        {
            playerController.SpeedPowerUp();
            Destroy(gameObject);
        }
    }
}
