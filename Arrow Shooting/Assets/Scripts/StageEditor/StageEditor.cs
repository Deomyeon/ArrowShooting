using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using System.Text;

public class StageEditor : MonoBehaviour
{
    public static StageEditor Instance;

    public SizeCounter widthCounter;
    public SizeCounter heightCounter;

    public Transform sizeCounter;
    public Transform blockTile;

    public bool isPlaying = false;

    public Button playButton;
    public Button stopButton;

    public Transform zoomButton;
    public Transform undoButton;

    public FileBrowser fileBrowser;


    //맵 표시
    public Dictionary<Vector2Int, Transform> blockTransform;
    public Dictionary<Transform, (BlockType, Vector2Int, Vector2Int)> blockData;

    Transform backParent;

    public Vector2Int size;

    const int blockDistance = 4;


    //입력
    [HideInInspector]
    public Collider2D checkCollider = null;

    public Transform selectBlock;
    [HideInInspector]
    public bool isSelected;
    [HideInInspector]
    public Vector2Int selectPos;
    [HideInInspector]
    public bool isPut = false;

    [HideInInspector]
    public Transform selectedTransform;

    public RectTransform popupSelectTab;


    public Transform backTab;

    // 블럭 선택
    BlockType selectedBlock;
    public Image selectedBlockImage;

    public Sprite[] blockSprites;


    private void Awake()
    {
        Instance = this;
        blockTransform = new Dictionary<Vector2Int, Transform>();
        blockData = new Dictionary<Transform, (BlockType, Vector2Int, Vector2Int)>();

        playButton.onClick.AddListener(Play);
        stopButton.onClick.AddListener(Stop);
    }

    private void Update()
    {
        if (size != new Vector2Int(widthCounter.value, heightCounter.value))
        {
            size = new Vector2Int(widthCounter.value, heightCounter.value);
            MakeMap(size);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (backTab.gameObject.activeSelf)
            {
                CloseBackTab();
            }
            else
            {
                OpenBackTab();
            }
        }
    }



    public void SetBlock()
    {
        if (blockData[selectedTransform].Item1 == selectedBlock)
            return;
        blockData[selectedTransform] = (selectedBlock, blockData[selectedTransform].Item2, Vector2Int.right);
        selectedTransform.eulerAngles = new Vector3(0, 0, 0);
        selectedTransform.GetComponent<SpriteRenderer>().sprite = blockSprites[(int)selectedBlock];
    }

    public void RotateBlock()
    {
        if (blockData[selectedTransform].Item3 == Vector2Int.right)
        {
            blockData[selectedTransform] = (blockData[selectedTransform].Item1, blockData[selectedTransform].Item2, Vector2Int.down);
            selectedTransform.eulerAngles = new Vector3(0, 0, 270);
        }
        else if (blockData[selectedTransform].Item3 == Vector2Int.left)
        {
            blockData[selectedTransform] = (blockData[selectedTransform].Item1, blockData[selectedTransform].Item2, Vector2Int.up);
            selectedTransform.eulerAngles = new Vector3(0, 0, 90);
        }
        else if (blockData[selectedTransform].Item3 == Vector2Int.up)
        {
            blockData[selectedTransform] = (blockData[selectedTransform].Item1, blockData[selectedTransform].Item2, Vector2Int.right);
            selectedTransform.eulerAngles = new Vector3(0, 0, 0);
        }
        else if (blockData[selectedTransform].Item3 == Vector2Int.down)
        {
            blockData[selectedTransform] = (blockData[selectedTransform].Item1, blockData[selectedTransform].Item2, Vector2Int.left);
            selectedTransform.eulerAngles = new Vector3(0, 0, 180);
        }
    }


    public void SelectBlock(int block)
    {
        selectedBlock = (BlockType)block;
        selectedBlockImage.sprite = blockSprites[(int)selectedBlock];
    }

    public void OpenBackTab()
    {
        SetState(false);
        backTab.gameObject.SetActive(true);
        GetComponent<EditorInput>().enabled = false;
    }

