using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class MouseManager : SerializedMonoBehaviour {
    [SerializeField] private ToolMode toolMode;

    private Ray ray;
    private RaycastHit hit;

    [SerializeField] private Camera cam;
    [SerializeField] private Texture2D cursorPointer;
    [SerializeField] private Texture2D cursorPlace;
    [SerializeField] private Texture2D cursorAttack;

    private UnitManager unitManager;

    private void Start () {
        unitManager = FindObjectOfType<UnitManager> ();
        ChangeTool (ToolMode.POINTER);
    }

    private void Update () {

        if (Input.GetKeyDown (KeyCode.A)) {
            ChangeTool (ToolMode.ATTACK);
        }

        ray = cam.ScreenPointToRay (Input.mousePosition);
        if (Physics.Raycast (ray, out hit, 650)) {
            if (Input.GetMouseButtonDown (0)) {
                unitManager.SelectUnit (hit.collider);
            } else if (Input.GetMouseButton (1)) {
                unitManager.OnMoveUnits (hit.point, toolMode);
            }
        }
    }

    public void ChangeTool (ToolMode tool) {
        toolMode = tool;
        switch (toolMode) {
            case ToolMode.POINTER:
                // Cursor.SetCursor (cursorPointer, new Vector2 (0, 0), CursorMode.Auto);
                break;
            case ToolMode.PLACE:
                // Cursor.SetCursor (cursorPlace, Vector2.zero, CursorMode.Auto);
                break;
            case ToolMode.ATTACK:
                // Cursor.SetCursor (cursorAttack, Vector2.zero, CursorMode.Auto);
                break;
        }
    }
}

public enum ToolMode {
    POINTER,
    PLACE,
    ATTACK
}