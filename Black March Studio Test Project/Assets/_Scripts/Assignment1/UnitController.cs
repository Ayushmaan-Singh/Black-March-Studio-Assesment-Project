using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    public Vector3Int currentPos { get; protected set; }

    public Vector3Int SetCurrentPos
    {
        get
        {
            return currentPos;
        }
        set
        {
            currentPos= value;
        }
    }

    public IEnumerator Movement(Stack<Vector3Int> paths)
    {
        GridManager.Instance.grid.GetGridObject(currentPos.x, currentPos.y).GetComponent<TileBehavior>().IsMovementPossible=IsMovable.movable;
        foreach (Vector3Int path in paths)
        {
            Vector3 currentPosition = GridManager.Instance.grid.GetWorldPosition(currentPos.x, currentPos.y)+ new Vector3(0, 0.55f, 0);
            Vector3 targetPosition = GridManager.Instance.grid.GetWorldPosition(path.x, path.y) + new Vector3(0, 0.55f, 0);
            
            for (float i=0;i<=1;i+=0.1f)
            {

                this.transform.position = Vector3.Lerp(currentPosition,targetPosition,i);

                yield return new WaitForSeconds(0.05f);
            }

            SetCurrentPos = path;
        }
        GridManager.Instance.grid.GetGridObject(currentPos.x, currentPos.y).GetComponent<TileBehavior>().IsMovementPossible=IsMovable.obstacle;
    }
}
