using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;
using UnityEngine.Playables;
public enum combosate
{
    NONE,
    Punch1,
    Punch2,
    Kick1,
    kick2
}

public class PlayerController : MonoBehaviour
{
    [Header("MOVEMENT CONTROL")]
    public float speed;
    public float horizontalValues;
    public float jumpForce;

    [Header("AUDIO CLIPS AND SOURCE")]
    public AudioSource source;
    public AudioClip Jump;
    public AudioClip Kick;
    public AudioClip CloseKick;
    public AudioClip Beam;
    public AudioClip SaiyanTransfrom;
    public AudioClip SuperKameHameHaa;

    [Header("GROUND CHECKER")]
    public Rigidbody2D rb2d;
    public LayerMask groundLayer;
    public Transform groundCheckpos;
    public bool isGrounded;


    [Header("ANIMATION CONTROLLER")]
    public Animator anime;

    [Header("TIMER CONTROLLER")]
    bool OnCD;
    float CoolingTime;
    [SerializeField] float CDTimer = 10f;
    public float timer;
    public int minute;
    public int second;
    [SerializeField] float TimerForSuperPower;

    [Header("UI HANDLER")]
    public TextMeshProUGUI timertext;
    public HealthUI PlayerHP;
    public HealthUI EnemyHP;
    public HealthUI PlayerMP;
    public HealthUI EnemyMP;
    public HealthUI PlayerKAI;

    [Header("HP,MP and KAI Controller")]
    public float MaxHp;
    public float CurrentHp;
    public float HelathReducingSpeed;
    public float EHelathReducingSpeed;
    public float EnemyMaxHP;
    public float CurrentEnemyHP;
    public float InititalPyerMP;
    public float CurrentPlayerMP;
    public float CurrentEnemyMP;
    public float PlayerCurrentKAI;


    [Header("RANGE CONTROLLER")]
    [SerializeField] float AttackRange = 4f;
    [SerializeField] float ChaseRange = 8f;
    public float maxScreen = 2f;
    public float minScreen = 2f;

    [Header("OTHER REFERENCE")]
    public EnemyController EnemyHitAnimation;
    public BeamControl GetBeamHitValues;
    public GameObject Beamm;
    public GameObject enemy;
    GameObject KiBeam;
    float PlayerToEnenmyDis;

    [Header("OTHERS")]
    public bool Isblocking;
    public bool IsSuperSaiyan;

    //COMBOCONTROLLER
    combosate CurrentComboState;
    bool RestingTime;
    float currentComboTime;
    float defaultComboTime = 0.7f;


    public PlayableDirector pd;
    public ParticleSystem HitPaticle;
    public ParticleSystem supersaiyanParticle;
    public ParticleSystem SuperSaiyanAura;
    
    int supersaiyanCheck;
    public static bool SupersaiyanDamage;
    public static bool ActivatingNormalShooting;
    void Start()
    {
        SuperSaiyanAura.Stop();
        supersaiyanParticle.Stop();
        HitPaticle.Stop();
         

        //Super sayian check
        IsSuperSaiyan = false;
        
        //playerhp  
        CurrentHp = MaxHp;
        PlayerHP.MaxHealth(MaxHp);

        //playermp
        CurrentPlayerMP = InititalPyerMP;
        PlayerMP.MinHealth(InititalPyerMP);

        //Playerkai
        PlayerCurrentKAI = InititalPyerMP;
        PlayerKAI.MinHealth(InititalPyerMP);
        

        //enemy
        CurrentEnemyHP = EnemyMaxHP;
        EnemyHP.MaxHealth(EnemyMaxHP);


        //enemymp
        CurrentEnemyMP = InititalPyerMP;
        EnemyMP.MinHealth(InititalPyerMP);

        //combo handling
        currentComboTime = defaultComboTime;
        CurrentComboState=combosate.NONE;

        //Timer/cooldown
        CoolingTime = 0;
        TimerForSuperPower = Time.time;
        
        //audio source
        source = gameObject.AddComponent<AudioSource>();


        //IsKicking = false;

        supersaiyanCheck = 0;

        AudioListener.volume = menuCotroller.volm;


        //normalbeam
        ActivatingNormalShooting = false;

        //kamehame
        SupersaiyanDamage = false;

       
    } 

   
    void Update()
    {

        InputHandler();
        HpAndMpController();
        EnemyScriptController();
        InGameTimer();
        ScreenRange();
        jump();
        animationcontroller();
        RestingTimeForComBo();
        SuperSaiyanMode();
        Blocking();
        SaiyanAlert();
        KaiHandler();
    }

