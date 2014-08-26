using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
//using System.Linq;
using System.Text;
using System.Threading;

namespace WirelessBrocast
{


    public enum  StatusIndex :int 
{
        BUSY=0,
        Door=1,
        AC=2,
        DC=3,
        AMP=4,
        SPEAKER=5

}
    public delegate void SlaveReceiveEventHandler(object sender, byte[] data); 

    public delegate void ComErrorEventHandler(string errmsg);

    public class KenWood
    {
         public event SlaveReceiveEventHandler OnSlaveReceiveEvent;
         public event ComErrorEventHandler OnComError;
       string comPort;
       SerialPort com;
       Thread ReceiverThread;
       Thread SendThread;
       int id;
       object toutObj = new object();
        public  bool IsMaster;
        public KenWood(int id,string comPort, bool IsMaster)
       {
           this.id = id;
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

           
          
           System.Threading.Thread.Sleep(500);
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


        public bool SetIO(int id,int dest, bool On)
        {
            // dest 1:PTT, 2:priority output 3:mute
            byte[] res = Send(new byte[] { (byte)id, (byte)'O', (byte)(dest) , (On)?(byte)1:(byte)0});
            if (res == null)
                return false;
            if (res[0] == id)
                return true;
            else
                return false;
        }

        public bool SetIO3(int id, int dest, bool On)
        {
            // dest 1:PTT, 2:priority output 3:mute


            byte[] res = Send(new byte[] { (byte)id, (byte)'O', On?(byte)(dest):(byte)0, (On) ? (byte)1 : (byte)0 });
            if (res == null)
                return false;
            if (res[0] == id)
                return true;
            else
                return false;
        }
        public bool SendDateTime(int id, int year,int month,int day,int hour,int min,int sec)
        {
             Send (new byte[] { (byte)id, (byte)'U', (byte)(year-2000),(byte)month,(byte)day,(byte)hour,(byte)min,(byte)sec });

             return true;
            

        }
        public bool Test(int id, bool IsSilent,int repeat)
        {
            byte[] res = Send(new byte[] { (byte)id, (byte)'T', (byte)(IsSilent?1:0), (byte)repeat });
            if (res == null)
                return false;
            if (res[0] == id)
                return true;
            else
                return false;

        }

        public bool EnableAmpSpkTest(int id)  // for IO
        {

            byte[] res = Send(new byte[] { (byte)id, (byte)'A'});   //make IO board  fetch test result
            if (res == null)
                return false;
            if (res[0] == id)
                return true;
            else
                return false;
        }
        public bool VoiceBroadcast(int id, bool start)
        {

            byte[] res = Send(new byte[] { (byte)id, (byte)'V', start?(byte)1:(byte)0 });
            if (id == 0)
                return true;
            if (res == null)
                return false;
            if (res[0] == id)
                return true;
            else
                return false;
        }
        public bool Play(int id,int inx, int cnt)
        {
            
            byte[] res = Send(new byte[] { (byte)id, (byte)'P', (byte)inx, (byte)cnt });
            if (res == null)
                return false;
            if (res[0] == id)
                return true;
            else
                return false;
        }
        public bool Echo(int id)
        {
            byte[] res = Send(new byte[] { (byte)id, (byte)'E' });
            if (res == null)
                return false;
            if (res[0] == id)
                return true;
            else
                return false;
        }
        public bool Abort(int id)
        {
            byte[] res = Send(new byte[] { (byte)id, (byte)'X' });
            if (res == null)
                return false;
            if (res[0] == id)
                return true;
            else
                return false;
        }
        public bool GetPlayStatus(int id,out byte status,out byte status1, out int cnt)
        {
            byte[] res;
           res=Send(new byte[] {(byte)id, (byte)'S' }) ;
          
            if (res!= null)
            {
                if (res.Length != 5)
                {
                    status = status1 = 0;
                        cnt = (byte)0;
                    return false;
                }
                status =  res[2];
                status1 = res[3];
                cnt = res[4];
                return true;
            }
            else
            {
                status =0;
                status1 = 0;
                cnt = 0;
                return false;
            }
        }

       public void Close()
       {
           IsExit = true;
           if (this.ReceiverThread != null)
           {
               this.ReceiverThread.Abort();
               this.SendThread.Abort();
           }
           com.Close();
           com.Dispose();
           
       }
       byte[] retcmd;

       //public void SendWithNoReply(byte[] cmd)
       //{
       //    MemoryStream ms = new MemoryStream();
       //    ms.WriteByte(0xaa);
       //    ms.WriteByte((byte)(cmd.Length));
       //    int crc = 0;

       //    for (int i = 0; i < cmd.Length; i++)
       //    {
       //        crc ^= cmd[i];
       //        ms.WriteByte(cmd[i]);
       //    }
       //    ms.WriteByte((byte)crc);
       //    ms.WriteByte(0xbb);
       //    ms.Position = 0;
       //    byte[] outdata = ms.ToArray();
          
       //}
        public byte[] Send( byte[]cmd)
       {
       
           MemoryStream ms = new MemoryStream();
           ms.WriteByte(0xaa);
           ms.WriteByte((byte)(cmd.Length ));
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
           if (cmd[0] == 0)  // brocast no reply
           {

               System.Threading.Thread.Sleep(100);
               lock (this)
               {
                   com.Write(outdata, 0, outdata.Length);
                   com.BaseStream.Flush();
               }
               return new byte[0]; ;
           }

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
                   } while (trycnt < 1);
                   return null; //timeout
               }
           }



     public  void Reply(byte[] cmd)
       {
           MemoryStream ms = new MemoryStream();
           ms.WriteByte(0xaa);
           ms.WriteByte((byte)(cmd.Length));
          
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

                   byte id = payload[0];
                   if (!this.IsMaster)
                   {
                       if ( id!=0 &&  id != this.id)
                           continue;
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

               catch (Exception ex){
                   if( ex is System.IO.IOException  && this.OnComError != null)
                       this.OnComError(ex.Message);
                   ;}
           }
           
       }
    }
}
