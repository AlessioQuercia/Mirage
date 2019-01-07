using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireAnimationScript : MonoBehaviour {
    SpriteRenderer fireObject;
    IEnumerator coroutine;

    // Use this for initialization
    void Start () {
        fireObject = this.GetComponent<SpriteRenderer>();
        coroutine = animate(fireObject);
        StartCoroutine(coroutine);
    }

    // Update is called once per frame
    void Update () {

	}

    private IEnumerator animate(SpriteRenderer spriteRenderer)
    {
        float value = Random.Range(0.5f, 1.0f);
        yield return new WaitForSeconds(value);
        spriteRenderer.flipX = !spriteRenderer.flipX;
        StartCoroutine(animate(spriteRenderer));
    }
}
