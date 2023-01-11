using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance { get; private set; }

    //Grid and Grid Holder
    public Grid<GameObject> grid { get; private set; }
    public Transform gridHolder;

    //Grid Data
    [SerializeField] private GridData_SO gridData;

    //For test purposes
    [SerializeField] private GameObject tileBasePrefab; 

    private void Awake()
    {
        Instance = this;

        int gridLength = gridData.length;
        int gridWidth = gridData.width;
        float cellSize = gridData.cellSize;
        grid = new Grid<GameObject>(gridLength, gridWidth, cellSize, gridData.originPos, gridHolder);


        //Remove later
        for(int x=0;x<gridData.width;x++)
        {
            for(int z=0;z<gridData.length;z++)
            {
                GameObject tile=Instantiate(tileBasePrefab,gridHolder);
                grid.SetGridObject(x,z,tile);

                tile.GetComponent<TileBehavior>().x = x;
                tile.GetComponent<TileBehavior>().z = z;

                tile.transform.position = grid.GetWorldPosition(x,z);
            }
        }
    }
}
