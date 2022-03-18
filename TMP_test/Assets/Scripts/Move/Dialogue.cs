using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class Dialogue : MonoBehaviour
{
	[Header("Text")]
	public TextMeshProUGUI text_Dialogue;
	public string[] arr;

	[Header("BackGround")]
	public Image[] img_bgList; //배경 이미지 ( 흔들리는 연출 사용시 사용되는 이미지 넘버링 Update에서 확인해야함)
	public Sprite sprite_bg; // 배경 스프라이트들

	public Image img_Character; // 캐릭터 이미지 
	public Sprite[] sprite_Character; // 캐릭터 스프라인트들

	public Image img_textBG;
	public GameObject go_SettingPopup;

	public Button btn1;
	public Button btn2;
	public Button btn_Return;
	public Button btn_Exit;
	public Button btn_Option;
	public Button btn_OptionClose;
	public int touchCount;
	public bool canTouch;

	public AudioSource se; // SE 오디오소스 플레이어
	public AudioClip se1 ,se2; // 교체하는 오디오클립



	public bool isKind; // 선택지 1
	public bool isKind2; // 선택지 2

	public bool isAutoStart;
	public bool isPause;

	// 흔들기
	float ShakeAmount; // 흔들리는정도
	float ShakeTime; // 흔들리는 시간
	Vector2 initialPosition; // 원점

	Color textColor_Idol = new Color(1, 0, 0, 1);
	Color textColor_Normal = new Color(1, 1, 1, 1);
	Color color_textBG = new Color(0, 0, 0, 0.4f);

	float interval = 0.02f; // 타이핑 딜레이 시간
	public Camera Cam_UI;


	public string Readtxt(string filePath) // Chat.txt 파일을 가져오는 함수, 각 문단은 '>' 로 구분한다.
	{
		FileInfo fileInfo = new FileInfo(filePath);
		string value = "";

		if (fileInfo.Exists)
		{
			StreamReader reader = new StreamReader(filePath);
			value = reader.ReadLine();
			reader.Close();
			Debug.Log(filePath.ToString() +"파일 읽음");
			arr = value.Split('>'); //line.Split(' ');
		}
		else
			value = "Assets/Resources/ 경로에 Chat.txt 파일이 없다";
		return value;
	}

	private void Awake()
	{
		Readtxt("Assets/Resources/Chat.txt");
		ShakeAmount = 0.2f;
	}

	private void OnEnable()
	{
		Init();
	}
	void Init()
	{
		Debug.Log("init");
		initialPosition = img_bgList[0].transform.position;

		touchCount = 0;
		canTouch = true;

		btn1.onClick.AddListener(btn_Choose1);
		btn2.onClick.AddListener(btn_Choose2);
		btn_Return.onClick.AddListener(btnClick_ReGame);
		btn_Exit.onClick.AddListener(btnClick_Exit);
		btn_Option.onClick.AddListener(btnClick_Option);
		btn_OptionClose.onClick.AddListener(btnClick_OptionClose);
		btn1.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = arr[4].ToString();
		btn2.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = arr[5].ToString();

		btn1.gameObject.SetActive(false);
		btn2.gameObject.SetActive(false);
		btn_Return.gameObject.SetActive(false);
		btn_Exit.gameObject.SetActive(false);

		for (int i = 1; i < img_bgList.Length; i++) // 처음 시작 배경 제외하고 모든 배경 alpha값 0으로 설정
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
		img_textBG.color = color_textBG;

		go_SettingPopup.SetActive(false);
		isPause = false;

		isAutoStart = false; // 자동 진행 확인을 위한 테스트 변수 (잘 되는듯?)

		img_Character.sprite = sprite_Character[0];


		StartCoroutine(CRT_StartChat());

		//SoundManager.Instance.PlaySound(bgm, Setting.Instance.volume_bgm, false);
	}
	IEnumerator CRT_StartChat()
	{
		yield return new WaitForSeconds(1f);
		if (!isAutoStart)
		{
			img_textBG.gameObject.SetActive(true);
			onClick(touchCount);
		}
		else yield break;
	}



	// Update is called once per frame
	void Update()
	{

		if (!isPause)
		{
			if (Input.GetKeyDown(KeyCode.Q))
			{
				Debug.Log("배경 바꾸기!");
				//StartCoroutine(CRT_ChangeBG(img_bgList[0], img_bgList[1]));
				se.Play();
			}
			if(Input.GetKeyDown(KeyCode.W))
			{
				se.clip = se2;
			}
			if(Input.GetKeyDown(KeyCode.E))
			{
				se.clip = se1;
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
						Debug.Log("버튼 선택 단계");
						StartCoroutine(CRT_ChangeBG(img_bgList[0], img_bgList[1]));

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
						Debug.Log("버튼 선택 단계");
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
						text_Dialogue.color = textColor_Normal;
						onClick(16);
						break;
					case 13:
						text_Dialogue.color = textColor_Idol;
						onClick(17);
						break;
					case 14:
						Debug.Log("엔딩");
						// 엔딩 페이즈
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
				img_bgList[0].transform.position = Random.insideUnitCircle * ShakeAmount + initialPosition;
				ShakeTime -= Time.deltaTime;
			}
			else
			{
				ShakeTime = 0.0f;
				img_bgList[0].transform.position = initialPosition;
			}


		}
	}

	public void onClick(int Count) // 대사 실행 함수, Count에 알맞은 배열을 넣으면 된다.
	{
		text_Dialogue.text = arr[Count].ToString();
		StartCoroutine(CRT_typing(Count, interval));
		touchCount++;
	}
	IEnumerator CRT_typing(int count, float interval) //타이핑 효과 연출을 하는 역할
	{
		for (int i = 0; i < arr[count].Length; i++)
		{
			text_Dialogue.text = arr[count].Substring(0, i);

			yield return new WaitForSeconds(interval);
		}
		canTouch = true;

	}

	IEnumerator CRT_CollisionDirecting()
	{
		// 부딫히는 소리 
		//SoundManager.Instance.PlaySound()
		yield return new WaitForSeconds(0.5f);
		onClick(touchCount);
	}
	public void ViberateForTime(float time)
	{
		ShakeTime = time;
	}
	void btn_Choose1()
	{
		Select();
		onClick(4);
		isKind = true;
	}

	void btn_Choose2()
	{
		Select();
		onClick(5);
		isKind = false;
	}

	void btn_Choose3()
	{
		Select();
		onClick(12);
		isKind2 = true;
	}
	void btn_Choose4()
	{
		Select();
		onClick(13);
		isKind2 = false;
	}
	void Select() // 선택지 버튼을 눌러서 다시 대화 상태로
	{
		img_textBG.color = color_textBG;
		text_Dialogue.color = textColor_Normal;
		btn1.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "";
		btn2.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "";
		btn1.gameObject.SetActive(false);
		btn2.gameObject.SetActive(false);
	}

	void btnClick_ReGame()
	{
		Init();
	}
	void btnClick_Exit()
	{
		Debug.Log("게임종료다!");
		// 종료하는거 스몰비에서 참고해와야함
	}
	void btnClick_Option()
	{
		isPause = true;
		go_SettingPopup.SetActive(true);
	}
	void btnClick_OptionClose()
	{
		isPause = false;
		go_SettingPopup.SetActive(false);
	}

	public void playsound_SE()
	{
		se.Play();
	}
	void Ending()
	{
		btn_Return.gameObject.SetActive(true);
	}

	IEnumerator CRT_DelayShowButton() // 버튼이 너무 빨리 보여지는거 딜레이 주기
	{
		text_Dialogue.text = "";
		StartCoroutine(CRT_HideImage(img_textBG, 0.5f));
		yield return new WaitForSeconds(1f);
		btn1.gameObject.SetActive(true);
		btn2.gameObject.SetActive(true);
	}

	IEnumerator CRT_ChangeBG(Image previous, Image later) // 자연스럽게 배경 바꾸기(이미지 2개 사용해서 alpha값 교체하는 방식)
	{
		StartCoroutine(CRT_ShowImage(later, 1f));
		StartCoroutine(CRT_HideImage(previous, 1f));
		yield return new WaitForSeconds(1f);
		canTouch = true;
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
