using UnityEngine;

public class WeaponManager : MonoBehaviour {
    // weapon selected
    [SerializeField] private Weapon[] weapons = new Weapon[3];
    [SerializeField] private WeaponSlot[] weaponSlots = new WeaponSlot[3];

    [SerializeField] private int weaponSelectedIndex = 0;

    private Ray ray;
    private RaycastHit hit;
    private Camera cam;
    [SerializeField] private LayerMask rayLayer;
    [SerializeField] private int blockSpawnLayer = 3;

    private void Awake () {
        cam = Camera.main;
        InitUI ();
    }

    private void InitUI () {
        for (int i = 0; i < weaponSlots.Length; i++) {
            weaponSlots[i].SetWeapon (weapons[i]);

            if (i != weaponSelectedIndex) {
                weaponSlots[i].UnSelectSlot ();
            } else {
                weaponSlots[i].SelectSlot ();
            }
        }
    }

    private void OnSelectWeapon () {
        for (int i = 0; i < weaponSlots.Length; i++) {
            if (i != weaponSelectedIndex) {
                weaponSlots[i].UnSelectSlot ();
            } else {
                weaponSlots[i].SelectSlot ();
            }
        }
    }

    public bool AddWeapon (Weapon weapon) {
        for (int i = 0; i < weapons.Length; i++) {
            if (weapons[i] != null) continue;

            weapons[i] = weapon;
            return true;
        }

        //If unable to take weapon then return false
        return false;
    }

    public void Update () {
        ray = cam.ScreenPointToRay (Input.mousePosition);

        foreach (Weapon weapon in weapons) {
            if (weapon == null) continue;
            weapon.TickCD (Time.deltaTime);
        }

        if (Physics.Raycast (ray, out hit, 500, rayLayer)) {
            var weapon = GetWeaponSelected ();
            if (Input.GetMouseButton (0) && weapon != null) {
                if (hit.collider.gameObject.layer == blockSpawnLayer) {
                    return;
                }
                weapon.SpawnWeapon (hit.point);
            }
        }

        float scroll = Input.GetAxisRaw ("Mouse ScrollWheel");
        if (scroll > 0) {
            weaponSelectedIndex++;
            if (weaponSelectedIndex >= weaponSlots.Length) weaponSelectedIndex = 0;
            OnSelectWeapon ();
        } else if (scroll < 0) {
            weaponSelectedIndex--;
            if (weaponSelectedIndex < 0) weaponSelectedIndex = weaponSlots.Length - 1;
            OnSelectWeapon ();
        }
    }

    public Weapon GetWeaponSelected () {
        return weapons[weaponSelectedIndex];
    }

    public void DropWeapon () {
        if (weapons[weaponSelectedIndex] != null) {
            Debug.LogFormat ("Dropped Weapon at Index {0}", weaponSelectedIndex);
            weapons[weaponSelectedIndex] = null;
            weaponSelectedIndex = 0;

            //Select a new weapon
            for (int i = 0; i < weapons.Length; i++) {
                if (weapons[i] != null) {
                    weaponSelectedIndex = i;
                    break;
                }
            }
        }
    }
}