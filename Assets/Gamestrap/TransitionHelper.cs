using UnityEngine;
using System;
using System.Collections;
/// <summary>
/// Helper class to do some transitions using coroutines.
/// </summary>
public class TransitionHelper : MonoBehaviour {

    public static IEnumerator Transition(float totalTime, Action<float> transition, Action callback = null)
    {
        float startTime = Time.time;
        float endTime = startTime + totalTime;

        if (totalTime > 0)
        {
            while (Time.time <= endTime)
            {
                float percentage = (Time.time - startTime) / totalTime;
                transition(percentage);
                yield return new WaitForEndOfFrame();
            }
        }
        // Call once more to make sure the t ends in 1
        transition(1f);
        if (callback != null)
        {
            callback();
        }
    }

}
