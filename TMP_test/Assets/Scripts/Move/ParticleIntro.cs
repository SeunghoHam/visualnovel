using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParticleIntro : MonoBehaviour
{
	[Header("AnimationIntro")]

	public GameObject Intro_Back;
	public GameObject Intro_Front;
	public Image img_Black;

	public ParticleSystem particle_Spawn;
	public ParticleSystem particle_Twinkle;
	public ParticleSystem particle_Correct;
	public ParticleSystem particle_Heart;


	public int introCount;
	public bool introTouch;

	private void OnEnable()
	{
		IntroInit();

	}

	void IntroInit()
	{
		img_Black.gameObject.SetActive(true);
		Intro_Back.SetActive(false);
		Intro_Front.SetActive(false);

		particle_Twinkle.gameObject.SetActive(true);
		particle_Spawn.gameObject.SetActive(true);
		img_Black.GetComponent<Image>().color = new Color(0, 0, 0, 1);
		Intro_Back.transform.localPosition = new Vector3(-1080f, 0, 0);
		Intro_Front.transform.localPosition = new Vector3(1080f, 0, 0);
		particle_Twinkle.Play();
		introTouch = true;
	}
	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

		if (Input.GetKeyDown(KeyCode.J)) // 하트 파티클 색 다르게 보이는거 하려는건데 상황 보고 쓸듯
		{
			//particle_Heart.startColor = new Color(1, 0, 0, 1);
			particle_Heart.Play();

		}
		if (Input.GetKeyDown(KeyCode.K))
		{
			//particle_Heart.startColor = new Color(0, 0, 1, 1);
			particle_Heart.Play();

		}
		if (Input.GetMouseButtonDown(0) && introTouch)
		{
			introTouch = false;

			switch (introCount)
			{
				case 0:
					StartCoroutine(CRT_Intro());
					break;
				case 1:
					Debug.Log("미연시로 전환");
					//StartCoroutine(CRT_DialogueIntro());
					break;
				case 2:
					Debug.Log("미연시로 진행중이니까 나오면안됨");

					break;
				default:
					break;
			}
			introCount++;

		}
		if (Input.GetKeyDown(KeyCode.S)) // 인트로 스킵 버튼
		{
			Debug.Log("스킵버튼");
			//StartCoroutine(CRT_DialogueIntro());
		}
	}
	IEnumerator CRT_Intro()
	{
		//particle_Spawn.Play();
		StartCoroutine(CRT_ChangeColor(img_Black.GetComponent<Image>(), 0)); // 흰색 배경 되기
		yield return new WaitForSeconds(0.8f);
		StartCoroutine(CRT_ChangeColor(img_Black.GetComponent<Image>(), 1)); // 검정색 배경 되기
		yield return new WaitForSeconds(0.5f);
		particle_Twinkle.Stop();
		Intro_Front.SetActive(true);
		Intro_Back.SetActive(true);
		yield return new WaitForSeconds(0.5f);
		LeanTween.moveLocal(Intro_Front, Vector3.zero, 0.8f);
		LeanTween.moveLocal(Intro_Back, Vector3.zero, 0.8f);
		yield return new WaitForSeconds(0.8f);


		particle_Spawn.Play();
		particle_Correct.Play();
		//particle_Spawn.Stop();
		introTouch = true;
	}

	IEnumerator CRT_ChangeColor(Image image, int i) // 0 = 흰색되기, 1 = 검정색되기
	{
		if (i == 0)
		{
			while (image.color.r < 1)
			{
				var color = image.color;
				color.r += (2 * Time.deltaTime);
				color.g += (2 * Time.deltaTime);
				color.b += (2 * Time.deltaTime);
				image.color = color;
				yield return null;

			}
		}
		else if (i == 1)
		{
			while (image.color.r > 0)
			{
				var color = image.color;
				color.r -= (2 * Time.deltaTime);
				color.g -= (2 * Time.deltaTime);
				color.b -= (2 * Time.deltaTime);
				image.color = color;
				yield return null;
			}
		}
	}
}
