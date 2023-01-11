using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyBehavior : UnitController, IEnemyAi
{
    internal GameObject PlayerInstance;

    public void ExecuteBehavior(Astar pathFinding)
    {
        UnitController playerController = PlayerInstance.GetComponent<UnitController>();
        List<Vector3Int> movablePos= FindPlayerNeighbours(playerController);
        List<Stack<Vector3Int>> allPaths= new List<Stack<Vector3Int>>();
        Stack<Vector3Int> selectedPath= new Stack<Vector3Int>();

        //Check if the enemy is already adjacent
        bool isAdjacent = false;
        if ((currentPos.x == playerController.currentPos.x && Mathf.Abs(currentPos.y-playerController.currentPos.y)==1)
            ||(currentPos.y == playerController.currentPos.y && Mathf.Abs(currentPos.x - playerController.currentPos.x) == 1))
        {
            isAdjacent= true;
        }

        //if not adjacent find paths to all adjacent locations
        if(!isAdjacent)
        {
            foreach(Vector3Int positions in movablePos)
            {
                Stack<Vector3Int> pos = pathFinding.AstarAlgorithm(currentPos,positions);
                if(pos!=null)
                {
                    allPaths.Add(pos);
                }
            }

            allPaths.OrderBy(paths=>paths.Count);

            selectedPath = allPaths[0];
            foreach(Stack<Vector3Int> pos in allPaths)
            {
                if(pos.Count<selectedPath.Count)
                {
                    selectedPath = pos;
                }
            }
        }

        //Start Movement
        if(allPaths.Count>0)
        {
            //List<Vector3Int> movPos = allPaths[allPaths.Count-1].ToList();
            GameManager.SetTargetObj.Invoke(selectedPath.Peek());
            StartCoroutine(Movement(selectedPath));
        }
    }

    public List<Vector3Int> FindPlayerNeighbours(UnitController playerController)
    {
        List<Vector3Int> neighbours = new List<Vector3Int>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                Vector3Int neighbourPos = new Vector3Int( playerController.currentPos.x - x, playerController.currentPos.y - y, playerController.currentPos.z);

                if (GridManager.Instance.grid.ValidataPos(neighbourPos.x, neighbourPos.y))
                {
                    //Here the current and neighbour pos equating prevents diagonal , remove it if we need diagonal movement
                    if ((x != 0 || y != 0) &&
                        (playerController.currentPos.x == neighbourPos.x || playerController.currentPos.y == neighbourPos.y))
                    {
                        if (GridManager.Instance.grid.GetGridObject(neighbourPos.x, neighbourPos.y).GetComponent<TileBehavior>().IsMovementPossible != IsMovable.obstacle)
                        {
                            neighbours.Add(neighbourPos);
                        }

                    }
                }
            }
        }
        return neighbours;
    }
}
