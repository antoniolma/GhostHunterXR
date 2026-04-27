using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meta.XR.MRUtilityKit;
using UnityEngine.SceneManagement;
using Unity.Collections;

public class GhostSpawner : MonoBehaviour
{
    public GameObject ghostPrefab;
    public GameObject ghostUpgradePrefab;
    public float spawnTime = 1;

    public float minEdgeDistance = 0.3f;
    public MRUKAnchor.SceneLabels spawnLabel;
    public float normalOffset;
    private int spawnTry = 1000;

    public List<GameObject> spawnedGhosts;
    public int maxGhostNumber;

    private float timer;

    public static GhostSpawner Instance;

    private void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (!MRUK.Instance && !MRUK.Instance.IsInitialized)
            return;

        timer += Time.deltaTime;
        if (timer > spawnTime && spawnedGhosts.Count < maxGhostNumber)
        {
            SpawnGhost();
            timer -= spawnTime;
        }
    }

    void SpawnGhost()
    {
        MRUKRoom room = MRUK.Instance.GetCurrentRoom();

        int currentTry = 0;
        bool hasFoundPosition = false;
        float ghostRandomizer = Random.Range(0.0f, 1.0f);

        while (currentTry < spawnTry)
        {
            hasFoundPosition = room.GenerateRandomPositionOnSurface(MRUK.SurfaceType.VERTICAL, minEdgeDistance, new LabelFilter(spawnLabel), out Vector3 pos, out Vector3 norm);
            
            if (hasFoundPosition)
            {
                Vector3 randomPostionNormalOffset = pos + norm * normalOffset;
                randomPostionNormalOffset.y = 0;
                
                GameObject ghost;
                if (ghostRandomizer < 0.8)
                {
                    ghost = Instantiate(ghostPrefab, randomPostionNormalOffset, Quaternion.identity);
                } else
                {
                    ghost = Instantiate(ghostUpgradePrefab, randomPostionNormalOffset, Quaternion.identity);
                    ghost.GetComponent<Ghost>().speed = 0.5f;
                }

                SceneManager.MoveGameObjectToScene(ghost, SceneManager.GetSceneByBuildIndex(1));
                spawnedGhosts.Add(ghost);
                return;
            } else
            {
                currentTry++;
            }
        }
    }
}
