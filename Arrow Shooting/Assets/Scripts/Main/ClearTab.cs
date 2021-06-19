using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ClearTab : MonoBehaviour
{

    public Image clearBack;
    public Text score;

    const float duration = 0.5f;

    public void OpenClearTab()
    {
        clearBack.raycastTarget = true;
        transform.DOMoveY(0, duration);
        DOTween.ToAlpha(() => clearBack.color, x => clearBack.color = x, 0.7f, duration);
        score.text = GameManager.Instance.stageName;
    }

    public void Leave()
    {
        DOTween.CompleteAll();
        SceneManager.LoadScene("Stage");
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
