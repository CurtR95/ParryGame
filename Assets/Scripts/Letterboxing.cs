using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Letterboxing : MonoBehaviour
{
    // Update is called once per frame
    // BUG this shit is fucked.
    void Update()
    {
        GameObject top = transform.Find("LetterboxTop").gameObject;
        GameObject bottom = transform.Find("LetterboxBottom").gameObject;
        Debug.Log(Camera.main.WorldToScreenPoint(top.transform.position));
        Vector3 newTop = Camera.main.ScreenToWorldPoint(new Vector3 (top.transform.position.x, 509.7f,top.transform.position.z));
        top.transform.position = new Vector3(newTop.x,newTop.y,-10);
        // Camera.main.orthographicSize
    }
}
