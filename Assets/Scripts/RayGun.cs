using System.Collections;
using System.Collections.Generic;
// using System.Numerics;
using UnityEngine;

public class RayGun : MonoBehaviour
{
    public LayerMask layerMask;
    public OVRInput.RawButton shootingButton;
    public LineRenderer linePrefab;
    public GameObject rayImpactPrefab;
    public Transform shootingPoint;
    public float maxLineDistance = 5;
    public float lineShowTimer = 0.3f;

    List<float> rayAngles = new List<float>();
    private float currentSpreadOffset = 0.2f;

    public AudioSource audioSource;
    public AudioClip laserSound;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rayAngles.Add(0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(shootingButton))
        {
            ShootSpread();
        }
    }

    public void ShootSpread()
    {
        foreach (float offset in rayAngles)
        {
            Vector3 angle = new Vector3(offset + shootingPoint.forward.x, shootingPoint.forward.y, shootingPoint.forward.z);
            Shoot(angle);
        }
    }

    public void Shoot(Vector3 angle)
    {
        audioSource.PlayOneShot(laserSound);

        Ray ray = new Ray(shootingPoint.position, angle);
        bool hasHit = Physics.Raycast(ray, out RaycastHit hit, maxLineDistance, layerMask);

        Vector3 endPoint = Vector3.zero;

        if (hasHit) {
            // Stops the ray, when hitting the mask
            endPoint = hit.point;
    
            Ghost ghost = hit.transform.GetComponent<Ghost>();
            Coffee coffee = hit.transform.GetComponent<Coffee>();
            if (ghost)
            {
                hit.collider.enabled = false;
                ghost.Kill();

                if (ghost.name.Contains("Upgrade"))
                {
                    rayAngles.Add(currentSpreadOffset);
                    rayAngles.Add(-currentSpreadOffset);
                    currentSpreadOffset += 0.2f;
                }
            }
            else if (coffee)
            {
                hit.collider.enabled = false;
                coffee.Kill();
            }
            else
            {
                Quaternion rayImpactRotation = Quaternion.LookRotation(-hit.normal);
                GameObject rayImpact = Instantiate(rayImpactPrefab, hit.point, rayImpactRotation);
                Destroy(rayImpact, 1);
            }

        } else
            // Else, stops at maxDistance
            endPoint = shootingPoint.position + angle * maxLineDistance;

        LineRenderer line = Instantiate(linePrefab);
        line.positionCount = 2;
        line.SetPosition(0, shootingPoint.position);

        line.SetPosition(1, endPoint);

        Destroy(line.gameObject, lineShowTimer);
    }
}
