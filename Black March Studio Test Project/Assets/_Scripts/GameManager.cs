using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public TurnManager currentTurn = TurnManager.Null;

    #region LayerMasks

    [SerializeField] private LayerMask gridLMask;
    [SerializeField] private LayerMask unitLMask;

    #endregion

    #region Tags

    [SerializeField] string playerUnitTag;
    [SerializeField] string enemyUnitTag;

    #endregion

    #region Actions

    private GameObject playerUnit;
    private GameObject enemyUnit;

    //Object selected through raycast 
    public GameObject targetObj;

    public Astar pathfinding;
    public GameObject activeUserPointer;


    public delegate void SetTargetPos(Vector3Int targetPos);
    public static SetTargetPos SetTargetObj;

    private Vector3Int targetPos;

    private bool playerFlowProcessing = false;
    private bool enemyFlowProcessing = false;

    #endregion

    #region Input sys

    private PlayerInput playerInput;
    private bool isLeftMBPressed = false;

    #endregion

    #region Spawning

    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    public GameObject activeUserPrefab;
    #endregion

    private void Awake()
    {
        playerInput = new PlayerInput();
        pathfinding = new Astar();

        InitializeUnits();
        StartCoroutine(GameplayManager());
        currentTurn = TurnManager.Player;

        SetTargetObj += SetTargetObject;
    }

    private void SetTargetObject(Vector3Int targetPos)
    {
        this.targetPos = targetPos;
    }

    private void InitializeUnits()
    {
        playerUnit = Instantiate(playerPrefab, GridManager.Instance.grid.GetWorldPosition(0, 0) + new Vector3(0, 0.55f, 0), Quaternion.identity);
        enemyUnit = Instantiate(enemyPrefab, GridManager.Instance.grid.GetWorldPosition(3, 5) + new Vector3(0, 0.55f, 0), Quaternion.identity);

        activeUserPointer = Instantiate(activeUserPrefab);
        activeUserPointer.SetActive(false);

        playerUnit.GetComponent<UnitController>().SetCurrentPos = new Vector3Int(0, 0, 0);
        enemyUnit.GetComponent<UnitController>().SetCurrentPos = new Vector3Int(3, 5, 0);
        enemyUnit.GetComponent<EnemyBehavior>().PlayerInstance = playerUnit;

        GridManager.Instance.grid.GetGridObject(0, 0).GetComponent<TileBehavior>().IsMovementPossible = IsMovable.obstacle;
        GridManager.Instance.grid.GetGridObject(3, 5).GetComponent<TileBehavior>().IsMovementPossible = IsMovable.obstacle;

    }

    private void OnEnable()
    {
        playerInput.Enable();
        playerInput.MouseInput.RightMB.Enable();

        playerInput.MouseInput.RightMB.performed += LeftMBClicked;
        playerInput.MouseInput.RightMB.canceled += LeftMBReleased;
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }

    private IEnumerator test()
    {
        yield return new WaitForSeconds(2);

        GridManager.Instance.grid.GetGridObject(3, 0).GetComponent<MeshRenderer>().enabled = false;

        Stack<Vector3Int> movablePath = pathfinding.AstarAlgorithm(new Vector3Int(0, 0, 0), new Vector3Int(3, 0, 0));

        foreach (Vector3Int pos in movablePath)
        {
            Debug.Log(pos);
        }
    }

    private IEnumerator GameplayManager()
    {
        while (true)
        {
            switch (currentTurn)
            {
                case TurnManager.Null:

                    playerInput.MouseInput.Disable();
                    break;

                case TurnManager.Player:
                    UIManager.Instance.ShowTurnText("Player Turn");
                    StartCoroutine(PlayerFlow());
                    break;

                case TurnManager.Enemy:
                    UIManager.Instance.ShowTurnText("Enemy Turn");
                    StartCoroutine(EnemyFlow());
                    break;

            }
            yield return new WaitForSeconds(Random.Range(0.01666f, 0.03333f));
        }
    }

    private Collider RaycastOnMousePos(LayerMask lMask)
    {
        Ray mousePos = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit rayHit;

        if (Physics.Raycast(mousePos, out rayHit, Mathf.Infinity, lMask))
        {
            return rayHit.collider;
        }
        else
        {
            return null;
        }
    }

    private IEnumerator PlayerFlow()
    {
        if (!playerFlowProcessing)
        {
            playerFlowProcessing = true;

            playerInput.MouseInput.Enable();
            ShowActiveUnitPointer(playerUnit);

            while (targetObj == null)
            {
                if (isLeftMBPressed)
                {
                    targetObj = RaycastOnMousePos(gridLMask).gameObject;
                    targetObj.GetComponent<Renderer>().material.color = Color.red;

                    isLeftMBPressed = false;
                }

                yield return null;
            }

            //Start Moving to the pos
            int x, z;
            GridManager.Instance.grid.GetXZ(targetObj.transform.position, out x, out z);

            Stack<Vector3Int> path = pathfinding.AstarAlgorithm(playerUnit.GetComponent<UnitController>().currentPos, new Vector3Int(x, z, 0));

            if (path != null)
            {
                StartCoroutine(playerUnit.GetComponent<UnitController>().Movement(path));
            }
            else
            {
                targetObj.GetComponent<Renderer>().material.color = Color.green;
                targetObj = null;

                playerFlowProcessing = false;

                yield break;
            }

            yield return new WaitUntil(() => Vector3.Distance(playerUnit.transform.position, GridManager.Instance.grid.GetWorldPosition(x, z)) <= 1f);
            targetObj.GetComponent<Renderer>().material.color = Color.green;
            targetObj = null;

            yield return new WaitForSeconds(1f);
            currentTurn = TurnManager.Enemy;
            playerFlowProcessing = false;

        }
    }

    private void LeftMBClicked(InputAction.CallbackContext context)
    {
        isLeftMBPressed = true;
    }

    private void LeftMBReleased(InputAction.CallbackContext context)
    {
        isLeftMBPressed = false;
    }

    private IEnumerator EnemyFlow()
    {
        if (!enemyFlowProcessing)
        {
            enemyFlowProcessing = true;
            ShowActiveUnitPointer(enemyUnit);

            //Start Movement
            enemyUnit.GetComponent<EnemyBehavior>().ExecuteBehavior(pathfinding);

            yield return new WaitUntil(() => Vector3.Distance(enemyUnit.transform.position, GridManager.Instance.grid.GetWorldPosition(targetPos.x, targetPos.y)) <= 1f);

            targetPos = new Vector3Int(-1, -1, -1);


            yield return new WaitForSeconds(1f);

            currentTurn = TurnManager.Player;
            enemyFlowProcessing = false;
        }
    }

    private void ShowActiveUnitPointer(GameObject activeUnit)
    {
        activeUserPointer.transform.parent = null;
        activeUserPointer.SetActive(false);

        activeUserPointer.transform.position = activeUnit.transform.position + new Vector3(0, 1.5f, 0);
        activeUserPointer.transform.parent = activeUnit.transform;
        activeUserPointer.SetActive(true);
    }
}

public enum TurnManager
{
    Null = 0,
    Player = 1,
    Enemy = 2
}
