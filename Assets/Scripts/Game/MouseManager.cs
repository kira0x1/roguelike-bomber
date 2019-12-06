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
    [SerializeField] private HashSet<Unit> unitsSelected = new HashSet<Unit> ();

    private void Start () {
        ChangeTool (ToolMode.POINTER);
    }

    private void Update () {
        ray = cam.ScreenPointToRay (Input.mousePosition);
        if (Physics.Raycast (ray, out hit, 650)) {

            if (Input.GetMouseButtonDown (0) && toolMode == ToolMode.POINTER) {
                OnSelect ();
            }
        }
    }

    private void OnSelect () {
        var unit = hit.collider.GetComponent<Unit> ();
        if (unit == null) {
            foreach (Unit unt in unitsSelected) {
                unt.OnDeselect ();
            }
            unitsSelected.Clear ();
        } else if (unitsSelected.Contains (unit)) {
            unitsSelected.Remove (unit);
        } else {
            unitsSelected.Add (unit);
        }
    }

    public void ChangeTool (ToolMode tool) {
        toolMode = tool;

        switch (toolMode) {
            case ToolMode.POINTER:
                Cursor.SetCursor (cursorPointer, new Vector2 (0, 0), CursorMode.Auto);
                break;
            case ToolMode.PLACE:
                Cursor.SetCursor (cursorPlace, Vector2.zero, CursorMode.Auto);
                break;
        }
    }
}

public enum ToolMode {
    POINTER,
    PLACE
}