using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public static class SimLib
{
    public static float ConvertToInGameLength(float val) {
        return val / 2 * 0.7f;
    }

    public static List<GameObject> FindNearbyPeople(GameObject gm, float radius) {
        Collider[] colliders = Physics.OverlapSphere(gm.transform.position, radius, LayerMask.GetMask("Person"));

        List<GameObject> nearby = new List<GameObject>();

        foreach (Collider c in colliders) {
            if (c.gameObject.GetInstanceID() != gm.GetInstanceID()) {
                nearby.Add(c.gameObject);
            }
        }

        return nearby;
    }
}
