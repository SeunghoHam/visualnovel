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

    public Button btn1; // ������1
    public Button btn2; // ������2
    public Button btn_Return; // �ٽ��ϱ�
    public Button btn_Exit; // ��������
    public Button btn_Option; //�ɼ�
    public Button btn_OptionClose; // �ɼǲ���

    public AudioSource se;
    public AudioClip se1, se2;

    int touchCount;
    bool canTouch;

    bool isKind; // ������1 ���
    bool isKind2; // ������2 ���
    bool isAutoStart; // �ڵ� ����
    bool isPause; // �ɼǹ�ư, �Ͻ�����

    Color textColor_Idol = new Color(1, 0, 0, 1); // ���̵� �ؽ�Ʈ ����
    Color textColor_Normal = new Color(1, 1, 1, 1); // �Ϲ� �ؽ�Ʈ ����
    Color color_textBG = new Color(0, 0, 0, 0.4f);

    float interval = 0.02f; // Ÿ���� ������ �ð�
    WaitForSeconds typeDelay;

    float ShakeAmount; // ��鸮�� ����
    float ShakeTime; // ��鸮�� �ð�
    Vector2 initialPosition; // ����

    public string Readtxt(string filePath)
	{
        FileInfo fileInfo = new FileInfo(filePath);
        string value = "";
        if (fileInfo.Exists)
        {
            StreamReader reader = new StreamReader(filePath);
            value = reader.ReadLine();
            reader.Close();
            Debug.Log(filePath.ToString() + " : ���� ����");
            arr = value.Split('>'); // '>' ��ȣ�� �� ������
        }
            else value = filePath.ToString() + "��ο� ������ ����";

        return value;
	}
	private void Awake()
	{
        Readtxt("Assets/Resources/Chat.txt"); // �ӽ� ���
        ShakeAmount = 0.2f;
	}
	private void Update()
	{
		if(!isPause)
		{
            if(Input.GetKeyDown(KeyCode.Space) && canTouch)
			{
                /* ��Ÿ ��� ����
                 * ��� ���� : CRT- CRT_ChangeBG(����, �ٲܰ�);
                 * ĳ���� ��������Ʈ ���� : changeCharacteSprite(�ٲ� �迭 ��);
                 * �ε����� ȿ��(ȭ�鶳��) : ViberateForTime(���ӽð�);
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
                        Debug.Log("��ư ���� �ܰ�");
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
                        Debug.Log("��ư ���� �ܰ�");
                        btn1.onClick.RemoveListener(btn_Choose1);
                        btn2.onClick.RemoveListener(btn_Choose2);
                        btn1.onClick.AddListener(btn_Choose3);
                        btn2.onClick.AddListener(btn_Choose4);

                        btn1.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = arr[12].ToString();
                        btn2.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = arr[13].ToString();

                        StartCoroutine(CRT_DelayShowButton()); break;
                    case 4:
                        Debug.Log("����");
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

        for (int i = 1; i < img_bgList.Length; i++) // ó�� ���� ��� �����ϰ� ��� ��� alpha�� 0���� ����
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

        img_Character.sprite = sprite_Character[0]; // ĳ���� ��������Ʈ
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
    public void onClick(int Count) // ��� ���� �Լ�, Count�� �˸��� �迭�� ������ �ȴ�.
    {
        text_Dialogue.text = arr[Count].ToString();
        StartCoroutine(CRT_typing(Count, interval));
        touchCount++;
    }
    IEnumerator CRT_typing(int count, float interval) //Ÿ���� ȿ�� ������ �ϴ� ����
    {
        for (int i = 0; i < arr[count].Length; i++)
        {
            text_Dialogue.text = arr[count].Substring(0, i);

            yield return typeDelay;
        }
        canTouch = true;

    }
    IEnumerator CRT_CollisionDirecting() // �ε����� �Ҹ� ��� (���� �̻��) �Ҹ� ��, ������ -> �ؽ�Ʈ ����� ����ص� �� 
    {
        // �΋H���� �Ҹ� 
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
    void Select() // ������ ��ư�� ������ �ٽ� ��ȭ ���·�
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
        Debug.Log("���������!");
        Destroy(this.gameObject);
        // �����ϴ°� �����񿡼� �����ؿ;���
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
    IEnumerator CRT_DelayShowButton() // ��ư�� �ʹ� ���� �������°� ������ �ֱ�
    {
        text_Dialogue.text = "";
        StartCoroutine(CRT_HideImage(img_textBG, 0.5f));
        yield return new WaitForSeconds(1f);
        btn1.gameObject.SetActive(true);
        btn2.gameObject.SetActive(true);
    }

    IEnumerator CRT_ChangeBG(Image previous, Image later) // �ڿ������� ��� �ٲٱ�(�̹��� 2�� ����ؼ� alpha�� ��ü�ϴ� ���)
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
