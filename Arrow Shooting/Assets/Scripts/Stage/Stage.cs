using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class Stage : MonoBehaviour
{
    public string stageLevel;

    public Transform backMain;

    public GameObject stageButton;

    public Transform popup;

    public Transform popupCancel;

    public Image[] stageTab = new Image[5];

    private Color stageTabDefault = new Color(1, 0.76f, 0.32f);
    private Color stageTabPressed = new Color(1, 0.5f, 0.32f);

    Image selectedButton;

    private const int itemCount = 24;
    private const int popupPos = -1000;

    public bool tabState = false; // true = open, false = close

    private void Start()
    {
        SetStageLevel("STAGE 1");
    }

    public void OpenEditor()
    {
        SceneManager.LoadScene("Editor");
    }


    private void OpenPopupTab(string stageName)
    {
        tabState = true;
        popupCancel.gameObject.SetActive(true);
        popup.DOMoveX(0, 0.5f).SetEase(Ease.InOutExpo);
        popup.GetComponent<PopupTab>().SetPopupTab(stageName);
    }

    public void ClosePopupTab()
    {
        if (tabState)
        {
            tabState = false;
            popup.DOMoveX(popupPos, 0.5f).SetEase(Ease.InOutExpo).OnComplete(() =>
            {
                popupCancel.gameObject.SetActive(false);
            });
        }
    }

    public void SetStageLevel(string stageLevel)
    {
        int num = int.Parse(stageLevel.Split(' ')[1]);
        for (int i = 0; i < stageTab.Length; i++)
        {
            if (i + 1 == num)
            {
                stageTab[i].color = stageTabPressed;
            }
            else
            {
                stageTab[i].color = stageTabDefault;
            }
        }
        this.stageLevel = stageLevel;
        SetBackMain();
    }

    private void SetBackMain()
    {

        DOTween.To(() => backMain.eulerAngles, x => backMain.eulerAngles = x, Vector3.zero, 0.5f).From(new Vector3(0, 180, 0));

        if (backMain.childCount == 0)
        {
            for (int i = 0; i < itemCount; i++)
            {
                GameObject stageBtn = GameObject.Instantiate(stageButton, backMain);
                stageBtn.GetComponentInChildren<Text>().text = (i + 1).ToString();
                stageBtn.name = string.Concat(stageLevel, $" - {i + 1}");
            }
        }

        for (int i = 0; i < backMain.childCount; i++)
        {
            Transform temp = backMain.GetChild(i);
            temp.gameObject.name = string.Concat(stageLevel, " - ", temp.GetComponentInChildren<Text>().text);
            temp.GetComponent<Button>().onClick.AddListener(() =>
            {
                OpenPopupTab(temp.gameObject.name);
            });

        }
    }
}
