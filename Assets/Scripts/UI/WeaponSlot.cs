using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSlot : MonoBehaviour {
    [SerializeField] private Image icon;
    [SerializeField] private Image border;
    [SerializeField] private Image cdImage;

    [SerializeField] private Color defaultColor;
    [SerializeField] private Color selectedColor;

    [SerializeField] private TextMeshProUGUI ammoText;

    private Weapon weapon;
    private bool hasWeapon;
    private int oldAmmo;

    private float currentCD;

    private void Start () {
        UpdateAmmo ();
    }

    private void Update () {
        UpdateCD ();
        UpdateAmmo ();
    }

    private void UpdateCD () {
        if (!hasWeapon) {
            cdImage.fillAmount = 0;
        } else if (weapon.clampTimer > 0) {
            cdImage.fillAmount = weapon.clampTimer / currentCD;
        } else {
            cdImage.fillAmount = 0;
        }
    }

    private void UpdateAmmo () {
        if (!hasWeapon) {
            ammoText.text = "";
        } else if (oldAmmo != weapon.GetAmmo ()) {
            ammoText.text = weapon.GetAmmo ().ToString ();
            oldAmmo = weapon.GetAmmo ();
        }
    }

    private void OnDisable () {
        if (weapon != null) {
            weapon.OnCoolDownEvent -= OnWeaponCoolDown;
        }
    }

    public void OnWeaponCoolDown (float coolDown) {
        currentCD = coolDown;
    }

    public void SetWeapon (Weapon weapon) {
        if (hasWeapon) {
            weapon.OnCoolDownEvent -= OnWeaponCoolDown;
        }

        this.weapon = weapon;

        if (weapon == null) {
            hasWeapon = false;
            icon.enabled = false;
        } else {
            hasWeapon = true;
            icon.sprite = weapon.icon;
            icon.enabled = true;
            weapon.OnCoolDownEvent += OnWeaponCoolDown;
        }
    }

    public Weapon GetWeapon () {
        return weapon;
    }

    public void SelectSlot () {
        border.color = selectedColor;
    }

    public void UnSelectSlot () {
        border.color = defaultColor;
    }
}