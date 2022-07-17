using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIWinner : MonoBehaviour
{

    [Header("GAMEOBJECT REFRERENCING")]
    public GameObject goku;
    public GameObject vegeta;
    public GameObject bar;
    public GameObject Background1;
    public GameObject Background2;
    public GameObject IfGoku;
    public GameObject IfVgeta;
    public  GameObject IfDraw;

    [Header("OTHER REFRECNCES")]
    public PlayerController controller;

    [Header("STATUS TEXT")]
    public TextMeshProUGUI WhoIsWinner;
    bool isWin;

    public static bool ispause;

    public static float DifficultyMinute;
    public static float DifficultySecond;

    int escapecount;
    void Start()
    {
        ispause = false;
    }
    void Update()
    { 
        Status();
        PauseGame();
    }

    private void Status()
    {
     
        if (controller.second == DifficultySecond && controller.minute == DifficultyMinute)
        {
            goku.SetActive(false);
            vegeta.SetActive(false);
            Background1.SetActive(false);
            Background2.SetActive(true);

            if (controller.CurrentEnemyHP < controller.CurrentHp)
            {
                IfDraw.SetActive(false);
                IfGoku.SetActive(true);
                IfVgeta.SetActive(false);

            }
            if (controller.CurrentEnemyHP > controller.CurrentHp)
            {
                IfDraw.SetActive(false);
                IfGoku.SetActive(false);
                IfVgeta.SetActive(true);
            }
            else if (controller.CurrentEnemyHP == controller.CurrentHp)
            {
                IfGoku.SetActive(false);
                IfVgeta.SetActive(false);
                IfDraw.SetActive(true);

            }
        }
        if(controller.CurrentHp <= 0)
        {
            goku.SetActive(false);
            vegeta.SetActive(false);
            Background1.SetActive(false);
            Background2.SetActive(true);

            IfDraw.SetActive(false);
            IfGoku.SetActive(false);
            IfVgeta.SetActive(true);

        }
        if(controller.CurrentEnemyHP <= 0)
        {
            goku.SetActive(false);
            vegeta.SetActive(false);
            Background1.SetActive(false);
            Background2.SetActive(true);

            IfDraw.SetActive(false);
            IfGoku.SetActive(true);
            IfVgeta.SetActive(false);
        }
        
    }

    public void Retry()
    {
        SceneManager.LoadScene(1);
    }

    public void quit()
    {
        SceneManager.LoadScene(0);
        
    }
  
    void PauseGame()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
           
            SceneManager.LoadScene("menu",LoadSceneMode.Additive);
            ispause = true;
            Time.timeScale = 0;
           
        }
    }

}
