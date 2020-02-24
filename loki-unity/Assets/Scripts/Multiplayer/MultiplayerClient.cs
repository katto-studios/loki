using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using UnityEngine;
using System.Text;
using System;
using UnityEngine.UI;

public class MultiplayerClient : MonoBehaviour {
    public InputField inIp;
    private string m_ip { get { return inIp.text; } }
    private Thread m_networkThread;
    public Text console;
    private string m_consoleTxt = "";

    private void Start() {

    }

    private void Update() {
        console.text = m_consoleTxt;
    }

    public void OnButtonConnect() {
        m_networkThread = new Thread(ConnectToServer);
        m_networkThread.Start();
    }

    public void OnButtonCreate() {
        m_networkThread = new Thread(CreateServer);
        m_networkThread.Start();
    }

    private void ConnectToServer() {
        //create tcp client
        try {
            TcpClient client = new TcpClient(m_ip, 42069);

            byte[] data = Encoding.ASCII.GetBytes("hello");
            NetworkStream stream = client.GetStream();

            stream.Write(data, 0, data.Length);
            Debug.Log("Sent data to " + m_ip);
            m_consoleTxt += "\nSent data to " + m_ip;

            data = new byte[256];

            int bytes = stream.Read(data, 0, data.Length);
            string responseData = Encoding.ASCII.GetString(data, 0, bytes);
            Debug.Log("Recieved: " + responseData);
            m_consoleTxt += "\nRecieved: " + responseData;

            stream.Close();
            client.Close();
        } catch (Exception e) {
            Debug.Log(e.Message);
            m_consoleTxt += "\nError: " + e.Message;
        }
    }

    private void CreateServer() {
        try {
            TcpListener server = new TcpListener(IPAddress.Parse("127.0.0.1"), 42069);
            server.Start();
            byte[] buffer = new byte[256];
            string data;

            while (true) {
                Debug.Log("Waiting for connection");
                m_consoleTxt += "\nWaiting for connection";

                TcpClient client = server.AcceptTcpClient();
                Debug.Log("Client connected!");
                m_consoleTxt += "\nClient connected";

                //clear data
                data = string.Empty;
                NetworkStream stream = client.GetStream();
                int count;
                while ((count = stream.Read(buffer, 0, buffer.Length)) != 0) {
                    data = Encoding.ASCII.GetString(buffer, 0, count);
                    Debug.Log("Client recieved: " + data);
                    m_consoleTxt += "\nCliend recieved: " + data;

                    data = data.ToUpper();
                    byte[] msg = Encoding.ASCII.GetBytes(data);
                    stream.Write(msg, 0, msg.Length);
                    Debug.Log("Client sent: " + data);
                    m_consoleTxt += "\nCliend sent: " + data;
                }

                client.Close();
            }
        } catch (Exception e) {
            Debug.Log(e.Message);
            m_consoleTxt += "\nError: " + e.Message;
        }
    }
}
