using System;
using Newtonsoft.Json;
using System.Windows.Forms;
using Quobject.SocketIoClientDotNet.Client;

namespace socketForms
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            socketIoManager();
        }

        private void socketIoManager()
        {
            // aqui ficam as configurações que nós definimos no back-end
            // nada de mais, mas sem elas não tem como fazer a conexão
            var option = new IO.Options() {
                Path = "/socket/",
                AutoConnect = true,
            };

            // aqui vc instancia o soket passando as configurações
            // com essa instancia vc consegue escutar as conexões e mandar informacoes
            // aqui vc coloca o endereço da api
            
            var socket = IO.Socket("http://localhost:0000", option);

            // esse evento só checa se a conexao foi feita
            socket.On(Socket.EVENT_CONNECT, () =>
            {
                Console.WriteLine("Connected");
                System.Diagnostics.Debug.WriteLine("Connected");
            });
            ////////////////////////////////////////////////////////////////////////////////

            // esse evento escuta todas as mensagens mandadas para esse chat
            // com o id da conversa você está participando
            var chatId = "49c3e4c0-9936-11ea-b8a9-5b7fcb8fc688";
            var mensagemss = new { texto = "", chatId = "" };
            
            socket.On("leadsOnline", (leads) => 
            {
                Console.WriteLine(leads);
            });
            
            socket.On("digitando-chat-" + chatId, (leads) => 
            {
                Console.WriteLine(leads);
            });

            socket.On("usuariosOnline", (usuarios) => 
            {
                Console.WriteLine(usuarios);
            });

            // esse é o evento que fica escutando as respostas do chat
            socket.On("chegouMensagem-chat", (mensagem) => 
            {
                // convertendo porcamente só para mostrar o objeto javascript em um objeto c#
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(mensagem);
                var msg = JsonConvert.DeserializeAnonymousType((string)json, mensagemss);

                // verificando o id do chat
                if(String.Equals(msg.chatId, chatId))
                {
                    // mostrando a mensagem do chat
                    Console.WriteLine(msg.texto);
                }
            });
        }



        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Console.WriteLine(textBox1.Text);
            System.Diagnostics.Debug.WriteLine(textBox1.Text);
        }
    }
}
