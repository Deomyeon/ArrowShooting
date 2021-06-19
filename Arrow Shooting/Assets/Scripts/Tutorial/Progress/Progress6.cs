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
                chatGuide.SetChatBox("Ʃ�丮���� �������ϴ�. ���� ���� �Ǽ���!", 1f, () =>
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

        // ���� ���α׷���


        InputManager.Instance.inputLock = true;
        Tutorial.Delay(1f, () =>
        {
            chatGuide.SetChatBox("�̹����� ���ʿ� ���̴� ��ư���� ����� �˷��帱�Կ�!", 1f, () =>
            {
                chatGuide.SetChatBox("�Ʒ��� ��ư�� �ǵ����� ��ư�̿���!\n ������ �̵��� ���� �ǵ��� �� �־��.", 1f, () =>
                {
                    chatGuide.SetChatBox("���� ��ư�� Ȯ��/��� ��ư�̿���!\n ��Ŭ������ �߽��� ���ϰ� ��ũ�ѷ� Ȯ��/��� �� �� �־��.\n ��ư Ȱ��ȭ �߿��� �Է��� �Ұ��ؿ�!", 1f, () =>
                    {
                        chatGuide.SetChatBox("�����Ӱ� �̿��ϸ鼭 Ŭ������ ������!", 0.5f, () =>
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
