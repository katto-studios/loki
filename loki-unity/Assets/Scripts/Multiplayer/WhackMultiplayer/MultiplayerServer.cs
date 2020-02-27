using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using UnityEngine;
using System.Text;
using System;
using UnityEngine.UI;

public class MultiplayerServer {
    private string m_consoleTxt;
    public string ConsoleTxt { get { return m_consoleTxt; } }
    private volatile bool m_shuttingDown = false;
    private Thread m_serverThread;
    private string m_ip = "127.0.0.1";
    private TcpListener m_server;

    public MultiplayerServer(string _ip = "127.0.0.1") {
        m_ip = _ip;

        m_serverThread = new Thread(CreateServer);
        m_serverThread.Start();
    }

    private void CreateServer() {
        try {
            m_server = new TcpListener(IPAddress.Any, 42069);
            m_server.Start();

            while (true) {
                UpdateLoop();
            }
        } catch (Exception e) {
            Debug.Log(e.Message);
            m_consoleTxt += "\nError: " + e.Message;
        }
    }

    private void UpdateLoop() {
        try {
            byte[] buffer = new byte[256];
            string data;

            Debug.Log("Waiting for connection");
            m_consoleTxt += "\nWaiting for connection";

            TcpClient client = m_server.AcceptTcpClient();
            Debug.Log("Client connected!");
            m_consoleTxt += "\nClient connected";
            NetworkStream stream = client.GetStream();

            int count;
            while ((count = stream.Read(buffer, 0, buffer.Length)) != 0) {
                data = Encoding.ASCII.GetString(buffer, 0, count);
                Debug.Log("Server recieved: " + data);
                m_consoleTxt += "\nServer recieved: " + data;

                data = "Ack";
                byte[] msg = Encoding.ASCII.GetBytes(data);
                stream.Write(msg, 0, msg.Length);
                Debug.Log( "Server: " + data);
                m_consoleTxt += "\nServer: " + data;
            }
            Thread.Sleep(1);

            client.Close();
        }catch(Exception e) {
            Debug.Log(e.Message);
            m_consoleTxt += "\n" + e.Message;
        }
    }

    public void StopServer() {
        m_shuttingDown = true;
        m_serverThread.Abort();
    }

    ~MultiplayerServer() {
        StopServer();
    }
}
