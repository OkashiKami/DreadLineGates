using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;
using System.Net.Sockets;
using System.Net;

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
        infolable.text = "Please Wait...\nStarting in 10s...";
        yield return new WaitForSeconds(10);
        infolable.text = "Please Wait...\nConnecting to our master server...";
        yield return new WaitForSeconds(3);
        GameControl.client = new Client();
        infolable.text = "Please Wait...\nCreating Server Socket...";
        GameControl.client.socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        yield return new WaitForSeconds(3);
        infolable.text = "Please Wait...\nCreating IPEndpoint...";
        GameControl.client.endpoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 2655);
        yield return new WaitForSeconds(4);
        infolable.text = "Please Wait...\nConnecting...";
        yield return new WaitForSeconds(3);
        try
        {
            GameControl.client.socket.Connect(GameControl.client.endpoint);
        }
        catch
        {
            infolable.text = "Connection: [FF0000]ERROR[-]\nConnecting to server Failed... (E:805:655)";
        }
        yield return new WaitForSeconds(5);
        if (GameControl.client.socket.Connected)
        {
            infolable.text = "Connection: [30FF00]PASSED[-]\nConnecting to server success";
            yield return new WaitForSeconds(3);
            infolable.text = "Please Wait...\nGetting enviorment ready for login...";
            yield return new WaitForSeconds(5);
            GameControl.functionList.Add(delegate { return LoginScene(); });
        }
    }

    internal static string SubmitLogin(UIInput user, UIInput pass)
    {
        string u = user.value;
        string p = pass.value;

        return string.Format("Loggin in with {0} as username and {1} as password", u, p);
    }
}

public class Client
{
    internal Socket socket;
    internal IPEndPoint endpoint;
}