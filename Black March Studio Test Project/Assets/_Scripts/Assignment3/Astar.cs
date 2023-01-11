using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum TileType
{
    Start,
    Goal,
    Grass,
    Blocked,
    Path
}

public class Astar
{
    private Vector3Int startPos, goalPos;
    private Node current;
    private HashSet<Node> openList;
    private HashSet<Node> closedList;
    private Dictionary<Vector3Int, Node> allNode = new Dictionary<Vector3Int, Node>();
    internal Stack<Vector3Int> paths;

    public Stack<Vector3Int> AstarAlgorithm(Vector3Int startPos, Vector3Int goalPos)
    {
        this.startPos = startPos;
        this.goalPos = goalPos;

        if (current == null)
            initialize();

        while (openList.Count > 0 && paths == null)
        {

            List<Node> neighbours = findNeighbours(current.position);
            ExamineNeighbour(neighbours, current);

            UpdateCurrentTile(ref current);

            paths = GeneratePath(current);
        }

        ClearData();
        if (paths!=null && paths.Count > 0)
        {
            return paths;
        }
        else
        {
            return null;
        }
    }

    //Used to initialize Nodes,open list close list
    private void initialize()
    {
        current = getNode(startPos);
        paths = null;

        openList = new HashSet<Node>();
        closedList = new HashSet<Node>();
        openList.Add(current);
    }

    private void ClearData()
    {
        current = null;

        openList.Clear();
        closedList.Clear();
        allNode.Clear();
    }

    private Node getNode(Vector3Int position)
    {
        if (allNode.ContainsKey(position))
        {
            return allNode[position];
        }
        else
        {
            Node node = new Node(position);
            allNode.Add(position, node);
            return node;
        }
    }

    private List<Node> findNeighbours(Vector3Int parentPos)
    {
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                Vector3Int neighbourPos = new Vector3Int(parentPos.x - x, parentPos.y - y, parentPos.z);

                if(GridManager.Instance.grid.ValidataPos(neighbourPos.x, neighbourPos.y))
                {
                    //Here the current and neighbour pos equating prevents diagonal , remove it if we need diagonal movement
                    if ((x != 0 || y != 0) && 
                        (current.position.x==neighbourPos.x ||current.position.y==neighbourPos.y))
                    {
                        if (neighbourPos != startPos &&
                            GridManager.Instance.grid.GetGridObject(neighbourPos.x,neighbourPos.y).GetComponent<TileBehavior>().IsMovementPossible != IsMovable.obstacle)
                        {
                            Node neighbour = getNode(neighbourPos);
                            neighbours.Add(neighbour);
                        }

                    }
                }
            }
        }
        return neighbours;
    }

    private void ExamineNeighbour(List<Node> Neighbour, Node current)
    {
        for (int i = 0; i < Neighbour.Count; i++)
        {
            Node neighbour = Neighbour[i];
            int gScore = DetermineGScore(Neighbour[i].position, current.position);

            //if (!ConnectedDiagonally(current, neighbour))
            //{
            //    continue;
            //}

            if (openList.Contains(neighbour))
            {
                if (current.G + gScore < neighbour.G)
                {
                    calcValues(current, neighbour, gScore);
                }
            }
            else if (!closedList.Contains(neighbour))
            {
                calcValues(current, neighbour, gScore);
                openList.Add(neighbour);
            }

        }
    }

    private void calcValues(Node parent, Node Neighbour, int cost)
    {
        Neighbour.parent = parent;
        Neighbour.G = parent.G + cost;
        Neighbour.H = (Mathf.Abs(Neighbour.position.x - goalPos.x) + Mathf.Abs(Neighbour.position.y - goalPos.y) * 10);

        Neighbour.F = Neighbour.G + Neighbour.H;
    }

    private void UpdateCurrentTile(ref Node current)
    {
        openList.Remove(current);
        closedList.Add(current);

        if (openList.Count > 0)
        {
            current = openList.OrderBy(x => x.F).First();
        }
    }

    private int DetermineGScore(Vector3Int neighbour, Vector3Int current)
    {
        int gScore = 0;
        int x = current.x - neighbour.x;
        int y = current.y - neighbour.y;

        if (Mathf.Abs(x - y) % 2 == 1)
        {
            gScore = 10;
        }
        else
            gScore = 14;

        return gScore;
    }

    private Stack<Vector3Int> GeneratePath(Node current)
    {
        if (current.position == goalPos)
        {
            Stack<Vector3Int> finalPath = new Stack<Vector3Int>();

            while (current.position != startPos)
            {
                finalPath.Push(current.position);
                current = current.parent;
            }
            return finalPath;
        }
        return null;
    }

    private bool ConnectedDiagonally(Node current, Node neighbour)
    {
        Vector3Int direction = current.position - neighbour.position;

        Vector3Int first = new Vector3Int(current.position.x + (direction.x * -1), current.position.y, current.position.z);
        Vector3Int second = new Vector3Int(current.position.x, current.position.y + (direction.y * -1), current.position.z);


        if(GridManager.Instance.grid.ValidataPos(first.x,first.y) && GridManager.Instance.grid.ValidataPos(second.x, second.y))
        {
            if (GridManager.Instance.grid.GetGridObject(first.x, first.y).GetComponent<TileBehavior>().IsMovementPossible == IsMovable.obstacle
            || GridManager.Instance.grid.GetGridObject(second.x, second.y).GetComponent<TileBehavior>().IsMovementPossible == IsMovable.obstacle)
            {
                return false;
            }
        }
        return true;
    }
}
