from flask import Flask, jsonify, request
from uuid import uuid4
from blockchain import Blockchain

app = Flask(__name__)
blockchain = Blockchain()

# Identificador único para o nó
node_identifier = str(uuid4()).replace("-", "")

@app.route("/mine", methods=["GET"])
def mine():
    last_block = blockchain.last_block
    last_proof = last_block["proof"]
    proof = blockchain.proof_of_work(last_proof=last_proof)

    # Recompensa se encontrar a prova
    blockchain.new_transaction(
        sender=0,
        recipient=node_identifier,
        amount=1
    ) 

    previous_hash = blockchain.hash(last_block)
    block = blockchain.new_block(proof, previous_hash)

    response = {
        "message": "Novo bloco criado",
        "index": block["index"],
        "transactions": block["transactions"],
        "proof": block["proof"],
        "previous_hash": block["previous_hash"]
    }

    return jsonify(response), 200

@app.route("/transactions/new", methods=["POST"])
def new_transaction():
    values = request.get_json()

    # Verificar os campos necessários na requisição
    required = ["sender", "recipient", "amount"]
    if not all(k in values for k in required):
        return f"Campo faltando", 400
    
    index = blockchain.new_transaction(sender=values["sender"], recipient=values["recipient"], amount=values["amount"])
    response = {"message": f"A transação será adicionada ao bloco {index}"}
    return jsonify(response), 201

if __name__ == "__main__":
    app.run(host="0.0.0.0", port=5000)