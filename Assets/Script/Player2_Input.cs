using System;
using System.IO.Ports;
using System.Threading;
using UnityEngine;

public class Player2_Input : MonoBehaviour
{
    private SerialPort serialPort1 = new SerialPort("COM8", 1200);
    private Animator animator;
    public float speed = 1f;

    public float minX = -8f;
    public float maxX = 8f;

    public int blood = 10;

    public GameObject player1;
    public GameObject player2;

    void Start()
    {
        try
        {
            animator = GetComponent<Animator>();

            serialPort1.Open();
            serialPort1.ReadTimeout = 500;
            Debug.Log("serialPort1 open");
            serialPort1.DiscardInBuffer();
        }
        catch (System.Exception ex)
        {
            Debug.LogError("can't open serialPort " + ex.Message);
        }
    }

    void Update()
    {
        float move = 0f;
        if (serialPort1.IsOpen)
        {
            try
            {
                if (serialPort1.BytesToRead > 0)
                {
                    char receivedData = (char)serialPort1.ReadChar();
                    if (receivedData == 'A')
                    {
                        Debug.Log("1 Attack Button Pressed");
                        animator.SetTrigger("Attack");

                    }
                    else if (receivedData == 'B')
                    {
                        Debug.Log("1 Defend Button Pressed");
                        animator.SetTrigger("Defend");
                    }
                    else if (receivedData == 'l')
                    {
                        Debug.Log("1 left");
                        move = -2.5f;
                        MovePlayer(move);

                    }
                    else if (receivedData == 'r')
                    {
                        Debug.Log("1 right");
                        move = 2.5f;
                        MovePlayer(move);
                    }
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError("Error: " + ex.Message);
            }
        }
    }
    private void MovePlayer(float move) {
        Vector3 newPosition = transform.position;
        newPosition.x += move * speed * Time.deltaTime;

        newPosition.x = Mathf.Clamp(newPosition.x,minX,maxX);

        transform.position = newPosition;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        if (collision.gameObject == player1)
        {
            Animator player1Animator = player1.GetComponent<Animator>();

            AnimatorStateInfo player1StateInfo = player1Animator.GetCurrentAnimatorStateInfo(0);

           if (player1StateInfo.IsName("Player1_attack"))
            {
                if (!stateInfo.IsName("Player2_defend")) blood--;
            }
        }
    }


    void OnApplicationQuit()
    {
        if (serialPort1.IsOpen)
        {
            serialPort1.Close();
            Debug.Log("serialPort1 close");
        }
    }
}
