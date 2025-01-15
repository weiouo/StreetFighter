using System;
using System.IO.Ports;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;
using static UnityEditor.Experimental.GraphView.GraphView;

public class GameController : MonoBehaviour
{
    // UI 元件
    public Button button; // 按鈕
    public Text timcnt; // 倒數計時文字
    public GameObject gover; // Game Over 圖片
    public GameObject tover; // Time Over 圖片
    public GameObject playerA; // playerA 圖片
    public GameObject playerB; // playerB 圖片

    [SerializeField] int HpA = 10;
    [SerializeField] int HpB = 10;
    [SerializeField] GameObject alifbar;
    [SerializeField] GameObject blifbar;
    public float waitTime = 2f;

    // 遊戲狀態
    private float timer = 120f; // 遊戲時間 120 秒 (2 分鐘)
    private bool isGameRunning = false; // 遊戲是否正在進行
    private int lastDisplayedTime = -1; // 上次顯示的整數秒數
    private Player1_Input playerB_state;
    private Player2_Input playerA_state;

    private SerialPort serialPort;

    void Start()
    {
        timcnt.text = "02:00"; // 設定初始倒數計時
        HpA = 10;
        HpB = 10;
        button.onClick.AddListener(OnButtonClick); // start按鈕點擊事件
        gover.SetActive(false); // 隱藏 Game Over 圖片
        tover.SetActive(false); // 隱藏 Time Over 圖片

        playerA_state = playerA.GetComponent<Player2_Input>();
        playerB_state = playerB.GetComponent<Player1_Input>();
    }

    void Update()
    {
        if (isGameRunning)
        {
            Update_alfbar();
            Update_blfbar();
            // 倒數計時邏輯
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
        button.gameObject.SetActive(false); // 隱藏按鈕
        Debug.Log("遊戲開始");
        isGameRunning = true; // 設置遊戲狀態為進行中
        playerA_state.setIsGaming(true);
        playerB_state.setIsGaming(true);
    }

    //start按鈕觸發
    void OnButtonClick()
    {
        StartGame(); // 按下按鈕開始遊戲
    }

        //倒數時間顯示
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

        Debug.Log("遊戲結束");
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

            Update_blfbar(); // 更新血條顯示
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

            Update_blfbar(); // 更新血條顯示
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





