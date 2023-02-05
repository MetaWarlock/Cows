using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject[] _enemyPrefabs; // array of enemy prefabs to choose from
    [SerializeField] float _timeBetweenWaves = 30f; // time between each wave
    [SerializeField] float _waveDelay = 0.5f; // delay between spawns within a wave
    [SerializeField] int _enemiesPerWave = 10; // number of enemies to spawn in a wave

    private float _countdown; // timer for the next wave
    private int _waveIndex = 0; // current wave number

    void Start()
    {
        _countdown = _timeBetweenWaves;
    }

    void Update()
    {
        // start next wave if countdown has reached 0
        if (_countdown <= 0f)
        {
            StartCoroutine(SpawnWave());
            _countdown = _timeBetweenWaves;
            return;
        }

        _countdown -= Time.deltaTime;
    }

    IEnumerator SpawnWave()
    {
        _waveIndex++;

        for (int i = 0; i < _enemiesPerWave; i++)
        {
            // choose a random enemy prefab to instantiate
            GameObject enemy = _enemyPrefabs[Random.Range(0, _enemyPrefabs.Length)];
            // spawn the enemy
            Instantiate(enemy, transform.position, transform.rotation);

            // wait for the specified delay before spawning the next enemy
            yield return new WaitForSeconds(_waveDelay);
        }
    }

    // method to start the next wave manually
    public void StartNextWave()
    {
        StartCoroutine(SpawnWave());
    }
}
