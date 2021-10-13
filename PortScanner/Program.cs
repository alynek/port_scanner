using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;

namespace PortScanner
{
    public class Program
    {
        public static void Main(string[] args)
        {
            List<int> portas = new List<int>();
            string enderecoIp;
            int portaMin;
            int portaMax;
            string intervaloDePortas;

            while (true)
            {
                Console.Clear();
                Console.WriteLine("Insira o endereço IP: (exemplo: 192.168.1.248)");
                enderecoIp = Console.ReadLine();

                if (EhEnderecoIPV4(enderecoIp))
                {
                    Console.WriteLine($"IP é valido");
                    break;
                }
            }

            while (true)
            {
                Console.Clear();
                Console.WriteLine("Insira o intervalo da porta (exemplo: 0-65535)");
                intervaloDePortas = Console.ReadLine();

                string[] arrayDePortas = intervaloDePortas.Split("-");

                if (PortaEhValida(arrayDePortas))
                {
                    portaMin = int.Parse(arrayDePortas[0]);
                    portaMax = int.Parse(arrayDePortas[1]);
                    break;
                }
            }

            for (int porta = portaMin; porta < portaMax+1; porta++)
            {
                try
                {
                    Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    IPEndPoint endpoint = new IPEndPoint(IPAddress.Parse(enderecoIp), porta);
                    socket.Connect(endpoint);
                    portas.Add(porta); 
                    Console.WriteLine(" ");
                    Console.WriteLine("Porta aberta encontrada..." + porta);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            portas.ForEach(p => Console.WriteLine($"Porta {p} está aberta no endereço {enderecoIp}"));
        }

        public static bool EhEnderecoIPV4(string enderecoIp)
        {
            Regex regex = new Regex(@"\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b");

            MatchCollection resultado = regex.Matches(enderecoIp);

            if (resultado.Count > 0) return true;

            return false;
        }

        public static bool PortaEhValida(string[] arrayDePortas)
        {
            if (arrayDePortas.Length == 2)
            {
                Regex regex = new Regex(@"^((6553[0-5])|(655[0-2][0-9])|(65[0-4][0-9]{2})|(6[0-4][0-9]{3})|([1-5][0-9]{4})|([0-5]{0,5})|([0-9]{1,4}))$");
                MatchCollection resultado1 = regex.Matches(arrayDePortas[0]);
                MatchCollection resultado2 = regex.Matches(arrayDePortas[1]);

                if (resultado1.Count > 0 && resultado2.Count > 0) return true;
            }
            return false;
        }
    }
}
