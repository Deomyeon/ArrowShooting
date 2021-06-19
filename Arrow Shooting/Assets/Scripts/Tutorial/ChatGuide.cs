using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ChatGuide : MonoBehaviour
{

    public Text textBox;
    public Image endPoint;

    private bool textOn;
    private bool timeComplete;
    private bool check;
    private bool end;

    const float textTime = 0.5f;
    float time = 0;

    public delegate void OnComplete();

    OnComplete onComplete;


    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            if (textOn && timeComplete)
            {
                textOn = false;
                timeComplete = false;
                check = true;
            }
        });
    }


    private void Update()
    {
        if (time > 0)
        {
            time -= Time.deltaTime;
            if (time <= 0)
            {
                timeComplete = true;
                endPoint.gameObject.SetActive(true);
            }
        }

        if (check)
        {
            check = false;
            endPoint.gameObject.SetActive(false);
            DOTween.ToAlpha(() => textBox.color, x => textBox.color = x, 0f, textTime).OnComplete(() =>
            {
                end = true;
            });
        }

        if (end)
        {
            end = false;
            gameObject.SetActive(false);
            onComplete();
        }
    }


    public void SetChatBox(string text, float time, OnComplete callBack)
    {
        gameObject.SetActive(true);
        DOTween.Complete(textBox.color);

        check = false;
        end = false;
        timeComplete = false;

        this.time = time;
        textBox.text = text;
        onComplete = callBack;

        textOn = false;
        DOTween.ToAlpha(() => textBox.color, x => textBox.color = x, 0.8f, textTime).OnComplete(() =>
        {
            textOn = true;
        });
    }

}
