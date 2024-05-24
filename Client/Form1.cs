using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.IO;

namespace ChatApp
{
    public partial class Form1 : Form
    {
        private Socket client;
        private string clientId;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ConnectToServer();
        }

        private void ConnectToServer()
        {
            try
            {
                client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                client.Connect("26.29.146.215", 8888);
                MessageBox.Show("Conectado ao servidor.");

                clientId = Prompt.ShowDialog("Digite seu nome de usuário:", "Identificação");
                while (string.IsNullOrWhiteSpace(clientId))
                {
                    MessageBox.Show("O nome de usuário não pode estar vazio ou conter apenas espaços em branco. Por favor, tente novamente.");
                    clientId = Prompt.ShowDialog("Digite seu nome de usuário:", "Identificação");
                }
                byte[] idData = Encoding.ASCII.GetBytes(clientId);
                client.Send(idData);

                Thread receiveThread = new Thread(ReceiveMessages);
                receiveThread.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao conectar ao servidor: " + ex.Message);
            }
        }

        private void ReceiveMessages()
        {
            try
            {
                byte[] buffer = new byte[1024];
                while (true)
                {
                    int bytesRead = client.Receive(buffer);
                    string messageHeader = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                    string[] headerParts = messageHeader.Split('|');

                    if (headerParts[0] == "FILE")
                    {
                        string fileName = headerParts[1];
                        int fileSize = int.Parse(headerParts[2]);

                        byte[] fileBuffer = new byte[fileSize];
                        int totalBytesReceived = 0;

                        while (totalBytesReceived < fileSize)
                        {
                            bytesRead = client.Receive(fileBuffer, totalBytesReceived, fileSize - totalBytesReceived, SocketFlags.None);
                            totalBytesReceived += bytesRead;
                        }

                        SaveFile(fileName, fileBuffer);
                    }
                    else
                    {
                        AddMessageToChat(messageHeader);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao receber mensagens do servidor: " + ex.Message);
            }
        }

        private void SaveFile(string fileName, byte[] fileData)
        {
            Invoke(new Action(() =>
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    FileName = fileName,
                    Filter = "All Files (*.*)|*.*"
                };
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllBytes(saveFileDialog.FileName, fileData);
                    MessageBox.Show("Arquivo recebido e salvo com sucesso.");
                    AddMessageToChat("Você recebeu o arquivo (" + fileName +")");
                }
            }));
        }

        private void AddMessageToChat(string message)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(AddMessageToChat), message);
                return;
            }
            chatBox.AppendText(message + Environment.NewLine);
        }

        private void sendMessageButton_Click(object sender, EventArgs e)
        {
            string message = messageTextBox.Text.Trim();
            if (!string.IsNullOrEmpty(message))
            {
                string receiverId = receiverTextBox.Text.Trim();
                string fullMessage = receiverId + "|" + message;
                byte[] data = Encoding.ASCII.GetBytes(fullMessage);
                client.Send(data);
                AddMessageToChat("Você: " + message);
                messageTextBox.Clear();
            }
        }

        private void sendFileButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string receiverId = receiverTextBox.Text.Trim();
                string fileName = Path.GetFileName(openFileDialog.FileName);
                byte[] fileData = File.ReadAllBytes(openFileDialog.FileName);
                int fileSize = fileData.Length;

                string header = $"FILE|{receiverId}|{fileName}|{fileSize}";
                byte[] headerData = Encoding.ASCII.GetBytes(header);
                client.Send(headerData);

                client.Send(fileData);
                AddMessageToChat("Você enviou o arquivo (" + fileName +")");
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                client?.Shutdown(SocketShutdown.Both);
                client?.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao fechar a conexão: " + ex.Message);
            }
        }
    }
}
