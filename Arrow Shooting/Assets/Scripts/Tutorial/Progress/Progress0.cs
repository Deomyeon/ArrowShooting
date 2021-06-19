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
            chatGuide.SetChatBox("���? ������ ȭ��� ���ƿԴµ� ������ �μ����� �ʾҳ׿�?", 1f, () =>
            {
                chatGuide.SetChatBox("�̷��� ������ ȭ��� ���ƿ��� ���� ������ �μ����� �ʴ´�ϴ�!", 1f, () =>
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
            });
        }

        if (MapManager.Instance.gameClear)
        {
            wait = true;


            Tutorial.Delay(0.5f, () =>
            {
                chatGuide.SetChatBox("�� ���߼̾��! ���������� �μ����׿�.\n �������� �Ѿ�Կ�.", 1f, () =>
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

        //���� ���α׷���

        // �Ƶ���!!

        InputManager.Instance.inputLock = true;
        Tutorial.Delay(1f, () =>
        {
            chatGuide.SetChatBox("�ȳ��ϼ��� ������ Ʃ�丮���� �����ϰڽ��ϴ�.\n ����Ͻ÷��� ȭ���� Ŭ�����ּ���.", 1f, () =>
            {
                chatGuide.SetChatBox("ȭ���� ���ῡ ������ ������ �����̶��ϴ�!\n �ϸ鼭 ��������?", 1f, () =>
                {
                    chatGuide.SetChatBox("ȭ���� �������� ���������ϰų� ���� ȭ��ǥ�� �����ּ���!", 1f, () =>
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
