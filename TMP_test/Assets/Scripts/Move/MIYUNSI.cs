using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class MIYUNSI : MonoBehaviour
{
    [Header("Text")]
    public TextMeshPro text_Dialogue;
    public string[] arr;

    [Header("BackGround")]
    public Image[] img_bgList;
    public Sprite sprite_bg;

    public Image img_Character;
    public Sprite[] sprite_Character;

    public Image img_textBG;
    public GameObject go_SettingPopup;

    public Button btn1; // 선택지1
    public Button btn2; // 선택지2
    public Button btn_Return; // 다시하기
    public Button btn_Exit; // 게임종료
    public Button btn_Option; //옵션
    public Button btn_OptionClose; // 옵션끄기

    public AudioSource se;
    public AudioClip se1, se2;

    int touchCount;
    bool canTouch;

    bool isKind; // 선택지1 결과
    bool isKind2; // 선택지2 결과
    bool isAutoStart; // 자동 시작
    bool isPause; // 옵션버튼, 일시정지

    Color textColor_Idol = new Color(1, 0, 0, 1); // 아이돌 텍스트 색상
    Color textColor_Normal = new Color(1, 1, 1, 1); // 일반 텍스트 색상
    Color color_textBG = new Color(0, 0, 0, 0.4f);

    float interval = 0.02f; // 타이핑 딜레이 시간
    WaitForSeconds typeDelay;

    float ShakeAmount; // 흔들리는 정도
    float ShakeTime; // 흔들리는 시간
    Vector2 initialPosition; // 원점

    public string Readtxt(string filePath)
	{
        FileInfo fileInfo = new FileInfo(filePath);
        string value = "";
        if (fileInfo.Exists)
        {
            StreamReader reader = new StreamReader(filePath);
            value = reader.ReadLine();
            reader.Close();
            Debug.Log(filePath.ToString() + " : 파일 읽음");
            arr = value.Split('>'); // '>' 기호로 줄 구분함
        }
            else value = filePath.ToString() + "경로에 파일이 없다";

        return value;
	}
	private void Awake()
	{
        Readtxt("Assets/Resources/Chat.txt"); // 임시 경로
        ShakeAmount = 0.2f;
	}
	private void Update()
	{
		if(!isPause)
		{
            if(Input.GetKeyDown(KeyCode.Space) && canTouch)
			{
                /* 기타 기능 사용법
                 * 배경 변경 : CRT- CRT_ChangeBG(이전, 바꿀거);
                 * 캐릭터 스프라이트 변경 : changeCharacteSprite(바꿀 배열 수);
                 * 부딪히는 효과(화면떨림) : ViberateForTime(지속시간);
                 * 
                 */
                canTouch = false;
				switch (touchCount)
				{
                    case 0:
                        isAutoStart = true;
                        img_textBG.gameObject.SetActive(true);
                        onClick(touchCount); break;
                    case 1:
                        Debug.Log("버튼 선택 단계");
                        StartCoroutine(CRT_DelayShowButton());
                        break;
                    case 2:
                        text_Dialogue.color = textColor_Idol;
                        if (isKind)
                            onClick(0);
                        else if (!isKind)
                            onClick(0);
                        break;
                    case 3:
                        Debug.Log("버튼 선택 단계");
                        btn1.onClick.RemoveListener(btn_Choose1);
                        btn2.onClick.RemoveListener(btn_Choose2);
                        btn1.onClick.AddListener(btn_Choose3);
                        btn2.onClick.AddListener(btn_Choose4);

                        btn1.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = arr[12].ToString();
                        btn2.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = arr[13].ToString();

                        StartCoroutine(CRT_DelayShowButton()); break;
                    case 4:
                        Debug.Log("엔딩");
                        if (isKind && isKind2)
                        { //1 
                            onClick(0);
                            Ending();
                        }
                        else if (isKind & !isKind2)
                        { //2
                            onClick(0);
                            Ending();
                        }
                        else if (!isKind & isKind2)
                        { //3
                            onClick(0);
                            Ending();
                        }
                        else if (!isKind && !isKind2)
                        { //4
                            onClick(0);
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
	private void OnEnable()
	{
        Init();
	}
    void Init()
	{
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
        btn_Option.gameObject.SetActive(false);

        for (int i = 1; i < img_bgList.Length; i++) // 처음 시작 배경 제외하고 모든 배경 alpha값 0으로 설정
        {
            img_bgList[i].color = new Color(1, 1, 1, 0);
            img_bgList[i].raycastTarget = false;
        }
        img_bgList[0].raycastTarget = false;
        img_bgList[0].color = new Color(1, 1, 1, 1);

        text_Dialogue.text = "";
        text_Dialogue.color = textColor_Normal;
        img_Character.sprite = sprite_Character[0];
        img_Character.gameObject.SetActive(false);
        img_textBG.gameObject.SetActive(false);
        img_textBG.color = color_textBG;

        go_SettingPopup.SetActive(false);
        isPause = false;
        isAutoStart = false;

        img_Character.sprite = sprite_Character[0]; // 캐릭터 스프라이트
        StartCoroutine(CRT_StartChat());

        typeDelay = new WaitForSeconds(interval);
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

            yield return typeDelay;
        }
        canTouch = true;

    }
    IEnumerator CRT_CollisionDirecting() // 부딪히는 소리 재생 (현재 미사용) 소리 후, 딜레이 -> 텍스트 재생에 사용해도 됨 
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
        Destroy(this.gameObject);
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
