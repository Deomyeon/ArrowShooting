using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Progress5 : MonoBehaviour
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
            chatGuide.SetChatBox("점프블록이 부서지지 않았네요?\n 한 블록 너머에 블록이 있기 때문이에요.", 1f, () =>
            {
                chatGuide.SetChatBox("단방향블록을 왼쪽으로 밀어 공간을 확보합시다!", 0.5f, () =>
                {
                    chatGuide.SetChatBox("화면의 왼쪽으로 스와이프하거나 왼쪽 화살표를 눌러주세요!", 1f, () =>
                    {
                        InputManager.Instance.canRotation = Vector2Int.left;
                        InputManager.Instance.inputLock = false;
                        progressCount = 2;
                        arrowGuide.SetRotation(Vector2Int.left, 3, 1f, () =>
                        {

                        });
                    });
                });
            });
        }

        if (MapManager.Instance.moveCount == 2 && progressCount == 2)
        {
            progressCount = 0;
            InputManager.Instance.inputLock = true;
            chatGuide.SetChatBox("다시 점프를 해보도록 하죠!", 0.5f, () =>
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
            chatGuide.SetChatBox("화면의 아래쪽으로 스와이프하거나 아래쪽 화살표를 눌러주세요!", 1f, () =>
            {
                InputManager.Instance.canRotation = Vector2Int.down;
                InputManager.Instance.inputLock = false;
                arrowGuide.SetRotation(Vector2Int.down, 3, 1f, () =>
                {

                });
            });
        }

        if (MapManager.Instance.gameClear)
        {
            wait = true;


            Tutorial.Delay(0.5f, () =>
            {
                chatGuide.SetChatBox("블록에 대한 소개를 모두 끝냈네요!\n 짧게 기능을 설명 하겠습니다.", 1f, () =>
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
            chatGuide.SetChatBox("이번에는 점프블록을 알려드릴게요.\n 점프블록을 부수면 점프블록이 가리키는 방향으로 한 블록을 넘어 화살이 넘어갑니다.", 1f, () =>
            {
                chatGuide.SetChatBox("점프블록을 바로 사용해볼까요?", 0.5f, () =>
                {
                    chatGuide.SetChatBox("화면의 아래쪽으로 스와이프하거나 아래쪽 화살표를 눌러주세요!", 1f, () =>
                    {
                        InputManager.Instance.canRotation = Vector2Int.down;
                        InputManager.Instance.inputLock = false;
                        progressCount = 1;
                        arrowGuide.SetRotation(Vector2Int.down, 3, 1f, () =>
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
