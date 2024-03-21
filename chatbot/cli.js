const net = require('net')

// Configuração do cliente
const HOST = '127.0.0.1'
const PORT = 65432

// Conectando ao servidor
const client = new net.Socket()
client.connect(PORT, HOST, () => {
    console.log('Conectado ao servidor')

    function sendMessage() {
        // Interação com o usuário
        const readline = require('readline')
        const r1 = readline.createInterface({
            input: process.stdin,
            output: process.stdout
        })

        // Mensagem para o servidor
        r1.question('Você: ', (input) => {
            client.write(input)
            if (['sair','tchau','até mais','fim','encerrar'].includes(input)) {
                client.destroy()
            }
            r1.close()
        })        
    }

    sendMessage()

    // Receber mensagem do servidor
    client.on('data', (data) => {
        message = data.toString()
        console.log('ChatBot: ', message)
        if (message === 'sair') {
            client.destroy()
        } else {
            sendMessage()
        }            
    })

})

// Tratar eventos de erro
client.on('error', (err) => [
    console.error('Erro: ', err.message)
])