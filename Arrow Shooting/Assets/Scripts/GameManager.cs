using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    
    private static GameManager instance;

    public static GameManager Instance
    {
        get
        {
            return instance;
        }
    }

    public string stageName;
    public VirtualBlock[][] virtualMap;

    public Dictionary<string, int> scores; // score min = 0, max = 99999999  ,9?? 8??

    private string file;


    private void Awake()
    {
        scores = new Dictionary<string, int>();
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        Dictionary<string, int> noButton = new Dictionary<string, int>();
        noButton["PopupCancel"] = 0;
        noButton["Back"] = 0;

        SceneManager.sceneLoaded += (scene, mode) => {
            Button[] buttons = Resources.FindObjectsOfTypeAll<Button>();
            for (int i = 0; i < buttons.Length; i++)
            {
                if (noButton.ContainsKey(buttons[i].gameObject.name)) continue;

                buttons[i].onClick.AddListener(() => GameManager.Instance.GetComponent<AudioSource>().Play());
            }
        };
    }

    private void Start()
    {
        file = string.Concat(Application.persistentDataPath, "/scores.score");

        if (Application.platform == RuntimePlatform.Android)
        {
            Screen.SetResolution(1920, 1080, true);
        }
        LoadScores();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.F11))
        {
            Screen.SetResolution(1920, 1080, !Screen.fullScreen);
        }
    }

    public void SaveScores()
    {
        if (!File.Exists(file))
        {
            File.Create(file);
        }
        else
        {
            StringBuilder str = new StringBuilder();
            foreach(string item in scores.Keys)
            {
                str.AppendLine(string.Concat(item, ":", scores[item])); 
            }
            File.WriteAllText(file, str.ToString());
        }
    }

    private void LoadScores()
    {
        if (File.Exists(file))
        {
            string[] scoreFile = File.ReadAllLines(file);

            for (int i = 0; i < scoreFile.Length; i++)
            {
                string[] scoreData = scoreFile[i].Split(':');
                scores[scoreData[0]] = int.Parse(scoreData[1]);
            }
        }
    }

    public void LoadStage(string stageName)
    {
        try {
            this.stageName = stageName;
            TextAsset textAsset = Resources.Load($"Files/{stageName}") as TextAsset;
            string[] textSplit = textAsset.text.Split('\n');

            Vector2Int v = Vector2Int.zero;
            Vector2Int mapSize = new Vector2Int(textSplit[0].Split(' ').Length, textSplit.Length);

            virtualMap = new VirtualBlock[mapSize.y][];

            for (int y = 0; y < mapSize.y; y++)
            {
                virtualMap[y] = new VirtualBlock[mapSize.x];

                string[] mapSplit = textSplit[y].Split(' ');

                for (int x = 0; x < mapSplit.Length; x++)
                {
                    string[] s = mapSplit[x].Split('(');
                    string[] pos = s[1].Split(')')[0].Split(',');

                    virtualMap[v.y][v.x] = new VirtualBlock(v, new Vector2Int(int.Parse(pos[0]), int.Parse(pos[1])), (BlockType)int.Parse(s[0]));
                    
                    v.x++;
                }
                v.x = 0;
                v.y++;
            }
        }
        catch(Exception e)
        {
            this.stageName = string.Empty;
        }
    }
}
