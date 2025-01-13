using UnityEngine;
using System.Collections;
using System.IO.Ports;
using UnityEngine.UI;

public class Read_Arduino : MonoBehaviour
{
    public string portname = "COM3";
    public int baudrate = 9600;
    public Text UI;
    public Text UI1;
    public float readInterval = 0.1f; // อ่านข้อมูลทุกๆ 0.1 วินาที

    private string data;
    private SerialPort serialPort;
    private bool isReading = true;
    public int sendValueFlow;

    public static Read_Arduino instance;

    private void Awake()
    {
        instance = this;
    }


    void Start()
    {
        try
        {
            serialPort = new SerialPort(portname, baudrate);
            serialPort.ReadTimeout = 100; // ลดเวลา timeout
            serialPort.Open();

            if (serialPort.IsOpen)
            {
                StartCoroutine(ReadSerialData());
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error opening serial port: " + e.Message);
        }
    }

    private IEnumerator ReadSerialData()
    {
        while (isReading)
        {
            if (serialPort != null && serialPort.IsOpen)
            {
                try
                {
                    if (serialPort.BytesToRead > 0)
                    {
                        data = serialPort.ReadLine();
                        string[] values = data.Split(',');

                        // Update UI ในเธรดหลัก
                        UpdateUIValues(values);
                    }
                }
                catch (System.Exception e)
                {
                    Debug.LogWarning("Read error: " + e.Message);
                }
            }

            yield return new WaitForSeconds(readInterval);
        }
    }

    private void UpdateUIValues(string[] values)
    {
        if (values.Length >= 2)
        {
            if (UI != null) UI.text = "BPM: " + values[0];
            if (UI1 != null) UI1.text = "FLOW: " + values[1];

            //Debug.Log(int.Parse(values[1]));

            /*
            if (Player.instance.isJump)
            {
                sendValueFlow = int.Parse(values[1]);
            }
            else
            {
                sendValueFlow = 0;
            }*/

            sendValueFlow = int.Parse(values[1]);

        }
    }

    void OnDisable()
    {
        isReading = false;
    }

    void OnApplicationQuit()
    {
        isReading = false;
        if (serialPort != null && serialPort.IsOpen)
        {
            serialPort.Close();
            serialPort.Dispose();
        }
    }
}