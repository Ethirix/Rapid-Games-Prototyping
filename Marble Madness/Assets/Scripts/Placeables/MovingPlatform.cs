using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Tooltip("Time in seconds for the platform to move")]
    [Range(1f, 10f)]
    public float time = 5f;

    [Tooltip("Time to wait at start and end positions")]
    [Range(0f, 10f)]
    public float waitTime = 2f;

    private Vector3 startPos;
    private Vector3 endPos;

    private GameObject platformObject;

    Coroutine moveRoutine;

    private bool active = true;

    private void Start()
    {
        startPos = transform.position;
        endPos = transform.GetChild(1).position;

        platformObject = transform.GetChild(0).gameObject;

        moveRoutine = StartCoroutine(MoveRoutine());
    }

    IEnumerator MoveRoutine()
    {
        float currentTime = 0f;

        while(active)
        {
            while(currentTime < time)
            {
                platformObject.transform.position = Vector3.Lerp(startPos, endPos, currentTime / time);
                currentTime += Time.deltaTime;
                yield return null;
            }

            yield return new WaitForSeconds(waitTime);

            while(currentTime > 0f)
            {
                platformObject.transform.position = Vector3.Lerp(startPos, endPos, currentTime / time);
                currentTime -= Time.deltaTime;
                yield return null;
            }

            yield return new WaitForSeconds(waitTime);

        }
    }


}
