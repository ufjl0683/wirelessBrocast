using System;
using System.Collections.Generic;
using System.Media;
//using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading;
using WirelessBrocast;

namespace Controller
{
   public  class ThreadPlaySpeechTest:IThreadTask
    {
       int recordid;
       int cnt;
       System.Collections.BitArray Status;
       TouchPanelManager touch_panel_mgr;
       System.Threading.Thread WorkThread;
       Controller controller;
       int playcnt = 0;
       System.Media.SoundPlayer player;
       bool IsAbort = false;
       public ThreadPlaySpeechTest(int recordid, int cnt, System.Collections.BitArray Status, TouchPanelManager touch_panel_mgr,Controller controller)
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

           bool IsDetect = false;
           
          // int[] param = (int[])args;
        //   playcnt = 0;
           //  Recoder record=null;
           //  int initsecs = 0;
           //  DateTime InitTime = DateTime.Now.AddHours(-1);
           //try
           //{
           //  record = new Recoder();
           //  record.SetRTCNow();
           //  initsecs = record.GetRecordTime(InitTime);
           //}
           //catch(Exception)
           //{
           //    initsecs = 0;
           //}
           touch_panel_mgr.ShowAlert("播音測試開始");
           controller.SpeakerOut = 0;
           Status.Set((int)StatusIndex.BUSY, true);          //     PlayStatus = 'P';
           for (int i = 0; i < cnt; i++)
           {
               playcnt++;

                 player = new SoundPlayer(AppDomain.CurrentDomain.BaseDirectory+"sound\\" + "TEST.wav");

               player.PlaySync();
               if (IsAbort)
               {
                   touch_panel_mgr.ShowAlert("中止");
                   return;
               }
           //   SpeechSynthesizer voice = new SpeechSynthesizer();
           //    voice.SelectVoiceByHints(VoiceGender.Male);
           //    voice.Volume = 100;
           //    string[] voiceText = new string[]
           //{
           //    "這是系統測試",
           //    "即將關水門請趕快離開",
           //    "即將關水門請趕快離開,緊急撤離",
           //    "即將關水門請趕快離開,緊急撤離,,緊急撤離"
           //};
           //    voice.Rate = -5;
           //    touch_panel_mgr.ShowAlert("播放詞" + (recordid+1)+",第"+(i+1)+"次");
           //    voice.Speak(voiceText[recordid]);
           //    voice.Dispose();
           }
          // playcnt = 0;
       //    PlayStatus = 'I';

          int sec = 0;
           //try
           //{
           //    //if (record != null)
           //    //{
           //    //    Status.Set((int)StatusIndex.BUSY, false);
           //    //    System.Threading.Thread.Sleep(5000);
           //    //    sec = record.GetRecordTime(InitTime);

           //    //    Console.WriteLine("Record  {0} secs", sec-initsecs);
           //    //    if (sec - initsecs > 0)
           //    //    {
           //    //        Status.Set((int)StatusIndex.AMP, false);
           //    //        Status.Set((int)StatusIndex.SPEAKER, false);
           //    //    }
           //    //    else
           //    //    {
           //    //        Status.Set((int)StatusIndex.AMP, true);
           //    //        Status.Set((int)StatusIndex.SPEAKER, true);
           //    //    }
           //    }

           //}
           //catch (Exception ex)
           //{


           //}

          if (controller.SpeakerOut != 0x0f)

              controller.Status.Set((int)StatusIndex.SPEAKER, true);
          else
              controller.Status.Set((int)StatusIndex.SPEAKER, false);
          //if (controller.IOCard != null)
          //{
          //    lock (controller.IOCard)
          //    {
          //      //  controller.IOCard.EnableAmpSpkTest(1);
          //        byte status, status1;
          //        int cnt = 0;
          //        if (controller.IOCard.GetPlayStatus(1, out status, out status1, out cnt))
          //        {
          //            if (status1 != 0x0f)
          //                controller.Status.Set((int)StatusIndex.SPEAKER, true);
          //            else
          //                controller.Status.Set((int)StatusIndex.SPEAKER, false);
          //        }
          //    }
          //}
           Status.Set( (int)StatusIndex.BUSY, false);
           touch_panel_mgr.ShowAlert("播音測試結束");
        
       }

      public void Start()
       {
           WorkThread = new Thread(Task);
           WorkThread.Start();
       }


       

       public void Abort()
       {
           IsAbort = true;
           player.Stop();
           WorkThread.Abort();
       }


       public void Join()
       {
           WorkThread.Join();
       }


      
    }
}
