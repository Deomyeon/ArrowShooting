using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Progress0 : MonoBehaviour
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
            chatGuide.SetChatBox("어라? 과녁이 화살로 날아왔는데 과녁이 부서지지 않았네요?", 1f, () =>
            {
                chatGuide.SetChatBox("이렇게 과녁이 화살로 날아오는 경우는 과녁이 부서지지 않는답니다!", 1f, () =>
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
            });
        }

        if (MapManager.Instance.gameClear)
        {
            wait = true;


            Tutorial.Delay(0.5f, () =>
            {
                chatGuide.SetChatBox("잘 맞추셨어요! 성공적으로 부서졌네요.\n 다음으로 넘어갈게요.", 1f, () =>
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

        //실제 프로그레스

        // 아도겐!!

        InputManager.Instance.inputLock = true;
        Tutorial.Delay(1f, () =>
        {
            chatGuide.SetChatBox("안녕하세요 게임의 튜토리얼을 시작하겠습니다.\n 계속하시려면 화면을 클릭해주세요.", 1f, () =>
            {
                chatGuide.SetChatBox("화살을 과녁에 맞히는 간단한 게임이랍니다!\n 하면서 배워볼까요?", 1f, () =>
                {
                    chatGuide.SetChatBox("화면의 왼쪽으로 스와이프하거나 왼쪽 화살표를 눌러주세요!", 1f, () =>
                    {
                        InputManager.Instance.canRotation = Vector2Int.left;
                        InputManager.Instance.inputLock = false;
                        progressCount = 1;
                        arrowGuide.SetRotation(Vector2Int.left, 3, 1f, () =>
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
