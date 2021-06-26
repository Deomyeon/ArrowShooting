using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum FileBroweType
{
    File,
    Directory
}

public class ExistFileData : MonoBehaviour
{
    public Button button;
    public Image image;
    public Text text;
    public FileBroweType type;
    public string filePath;

    public Sprite[] browseSprite = new Sprite[2];
}
