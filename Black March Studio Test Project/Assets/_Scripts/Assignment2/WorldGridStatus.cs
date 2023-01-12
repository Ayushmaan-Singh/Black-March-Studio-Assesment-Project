using UnityEngine;
using System.Collections;
using UnityEditor;

public class WorldGridStatus : MonoBehaviour
{
    public static int X = 10, Y = 10;

    public GridData_SO gridData;
    public GridStatus_SO gridStatus;

    [System.Serializable]
    public class Column
    {
        public bool[] rows = new bool[Y];
    }

    public Column[] columns = new Column[X];

    public bool[][] trybool;

    public void OnColumnChanged()
    {
        gridStatus.gridLayout = columns;
    }

    private void OnValidate()
    {
        columns = gridStatus.gridLayout;

        EditorUtility.SetDirty(gridStatus);
        EditorUtility.SetDirty(gridData);
    }
}
