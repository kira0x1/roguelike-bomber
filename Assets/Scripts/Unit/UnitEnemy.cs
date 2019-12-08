using UnityEngine;

public class UnitEnemy : Unit {
    protected float aggroDelay = 3.0f;
    protected float lastHitTime;
    protected float lastAggroCheck;

    public override void TakeDamage (float damage, Unit attacker) {
        base.TakeDamage (damage);
        if (!isDead) {
            if (Time.time >= lastHitTime) {
                lastHitTime = Time.time + aggroDelay;
                TargetUnit (attacker);
            }
        }
    }

    protected override void Update () {
        base.Update ();

        if (unitMode != UnitMode.ATTACK && Time.time >= lastAggroCheck) {
            lastAggroCheck = Time.time + aggroDelay;
            var closestUnit = unitManager.GetClosestUnit (this);
            if (Vector3.Distance (transform.position, closestUnit.transform.position) <= aggroRadius) {
                TargetUnit (closestUnit);
            }
        }
    }
}