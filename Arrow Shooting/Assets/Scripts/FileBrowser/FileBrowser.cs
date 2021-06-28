using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class FileBrowser : MonoBehaviour
{
    public Text browserName;

    public DirectoryLink directoryLink;


    public ScrollRect existsScroll;

    public List<ExistFileData> existFiles;

    public GameObject existItem;


    public InputField fileNameField;
    public Button fileNameButton;

    public string fullPath;


    public delegate void OnSelectedFile(string filePath);
    OnSelectedFile onSelectedFile;


    private void Awake()
    {
        existFiles = new List<ExistFileData>();
    }

    private void ReadFilesInDirectory(string path)
    {
        existFiles.Clear();
        existsScroll.content.localPosition = Vector3.zero;

        string[] directories = Directory.GetDirectories(path);
        string[] files = Directory.GetFiles(path);

        existsScroll.content.sizeDelta = new Vector2(0, (directories.Length + files.Length) * 110);

        if (directories.Length + files.Length > existsScroll.content.childCount)
        {
            int count = directories.Length + files.Length - existsScroll.content.childCount;
            for (int i = 0; i < count; i++)
            {
                GameObject.Instantiate(existItem, existsScroll.content);
            }
        }

        for (int i = 0; i < existsScroll.content.childCount; i++)
        {
            Transform obj = existsScroll.content.GetChild(i);

            if (i < directories.Length + files.Length)
            {
                obj.gameObject.SetActive(true);
                ExistFileData data = obj.GetComponent<ExistFileData>();
                data.button.onClick.RemoveAllListeners();

                int idx;

                if (directories.Length != 0 && i / directories.Length == 0) // directory
                {
                    idx = i;
                    data.type = FileBroweType.Directory;

                    data.text.text = directories[idx].Split('\\', '/')[directories[idx].Split('\\', '/').Length - 1];
                    data.filePath = directories[idx];

                    data.button.onClick.AddListener(() =>
                    {
                        directoryLink.currentPath = data.filePath;
                        directoryLink.directoryPathField.text = directoryLink.currentPath;
                        fullPath = Path.Combine(directoryLink.currentPath, fileNameField.text);
                        ReadFilesInDirectory(directoryLink.currentPath);
                    });
                }
                else // file
                {
                    idx = i - directories.Length;
                    data.type = FileBroweType.File;

                    data.text.text = files[idx].Split('\\', '/')[files[idx].Split('\\', '/').Length - 1];
                    data.filePath = files[idx];

                    data.button.onClick.AddListener(() =>
                    {
                        string t = data.filePath.Split('\\', '/')[data.filePath.Split('\\', '/').Length - 1];
                        fileNameField.text = t.Replace(".arrowshooting", "");
                        fullPath = Path.Combine(directoryLink.currentPath, fileNameField.text);
                    });
                }

                data.image.sprite = data.browseSprite[(int)data.type];
            }
            else
            {
                obj.gameObject.SetActive(false);
            }
        }

    }

    public void SetQuickPath()
    {
        File.WriteAllText(string.Concat(Application.persistentDataPath, "\\", "QuickPath.quickpath"), directoryLink.currentPath);
    }

    public void SaveFileBrowser(string browserName, OnSelectedFile callback)
    {
        gameObject.SetActive(true);
        this.browserName.text = browserName;
        onSelectedFile = callback;

        bool useQuickPath = false;
        if (File.Exists(string.Concat(Application.persistentDataPath, "\\", "QuickPath.quickpath")))
        {
            string quickPath = File.ReadAllText(string.Concat(Application.persistentDataPath, "\\", "QuickPath.quickpath"));
            if (Directory.Exists(quickPath))
            {
                useQuickPath = true;
                directoryLink.currentPath = quickPath;
            }
        }
        if (!useQuickPath)
        {
            if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
            {
                directoryLink.currentPath = Path.GetPathRoot(Directory.GetCurrentDirectory());
            }
            else if (Application.platform == RuntimePlatform.Android)
            {
                directoryLink.currentPath = Application.persistentDataPath;
            }
        }

        directoryLink.directoryPathField.text = directoryLink.currentPath;
        directoryLink.directoryPathButton.onClick.RemoveAllListeners();
        directoryLink.directoryPathButton.onClick.AddListener(() =>
        {
            if (Directory.Exists(directoryLink.directoryPathField.text))
            {
                directoryLink.currentPath = directoryLink.directoryPathField.text;
                ReadFilesInDirectory(directoryLink.currentPath);
            }
            else
            {
                directoryLink.directoryPathField.text = directoryLink.currentPath;
            }
        });

        ReadFilesInDirectory(directoryLink.currentPath);

        fileNameField.text = browserName.Split('\\', '/')[browserName.Split('\\', '/').Length - 1].Split('.')[0];
        fullPath = Path.Combine(directoryLink.currentPath, fileNameField.text);

        fileNameButton.transform.GetChild(0).GetComponent<Text>().text = "Save";
        fileNameButton.onClick.RemoveAllListeners();
        fileNameButton.onClick.AddListener(() =>
        {
            fullPath = Path.Combine(directoryLink.currentPath, fileNameField.text.Replace('.', '_').Replace(' ', '_'));
            string filePath = string.Concat(fullPath, ".arrowshooting");
            onSelectedFile(filePath);
        });
    }

    public void LoadFileBrower(string browserName, OnSelectedFile callback)
    {
        gameObject.SetActive(true);
        this.browserName.text = browserName;
        onSelectedFile = callback;


        bool useQuickPath = false;
        if (File.Exists(string.Concat(Application.persistentDataPath, "\\", "QuickPath.quickpath")))
        {
            string quickPath = File.ReadAllText(string.Concat(Application.persistentDataPath, "\\", "QuickPath.quickpath"));
            if (Directory.Exists(quickPath))
            {
                useQuickPath = true;
                directoryLink.currentPath = quickPath;
            }
        }
        if (!useQuickPath)
        {
            if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
            {
                directoryLink.currentPath = Path.GetPathRoot(Directory.GetCurrentDirectory());
            }
            else if (Application.platform == RuntimePlatform.Android)
            {
                directoryLink.currentPath = Application.persistentDataPath;
            }
        }

        directoryLink.directoryPathField.text = directoryLink.currentPath;
        directoryLink.directoryPathButton.onClick.RemoveAllListeners();
        directoryLink.directoryPathButton.onClick.AddListener(() =>
        {
            if (Directory.Exists(directoryLink.directoryPathField.text))
            {
                directoryLink.currentPath = directoryLink.directoryPathField.text;
                ReadFilesInDirectory(directoryLink.currentPath);
            }
            else
            {
                directoryLink.directoryPathField.text = directoryLink.currentPath;
            }
        });

        ReadFilesInDirectory(directoryLink.currentPath);

        fileNameField.text = "";
        fullPath = fileNameField.text;

        fileNameButton.transform.GetChild(0).GetComponent<Text>().text = "Load";
        fileNameButton.onClick.RemoveAllListeners();
        fileNameButton.onClick.AddListener(() =>
        {
            string filePath = string.Concat(fullPath, ".arrowshooting");
            if (File.Exists(filePath))
            {
                onSelectedFile(filePath);
            }
        });
    }



    public void CloseFileBrowser()
    {
        gameObject.SetActive(false);
    }

}
