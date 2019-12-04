using UnityEngine;

public class TriggerBomb : Bomb {

    [SerializeField] protected KeyCode triggerKey = KeyCode.Mouse1;
    protected bool hasTriggered;

    protected override void OnEnable () {
        bombModel.gameObject.SetActive (true);
        effectDone = false;
        hasDetonated = false;
        hasTriggered = false;
        timer = 0f;
    }

    protected override void OnDisable () {
        bombModel.transform.localPosition = Vector3.zero;
    }

    protected override void Update () {
        if (hasTriggered) {
            TickBomb ();
        } else if (Input.GetKeyUp (triggerKey)) {
            hasTriggered = true;
        }
    }
}