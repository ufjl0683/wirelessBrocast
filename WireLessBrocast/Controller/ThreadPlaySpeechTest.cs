using System;
using System.Collections.Generic;
using System.Linq;
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
         int playcnt = 0;
       public ThreadPlaySpeechTest(int recordid, int cnt, System.Collections.BitArray Status, TouchPanelManager touch_panel_mgr)
       {
           this.recordid = recordid;
           this.cnt = cnt;
           this.Status = Status;
           this.touch_panel_mgr = touch_panel_mgr;
          
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
         
           Status.Set((int)StatusIndex.BUSY, true);          //     PlayStatus = 'P';
           for (int i = 0; i < cnt; i++)
           {
               playcnt++;
              SpeechSynthesizer voice = new SpeechSynthesizer();
               voice.SelectVoiceByHints(VoiceGender.Male);
               voice.Volume = 100;
               string[] voiceText = new string[]
           {
               "這是系統測試",
               "即將關水門請趕快離開",
               "即將關水門請趕快離開,緊急撤離",
               "即將關水門請趕快離開,緊急撤離,,緊急撤離"
           };
               voice.Rate = -5;
               touch_panel_mgr.ShowAlert("播放詞" + (recordid+1)+",第"+(i+1)+"次");
               voice.Speak(voiceText[recordid]);
               voice.Dispose();
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
           Status.Set( (int)StatusIndex.BUSY, false);
           
        
       }

      public void Start()
       {
           WorkThread = new Thread(Task);
           WorkThread.Start();
       }


       

       public void Abort()
       {
           WorkThread.Abort();
       }


       public void Join()
       {
           WorkThread.Join();
       }


      
    }
}
