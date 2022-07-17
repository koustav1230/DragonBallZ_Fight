using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("AI MOVEMENT")]
    [SerializeField] Transform[] WayPoints;
    [SerializeField] float MovingSpeed = 1f;
    [SerializeField] int IndexPoint = 0;

    [Header("SOUND")]
    public AudioSource source;
    public AudioClip beamsound;

    [Header("ANIMATION")]
    public Animator beamthrow;

    [Header("AI COOLINGDOWN")]
    float BeamThrowTime;
    public float BeamCoolDown;
    float VegetaSupermodeTime;
    public float SuperModeCD;

    [Header("OTHER REFERENCES")]
    public PlayerController enemy;
    GameObject kibeamVeg;
    Vector2 pos;

    void Start()
    {
        BeamThrowTime = 0;    
    }
    void Update()
    {
        Move();
        EnemyCoolingDown();
    }

    private void EnemyCoolingDown()
    {
        if (enemy.CurrentEnemyMP >= 50)
        {
            BeamThrowTime += Time.deltaTime;
            VegetaSupermodeTime += Time.deltaTime;
            if (BeamThrowTime >= BeamCoolDown)
            {
                source.PlayOneShot(beamsound);
                beamthrow.SetTrigger("Vegbeam");
                kibeamVeg = Objectpooling.instance.GetBeamObject2();
                pos = this.transform.position;

                pos.y += .5f;
                pos.x -= 1f;
                kibeamVeg.transform.position = pos;
                kibeamVeg.SetActive(true);
                BeamThrowTime = 0;
            }
            if (VegetaSupermodeTime >= SuperModeCD)
            {
                enemy.CurrentEnemyMP = 0;
                VegetaSupermodeTime = 0;
            }
        }
    }


    void Move()
    {
        transform.position = Vector2.MoveTowards(transform.position, WayPoints[IndexPoint].transform.position, Time.deltaTime * MovingSpeed);

        if (transform.position == WayPoints[IndexPoint].transform.position)
        {
            IndexPoint++;
        }
        if (IndexPoint == WayPoints.Length)
        {
            IndexPoint = 0;
        }
    }
}
