using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] citizensList;
    [SerializeField] int numberOfSpawns;
    void Start() {
        Spawn();
    }

    void Spawn() {
        while(numberOfSpawns > 0){
            Instantiate(citizensList[Random.Range(0, citizensList.Length)],this.transform.position*Random.Range(0,5),Quaternion.identity);
            numberOfSpawns--;
        }
    }
}