    public void CloseBackTab()
    {
        backTab.gameObject.SetActive(false);
        GetComponent<EditorInput>().enabled = true;
    }

    private void Play()
    {

        Vector2Int mapSize = new Vector2Int(widthCounter.value, heightCounter.value);

        GameManager.Instance.virtualMap = new VirtualBlock[mapSize.y][];
        for (int i = 0; i < mapSize.y; i++)
        {
            GameManager.Instance.virtualMap[i] = new VirtualBlock[mapSize.x];
        }

        foreach ((BlockType, Vector2Int, Vector2Int) targetData in blockData.Values)
        {
            GameManager.Instance.virtualMap[targetData.Item2.y][targetData.Item2.x] = new VirtualBlock(targetData.Item2, targetData.Item3, targetData.Item1); ;
        }

        SetState(true);
    }

    private void Stop()
    {
        SetState(false);
    }

    private void SetState(bool state)
    {
        isPlaying = state;

        if (state)
        {
            Camera.main.transform.DOMove(new Vector3(0, 0, Camera.main.transform.position.z), 0.5f);

            if (MapManager.Instance.backParent != null)
            {
                Destroy(MapManager.Instance.backParent.gameObject);
            }
            for (int i = 0; i < MapManager.Instance.blockParent.childCount; i++)
            {
                Destroy(MapManager.Instance.blockParent.GetChild(i).gameObject);
            }

            GetComponent<MapManager>().LoadMap();
            GetComponent<InputManager>().inputLock = false;

            selectBlock.gameObject.SetActive(false);
            popupSelectTab.gameObject.SetActive(false);
        }

        backParent.gameObject.SetActive(!state);
        if (GetComponent<MapManager>().backParent != null)
        {
            GetComponent<MapManager>().backParent.gameObject.SetActive(state);
        }
        if (GetComponent<MapManager>().blockParent != null)
        {
            GetComponent<MapManager>().blockParent.gameObject.SetActive(state);
        }

        GetComponent<StageEditor>().enabled = !state;
        GetComponent<EditorInput>().enabled = !state;
        GetComponent<MapManager>().enabled = state;
        GetComponent<InputManager>().enabled = state;

        sizeCounter.gameObject.SetActive(!state);
        blockTile.gameObject.SetActive(!state);

        selectedBlockImage.gameObject.SetActive(!state);

        zoomButton.gameObject.SetActive(state);
        undoButton.gameObject.SetActive(state);

        playButton.gameObject.SetActive(!state);
        stopButton.gameObject.SetActive(state);
    }

    public void CloseEditor()
    {
        DOTween.CompleteAll();
        SceneManager.LoadScene("Stage");
    }

    public void SaveData()
    {
        fileBrowser.SaveFileBrowser("Save Stage Data", (string filePath) =>
        {

            StringBuilder sb = new StringBuilder();

            int i = 0;
            for (int x = 0; x < widthCounter.value; x++)
            {
                for (int y = 0; y < heightCounter.value; y++)
                {
                    (BlockType, Vector2Int, Vector2Int) targetData = blockData[blockTransform[new Vector2Int(y, x)]];
                    if (i == widthCounter.value - 1)
                    {
                        sb.Append(string.Concat(((int)targetData.Item1).ToString(), "(", targetData.Item3.x, ",", targetData.Item3.y, ")\n"));
                        i = 0;
                    }
                    else
                    {
                        sb.Append(string.Concat(((int)targetData.Item1).ToString(), "(", targetData.Item3.x, ",", targetData.Item3.y, ") "));
                        i++;
                    }
                }
            }
            sb.Remove(sb.Length - 1, 1);

            File.WriteAllText(filePath, sb.ToString());

            fileBrowser.CloseFileBrowser();
        });
    }

