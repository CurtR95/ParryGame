using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float screenPadding = 3.0f, height = 3.0f;

    public IEnumerator FollowTarget() {
        while (true) {
            Vector3 wantedPosition = new Vector3(target.position.x, target.position.y+(height*Camera.main.orthographicSize/5.0f), -10);
            transform.position = Vector3.Lerp(Camera.main.transform.position, wantedPosition, Time.deltaTime * 5f);
            transform.position = wantedPosition;
            yield return null;
        }
    }
}
