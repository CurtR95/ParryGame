using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating(nameof(DirectionIndicator), 0f, 2f);
    }

    void DirectionIndicator() {
        if (transform.Find("DirectionSprite")) {
            Destroy(transform.Find("DirectionSprite").gameObject);
        }
        int randomDirection = Random.Range(0, 8);
        GameObject DirectionSprite = new GameObject();
        DirectionSprite.name = "DirectionSprite";
        DirectionSprite.transform.position = new Vector3(0, 3, 0);
        DirectionSprite.transform.parent = transform;
        DirectionSprite.AddComponent<SpriteRenderer>();
        Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites/DirectionGuide");
        DirectionSprite.GetComponent<SpriteRenderer>().sprite = sprites[randomDirection];
    }
}
