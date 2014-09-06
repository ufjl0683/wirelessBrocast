using Controller;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using WirelessBrocast;


namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {

            Test.BroadcastEntities db = new BroadcastEntities();

            var q= from n in db.tblUser  select n;

                   foreach(tblUser user in q)
                       Console.WriteLine(user.UserName);
            //Regex reg = new Regex("([0-9]{1,2}):([0-9]{1,2})");

            //MatchCollection match = reg.Matches("00:23");
            //if(match.Count!=0){
                
            //}

            //System.Collections.BitArray bary = new System.Collections.BitArray(8);
            //bary.Set(7,true );
            //byte[] test = new byte[1];
            //bary.CopyTo(test, 0);
        //    WirelessBrocast.Recoder Recorder = new WirelessBrocast.Recoder();

            //Console.WriteLine("total:" + Recorder.GetRecordTime(DateTime.Now.AddMinutes(-5)));
            //Recorder.SetRTCNow();
            //Console.ReadKey();
            //player = new System.Media.SoundPlayer(AppDomain.CurrentDomain.BaseDirectory + "1.wav");
            //player.Play();


            //while (true)
            //{
            //    Console.WriteLine(player.IsLoadCompleted);
            //    System.Threading.Thread.Sleep(100);
            //}
            
           // player.Play();

            //KenWood master = new KenWood(0, "Com9", true);
            //master.SetIO(1, 1, true);
            //Console.ReadKey();
            //master.SetIO(1, 1, false);
            //Console.ReadKey();
            //master.SetIO(1, 2, true);
            //Console.ReadKey();
            //master.SetIO(1, 2, false);
            //Console.ReadKey();

            //for (int i = 0; i < 5; i++)
            //{
            //    byte  status,status1;  int cnt;
            //    master.GetPlayStatus(1, out status, out status1, out cnt);
            //    Console.WriteLine("{0:X2}  {1:X2}", status, status1);
            //}


            



        //////   KenWood slave = new KenWood("Com4", false);
        //// //  slave.OnSlaveReceiveEvent += slave_OnSlaveReceiveEvent;

        ////   byte[] res;
         
            //while (true)
            //{
            //    byte status,status1; int cnt;
            //    if (master.GetPlayStatus(1, out status,out status1, out cnt))
            //    {
            //        BitArray bary = new BitArray(new byte[] { status });
            //        Console.WriteLine("Play status:{0} cnt:{1}", bary.Get(0), cnt);
            //        if (bary.Get(0) == false)
            //            break;
            //        System.Threading.Thread.Sleep(1000);
            //    }
            //}
           // PanelTest();
            //Thread th = new Thread(PlayTask);
            //th.Start();
            Console.ReadKey();
            //if (master.Play(1, 2, 5))
            //{
            //    Console.WriteLine("success");
            //}
            //while (true)
            //{
            //    char status; int cnt;
            //    if (master.GetPlayStatus(1,out status, out cnt))
            //    {
            //        Console.WriteLine("status:{0} cnt:{1}", status, cnt);
            //        if (status == 'I')
            //            break;
            //        System.Threading.Thread.Sleep(1000);
            //    }
            //}
          

        }
        static System.Media.SoundPlayer player;
        static void PlayTask()
        {
            player = new System.Media.SoundPlayer(AppDomain.CurrentDomain.BaseDirectory + "2.wav");
            for (int i = 0; i < 2; i++)
            {
                player.Play();
            }
            
            
        }
        static TouchPanel Pan;
        static void PanelTest()
        {

            Panel MainPanel = CreateMainPanel();
            Pan= new Controller.TouchPanel("Com18");
           MainPanel.OnMenuSelect += MainPanel_OnMenuSelect;
           Pan.Attatch(MainPanel);
           //Pan.ClearScreen(LayerConst.Layer1);
           //Pan.ClearScreen(LayerConst.Layer2);
           //for (int i = 1; i <7; i++)
           //    Pan.ShowString(LayerConst.Layer2, i, "hello,中華民國103年", Color.White, Color.Black);

         //  Pan.ShowString(LayerConst.Layer2, 7, "Message area!", Color.Red, Color.Black);
          
        }


        static string BrocastType;
        static int SoundId;
        static int PlayTimes;
        static Panel CreateMainPanel()
        {
          return  new Panel(
           new Menu[]{ new Menu(){ Text="1.一般廣播", MenuType= EnumMenuType.Normal },
               new Menu(){ Text="2.靜音廣播", MenuType= EnumMenuType.Normal},
                new Menu(){ Text=" 確定", MenuType= EnumMenuType.Confirm},
                  //new Menu(){ Text=" 確定", MenuType= EnumMenuType.Confirm}
                  //,
                  //  new Menu(){ Text=" 確定", MenuType= EnumMenuType.Confirm}
               
               });
        }
        static Panel CreateMediaSelectPanel()
        {
          return   new Panel(
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

        static void MainPanel_OnMenuSelect(int menuid)
        {
            Panel MediaSelectPanel=null;
            if (menuid == 2)
            {
                MediaSelectPanel = CreateMediaSelectPanel();

                Pan.CurrentPanel.OnMenuSelect -= MainPanel_OnMenuSelect;
                MediaSelectPanel.OnMenuSelect += MediaSelectPanel_OnMenuSelect;
            }
            else if (menuid == 0)
                BrocastType = "Normal";
            else if (menuid == 1)
                BrocastType = "Silence";
          
            if(MediaSelectPanel!=null)
            Pan.Attatch(MediaSelectPanel);
            //throw new NotImplementedException();
        }

        static void MediaSelectPanel_OnMenuSelect(int menuid)
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
                    Pan.CurrentPanel.OnMenuSelect -= MediaSelectPanel_OnMenuSelect;
                    TimesSelectPanel = CreateTimesSelectPanel();
                    TimesSelectPanel.OnMenuSelect += TimesSelectPanel_OnMenuSelect;
                    Pan.Attatch(TimesSelectPanel);
                    break;
                case 5:
                    Pan.CurrentPanel.OnMenuSelect -= MediaSelectPanel_OnMenuSelect;
                    Panel  panel = CreateMainPanel();
                    panel.OnMenuSelect += MainPanel_OnMenuSelect;
                    Pan.Attatch(panel);
                    break;
            }
        }

        static void TimesSelectPanel_OnMenuSelect(int menuid)
        {
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
                    break;
                case 5:
                    Pan.CurrentPanel.OnMenuSelect -= MediaSelectPanel_OnMenuSelect;
                    Panel panel = CreateMainPanel();
                    panel.OnMenuSelect += MainPanel_OnMenuSelect;
                    Pan.Attatch(panel);
                    break;
            }

            //throw new NotImplementedException();
        }
        static void slave_OnSlaveReceiveEvent(object sender, byte[] data)
        {

            Console.WriteLine(System.Text.ASCIIEncoding.ASCII.GetString(data));
            (sender as KenWood).Reply(data);
            //throw new NotImplementedException();
        }
    }
}
