using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Diagnostics;
using System.Timers;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;


namespace MqttClientApp
{
    class Program
    {

        MqttClient client;
        string strTopic = "MQTTTestNCR";
        string strHost = "test.mosquitto.org";
        int Port = 1883;
        static void Main(string[] args)
        {
            Program program = new Program();
            program.ConnectAndSubscribe();

            Console.ReadLine();

            program.DisconnectAndUnsubscribe();
        }


        public void ConnectAndSubscribe()
        {
            try
            {
                if (client == null)
                {
                    client = new MqttClient(strHost);
                    client.Connect(new Guid().ToString(), null, null, true, 10);
                    System.Threading.Thread.Sleep(1000);
                    if (client.IsConnected)
                    {
                        client.Subscribe(new string[] { strTopic }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });
                        client.MqttMsgPublishReceived += Client_MqttMsgPublishReceived;
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        public void DisconnectAndUnsubscribe()
        {
            try
            {
                if (client.IsConnected) client.Disconnect();
                client.MqttMsgPublishReceived -= Client_MqttMsgPublishReceived;
                client = null;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void Client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            Console.WriteLine(System.Text.Encoding.UTF8.GetString(e.Message));
        }
    }
}
