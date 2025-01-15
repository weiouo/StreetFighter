using System;
using System.IO.Ports;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;
using static UnityEditor.Experimental.GraphView.GraphView;

public class GameController : MonoBehaviour
{
    // UI ����
    public Button button; // ���s
    public Text timcnt; // �˼ƭp�ɤ�r
    public GameObject gover; // Game Over �Ϥ�
    public GameObject tover; // Time Over �Ϥ�
    public GameObject playerA; // playerA �Ϥ�
    public GameObject playerB; // playerB �Ϥ�

    [SerializeField] int HpA = 10;
    [SerializeField] int HpB = 10;
    [SerializeField] GameObject alifbar;
    [SerializeField] GameObject blifbar;
    public float waitTime = 2f;

    // �C�����A
    private float timer = 120f; // �C���ɶ� 120 �� (2 ����)
    private bool isGameRunning = false; // �C���O�_���b�i��
    private int lastDisplayedTime = -1; // �W����ܪ���Ƭ��
    private Player1_Input playerB_state;
    private Player2_Input playerA_state;

    private SerialPort serialPort;

    void Start()
    {
        timcnt.text = "02:00"; // �]�w��l�˼ƭp��
        HpA = 10;
        HpB = 10;
        button.onClick.AddListener(OnButtonClick); // start���s�I���ƥ�
        gover.SetActive(false); // ���� Game Over �Ϥ�
        tover.SetActive(false); // ���� Time Over �Ϥ�

        playerA_state = playerA.GetComponent<Player2_Input>();
        playerB_state = playerB.GetComponent<Player1_Input>();
    }

    void Update()
    {
        if (isGameRunning)
        {
            Update_alfbar();
            Update_blfbar();
            // �˼ƭp���޿�
            timer -= Time.deltaTime;
            timer = Mathf.Max(0, timer);
            int currentDisplayedTime = Mathf.FloorToInt(timer);

            if (currentDisplayedTime != lastDisplayedTime)
            {
                lastDisplayedTime = currentDisplayedTime;
                UpdateTimerText(currentDisplayedTime);
            }

            if (timer <= 0 || HpA <= 0 || HpB <= 0)
            {
                EndGame();
            }
        }
        else
        {
            if (playerA_state.startGaming()) StartGame();
            playerA_state.stopGaming();
        }
    }

    public void StartGame()
    {
        button.gameObject.SetActive(false); // ���ë��s
        Debug.Log("�C���}�l");
        isGameRunning = true; // �]�m�C�����A���i�椤
        playerA_state.setIsGaming(true);
        playerB_state.setIsGaming(true);
    }

    //start���sĲ�o
    void OnButtonClick()
    {
        StartGame(); // ���U���s�}�l�C��
    }

        //�˼Ʈɶ����
    void UpdateTimerText(int secondsRemaining)
    {
        int minutes = secondsRemaining / 60;
        int seconds = secondsRemaining % 60;
        timcnt.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void EndGame()
    {
        isGameRunning = false;
        playerA_state.setIsGaming(false);
        playerB_state.setIsGaming(false);


        if (timer == 0)
        {
            tover.SetActive(true);
            Invoke("ShowGameOver", waitTime);
        }
        else
        {
            gover.SetActive(true);
        }

        Debug.Log("�C������");
    }

    void ShowGameOver()
    {
        tover.SetActive(false);
        gover.SetActive(true);
    }

    void ReduceHpB()
    {
        if (isGameRunning)
        {
            HpB -= 1;
            

            if (HpB <= 0 || HpA <= 0)
            {
                EndGame();
            }

            Update_blfbar(); // ��s������
        }
    }
    void ReduceHpA()
    {
        if (isGameRunning)
        {
            HpA -= 1;


            if (HpB<= 0 || HpA <= 0)
            {
                EndGame();
            }

            Update_blfbar(); // ��s������
        }
    }
    void Update_alfbar()
    {
        HpA = playerA_state.blood;
        for (int i = 0; i < alifbar.transform.childCount; i++)
        {
            if (HpA > i)
            {
                alifbar.transform.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                alifbar.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    void Update_blfbar()
    {
        HpB = playerB_state.blood;
        for (int i = 0; i < blifbar.transform.childCount; i++)
        {
            if (HpB > i)
            {
                blifbar.transform.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                blifbar.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
    
}





