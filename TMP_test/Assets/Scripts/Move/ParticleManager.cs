using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParticleManager : MonoBehaviour
{
    public Camera Cam_Particle;
    public GameObject p_touchEffect;
    float spawnTime;
    public float defaultTime = 0.05f;
    // 단순 터치 파티클
    public Transform particle_transform;
    public ParticleSystem particle_particle;
    Vector3 pos_particle;

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
        spawnTime += Time.deltaTime;

    }
    void MoveParticlePosition()
	{
        pos_particle = Cam_Particle.ScreenToWorldPoint(Input.mousePosition);
        pos_particle.z = 0;
        particle_transform.localPosition = pos_particle;
	}

    void particleCreate()
    {
        Vector3 mPosiiton = Cam_Particle.ScreenToWorldPoint(Input.mousePosition);
        mPosiiton.z = 0;
        p_touchEffect.transform.localPosition = mPosiiton;
        Instantiate(p_touchEffect, mPosiiton, Quaternion.identity, this.gameObject.transform);
    }
}
