using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class particle_Star : MonoBehaviour
{
    Vector2 direction;
    public float moveSpeed = 0.05f;
    public float minSize = 2f;
    public float maxSize = 4f;
    public float sizeSpeed = 1f;

    public float minDir;
    public float maxDir;

    SpriteRenderer img_touchEffect;
    public Color[] colors;
    public float colorSpeed = 1f;

	private void Awake()
	{
        img_touchEffect = GetComponent<SpriteRenderer>();
    }
	// Start is called before the first frame update
	void Start()
    {
        direction = new Vector2(Random.Range(minDir, maxDir), Random.Range(minDir, maxDir));
        float size = Random.Range(minSize, maxSize);
        this.transform.localScale = new Vector2(size, size);
        img_touchEffect.color = colors[Random.Range(0, colors.Length)];
    }

    // Update is called once per frame
    void Update()
    {

        this.transform.Translate(direction * moveSpeed);
        transform.localScale = Vector2.Lerp(transform.localScale, Vector2.zero, Time.deltaTime);

        Color color = img_touchEffect.color;
        color.a = Mathf.Lerp(img_touchEffect.color.a, 0, Time.deltaTime * colorSpeed);
        img_touchEffect.color = color;

        if (img_touchEffect.color.a <= 0.01f)
            Destroy(this.gameObject);
    }
}
