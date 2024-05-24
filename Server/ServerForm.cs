using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;

namespace SimpleServerGUI
{
    public partial class ServerForm : Form
    {
        private static Dictionary<string, Socket> connectedClients = new Dictionary<string, Socket>();
        private static readonly object lockObject = new object();
        private BackgroundWorker serverWorker;
        private Socket server;
        private List<Socket> clientList = new List<Socket>();

        public ServerForm()
        {
            InitializeComponent();
            InitializeServerWorker();
        }

        private void InitializeServerWorker()
        {
            serverWorker = new BackgroundWorker();
            serverWorker.DoWork += ServerWorker_DoWork;
            serverWorker.RunWorkerCompleted += ServerWorker_RunWorkerCompleted;
            serverWorker.WorkerSupportsCancellation = true;
        }

        private void StartServerButton_Click(object sender, EventArgs e)
        {
            if (!serverWorker.IsBusy)
            {
                serverWorker.RunWorkerAsync();
            }
        }

        private void ServerWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            StartServer();
        }

        private void ServerWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                LogMessage("Erro: " + e.Error.Message);
            }
        }

        private void StartServer()
        {
            try
            {
                IPAddress ipAddress = IPAddress.Parse("26.29.146.215");
                int port = 8888;
                server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                server.Bind(new IPEndPoint(ipAddress, port));
                server.Listen(10);
                LogMessage("Servidor iniciado. Aguardando conexões...");

                while (true)
                {
                    Socket client = server.Accept();
                    clientList.Add(client); // Adiciona o cliente à lista de clientes

                    LogMessage("Aguardando identificação do cliente...");

                    // Solicitar um identificador único ao cliente
                    byte[] buffer = new byte[1024];
                    int bytesRead = client.Receive(buffer);
                    string clientId = Encoding.ASCII.GetString(buffer, 0, bytesRead);

                    lock (lockObject)
                    {
                        // Adicionar cliente à lista de clientes conectados
                        connectedClients.Add(clientId, client);
                    }

                    // Iniciar uma nova thread para lidar com as mensagens do cliente
                    Thread clientThread = new Thread(() => HandleClient(clientId, client));
                    clientThread.Start();
                }
            }
            catch (Exception ex)
            {
                LogMessage("Erro: " + ex.Message);
            }
        }

        private void HandleClient(string clientId, Socket client)
        {
            try
            {
                LogMessage("(" + clientId + ") conectado.");

                while (true)
                {
                    byte[] buffer = new byte[1024];
                    int bytesRead = client.Receive(buffer);
                    if (bytesRead == 0)
                    {
                        lock (lockObject)
                        {
                            connectedClients.Remove(clientId);
                        }
                        LogMessage("(" + clientId + ") desconectado.");
                        clientList.Remove(client); // Remove o cliente da lista
                        break;
                    }

                    string messageHeader = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                    string[] headerParts = messageHeader.Split('|');

                    if (headerParts[0] == "FILE")
                    {
                        string receiverId = headerParts[1];
                        string fileName = headerParts[2];
                        int fileSize = int.Parse(headerParts[3]);

                        byte[] fileBuffer = new byte[fileSize];
                        int totalBytesReceived = 0;

                        while (totalBytesReceived < fileSize)
                        {
                            bytesRead = client.Receive(fileBuffer, totalBytesReceived, fileSize - totalBytesReceived, SocketFlags.None);
                            totalBytesReceived += bytesRead;
                        }

                        // Encaminhar o arquivo para o destinatário
                        SendFile(receiverId, fileName, fileBuffer);
                    }
                    else
                    {
                        string receiverId = headerParts[0];
                        string actualMessage = headerParts[1];
                        // Encaminhar a mensagem para o(s) destinatário(s) correto(s)
                        SendMessage(clientId, receiverId, actualMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                LogMessage("Erro ao lidar com as mensagens do cliente " + clientId + ": " + ex.Message);
            }
        }

        private void SendMessage(string senderId, string receiverId, string message)
        {
            try
            {
                lock (lockObject)
                {
                    if (connectedClients.ContainsKey(receiverId))
                    {
                        Socket receiverClient = connectedClients[receiverId];
                        if (receiverClient.Connected)
                        {
                            byte[] data = Encoding.ASCII.GetBytes(senderId + ": " + message);
                            receiverClient.Send(data);
                        }
                        else
                        {
                            LogMessage("O cliente '" + receiverId + "' está desconectado.");
                            connectedClients.Remove(receiverId);
                        }
                    }
                    else
                    {
                        LogMessage("O cliente '" + receiverId + "' não está conectado.");
                    }
                }
            }
            catch (Exception ex)
            {
                LogMessage("Erro ao enviar mensagem para o cliente " + receiverId + ": " + ex.Message);
            }
        }

        private void SendFile(string receiverId, string fileName, byte[] fileData)
        {
            try
            {
                lock (lockObject)
                {
                    if (connectedClients.ContainsKey(receiverId))
                    {
                        Socket receiverClient = connectedClients[receiverId];
                        if (receiverClient.Connected)
                        {
                            // Enviar o cabeçalho do arquivo
                            string header = $"FILE|{fileName}|{fileData.Length}";
                            byte[] headerBytes = Encoding.ASCII.GetBytes(header);
                            receiverClient.Send(headerBytes);

                            // Enviar os dados do arquivo
                            receiverClient.Send(fileData);
                        }
                        else
                        {
                            LogMessage("O cliente '" + receiverId + "' está desconectado.");
                            connectedClients.Remove(receiverId);
                        }
                    }
                    else
                    {
                        LogMessage("O cliente '" + receiverId + "' não está conectado.");
                    }
                }
            }
            catch (Exception ex)
            {
                LogMessage("Erro ao enviar arquivo para o cliente " + receiverId + ": " + ex.Message);
            }
        }

        private void LogMessage(string message)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(LogMessage), message);
                return;
            }

            serverLogTextBox.AppendText(message + Environment.NewLine);
        }

        private void StopServerButton_Click(object sender, EventArgs e)
        {
            if (server != null && server.IsBound)
            {
                // Desconectar todos os clientes antes de parar o servidor
                foreach (Socket client in clientList)
                {
                    client.Close();
                }

                server.Close();
                LogMessage("Servidor desligado.");
            }
        }
    }
}
