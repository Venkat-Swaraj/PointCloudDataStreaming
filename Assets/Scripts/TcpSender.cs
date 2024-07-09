using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using UnityEngine.Serialization;

public class TcpSender : MonoBehaviour
{
    [SerializeField] private string serverIP;
    private TcpClient _client;
    private NetworkStream _stream;
    private PointCloudCapture _pointCloudCapture;
    private bool _isConnected = false;

    void Start()
    {
        _pointCloudCapture = GetComponent<PointCloudCapture>();
        InvokeRepeating(nameof(SendPointCloudData), 1.0f, 0.5f); // Send data every 0.5 seconds
        EstablishConnection();
    }
    
    public void EstablishConnection()
    {
        try
        {
            if (!_isConnected)
            {
                _client = new TcpClient(serverIP, 12345); // Replace with your server's LAN IP address
                _stream = _client.GetStream();
                _isConnected = true;
                Debug.Log("Connection established.");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to establish connection: {e.Message}");
            _isConnected = false;
        }
    }
    void CloseConnection()
    {
        if (_isConnected)
        {
            _stream.Close();
            _client.Close();
            _isConnected = false;
            Debug.Log("Connection closed.");
        }
    }
    void SendPointCloudData()
    {
        if (!_isConnected)
        {
            EstablishConnection();
            return;
        }

        List<Vector3> pointCloudData = _pointCloudCapture.GetPointCloudData();
        if (pointCloudData.Count > 0)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var point in pointCloudData)
            {
                if (IsValidPoint(point))
                {
                    sb.Append($"{point.x},{point.y},{point.z}|");
                }
            }

            // Remove the trailing separator if needed
            if (sb.Length > 0 && sb[^1] == '|')
            {
                sb.Remove(sb.Length - 1, 1);
            }

            byte[] data = Encoding.UTF8.GetBytes(sb.ToString());
            try
            {
                _stream.Write(data, 0, data.Length);
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to send data: {e.Message}");
                CloseConnection();
            }
        }
    }
    
    private bool IsValidPoint(Vector3 point)
    {
        // Add any additional validation logic if necessary
        return !float.IsNaN(point.x) && !float.IsNaN(point.y) && !float.IsNaN(point.z);
    }
    void OnApplicationQuit()
    {
        CloseConnection();
    }
}
