using System.Collections;
using UnityEngine;
using System.IO.Ports;
using System.Threading;

public class ArduinoController : MonoBehaviour
{
    SerialPort serialPort = new SerialPort("COM4", 9600); // 시리얼 포트와 보드레이트 설정
    private string incomingData = ""; // 들어오는 데이터를 저장할 변수
    public string resultData = "";
    private Thread readThread; // 데이터를 비동기적으로 읽기 위한 스레드

    void Start()
    {
        try
        {
            if (!serialPort.IsOpen) // 포트가 열려 있지 않으면 열기
            {
                serialPort.Open(); // 시리얼 포트 열기
                serialPort.ReadTimeout = 1000; // 읽기 타임아웃 설정 (1초)
                Debug.Log("Serial port opened successfully on " + serialPort.PortName);
            }

            // 시리얼 포트에서 데이터를 읽는 별도의 스레드 시작
            readThread = new Thread(ReadSerialData);
            readThread.Start();
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to open serial port: " + e.Message);
        }
    }

    void ReadSerialData()
    {
        while (serialPort.IsOpen)
        {
            try
            {
                // 시리얼 포트에서 데이터 읽기
                incomingData = serialPort.ReadLine().Trim();
                if (!string.IsNullOrEmpty(incomingData))
                {
                    Debug.Log("Received Data: " + incomingData);
                    HandleInput(incomingData);
                }
            }
            catch (System.TimeoutException)
            {
                // 타임아웃 발생 시 무시
            }
            catch (System.Exception ex)
            {
                Debug.LogError("Error reading from serial port: " + ex.Message);
            }
        }
    }

    public void ResetData()
    {
        resultData = "";
    }

    // 데이터를 받아서 키 입력을 시뮬레이션하는 함수
    private void HandleInput(string data)
    {
        switch (data)
        {
            case "1":
                resultData = "1";
                break;
            case "2":
                resultData = "2";
                break;
            case "3":
                resultData = "3";
                SimulateKeyPress(KeyCode.Z);
                break;
            case "4":
                resultData = "4";
                SimulateKeyPress(KeyCode.X);
                break;
            case "5":
                resultData = "5";
                SimulateKeyPress(KeyCode.C);
                break;
            case "6":
                resultData = "6";
                SimulateKeyPress(KeyCode.V);
                break;
            default:
                Debug.LogWarning("Unhandled data: " + data);
                break;
        }
    }

    // 키 입력을 시뮬레이션하는 함수
    private void SimulateKeyPress(KeyCode key)
    {
        // 키 입력 시뮬레이션 동작을 로그로 출력
        Debug.Log("Simulated Key Press: " + key);
    }

    private void OnDestroy()
    {
        // Unity 종료 시 시리얼 포트를 안전하게 닫기
        if (serialPort != null && serialPort.IsOpen)
        {
            serialPort.Close();
            Debug.Log("Serial port closed.");
        }

        // 스레드 종료
        if (readThread != null && readThread.IsAlive)
        {
            readThread.Abort();
        }
    }
}
