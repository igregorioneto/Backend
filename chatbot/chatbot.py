import os, socket

# Configuração do Servidor
HOST = '127.0.0.1'
PORT = 65432

# Inicia o servidor
with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as s:
    s.bind((HOST, PORT))
    s.listen()

    # Aceita conexão com o cliente
    conn, addr = s.accept()
    with conn:
        print(f"Conectado por: {addr}")
        while True:
            # Recebe mensagem do Cliente
            data = conn.recv(1024)
            if not data:
                break
            message = data.decode()

            response = f"Response: {message}"

            # Envia mensagem para o cliente
            conn.sendall(response.encode())