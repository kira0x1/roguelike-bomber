using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class UnitManager : SerializedMonoBehaviour {

    [SerializeField] private HashSet<Unit> friendlyUnits = new HashSet<Unit> ();
    [SerializeField] private HashSet<Unit> enemyUnits = new HashSet<Unit> ();
    [SerializeField] private HashSet<Unit> selectedUnits = new HashSet<Unit> ();
    [SerializeField] private HashSet<Unit> allUnits = new HashSet<Unit> ();

    public HashSet<Unit> GetFriendly () {
        return friendlyUnits;
    }

    public HashSet<Unit> GetEnemy () {
        return enemyUnits;
    }

    public HashSet<Unit> GetUnits () {
        return allUnits;
    }

    public void OnMoveUnits (Vector3 destination, ToolMode toolMode) {
        foreach (Unit unit in selectedUnits) {
            unit.OnMove (destination, toolMode);
        }
    }

    public Unit GetClosestUnit (Unit unit) {
        return GetClosestUnit (unit.transform.position, unit.IsFriendly);
    }

    public Unit GetClosestUnit (Vector3 from, bool isFriendly = false) {
        float distance = 5000;
        Unit unitClosest = null;

        var unitArray = friendlyUnits;
        if (isFriendly) unitArray = enemyUnits;

        foreach (Unit otherUnit in unitArray) {
            float dist = Vector3.Distance (from, otherUnit.transform.position);
            if (dist < distance) {
                distance = dist;
                unitClosest = otherUnit;
            }
        }

        return unitClosest;
    }

    private void Awake () {
        InitUnits ();
    }

    private void InitUnits () {
        var unitsFound = FindObjectsOfType<Unit> ();
        foreach (var unit in unitsFound) {
            allUnits.Add (unit);
            unit.unitManager = this;

            if (unit.IsFriendly) {
                friendlyUnits.Add (unit);
            } else {
                enemyUnits.Add (unit);
            }
        }
    }

    public void SelectUnit (Collider hit) {
        var unit = hit.GetComponent<Unit> ();
        if (unit) {
            if (unit.IsFriendly) {
                if (selectedUnits.Contains (unit)) {
                    unit.OnDeselect ();
                    selectedUnits.Remove (unit);
                } else {
                    unit.OnSelect ();
                    selectedUnits.Add (unit);
                }
            }
        } else {
            foreach (Unit unt in selectedUnits) {
                unt.OnDeselect ();
            }

            selectedUnits.Clear ();
        }
    }
}