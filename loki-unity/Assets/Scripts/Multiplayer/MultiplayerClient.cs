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
    public InputField inIp, inChat;
    private string m_ip { get { return inIp.text; } }
    private Thread m_networkThread;
    public Text console;
    private string m_consoleTxt = "";
    private volatile bool m_shuttingDown = false;
    private bool m_sendMsg = false;

    private void Start() {

    }

    private void Update() {
        console.text = m_consoleTxt;
        m_sendMsg = Input.GetKeyDown(KeyCode.Return);
    }

    public void OnButtonConnect() {
        m_networkThread = new Thread(ConnectToServer);
        m_networkThread.Start();
    }

    public void OnButtonCreate() {
        m_networkThread = new Thread(CreateServer);
        m_networkThread.Start();
    }

    //I am a client
    private void ConnectToServer() {
        //create tcp client
        try {
           TcpClient client = new TcpClient(m_ip, 42069);

            byte[] data = Encoding.ASCII.GetBytes("Client connection request");
            NetworkStream stream = client.GetStream();

            stream.Write(data, 0, data.Length);
            Debug.Log("Client send: " + "Client connection request");
            m_consoleTxt += "\nClient send: " + "Client connection request";

            data = new byte[256];

            int bytes = stream.Read(data, 0, data.Length);
            string responseData = Encoding.ASCII.GetString(data, 0, bytes);
            Debug.Log("Recieved: " + responseData);
            m_consoleTxt += "\nRecieved: " + responseData;

            while (!m_shuttingDown) {
                if (m_sendMsg) {
                    byte[] sendToServer = Encoding.ASCII.GetBytes(inChat.text);
                    Debug.Log("Client send: " + inChat.text);
                    m_consoleTxt += "\nClient send: " + inChat.text;

                    bytes = stream.Read(data, 0, data.Length);
                    responseData = Encoding.ASCII.GetString(data, 0, bytes);
                    Debug.Log("Recieved: " + responseData);
                    m_consoleTxt += "\nRecieved: " + responseData;
                }

                Thread.Sleep(1);
            }

            stream.Close();
            client.Close();
        } catch (Exception e) {
            Debug.Log(e.Message);
            m_consoleTxt += "\nError: " + e.Message;
        }
    }

    //I am a server
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
                NetworkStream stream = client.GetStream();

                while (!m_shuttingDown) {
                    //clear data
                    data = "";
                    int count;
                    while ((count = stream.Read(buffer, 0, buffer.Length)) != 0) {
                        data = Encoding.ASCII.GetString(buffer, 0, count);
                        Debug.Log("Server recieved: " + data);
                        m_consoleTxt += "\nServer recieved: " + data;

                        data = "Ack";
                        byte[] msg = Encoding.ASCII.GetBytes(data);
                        stream.Write(msg, 0, msg.Length);
                        Debug.Log("Server sent: " + data);
                        m_consoleTxt += "\nServer sent: " + data;
                    }

                    Thread.Sleep(1);
                }

                client.Close();
            }
        } catch (Exception e) {
            Debug.Log(e.Message);
            m_consoleTxt += "\nError: " + e.Message;
        }
    }

    private void OnDestroy() {
        m_shuttingDown = true;
    }

    private void OnApplicationQuit() {
        m_shuttingDown = true;
    }
}
