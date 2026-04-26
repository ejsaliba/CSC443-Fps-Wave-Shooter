using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using StarterAssets;

public class ShopMenuKeyboard : MonoBehaviour
{
    public static ShopMenuKeyboard Instance;

    [Header("UI")]
    [SerializeField] private GameObject shopUI;
    [SerializeField] private TextMeshProUGUI[] options;
    [SerializeField] private TextMeshProUGUI messageText;

    [Header("Weapon Data")]
    [SerializeField] private WeaponData machineGunData;
    [SerializeField] private WeaponData sniperData;
    [SerializeField] private WeaponData rocketData;

    private int currentIndex = 0;
    private bool isOpen = false;
    public bool IsOpen => isOpen;

    private WeaponSwitcher weaponSwitcher;
    private FirstPersonController playerController;
    private PlayerInput playerInput;
    private ActiveWeapon activeWeapon;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        shopUI.SetActive(false);
        UpdateSelection();

        weaponSwitcher = FindObjectOfType<WeaponSwitcher>();
        playerController = FindObjectOfType<FirstPersonController>();
        playerInput = FindObjectOfType<PlayerInput>();
        activeWeapon = FindObjectOfType<ActiveWeapon>();
    }

    private void Update()
    {
        if (!isOpen) return;

        if (Keyboard.current.wKey.wasPressedThisFrame)
        {
            currentIndex--;
            if (currentIndex < 0) currentIndex = options.Length - 1;
            UpdateSelection();
        }

        if (Keyboard.current.sKey.wasPressedThisFrame)
        {
            currentIndex++;
            if (currentIndex >= options.Length) currentIndex = 0;
            UpdateSelection();
        }

        if (Keyboard.current.enterKey.wasPressedThisFrame)
        {
            Select();
        }
    }

    // ---------------- OPEN / CLOSE ----------------

    public void OpenShop()
    {
        isOpen = true;

        shopUI.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Time.timeScale = 0f;

        // disable gameplay
        if (playerController != null) playerController.enabled = false;
        if (playerInput != null) playerInput.enabled = false;
        if (activeWeapon != null) activeWeapon.enabled = false;

        ClearMessage();
    }

    void CloseShop()
    {
        isOpen = false;

        shopUI.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Time.timeScale = 1f;

        // enable gameplay
        if (playerController != null) playerController.enabled = true;
        if (playerInput != null) playerInput.enabled = true;
        if (activeWeapon != null) activeWeapon.enabled = true;

        if (WaveManager.Instance != null)
            WaveManager.Instance.StartNextWave();
    }

    // ---------------- INPUT ----------------

    void Select()
    {
        switch (currentIndex)
        {
            case 0: BuyDoubleJump(); break;
            case 1: BuyMachineGun(); break;
            case 2: BuySniper(); break;
            case 3: BuyRocket(); break;
            case 4: BuyAmmo(); break;
            case 5: CloseShop(); break;
        }
    }

    void UpdateSelection()
    {
        for (int i = 0; i < options.Length; i++)
        {
            options[i].color = (i == currentIndex) ? Color.yellow : Color.white;
        }
    }

    // ---------------- SHOP LOGIC ----------------

    bool Spend(int cost)
    {
        if (ScoreManager.Instance.currentScore < cost)
        {
            ShowMessage("Not enough points!");
            return false;
        }

        ScoreManager.Instance.currentScore -= cost;
        return true;
    }

    void BuyDoubleJump()
    {
        if (!Spend(100)) return;

        PlayerMovementUpgrade.Instance.EnableDoubleJump();
        ShowMessage("Double Jump Unlocked");
    }

    void BuyMachineGun()
    {
        if (!Spend(200)) return;
        UnlockWeapon(machineGunData);
    }

    void BuySniper()
    {
        if (!Spend(300)) return;
        UnlockWeapon(sniperData);
    }

    void BuyRocket()
    {
        if (!Spend(500)) return;
        UnlockWeapon(rocketData);
    }

    void BuyAmmo()
    {
        if (!Spend(100)) return;

        foreach (Weapon w in FindObjectsOfType<Weapon>())
        {
            w.RefillAmmo();
        }

        ShowMessage("Ammo Refilled");
    }

    void UnlockWeapon(WeaponData data)
    {
        Weapon[] weapons = weaponSwitcher.GetComponentsInChildren<Weapon>(true);

        foreach (Weapon w in weapons)
        {
            if (w.Data == data)
            {
                weaponSwitcher.UnlockWeapon(w);
                ShowMessage("Weapon Unlocked");
                return;
            }
        }

        ShowMessage("Weapon not found");
    }

    // ---------------- UI ----------------

    void ShowMessage(string msg)
    {
        if (messageText == null) return;

        messageText.text = msg;
        CancelInvoke(nameof(ClearMessage));
        Invoke(nameof(ClearMessage), 2f);
    }

    void ClearMessage()
    {
        if (messageText != null)
            messageText.text = "";
    }
}