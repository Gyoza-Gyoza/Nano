using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue_Dylan
{
    public bool changeCamera;
    public Sprite portrait;
    public string name;
    public Color nameColor;

    [TextArea(3, 10)]
    public string[] sentences;
}
