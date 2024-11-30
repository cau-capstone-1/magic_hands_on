using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class ArduinoController : MonoBehaviour
{
    SerialPort serialPort = new SerialPort("COM3", 9600); // COM 포트와 바우드레이트 설정
    private string incomingData = ""; // 들어오는 데이터를 저장할 변수

    void Start()
    {
        serialPort.Open(); // 시리얼 포트 열기
        serialPort.ReadTimeout = 50; // 읽기 타임아웃 설정
    }

    void Update()
    {
        // 시리얼 포트에서 데이터를 읽어옴
        if (serialPort.IsOpen)
        {
            try
            {
                incomingData = serialPort.ReadLine().Trim(); // 한 줄 읽고 공백 제거
                HandleInput(incomingData);
            }
            catch (System.Exception) { }
        }
    }

    // 데이터를 받아서 키 입력을 시뮬레이션하는 함수
    private void HandleInput(string data)
    {
        switch (data)
        {
            case "3":
                SimulateKeyPress(KeyCode.Z);
                break;
            case "4":
                SimulateKeyPress(KeyCode.X);
                break;
            case "5":
                SimulateKeyPress(KeyCode.C);
                break;
            case "6":
                SimulateKeyPress(KeyCode.V);
                break;
            default:
                break;
        }
    }

    // 키 입력을 시뮬레이션하는 함수
    private void SimulateKeyPress(KeyCode key)
    {
        // 유니티에서 키 입력을 감지하는 방법
        if (Input.GetKeyDown(key))
        {
            // 여기에 키 입력에 반응하는 코드를 추가
            Debug.Log(key + " key pressed.");
        }
    }

    private void OnDestroy()
    {
        if (serialPort != null && serialPort.IsOpen)
        {
            serialPort.Close(); // 시리얼 포트 닫기
        }
    }
}
