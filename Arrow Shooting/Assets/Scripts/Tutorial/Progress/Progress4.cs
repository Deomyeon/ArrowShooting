using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Progress4 : MonoBehaviour
{
    public Transform canvas;

    public ArrowGuide arrowGuide;
    public ChatGuide chatGuide;

    int progressCount;

    bool wait;

    private void Awake()
    {
        progressCount = 0;
        wait = true;
    }

    private void Update()
    {
        if (wait)
            return;

        if (MapManager.Instance.moveCount == 1 && progressCount == 1)
        {
            progressCount = 0;
            InputManager.Instance.inputLock = true;
            chatGuide.SetChatBox("바로 과녁을 부숩시다!", 0.5f, () =>
            {
                chatGuide.SetChatBox("화면의 오른쪽으로 스와이프하거나 오른쪽 화살표를 눌러주세요!", 1f, () =>
                {
                    InputManager.Instance.canRotation = Vector2Int.right;
                    InputManager.Instance.inputLock = false;
                    progressCount = 2;
                    arrowGuide.SetRotation(Vector2Int.right, 3, 1f, () =>
                    {

                    });
                });
            });
        }

        if (MapManager.Instance.moveCount == 2 && progressCount == 2)
        {
            progressCount = 0;
            InputManager.Instance.inputLock = true;
            chatGuide.SetChatBox("과녁 하나를 부쉈네요! 나머지 하나도 부숴보죠.", 1f, () =>
            {
                chatGuide.SetChatBox("화면의 아래쪽으로 스와이프하거나 아래쪽 화살표를 눌러주세요!", 1f, () =>
                {
                    InputManager.Instance.canRotation = Vector2Int.down;
                    InputManager.Instance.inputLock = false;
                    progressCount = 3;
                    arrowGuide.SetRotation(Vector2Int.down, 3, 1f, () =>
                    {

                    });
                });
            });
        }

        if (MapManager.Instance.moveCount == 3 && progressCount == 3)
        {
            progressCount = 0;
            InputManager.Instance.inputLock = true;
            chatGuide.SetChatBox("화면의 오른쪽으로 스와이프하거나 오른쪽 화살표를 눌러주세요!", 1f, () =>
            {
                InputManager.Instance.canRotation = Vector2Int.right;
                InputManager.Instance.inputLock = false;
                arrowGuide.SetRotation(Vector2Int.right, 3, 1f, () =>
                {

                });
            });
        }

        if (MapManager.Instance.gameClear)
        {
            wait = true;


            Tutorial.Delay(0.5f, () =>
            {
                chatGuide.SetChatBox("증식블록도 이해가 되셨죠?\n 다음은 점프블록입니다! 이번이 마지막 블록이네요.", 1f, () =>
                {
                    EndProgress();
                });
            });
        }
    }

    public void StartProgress()
    {
        canvas.gameObject.SetActive(true);
        wait = false;

        // 실제 프로그레스


        InputManager.Instance.inputLock = true;
        Tutorial.Delay(1f, () =>
        {
            chatGuide.SetChatBox("이번에는 증식블록을 알려드릴게요.\n 증식블록을 부수면 같은 화살이 하나 생겨요.", 1f, () =>
            {
                chatGuide.SetChatBox("과녁이 2개이니 화살도 2개가 필요하겠죠.\n 바로 증식해 보도록 합시다!", 0.5f, () =>
                {
                    chatGuide.SetChatBox("화면의 오른쪽으로 스와이프하거나 오른쪽 화살표를 눌러주세요!", 1f, () =>
                    {
                        InputManager.Instance.canRotation = Vector2Int.right;
                        InputManager.Instance.inputLock = false;
                        progressCount = 1;
                        arrowGuide.SetRotation(Vector2Int.right, 3, 1f, () =>
                        {

                        });
                    });
                });
            });
        });
    }


    public void EndProgress()
    {
        canvas.gameObject.SetActive(false);
        Tutorial.progressEnd = true;
    }
}
