using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScrollIndexCallback : MonoBehaviour 
{
    void ScrollCellIndex(int idx)
    {
        string name = "Level " + idx.ToString();
        gameObject.name = name;
    }
}
