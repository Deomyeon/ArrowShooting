using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    public Vector2Int inputRotation;

    private bool canInputUp;
    private bool canInputDown;
    private bool canInputLeft;
    private bool canInputRight;

    public Vector2Int canRotation
    {
        set 
        {
            this.canInputUp = false;
            this.canInputDown = false;
            this.canInputRight = false;
            this.canInputLeft = false;
            if (value == Vector2Int.up)
            {
                this.canInputUp = true;
            }
            else if (value == Vector2Int.down)
            {
                this.canInputDown = true;
            }
            else if (value == Vector2Int.right)
            {
                this.canInputRight = true;
            }
            else if (value == Vector2Int.left)
            {
                this.canInputLeft = true;
            }
            else if (value == Vector2Int.zero)
            {
                this.canInputUp = true;
                this.canInputDown = true;
                this.canInputRight = true;
                this.canInputLeft = true;
            }
        }
    }

    Vector2 mouseStart;
    bool mousePressed;

    public bool isLock = false;
    public bool inputLock {

        get
        {
            return isLock;
        }

        set 
        {
            mousePressed = false;
            isLock = value; 
        } 
    }

    private void Awake()
    {
        Instance = this;
        isLock = false;

        canInputUp = true;
        canInputDown = true;
        canInputLeft = true;
        canInputRight = true;

        mouseStart = Vector2.zero;
        mousePressed = false;
    }

    private void Update()
    {
        if (!inputLock)
        {
            inputRotation = Vector2Int.zero;
            GetInput();
            if (inputRotation != Vector2Int.zero)
            {
                bool b = true;
                foreach (bool item in MapManager.Instance.canMove.Values)
                {
                    b &= item;
                }
                if (b)
                {
                    MapManager.Instance.MoveBlocks(inputRotation);
                }
            }
        }
    }


    private void GetInput()
    {
        GetKeyInput();
        GetMouseInput();
    }

    private void GetKeyInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && canInputUp)
        {
            inputRotation = Vector2Int.up;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) && canInputDown)
        {
            inputRotation = Vector2Int.down;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) && canInputRight)
        {
            inputRotation = Vector2Int.right;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow) && canInputLeft)
        {
            inputRotation = Vector2Int.left;
        }
    }

    private void GetMouseInput()
    {
        if (mousePressed)
        {
            if (Input.GetMouseButtonUp(0))
            {
                RaycastHit2D input = Physics2D.CircleCast(Camera.main.ScreenToWorldPoint(Input.mousePosition), 0.1f, Vector2.zero);
                if (input.collider != null)
                {
                    if (input.collider.gameObject.CompareTag("InputField"))
                    {
                        Vector2 mouseEnd = (Vector2)Camera.main.ScreenToViewportPoint(Input.mousePosition) - Vector2.one * 0.5f;
                        mousePressed = false;
                        if (Mathf.Abs(mouseEnd.x - mouseStart.x) < Mathf.Abs(mouseEnd.y - mouseStart.y))
                        {
                            if (mouseEnd.y > mouseStart.y && canInputUp) // 위로
                            {
                                inputRotation = Vector2Int.up;
                            }
                            if (mouseEnd.y < mouseStart.y && canInputDown) // 아래로
                            {
                                inputRotation = Vector2Int.down;
                            }
                        }
                        else
                        {
                            if (mouseEnd.x > mouseStart.x && canInputRight) // 우로
                            {
                                inputRotation = Vector2Int.right;
                            }
                            if (mouseEnd.x < mouseStart.x && canInputLeft) // 좌로
                            {
                                inputRotation = Vector2Int.left;
                            }
                        }
                    }
                }
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit2D input = Physics2D.CircleCast(Camera.main.ScreenToWorldPoint(Input.mousePosition), 0.1f, Vector2.zero);
                if (input.collider != null)
                {
                    if (input.collider.gameObject.CompareTag("InputField"))
                    {
                        mouseStart = (Vector2)Camera.main.ScreenToViewportPoint(Input.mousePosition) - Vector2.one * 0.5f;
                        mousePressed = true;
                    }
                }
            }
        }
    }
}
