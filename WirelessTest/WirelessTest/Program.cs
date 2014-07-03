using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Net;
using System.Text.RegularExpressions;
namespace WirelessTest
{
    class Program
    {
        static  object lockobj = new object();
        static void Main(string[] args)
        {
          //  testc();
            KenWoodTest();
          //  Vlan100sdTest();
          //  Vland100sd_SetRTCNow();
            Console.ReadKey();
           
        }

        static void Vlan100sdTest()
        {
            WebClient client = new WebClient();
            client.Credentials = new NetworkCredential("vlansd", "1234");
           string res= client.UploadString("http://192.168.1.100/vlansys/vlaninquiry?",
                "cDateTime=on&StartYear=2014&StartMonth=5&StartDay=25&StartHour=20&StartMinute=18&StartSecond=0&StopYear=2014&StopMonth=5&StopDay=25&StopHour=20&StopMinute=34&StopSecond=23&tCallerID=&tDTMF=&tRings=&tRecLength=");
           Regex regex = new Regex(@"RecLength=(\d+).*StartTime=(.*?),");
          MatchCollection collection= regex.Matches(res);
           
          for (int i = 0; i < collection.Count; i++)
          {

              Console.WriteLine(collection[i].Groups[2].Value);
          }

       //   Regex rege1 = new Regex("RecLength=4=(.*),.*(StartTime=.*)");
        //    Console.WriteLine(res);

        }

         static void  Vland100sd_SetRTCNow( )
         {

             WebClient client = new WebClient();

             client.Credentials = new NetworkCredential("vlansd", "1234");
             string res = client.UploadString("http://192.168.1.100/vlansys/syscgi?SetRTC ",
                  "RTCClick="+DateTime.Now.ToString("yyyy/M/d H:m:s"));
             Console.WriteLine(DateTime.Now.ToString("yyyy/M/d H:m:s"+res));
         }
        static void KenWoodTest()
        {
            RadioDevice master, slave;
            master = new RadioDevice("Com3", true);
            slave = new RadioDevice("Com4", false);

            slave.OnSlaveReceiveEvent += slave_OnSlaveReceiveEvent;
            while (true)
            {
                byte[] ret = master.Send(new byte[] { 1, 2, 3, 4, 2, 3, 4, 2, 3, 4 });
                if (ret == null)
                {
                    Console.WriteLine("timeout!");
                    master.Close();
                    master = new RadioDevice("Com3", true);
                    continue;
                }
                for (int i = 0; i < ret.Length; i++)
                {
                    Console.Write(ret[i] + ",");

                }
                Console.WriteLine();
                // System.Threading.Thread.Sleep(1000);
                // Console.ReadKey();
            }
        }

        static void slave_OnSlaveReceiveEvent(object sender, byte[] data)
        {
          //  throw new NotImplementedException();
           // System.Threading.Thread.Sleep(100);
            (sender as RadioDevice).Reply(data );
        }

        static void  testc()
        {
             SerialPort port1;
        
          

            new System.Threading.Thread(Receiver).Start();

            lock (lockobj)
            {
                System.Threading.Monitor.Wait(lockobj);
            }


            port1 = new SerialPort("COM3", 9600, Parity.None, 8, StopBits.One);
            port1.Open();
            System.IO.StreamReader rd = new System.IO.StreamReader(port1.BaseStream);
           
            for (int i = 0; i < 100;i++ )
            {



                System.Threading.Thread.Sleep(100);
                
                port1.Write("hellohel" + i + "\n");
                port1.BaseStream.Flush();
                System.Threading.Thread.Sleep(3000);
                if(port1.BytesToRead==4)
                Console.WriteLine(rd.ReadLine());
                

               // Console.WriteLine( port1.ReadLine());
             
             //   Console.WriteLine(rd.ReadLine());
             //   System.Threading.Thread.Sleep(2000);
               System.Threading.Thread.Sleep(100);
               
                //System.Threading.Thread.Sleep(1000);
            }
            port1.Close();
            Console.ReadKey();
            
           //for (int i = 0; i < 100; i++)
           //{
           //    port1.Write("hello"+i+"\n");
           //   // port1.BaseStream.Flush();
           //    System.Threading.Thread.Sleep(10000);
           //}
        }

       static void Receiver()
        {
            SerialPort port2;
            port2 = new SerialPort("COM4", 9600, Parity.None, 8, StopBits.One);

            port2.Open();
          ;
         System.IO.StreamReader rd = new System.IO.StreamReader(port2.BaseStream);
            lock (lockobj)
            {
                System.Threading.Monitor.Pulse(lockobj);
            }
            while (true)
            {



            //    while (true)
                 //   Console.WriteLine("{0:X2}", port2.ReadByte());
                  

                //port2.Close();
                //System.Threading.Thread.Sleep(1000);
                System.Threading.Thread.Sleep(100);
                Console.WriteLine(rd.ReadLine());
                //port2.Open();
                 System.Threading.Thread.Sleep(100);
            //    System.Threading.Thread.Sleep(1000);
                port2.Write("ok!\n");
                port2.BaseStream.Flush();
            //  
               
                //System.Threading.Thread.Sleep(1000);
                //port2.Close();
            }
        }
    }
}
