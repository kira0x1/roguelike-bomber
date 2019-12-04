using UnityEngine;
using UnityEngine.UI;

public class WeaponSlot : MonoBehaviour {
    [SerializeField] private Image icon;
    [SerializeField] private Image border;

    [SerializeField] private Color defaultColor;
    [SerializeField] private Color selectedColor;

    private Weapon weapon;
    private bool hasWeapon;

    public void SetWeapon (Weapon weapon) {
        this.weapon = weapon;

        if (weapon == null) {
            hasWeapon = false;
            icon.enabled = false;
        } else {
            hasWeapon = true;
            icon.sprite = weapon.icon;
            icon.enabled = true;
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