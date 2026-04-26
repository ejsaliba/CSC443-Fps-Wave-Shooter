using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance;

    [Header("Spawners")]
    [SerializeField] private EnemySpawner spawnerA;
    [SerializeField] private EnemySpawner spawnerB;

    [Header("Barrier")]
    [SerializeField] private GameObject barrier;

    private EnemySpawner currentSpawner;

    private int currentWave = 0;
    private int enemiesAlive = 0;

    private bool waitingForPlayer = false;
    private bool waveInProgress = false;
    private bool waveEnded = false;

    private int activeZone = 0; // 0 = A, 1 = B

    public int CurrentWave => currentWave;
    public bool IsWaveInProgress => waveInProgress;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (spawnerA == null || spawnerB == null)
        {
            Debug.LogError("WaveManager: Missing spawners!");
            return;
        }

        if (barrier == null)
        {
            Debug.LogError("WaveManager: Missing barrier reference!");
            return;
        }

        StartNextWave();
    }

    public void StartNextWave()
    {
        currentWave++;

        waveEnded = false;
        waitingForPlayer = false;
        waveInProgress = true;

        // switch zones
        activeZone = 1 - activeZone;
        currentSpawner = (activeZone == 0) ? spawnerA : spawnerB;

        barrier.SetActive(true);

        SpawnWave();
    }

    void SpawnWave()
    {
        int melee = 0;
        int ranged = 0;
        int explosive = 0;

        if (currentWave == 1)
        {
            melee = 5;
        }
        else if (currentWave == 2)
        {
            melee = 5;
            ranged = 5;
        }
        else if (currentWave == 3)
        {
            melee = 5;
            ranged = 5;
            explosive = 2;
        }
        else if (currentWave == 4)
        {
            melee = 10;
            ranged = 5;
            explosive = 2;
        }
        else
        {
            int extra = (currentWave - 4) * 2;

            melee = 10 + extra;
            ranged = 5 + extra;
            explosive = 2 + extra;
        }

        enemiesAlive = melee + ranged + explosive;

        currentSpawner.StartWave(melee, ranged, explosive);
    }

    public void OnEnemyDied()
    {
        if (waveEnded) return;

        enemiesAlive--;

        if (enemiesAlive <= 0)
        {
            WaveCompleted();
        }
    }

    void WaveCompleted()
    {
        if (waveEnded) return;
        waveEnded = true;

        barrier.SetActive(false);
        waitingForPlayer = true;
        waveInProgress = false;

        // 🔥 SAFE SHOP CALL (THIS FIXES YOUR CRASH)
        if (ShopMenuKeyboard.Instance != null)
        {
            ShopMenuKeyboard.Instance.OpenShop();
        }
        else
        {
            Debug.LogWarning("ShopMenuKeyboard instance is NULL - shop not opened");
        }
    }

    public void PlayerEnteredZone(int zoneIndex)
    {
        if (!waitingForPlayer) return;

        if (zoneIndex == activeZone)
        {
            StartNextWave();
        }
    }
}