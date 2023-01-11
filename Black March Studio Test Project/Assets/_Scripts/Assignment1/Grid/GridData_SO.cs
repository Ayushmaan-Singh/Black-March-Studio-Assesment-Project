using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Grid/Grid Data")]
public class GridData_SO : ScriptableObject
{
    public int length;
    public int width;
    public int cellSize;

    public Vector3 originPos;
}
