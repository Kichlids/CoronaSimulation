using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class test : MonoBehaviour
{
    NavMeshSurface s;

    public GameObject personPrefab;

    private void Start() {
        s = GetComponent<NavMeshSurface>();
    }

    private void Update() {

        if (Input.GetKeyDown(KeyCode.Q)) {
            GameObject inst = Instantiate(personPrefab, new Vector3(-10, 1, -11), Quaternion.identity);
        }

        //s.BuildNavMesh();
    }
}