    public void LoadData()
    {
        fileBrowser.LoadFileBrower("Load Stage Data", (string filePath) =>
        {
            string readFile = File.ReadAllText(filePath);

            string[] readLine = readFile.Split('\n');

            widthCounter.value = readLine[0].Split(' ').Length;
            widthCounter.text.text = widthCounter.value.ToString();
            heightCounter.value = readLine.Length;
            heightCounter.text.text = heightCounter.value.ToString();

            size = new Vector2Int(widthCounter.value, heightCounter.value);
            MakeMap(size);

            for (int y = 0; y < heightCounter.value; y++)
            {
                for (int x = 0; x < widthCounter.value; x++)
                {
                    string[] readData = readLine[y].Split(' ');

                    BlockType type = (BlockType)int.Parse(readData[x].Split('(')[0]);

                    string[] temp = readData[x].Split('(')[1].Split(',');

                    int rotX = int.Parse(temp[0]);
                    int rotY = int.Parse(temp[1].Split(')')[0]);

                    blockData[blockTransform[new Vector2Int(x, y)]] = (type, new Vector2Int(x, y), new Vector2Int(rotX, rotY));

                    if (new Vector2Int(rotX, rotY) == Vector2Int.right)
                    {
                        blockTransform[new Vector2Int(x, y)].eulerAngles = new Vector3(0, 0, 0);
                    }
                    else if (new Vector2Int(rotX, rotY) == Vector2Int.left)
                    {
                        blockTransform[new Vector2Int(x, y)].eulerAngles = new Vector3(0, 0, 180);
                    }
                    else if (new Vector2Int(rotX, rotY) == Vector2Int.up)
                    {
                        blockTransform[new Vector2Int(x, y)].eulerAngles = new Vector3(0, 0, 90);
                    }
                    else if (new Vector2Int(rotX, rotY) == Vector2Int.down)
                    {
                        blockTransform[new Vector2Int(x, y)].eulerAngles = new Vector3(0, 0, 270);
                    }

                    blockTransform[new Vector2Int(x, y)].GetComponent<SpriteRenderer>().sprite = blockSprites[(int)type];
                }
            }

            selectBlock.gameObject.SetActive(false);
            popupSelectTab.gameObject.SetActive(false);

            fileBrowser.CloseFileBrowser();
        });
    }


    void MakeMap(Vector2Int size)
    {
        blockTransform.Clear();
        if (backParent == null)
        {
            backParent = new GameObject("BackParent").transform;
        }
        Vector2 back = (size - Vector2.one) * 0.5f;

        int count = size.x * size.y;

        if (count > backParent.childCount)
        {
            int newChildCount = count - backParent.childCount;
            for (int i = 0; i < newChildCount; i++)
            {
                GameObject obj = new GameObject((backParent.childCount + i).ToString());
                obj.transform.parent = backParent.transform;
                obj.AddComponent<SpriteRenderer>();
                obj.GetComponent<SpriteRenderer>().sprite = blockSprites[(int)BlockType.None];
                obj.AddComponent<BoxCollider2D>();
                obj.tag = "BlockSet";
                obj.GetComponent<SpriteRenderer>().sortingLayerName = "BlockBackground";
            }
        }
        for (int i = 0; i < backParent.childCount; i++)
        {
            Transform child = backParent.GetChild(i);
            int x = i / size.y;
            int y = i % size.y;
            if (i >= count)
            {
                child.gameObject.SetActive(false);
            }
            else
            {
                child.gameObject.SetActive(true);
                child.position = (new Vector2Int(x, y) - back) * blockDistance;
                child.GetComponent<SpriteRenderer>().sprite = blockSprites[(int)BlockType.None];
                blockTransform[new Vector2Int(x, size.y - 1 - y)] = child;
                blockData[child] = (BlockType.None, new Vector2Int(x, size.y - 1 - y), Vector2Int.right);
            }
        }

        if (size.y > size.x)
        {
            Camera.main.orthographicSize = size.y * 4;
        }
        else
        {
            Camera.main.orthographicSize = size.x * 4;
        }
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, 20, Camera.main.orthographicSize);

        if (blockTransform.ContainsKey(selectPos))
        {
            selectBlock.position = blockTransform[selectPos].position;
        }
        else
        {
            selectBlock.gameObject.SetActive(false);
            isSelected = false;
        }
    }
}
