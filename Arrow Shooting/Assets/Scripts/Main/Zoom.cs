using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Zoom : MonoBehaviour
{
    public bool isZoom = false;

    public static bool canZoom = true;

    Vector2 pivot;

    private void Awake()
    {
        pivot = Vector2.zero;
        GetComponent<Button>().onClick.AddListener(SwitchZoom);
    }

    private void Update()
    {
        if (!canZoom)
        {
            transform.parent.GetComponent<Image>().color = new Color(1f, 0.2f, 0.2f);
            isZoom = false;
            return;
        }
        if (isZoom)
        {
            if (Input.GetMouseButtonDown(0))
            {

                RaycastHit2D cast = Physics2D.CircleCast(Camera.main.ScreenToWorldPoint(Input.mousePosition), 0.1f, Vector2.zero);

                if (cast.collider != null)
                {
                    if (cast.collider.CompareTag("InputField"))
                    {
                        pivot = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        const float bd = MapManager.blockDistance;
                        pivot = new Vector2(Mathf.Clamp(pivot.x, bd * -MapManager.Instance.mapSize.x / 2, bd * MapManager.Instance.mapSize.x / 2), Mathf.Clamp(pivot.y, bd * -MapManager.Instance.mapSize.y / 2, bd * MapManager.Instance.mapSize.y / 2));
                    }
                }
            }

            Vector3 movePos = new Vector3(pivot.x, pivot.y, Camera.main.transform.position.z);

            if (Vector3.Distance(Camera.main.transform.position, movePos) > 0.1f)
            {
                Camera.main.transform.DOMove(movePos, 0.5f);
            }

            if ((int)Input.mouseScrollDelta.y != 0)
            {
                float fixedSize = Camera.main.orthographicSize + -Input.mouseScrollDelta.y * 4;
                if ((int)fixedSize >= 20 && (int)fixedSize <= MapManager.Instance.camSize)
                DOTween.To(() => Camera.main.orthographicSize, x => Camera.main.orthographicSize = x, fixedSize, 0.1f);
            }
        }
    }

    public void SwitchZoom()
    {
        if (canZoom)
        {
            isZoom = !isZoom;
            if (isZoom)
            {
                transform.parent.GetComponent<Image>().color = new Color(0.2f, 1f, 0.2f);
                InputManager.Instance.inputLock = true;
            }
            else
            {
                transform.parent.GetComponent<Image>().color = new Color(1f, 0.2f, 0.2f);
                InputManager.Instance.inputLock = false;
            }
        }
    }
}
