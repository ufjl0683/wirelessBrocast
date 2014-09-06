using System;
using System.Collections.Generic;
//using System.Linq;
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
       Controller controller;
         int playcnt = 0;
         System.Media.SoundPlayer player = new System.Media.SoundPlayer();
         bool IsAbort = false;
         public ThreadPlaySound(int recordid, int cnt, System.Collections.BitArray Status, TouchPanelManager touch_panel_mgr,Controller controller)
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
           Status.Set((int)StatusIndex.BUSY, true);

           controller.SpeakerOut = 0;
           //     PlayStatus = 'P';
           //if (!System.IO.File.Exists(AppDomain.CurrentDomain.BaseDirectory + "sound\\" + recordid + ".wav"))
               
           //        touch_panel_mgr.ShowAlert(recordid + ".wav 不存在！");

           for (int i = 0; i < cnt; i++)
           {
               playcnt++;
           
            
              
                   player = new SoundPlayer(AppDomain.CurrentDomain.BaseDirectory + "sound\\"+recordid + "-C.wav");
                   player.PlaySync();


                   if (IsAbort)
                   {
                       touch_panel_mgr.ShowAlert("中止");
                       break;
                   }
                   player = new SoundPlayer(AppDomain.CurrentDomain.BaseDirectory + "sound\\" + recordid + "-T.wav");
                   player.PlaySync();

                   if (IsAbort)
                   {
                       touch_panel_mgr.ShowAlert("中止");
                       break;
                   }

                   player = new SoundPlayer(AppDomain.CurrentDomain.BaseDirectory + "sound\\" + recordid + "-K.wav");
                   player.PlaySync();

                   if (IsAbort)
                   {
                       touch_panel_mgr.ShowAlert("中止");
                       break;
                   }

                   player = new SoundPlayer(AppDomain.CurrentDomain.BaseDirectory + "sound\\" + recordid + "-E.wav");
                   player.PlaySync();
                   touch_panel_mgr.ShowAlert("播放詞" + (recordid + 1) + ",第" + (i + 1) + "次");

                   if (IsAbort)
                   {
                       touch_panel_mgr.ShowAlert("中止");
                       break;
                   }

                  
           }
          // playcnt = 0;
       //    PlayStatus = 'I';
           if (controller.SpeakerOut != 0x0f)

               controller.Status.Set((int)StatusIndex.SPEAKER, true);
           else
               controller.Status.Set((int)StatusIndex.SPEAKER, false);
           //if (this.controller.IOCard != null)
           //{
           //    //this.controller.IOCard.EnableAmpSpkTest(1);
           //    byte status, status1;
           //    int cnt = 0;
           //    if (controller.IOCard.GetPlayStatus(1, out status, out status1, out cnt))
           //    {
           //        if (status1 != 0x0f)
           //            controller.Status.Set((int)StatusIndex.SPEAKER, true);
           //        else
           //            controller.Status.Set((int)StatusIndex.SPEAKER, false);
           //    }
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
           //player.Stop();
           IsAbort = true;
           player.Dispose();
           WorkThread.Abort();
       }


       public void Join()
       {
           WorkThread.Join();
       }


    }
}
