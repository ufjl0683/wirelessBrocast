using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WirelessBrocast;

namespace Controller
{

    public delegate void PanelCommandEventHandler(string cmd,params int[]param);
  public   class TouchPanelManager
    {

      public event PanelCommandEventHandler OnPanelCmdEvent;
       string ComPort;
       TouchPanel touchPanel;
         string BrocastType;
        int SoundId;
        int PlayTimes;
        int LastMenuId;
      public   TouchPanelManager(string ComPort)
      {
          this.ComPort = ComPort;
          touchPanel = new TouchPanel(ComPort);
          InitialPanel();
      }


         public void  ShowAlert(string message)
         {
             this.touchPanel.Alert(message);
         }
        void InitialPanel()
      {

          Panel MainPanel = CreateMainPanel();

          MainPanel.OnMenuSelect += MainPanel_OnMenuSelect;
          touchPanel.Attatch(MainPanel);
          //Pan.ClearScreen(LayerConst.Layer1);
          //Pan.ClearScreen(LayerConst.Layer2);
          //for (int i = 1; i <7; i++)
          //    Pan.ShowString(LayerConst.Layer2, i, "hello,中華民國103年", Color.White, Color.Black);

          //  Pan.ShowString(LayerConst.Layer2, 7, "Message area!", Color.Red, Color.Black);

      }


        Panel CreateMainPanel()
      {
          return new Panel(
           new Menu[]{ new Menu(){ Text="1.預錄詞廣播", MenuType= EnumMenuType.Normal },
               new Menu(){ Text="2.靜音測試", MenuType= EnumMenuType.Normal},
               new Menu(){Text="3.系統狀態",MenuType= EnumMenuType.Normal},
                new Menu(){ Text=" 確定", MenuType= EnumMenuType.Confirm},
                  //new Menu(){ Text=" 確定", MenuType= EnumMenuType.Confirm}
                  //,
                  //  new Menu(){ Text=" 確定", MenuType= EnumMenuType.Confirm}
               
               });
      }
        Panel CreateMediaSelectPanel()
      {
          return new Panel(
                    new Menu[]{ new Menu() { MenuType = EnumMenuType.Normal, Text = "1.廣播詞 1" },
                                  new Menu() { MenuType = EnumMenuType.Normal, Text = "2.廣播詞 2" },
                                  new Menu() { MenuType = EnumMenuType.Normal, Text = "3.廣播詞 3" },
                                  new Menu() { MenuType = EnumMenuType.Normal, Text = "4.廣播詞 4" },
                                   new Menu() { MenuType = EnumMenuType.Confirm, Text = "確定" },
                                     new Menu() { MenuType = EnumMenuType.Cancel, Text = "取消" },
                              });
      }

      static Panel CreateTimesSelectPanel()
      {

          return new Panel(
                  new Menu[]{ new Menu() { MenuType = EnumMenuType.Normal, Text = "1. X 1 次" },
                                  new Menu() { MenuType = EnumMenuType.Normal, Text = "2. X 3 次" },
                                  new Menu() { MenuType = EnumMenuType.Normal, Text = "3. X 5 次" },
                                  new Menu() { MenuType = EnumMenuType.Normal, Text = "4. X 10 次" },
                                   new Menu() { MenuType = EnumMenuType.Confirm, Text = "確定" },
                                   new Menu() { MenuType = EnumMenuType.Cancel, Text = "取消" },
                              });
      }

