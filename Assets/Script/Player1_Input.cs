using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using UnityEngine;

public class Player1_Input : MonoBehaviour
{
    private SerialPort serialPort1 = new SerialPort("COM7", 1200);
    private Animator animator;

    public float speed = 8f;

    public float minX = -8f;
    public float maxX = 8f;

    public int blood = 10;
    private bool defend = false;
    private bool isGaming = false;

    public GameObject player1;
    public GameObject player2;

    private Player2_Input player2_state;
    void Start()
    {
        try
        {
            animator = GetComponent<Animator>();
            player2_state = player2.GetComponent<Player2_Input>();

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
        if (isGaming)
        {
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
                            if (transform.position.x - player2.transform.position.x < 3 && transform.position.x - player2.transform.position.x > -3)
                            {

                                if (!player2_state.getDefend())
                                {
                                    Invoke("attack", 0.5f);
                                }
                                player2_state.setDefend();
                            }

                        }
                        else if (receivedData == 'B')
                        {
                            Debug.Log("1 Defend Button Pressed");
                            animator.SetTrigger("Defend");
                            defend = true;
                        }
                        else if (receivedData == 'l')
                        {
                            Debug.Log("1 left");
                            move = -5f;
                            MovePlayer(move);

                        }
                        else if (receivedData == 'r')
                        {
                            Debug.Log("1 right");
                            move = 5f;
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
        if (blood <= 0) animator.SetTrigger("Die");
    }
    private void MovePlayer(float move)
    {
        Vector3 newPosition = transform.position;
        newPosition.x += move * speed * Time.deltaTime;

        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);

        transform.position = newPosition;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
    }
    void attack()
    {
        player2_state.blood--;
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
    void OnApplicationQuit()
    {
        if (serialPort1.IsOpen)
        {
            serialPort1.Close();
            Debug.Log("serialPort1 close");
        }
    }
}
