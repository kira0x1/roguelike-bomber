using System;
using UnityEngine;

[CreateAssetMenu (fileName = "Weapon", menuName = "Weapons/New Weapon", order = 0)]
public class Weapon : ScriptableObject {
    [SerializeField] protected string weaponTag = "bomb";
    [SerializeField] protected float weaponCD = 0.2f;
    [SerializeField] protected float yOffset = 0.2f;

    [SerializeField] protected bool clampSpawn;
    [SerializeField] protected int maxSpawn = 4;
    [SerializeField] protected int spawnedCount;
    [SerializeField] protected float coolDownAfterCount = 2f;

    [HideInInspector]
    public float clampTimer;

    [SerializeField] protected int ammo = 10;
    [SerializeField] protected int maxAmmo = 10;

    [SerializeField] public Sprite icon;

    [HideInInspector]
    public float cdTimer;

    protected Camera cam;

    public Action<float> OnCoolDownEvent;

    public int GetAmmo () {
        return ammo;
    }

    public int GetMaxAmmo () {
        return maxAmmo;
    }

    private void OnEnable () {
        ammo = maxAmmo;
        spawnedCount = 0;
    }

    protected virtual void Awake () {
        cam = Camera.main;
    }

    public virtual void TickCD (float tick) {
        if (cdTimer > 0) {
            cdTimer -= tick;
        }

        if (clampTimer > 0) {
            clampTimer -= tick;
        }
    }

    public virtual void OnTrigger () {
        spawnedCount--;
        if (clampSpawn) {
            clampTimer = coolDownAfterCount;
            OnCoolDownEvent?.Invoke (coolDownAfterCount);
        }
    }

    public virtual void SpawnWeapon (Vector3 spawn) {
        if (ammo <= 0) return;
        if (cdTimer > 0) return;
        if (clampSpawn && (clampTimer > 0 || spawnedCount >= maxSpawn)) return;

        ammo--;
        spawnedCount++;

        spawn.y += yOffset;
        GameObject go = ObjectPool.instance.GetPooledObject (weaponTag);
        //If the object is null exit out
        if (!go) return;

        go.transform.position = spawn;
        go.SetActive (true);

        cdTimer = weaponCD;

        // OnCoolDownEvent?.Invoke (weaponCD);
    }
}