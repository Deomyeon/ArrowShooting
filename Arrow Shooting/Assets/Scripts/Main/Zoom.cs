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
        if (GetComponent<Button>() != null)
        {
            GetComponent<Button>().onClick.AddListener(SwitchZoom);
        }
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
            UseZoom();
        }
    }

    private void UseZoom()
    {
        if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
        {
            if (Input.GetMouseButtonUp(0))
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

                Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize + deltaMagnitudeDiff * 0.5f, 20f, MapManager.Instance.camSize);
            }
            else if (Input.touchCount < 2)
            {
                if (Input.GetMouseButtonUp(0))
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
