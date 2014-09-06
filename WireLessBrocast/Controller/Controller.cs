using System;
using System.Collections.Generic;
//using System.Linq;
using System.Runtime.InteropServices;
using System.Speech.Synthesis;
using System.Text;
using System.Threading;
using WirelessBrocast;

namespace Controller
{
   public  class Controller
    {
       public string WirelessComPort { get; set; }
       public string TouchPanelComPort { get; set; }
       public string IOComPort { get; set; }
       public TouchPanelManager touch_panel_mgr;
       KenWood kenwood;
      public byte SpeakerOut = 0;
      public  KenWood IOCard;
     //  Recoder recoder;

       System.Threading.Thread IOStatusThread;

      public  System.Collections.BitArray Status = new System.Collections.BitArray(16);
      // char PlayStatus = 'I';  //idle
      IThreadTask voiceThread;
       int id;
      
       public Controller(int id, string WirelessComPort, string TouchPanelComPort,string IOComPort)
       {   this.id=id;
       this.WirelessComPort = WirelessComPort;
       this.TouchPanelComPort = TouchPanelComPort;
       this.IOComPort = IOComPort;
       this.kenwood = new KenWood((byte)id, WirelessComPort, false);
       try
       {
           this.IOCard = new KenWood((byte)0, IOComPort, true);
       }
       catch (Exception ex)
       {
           IOCard = null;
       }
           kenwood.OnSlaveReceiveEvent += kenwood_OnSlaveReceiveEvent;

           this.touch_panel_mgr = new TouchPanelManager(TouchPanelComPort);
           touch_panel_mgr.OnPanelCmdEvent += touch_panel_mgr_OnPanelCmdEvent;

           IOStatusThread = new Thread(IOTask);
           IOStatusThread.Start();
         //  recoder = new Recoder();
       //    new System.Threading.Thread(new ParameterizedThreadStart(PlaySpeech)).Start(new int[] { 0, 3 });
       }


       void IOTask()
       {

           byte[] m_status=new byte[2];
           int cnt=0;
          // bool IsSucess;
           while (true)
           {
               if (IOCard != null)
               {

                   lock (IOCard)
                   {
                       if (IOCard.GetPlayStatus(1, out m_status[0], out m_status[1], out cnt))
                       {
                           System.Collections.BitArray ary = new System.Collections.BitArray(m_status);
                           Status.Set((int)StatusIndex.AC, ary.Get((int)StatusIndex.AC));
                           Status.Set((int)StatusIndex.DC, ary.Get((int)StatusIndex.DC));
                          Status.Set((int)StatusIndex.AMP, ary.Get((int)StatusIndex.AMP));
                           //Status.Set((int)StatusIndex.BUSY, ary.Get((int)StatusIndex.BUSY));
                           Status.Set((int)StatusIndex.Door, ary.Get((int)StatusIndex.Door));
                           if (m_status[1] > SpeakerOut)
                               SpeakerOut = m_status[1];

                       //    Status.Set((int)StatusIndex.SPEAKER, ary.Get((int)StatusIndex.SPEAKER));
                       }
                   }

                   Console.WriteLine("擴大機輸出讀值:{0:X2}", m_status[1]);

               }


               System.Threading.Thread.Sleep(1000);

           }
       }
       

       void touch_panel_mgr_OnPanelCmdEvent(string cmd, params int[] param)
       {
           if (voiceThread != null)
           {
               voiceThread.Abort();
               voiceThread.Join();
               System.Threading.Thread.Sleep(1000);
           }
           if (cmd.ToUpper() == "NORMAL")
           {
               voiceThread = new ThreadPlaySound(param[0], param[1], Status, touch_panel_mgr,this);
             //new System.Threading.Thread(new ParameterizedThreadStart(PlaySpeechTask));
              
           }
           if (cmd == "Silence")
           {
               voiceThread = new ThreadPlay20KTest(param[1], Status, touch_panel_mgr,this);
           }

           voiceThread.Start();
       }
       