        void MainPanel_OnMenuSelect(int menuid)
      {
          Panel MediaSelectPanel = null;
          if (menuid == 3)
          {
              if (LastMenuId == 2)
                  return;
              MediaSelectPanel = CreateMediaSelectPanel();

              touchPanel.CurrentPanel.OnMenuSelect -= MainPanel_OnMenuSelect;
              MediaSelectPanel.OnMenuSelect += MediaSelectPanel_OnMenuSelect;
          }
          else if (menuid == 0)
              BrocastType = "Normal";
          else if (menuid == 1)
              BrocastType = "Silence";
          else if (menuid == 2)  // show status  play  testing  door   power1  power2   amp/speaker
          {
              string door, ac, dc, amp, speaker;
              door = (Program.controller.Status.Get((int)StatusIndex.Door)) ? "開" : "關";
              ac = (!Program.controller.Status.Get((int)StatusIndex.AC) )? "正常" : "故障";
              dc = (!Program.controller.Status.Get((int)StatusIndex.DC)) ? "正常" : "故障";
              amp = (!Program.controller.Status.Get((int)StatusIndex.AMP)) ? "正常" : "故障";
              speaker = (!Program.controller.Status.Get((int)StatusIndex.SPEAKER)) ? "正常" : "故障";
              string s = string.Format("箱門:{0} 交流:{1} 直流:{2} 擴大機:{3} 喇吧:{4}",door,ac,dc,amp,speaker);
              touchPanel.Alert(s);
              LastMenuId = menuid;
              return;
          }
          LastMenuId = menuid;
          if (MediaSelectPanel != null)
              touchPanel.Attatch(MediaSelectPanel);
          //throw new NotImplementedException();
      }

        void MediaSelectPanel_OnMenuSelect(int menuid)
      {
          Panel TimesSelectPanel;
          switch (menuid)
          {
              case 0:
                  SoundId = 0;
                  // TimesSelectPanel=  CreateTimesSelectPanel();
                  break;
              case 1:
                  SoundId = 1;
                  //  TimesSelectPanel = CreateTimesSelectPanel();
                  break;
              case 2:
                  SoundId = 2;
                  //   TimesSelectPanel = CreateTimesSelectPanel();
                  break;
              case 3:
                  SoundId = 3;
                  //   TimesSelectPanel = CreateTimesSelectPanel();
                  break;
              case 4:   //Confirm
                  touchPanel.CurrentPanel.OnMenuSelect -= MediaSelectPanel_OnMenuSelect;
                  TimesSelectPanel = CreateTimesSelectPanel();
                  TimesSelectPanel.OnMenuSelect += TimesSelectPanel_OnMenuSelect;
                  touchPanel.Attatch(TimesSelectPanel);
                  break;
              case 5:
                  touchPanel.CurrentPanel.OnMenuSelect -= MediaSelectPanel_OnMenuSelect;
                  Panel panel = CreateMainPanel();
                  panel.OnMenuSelect += MainPanel_OnMenuSelect;
                  touchPanel.Attatch(panel);
                  break;
          }
      }

        void TimesSelectPanel_OnMenuSelect(int menuid)
      {
          Panel panel;
          switch (menuid)
          {
              case 0:
                  PlayTimes = 1;
                  // TimesSelectPanel=  CreateTimesSelectPanel();
                  break;
              case 1:
                  PlayTimes = 3;
                  //  TimesSelectPanel = CreateTimesSelectPanel();
                  break;
              case 2:
                  PlayTimes = 5;
                  //   TimesSelectPanel = CreateTimesSelectPanel();
                  break;
              case 3:
                  PlayTimes = 10;
                  //   TimesSelectPanel = CreateTimesSelectPanel();
                  break;
              case 4:   //Confirm
                    touchPanel.CurrentPanel.OnMenuSelect -= MediaSelectPanel_OnMenuSelect;
                    panel = CreateMainPanel();
                  panel.OnMenuSelect += MainPanel_OnMenuSelect;
                  touchPanel.Attatch(panel);
                  if (this.OnPanelCmdEvent != null)
                  {
                      this.OnPanelCmdEvent(BrocastType, SoundId, PlayTimes);
                  }
                  break;
              case 5:
                  touchPanel.CurrentPanel.OnMenuSelect -= MediaSelectPanel_OnMenuSelect;
                    panel = CreateMainPanel();
                  panel.OnMenuSelect += MainPanel_OnMenuSelect;
                  touchPanel.Attatch(panel);
                  break;
          }

          //throw new NotImplementedException();
      }
      
    }
}
