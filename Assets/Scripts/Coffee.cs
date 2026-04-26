using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class Coffee : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    public void Kill()
    {
        CoffeeSpawner.Instance.DestroyCoffee(gameObject);
    }
}