       void kenwood_OnSlaveReceiveEvent(object sender, byte[] data)
       {
           try
           {
               if (data[0]!=0  && data[0] != this.id)
                   return;

               string cmd = System.Text.ASCIIEncoding.ASCII.GetString(data);
               if (cmd[1] == 'P') //play   p+index + times
               {

                   if(data[0]!=0)
                   kenwood.Reply(new byte[] { data[0], (byte)'P', (byte)1 });
                   int inx = cmd[2];
                   int repeat = cmd[3];

                   Console.WriteLine("預錄詞命令接收,停止現有任務中!");
                   if (voiceThread != null)
                   {
                       voiceThread.Abort();
                       voiceThread.Join();
                      // System.Threading.Thread.Sleep(1000);
                   }
                   Console.WriteLine("預錄詞{0},{1}次",inx+1,repeat);
                   voiceThread = new ThreadPlaySound(inx, repeat, Status, touch_panel_mgr,this);  // new System.Threading.Thread(new ParameterizedThreadStart(PlaySpeechTask)  );
                   // voiceThread.Start(new int[] { inx, repeat });
                   voiceThread.Start();
               }
               if (cmd[1] == 'T') //tetsing   p+index + times
               {
                   if (data[0] != 0)  //if brocast data[0]==0 no nedd to reply
                   kenwood.Reply(new byte[] { data[0], (byte)'T' });
                  int repeat = cmd[3];
                   bool IsSilent = cmd[2]==1?true:false;
                   Console.WriteLine("測試命令接收,停止現有任務中!");
                 
                   if (voiceThread != null)
                   {
                       voiceThread.Abort();
                       voiceThread.Join();
                      // System.Threading.Thread.Sleep(1000);
                   }
                   
               

                   if (!IsSilent)
                   {

                       Console.WriteLine("開始測試!");
                       voiceThread = new ThreadPlaySpeechTest(0, repeat, Status, touch_panel_mgr, this);  // new System.Threading.Thread(new ParameterizedThreadStart(PlaySpeechTask)  );

                   }
                   else
                   {
                       Console.WriteLine("開始測試!");
                       voiceThread = new ThreadPlay20KTest(repeat, Status, touch_panel_mgr, this);
                   }
                   // voiceThread.Start(new int[] { inx, repeat });
                   voiceThread.Start();
               }
               else if (cmd[1] == 'S') //get play status  [id status cnt]
               {

                   // bit 0~bit7    play  testing  door   power1  power2   amp/speaker
                   byte[] statusdata = new byte[2];
                   Status.CopyTo(statusdata, 0);
                   int pcnt = 0;
                   if (voiceThread != null)
                       pcnt = voiceThread.PlayCnt;
                   if (data[0] != 0)
                   kenwood.Reply(new byte[] { data[0], (byte)'S', statusdata[0],statusdata[1] ,(byte)pcnt });
               }
               else if (cmd[1] == 'X')
               {
                   if (data[0] != 0)
                   kenwood.Reply(new byte[] { data[0], (byte)'X' });
                   Console.WriteLine("中止現有任務!");
                   if (voiceThread != null)
                   {
                       voiceThread.Abort();
                      // voiceThread.Join();
                     //  Status.Set((int)StatusIndex.BUSY, false);
                       Console.WriteLine("任務已中止!");
                       System.Threading.Thread.Sleep(1000);
                   }
                  
               }
               else if (cmd[1] == 'E') //echo
               {

                   if (data[0] != 0)
                   kenwood.Reply(new byte[] { data[0], (byte)'E'});

                   System.Threading.Thread th = new Thread(
                       () =>
                       {
                           Console.WriteLine("Echo begin!");
                           if (IOCard != null)
                           {
                               lock(IOCard)
                               IOCard.SetIO(1, 1, true);
                           }
                           System.Threading.Thread.Sleep(9000);
                           if (IOCard != null)
                           {
                               lock (IOCard)
                               IOCard.SetIO(1, 1, false);
                           }
                           Console.WriteLine("Echo end!");
                       }
                  );

                   th.Start();
                     //  voiceThread.Abort();
                      // do swich ptt of radio here 
               }
               else if (cmd[1] == 'V')  //voice broadcast
               {
                
                   if (cmd[2] == 1)   // switch ptt of wireless to active 
                   {
                       Console.WriteLine("PTT 切入!");
                       Status.Set((int)StatusIndex.BUSY, true);
                       if (IOCard != null)
                       {
                           lock(IOCard)
                            IOCard.SetIO(1, 2, true);
                       }
                   }
                   else    // switch ptt to normal
                   {
                       Status.Set((int)StatusIndex.BUSY, false); ;
                       Console.WriteLine("PTT 結束!");
                       if (IOCard != null)
                       {
                           lock(IOCard)
                             IOCard.SetIO(1, 2, false);
                       }
                   }
                   if (data[0] != 0)
                       kenwood.Reply(new byte[] { data[0], (byte)'V' });

               }
               else if (cmd[1] == 'U') //Set Date Time
               {
                   Console.WriteLine("Set Date Time");
                   SetSysDateTime(2000 + cmd[2],cmd[3],cmd[4],cmd[5],cmd[6],cmd[7]);
                   if (data[0] != 0)
                       kenwood.Reply(new byte[] { data[0], (byte)'U' });
                 
               }

           }
           catch (Exception ex)
           {
               Console.WriteLine(ex.Message + "," + ex.StackTrace);
           }

          
           //throw new NotImplementedException();
       }

    
       //public void PlaySpeechTask(object args)
       //{
       //    int[] param = (int[])args;
       //    playcnt = 0;
       //    Status.Set(0, true);          //     PlayStatus = 'P';
       //    for (int i = 0; i < param[1]; i++)
       //    {
       //        playcnt++;
       //       SpeechSynthesizer voice = new SpeechSynthesizer();
       //        voice.SelectVoiceByHints(VoiceGender.Male);
       //        voice.Volume = 100;
       //        string[] voiceText = new string[]
       //    {
       //        "這是系統測試",
       //        "即將關水門請趕快離開",
       //        "即將關水門請趕快離開,緊急撤離",
       //        "即將關水門請趕快離開,緊急撤離,,緊急撤離"
       //    };
       //        voice.Rate = 0;
       //        touch_panel_mgr.ShowAlert("播放詞" + (param[0]+1)+",第"+(i+1)+"次");
       //        voice.Speak(voiceText[param[0]]);
       //        voice.Dispose();
       //    }
       //   // playcnt = 0;
       ////    PlayStatus = 'I';
       //    Status.Set(0, false);
           
       //}

       [StructLayout(LayoutKind.Sequential)]
public struct SYSTEMTIME
{
	public short wYear;
	public short wMonth;
	public short wDayOfWeek;
	public short wDay;
	public short wHour;
	public short wMinute;
	public short wSecond;
	public short wMilliseconds;
}
//Add the following extern method to your class:

[DllImport("kernel32.dll", SetLastError = true)]
public static extern bool SetSystemTime(ref SYSTEMTIME st);

       [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
internal static extern bool SetLocalTime(ref SYSTEMTIME lpSystemTime);
//Then call the method with an instance of your struct like this:


      void  SetSysDateTime(int year,int month,int day,int hour,int min,int sec)
       {
           SYSTEMTIME st = new SYSTEMTIME();
        
           st.wYear = (short)year; // must be short
           st.wMonth = (short)month;
           st.wDay = (short)day;
           st.wHour =(short)hour;
           st.wMinute = (short)min;
           st.wSecond = (short)sec;

           SetLocalTime(ref st); 
       }
// invoke this method.
    }
}
