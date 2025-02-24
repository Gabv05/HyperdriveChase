using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneMovementScript : MonoBehaviour
{

    GameObject player;
    public droneStatsScript droneStats;

    // Start is called before the first frame update
    void Start()
    {
        droneStats = GetComponent<droneStatsScript>(); // access droneStatsScript
        player = GameObject.Find("Player");  //find player gameobject
        droneStats.droneSpeed = 5.0f; //set speed of drone
        droneStats.rotationSpeed = 2.0f; //set rotation speed of drone
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerPosition = player.transform.position; //get players position
        Vector3 dronePosition = transform.position; //get drones position
        Vector3 direction = playerPosition - dronePosition; //get direction to player 
        direction.Normalize(); //normalize direction
        if (Vector3.Distance(playerPosition, dronePosition) > 5.0f) {  //if distance between player and drone is greater than 20
            transform.position += direction * droneStats.droneSpeed * Time.deltaTime; //move drone towards player
        } 
        
        Quaternion rotation = Quaternion.LookRotation(direction); //get rotation to look at player
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * droneStats.rotationSpeed); //rotate drone to look at player
    }
}
