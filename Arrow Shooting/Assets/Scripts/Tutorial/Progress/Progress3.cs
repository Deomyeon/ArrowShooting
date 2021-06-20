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
            chatGuide.SetChatBox("������ �������� �ʳ׿�?\n �ܹ�������� �ܹ��� �̵��� �����ؿ�!", 1f, () =>
            {
                    chatGuide.SetChatBox("������ ȭ��ǥ�� ����Ű�� �������θ� �̵��� �����ؿ�.\n ���� �̵����Ѻ���!", 0.5f, () =>
                    {
                        chatGuide.SetChatBox("ȭ���� �������� ���������ϰų� ���� ȭ��ǥ�� �����ּ���!", 1f, () =>
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
            chatGuide.SetChatBox("���� ���������� �о� ������ �μ���!", 0.5f, () =>
            {
                chatGuide.SetChatBox("ȭ���� ���������� ���������ϰų� ������ ȭ��ǥ�� �����ּ���!", 1f, () =>
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
                chatGuide.SetChatBox("������ Ư�������� ���ĺ����̿���! �� �� ���ҳ׿�.", 1f, () =>
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

        // ���� ���α׷���

        InputManager.Instance.inputLock = true;
        Tutorial.Delay(1f, () =>
        {
            chatGuide.SetChatBox("�̹����� �ܹ�������� �˷��帱�Կ�.\n ���� ���� ������ �ܹ�������̿���.", 1f, () =>
            {
                chatGuide.SetChatBox("�켱 �� ������ ���������� �Űܺ��ô�.", 0.5f, () =>
                {
                    chatGuide.SetChatBox("ȭ���� ���������� ���������ϰų� ������ ȭ��ǥ�� �����ּ���!", 1f, () =>
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