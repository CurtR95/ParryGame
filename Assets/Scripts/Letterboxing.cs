using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Letterboxing : MonoBehaviour
{
    void Start() {
        Slide("out");
    }
    // Update is called once per frame
    void Update()
    {
        float scale = Camera.main.orthographicSize/5.0f;
        transform.localScale = new Vector3(scale,scale,scale);
    }

    public void Slide(string slideMode) {
        Transform topLetterbox = transform.Find("LetterboxTop");
        Transform bottomLetterbox = transform.Find("LetterboxBottom");
        if (slideMode == "in") {
            StopCoroutine(nameof(SlideOut));
            StartCoroutine(SlideIn(topLetterbox.localPosition, bottomLetterbox.localPosition));
        }
        else if (slideMode == "out") {
            StopCoroutine(nameof(SlideIn));
            StartCoroutine(SlideOut(topLetterbox.localPosition, bottomLetterbox.localPosition));
        }
    }

    IEnumerator SlideIn(Vector3 topCurrentPosition, Vector3 bottomCurrentPosition) {
        Transform topLetterbox = transform.Find("LetterboxTop");
        Transform bottomLetterbox = transform.Find("LetterboxBottom");
        while (true) {
            topLetterbox.localPosition = Vector3.Lerp(topLetterbox.localPosition, topCurrentPosition - new Vector3(0,topLetterbox.localScale.y,0), Time.deltaTime*5);
            bottomLetterbox.localPosition = Vector3.Lerp(bottomLetterbox.localPosition, bottomCurrentPosition + new Vector3(0,bottomLetterbox.localScale.y,0), Time.deltaTime*5);
            yield return null;
        }
    }

    IEnumerator SlideOut(Vector3 topCurrentPosition, Vector3 bottomCurrentPosition) {
        Transform topLetterbox = transform.Find("LetterboxTop");
        Transform bottomLetterbox = transform.Find("LetterboxBottom");
        while (true) {
            topLetterbox.localPosition = Vector3.Lerp(topLetterbox.localPosition, topCurrentPosition + new Vector3(0,topLetterbox.localScale.y,0), Time.deltaTime);
            bottomLetterbox.localPosition = Vector3.Lerp(bottomLetterbox.localPosition, bottomCurrentPosition - new Vector3(0,bottomLetterbox.localScale.y,0), Time.deltaTime);
            yield return null;
        }
    }
}