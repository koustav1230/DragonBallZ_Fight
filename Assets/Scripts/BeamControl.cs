using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamControl : MonoBehaviour
{

    public float BeamSpeed;
    PlayerController pc;

    void Start()
    {
        pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    void Update()
    {
        float speed=BeamSpeed*Time.deltaTime;
        transform.Translate(speed, 0, 0);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {   
        if(collision.gameObject.tag == "enemy")
        {
            
            gameObject.SetActive(false);

            if(PlayerController.SupersaiyanDamage)
            {
                pc.CurrentEnemyHP -= 20;
            }
            if (!PlayerController.SupersaiyanDamage)
            {
                pc.CurrentEnemyHP -= 5;
            }
        }
       
    }
}
