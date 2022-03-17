using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParticleManager : MonoBehaviour
{
    public Camera depthCam;
    public GameObject p_touchEffect;
    public GameObject p_featherEffect;
    public GameObject go_Feather;
    float spawnTime;
    public float defaultTime = 0.05f;

    //public Image img_White;

    WaitForSeconds spawnDelay = new WaitForSeconds(0.01f);

    // 단순 터치 파티클
    public Transform particle_transform;
    public ParticleSystem particle_particle;
    public bool dragPlayMode;
    Vector3 pos_particle;

    void Start()
    {
        //img_White.color = new Color(1, 1, 1, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
		{
            MoveParticlePosition();
            particle_particle.Play();
		}
        if (Input.GetMouseButton(0) && spawnTime >= defaultTime)
        {
            particleCreate();
            spawnTime = 0f;

            //DebugRect();
        }
        if(Input.GetKeyDown(KeyCode.P))
		{
            //img_White.color = new Color(1, 1, 1, 0);
            StartCoroutine(CRT_particleFeather());
		}

        spawnTime += Time.deltaTime;

    }
    void MoveParticlePosition()
	{
        pos_particle = depthCam.ScreenToWorldPoint(Input.mousePosition);
        pos_particle.z = 0;
        particle_transform.localPosition = pos_particle;
	}

    void particleCreate()
    {

        Vector3 mPosiiton = depthCam.ScreenToWorldPoint(Input.mousePosition);
        //Vector3 mPosiiton = Input.mousePosition;
        mPosiiton.z = 0;

        p_touchEffect.transform.localPosition = mPosiiton;
        //Debug.Log(mPosiiton);
        Instantiate(p_touchEffect, mPosiiton, Quaternion.identity, this.gameObject.transform);
    }

    IEnumerator CRT_particleFeather()
	{
        go_Feather.transform.position = new Vector3(Screen.width, 0, 0);
        LeanTween.move(go_Feather, new Vector3(0, Screen.height, 0), 1f);
       // StartCoroutine(Show_Delay_Image(img_White, 1f));
        for (int i = 0; i < 60; i++)
		{
            FeatherOn();
            yield return spawnDelay;
        }
        
        yield break;
    }
    void FeatherOn()
	{
        Vector3 featherPos = new Vector3(go_Feather.transform.position.x, go_Feather.transform.position.y,+10);
        Vector3 mPosition = depthCam.ScreenToWorldPoint(featherPos);
        //p_featherEffect.transform.position = featherPos;
        Instantiate(p_featherEffect, mPosition, Quaternion.identity, this.gameObject.transform);
    }

    IEnumerator Show_Delay_Image(Image image, float speed)
    {
        while (image.color.a < 1)
        {
            var color = image.color;
            color.a += (speed * Time.deltaTime);

            image.color = color;
            yield return null;
        }
    }

}
