using TMPro;
using UnityEngine;

public class WeaponUI : MonoBehaviour
{
    [SerializeField] ActiveWeapon activeWeapon;
    [SerializeField] TextMeshProUGUI ammoText;
    [SerializeField] TextMeshProUGUI weaponNameText;

    [Header("Player Health UI")]
    [SerializeField] PlayerHealth playerHealth;
    [SerializeField] TextMeshProUGUI healthText;

    void Update()
    {
        // ---------------- WEAPON UI ----------------
        Weapon weapon = activeWeapon.CurrentWeapon;

        if (weapon == null)
        {
            ammoText.text = "";
            weaponNameText.text = "";
        }
        else
        {
            weaponNameText.text = weapon.Data.weaponName;
            ammoText.text = $"{weapon.CurrentAmmo} / {weapon.Data.maxAmmo}";
        }

        // ---------------- HEALTH UI ----------------
        if (playerHealth != null)
        {
            healthText.text = "HP: " + playerHealth.GetHPValue();
        }
        else
        {
            healthText.text = "";
        }
    }
}