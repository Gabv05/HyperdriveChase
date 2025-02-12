using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class loadNewPathScript : MonoBehaviour
{
    // Switch these out for sections of path
    public GameObject myPrefab1;
    public GameObject myPrefab2;
    public GameObject myPrefab3;
    public GameObject myPrefab4;
    public GameObject myPrefab5;
    public GameObject myPrefab6;
    
    // Empty GameObject with trigger on
    public GameObject triggerGate;

    //Sets starting point at 0, and keeps track of where to place next path
    private int currentEnd = 0;
    private List<int> recentSelections = new List<int>() { -1, -1, -1}; // Track last three selections
    private GameObject[] typesList;
    private List<GameObject> spawnedPrefabs = new List<GameObject>(); // Store instantiated prefabs
    public float prefabLifetime = 5f; // Time before prefabs are removed
    public int maxPrefabs = 5; // Maximum number of prefabs allowed at a time

    void Start()
    {
        typesList = new GameObject[] { myPrefab1, myPrefab2, myPrefab3, myPrefab4, myPrefab5, myPrefab6 };

        // Spawn initial 5 prefabs at the start
        for (int i = 0; i < maxPrefabs; i++)
        {
            SpawnPrefab();
        }
        // Spawn first gate far away
        triggerGate.transform.position = new Vector3(0f, 5f, 51f);
    }

    void Update()
    {
    }

    public void SpawnPrefab()
    {

        GameObject selectedPrefab = SelectPath();
        if (selectedPrefab == null)
        {
            Debug.LogWarning("No prefab selected!");
            return;
        }

        Vector3 spawnPosition = new Vector3(0, 2, currentEnd);
        GameObject newInstance = Instantiate(selectedPrefab, spawnPosition, Quaternion.identity);
        spawnedPrefabs.Add(newInstance);

        StartCoroutine(DestroyAfterTime(newInstance, prefabLifetime));

        // Remove oldest prefab if exceeding max limit
        if (spawnedPrefabs.Count > maxPrefabs && spawnedPrefabs[0] != null)
        {
            Destroy(spawnedPrefabs[0]);
            spawnedPrefabs.RemoveAt(0);
        }

        // Moves gate to next prefab and keeps track of where to spawn next prefab
        triggerGate.transform.position += new Vector3(0, 0, 12f);
        currentEnd += 12;
    }

    // Randomly selects prefab to spawn next
    GameObject SelectPath()
    {
        int next;
        do
        {
            next = Random.Range(0, typesList.Length);
        } while (recentSelections.Contains(next));

        recentSelections.RemoveAt(0);
        recentSelections.Add(next);

        return typesList[next];
    }

    // Destroys after time (might not be necessary)
    IEnumerator DestroyAfterTime(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (obj != null)
        {
            spawnedPrefabs.Remove(obj);
            Destroy(obj);
        }
    }
}