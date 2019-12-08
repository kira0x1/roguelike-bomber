using HighlightPlus;
using UnityEngine;
using UnityEngine.AI;

[SelectionBase]
public class Unit : MonoBehaviour {

    protected Unit target;
    [SerializeField]
    protected bool inCombat;

    public enum UnitMode { IDLE, PATROL, ATTACK }
    public UnitMode unitMode = UnitMode.IDLE;

    [SerializeField] protected float health = 100;
    [SerializeField] protected float maxHealth = 100;

    [SerializeField] protected NavMeshAgent agent;
    [SerializeField] protected HighlightEffect highlightEffect;
    [SerializeField] protected bool isFriendly;
    [SerializeField] protected float aggroRadius = 8f;
    [SerializeField] protected float minAttackDistance = 2f;
    [SerializeField] protected float attackDamage = 10;
    [SerializeField] protected float attackCD = 0.2f;
    [SerializeField] protected float nextAttackTime;

    protected bool isDead;
    protected bool isSelected;

    public UnitManager unitManager { get; set; }

    protected virtual void Update () {
        if (!isDead) {
            HandleCombat ();
        }
    }
    protected virtual void HandleCombat () {
        if (unitMode == UnitMode.ATTACK) {
            if (inCombat) {
                agent.destination = target.transform.position;

                float distance = Vector3.Distance (transform.position, target.transform.position);
                if (distance <= minAttackDistance) {
                    if (Time.time >= nextAttackTime) {
                        AttackUnit (target);
                    }
                }
            } else {
                target = unitManager.GetClosestUnit (transform.position, isFriendly);
                Debug.Log (target.name);
                inCombat = true;
            }
        }
    }

    protected virtual void AttackUnit (Unit unit) {
        unit.TakeDamage (attackDamage, this);
        nextAttackTime = Time.time + attackCD;
    }

    public bool IsFriendly {
        get { return isFriendly; }
    }

    public virtual void TargetUnit (Unit target) {
        this.target = target;
        unitMode = UnitMode.ATTACK;
        inCombat = true;
    }

    public virtual void OnMove (Vector3 destination) {
        agent.destination = destination;
    }

    public virtual void OnMove (Vector3 destination, ToolMode toolMode) {
        OnMove (destination);
        if (toolMode == ToolMode.ATTACK) {
            unitMode = UnitMode.ATTACK;
        } else {
            unitMode = UnitMode.IDLE;
        }
    }

    public virtual void TakeDamage (float damage) {
        health -= damage;
        if (health <= 0) {
            health = 0;
            Die ();
        }
    }

    public virtual void TakeDamage (float damage, Unit attacker) {
        TakeDamage (damage);
        if (!isDead && unitMode != UnitMode.ATTACK) {
            TargetUnit (attacker);
        }
    }

    public virtual void Heal (float heal) {
        health += heal;

        if (health > maxHealth) {
            health = maxHealth;
        }
    }

    protected virtual void Die () {
        isDead = true;
    }

    #region Highlight
    private void OnMouseOver () {
        if (!isSelected) {
            SetHightlight (true);
        }
    }

    private void OnMouseExit () {
        if (!isSelected) {
            SetHightlight (false);
        }
    }

    private void SetHightlight (bool hl) {
        highlightEffect.SetHighlighted (hl);
    }

    public void OnSelect () {
        //Highlight
        isSelected = true;
        SetHightlight (true);
    }

    public void OnDeselect () {
        //Remove Highlight
        isSelected = false;
        SetHightlight (false);
    }
    #endregion
}