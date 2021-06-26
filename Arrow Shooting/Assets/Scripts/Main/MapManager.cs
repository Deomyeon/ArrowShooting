using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance;
    public bool tutorial = false;
    public bool canUndo = true;
    public Vector2Int mapSize;

    public Sprite bgBlock;

    public Dictionary<Vector2Int, Transform> blockTransform;
    public Dictionary<Vector2Int, Block> blockData;
    public Stack<Dictionary<Block, (Vector2Int, Vector2Int, bool)>> prevDatas;
    public Dictionary<Vector2Int, bool> canMove;

    public ClearTab clearTab;

    public int moveCount;

    public const float blockMoveTime = 0.4f;

    List<Vector2Int> moveBlockList;

    public bool gameClear;

    public GameObject[] blockPrefabs;
    public Sprite[] arrowImg = new Sprite[2];

    public Transform blockParent;
    public Transform backParent;

    public const int blockDistance = 4;
    public float camSize = 20;

    void Awake()
    {
        Instance = this;
        blockTransform = new Dictionary<Vector2Int, Transform>();
        blockData = new Dictionary<Vector2Int, Block>();
        prevDatas = new Stack<Dictionary<Block, (Vector2Int, Vector2Int, bool)>>();
        canMove = new Dictionary<Vector2Int, bool>();
        moveBlockList = new List<Vector2Int>();
        gameClear = false;

        blockParent = new GameObject("BlockParent").transform;
    }

    private void Start()
    {
        if (tutorial)
        {
            GetComponent<Tutorial>().StartTutorial();
        }
        else
        {
            if (MapManager.Instance.backParent != null)
            {
                Destroy(MapManager.Instance.backParent.gameObject);
            }
            for (int i = 0; i < MapManager.Instance.blockParent.childCount; i++)
            {
                Destroy(MapManager.Instance.blockParent.GetChild(i).gameObject);
            }
            LoadMap();
        }

    }

    void LateUpdate()
    {
        if (isCompleted() && !gameClear)
        {
            CompleteStage();
        }
    }


    public void Undo()
    {
        if (prevDatas.Count > 0 && canUndo)
        {
            Dictionary<Block, (Vector2Int, Vector2Int, bool)> dic = prevDatas.Pop();

            for (int i = 0; i < blockParent.childCount; i++)
            {
                if (!dic.ContainsKey(blockParent.GetChild(i).gameObject.GetComponent<Block>()) && blockParent.GetChild(i).gameObject.activeSelf)
                {
                    Destroy(blockParent.GetChild(i).gameObject);
                }
            }

            DOTween.CompleteAll();

            blockData.Clear();
            foreach (Block item in dic.Keys)
            {
                blockData[dic[item].Item1] = item;
                item.Move(dic[item].Item1, dic[item].Item2, item.type, dic[item].Item3);
                item.gameObject.SetActive(true);
            }
            moveCount--;
        }
    }


    public void CompleteStage()
    {
        gameClear = true;
        InputManager.Instance.inputLock = true;

        DOTween.To(() => Camera.main.orthographicSize, x => Camera.main.orthographicSize = x, camSize, 0.1f);
        Camera.main.transform.DOMove(new Vector3(0, 0, Camera.main.transform.position.z), 0.1f).OnComplete(() =>
        {
            if (!tutorial)
            {
                if (GameManager.Instance.scores.ContainsKey(GameManager.Instance.stageName))
                {
                    GameManager.Instance.scores[GameManager.Instance.stageName] = GameManager.Instance.scores[GameManager.Instance.stageName] > moveCount ? moveCount : GameManager.Instance.scores[GameManager.Instance.stageName];
                }
                else
                {
                    GameManager.Instance.scores[GameManager.Instance.stageName] = moveCount;
                }
                GameManager.Instance.SaveScores();

                if (clearTab != null)
                {
                    clearTab.OpenClearTab();
                }
            }
        });

    }

    public bool isCompleted()
    {
        bool complete = true;
        foreach(Block item in MapManager.Instance.blockData.Values)
        {
            if (item.type == BlockType.Target)
            {
                complete = false;
            }
        }
        return complete;
    }

    public void MoveBlocks(Vector2Int movePoint)
    {
        Dictionary<Block, (Vector2Int, Vector2Int, bool)> dic = new Dictionary<Block, (Vector2Int, Vector2Int, bool)>();
        foreach(Vector2Int item in blockData.Keys)
        {
            bool arrow = false;
            if (blockData[item].type == BlockType.Arrow)
            {
                arrow = (blockData[item] as Arrow).power;
            }
            dic[blockData[item]] = (item, blockData[item].Rotation, arrow);
        }

        // 블럭 전체를 rotation 방향으로 이동

        Vector2Int min = Vector2Int.zero;
        Vector2Int max = mapSize;

        if (movePoint == Vector2Int.up)
        {
            for (int i = min.y; i < max.y; i++)
            {
                for (int j = min.x; j < max.x; j++)
                {
                    moveBlockList.Add(new Vector2Int(j, i));
                }
            }
        }
        else if (movePoint == Vector2Int.down)
        {
            for (int i = max.y - 1; i > min.y - 1; i--)
            {
                for (int j = min.x; j < max.x; j++)
                {
                    moveBlockList.Add(new Vector2Int(j, i));
                }
            }
        }
        else if (movePoint == Vector2Int.right)
        {
            for (int i = max.x - 1; i > min.x - 1; i--)
            {
                for (int j = min.y; j < max.y; j++)
                {
                    moveBlockList.Add(new Vector2Int(i, j));
                }
            }
        }
        else if (movePoint == Vector2Int.left)
        {
            for (int i = min.x; i < max.x; i++)
            {
                for (int j = min.y; j < max.y; j++)
                {
                    moveBlockList.Add(new Vector2Int(i, j));
                }
            }
        }

        for (int i = 0; i < moveBlockList.Count; i++)
        {
            if (blockData.ContainsKey(moveBlockList[i]))
            {
                Block data = blockData[moveBlockList[i]];
                data.MoveBlock(movePoint);

            }
        }

        for (int i = 0; i < moveBlockList.Count; i++)
        {
            if (blockData.ContainsKey(moveBlockList[i]))
            {
                Block data = blockData[moveBlockList[i]];
                if (data.type == BlockType.Arrow)
                {
                    (data as Arrow).Action(movePoint);
                }
            }
        }

        moveBlockList.Clear();

        moveCount++;

        bool b = false; ;
        foreach(Vector2Int item in blockData.Keys)
        {
            bool arrow = false;
            if (blockData[item].type == BlockType.Arrow)
            {
                arrow = (blockData[item] as Arrow).power;
            }
            if (dic[blockData[item]] != (item, blockData[item].Rotation, arrow))
            {
                b = true;
            }
        }
        if (b)
        {
            prevDatas.Push(dic);
        }
    }

    public void LoadMap()
    {
        mapSize = new Vector2Int(GameManager.Instance.virtualMap[0].Length, GameManager.Instance.virtualMap.Length);
        gameClear = false;
        moveCount = 0;
        blockData.Clear();
        prevDatas.Clear();
        MakeMap(mapSize);
        for (int y = 0; y < GameManager.Instance.virtualMap.Length; y++)
        {
            for (int x = 0; x < GameManager.Instance.virtualMap[y].Length; x++)
            {
                VirtualBlock vb = GameManager.Instance.virtualMap[y][x];
                MakeBlock(vb.position, vb.rotation, vb.type);
            }
        }
    }

    public void MakeBlock(Vector2Int pos, Vector2Int rot, BlockType type)
    {
        switch(type)
        {
            case BlockType.Arrow:
            case BlockType.Target:
            case BlockType.Wall:
            case BlockType.Rotation:
            case BlockType.Single:
            case BlockType.Multiple:
            case BlockType.Jump:
                GameObject newBlock = Instantiate<GameObject>(blockPrefabs[(int)type - 1]);
                newBlock.transform.parent = blockParent;
                newBlock.GetComponent<Block>().SetBlock(pos, rot, type);
                blockData[pos] = newBlock.GetComponent<Block>();
                if (blockData[pos] is Arrow)
                {
                    blockData[pos].GetComponent<SpriteRenderer>().sortingOrder = 100;
                }
            break;
            default:
            break;
        }
    }


    void MakeMap(Vector2Int size)
    {

        if (size.y > size.x)
        {
            Camera.main.orthographicSize = size.y * 4;
        }
        else
        {
            Camera.main.orthographicSize = size.x * 4;
        }
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, 20, Camera.main.orthographicSize);
        camSize = Camera.main.orthographicSize;

        backParent = new GameObject("BackParent").transform;
        Vector2 back = (size - Vector2.one) * 0.5f;

        for (int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
            {
                GameObject obj = new GameObject(string.Concat(x.ToString(), " ", y.ToString()));
                obj.transform.position = (new Vector2Int(x, y) - back) * blockDistance;
                obj.transform.parent = backParent.transform;
                obj.AddComponent<SpriteRenderer>();
                obj.GetComponent<SpriteRenderer>().sprite = bgBlock;
                obj.GetComponent<SpriteRenderer>().sortingLayerName = "BlockBackground";

                blockTransform[new Vector2Int(x, size.y - 1 - y)] = obj.transform;
            }
        }
    }
}
