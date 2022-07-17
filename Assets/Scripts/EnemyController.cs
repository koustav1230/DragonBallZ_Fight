using UnityEngine;

public class EnemyController : MonoBehaviour
{



    [Header("AudioClips")]
    public AudioSource VegetaSource;
    public AudioClip kick;
    public AudioClip Kick2;
    public AudioClip Punch;

    [Header("Animations")]
    public Animator Enemyanimation;

    [Header("EnemyController")]
    public float EnemySpeed = 4f;
    public Transform attackPoint;

    [Header("PlayerScriptController")]
    public PlayerController Player;

    private Rigidbody2D EnemyBody;
    private Transform target;
    public float AttactDistance=1f;
    private float CurrentAttckTime;
    private float DefaultAttckTime = 0.30f;
    private bool Isattacking;
    int Attackpattern;
    Collider2D col;
    PolygonCollider2D col2;
    int jumpPattern;
    public ParticleSystem VegHitParticle;
    void Awake()
    {
        col2 = GetComponent<PolygonCollider2D>();
        EnemyBody = GetComponent<Rigidbody2D>();
        target = GameObject.FindWithTag("AttckPoint").transform;
    }

    void Start()
    {
        CurrentAttckTime = DefaultAttckTime;
        
    }

    
    void Update()
    {
        EnemyCombo();

    }

    void EnemyCombo()
    {
        if (!Isattacking)
        {
            return;
        }
        if (CurrentAttckTime <= DefaultAttckTime)
        {
            CurrentAttckTime += Time.deltaTime;
        }
        else
        {
            Attackpattern = Random.Range(0, 5);
            EnemeyAttackAnimation(Attackpattern);
            CurrentAttckTime = 0;
        }
    }

    void FixedUpdate()
    {
        FollowThePlayer();
    }

    public void EnemyHit()
    {
        Enemyanimation.SetTrigger("Hit");
    }



    void FollowThePlayer()
    {
        col2.isTrigger = true;
        EnemyBody.bodyType = RigidbodyType2D.Kinematic;
        if (!Player.isGrounded)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(attackPoint.position.x, transform.position.y), Time.deltaTime * EnemySpeed);
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, attackPoint.position, Time.deltaTime * EnemySpeed);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "AttckPoint")
        {
            Isattacking = true;
            print(collision.gameObject.tag);
            

        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        
        if (collision.gameObject.tag == "AttckPoint")
        {
            Isattacking = false;
        }
    }


    void EnemeyAttackAnimation(int Attack)
    {
        if (Attack == 0)
        {
            VegHitParticle.Play();
            Enemyanimation.SetTrigger("punch1");
            Player.CurrentEnemyMP += 10;
        }
        if (Attack == 1)
        {
            VegHitParticle.Play();
            Enemyanimation.SetTrigger("punch2");
            VegetaSource.PlayOneShot(Punch);
            Player.CurrentEnemyMP += 10;
        }
        if (Attack == 3)
        {
            VegHitParticle.Play();
            Enemyanimation.SetTrigger("kick1");
            VegetaSource.PlayOneShot(kick);
            Player.CurrentEnemyMP += 10;
        }
        if (Attack == 4)
        {
            VegHitParticle.Play();
            Enemyanimation.SetTrigger("kick2");
            VegetaSource.PlayOneShot(Kick2);
            Player.CurrentEnemyMP += 10;
        }
        
    
    }

}
