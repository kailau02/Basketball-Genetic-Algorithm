using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetControl : MonoBehaviour
{
    public Game game;
    public Color defaultColor;
    public Color winColor;
    public SpriteRenderer targetSprite;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag.Equals("ball")) {
            targetSprite.color = winColor;
            StartCoroutine(game.waitForNextRound(true));
        }
    }

    public void randomizePosition() {
        targetSprite.color = defaultColor;
        float newX = Random.Range(0f, 6f);
        float newY = Random.Range(0f, 3f);
        transform.position = new Vector3(newX, newY, 0f);
    }

    public void resetColor() {
        targetSprite.color = defaultColor;
    }

}
