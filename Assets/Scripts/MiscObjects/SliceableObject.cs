using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliceableObject : MovingObject
{
	// Use this for initialization
	void Start () {
        this.BaseStart();
	}
	
	// Update is called once per frame
	void Update () {
        this.BaseUpdate();
	}

    private void OnTriggerExit2D(Collider2D collision)
    {
        var slicer = collision.GetComponent<SliceDetector>();
        if (slicer != null)
        {
            var slice = slicer.GetSlice();
            if (slice != null)
            {
                RaycastHit2D hit = Physics2D.Linecast(slice.Value.Start, slice.Value.End);
                if (hit.collider != null && hit.collider.gameObject == this.gameObject)
                {
                    this.SliceInHalf(slice.Value);
                }
            }
        }
    }

    private void SliceInHalf(SliceInfo slice)
    {
        float diffX = slice.End.x - slice.Start.x;
        float slope = (slice.End.y - slice.Start.y) / (diffX == 0.0f ? 0.001f : diffX);
        SliceableObject half = Instantiate(this.gameObject).GetComponent<SliceableObject>();
        Sprite halfSprite = half.GetComponent<SpriteRenderer>().sprite;
        Sprite thisSprite = this.GetComponent<SpriteRenderer>().sprite;
        Texture2D halfTex = (Texture2D)Instantiate(halfSprite.texture);
        Texture2D thisTex = (Texture2D)Instantiate(thisSprite.texture);
        int w = halfTex.width;
        int h = halfTex.height;
        Color[] halfPixels = halfTex.GetPixels();
        Color[] thisPixels = thisTex.GetPixels();
        
        for (int x = 0; x < w; x++)
        {
            for (int y = 0; y < h; y++)
            {
                if (y - (h / 2) > slope * (x - (w / 2)))
                {
                    halfPixels[x + y * w] = new Color(0, 0, 0, 0);
                }
                else
                {
                    thisPixels[x + y * w] = new Color(0, 0, 0, 0);
                }
            }
        }

        halfTex.SetPixels(halfPixels);
        thisTex.SetPixels(thisPixels);

        thisTex.Apply();
        halfTex.Apply();

        half.GetComponent<SpriteRenderer>().sprite = Sprite.Create(halfTex, halfSprite.rect, new Vector2(0.5f, 0.5f));
        this.GetComponent<SpriteRenderer>().sprite = Sprite.Create(thisTex, thisSprite.rect, new Vector2(0.5f, 0.5f));        

        half.GetComponent<Rigidbody2D>().gravityScale = 1.0f;
        this.GetComponent<Rigidbody2D>().gravityScale = 1.0f;
    }
}
