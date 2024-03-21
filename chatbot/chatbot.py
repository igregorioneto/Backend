import os, socket
import nltk
from nltk.chat.util import Chat, reflections

# Configuração do Servidor
HOST = '127.0.0.1'
PORT = 65432

pairs = [
    [
        r'Oi|Olá|E aí|Olá!|Oi!|E aí!',
        ['Olá, em que posso ajudar?']
    ],
    [
        r'Como vai você?|Como você está?',
        ['Estou bem, obrigado por perguntar. Como posso ajudar você hoje?']
    ],
    [
        r'Quero abrir uma conta bancária|Gostaria de abrir uma conta',
        ['Ótimo! Podemos ajudar com isso. Você prefere uma conta corrente ou uma conta poupança?']
    ],
    [
        r'Conta corrente',
        ['Entendi. Para abrir uma conta corrente, precisaremos dos seguintes documentos: comprovante de residência, documento de identidade (RG ou CNH) e comprovante de renda. Você possui esses documentos?']
    ],
    [
        r'Conta poupança',
        ['Entendi. Para abrir uma conta poupança, precisaremos dos seguintes documentos: comprovante de residência, documento de identidade (RG ou CNH) e comprovante de renda. Você possui esses documentos?']
    ],
    [
        r'(Sim|Tenho|Possuo)',
        ['Perfeito! Vou precisar que você agende um horário em uma de nossas agências para concluir o processo de abertura de conta. Posso ajudar com mais alguma coisa?']
    ],
    [
        r'(Não|Não tenho|Não possuo)',
        ['Sem problemas. Se precisar de mais alguma informação ou desejar abrir uma conta mais tarde, estarei aqui para ajudar.']
    ],
    [
        r'(Obrigado|Obrigado!)',
        ['De nada! Estou aqui para ajudar. Se precisar de mais alguma coisa, não hesite em me chamar.']
    ],
    [
        r'Sair|Tchau|Até mais|Fim|Encerrar',
        ['Obrigado por usar nossos serviços! Tenha um ótimo dia.']
    ],
    [
        r'(.*)',
        ['Desculpe, não entendi. Você pode reformular sua pergunta?']
    ]
]

chatbot = Chat(pairs, reflections)

# Inicia o servidor
with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as s:
    s.bind((HOST, PORT))
    s.listen()
    print(f"Servidor executado em : {HOST}:{PORT}")
    # Aceita conexão com o cliente
    conn, addr = s.accept()
    print(f"Conectado por: {addr}")
    with conn:            
        while True:
            # Recebe mensagem do Cliente
            data = conn.recv(1024)
            if not data:
                break
            message = data.decode()

            # response = f"{message}"
            response = chatbot.respond(message)

            # Envia mensagem para o cliente
            conn.sendall(response.encode())