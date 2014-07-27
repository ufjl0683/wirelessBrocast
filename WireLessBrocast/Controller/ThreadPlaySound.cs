using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;
using WirelessBrocast;

namespace Controller
{
    public class ThreadPlaySound : IThreadTask
    {
        int recordid;
       int cnt;
       System.Collections.BitArray Status;
       TouchPanelManager touch_panel_mgr;
       System.Threading.Thread WorkThread;
         int playcnt = 0;
         System.Media.SoundPlayer player = new System.Media.SoundPlayer();
         public ThreadPlaySound(int recordid, int cnt, System.Collections.BitArray Status, TouchPanelManager touch_panel_mgr)
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
            if (!System.IO.File.Exists(AppDomain.CurrentDomain.BaseDirectory + recordid + ".wav"))
               
                   touch_panel_mgr.ShowAlert(recordid + ".wav 不存在！");

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
              
                   player = new SoundPlayer(AppDomain.CurrentDomain.BaseDirectory + recordid + ".wav");
                   player.PlaySync();
                   touch_panel_mgr.ShowAlert("播放詞" + (recordid + 1) + ",第" + (i + 1) + "次");
               
              
              
            //   voice.Speak(voiceText[recordid]);
             //  voice.Dispose();
           }
          // playcnt = 0;
       //    PlayStatus = 'I';
         
         
           Status.Set( (int)StatusIndex.BUSY, false);
           
        
       }

      public void Start()
       {
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
