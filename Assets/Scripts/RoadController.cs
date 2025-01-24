using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class RoadController : MonoBehaviour
{
    [SerializeField] private GameObject[] gasObjects;

    private void OnDisable()
    {
        foreach (var gas in gasObjects)
        {
            gas.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.SpawnRoad(transform.position + new Vector3(0, 0, 10));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.DespawnRoad(gameObject);
        }
    }

    public void SpawnGas()
    {
        int index = Random.Range(0, gasObjects.Length);
        gasObjects[index].SetActive(true);
    }
}