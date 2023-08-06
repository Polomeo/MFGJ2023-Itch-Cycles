using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISpriteAnimator : MonoBehaviour
{
    public Image image;

    public Sprite[] sprites;
    public float animSpeed = 0.2f;

    private int spriteIndex;
    private Coroutine coroutineAnim;
    private bool isDone;

    public void PlayUIAnim()
    {
        isDone = false;
        StartCoroutine(PlayAnimUI());
    }

    public void StopUIAnim()
    {
        isDone = true;
        StopCoroutine(PlayAnimUI());
    }

    IEnumerator PlayAnimUI()
    {
        // Wait the frame time
        yield return new WaitForSeconds(animSpeed);

        // If we are in the last frame ->
        // play the next to last so the house keeps burning
        if(spriteIndex >= sprites.Length)
        {
            spriteIndex = 7;
        }

        // Set the sprite
        image.sprite = sprites[spriteIndex];

        // Set the next sprite
        spriteIndex++;

        // Call the coroutine recursively
        if(!isDone)
        {
            coroutineAnim = StartCoroutine(PlayAnimUI());
        }
    }
}
