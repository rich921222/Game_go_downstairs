using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorManager : MonoBehaviour
{
    
    [SerializeField] GameObject[] FloorPrefabs;

    public void SpawnFloor()
    {
        int r = Random.Range(0,100);
        if(r >= 30)
        {
            r = 0;
        }
        else
        {
            r = 1;
        }
        GameObject floor = Instantiate(FloorPrefabs[r],transform);
        floor.transform.position = new Vector3(Random.Range(-3.8f,3.8f),-6f,0f);
    }
}
