using System.Collections.Generic;
using UnityEngine;

public static class TransformExtension {

    public static Transform GetClosestTranform(Vector3 currentPosition, Transform[] transforms) {
        return GetClosestObject(currentPosition, transforms);
    }

    public static Transform GetClosestTranform(Vector3 currentPosition, List<Transform> transforms) {
        return GetClosestTranform(currentPosition, transforms.ToArray());
    }


    public static T GetClosestObject<T>(Vector3 currentPosition, T[] objects) where T : Component {
        T bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        for (int i = 0; i < objects.Length; i++) {
            Vector3 directionToTarget = objects[i].transform.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr) {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = objects[i];
            }
        }

        return bestTarget;
    }

    public static T GetClosestObject<T>(Vector3 currentPosition, List<T> objects) where T : Component {
        return GetClosestObject(currentPosition, objects.ToArray());
    }
}
