using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScrollIndexCallbackLine : MonoBehaviour 
{
    void ScrollCellIndex(int idx)
    {
        string name = "Line " + Mathf.FloorToInt( idx * 0.5f );
        gameObject.name = name;
    }
}
