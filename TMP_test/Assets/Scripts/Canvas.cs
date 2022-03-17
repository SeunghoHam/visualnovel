using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Canvas : MonoBehaviour
{
	public Transform particle_transform;
	public ParticleSystem particle_particle;
	public bool  dragPlayMode;
	Vector3 pos_particle;




	// Start is called before the first frame update

	private void Awake()
	{

	}
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		if(Input.GetMouseButtonDown(0))
		{
			particle_particle.Play();
		}
		
	}

	void MoveParticlePosition()
	{
		//pos_particle = Camera.main
		pos_particle.z = 0;

		particle_transform.localPosition = pos_particle;


	}

}
