using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenScaleCamera : MonoBehaviour
{
    private void Start()
    {
        ScaleObjectToScreen(gameObject);
    }

    void ScaleObjectToScreen(GameObject obj)
    {
        float cameraHeight = Camera.main.orthographicSize * 2;
        float cameraWidth = cameraHeight * Camera.main.aspect;

        SpriteRenderer spriteRenderer = obj.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            float spriteWidth = spriteRenderer.sprite.bounds.size.x;

            Vector3 scale = obj.transform.localScale;
            scale.x = cameraWidth / spriteWidth;
            obj.transform.localScale = scale;
        }
    }
}
