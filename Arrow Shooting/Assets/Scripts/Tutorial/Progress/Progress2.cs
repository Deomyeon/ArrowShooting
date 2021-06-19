using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Progress2 : MonoBehaviour
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
            chatGuide.SetChatBox("어라? 과녁이 부서지지 않았어요! 왜 그럴까요?", 1f, () =>
            {
                chatGuide.SetChatBox("블록을 부수려면 화살이 블록을 향하고 있어야 하기 때문이에요!", 1f, () =>
                {
                    chatGuide.SetChatBox("회전블록을 부수면 블록의 화살표가 가리키는 방향으로 화살이 돌아가요!", 0.5f, () =>
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
            });
        }

        if (MapManager.Instance.moveCount == 2 && progressCount == 2)
        {
            progressCount = 0;
            InputManager.Instance.inputLock = true;
            chatGuide.SetChatBox("이제 위쪽으로 밀어 화살을 오른쪽으로 돌려보죠!", 0.5f, () =>
            {
                chatGuide.SetChatBox("화면의 위쪽으로 스와이프하거나 위쪽 화살표를 눌러주세요!", 1f, () =>
                {
                    InputManager.Instance.canRotation = Vector2Int.up;
                    InputManager.Instance.inputLock = false;
                    progressCount = 3;
                    arrowGuide.SetRotation(Vector2Int.up, 3, 1f, () =>
                    {

                    });
                });
            });
        }

        if (MapManager.Instance.moveCount == 3 && progressCount == 3)
        {
            progressCount = 0;
            InputManager.Instance.inputLock = true;
            chatGuide.SetChatBox("화살이 돌아서 과녁을 가리키고 있네요!", 0.5f, () =>
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
                chatGuide.SetChatBox("회전블록 같은 특수블록을 더 알려드릴게요!\n 다음으로 넘어가죠.", 1f, () =>
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
            chatGuide.SetChatBox("쉬워 보이죠? 바로 시작해보죠", 1f, () =>
            {
                chatGuide.SetChatBox("새로운 블록이 많이 보이네요.\n 위쪽의 블록은 회전블록\n 아래의 블록은 움직일 수 없는 벽블록이라고 해요!", 1f, () =>
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
