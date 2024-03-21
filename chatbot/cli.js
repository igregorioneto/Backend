const net = require('net')

// Configuração do cliente
const HOST = '127.0.0.1'
const PORT = 65432

// Conectando ao servidor
const client = new net.Socket()
client.connect(PORT, HOST, () => {
    console.log('Conectado ao servidor')
    
})