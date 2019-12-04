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
    [SerializeField] protected float cdClampTimer;

    [SerializeField] public Sprite icon;

    [HideInInspector]
    public float cdTimer;
    protected Camera cam;

    private void OnEnable () {
        spawnedCount = 0;
    }

    protected virtual void Awake () {
        cam = Camera.main;
    }

    public virtual void TickCD (float tick) {
        if (cdTimer > 0) {
            cdTimer -= tick;
        }

        if (cdClampTimer > 0) {
            cdClampTimer -= Time.deltaTime;
        }
    }

    public virtual void OnTrigger () {
        spawnedCount--;
        cdClampTimer = coolDownAfterCount;
    }

    public virtual void SpawnWeapon (Vector3 spawn) {
        if (cdTimer > 0) return;
        if (clampSpawn && (spawnedCount >= maxSpawn || cdClampTimer > 0)) return;
        spawnedCount++;

        spawn.y += yOffset;
        GameObject go = ObjectPool.instance.GetPooledObject (weaponTag);
        //If the object is null exit out
        if (!go) return;

        go.transform.position = spawn;
        go.SetActive (true);
        cdTimer = weaponCD;
    }
}