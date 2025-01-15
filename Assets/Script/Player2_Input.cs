using System;
using System.Drawing.Text;
using System.IO.Ports;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class Player2_Input : MonoBehaviour
{
    private SerialPort serialPort2 = new SerialPort("COM8", 1200);
    private Animator animator;
    private Animator player1Animator;
    public float speed = 4f;

    public float minX = -8f;
    public float maxX = 8f;

    public int blood = 10;

    private bool defend = false;
    private bool isGaming = false;
    private bool starting = false;

    public GameObject player1;
    public GameObject player2;

    private Player1_Input player1_state;


    void Start()
    {
        try
        {
            animator = GetComponent<Animator>();
            player1Animator = player1.GetComponent<Animator>();
            player1_state = player1.GetComponent<Player1_Input>();

            serialPort2.Open();
            serialPort2.ReadTimeout = 500;
            Debug.Log("serialPort2 open");
            serialPort2.DiscardInBuffer();
        }
        catch (System.Exception ex)
        {
            Debug.LogError("can't open serialPort " + ex.Message);
        }
    }

    void Update()
    {
        float move = 0f;
        if (isGaming)
        {
            if (serialPort2.IsOpen)
            {
                try
                {
                    if (serialPort2.BytesToRead > 0)
                    {

                        char receivedData = (char)serialPort2.ReadChar();
                        if (receivedData == 'A')
                        {
                            Debug.Log("2 Attack Button Pressed");
                            animator.SetTrigger("Attack");
                            if (transform.position.x - player1.transform.position.x < 3 && transform.position.x - player1.transform.position.x > -3)
                            {
                                if (!player1_state.getDefend())
                                {
                                    Invoke("attack", 0.5f);
                                }
                            }
                            player1_state.setDefend();

                        }
                        else if (receivedData == 'B')
                        {
                            Debug.Log("2 Defend Button Pressed");
                            animator.SetTrigger("Defend");
                            defend = true;
                        }
                        else if (receivedData == 'l')
                        {
                            Debug.Log("2 left");
                            move = -2.5f;
                            MovePlayer(move);

                        }
                        else if (receivedData == 'r')
                        {
                            Debug.Log("2 right");
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
        else
        {
            if (serialPort2.IsOpen)
            {
                try
                {
                    if (serialPort2.BytesToRead > 0)
                    {
                        char receivedData = (char)serialPort2.ReadChar();
                        if (receivedData == 'A')
                        {
                          starting = true;
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    Debug.LogError("Error: " + ex.Message);
                }
            }
        }
        if (blood <= 0) animator.SetTrigger("Die");
    }
    private void MovePlayer(float move) {
        Vector3 newPosition = transform.position;
        newPosition.x += move * speed * Time.deltaTime;

        newPosition.x = Mathf.Clamp(newPosition.x,minX,maxX);

        transform.position = newPosition;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
    }
    void attack()
    {
        player1_state.blood--;
    }
    public bool getDefend()
    {
        return defend;
    }
    public void setDefend()
    {
        defend = false;
    }

    public void setIsGaming(bool state)
    {
        isGaming = state;
    }

    public bool startGaming()
    {
        return starting;
    }
    public void stopGaming()
    {
        starting = false;
    }

    void OnApplicationQuit()
    {
        if (serialPort2.IsOpen)
        {
            serialPort2.Close();
            Debug.Log("serialPort2 close");
        }
    }
}
