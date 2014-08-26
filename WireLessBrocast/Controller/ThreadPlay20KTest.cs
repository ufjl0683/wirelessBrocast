using System;
using System.Collections.Generic;
//using System.Linq;
using System.Media;
using System.Text;
using System.Threading;
using WirelessBrocast;

namespace Controller
{
    public  class ThreadPlay20KTest: IThreadTask
    {
         int recordid;
       int cnt;
       System.Collections.BitArray Status;
       TouchPanelManager touch_panel_mgr;
       System.Threading.Thread WorkThread;
         int playcnt = 0;
         System.Media.SoundPlayer player = new System.Media.SoundPlayer();
         Controller controller;
         public ThreadPlay20KTest( int cnt, System.Collections.BitArray Status, TouchPanelManager touch_panel_mgr,Controller controller)
       {
           this.recordid = recordid;
           this.cnt = cnt;
           this.Status = Status;
           this.touch_panel_mgr = touch_panel_mgr;
           this.controller = controller;
          
       }

       public int PlayCnt
       {
           get
           {
               return playcnt;
           }
            
       }
       void Task()
       {
           
       

         //  Recoder record = null;
           int initsecs = 0;
           DateTime InitTime = DateTime.Now.AddHours(-1);
           //try
           //{
           // //   record = new Recoder();
           //  //  record.SetRTCNow();
           //  //  initsecs = record.GetRecordTime(InitTime);
           //}
           //catch (Exception)
           //{
           //    initsecs = 0;
           //}
           Status.Set((int)StatusIndex.BUSY, true);          //     PlayStatus = 'P';
           touch_panel_mgr.ShowAlert("靜音測試開始");
           for (int i = 0; i < cnt; i++)
           {
               playcnt++;
           
              
             //  voice.SelectVoiceByHints(VoiceGender.Male);
             //  voice.Volume = 100;
           //    string[] voiceText = new string[]
           //{
           //    "1.wav",
           //    "即將關水門請趕快離開",
           //    "即將關水門請趕快離開,緊急撤離",
           //    "即將關水門請趕快離開,緊急撤離,,緊急撤離"
           //};
              // voice.Rate = -5;
               Console.WriteLine("20khz begin");

               player = new SoundPlayer(AppDomain.CurrentDomain.BaseDirectory  + "20k.wav");
                
               player.PlaySync();

               Console.WriteLine("20khz end");
              
            //   voice.Speak(voiceText[recordid]);
             //  voice.Dispose();
           }
          // playcnt = 0;
       //    PlayStatus = 'I';
         
           int sec = 0;
           try
           {
               //System.Threading.Thread.Sleep(5000);
           //    sec = record.GetRecordTime(InitTime);
              
           //    Console.WriteLine("Record  {0} secs", sec-initsecs);
               //if (sec - initsecs > 0)
               //{
               //    Status.Set((int)StatusIndex.AMP, false);
               //    Status.Set((int)StatusIndex.SPEAKER, false);
               //}
               //else
               //{
               //    Status.Set((int)StatusIndex.AMP, true);
               //    Status.Set((int)StatusIndex.SPEAKER, true);
               //}
           }
           catch (Exception ex)
           {

              
           }
           if (controller.IOCard != null)
           {
               lock (controller.IOCard)
               {
                   byte status, status1;
                   int cnt = 0;
                   if (controller.IOCard.GetPlayStatus(1, out status, out status1, out cnt))
                   {
                       if (status1 != 0x0f)
                           controller.Status.Set((int)StatusIndex.SPEAKER, true);
                       else
                           controller.Status.Set((int)StatusIndex.SPEAKER, false);
                   }
               }
               // controller.IOCard.EnableAmpSpkTest(1);
           }
           Status.Set( (int)StatusIndex.BUSY, false);
         
           touch_panel_mgr.ShowAlert("靜音測試結束");
       }

      public void Start()
       {
           player.Stop();
           WorkThread = new Thread(Task);
           WorkThread.Start();
       }


       

       public void Abort()
       {
           player.Stop();
           WorkThread.Abort();
       }


       public void Join()
       {
           WorkThread.Join();
       }
    }
}
