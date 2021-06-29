using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;


public class PauseTab : MonoBehaviour
{

    public Image pauseBack;
    public Zoom zoom;

    bool isMove = false;
    
    const float tabPos = 1000;
    const float duration = 0.5f;

    bool isPause = false;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPause)
            {
                ClosePauseTab();
            }
            else
            {
                OpenPauseTab();
            }
        }
    }

    public void OpenPauseTab()
    {
        if (!isMove && !MapManager.Instance.gameClear)
        {
            isPause = true;
            if (zoom.isZoom)
            {
                zoom.SwitchZoom();
            }
            InputManager.Instance.inputLock = true;
            pauseBack.raycastTarget = true;
            isMove = true;
            transform.DOMoveY(0, duration).OnComplete(() =>
            {
                isMove = false;
            });
            DOTween.ToAlpha(() => pauseBack.color, x => pauseBack.color = x, 0.7f, duration);
        }
    }

    private void ClosePauseTab()
    {
        if (!isMove)
        {
            isPause = false;
            DOTween.ToAlpha(() => pauseBack.color, x => pauseBack.color = x, 0f, duration);
            isMove = true;
            transform.DOMoveY(tabPos, duration).OnComplete(() =>
            {
                isMove = false;
                pauseBack.raycastTarget = false;
                InputManager.Instance.inputLock = false;
            });
        }
    }

    public void Menu()
    {
        DOTween.CompleteAll();
        SceneManager.LoadScene("Stage");
    }

    public void Play()
    {
        ClosePauseTab();
    }

    public void RePlay()
    {
        DOTween.CompleteAll();
        SceneManager.LoadScene("Main");
    }

}
