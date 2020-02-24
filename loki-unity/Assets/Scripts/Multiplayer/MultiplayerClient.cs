using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using UnityEngine;
using System.Text;

public class MultiplayerClient : MonoBehaviour {
    private Thread m_socketThread;
    private volatile bool m_keepReading = false;

    // Start is called before the first frame update
    void Start() {
        Application.runInBackground = true;

    }

    // Update is called once per frame
    void Update() {

    }

    private Socket m_listener;
    private Socket m_handler;

    private string GetIpAddress() {
        IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());

        foreach(IPAddress ip in host.AddressList) {
            if(ip.AddressFamily == AddressFamily.InterNetwork) {
                return ip.ToString();
            }
        }

        return null;
    }

    private void NetworkCode() {
        string data;
        //byte buffer
        byte[] buffer = new byte[1024];

        Debug.Log("Ip: " + GetIpAddress().ToString());
        IPAddress[] ipArr = Dns.GetHostAddresses(GetIpAddress());
        IPEndPoint localEndPoint = new IPEndPoint(ipArr[0], 42069);

        m_listener = new Socket(ipArr[0].AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        //bind and listen to connections
        try {
            m_listener.Bind(localEndPoint);
            m_listener.Listen(10);

            while (true) {
                m_keepReading = true;
                Debug.Log("Waiting for connection");

                m_handler = m_listener.Accept();
                Debug.Log("Client connected");

                data = null;

                while (m_keepReading) {
                    buffer = new byte[1024];

                    int bytesRecieved = m_handler.Receive(buffer);
                    Debug.Log("Recieved from server");

                    if(bytesRecieved <= 0) {
                        m_keepReading = false;
                        m_handler.Disconnect(true);
                        break;
                    }

                    data += Encoding.ASCII.GetString(buffer, 0, bytesRecieved);

                    if(data.IndexOf("<EOF>") > -1) {
                        break;
                    }
                }

                Thread.Sleep(1);
            }
        }catch(System.Exception e) {
            Debug.LogError(e.Message);
        }
    }

    private void StopServer() {
        m_keepReading = false;

        if(m_socketThread != null) {
            m_socketThread.Abort();
        }

        if(m_handler != null && m_handler.Connected) {
            m_handler.Disconnect(false);
            Debug.Log("Disconnected");
        }
    }

    private void OnDisable() {
        StopServer();
    }
}
