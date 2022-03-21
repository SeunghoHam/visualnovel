using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
	public BoxCollider[]  col_Box;
	Vector3 vec_movePoint;
	public GameObject go_movePoint;

	private void Awake()
	{
		col_Box = new BoxCollider[4];
		for (int i = 0; i < this.gameObject.transform.childCount; i++)
		{
			col_Box[i] = this.gameObject.transform.GetChild(0).GetComponent<BoxCollider>();
			col_Box[i].isTrigger = false;
		}
		vec_movePoint = go_movePoint.transform.position;
	}
	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	public void fElevator()
	{
		Debug.Log("fElevator");
		for (int i = 0; i < col_Box.Length; i++)
		{
			col_Box[i].isTrigger = false;
		}
		Quaternion nyaong = Quaternion.Euler(this.transform.position);
		Quaternion mung = Quaternion.Euler(vec_movePoint);

		//this.transform.Translate(); Quaternion.Lerp(nyaong, mung, 1f);
		
	}
	/*
	private void OnCollisionEnter(Collision collision)
	{
		if(collision.transform.name == "Sphere_Object")
		{
			Debug.Log("´ê¾Æ¹ö·Ç´Ù");
			StartCoroutine(CRT_MoveElevator());
		}
	}

	IEnumerator CRT_MoveElevator()
	{
		yield return new WaitForSeconds(1f);
	}*/
}
