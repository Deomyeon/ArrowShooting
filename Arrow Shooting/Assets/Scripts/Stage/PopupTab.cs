using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class PopupTab : MonoBehaviour
{

    public Text stageText;
    public Text scoreText;
    public Transform grid;

    public GameObject blockPrefab;
    public Sprite[] blockSprites;

    const int gridSize = 400;


    public void SetPopupTab(string stageName)
    {
        if (stageText.text != stageName)
        {
            stageText.text = stageName;
            if (!GameManager.Instance.scores.ContainsKey(stageName))
            {
                GameManager.Instance.scores[stageName] = 0;
            }
            scoreText.text = string.Concat("Score : ", GameManager.Instance.scores[stageName].ToString());

            GameManager.Instance.LoadStage(stageName);

            DrawMap();
        }

    }

    private void DrawMap()
    {
        if (GameManager.Instance.stageName != string.Empty)
        {
            int count = GameManager.Instance.virtualMap[0].Length * GameManager.Instance.virtualMap.Length;
            if (grid.childCount >= count)
            {
                for (int i = 0; i < grid.childCount; i++)
                {
                    if (i < count)
                    {
                        grid.GetChild(i).gameObject.SetActive(true);
                    }
                    else
                    {
                        grid.GetChild(i).gameObject.SetActive(false);
                    }
                }
            }
            else if (grid.childCount < count)
            {
                int tempCount = count - grid.childCount;
                for (int i = 0; i < tempCount; i++)
                {
                    GameObject temp = GameObject.Instantiate(blockPrefab, grid);
                }
            }

            grid.GetComponent<GridLayoutGroup>().cellSize = Vector2.one * gridSize / GameManager.Instance.virtualMap.Length;
            for (int y = 0; y < GameManager.Instance.virtualMap.Length; y++)
            {
                for (int x = 0; x < GameManager.Instance.virtualMap[y].Length; x++)
                {
                    VirtualBlock vb = GameManager.Instance.virtualMap[y][x];
                    GameObject temp = grid.GetChild(y * GameManager.Instance.virtualMap[y].Length + x).gameObject;

                    if (vb.rotation == Vector2Int.right)
                    {
                        temp.transform.eulerAngles = new Vector3(0, 0, 0);
                    }
                    else if (vb.rotation == Vector2Int.up)
                    {
                        temp.transform.eulerAngles = new Vector3(0, 0, 90);
                    }
                    else if (vb.rotation == Vector2Int.left)
                    {
                        temp.transform.eulerAngles = new Vector3(0, 0, 180);
                    }
                    else if (vb.rotation == Vector2Int.down)
                    {
                        temp.transform.eulerAngles = new Vector3(0, 0, 270);
                    }

                    temp.GetComponent<Image>().sprite = blockSprites[(int)vb.type];
                }
            }
        }
        else
        {
            for (int i = 0; i < grid.childCount; i++)
            {
                grid.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    public void Play()
    {
        if (GameManager.Instance.stageName != string.Empty)
        {
            SceneManager.LoadScene("Main");
        }
    }

}
