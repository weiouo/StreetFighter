using System;
using System.IO.Ports;
using System.Threading;
using UnityEngine;

public class Player2_Input : MonoBehaviour
{
    private SerialPort serialPort1 = new SerialPort("COM8", 1200);
    private SerialPort serialPort2 = new SerialPort("COM5", 1200);
    private Animator animator;

    void Start()
    {
        try
        {
            animator = GetComponent<Animator>();

            serialPort1.Open();
            serialPort1.ReadTimeout = 500;
            Debug.Log("serialPort1 open");
            serialPort2.Open();
            serialPort2.ReadTimeout = 500;
            Debug.Log("serialPort2 open");
            serialPort1.DiscardInBuffer();
            serialPort2.DiscardInBuffer();
        }
        catch (System.Exception ex)
        {
            Debug.LogError("can't open serialPort " + ex.Message);
        }
    }

    void Update()
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

                    }
                    else if (receivedData == 'B')
                    {
                        Debug.Log("1 Defend Button Pressed");
                        animator.SetTrigger("Defend");
                    }
                    serialPort1.DiscardInBuffer();
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError("Error: " + ex.Message);
            }
        }
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
                        

                    }
                    else if (receivedData == 'D')
                    {
                        Debug.Log("2 Defend Button Pressed");
               
                    }
                    serialPort2.DiscardInBuffer();
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError("Error: " + ex.Message);
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
        if (serialPort2.IsOpen)
        {
            serialPort2.Close();
            Debug.Log("serialPort2 close");
        }
    }
}
