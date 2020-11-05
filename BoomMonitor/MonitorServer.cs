using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace BoomMonitor
{
    class MonitorServer
    {

        private static IPAddress remoteIPAddress;
        //private static int remotePort;
        private static int localPort;
        private static Thread tRec;

        static bool isCancellationRequested = true;
        private static UdpClient receivingUdpClient;
        [STAThread]
        public static void Start()
        {
            try
            {

                localPort = Convert.ToInt32(49000);
                remoteIPAddress = IPAddress.Parse("127.0.0.1");
                tRec = new Thread(new ThreadStart(Receiver));
                tRec.Start();


            }
            catch (Exception ex)
            {
                Log.Add(ex.Message);
            }
        }

        public static void Send(string datagram, int port)
        {
            // Создаем UdpClient
            UdpClient sender = new UdpClient();

            // Создаем endPoint по информации об удаленном хосте
            IPEndPoint endPoint = new IPEndPoint(remoteIPAddress, port);

            try
            {
                // Преобразуем данные в массив байтов
                byte[] bytes = Encoding.UTF8.GetBytes(datagram);

                // Отправляем данные
                sender.Send(bytes, bytes.Length, endPoint);
            }
            catch (Exception ex)
            {
                Log.Add("Exception: " + ex.Message);
            }
            finally
            {
                // Закрыть соединение
                sender.Close();
            }
        }

        public static void Receiver()
        {
            // Создаем UdpClient для чтения входящих данных
            bool opened = false;

            while (!opened)
            {
                try { receivingUdpClient = new UdpClient(localPort); opened = true; }
                catch
                {

                    Form1.Instance.BeginInvoke((Action)(() =>
                    {
                        Form1.Instance.Hide();
                    }));

                    MessageBox.Show("Only one copy of BoomMonitor can be running at a time\nThis error may also occur due to a busy port by another application", "Error! One copy is already running!", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    Application.Exit(); break;
                }

            }


            IPEndPoint RemoteIpEndPoint = null;

            try
            {


                while (isCancellationRequested)
                {



                    // Ожидание дейтаграммы
                    byte[] receiveBytes = receivingUdpClient.Receive(
                       ref RemoteIpEndPoint);

                    // Преобразуем и отображаем данные
                    string returnData = Encoding.UTF8.GetString(receiveBytes);

                    if (returnData.StartsWith("{\"hello"))
                    {

                        //Log.Add(returnData);
                        try
                        {
                            var bot = JObject.Parse(returnData)["hello"];


                            if (Form1.Instance.InvokeRequired)
                            {
                                Form1.Instance.BeginInvoke((Action)(() =>
                                {
                                    Form1.Instance.CreateBot(bot);
                                }));
                            }
                            else
                            {
                                Form1.Instance.CreateBot(bot);
                            }

                        }
                        catch (Exception ex)
                        {
                            Log.Add(ex.Message);
                        }
                    }
                    else if (returnData.StartsWith("{\"log"))
                    {
                        try
                        {
                            var log = JObject.Parse(returnData)["log"];

                            var message = log["message"].ToString();
                            var name = log["name"].ToString();
                            var port = Convert.ToInt32(log["port"]);

                            Log.Add(message, name);

                            foreach (var bot in Form1.Instance.bots)
                            {
                                if (bot.Port == port)
                                    bot.AddLog(message);
                            }

                        }
                        catch (Exception ex)
                        {
                            Log.Add(ex.Message);
                        }
                    }
                    else
                    {
                        Log.Add("Unknown data was received, you may need to update the program");
                    }

                }
            }
            catch (SocketException)
            {
                //

            }
            catch (Exception ex)
            {
                //if (ex.InnerException == )
                Log.Add("Exception: " + ex.Message);
            }
        }
        public static void Stop()
        {

            isCancellationRequested = false;
            if (receivingUdpClient != null)
                receivingUdpClient.Close();
            receivingUdpClient = null;


        }


    }

}

