using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;
using UnityEngine.PlayerLoop;
using UnityEngine.InputSystem;

public class UIManager:MonoBehaviour
{
    public static UIManager Instance; 
    [SerializeField] private LayerMask unitLMask;

    [SerializeField]private GameObject DataPanel;
    [SerializeField]private GameObject TurnText;

    private void Awake()
    {
        if (!Instance)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Update()
    {

        Collider obj = RaycastOnMousePos(unitLMask);

        if (obj!=null)
        {
            ShowDataPanel(obj.gameObject);
        }
        else
        {
            HideDataPanel();
        }

    }

    public void ShowDataPanel(GameObject obj)
    {
        if (obj.tag == "Player")
        {
            Vector3Int pos = obj.GetComponent<UnitController>().currentPos;
            DataPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Player";
            DataPanel.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = $"Position: \nX:{pos.x}\nZ:{pos.y}";
        }
        else if (obj.tag == "Enemy")
        {
            Vector3Int pos = obj.GetComponent<UnitController>().currentPos;
            DataPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Enemy";
            DataPanel.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = $"Position: \nX:{pos.x}\nZ:{pos.y}";
        }
        DataPanel.SetActive(true);
    }

    public void HideDataPanel()
    {
        DataPanel.SetActive(false);
    }

    public void ShowTurnText(string text)
    {
        TurnText.SetActive(false);

        TurnText.GetComponent<TextMeshProUGUI>().text =text;

        TurnText.SetActive(true);
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
}
