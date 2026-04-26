using System.Collections.Generic;
using UnityEngine;
using Meta.XR.MRUtilityKit;
using UnityEditor.ProjectWindowCallback;

public class CoffeeSpawner : MonoBehaviour
{
    public int numberOfCoffeeToSpawn = 5;
    public GameObject coffeePrefab;
    public float minHeight = 0.5f;
    public float maxHeight = 2.0f;

    public List<GameObject> spawnedCoffee;

    public List<GameObject> spawnedGhosts;
    public GhostSpawner ghostSpawner;
    public float timeStarted;
    private bool ended = false;

    public int maxNumberOfTry = 100;
    private int currentNumberOfTry = 0;

    public static CoffeeSpawner Instance;

    void Awake()
    {
        Instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MRUK.Instance.RegisterSceneLoadedCallback(SpawnCoffee);
    }

    void Update()
    {
        spawnedGhosts = ghostSpawner.spawnedGhosts;
        if (!ended && Time.deltaTime > timeStarted + 4.0f)
        {
            endEffect();
            ended = true;
        }
    }

    public void DestroyCoffee(GameObject coffee)
    {
        getEffect();
        spawnedCoffee.Remove(coffee);
        Destroy(coffee);
    }

    public void SpawnCoffee()
    {
        for (int i = 0; i < numberOfCoffeeToSpawn; i++)
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

            randomPosition.y = Random.Range(minHeight, maxHeight);

            GameObject spawned = Instantiate(coffeePrefab, randomPosition, Quaternion.identity);
            spawnedCoffee.Add(spawned);

        }  
    }

    public void getEffect()
    {
        // Ghosts
        foreach (GameObject ghost in spawnedGhosts) {
            ghost.GetComponent<Ghost>().ApplySlow(1);
        }

        timeStarted = Time.deltaTime;
        ended = false;
    }

    public void endEffect()
    {
        // Ghosts
        foreach (GameObject ghost in spawnedGhosts) {
            ghost.GetComponent<Ghost>().ApplySlow(0);
        }
    }
}
