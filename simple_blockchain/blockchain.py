import hashlib, json
from time import time

class Blockchain:
    def __init__(self):
        self.chain = []
        self.current_transactions = []

        # Criar bloco Genesis quando uma nova instância 
        # da classe Blockchain é criada.
        self.new_block(previous_hash="1",proof=100)

    """
    Adicionar um novo bloco a blockchain.
    Parâmetros = Prova e Hash do bloco anterior.
    Se o previous_hash não for fornecido é utilizado o do bloco anterior.
    """
    def new_block(self, proof, previous_hash=None):
        block = {
            "index": len(self.chain) + 1,
            "timestamp": time(),
            "transactions": self.current_transactions,
            "proof": proof,
            "previous_hash": previous_hash or self.hash(self.chain[-1])
        }

        # Resetar as transações atuais
        self.current_transactions = []
        self.chain.append(block)
        return block
    
    """
    Este método é usado para adicionar uma nova transação.
    Parametros = Remetente, Destinatário, Valor da transação
    Retorna o índice do bloco que conterá a transação.
    """
    def new_transaction(self, sender, recipient, amount):
        self.current_transactions.append({
            "sender": sender,
            "recipient": recipient,
            "amount": amount,
        })
        return self.last_block["index"] + 1
    
    """
    Método estático que calcula o hash de um bloco.
    O Hash resultante é retornado.
    """
    @staticmethod
    def hash(block):
        block_string = json.dumps(block, sort_keys=True).encode()
        return hashlib.sha256(block_string).hexdigest()
    
    """
    Propriedade somente leitura. Retorna o bloco chain
    """
    @property
    def last_block(self):
        return self.chain[-1]
    
    """
    Implementa o Prova de Trabalho que serve para resolver um problema específico.
    Onde é usado para encontrar um valor (nonce) que, quando combinado com os dados do bloco,
    produz um hash que atenda a determinados critérios.
    Neste caso o critério específico da prova é que o hash resultante tenha quatro zeros iniciais.
    O Método retorna uma prova gerada.
    """
    def proof_of_work(self, last_proof):
        proof = 0
        while self.valid_proof(last_proof, proof) is False:
            proof += 1
        return proof
    
    """
    Encontrar um valor de prova (proof) que atenda ao critério
    de encontrar no hash onde os quatro primeiros valores sejam iguais a '0000'
    """
    @staticmethod
    def valid_proof(last_proof, proof):
        guess = f"{last_proof}{proof}".encode()
        guess_hash = hashlib.sha256(guess).hexdigest()
        return guess_hash[:4] == "0000"

    