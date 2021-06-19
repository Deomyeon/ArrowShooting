using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorial : MonoBehaviour
{

    int tutorialProgress = 0;
    public static bool progressEnd = false;

    const int lastProgress = 7;

    static float time = 0;

    public delegate void OnComplete();

    private static OnComplete onComplete;


    public void Update()
    {

        if (time > 0)
        {
            time -= Time.deltaTime;
            if (time <= 0)
            {
                onComplete();
            }
        }


        if (tutorialProgress >= lastProgress)
        {
            GameManager.Instance.SaveScores();
            SceneManager.LoadScene("Title");
        }

        if (progressEnd)
        {
            if (tutorialProgress < lastProgress)
            {
                switch (tutorialProgress)
                {
                    case 0:
                        GetComponent<Progress0>().enabled = false;
                        break;
                    case 1:
                        GetComponent<Progress1>().enabled = false;
                        break;
                    case 2:
                        GetComponent<Progress2>().enabled = false;
                        break;
                    case 3:
                        GetComponent<Progress3>().enabled = false;
                        break;
                    case 4:
                        GetComponent<Progress4>().enabled = false;
                        break;
                    case 5:
                        GetComponent<Progress5>().enabled = false;
                        break;
                    case 6:
                        GetComponent<Progress6>().enabled = false;
                        break;
                }


                tutorialProgress++;

                MakeProgress();
            }
        }
    }


    public static void Delay(float time, OnComplete callBack)
    {
        Tutorial.time = time;
        Tutorial.onComplete = callBack;
    }


    public void StartTutorial()
    {
        MapManager.Instance.canUndo = false;
        Zoom.canZoom = false;
        MakeProgress();
    }


    void MakeProgress()
    {
        MapManager.Instance.gameClear = false;
        if (InputManager.Instance != null)
        {
            InputManager.Instance.inputLock = false;

        }

        if (MapManager.Instance.backParent != null)
        {
            Destroy(MapManager.Instance.backParent.gameObject);
        }
        for (int i = 0; i < MapManager.Instance.blockParent.childCount; i++)
        {
            Destroy(MapManager.Instance.blockParent.GetChild(i).gameObject);
        }
        MapManager.Instance.blockData.Clear();

        GameManager.Instance.LoadStage(string.Concat("Tutorial", tutorialProgress));
        MapManager.Instance.LoadMap();
        progressEnd = false;



        switch (tutorialProgress)
        {
            case 0:
                GetComponent<Progress0>().enabled = true;
                GetComponent<Progress0>().StartProgress();
                break;
            case 1:
                GetComponent<Progress1>().enabled = true;
                GetComponent<Progress1>().StartProgress();
                break;
            case 2:
                GetComponent<Progress2>().enabled = true;
                GetComponent<Progress2>().StartProgress();
                break;
            case 3:
                GetComponent<Progress3>().enabled = true;
                GetComponent<Progress3>().StartProgress();
                break;
            case 4:
                GetComponent<Progress4>().enabled = true;
                GetComponent<Progress4>().StartProgress();
                break;
            case 5:
                GetComponent<Progress5>().enabled = true;
                GetComponent<Progress5>().StartProgress();
                break;
            case 6:
                GetComponent<Progress6>().enabled = true;
                GetComponent<Progress6>().StartProgress();
                break;
        }
    }

}
