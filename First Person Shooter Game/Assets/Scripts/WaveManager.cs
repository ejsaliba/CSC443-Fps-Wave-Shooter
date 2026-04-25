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

    private int activeZone = 0; // 0 = A, 1 = B

    public int CurrentWave => currentWave;
    public bool IsWaveInProgress => waveInProgress;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        StartNextWave();
    }

    public void StartNextWave()
    {
        currentWave++;

        // switch zones
        activeZone = 1 - activeZone;
        currentSpawner = (activeZone == 0) ? spawnerA : spawnerB;

        barrier.SetActive(true);
        waveInProgress = true;
        waitingForPlayer = false;

        SpawnWave();
    }

    void SpawnWave()
    {
        int melee = 0;
        int ranged = 0;
        int explosive = 0;

        // your exact wave logic
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
        enemiesAlive--;

        if (enemiesAlive <= 0)
        {
            WaveCompleted();
        }
    }

    void WaveCompleted()
    {
        barrier.SetActive(false);
        waitingForPlayer = true;
        waveInProgress = false;
    }

    public void PlayerEnteredZone(int zoneIndex)
    {
        if (!waitingForPlayer) return;

        // must go to the next zone (alternate)
        if (zoneIndex == activeZone)
        {
            StartNextWave();
        }
    }
}