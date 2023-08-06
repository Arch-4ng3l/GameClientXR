using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System;
using System.Threading.Tasks;
using UnityEngine;
using System.Net.Http;
using System.IO;
using Newtonsoft.Json;

public class Client
{

    public int receiveBufferSize = 512;

    private ClientWebSocket _Socket;
    private Uri _Uri;
    private Uri _HttpUri;
    public Client(Uri uri, Uri uri1)
    {
        _Socket = new ClientWebSocket();
        SetOrigin();
        _Uri = uri;
        _HttpUri = uri1;

    }

    private void SetOrigin()
    {
        _Socket.Options.SetRequestHeader("Origin", "http://192.168.178.73/");
    }


    public async Task<int> Connect(int count = 0, CancellationToken cancellationToken = default)
    {

        Thread.Sleep(count * 500);
        try
        {
            
            await _Socket.ConnectAsync(_Uri, cancellationToken);
           
        }
        catch (Exception e) when (e is WebSocketException)
        {
            
            _Socket = new ClientWebSocket();
            SetOrigin();
            count++;
            if (count < 15)
            {
                int t = await Connect(count);
                return t;
            }
            else
            {
                return 1;
            }

        }
        catch (Exception e)
        {

            return 1;
        }
        return 0;
    }

    public async Task SendJson<T>(T send, CancellationToken cancellationToken = default)
    { 
        string jsonMessage = JsonUtility.ToJson(send);
        byte[] byteMessage = Encoding.UTF8.GetBytes(jsonMessage);
        await _Socket.SendAsync(new ArraySegment<byte>(byteMessage), WebSocketMessageType.Text, true, cancellationToken);
    }

    public async Task<T[]> ReceiveJson<T>(CancellationToken cancellationToken = default)
    {
        byte[] receiveBuffer = new byte[receiveBufferSize];
        int bytesReceived = 0;
        WebSocketReceiveResult result;

        while (true)
        {
            result = await _Socket.ReceiveAsync(new ArraySegment<byte>(receiveBuffer), cancellationToken);
            bytesReceived += result.Count;
            if (result.EndOfMessage)
            {
                break;
            }

            if (bytesReceived >= receiveBufferSize)
            {
                Array.Resize(ref receiveBuffer, receiveBufferSize * 2);
                receiveBufferSize *= 2;
            }
        }

        string messageReceived = Encoding.UTF8.GetString(receiveBuffer);
        string finalString = messageReceived.Replace('\x00', ' ').Trim();
        T[] list = null;
        
        try
        {
            

            list = JsonConvert.DeserializeObject<T[]>(finalString);
            
            return list;
            
        }
        catch (Exception ex)
        {
            
            return list;
        }

        
        
    }


    public async Task GetAssets(string path, string targetPath)
    {
        try
        {
            HttpClient httpClient = new();
            HttpResponseMessage response = await httpClient.GetAsync(_HttpUri, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();
            Stream stream = await response.Content.ReadAsStreamAsync();
            FileStream fileStream = new(path, FileMode.Create, FileAccess.ReadWrite, FileShare.None);
            await stream.CopyToAsync(fileStream);
            path = fileStream.Name;
            targetPath = fileStream.Name.Replace("assets.zip", "Downloads");
            fileStream.Close();
            Directory.Delete(targetPath + @"\assets", true);

        }
        catch (Exception e) when (e is DirectoryNotFoundException)
        {
        }
        catch (Exception e)
        {
            Debug.Log($"{e}");
            return;
        }

        ZipExtractor zipExtractor = new(path, targetPath);
        int err = zipExtractor.Extract();
        if (err != 0)
        {
            return;
        }

    }

 
}

