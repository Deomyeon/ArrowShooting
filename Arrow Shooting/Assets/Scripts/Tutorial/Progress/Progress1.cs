using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Progress1 : MonoBehaviour
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
            chatGuide.SetChatBox("과녁 하나가 부서졌네요!\n 그런데 화살의 색이 바뀌었네요?", 1f, () =>
            {
                chatGuide.SetChatBox("과녁을 부순 화살은 색이 바뀌고 다른 과녁을 부술 수 없게 된답니다.", 1f, () =>
                {
                    chatGuide.SetChatBox("그럼 나머지 화살로 과녁을 부숴봅시다.", 0.5f, () =>
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
            });
        }

        if (MapManager.Instance.moveCount == 2 && progressCount == 2)
        {
            progressCount = 0;
            InputManager.Instance.inputLock = true;
            chatGuide.SetChatBox("이제 오른쪽으로 밀면 끝나겠죠!", 0.5f, () =>
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
                chatGuide.SetChatBox("기본적인 이해가 끝난 것 같네요! 다음으로 넘어가죠.", 1f, () =>
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
            chatGuide.SetChatBox("이번에는 과녁이 여러 개 있는 상황을 해결해 볼게요!", 1f, () =>
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
    }


    public void EndProgress()
    {
        canvas.gameObject.SetActive(false);
        Tutorial.progressEnd = true;
    }
}
