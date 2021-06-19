using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Progress3 : MonoBehaviour
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
            chatGuide.SetChatBox("블록이 움직이지 않네요?\n 단방향블록은 단방향 이동만 가능해요!", 1f, () =>
            {
                    chatGuide.SetChatBox("블록의 화살표가 가리키는 방향으로만 이동이 가능해요.\n 위로 이동시켜보죠!", 0.5f, () =>
                    {
                        chatGuide.SetChatBox("화면의 위쪽으로 스와이프하거나 위쪽 화살표를 눌러주세요!", 1f, () =>
                        {
                            InputManager.Instance.canRotation = Vector2Int.up;
                            InputManager.Instance.inputLock = false;
                            progressCount = 2;
                            arrowGuide.SetRotation(Vector2Int.up, 3, 1f, () =>
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
            chatGuide.SetChatBox("이제 오른쪽으로 밀어 과녁을 부수죠!", 0.5f, () =>
            {
                chatGuide.SetChatBox("화면의 오른쪽으로 스와이프하거나 오른쪽 화살표를 눌러주세요!", 1f, () =>
                {
                    InputManager.Instance.canRotation = Vector2Int.right;
                    InputManager.Instance.inputLock = false;
                    arrowGuide.SetRotation(Vector2Int.right, 3, 1f, () =>
                    {

                    });
                });
            });
        }

        if (MapManager.Instance.gameClear)
        {
            wait = true;


            Tutorial.Delay(0.5f, () =>
            {
                chatGuide.SetChatBox("다음의 특수블록은 증식블록이에요! 얼마 안 남았네요.", 1f, () =>
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
            chatGuide.SetChatBox("이번에는 단방향블록을 알려드릴게요.\n 새로 생긴 블록이 단방향블록이에요.", 1f, () =>
            {
                chatGuide.SetChatBox("우선 저 블록을 오른쪽으로 옮겨봅시다.", 0.5f, () =>
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
