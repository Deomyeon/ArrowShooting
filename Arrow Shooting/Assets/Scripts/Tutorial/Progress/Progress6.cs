using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Progress6 : MonoBehaviour
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


        if (MapManager.Instance.gameClear)
        {
            wait = true;

            InputManager.Instance.inputLock = true;
            Tutorial.Delay(1f, () =>
            {
                chatGuide.SetChatBox("튜토리얼이 끝났습니다. 좋은 게임 되세요!", 1f, () =>
                {
                    MapManager.Instance.canUndo = false;
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
            chatGuide.SetChatBox("이번에는 왼쪽에 보이는 버튼들의 기능을 알려드릴게요!", 1f, () =>
            {
                chatGuide.SetChatBox("아래의 버튼은 되돌리기 버튼이에요!\n 이전에 이동한 것을 되돌릴 수 있어요.", 1f, () =>
                {
                    chatGuide.SetChatBox("위의 버튼은 확대/축소 버튼이에요!\n 좌클릭으로 중심을 정하고 스크롤로 확대/축소 할 수 있어요.\n 버튼 활성화 중에는 입력이 불가해요!", 1f, () =>
                    {
                        chatGuide.SetChatBox("자유롭게 이용하면서 클리어해 보세요!", 0.5f, () =>
                        {
                            InputManager.Instance.inputLock = false;
                            InputManager.Instance.canRotation = Vector2Int.zero;
                            MapManager.Instance.canUndo = true;
                            Zoom.canZoom = true;
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
