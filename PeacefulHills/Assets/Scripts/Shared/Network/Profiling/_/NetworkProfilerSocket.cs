using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Unity.Collections;
using UnityEngine;

namespace PeacefulHills.Network.Profiling
{
    public class NetworkProfilerSocket : IDisposable
    {
        public int ConnectionCount => _connectionSockets.Count;
        
        private readonly Socket _serverSocket;
        private readonly List<Socket> _connectionSockets = new List<Socket>();
        
        private bool _working;

        public NetworkProfilerSocket()
        {
            _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _serverSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, 1);
            _serverSocket.Bind(new IPEndPoint(IPAddress.Any, 8005));
            _serverSocket.Listen(10);

            _working = true;
            
            var acceptThread = new Thread(Accept);
            acceptThread.Start();
        }

        private void Accept()
        {
            while (_working)
            {
                try
                {
                    Socket connectionSocket = _serverSocket.Accept();
                    connectionSocket.Blocking = false;
                    Debug.Log("Connection accepted!");
                    _connectionSockets.Add(connectionSocket);
                }
                catch (SocketException)
                {
                }
            }
        }

        public void Send(byte[] bytes)
        {
            for (int i = 0; i < _connectionSockets.Count; i++)
            {
                Socket connectionSocket = _connectionSockets[i];
                try
                {
                    connectionSocket.Send(bytes);
                }
                catch (SocketException)
                {
                    _connectionSockets.RemoveAtSwapBack(i);
                }
            }
        }

        public void Close()
        {
            foreach (Socket connectionSocket in _connectionSockets)
            {
                connectionSocket.Shutdown(SocketShutdown.Both);
                connectionSocket.Close();
            }
            _connectionSockets.Clear();
        }

        public void Stop()
        {
            _working = false;
        }
        
        public void Dispose()
        {
            _working = false;
            _serverSocket.Dispose();
        }
    }
}