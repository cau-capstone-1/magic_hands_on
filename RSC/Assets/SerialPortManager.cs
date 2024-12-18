using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class SerialPortManager : MonoBehaviour
{
    public TMPro.TMP_Dropdown portDropdown; // 포트 선택 UI (Dropdown)
    public TMPro.TMP_Text deviceInfoText; // 디바이스 정보 출력 UI

    private string selectedPort = null;

    void OnEnable()
    {
        Init();
    }

    void Start()
    {
        Init();
    }

    void Init()
    {
        selectedPort = GameData.instance.serialPort;
        List<string> ports = new List<string> { "" };

        foreach (string port in SerialPort.GetPortNames())
        {
            ports.Add(port);
        }

        if (ports.Count == 0)
        {
            ports.Add("No Ports Available");
        }

        portDropdown.ClearOptions();
        portDropdown.AddOptions(ports);

        if (selectedPort != null && ports.Contains(selectedPort))
        {
            portDropdown.value = ports.IndexOf(selectedPort);
        }
    }

    public void OnPortSelected(int index)
    {
        selectedPort = portDropdown.options[index].text;

        GameData.instance.serialPort = selectedPort;

        if (selectedPort != "No Ports Available")
        {
            StartCoroutine(ShowDeviceInfo(selectedPort)); // 3초 타임아웃
        }
        else
        {
            deviceInfoText.text = "No device connected.";
        }
    }

    private IEnumerator ShowDeviceInfo(string portName)
    {
        deviceInfoText.text = "Connecting...";
        string response = "Timeout: No response from device.";

        Thread thread = new Thread(() =>
        {
            try
            {
                using (SerialPort serialPort = new SerialPort(portName, 9600))
                {
                    serialPort.Open();
                    response = serialPort.PortName;
                    serialPort.Close();
                }
            }
            catch (TimeoutException)
            {
                response = "Timeout: No response from device.";
            }
            catch (System.Exception ex)
            {
                response = $"Error: {ex.Message}";
            }
        });

        thread.Start();
        while (thread.IsAlive)
        {
            yield return null; // 스레드가 완료될 때까지 대기
        }

        deviceInfoText.text = response;
    }
}
