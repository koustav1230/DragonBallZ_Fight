using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vegbeamcotroller : MonoBehaviour
{
    [Header("BEAM SPEED")]
    public float speedControl;
    PlayerController playerHealth;

    void Start()
    {
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }
    void Update()
    {
        BeamSpeed();
    }

    void BeamSpeed()
    {
        float speed = speedControl * Time.deltaTime;
        transform.Translate(-speed, 0, 0);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag=="Player")
        {
            if (!playerHealth.Isblocking)
            {
                playerHealth.CurrentHp -= 10;
                
            }
            if(playerHealth.Isblocking)
            {
                playerHealth.CurrentHp -= 2.5f;
            }

            gameObject.SetActive(false);
        }
    }
}
