﻿using System;
using System.Collections.Generic;
using System.Linq;
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
       public TouchPanelManager touch_panel_mgr;
       KenWood kenwood;
       Recoder recoder;
       
      public  System.Collections.BitArray Status = new System.Collections.BitArray(16);
      // char PlayStatus = 'I';  //idle
      IThreadTask voiceThread;
       int id;
      
       public Controller(int id, string WirelessComPort, string TouchPanelComPort)
       {   this.id=id;
       this.WirelessComPort = WirelessComPort;
       this.TouchPanelComPort = TouchPanelComPort;
       this.kenwood = new KenWood((byte)id, WirelessComPort, false);
           kenwood.OnSlaveReceiveEvent += kenwood_OnSlaveReceiveEvent;

           this.touch_panel_mgr = new TouchPanelManager(TouchPanelComPort);
           touch_panel_mgr.OnPanelCmdEvent += touch_panel_mgr_OnPanelCmdEvent;
       //    new System.Threading.Thread(new ParameterizedThreadStart(PlaySpeech)).Start(new int[] { 0, 3 });
       }

       void touch_panel_mgr_OnPanelCmdEvent(string cmd, params int[] param)
       {
           if (voiceThread != null)
           {
               voiceThread.Abort();
               voiceThread.Join();
               System.Threading.Thread.Sleep(1000);
           }
           voiceThread =   new ThreadPlaySpeech(param[0], param[1], Status, touch_panel_mgr);//new System.Threading.Thread(new ParameterizedThreadStart(PlaySpeechTask));
           voiceThread.Start(  );
       }

       void kenwood_OnSlaveReceiveEvent(object sender, byte[] data)
       {
           try
           {
               if (data[0] != this.id)
                   return;

               string cmd = System.Text.ASCIIEncoding.ASCII.GetString(data);
               if (cmd[1] == 'P') //play   p+index + times
               {
                   kenwood.Reply(new byte[] { data[0], (byte)'P', (byte)1 });
                   int inx = cmd[2];
                   int repeat = cmd[3];
                   if (voiceThread != null)
                   {
                       voiceThread.Abort();
                       voiceThread.Join();
                       System.Threading.Thread.Sleep(1000);
                   }
                   voiceThread = new ThreadPlaySpeech(inx, repeat, Status, touch_panel_mgr);  // new System.Threading.Thread(new ParameterizedThreadStart(PlaySpeechTask)  );
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
                   kenwood.Reply(new byte[] { data[0], (byte)'S', statusdata[0],statusdata[1] ,(byte)pcnt });
               }
               else if (cmd[1] == 'X')
               {
                   if (voiceThread != null)
                   {
                       voiceThread.Abort();
                   }
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


    }
}
