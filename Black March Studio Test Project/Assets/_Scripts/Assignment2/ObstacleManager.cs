using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    public static ObstacleManager Instance;

    public GridStatus_SO gridStatus;
    public GridData_SO gridData;

    public ObjectPool<GameObject> objPooled;
    public Dictionary<Vector2Int,GameObject> objGenerated;

    public GameObject obstacles;
    public Transform obstacleHolder;

    private void Awake()
    {
        if(Instance)
            Destroy(gameObject);
        else
            Instance= this;

        objPooled = new ObjectPool<GameObject>(gridData.length*gridData.width);
        objGenerated= new Dictionary<Vector2Int, GameObject>();

    }

    private void Start()
    {
        GenerateObstacle();
    }

    private void GenerateObstacle()
    {
        for(int x=0;x<gridData.width;x++)
        {
            for(int y=0;y<gridData.length;y++)
            {
                if (gridStatus.gridLayout[x].rows[y]==true)
                {
                    GridManager.Instance.grid.GetGridObject(x, y).GetComponent<TileBehavior>().ChangeTileStatus();

                    GameObject obj = objPooled.GetItemFromPool();
                    if (obj)
                    {
                        obj.transform.parent = obstacleHolder;
                    }
                    else
                    {
                        obj = Instantiate(obstacles, obstacleHolder);
                    }

                    obj.transform.position = GridManager.Instance.grid.GetWorldPosition(x, y)+new Vector3(0,0.5f,0);

                    objGenerated.Add(new Vector2Int(x, y), obj);
                }
            }
        }
    }
}
