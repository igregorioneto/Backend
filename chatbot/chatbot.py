import os, socket

# Configuração do Servidor
HOST = '127.0.0.1'
PORT = 65432

# Inicia o servidor
with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as s:
    s.bind((HOST, PORT))
    s.listen()
    print(f"Servidor executado em : {HOST}:{PORT}")
    # Aceita conexão com o cliente
    conn, addr = s.accept()
    print(f"Conectado por: {addr}")
    while True:
        with conn:            
            while True:
                # Recebe mensagem do Cliente
                data = conn.recv(1024)
                if not data:
                    break
                message = data.decode()

                response = f"{message}"

                # Envia mensagem para o cliente
                conn.sendall(response.encode())