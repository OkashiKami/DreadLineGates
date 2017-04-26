using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;

public class FUNCTIONS
{
    internal static string ConnectionScene()
    {
        SceneManager.LoadScene("Connection");
        return "Successfully loaded connection scene";
    }
    internal static string LoginScene()
    {
        SceneManager.LoadScene("Login");
        return "Successfully loaded Login scene";
    }

    internal static IEnumerator ConnectToServer(UILabel infolable)
    {
        infolable.text = "Please Wait...\nStarting in 5s...";
        yield return new WaitForSeconds(5);
        GameControl.client = new Client();
        infolable.text = "Please Wait...\nCreating client Socket...";
        GameControl.client.socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        yield return new WaitForSeconds(1);
        infolable.text = "Please Wait...\nCreating IPEndpoint...";
        GameControl.client.endpoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 2655);
        yield return new WaitForSeconds(3);
        infolable.text = "Please Wait...\nConnecting to our master server...";
        yield return new WaitForSeconds(0.5f);
        try
        {
            GameControl.client.socket.Connect(GameControl.client.endpoint);
        }
        catch
        {
            infolable.text = "Connection: [FF0000]ERROR[-]\nConnecting to server Failed... (E:805:655)";
        }
        yield return new WaitForSeconds(3.6f);
        if (GameControl.client.socket.Connected)
        {
            GameControl.client.isConnected = true;
            infolable.text = "Connection: [30FF00]PASSED[-]\nConnecting to server success";
            yield return new WaitForSeconds(1.2f);
            infolable.text = "Please Wait...\nGetting enviorment ready for login...";
            yield return new WaitForSeconds(0.2f);
            GameControl.client.StartReceiver();
            GameControl.functionList.Add(delegate { return LoginScene(); });
        }
    }

    internal static string SubmitLogin(UIInput[] get)
    {
        string u = get[0].value;
        string p = get[1].value;

        GameControl.client.Send(u, p);
        return string.Format("Using U:{0}, P:{1}", u, p);
    }
}

public class Client
{
    internal Socket socket;
    internal IPEndPoint endpoint;
    internal bool isConnected;

    public void Send(string message)
    {
        try
        {
            socket.Send(Encoder.Genarate("logininfo=INFO-" + message + "|"));
        }
        catch
        {
            try { socket.Shutdown(SocketShutdown.Both); } catch { }
            try { socket.Close(); } catch { }
            isConnected = false;
        }
    }
    public void Send(string user, string pass)
    {
        try
        {
            socket.Send(Encoder.Genarate("logininfo=USER-" + user + ":PASS-" + pass + "|"));
        }
        catch
        {
            try { socket.Shutdown(SocketShutdown.Both); } catch { }
            try { socket.Close(); } catch { }
            isConnected = false;
        }
    }

    internal void StartReceiver()
    {
        new Thread(new ThreadStart(Receiver)).Start();
        new Thread(new ThreadStart(testsend)).Start();
    }

    

    private void Receiver()
    {
        while(isConnected)
        {
            try
            {
                byte[] buff = new byte[socket.ReceiveBufferSize];
                socket.Receive(buff, 0, buff.Length, SocketFlags.None);
                string message = Encoder.Genarate(buff);
                message = message.Split('|')[0];
                if (!message.Equals(""))
                {
                    //Debug.Log("Server Message: '" + message + "'");
                    Encoder.Decode(this, message);
                }
            }
            catch
            {
                try { socket.Shutdown(SocketShutdown.Both); } catch { }
                try { socket.Close(); } catch { }
                isConnected = false;
            }
        }
    }

    private void testsend()
    {
        while (isConnected)
        {
            try
            {
                socket.Send(Encoder.Genarate(""));
            }
            catch
            {
                try { socket.Shutdown(SocketShutdown.Both); } catch { }
                try { socket.Close(); } catch { }
                isConnected = false;
            }
        }
    }

    public static class Encoder
    {
        public static byte[] Genarate(string input) { return Encoding.UTF8.GetBytes(input); }
        public static string Genarate(byte[] input) { return Encoding.UTF8.GetString(input); }

        internal static void Decode(Client client, string ms)
        {
            string value = string.Empty;
            if (ms.StartsWith("logininfo"))
            {
                value = ms.Split('=')[1];
            }
            string[] values = value.Split(':');

            string user = string.Empty;
            string pass = string.Empty;
            string info = string.Empty;

            for (int i = 0; i < value.Length - 1; i++)
            {
                if (values[i].StartsWith("USER-"))
                {
                    user = values[i].Replace("USER-", "");
                    Debug.Log(string.Format("From Client: USER = {0}", user));
                }
                else if (values[i].StartsWith("PASS-"))
                {
                    pass = values[i].Replace("PASS-", "");
                    Debug.Log(string.Format("From Client: PASS = {0}", pass));
                }
                else if (values[i].StartsWith("INFO-"))
                {
                    info = values[i].Replace("INFO-", "");
                    Debug.Log(string.Format("From Client: {0}", info));
                }
            }
        }
    }
}