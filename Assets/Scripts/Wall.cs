using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{

    private SpriteRenderer spriteRenderer;
    public int hp = 4;
    public Sprite damagedSprite;

    // Start is called before the first frame update
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void TakeDamage(int loss)
    {
        hp -= loss;
        spriteRenderer.sprite = damagedSprite;
        if(hp <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}
