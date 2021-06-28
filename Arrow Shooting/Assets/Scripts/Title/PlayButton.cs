using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
    public Button tutorialButton;

    private void Start()
    {
        GameManager.Instance.stageName = "";
        tutorialButton.onClick.AddListener(TutorialButton);
        if (File.Exists(string.Concat(Application.persistentDataPath, "/scores.score")))
        {
            GetComponent<Button>().onClick.AddListener(() => {
                SceneManager.LoadScene("Stage");
            });
            tutorialButton.gameObject.SetActive(true);
        }
        else
        {
            GetComponent<Button>().onClick.AddListener(() => {
                SceneManager.LoadScene("Tutorial");
            });
            tutorialButton.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if (File.Exists(string.Concat(Application.persistentDataPath, "/scores.score")))
            {
                SceneManager.LoadScene("Stage");
            }
            else
            {
                SceneManager.LoadScene("Tutorial");
            }
        }
    }

    private void TutorialButton()
    {
        SceneManager.LoadScene("Tutorial");
    }
}
