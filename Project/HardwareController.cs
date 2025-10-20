using System;
using System.IO.Ports;

namespace Project
{
    public class HardwareController : IDisposable
    {
        private SerialPort _serialPort;

        public HardwareController(string portName, int baudRate)
        {
            _serialPort = new SerialPort(portName, baudRate);
            _serialPort.Open();
        }

        // 예시 1: 팬 켜기
        public void TurnFanOn()
        {
            SendCommand("FAN_ON");
        }

        // 예시 2: 팬 끄기
        public void TurnFanOff()
        {
            SendCommand("FAN_OFF");
        }

        // 예시 3: 자동 운전 온도 임계값 설정
        public void SetAutoTempThreshold(double temp)
        {
            SendCommand($"SET_THRESHOLD:{temp}");
        }

        // 명령을 시리얼 포트로 전송하는 핵심 메서드
        private void SendCommand(string command)
        {
            if (_serialPort != null && _serialPort.IsOpen)
            {
                _serialPort.WriteLine(command);
                Console.WriteLine($"[명령 전송] -> {command}");
            }
        }

        // 프로그램 종료 시 포트 닫기
        public void Dispose()
        {
            _serialPort?.Close();
        }
    }
}