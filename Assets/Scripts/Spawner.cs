using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float playerRadius;

    [SerializeField] private GameObject coinPrefab;

    [SerializeField] private int maxCoins;
    [SerializeField] private int areaX;
    [SerializeField] private int areaY;
    [SerializeField] private float interval;

    [Header("Elevation Settings")]
    [Tooltip("Which layers count as a platform/floor?")]
    [SerializeField] private LayerMask groundLayer = ~0;
    [Tooltip("How high above the platform should the coin hover?")]
    [SerializeField] private float coinHeightOffset = 0.5f;

    private float _duration;
    private int _totalSpawnedCoins;

    private void Start()
    {
        _duration = 0;
        _totalSpawnedCoins = 0;
    }

    private void Update()
    {
        if (transform.childCount >= maxCoins) return;

        _duration += Time.deltaTime;

        if (_duration >= interval)
        {
            PeriodicCoinsSpawn();
            _duration = 0;
        }
    }

    private void PeriodicCoinsSpawn()
    {
        Spawn();
        _totalSpawnedCoins++;
    }

    private void Spawn(GameObject specificCoin = null)
    {
        Vector3 spawnPos = Vector3.zero;
        GameObject coinToSpawn = specificCoin == null ? coinPrefab : specificCoin;

        int attempts = 0;
        bool foundValidPosition = false;

        // Safety limit of 50 attempts prevents Unity from freezing 
        // if the player's radius temporarily covers the whole spawn area.
        while (attempts < 50)
        {
            attempts++;

            // 1. Pick a random X and Z relative to the Spawner's position
            float x = Random.Range((float)-areaX, (float)areaX);
            float z = Random.Range((float)-areaY, (float)areaY);
            Vector3 randomTopPos = transform.position + new Vector3(x, 100f, z); // Start 100 units high

            // 2. Shoot a raycast straight down to find the platform height
            if (Physics.Raycast(randomTopPos, Vector3.down, out RaycastHit hit, 200f, groundLayer))
            {
                // Set the spawn position to the hit point + the coin's offset
                spawnPos = hit.point + new Vector3(0, coinHeightOffset, 0);

                // 3. Check if it's far enough from the player
                if (Vector3.Distance(spawnPos, player.position) >= playerRadius)
                {
                    foundValidPosition = true;
                    break;
                }
            }
        }

        if (foundValidPosition)
        {
            Instantiate(coinToSpawn, spawnPos, Quaternion.identity, transform);
        }
        else
        {
            Debug.LogWarning("Spawner skipped a coin: Could not find a valid spot away from the player.");
        }
    }

    // Draws the green spawn area box when you click the Spawner in the Scene view
    private void OnDrawGizmosSelected()
    {
        // Draw the Spawn Area Box
        Gizmos.color = new Color(0, 1, 0, 0.4f); // Semi-transparent green
        Vector3 center = transform.position;
        Vector3 size = new Vector3(areaX * 2, 50f, areaY * 2); // 50f tall just to see it easily
        Gizmos.DrawWireCube(center, size);

        // Draw a red sphere around the player to show the "No Spawn" zone
        if (player != null)
        {
            Gizmos.color = new Color(1, 0, 0, 0.3f);
            Gizmos.DrawWireSphere(player.position, playerRadius);
        }
    }
}