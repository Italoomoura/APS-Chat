# Client

Client é uma aplicação de chat que permite aos usuários enviar mensagens para um servidor e receber mensagens de outros usuários conectados ao mesmo servidor.

## Como Executar

1. **Pré-requisitos:**
   - Certifique-se de ter o .NET Framework instalado na sua máquina.

2. **Clonando o Repositório:**

    ```
    git clone https://github.com/Italoomoura/APS-Chat.git
    ```

3. **Compilando e Executando:**
    - Abra o terminal e navegue até o diretório do projeto e execute:

    ```
    dotnet run --project Client
    ```

## Como Usar

1. Ao iniciar a aplicação, você será solicitado a inserir um identificador único (nome de usuário).
2. Após inserir o identificador, você será conectado ao servidor.
3. Insira o nome do destinatário na caixa de texto "Destinatário".
4. Digite sua mensagem na caixa de texto "Mensagem" e clique em "Enviar".
5. As mensagens recebidas serão exibidas na caixa de chat.


# Server

Aplicação de servidor simples que permite aos clientes se conectarem e trocarem mensagens. A aplicação possui uma interface gráfica para facilitar sua utilização.

## Como Executar

1. **Pré-requisitos:**
   - Certifique-se de ter o .NET Framework instalado na sua máquina.

2. **Clonando o Repositório:**

    ```
    git clone https://github.com/Italoomoura/APS-Chat.git
    ```

3. **Compilando e Executando:**
    - Abra o terminal e navegue até o diretório do projeto e execute:

    ```
    dotnet run --project Server
    ```

## Como Utilizar

- Ao iniciar o servidor, ele estará aguardando conexões de clientes.
- Os clientes podem se conectar ao servidor usando o endereço IP e porta especificados.
- Após a conexão, os clientes podem trocar mensagens entre si.
- Para desligar o servidor, clique no botão "Desligar Servidor".
