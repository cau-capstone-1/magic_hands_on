using System.Collections.Generic;
using System.IO.Ports;
using UnityEngine;
using UnityEngine.UI;

public class SerialPortManager : MonoBehaviour
{
    public TMPro.TMP_Dropdown portDropdown; // 포트 선택 UI (Dropdown)
    public TMPro.TMP_Text deviceInfoText; // 디바이스 정보 출력 UI

    private string selectedPort = "";

    void Start()
    {
        List<string> ports = new List<string>(SerialPort.GetPortNames());
        if (ports.Count == 0)
        {
            ports.Add("No Ports Available");
        }

        // Dropdown UI에 포트 목록 추가
        portDropdown.ClearOptions();
        portDropdown.AddOptions(ports);
    }

    // 포트 선택 이벤트 핸들러
    public void OnPortSelected(int index)
    {
        selectedPort = portDropdown.options[index].text;
        if (selectedPort != "No Ports Available")
        {
            ShowDeviceInfo(selectedPort);
        }
        else
        {
            deviceInfoText.text = "No device connected.";
        }
    }

    // 선택한 포트에 연결된 디바이스 정보 출력
    private void ShowDeviceInfo(string portName)
    {
        try
        {
            using (SerialPort serialPort = new SerialPort(portName, 9600))
            {
                serialPort.Open();
                // 디바이스에 간단한 핑 요청 또는 정보 요청을 보낼 수 있음
                serialPort.WriteLine("INFO");
                string response = serialPort.ReadLine(); // 응답 대기
                deviceInfoText.text = $"Device Info: {response}";
                serialPort.Close();
            }
        }
        catch (System.Exception ex)
        {
            deviceInfoText.text = $"Error: {ex.Message}";
        }
    }
}
