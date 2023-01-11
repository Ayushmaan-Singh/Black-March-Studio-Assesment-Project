using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBehavior : MonoBehaviour
{
    public int x;
    public int z;

    [SerializeField] private IsMovable isMovable = IsMovable.movable;
    internal IsMovable IsMovementPossible
    {
        get
        {
            return isMovable;
        }
        set 
        { 
            isMovable = value; 
        }
    }


    public void ChangeTileStatus()
    {
        if (isMovable == IsMovable.movable)
        {
            isMovable = IsMovable.obstacle;
        }
        else
        {
            isMovable = IsMovable.movable;
        }
    }
}

public enum IsMovable
{
    movable,
    obstacle
}
