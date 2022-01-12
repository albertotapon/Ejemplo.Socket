using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Linq;

namespace Calculator.Cliente
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Console C#\r");
            Console.WriteLine("------------------------\n");

            while (true)
            {
                Console.WriteLine("Mensaje:");
                string mensaje = Console.ReadLine();

                var resultado = EnviaMenaje(mensaje);

                Console.WriteLine(resultado);
            }

        }

        static string EnviaMenaje(string mensaje)
        {
            try
            {
                // Connect to a Remote server
                // Get Host IP Address that is used to establish a connection
                // In this case, we get one IP address of localhost that is IP : 127.0.0.1
                // If a host has multiple addresses, you will get a list of addresses

                IPHostEntry host = Dns.GetHostEntry("localhost");
                //IPHostEntry host = Dns.GetHostEntry("C13-16");
                IPAddress ipAddress = host.AddressList[0];

                //IPAddress ipAddress = IPAddress.Parse("ip destino");

                IPEndPoint remoteEP = new IPEndPoint(ipAddress, 2800);

                // Create a TCP/IP  socket.
                using Socket sender = new Socket(ipAddress.AddressFamily,
                    SocketType.Stream, ProtocolType.Tcp);

                // Connect the socket to the remote endpoint. Catch any errors.
                try
                {
                    // Connect to Remote EndPoint
                    sender.Connect(remoteEP);

                    Console.WriteLine("Socket connected to {0}",
                        sender.RemoteEndPoint.ToString());

                    Console.WriteLine("Socket redad for {0}",
                        sender.LocalEndPoint.ToString());

                    var cacheEnvio = Encoding.UTF8.GetBytes(mensaje);

                    // Send the data through the socket.
                    int bytesSend = sender.Send(cacheEnvio);

                    // Receive the response from the remote device.
                    byte[] bufferRec = new byte[1024];
                    int bytesRec1 = sender.Receive(bufferRec);

                    var resultado = Encoding.UTF8.GetString(bufferRec, 0, bytesRec1);

                    // Release the socket.
                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();

                    return resultado;

                }
                catch (ArgumentNullException ane)
                {
                    Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
                }
                catch (SocketException se)
                {
                    Console.WriteLine("SocketException : {0}", se.ToString());
                }
                catch (Exception e)
                {
                    Console.WriteLine("Unexpected exception : {0}", e.ToString());
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return null;
        }
    }
}
