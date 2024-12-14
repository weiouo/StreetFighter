using System;
using System.IO.Ports;
using System.Threading;
using UnityEngine;

public class Player2_Input : MonoBehaviour
{
    private SerialPort serialPort = new SerialPort("COM8", 1200);
    private Animator animator;

    void Start()
    {
        try
        {
            animator = GetComponent<Animator>();

            serialPort.Open();
            serialPort.ReadTimeout = 500;
            Debug.Log("serialPort open");
            serialPort.DiscardInBuffer();
        }
        catch (System.Exception ex)
        {
            Debug.LogError("can't open serialPort " + ex.Message);
        }
    }

    void Update()
    {
        if (serialPort.IsOpen)
        {
            try
            {
                if (serialPort.BytesToRead > 0)
                {
                    char receivedData = (char)serialPort.ReadChar();
                    if (receivedData == 'A')
                    {
                        Debug.Log("Attack Button Pressed");
                        animator.SetTrigger("Attack");
                    }
                    else
                    {
                        Debug.Log("Defend Button Pressed");
                        animator.SetTrigger("Defend");
                    }
                    serialPort.DiscardInBuffer();
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
        if (serialPort.IsOpen)
        {
            serialPort.Close();
            Debug.Log("serialPort close");
        }
    }
}
