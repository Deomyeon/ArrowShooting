using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ClearTab : MonoBehaviour
{

    public Image clearBack;
    public Text stage;
    public Text score;

    const float duration = 0.5f;

    public void OpenClearTab()
    {
        clearBack.raycastTarget = true;
        transform.DOMoveY(0, duration);
        DOTween.ToAlpha(() => clearBack.color, x => clearBack.color = x, 0.7f, duration);
        stage.text = GameManager.Instance.stageName;
        score.text = (1000 - MapManager.Instance.moveCount).ToString();
    }

    public void Leave()
    {
        DOTween.CompleteAll();
        SceneManager.LoadScene("Stage");
    }

    public void RePlay()
    {
        DOTween.CompleteAll();
        SceneManager.LoadScene("Main");
    }

    public void Next()
    {
        string[] strs = GameManager.Instance.stageName.Split(' ');
        int smallStage = int.Parse(strs[strs.Length - 1]);
        if (smallStage >= 24)
        {
            Leave();
        }
        else
        {
            DOTween.CompleteAll();
            GameManager.Instance.stageName = string.Concat(GameManager.Instance.stageName.Substring(0, GameManager.Instance.stageName.Length - smallStage.ToString().Length), smallStage + 1);
            GameManager.Instance.LoadStage(GameManager.Instance.stageName);
            if (GameManager.Instance.stageName == string.Empty)
            {
                Leave();
            }
            else
            {
                SceneManager.LoadScene("Main");
            }
        }
    }

}
