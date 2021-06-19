using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ArrowGuide : MonoBehaviour
{
    public Vector2Int rotation;

    private RectTransform arrowGuide;
    public RectTransform button;


    public Image arrowKey;

    public Sprite upArrow;
    public Sprite downArrow;
    public Sprite rightArrow;
    public Sprite leftArrow;

    bool b = false;
    bool k = false;

    float keyTime;
    float time;
    float count;

    public delegate void OnComplete();

    OnComplete onComplete;

    private void Awake()
    {
        arrowGuide = GetComponent<RectTransform>();
    }


    private void Update()
    {
        if (b && k)
        {
            if (count > 0)
            {
                count--;
                Arrow();
            }
            else
            {
                gameObject.SetActive(false);
                b = false;
                k = false;
                onComplete();
            }
        }
    }



    public void SetRotation(Vector2Int rotation, int count, float time, OnComplete callBack)
    {
        gameObject.SetActive(true);
        DOTween.Complete(button);
        DOTween.Complete(arrowKey.color);
        onComplete = callBack;

        Vector2 size = button.parent.GetComponent<RectTransform>().sizeDelta;

        button.localPosition = new Vector3(-size.x / 2, 0, 0);

        keyTime = time / 8;
        this.time = time;
        this.count = count;

        this.rotation = rotation;

        Vector3 v = arrowGuide.eulerAngles;

        if (rotation == Vector2Int.up)
        {
            v.z = 90;
            arrowKey.sprite = upArrow;
        }
        else if (rotation == Vector2Int.down)
        {
            v.z = 270;
            arrowKey.sprite = downArrow;
        }
        else if (rotation == Vector2Int.right)
        {
            v.z = 0;
            arrowKey.sprite = rightArrow;
        }
        else if (rotation == Vector2Int.left)
        {
            v.z = 180;
            arrowKey.sprite = leftArrow;
        }

        arrowGuide.eulerAngles = v;
        arrowKey.transform.eulerAngles = Vector3.zero;

        b = true;
        k = true;

    }

    private void Arrow()
    {
        b = false;
        k = false;

        Vector2 size = button.parent.GetComponent<RectTransform>().sizeDelta / 2;

        button.localPosition = new Vector3(-size.x, 0, 0);

        button.DOLocalMoveX(size.x, time).OnComplete(() =>
        {
            b = true;
        });


        DOTween.ToAlpha(() => arrowKey.color, x => arrowKey.color = x, 0.5f, keyTime).OnComplete(() =>
        {
            DOTween.ToAlpha(() => arrowKey.color, x => arrowKey.color = x, 1f, keyTime).OnComplete(() =>
            {
                DOTween.ToAlpha(() => arrowKey.color, x => arrowKey.color = x, 0.5f, keyTime).OnComplete(() =>
                {
                    DOTween.ToAlpha(() => arrowKey.color, x => arrowKey.color = x, 1f, keyTime).OnComplete(() =>
                    {
                        DOTween.ToAlpha(() => arrowKey.color, x => arrowKey.color = x, 0.5f, keyTime).OnComplete(() =>
                        {
                            DOTween.ToAlpha(() => arrowKey.color, x => arrowKey.color = x, 1f, keyTime).OnComplete(() =>
                            {
                                DOTween.ToAlpha(() => arrowKey.color, x => arrowKey.color = x, 0.5f, keyTime).OnComplete(() =>
                                {
                                    DOTween.ToAlpha(() => arrowKey.color, x => arrowKey.color = x, 1f, keyTime).OnComplete(() =>
                                    {
                                        k = true;
                                    });
                                });
                            });
                        });
                    });
                });
            });
        });

    }
}
