using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
//using System.Text;

public class Dialogue : MonoBehaviour
{
	[Header("Text")]
	public TextMeshProUGUI text_Dialogue;
	public string[] arr;

	[Header("GameObject")]
	public Image img_bg; //��� �̹���
	public Image img_bg2; // ��� �̹���2 

	public Sprite sprite_bg; // ��� ��������Ʈ��


	public Image img_Character; // ĳ���� �̹��� 
	public Sprite[] sprite_Character; // ĳ���� ��������Ʈ��

	public Image img_textBG;



	[Header("BackGround")]
	public Image[] img_bgList;

	public Button btn1;
	public Button btn2;
	public Button btn_Return;
	public Button btn_Exit;

	public int touchCount;
	public bool canTouch;



	public bool isKind;
	public bool isKind2;


	public bool isAutoStart;
	// ����
	float ShakeAmount; // ��鸮������
	float ShakeTime; // ��鸮�� �ð�
	Vector2 initialPosition; // ����

	Color textColor_Idol = new Color(1, 0, 0, 1);



	public Camera Cam_UI;

	private Canvas canvas;
	public string Readtxt(string filePath) // Chat.txt ������ �������� �Լ�, �� ������ '>' �� �����Ѵ�.
	{
		FileInfo fileInfo = new FileInfo(filePath);
		string value = "";

		if (fileInfo.Exists)
		{
			StreamReader reader = new StreamReader(filePath);
			value = reader.ReadLine();
			reader.Close();
			Debug.Log("Chat.txt ���� ����");
			arr = value.Split('>'); //line.Split(' ');
		}
		else
			value = "Assets/Resources/ ��ο� Chat.txt ������ ����";
		return value;
	}

	private void Awake()
	{
		Readtxt("Assets/Resources/Chat.txt");
		ShakeAmount = 0.2f;
		//Init();

		canvas = GetComponent<Canvas>();
	}

