using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meta.XR.MRUtilityKit;
using UnityEngine.SceneManagement;

public class OrbsSpawner : MonoBehaviour
{   
    public int numberOfOrbsToSpawn = 5;
    public GameObject orbPrefab;
    public float height;

    public List<GameObject> spawnedOrbs;

    public int maxNumberOfTry = 100;
    private int currentNumberOfTry = 0;

    public static OrbsSpawner instance;

    private void Awake()
    {
        instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MRUK.Instance.RegisterSceneLoadedCallback(SpawnOrbs);
    }

    async public void DestroyOrb(GameObject orb)
    {
        spawnedOrbs.Remove(orb);
        Destroy(orb);

        if (spawnedOrbs.Count == 0)
        {
            LeaderboardManager leaderboardManager = GameObject.FindGameObjectWithTag("LeaderboardManager").GetComponent<LeaderboardManager>();
            TimeManager timeManager = GameObject.FindGameObjectWithTag("TimeManager").GetComponent<TimeManager>();
            
            leaderboardManager.UpdateLeaderboard(timeManager.time);
            leaderboardManager.lastDeath = timeManager.totalTime;
            await UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(1);
            UnityEngine.SceneManagement.SceneManager.LoadScene(1, UnityEngine.SceneManagement.LoadSceneMode.Additive);
        }
    }

    public void SpawnOrbs()
    {
        for (int i = 0; i < numberOfOrbsToSpawn; i++)
        {
            Vector3 randomPosition = Vector3.zero;

            MRUKRoom room = MRUK.Instance.GetCurrentRoom();

            while (currentNumberOfTry < maxNumberOfTry)
            {
                bool hasFound = room.GenerateRandomPositionOnSurface(MRUK.SurfaceType.FACING_UP,
                    1, new LabelFilter(MRUKAnchor.SceneLabels.FLOOR), out randomPosition, out Vector3 n);

                if (hasFound)
                    break;

                currentNumberOfTry++;
            }

            randomPosition.y = height;

            GameObject spawned = Instantiate(orbPrefab, randomPosition, Quaternion.identity);
            spawnedOrbs.Add(spawned);

            SceneManager.MoveGameObjectToScene(spawned, SceneManager.GetSceneByBuildIndex(1));

        }  
    }
}
