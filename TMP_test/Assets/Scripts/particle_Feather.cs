using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particle_Feather : MonoBehaviour
{
    Vector2 direction;
    float moveSpeed = 0.05f;
    float minSize = 2f;
    float maxSize = 4f;
    float sizeSpeed = 1f;
    float minDir;
    float maxDir;
    SpriteRenderer sprite_touchEffect;
    public Color[] colors;
    float colorSpeed = 2f;
	// Start is called before the first frame update
	private void Awake()
	{
        sprite_touchEffect = GetComponent<SpriteRenderer>();

    }
    void Start()
    {
        direction = new Vector2(Random.Range(minDir, maxDir), Random.Range(minDir, maxDir));
        float size = Random.Range(minSize, maxSize);
        this.transform.localScale = new Vector2(size, size);
        sprite_touchEffect.color = colors[Random.Range(0, colors.Length)];

        //LeanTween.move(this.gameObject, new Vector2(0, Screen.height), 2f);
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Translate(direction * moveSpeed);
        transform.localScale = Vector2.Lerp(transform.localScale, Vector2.zero, Time.deltaTime);

        Color color = sprite_touchEffect.color;
        color.a = Mathf.Lerp(sprite_touchEffect.color.a, 0, Time.deltaTime * colorSpeed);
        sprite_touchEffect.color = color;

        if (sprite_touchEffect.color.a <= 0.01f)
            Destroy(this.gameObject);
    }
}
