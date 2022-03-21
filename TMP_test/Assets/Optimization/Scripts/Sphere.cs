using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sphere : MonoBehaviour
{
    Rigidbody rigid;
	Camera cam;

	[SerializeField] float moveLengthZ;
	[SerializeField] float moveLengthX;


	bool isJumping;
	private void Awake()
	{
		rigid = GetComponent<Rigidbody>();
		cam = Camera.main;
	}
	// Start is called before the first frame update
	void Start()
    {
		cam.transform.eulerAngles = new Vector3(20, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
		cam.transform.position = this.gameObject.transform.position + new Vector3(0f, 3f, -5f);
		

        if(Input.GetKeyDown(KeyCode.Space))
		{
			isJumping = true;
			//rigid.AddForce(Vector3.up * 2);
		}
    }

	private void FixedUpdate()
	{
		CharacterMovement();
		CharacterJumping();
		
	}

	void CharacterJumping()
	{
		if(isJumping)
		{
			rigid.AddForce(new Vector3(0f, 10f, 0f), ForceMode.VelocityChange);
			isJumping = false;
		}
	}
	void CharacterMovement()
	{
		rigid.AddForce(new Vector3(moveLengthX,0,moveLengthZ));
		rigid.AddTorque(new Vector3(moveLengthZ * 30, 0, -moveLengthX * 30));

		if (Input.GetKey(KeyCode.W))
		{
			moveLengthZ = 2f;
		}
		else if(Input.GetKey(KeyCode.S))
		{
			moveLengthZ = -2f;
		}
		else moveLengthZ = 0f;

		if (Input.GetKey(KeyCode.D))
		{
			moveLengthX = 2f;
		}
		else if (Input.GetKey(KeyCode.A))
		{
			moveLengthX = -2f;
		}
		else moveLengthX = 0f;
	}

	
	private void OnCollisionEnter(Collision collision)
	{
		if (collision.transform.name == "col_Center")
		{
			collision.transform.GetChild(0).gameObject.SetActive(true);
			StartCoroutine(CRT_ElevatorOn(collision.transform.position));

			collision.transform.parent.gameObject.GetComponent<Elevator>().fElevator();
			collision.gameObject.GetComponent<BoxCollider>().isTrigger = true;
		}
	}

	private void OnCollisionStay(Collision collision)
	{
		
	}

	private void OnCollisionExit(Collision collision)
	{
		
	}


	IEnumerator CRT_ElevatorOn(Vector3 pos)
	{
		yield return new WaitForSeconds(1f);
		this.gameObject.transform.position = pos;
	}
}