	private void OnEnable()
	{
		Init();
	}
	void Init()
	{
		Debug.Log("init");
		initialPosition = img_bg.transform.position;
		touchCount = 0;
		canTouch = true;

		img_bg.color = new Color(1, 1, 1, 1);
		img_bg2.color = new Color(1, 1, 1, 0);
		img_bg.gameObject.SetActive(true);
		img_bg2.gameObject.SetActive(true);

		btn1.onClick.AddListener(btn_Choose1);
		btn2.onClick.AddListener(btn_Choose2);
		btn_Return.onClick.AddListener(btnClick_ReGame);
		btn_Exit.onClick.AddListener(btnClick_Exit);
		btn1.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = arr[4].ToString();
		btn2.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = arr[5].ToString();

		btn1.gameObject.SetActive(false);
		btn2.gameObject.SetActive(false);
		btn_Return.gameObject.SetActive(false);
		btn_Exit.gameObject.SetActive(false);

		for (int i = 1; i < img_bgList.Length; i++) // ó�� ���� ��� �����ϰ� ��� ��� alpha�� 0���� ����
		{
			img_bgList[i].color = new Color(1, 1, 1, 0);
			img_bgList[i].raycastTarget = false;
			//Debug.Log(img_bgList[1].name);
		}

		img_bgList[0].color = new Color(1, 1, 1, 1);

		text_Dialogue.text = "";
		text_Dialogue.color = new Color(1, 1, 1, 1);
		img_Character.sprite = sprite_Character[0];
		img_Character.gameObject.SetActive(false);
		img_textBG.gameObject.SetActive(false);

		isAutoStart = false; // �ڵ� ���� Ȯ���� ���� �׽�Ʈ ���� (�� �Ǵµ�?)

		img_Character.sprite = sprite_Character[0];


		StartCoroutine(CRT_StartChat());
	}
	IEnumerator CRT_StartChat()
	{
		yield return new WaitForSeconds(1f);
		if (!isAutoStart)
		{
			onClick(touchCount);
		}
		else yield break;
	}



	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Q))
		{
			Debug.Log("��� �ٲٱ�!");
			StartCoroutine(CRT_ChangeBG(img_bgList[0], img_bgList[1]));
		}
		if (Input.GetKeyDown(KeyCode.Space) && canTouch)
		{
			canTouch = false;

			switch (touchCount)
			{
				case 0:
					isAutoStart = true;

					img_textBG.gameObject.SetActive(true);
					onClick(touchCount);
					break;

				case 1:

					text_Dialogue.text = "";
					ViberateForTime(0.5f);
					StartCoroutine(CRT_CollisionDirecting());


					break;
				case 2:
					onClick(touchCount);

					break;
				case 3:
					img_Character.gameObject.SetActive(true);
					text_Dialogue.color = textColor_Idol;
					onClick(touchCount);

					break;
				case 4:
					Debug.Log("��ư ���� �ܰ�");
					changeCharacterSprite(1);
					StartCoroutine(CRT_DelayShowButton());
					canTouch = false;
					break;
				case 5:
					changeCharacterSprite(0);
					if (isKind)
					{
						text_Dialogue.color = textColor_Idol;
						onClick(6);
					}
					else if (!isKind)
					{
						text_Dialogue.color = textColor_Idol;
						onClick(7);
					}
					break;
				case 6:
					onClick(8);
					break;
				case 7:
					onClick(9);
					break;
				case 8:
					onClick(10);
					break;
				case 9:
					onClick(11);
					break;
				case 10:
					Debug.Log("��ư ���� �ܰ�");
					btn1.onClick.RemoveListener(btn_Choose1);
					btn2.onClick.RemoveListener(btn_Choose2);
					btn1.onClick.AddListener(btn_Choose3);
					btn2.onClick.AddListener(btn_Choose4);

					btn1.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = arr[12].ToString();
					btn2.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = arr[13].ToString();

					StartCoroutine(CRT_DelayShowButton());
					break;
				case 11:
					if (isKind2)
					{
						text_Dialogue.color = textColor_Idol;
						onClick(14);
					}
					else if (!isKind2)
					{
						text_Dialogue.color = textColor_Idol;
						onClick(15);
					}
					break;
				case 12:
					text_Dialogue.color = new Color(1, 1, 1, 1);
					onClick(16);
					break;
				case 13:
					text_Dialogue.color = textColor_Idol;
					onClick(17);
					break;
				case 14:
					Debug.Log("����");
					// ���� ������
					if (isKind && isKind2)
					{
						//1 
						onClick(18);
						Ending();
					}
					else if (isKind & !isKind2)
					{
						//2
						onClick(19);
						Ending();
					}
					else if (!isKind & isKind2)
					{
						//3
						onClick(19);
						Ending();
					}
					else if (!isKind && !isKind2)
					{
						//4
						onClick(20);
						Ending();
					}
					break;
				default:
					break;
			}
		}

		if (ShakeTime > 0)
		{
			img_bg.transform.position = Random.insideUnitCircle * ShakeAmount + initialPosition;
			ShakeTime -= Time.deltaTime;
		}
		else
		{
			ShakeTime = 0.0f;
			img_bg.transform.position = initialPosition;
		}
	}

	/*
	IEnumerator CRT_DialogueIntro()
	{
		//StopCoroutine(CRT_Intro()); // ó�� ��Ʈ�� ���߱�
		//isIntro = false; // ��ƼŬ��Ʈ�� -> �̿��÷� Ŭ�� ������Ʈ ����

		//particle_Twinkle.Stop();
		//particle_Spawn.Stop();

		//particle_Correct.Stop();
		//particle_Spawn.Stop();
		//Intro_Front.SetActive(false);
		//Intro_Back.SetActive(false);
		//StartCoroutine(CRT_HideImage(img_Black.GetComponent<Image>(), 1f));
		img_bg.gameObject.SetActive(true);
		yield return new WaitForSeconds(1f);


		canTouch = true;

		//yield return new WaitForSeconds(0.5f);
		yield return new WaitForSeconds(2f);
		if (!isAutoStart)
		{
			img_textBG.gameObject.SetActive(true);
			onClick(0);
		}
		else yield break;
	}
	*/
	public void onClick(int Count) // ��� ���� �Լ�, Count�� �˸��� �迭�� ������ �ȴ�.
	{
		text_Dialogue.text = arr[Count].ToString();
		StartCoroutine(CRT_typing(Count, 0.02f));
		touchCount++;
	}
	IEnumerator CRT_typing(int count, float interval) //Ÿ���� ȿ�� ������ �ϴ� ����
	{
		for (int i = 0; i < arr[count].Length; i++)
		{
			text_Dialogue.text = arr[count].Substring(0, i);

			yield return new WaitForSeconds(interval);
		}

		//yield return new WaitForSeconds(1f);
		canTouch = true;
	}

	IEnumerator CRT_CollisionDirecting()
	{
		// �΋H���� �Ҹ�
		yield return new WaitForSeconds(0.5f);
		onClick(touchCount);
	}
	public void ViberateForTime(float time)
	{
		ShakeTime = time;
	}


	void btn_Choose1()
	{
		Select1();
		onClick(4);
		isKind = true;
		canTouch = true;
		touchCount = 5;
	}

	void btn_Choose2()
	{
		Select1();
		onClick(5);
		isKind = false;
		canTouch = true;
		touchCount = 5;
	}
	void Select1()
	{
		img_textBG.color = new Color(0, 0, 0, 0.4f);
		text_Dialogue.color = new Color(1, 1, 1, 1);
		btn1.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "";
		btn2.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "";
		btn1.gameObject.SetActive(false);
		btn2.gameObject.SetActive(false);
	}
	void btn_Choose3()
	{
		Select1();
		onClick(12);
		isKind2 = true;
	}
	void btn_Choose4()
	{
		Select1();
		onClick(13);
		isKind2 = false;
	}

	void btnClick_ReGame()
	{
		Init();
	}
	void btnClick_Exit()
	{
		Debug.Log("���������!");
		// �����ϴ°� �����񿡼� �����ؿ;���
	}

	void Ending()
	{
		btn_Return.gameObject.SetActive(true);
	}
	IEnumerator CRT_DelayShowButton() // ��ư�� �ʹ� ���� �������°� ������ �ֱ�
	{
		text_Dialogue.text = "";
		StartCoroutine(CRT_HideImage(img_textBG, 0.5f));
		yield return new WaitForSeconds(1f);
		btn1.gameObject.SetActive(true);
		btn2.gameObject.SetActive(true);
	}
	IEnumerator CRT_ChangeBG(Image previous, Image later)
	{
		StartCoroutine(CRT_ShowImage(later, 1f));
		StartCoroutine(CRT_HideImage(previous, 1f));
		yield return new WaitForSeconds(1f);
		canTouch = true;
	}


	void CameraZoom(Image image)
	{
		LeanTween.scale(image.gameObject, new Vector3(3f, 3f, 1f), 1f);
	}

	void changeCharacterSprite(int num)
	{
		img_Character.sprite = sprite_Character[num];
	}

	IEnumerator CRT_ShowImage(Image image, float speed)
	{
		while (image.color.a < 1)
		{
			var color = image.color;
			color.a += (speed * Time.deltaTime);

			image.color = color;
			yield return null;
		}
	}

	IEnumerator CRT_HideImage(Image image, float speed)
	{
		while (image.color.a > 0)
		{
			var color = image.color;
			color.a -= (speed * Time.deltaTime);

			image.color = color;
			yield return null;
		}
	}


}
