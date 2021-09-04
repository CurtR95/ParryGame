using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControlZones : MonoBehaviour
{
    public float Zoom = 5.0f;
    public bool staticCamera = false;
    public Vector2 staticCameraPosition;
    private Coroutine lastRoutine = null;
    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.CompareTag("Player")){
            if (staticCamera){
                StartCoroutine(nameof(TriggerStatic));
                Camera.main.transform.position = new Vector3 (staticCameraPosition.x, staticCameraPosition.y, -10);
            }
            else {
                lastRoutine = StartCoroutine(Camera.main.GetComponent<CameraFollow>().FollowTarget());
            }
            StartCoroutine(nameof(TriggerZoom));
       }
    }
    void OnTriggerExit2D(Collider2D collider) {
        if (lastRoutine != null) StopCoroutine(lastRoutine);
        StopCoroutine(nameof(TriggerStatic));
        StopCoroutine(nameof(TriggerZoom));
    }
    
    IEnumerator TriggerStatic(){
        while(true) {
            Vector3 cameraPosition = Camera.main.transform.position;
            Camera.main.transform.position = Vector3.Lerp(cameraPosition, new Vector3(staticCameraPosition.x, staticCameraPosition.y, -10), Time.deltaTime * 5f);
            yield return null;
        }
    }
    IEnumerator TriggerZoom () {
        while(true) {
            float diff = Zoom-Camera.main.orthographicSize;
            Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, Zoom+diff, Time.deltaTime * 1f);
            yield return null;
        }
    }
}
