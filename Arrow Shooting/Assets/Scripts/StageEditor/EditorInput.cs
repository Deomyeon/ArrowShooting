using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EditorInput : MonoBehaviour
{
    public bool scrolled = false;
    public bool isMoveScroll = false;

    Vector2 pivot;

    public RectTransform sizeCounter;
    public RectTransform sizeHandle;
    public RectTransform sizePanel;



    private void Update()
    {
        UseZoom();
    }


    private void UseZoom()
    {
        if (!scrolled && !isMoveScroll)
        {
            if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
            {
                MoveMouse();

                if ((int)Input.mouseScrollDelta.y != 0)
                {

                    float v = 0;
                    if (StageEditor.Instance.widthCounter.value > StageEditor.Instance.heightCounter.value)
                    {
                        v = StageEditor.Instance.widthCounter.value * 4;
                    }
                    else
                    {
                        v = StageEditor.Instance.heightCounter.value * 4;
                    }
                    float fixedSize = Camera.main.orthographicSize + -Input.mouseScrollDelta.y * 4;
                    if ((int)fixedSize >= 20 && (int)fixedSize <= v)
                        DOTween.To(() => Camera.main.orthographicSize, x => Camera.main.orthographicSize = x, fixedSize, 0.1f);
                }
            }
            else if (Application.platform == RuntimePlatform.Android)
            {
                if (Input.touchCount == 2)
                {
                    Touch touchZero = Input.GetTouch(0);
                    Touch touchOne = Input.GetTouch(1);

                    Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                    Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

                    float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                    float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

                    float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

                    float v = 0;
                    if (StageEditor.Instance.widthCounter.value > StageEditor.Instance.heightCounter.value)
                    {
                        v = StageEditor.Instance.widthCounter.value * 4;
                    }
                    else
                    {
                        v = StageEditor.Instance.heightCounter.value * 4;
                    }

                    Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize + deltaMagnitudeDiff * 0.5f, 20f, v);
                }
                else if (Input.touchCount < 2)
                {
                    MoveMouse();
                }
            }
        }
    }

    private void MoveMouse()
    {
        if (Input.GetMouseButtonDown(0))
        {

            RaycastHit2D[] colliders = Physics2D.CircleCastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), 0.1f, Vector2.zero);

            List<string> strings = new List<string>();

            for (int i = 0; i < colliders.Length; i++)
            {
                strings.Add(colliders[i].collider.tag);
            }

            if (strings.Contains("Tab"))
            {
                return;
            }

            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].collider.CompareTag("BlockSet"))
                {
                    StageEditor.Instance.isSelected = false;
                    StageEditor.Instance.selectBlock.gameObject.SetActive(false);

                    StageEditor.Instance.popupSelectTab.gameObject.SetActive(false);

                    StageEditor.Instance.checkCollider = colliders[i].collider;

                    if (StageEditor.Instance.selectedTransform == StageEditor.Instance.checkCollider.transform)
                    {
                        StageEditor.Instance.isPut = true;
                        StageEditor.Instance.selectedTransform = null;
                    }
                }
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {

            RaycastHit2D[] colliders = Physics2D.CircleCastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), 0.1f, Vector2.zero);

            List<string> strings = new List<string>();

            for (int i = 0; i < colliders.Length; i++)
            {
                strings.Add(colliders[i].collider.tag);
            }

            if (strings.Contains("Tab"))
            {
                return;
            }

            if (!StageEditor.Instance.isSelected)
            {
                for (int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i].collider.CompareTag("InputField"))
                    {
                        pivot = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        const float bd = MapManager.blockDistance;
                        pivot = new Vector2(Mathf.Clamp(pivot.x, bd * -StageEditor.Instance.widthCounter.value / 2, bd * StageEditor.Instance.widthCounter.value / 2), Mathf.Clamp(pivot.y, bd * -StageEditor.Instance.heightCounter.value / 2, bd * StageEditor.Instance.heightCounter.value / 2));
                    }

                }
            }

            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].collider.CompareTag("BlockSet"))
                {
                    if (StageEditor.Instance.checkCollider == null)
                        return;

                    pivot = StageEditor.Instance.checkCollider.transform.position;


                    if (StageEditor.Instance.isPut)
                    {
                        StageEditor.Instance.checkCollider = null;
                        StageEditor.Instance.isPut = false;
                    }


                    if (StageEditor.Instance.checkCollider == colliders[i].collider)
                    {
                        StageEditor.Instance.selectBlock.position = StageEditor.Instance.checkCollider.transform.position;


                        StageEditor.Instance.selectedTransform = StageEditor.Instance.checkCollider.transform;

                        StageEditor.Instance.selectPos = StageEditor.Instance.blockData[StageEditor.Instance.selectedTransform].Item2;

                        StageEditor.Instance.selectBlock.gameObject.SetActive(true);

                        StageEditor.Instance.isSelected = true;


                        StageEditor.Instance.popupSelectTab.gameObject.SetActive(true);

                    }
                }
            }
        }

        Vector3 movePos = new Vector3(pivot.x, pivot.y, Camera.main.transform.position.z);

        if (Vector3.Distance(Camera.main.transform.position, movePos) > 0.1f)
        {
            Camera.main.transform.DOMove(movePos, 0.5f);
        }
    }

    public void UseSizeCounter()
    {
        if (!isMoveScroll)
        {
            isMoveScroll = true;
            if (scrolled)
            {
                DOTween.To(() => sizeCounter.sizeDelta, x => sizeCounter.sizeDelta = x, new Vector2(sizeCounter.sizeDelta.x, 0), 0.5f).OnComplete(() =>
                {
                    sizePanel.gameObject.SetActive(false);
                    sizeHandle.GetChild(0).eulerAngles = new Vector3(0, 0, 0);
                    isMoveScroll = false;
                });
            }
            else
            {
                sizePanel.gameObject.SetActive(true);
                DOTween.To(() => sizeCounter.sizeDelta, x => sizeCounter.sizeDelta = x, new Vector2(sizeCounter.sizeDelta.x, 700), 0.5f).OnComplete(() =>
                {
                    sizeHandle.GetChild(0).eulerAngles = new Vector3(0, 0, 180);
                    isMoveScroll = false;
                });
            }
            scrolled = !scrolled;
        }
    }

}
