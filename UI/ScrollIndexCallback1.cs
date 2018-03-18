using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScrollIndexCallback1 : MonoBehaviour 
{
    void ScrollCellIndex(int idx)
    {
        string name = "Line " + idx.ToString();
        gameObject.name = name;
    }
}
