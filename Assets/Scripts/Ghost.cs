using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ghost : MonoBehaviour
{
    public Animator animator;
    public NavMeshAgent agent;
    public float speed = 1;

    public float eatDistance = 0.2f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!agent.enabled)
            return;

        GameObject closest = GetClosestOrb();

        if (closest)
        {
            Vector3 targetPosition = closest.transform.position;

            agent.SetDestination(targetPosition);
            agent.speed = speed;
        }
        
    }

    public GameObject GetClosestOrb()
    {
        GameObject closest = null;
        float minDist = Mathf.Infinity;

        List<GameObject> orbs = OrbsSpawner.instance.spawnedOrbs;

        Vector3 ghostPosition = transform.position;
        ghostPosition.y = 0;

        foreach (var item in orbs)
        {
            Vector3 orbPosition = item.transform.position;
            orbPosition.y = 0;

            float d = Vector3.Distance(ghostPosition, orbPosition);

            if (d < minDist)
            {
                minDist = d;
                closest = item;
            }
        }

        if (minDist < eatDistance)
        {
            OrbsSpawner.instance.DestroyOrb(closest);
        }

        return closest;
    }

    public void ApplySlow(int type)
    {
        if (type == 1)
            speed /= 3;
        else
            speed *=3;
    }

    public void Kill()
    {
        agent.enabled = false;
        animator.SetTrigger("Death");
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