    public void KaiHandler()
    {
        if (!IsSuperSaiyan)
        {
            if (Input.GetKey(KeyCode.R))
            {
                
                anime.SetTrigger("Kaicharge");
                PlayerCurrentKAI += Time.deltaTime * 10;
                PlayerKAI.HealthController(PlayerCurrentKAI);
            }
            if (PlayerCurrentKAI >= 5)
            {

                ActivatingNormalShooting = true;
            }
            if (PlayerCurrentKAI <= 4)
            {
                ActivatingNormalShooting = false;
            }
            if (PlayerCurrentKAI <= 0)
            {
                ActivatingNormalShooting = false;
            }
            if (PlayerCurrentKAI >= 50)
            {
                PlayerCurrentKAI = 50;
            }
            else
            {
                
            }
        }
    }





    void SaiyanAlert()
    {
        if (CurrentPlayerMP >= 50)
        {
            supersaiyanCheck++;
        }
        if (supersaiyanCheck ==1)
        {
            pd.Play();
        }


    }

    void InputHandler()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheckpos.position, 0.1f, groundLayer);
        horizontalValues = Input.GetAxis("Horizontal");
    }

    public void HpAndMpController()
    {
        PlayerHP.HealthController(CurrentHp);
        PlayerMP.HealthController(CurrentPlayerMP);
        EnemyMP.HealthController(CurrentEnemyMP);
        EnemyHP.HealthController(CurrentEnemyHP);
    }

    void Blocking()
    {
        if (PlayerToEnenmyDis <= 2f)
        {
            if (!Isblocking)
            {
                CurrentHp -= Time.deltaTime * HelathReducingSpeed;
                PlayerHP.HealthController(CurrentHp);
            }
            else
            {
                Isblocking = false;
            }
        }
    }

     void SuperSaiyanMode()
    {
        
        if (OnCD)
        {
            CoolingTime += Time.deltaTime;
        }
        if (CoolingTime >= CDTimer)
        {
            SuperSaiyanAura.Stop();
            supersaiyanParticle.Stop();
            IsSuperSaiyan = false;
            anime.SetTrigger("BackToNormal");
            CurrentPlayerMP = 0;
            PlayerMP.HealthController(CurrentPlayerMP);
        }
    }

    void EnemyScriptController()
    {
        Vector2 enenmyPos = enemy.transform.position;

        PlayerToEnenmyDis = Vector2.Distance(transform.position, enenmyPos);
        if (PlayerToEnenmyDis > ChaseRange)
        {
            enemy.GetComponent<EnemyMovement>().enabled = true;
            enemy.GetComponent<EnemyController>().enabled = false;
        }
        if (PlayerToEnenmyDis <= ChaseRange)
        {
            enemy.GetComponent<EnemyMovement>().enabled = false;
            enemy.GetComponent<EnemyController>().enabled = true;
        }
    }

    void ScreenRange()
    {
        float speedCotrol = horizontalValues * speed * Time.deltaTime;
        float Xposition = transform.position.x + speedCotrol;
        float ofset = Mathf.Clamp(Xposition, minScreen, maxScreen);
        transform.position = new Vector2(ofset, transform.position.y);
    }

    void InGameTimer()
    {
        timer = Time.time - TimerForSuperPower;
        minute = ((int)timer / 60);
        second = ((int)timer % 60);
        timertext.text = ($"{minute}:{second}");
    }

    public void animationcontroller()
    {
        if(Input.GetKeyDown(KeyCode.Q) && CurrentPlayerMP>=50)
        {
            //pd.Pause();
            OnCD = true;
            IsSuperSaiyan=true;
            source.PlayOneShot(SaiyanTransfrom);
            anime.SetTrigger("SuperSaiyan");
            supersaiyanParticle.Play();
            SuperSaiyanAura.Play();
        }
        if(Input.GetKeyDown(KeyCode.E))
        {

            if (IsSuperSaiyan)
            {
                //pd.Resume();
                Invoke("SuperKameHameHa", 2.5f);
                source.PlayOneShot(SuperKameHameHaa);
                anime.SetTrigger("Kamehameha");
                SupersaiyanDamage = true;

            }
            if (!IsSuperSaiyan && ActivatingNormalShooting)
            {
                ChargeBeams();
                source.PlayOneShot(Beam);
                anime.SetTrigger("Beam");
                SupersaiyanDamage = false;
                PlayerCurrentKAI -= 5;
                PlayerKAI.HealthController(PlayerCurrentKAI);

            }

        }
        else if(Input.GetKey(KeyCode.A))
        {
            anime.SetBool("Isrunning",true);
        }
        else if(Input.GetKey(KeyCode.D))
        {
            anime.SetBool("Isrunning", true);
        }
        else if(Input.GetMouseButtonDown(1))
        {
           

            if (CurrentComboState == combosate.Punch2 ||
                CurrentComboState == combosate.Kick1 ||
                CurrentComboState == combosate.kick2)
            {
                return;
            }

            if (IsSuperSaiyan)
            {
                anime.SetTrigger("SSPunch");
            }
            HandCombo();
        }
        else if(Input.GetMouseButtonDown(0))
        {
            
            if (CurrentComboState == combosate.kick2 ||
                CurrentComboState == combosate.Punch2)
            {
                return;
            }
            if(IsSuperSaiyan)
            {
                anime.SetTrigger("SSKIck");
            }

            LegCombo();
        }
        else if(Input.GetKey(KeyCode.LeftShift))
        {
            Isblocking = true;
            if (!IsSuperSaiyan)
            {
                anime.SetTrigger("block");
            }
            else
            {
                anime.SetTrigger("Superblock");
            }
        }
        else if(CoolingTime>=CDTimer)
        {
            OnCD = false;
            CoolingTime = 0;

        }
        else
        {
            
            anime.SetBool("Isrunning", false);
        }      

    }

    private void ChargeBeams()
    {
        pd.Resume();
        Vector2 pos;
        KiBeam = Objectpooling.instance.GetBeamObject();
        pos = this.transform.position;
        pos.x += 1f;
        pos.y += 0.5f;
        KiBeam.transform.localScale = new Vector2(3f, 3f);
        KiBeam.transform.position = pos;
        KiBeam.SetActive(true);
    }

    public void SuperKameHameHa()
    {
        Vector2 pos;
        KiBeam = Objectpooling.instance.GetBeamObject();
        pos = this.transform.position;
        pos.x += 1f;
        pos.y += 0.5f;
        KiBeam.transform.localScale = new Vector2(10f, 10f);
        KiBeam.transform.position = pos;
        KiBeam.SetActive(true);
    }

    public void LegCombo()
    {
        if (CurrentComboState == combosate.NONE ||
            CurrentComboState == combosate.Punch1)
        {
            CurrentComboState = combosate.Kick1;
        }
        else if (CurrentComboState == combosate.Kick1)
        {
            CurrentComboState++;
        }

        RestingTime = true;
        currentComboTime = defaultComboTime;

        if (CurrentComboState == combosate.Kick1)
        {
            
            anime.SetTrigger("Kick1");
            if(PlayerToEnenmyDis>2f)
            {

                source.PlayOneShot(Kick);
            }
            else if(PlayerToEnenmyDis<=2f)
            {
                
                HitPaticle.Play();
                CurrentPlayerMP += 5;
                PlayerMP.HealthController(CurrentPlayerMP);
                source.PlayOneShot(CloseKick);
                EnemyHitAnimation.EnemyHit();
                CurrentEnemyHP-=Time.deltaTime * EHelathReducingSpeed;
                EnemyHP.HealthController(CurrentEnemyHP);
            }
            
        }
        if (CurrentComboState == combosate.kick2)
        {
            
            anime.SetTrigger("Kick2");
            if (PlayerToEnenmyDis <= 2f)
            {
                HitPaticle.Play();
                CurrentPlayerMP += 5;
                PlayerMP.HealthController(CurrentPlayerMP);
                CurrentEnemyHP -= Time.deltaTime * EHelathReducingSpeed;
                EnemyHP.HealthController(CurrentEnemyHP);
            }
        }
    }

    public void HandCombo()
    {
        CurrentComboState++;
        RestingTime = true;

        if (CurrentComboState == combosate.Punch1)
        {
            anime.SetTrigger("Punch1");
            if(PlayerToEnenmyDis>2)
            {
                source.PlayOneShot(Kick);
            }
            if (PlayerToEnenmyDis <= 2f)
            {
                HitPaticle.Play();
                CurrentPlayerMP += 2.5f;
                PlayerMP.HealthController(CurrentPlayerMP);
                CurrentEnemyHP -= Time.deltaTime * EHelathReducingSpeed;
                EnemyHP.HealthController(CurrentEnemyHP);
            }
        }
        if (CurrentComboState == combosate.Punch2)
        {
            anime.SetTrigger("Punch2");
            if (PlayerToEnenmyDis > 2)
            {
                source.PlayOneShot(Kick);
            }
            if (PlayerToEnenmyDis <= 2f)
            {
                HitPaticle.Play();
                CurrentPlayerMP += 2.5f;
                PlayerMP.HealthController(CurrentPlayerMP);
                CurrentEnemyHP -= Time.deltaTime * EHelathReducingSpeed;
                EnemyHP.HealthController(CurrentEnemyHP);
            }
        }
    }


    public void RestingTimeForComBo()
    {
        if(RestingTime)
        {
            currentComboTime -= Time.deltaTime;
            if(currentComboTime<=0)
            {
                CurrentComboState = combosate.NONE;
                RestingTime = false;
                currentComboTime = defaultComboTime;
            }
        }
    }


    public void jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {           
            source.PlayOneShot(Jump);
            if (!IsSuperSaiyan)
            {
                anime.SetTrigger("jump");
            }
            else
            {
                anime.SetTrigger("SaiyanJump");
            }
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpForce);
        }

    }


     void OnDrawGizmos()
    {

        Gizmos.DrawWireSphere(this.transform.position, AttackRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, ChaseRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(enemy.transform.position, 2f);
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(groundCheckpos.position, 0.1f);
        
    }

  

}
