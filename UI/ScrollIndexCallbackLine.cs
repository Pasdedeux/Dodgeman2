using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScrollIndexCallbackLine : MonoBehaviour 
{
    void ScrollCellIndex(int idx)
    {
        string name = "Line " + idx.ToString();
        gameObject.name = name;
    }
}
