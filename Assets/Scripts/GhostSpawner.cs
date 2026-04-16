using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meta.XR.MRUtilityKit;

public class GhostSpawner : MonoBehaviour
{
    public GameObject ghostPrefab;
    public float spawnTime = 1;

    public float minEdgeDistance = 0.3f;
    public MRUKAnchor.SceneLabels spawnLabel;
    public float normalOffset;
    private int spawnTry = 1000;

    private float timer;

    // Update is called once per frame
    void Update()
    {
        if (!MRUK.Instance && !MRUK.Instance.IsInitialized)
            return;

        timer += Time.deltaTime;
        if (timer > spawnTime)
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

        while (currentTry < spawnTry)
        {
            hasFoundPosition = room.GenerateRandomPositionOnSurface(MRUK.SurfaceType.VERTICAL, minEdgeDistance, new LabelFilter(spawnLabel), out Vector3 pos, out Vector3 norm);
            
            if (hasFoundPosition)
            {
                Vector3 randomPostionNormalOffset = pos + norm * normalOffset;
                randomPostionNormalOffset.y = 0;

                Instantiate(ghostPrefab, randomPostionNormalOffset, Quaternion.identity);
                return;
            } else
            {
                currentTry++;
            }
        }


        
    }
}
