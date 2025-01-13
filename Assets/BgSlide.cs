using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgSlide : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private float startX;
    private float width;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        startX = transform.position.x;
        width = spriteRenderer.bounds.size.x;
    }

    private void Update()
    {
        // Move the sprite
        transform.Translate(Vector3.left * GameManager.Instance.gameSpeed * Time.deltaTime);

        // Reset position when it moves too far
        if (transform.position.x < startX - width)
        {
            transform.position = new Vector3(startX, transform.position.y, transform.position.z);
        }
    }
}