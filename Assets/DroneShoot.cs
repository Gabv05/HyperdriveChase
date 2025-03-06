using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneShoot : MonoBehaviour
{

    public GameObject laserPrefab;
    public Transform laserSpawn;

    private GameObject currentLaser;
    private droneStatsScript droneStats;
    // Start is called before the first frame update
    void Start()
    {
        droneStats = GetComponent<droneStatsScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (droneStats.inRange) {
            shootLaser();
        } /*else {
                Destroy(currentLaser);
        }
        */
    }

    void shootLaser() {

        if (currentLaser == null) {
            currentLaser = Instantiate(laserPrefab, laserSpawn.position, Quaternion.identity) as GameObject;
        }

        currentLaser.transform.position = laserSpawn.position + droneStats.directionToPlayer * (droneStats.distanceFromPlayer / 2);
        currentLaser.transform.rotation = Quaternion.LookRotation(droneStats.directionToPlayer);
        
        float laserLength = Mathf.Max(0.1f, droneStats.distanceFromPlayer);
        currentLaser.transform.localScale = new Vector3(0.1f, 0.1f, laserLength); //scale laser to player 
    }
}
