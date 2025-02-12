using UnityEngine;

public class triggerScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject targetObject; // Reference to the other GameObject

    void OnTriggerEnter(Collider other)
    {
        // Access the script on the target object
        loadNewPathScript targetScript = targetObject.GetComponent<loadNewPathScript>();


        // Call the function on the target object
        Debug.Log("worked triggeer side"); 
        targetScript.SpawnPrefab();

    }
}
