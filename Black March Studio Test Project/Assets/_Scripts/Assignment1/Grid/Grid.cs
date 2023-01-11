using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Grid<TObj>
{
    //Grid Dimension and origin for it in world space
    private int width;
    private int length;
    public float cellSize;
    public Vector3 originPosition;

    //Grid
    private TObj[][] gridArray;

#if UNITY_EDITOR
    //For Debugging
    private TextMesh[,] debugTextArray;
    private bool showDebug = true;
#endif
    //Visual Representation For Grid Mode
    private GameObject myLine;
    private Transform gridHolder;
    private Material gridMaterial;
    private Color color = Color.red;          //Remove later

    public Grid(int width, int height, float cellSize, Vector3 originPosition, Transform gridHolder)
    {
        this.width = width;
        this.length = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;
        this.gridHolder = gridHolder;

        gridArray = ArrayUtils.CreateJaggedArray<TObj[][]>(width,length);
    }

    //To Get World position of a grid ,given the x and z
    public Vector3 GetWorldPosition(int x, int z)
    {
        return new Vector3(x, 0, z) * cellSize + originPosition;
    }

    //To get cell index of grid ,given the world position
    public void GetXZ(Vector3 worldPosition, out int x, out int z)
    {
        x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
        z = Mathf.FloorToInt((worldPosition - originPosition).z / cellSize);
    }

    //To change the Grid object at given location
    public void TriggerGridObjectChanged(int x, int z)
    {

    }

    #region Set And Get Object on grid array

    //Set value at the  x and z on grid array to the value provided               correct bottom set function by using events instead of boolean
    public bool SetGridObject(int x, int z, TObj value)
    {
        if (ValidataPos(x, z))
        {
            gridArray[x][z] = value;
            return true;
        }
        else
        {
            return false;
        }
    }

    //Set value at the worldposition converted to x and y on grid array to the value provided
    public bool SetGridObject(Vector3 worldPosition, TObj value)
    {
        int x, z;
        GetXZ(worldPosition, out x, out z);
        return SetGridObject(x, z, value);
    }

    //Return a value in a array of TObj at given world position if exsist otherwise return the class of type TObj
    public TObj GetGridObject(Vector3 worldPosition)
    {
        int x, z;
        GetXZ(worldPosition, out x, out z);
        return GetGridObject(x, z);
    }

    //Return a value in a array of TObj at given x and z if exsist otherwise return the class of type TObj
    public TObj GetGridObject(int x, int z)
    {
        if (x >= 0 && z >= 0 && x < width && z < length)
            return gridArray[x][z];
        else
            return default;
    }

    #endregion

    #region Display Grid When In Grid Mode , redo this part

    #region  Using Line Renderer
    //Displays Grid When switching to build mode
    public void DisplayGrid()
    {
        debugTextArray = new TextMesh[width, length];
        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int z = 0; z < gridArray.GetLength(1); z++)
            {
                //debugTextArray[x, z] = UtilsClass.CreateWorldText(gridArray[x, z].ToString(), null, GetWorldPosition(x, z) + new Vector3(cellSize,0, cellSize) * 0.5f,
                //30, Color.white, TextAnchor.MiddleCenter);
                DrawLine(GetWorldPosition(x, z), GetWorldPosition(x, z + 1), color, 0.2f, 0.2f);
                DrawLine(GetWorldPosition(x, z), GetWorldPosition(x + 1, z), color, 0.2f, 0.2f);
            }
        }


        DrawLine(GetWorldPosition(0, length), GetWorldPosition(width, length), color, 0.2f, 0.2f);
        DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, length), color, 0.2f, 0.2f);

        //OnGridObjectchanged += (object sender, OnGridObjectChangedEventArgs eventArgs) =>
        //  {
        //      debugTextArray[eventArgs.x, eventArgs.y].text = gridArray[eventArgs.x, eventArgs.y]?.ToString();

        //  };
    }

    //To Draw a using line renderer given parameters
    private void DrawLine(Vector3 start, Vector3 end, Color color, float Swidth, float EWidth)
    {
        myLine = new GameObject();

        myLine.transform.parent = gridHolder.transform;

        myLine.transform.position = start;
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        //lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
        lr.startColor = color;
        lr.endColor = color;
        lr.startWidth = Swidth;
        lr.endWidth = EWidth;
        start.y = 1;
        end.y = 1;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
    }
    #endregion

    #region Using GameObject with Textures



    #endregion

    //Validate Cell Pos, given world position
    public bool ValidataPos(Vector3 worldPos)
    {
        int x, z;
        GetXZ(worldPos, out x, out z);
        if (x >= 0 && x < width && z >= 0 && z < length)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //Validate Cell Pos, given cell position
    public bool ValidataPos(int x, int z)
    {
        if (x >= 0 && x < width && z >= 0 && z < length)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //To Destroy All Child Object of gridHolder
    public void DestroyGrid(float duration = 0f)
    {
        for (int i = 0; i < gridHolder.transform.childCount; i++)
        {
            GameObject.Destroy(gridHolder.transform.GetChild(i).gameObject, duration);
        }
    }
    #endregion
}


//public class GridObject<TObj>
//{
//    private int x;
//    private int y = 0;
//    private int z;
//    private TObj gridObj;

//    public void SetGridObject(TObj gridObj)
//    {
//        this.gridObj = gridObj;
//        //grid.TriggerGridObjectChanged(x, z);
//    }

//    public void ClearGridObj()
//    {
//        this.gridObj = default;
//    }

//    public GridObject(int x, int z)
//    {
//        this.x = x;
//        this.z = z;
//        this.gridObj = default;
//    }

//    public GridObject(int x, int z, TObj gridObj)
//    {
//        this.x = x;
//        this.z = z;
//        this.gridObj = gridObj;
//    }

//    public bool CanBuild()
//    {
//        return gridObj == null;
//    }

//    public TObj GetGridObject()
//    {
//        return gridObj;
//    }
//}
