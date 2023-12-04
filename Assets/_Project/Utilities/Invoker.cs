using System;
using System.Collections;
using UnityEngine;

public static class Invoker {
    public static IEnumerator WaitAndInvoke(Action action, float delay) {
        yield return new WaitForSeconds(delay);
        action.Invoke();
    }

    public static IEnumerator WaitForTrue(Func<bool> condition, float maxTime, Action onSuccess, Action onFailure) {
        float time = 0;

        while (condition() == false && time < maxTime) {
            yield return null;
            time += Time.deltaTime;
        }

        if (time >= maxTime && onFailure != null)
            onFailure.Invoke();
        else if (onSuccess != null)
            onSuccess.Invoke();
    }

    public static IEnumerator WaitForTrue(Func<bool> condition, float maxTime, Action onSuccess) {
        float time = 0;

        while (condition() == false && time < maxTime) {
            yield return null;
            time += Time.deltaTime;
        }

        onSuccess.Invoke();
    }
}