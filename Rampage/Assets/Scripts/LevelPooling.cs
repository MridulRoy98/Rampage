using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.SceneManagement;

public class LevelPooling : MonoBehaviour
{
    [SerializeField] private float offsetAmount = 0.5f;
    [SerializeField] private int offsetSpeed = 3;
    [SerializeField] private Vector3 BackTrigger;
    [SerializeField] private Vector3 SpawnPoint;

    void Update()
    {
        transform.position = new Vector3 (0, 0, transform.position.z + offsetAmount * (offsetSpeed * Time.deltaTime));

        //Trigger level chunk to move to spawnpoint
        if(transform.position.z >= BackTrigger.z)
        {
            transform.position = new Vector3(0, 0, transform.position.z - 144);
        }
    }
}
