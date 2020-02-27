using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using UnityEngine;
using System.Text;
using System;
using UnityEngine.UI;

public class MultiplayerClient {
    private string m_consoleTxt;
    public string ConsoleTxt { get { return m_consoleTxt; } }
    //private Thread m_clientThread;
    private string m_ip = "127.0.0.1";
    private TcpClient m_client = null;

    public MultiplayerClient(string _ip = "127.0.0.1") {
        m_ip = _ip;

        ConnectToServer();
    }

    private void ConnectToServer() {
        //create tcp client
        try {
            m_client = new TcpClient(m_ip, 42069);

            byte[] data = Encoding.ASCII.GetBytes("Client connection request");
            NetworkStream stream = m_client.GetStream();

            stream.Write(data, 0, data.Length);
            Debug.Log(PlayfabUserInfo.GetUsername() + ": Client connection request");
            m_consoleTxt += "\n" + PlayfabUserInfo.GetUsername() + ": Client connection request";

            data = new byte[256];

            int bytes = stream.Read(data, 0, data.Length);
            string responseData = Encoding.ASCII.GetString(data, 0, bytes);
            Debug.Log("Recieved: " + responseData);
            m_consoleTxt += "\nRecieved: " + responseData;
        } catch (Exception e) {
            Debug.Log(e.Message);
            m_consoleTxt += "\nError: " + e.Message;
        }
    }

    public void SendMessageToServer(string _message) {
        try {
            NetworkStream stream = m_client.GetStream();

            byte[] sendToServer = Encoding.ASCII.GetBytes(_message);
            Debug.Log(PlayfabUserInfo.GetUsername() + _message);
            m_consoleTxt += "\n" + PlayfabUserInfo.GetUsername() + _message;
            stream.Write(sendToServer, 0, sendToServer.Length);
        } catch (Exception e) {
            Debug.Log(e.Message);
            m_consoleTxt += "\nError: " + e.Message;
        }
    }

    public void StopClient() {
        m_client.GetStream().Close();
        m_client.Close();
    }

    ~MultiplayerClient() {
        StopClient();
    }
}
