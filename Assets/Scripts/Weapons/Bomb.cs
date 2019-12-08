using System.Collections;
using UnityEngine;

public class Bomb : MonoBehaviour {

    //[VARIABLES]
    [Space]
    [Header ("Physics Settings")]
    [SerializeField] protected float bombRadius = 15f;
    [SerializeField] protected float bombForce = 10f;
    [SerializeField] protected float upwardModifier = 4f;
    [SerializeField] protected ForceMode forceMode;
    [SerializeField] protected LayerMask physLayer;

    [Space]

    [Header ("Explosion Settings")]
    [SerializeField] protected float detonateDelay = 0.25f;
    [SerializeField] protected float disableDurationOffset = -0.1f;
    [SerializeField] protected ParticleSystem triggerEffect;

    [Space]
    [Header ("Model Settings")]
    [SerializeField] protected Rigidbody bombModel;
    [SerializeField] protected Color gizmoColor = new Color (255, 0, 0, 187f);

    protected float timer;
    protected bool hasDetonated;
    protected bool effectDone;

    //Weapon ScriptableObject
    [SerializeField] protected Weapon weapon;
    [SerializeField] protected bool isSticky;

    protected virtual void OnEnable () {
        bombModel.gameObject.SetActive (true);
        bombModel.isKinematic = false;
        effectDone = false;
        hasDetonated = false;
        timer = 0f;
    }

    protected virtual void OnDisable () {
        bombModel.isKinematic = true;
        bombModel.transform.localPosition = Vector3.zero;
    }

    protected virtual void Update () {
        TickBomb ();
    }

    protected virtual void TickBomb () {
        if (!hasDetonated && timer > detonateDelay) {
            timer = 0f;
            hasDetonated = true;
            TriggerWeapon ();
        } else if (!effectDone && timer > triggerEffect.main.duration + disableDurationOffset) {
            timer = 0f;
            effectDone = true;
            gameObject.SetActive (false);
            triggerEffect.gameObject.SetActive (false);
        } else {
            timer += Time.deltaTime;
        }
    }

    protected virtual void StartEffect () {
        triggerEffect.gameObject.SetActive (true);
        bombModel.gameObject.SetActive (false);
    }

    protected virtual void TriggerWeapon () {
        StartEffect ();
        weapon.OnTrigger ();

        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere (explosionPos, bombRadius, physLayer);

        foreach (Collider hit in colliders) {
            Rigidbody rb = hit.GetComponent<Rigidbody> ();

            if (rb != null) {
                rb.AddExplosionForce (bombForce, explosionPos, bombRadius, upwardModifier, forceMode);
            }
        }
    }

    private void OnDrawGizmos () {
        Gizmos.color = gizmoColor;
        Gizmos.DrawWireSphere (transform.position, bombRadius);
    }
}