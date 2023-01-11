using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Grid/Grid Status")]
public class GridStatus_SO : ScriptableObject
{
    public WorldGridStatus.Column[] gridLayout;
}
