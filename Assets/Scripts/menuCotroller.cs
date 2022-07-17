using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class menuCotroller : MonoBehaviour
{
    // Start is called before the first frame update

    public AudioSource source;
    public AudioClip clip;
    public GameObject menu;
    public GameObject optionmenu;
    public GameObject instructionmenu;
    public GameObject resume;
    public GameObject Sounds;
    public GameObject backmenu;

    public UIWinner uiresume;
    public Slider volumes;
    public static Slider vol;
    public static float volm;
    public Slider Loading;
    public GameObject levelload;
    
    void Start()
    {
        resume.SetActive(false);
        volumes.value = 100;
      

    }


    void Update()
    {
       if(UIWinner.ispause)
        {
            resume.SetActive(true);
        }
    }


    public void LevelLoading(int SceneIndexValue)
    {

       StartCoroutine(LoadingInProgress(SceneIndexValue));
       
    }

    IEnumerator LoadingInProgress(int SceneIndexValue)
    {
        AsyncOperation LoadOperation = SceneManager.LoadSceneAsync(SceneIndexValue);
        while(!LoadOperation.isDone)
        {
            levelload.SetActive(true);
            float processing = Mathf.Clamp01(LoadOperation.progress / .9f);
            Loading.value=processing;
            yield return null;
        }
    }

    public void SetVol()
    {
        AudioListener.volume = volumes.value;
        volm=AudioListener.volume;
    }

   
    public void leveltomenu()
    {
        menu.SetActive(true);
        optionmenu.SetActive(false);
        instructionmenu.SetActive(false);
        Sounds.SetActive(false);
        backmenu.SetActive(false);
    }

    public void play(string level)
    {
       if(level == "hard")
        {
            UIWinner.DifficultyMinute = 0;
            UIWinner.DifficultySecond = 30;
        }
        if (level == "medium")
        {
            UIWinner.DifficultyMinute = 1;
            UIWinner.DifficultySecond = 0;
        }
        if (level == "easy")
        {
            UIWinner.DifficultyMinute = 1;
            UIWinner.DifficultySecond = 30;
        }



    }

    public void Resumes()
    {
        SceneManager.UnloadScene("menu");
        Time.timeScale = 1;
    }

    public void option()
    {
     
        menu.SetActive(false);
        optionmenu.SetActive(true);

    }
    public void back()
    {
        menu.SetActive(true);
        optionmenu.SetActive(false);
    }

    public void soundmenu()
    {
        menu.SetActive(false);
        optionmenu.SetActive(false);
        instructionmenu.SetActive(false);
        Sounds.SetActive(true);

    }
    public void intructionsmenu()
    {
        Sounds.SetActive(false);
        menu.SetActive(false);
        optionmenu.SetActive(false);
        instructionmenu.SetActive(true);
    }
    public void isntructionToOption()
    {
        Sounds.SetActive(false);
        menu.SetActive(false);
        instructionmenu.SetActive(false);
        optionmenu.SetActive(true);
    }
    public void quit()
    {
        Application.Quit();
    }
    public void Sound()
    {
        source.PlayOneShot(clip);   
    }



}
