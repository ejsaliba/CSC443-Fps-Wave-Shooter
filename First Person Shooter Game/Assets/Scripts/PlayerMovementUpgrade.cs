using UnityEngine;

public class PlayerMovementUpgrade : MonoBehaviour
{
    public static PlayerMovementUpgrade Instance;

    private bool doubleJumpUnlocked = false;

    private void Awake()
    {
        Instance = this;
    }

    public void EnableDoubleJump()
    {
        doubleJumpUnlocked = true;
    }

    public bool HasDoubleJump()
    {
        return doubleJumpUnlocked;
    }
}