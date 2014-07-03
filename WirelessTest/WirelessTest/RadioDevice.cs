using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;

namespace WirelessTest
{

    public delegate  void SlaveReceiveEventHandler(object sender,byte[]data); 
   public  class RadioDevice
    {
       public event SlaveReceiveEventHandler OnSlaveReceiveEvent;

       string comPort;
       SerialPort com;
       Thread ReceiverThread;
       Thread SendThread;
       object toutObj = new object();
        public  bool IsMaster;
       public RadioDevice(string comPort, bool IsMaster)
       {
           this.comPort = comPort;
           this.IsMaster=IsMaster;
           com = new SerialPort(comPort, 9600, Parity.None, 8, StopBits.One);
           try
           {
               com.Open();
           }
           catch (Exception ex)
           {
               Console.WriteLine(ex.Message);
               throw ex;
           }

           
          
           System.Threading.Thread.Sleep(1000);
           int cnt = com.BytesToRead;
           for (int i = 0; i < cnt; i++)
           {
              // lock(this)
               com.ReadByte();
           }
           
        //   long l=  com.BaseStream.Length;
       //    for (int i = 0; i < l; i++)
        //       com.ReadByte();
           
         //  if(!IsMaster)
           ReceiverThread = new Thread(ReceiverTask) ;

           ReceiverThread.Start();
       }


       public void Close()
       {
           IsExit = true;
           com.Close();
           com.Dispose();
       }
       byte[] retcmd;
        public byte[] Send(byte[]cmd)
       {
       
           MemoryStream ms = new MemoryStream();
           ms.WriteByte(0xaa);
           ms.WriteByte((byte)cmd.Length);
           int crc = 0;
           for (int i = 0; i < cmd.Length; i++)
           {
               crc ^= cmd[i];
               ms.WriteByte(cmd[i]);
           }
           ms.WriteByte((byte)crc);
           ms.WriteByte(0xbb);
           ms.Position = 0;
           byte[] outdata = ms.ToArray();
          
               lock (toutObj)
               {
                   int trycnt = 0;
                   do
                   {
                        System.Threading.Thread.Sleep(100);
                        lock (this)
                        {
                            com.Write(outdata, 0, outdata.Length);
                            com.BaseStream.Flush();
                        }
                       Console.WriteLine("send payload");
                       if (System.Threading.Monitor.Wait(toutObj, 3000))
                       {
                           return retcmd;
                       }
                       else
                       {
                           trycnt++;


                       }
                   } while (trycnt < 3);
                   return null; //timeout
               }
           }



     public  void Reply(byte[] cmd)
       {
           MemoryStream ms = new MemoryStream();
           ms.WriteByte(0xaa);
           ms.WriteByte((byte)cmd.Length);
           int crc = 0;
           for (int i = 0; i < cmd.Length; i++)
           {
               crc ^= cmd[i];
               ms.WriteByte(cmd[i]);
           }
           ms.WriteByte((byte)crc);
           ms.WriteByte(0xbb);
           ms.Position = 0;
           byte[] outdata = ms.ToArray();
           System.Threading.Thread.Sleep(100);
           lock (this)
           {
               
               com.Write(outdata, 0, outdata.Length);
               com.BaseStream.Flush();
           }
       }
     bool IsExit;
       void ReceiverTask()
       {
           //aa len data crc bb

           while (true)
           {
               int data;
               if (IsExit)
                   return;
               try
               {
                   do
                   { 
                       data = com.ReadByte();
                       Console.WriteLine("{0:X2}", data);
                   } while (data != 0xaa);
                   int len;
                  
                    len = com.ReadByte();
                   if (len > 48) continue;  //
                   Console.WriteLine("{0:X2}", len);
                   byte[] payload = new byte[len];
                   for (int i = 0; i < len; i++)
                   {
                      
                       payload[i] = (byte)com.ReadByte();
                       Console.WriteLine("{0:X2}", payload[i]);
                   }
                   int crc ;
                  
                   crc= com.ReadByte();
                   Console.WriteLine("{0:X2}", crc);
                   int crcchk = 0;
                   for (int i = 0; i < payload.Length; i++)
                   {
                       //  payload[i] = (byte)com.ReadByte();
                       crcchk ^= payload[i];
                   }
                   if (crc != crcchk)
                   {
                       Console.WriteLine("chksum error!");
                       continue;
                   }
                   int tail = com.ReadByte();
                   Console.WriteLine("{0:X2}", tail);
                   if (tail != 0xbb)
                   {
                       Console.WriteLine("no 0xbb");
                       continue;
                   }
                   // payload receive
                   retcmd = payload;
                   if (!IsMaster)
                   {
                       if (OnSlaveReceiveEvent != null)
                       {
                           Console.WriteLine("receive payload");
                           OnSlaveReceiveEvent(this, payload);

                       }
                       // reply master here

                   }
                   else  //
                   {
                       lock (toutObj)
                       {
                           System.Threading.Monitor.PulseAll(toutObj);
                       }
                   }

               }

               catch { ;}
           }
           
       }
    }


  
}
