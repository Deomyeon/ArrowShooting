using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
    private void Start()
    {
        GameManager.Instance.stageName = "";
        if (File.Exists(string.Concat(Application.persistentDataPath, "/scores.score")))
        {
            GetComponent<Button>().onClick.AddListener(() => {
                SceneManager.LoadScene("Stage");
            });
        }
        else
        {
            GetComponent<Button>().onClick.AddListener(() => {
                SceneManager.LoadScene("Tutorial");
            });
        }
    }
}
