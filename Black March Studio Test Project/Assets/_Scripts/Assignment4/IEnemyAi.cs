using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyAi
{
    public void ExecuteBehavior(Astar pathFinding);
    List<Vector3Int> FindPlayerNeighbours(UnitController playerController);
}
