using UnityEngine;

public class DebugUnits : MonoBehaviour {
    private UnitManager unitManager;

    private void Awake () {
        unitManager = FindObjectOfType<UnitManager> ();
    }

    private void Start () {
        Debug.LogFormat ("Total Units: {0}", unitManager.GetUnits ().Count);
        Debug.LogFormat ("Friendly Units: {0}", unitManager.GetFriendly ().Count);
        Debug.LogFormat ("Enemy Units: {0}", unitManager.GetEnemy ().Count);
    }
}