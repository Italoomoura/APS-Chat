using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.ComponentModel;
using System.Windows.Forms;

namespace SimpleServerGUI
{
    public partial class ServerForm : Form
    {
        private static Dictionary<string, TcpClient> connectedClients = new Dictionary<string, TcpClient>();
        private static readonly object lockObject = new object();
        private BackgroundWorker serverWorker;
        private TcpListener server;
        private List<TcpClient> clientList = new List<TcpClient>();

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
                IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
                int port = 8888;
                server = new TcpListener(ipAddress, port);
                server.Start();
                LogMessage("Servidor iniciado. Aguardando conexões...");

                while (true)
                {
                    TcpClient client = server.AcceptTcpClient();
                    clientList.Add(client); // Adiciona o cliente à lista de clientes

                    LogMessage("Aguardando identificação do cliente...");

                    // Solicitar um identificador único ao cliente
                    NetworkStream stream = client.GetStream();
                    byte[] buffer = new byte[1024];
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
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

        private void HandleClient(string clientId, TcpClient client)
        {
            try
            {
                LogMessage("(" + clientId + ") conectado.");

                NetworkStream stream = client.GetStream();
                while (true)
                {
                    byte[] buffer = new byte[1024];
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    string message = Encoding.ASCII.GetString(buffer, 0, bytesRead);

                    if (string.IsNullOrEmpty(message))
                    {
                        lock (lockObject)
                        {
                            connectedClients.Remove(clientId);
                        }
                        LogMessage("(" + clientId + ") desconectado.");
                        clientList.Remove(client); // Remove o cliente da lista
                        break;
                    }

                    // Encaminhar a mensagem para o(s) destinatário(s) correto(s)
                    SendMessage(clientId, message);
                }
            }
            catch (Exception ex)
            {
                LogMessage("Erro ao lidar com as mensagens do cliente " + clientId + ": " + ex.Message);
            }
        }

        private void SendMessage(string senderId, string message)
        {
            try
            {
                string[] parts = message.Split('|');
                string receiverId = parts[0];
                string actualMessage = parts[1];

                lock (lockObject)
                {
                    if (connectedClients.ContainsKey(receiverId))
                    {
                        TcpClient receiverClient = connectedClients[receiverId];
                        if (receiverClient.Connected)
                        {
                            NetworkStream stream = receiverClient.GetStream();
                            byte[] data = Encoding.ASCII.GetBytes(senderId + ": " + actualMessage);
                            stream.Write(data, 0, data.Length);
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
                LogMessage("Erro ao enviar mensagem para o cliente " + senderId + ": " + ex.Message);
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
            if (server != null && server.Server.IsBound)
            {
                // Desconectar todos os clientes antes de parar o servidor
                foreach (TcpClient client in clientList)
                {
                    client.Close();
                }

                server.Stop();
                LogMessage("Servidor desligado.");
            }
        }
    }
}