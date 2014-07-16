using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using WirelessBrocast;

namespace wpfBroadcast
{
    /// <summary>
    /// App.xaml 的互動邏輯
    /// </summary>
    public partial class App : Application

    {

       public static tblUser loginUser;
       public static wpfBroadcast.BroadcastEntities db=new BroadcastEntities();
       public static KenWood Kenwood;
       public static string ComPort;
       public static bool InTmr = false;
        public static System.Threading.Timer tmr;
    //   static bool IsScheduleTest;
       static App()
       {
           try
           {
#if DEBUG
               return;
#endif
               if (Environment.GetCommandLineArgs().Length < 2)
                   throw new Exception("缺少通訊設置參數");
               ComPort = Environment.GetCommandLineArgs()[1].ToString().Trim();
              Kenwood = new KenWood(0, ComPort, true);

           }
           catch (Exception ex)
           {
               MessageBox.Show(ex.Message + "," + ex.StackTrace);
               Environment.Exit(-1);
           }

        tmr = new System.Threading.Timer((s) =>
               {

                   if (Kenwood == null)
                       return;
                   if (InTmr)
                       return;
                   InTmr = true;


                   lock (db)
                   {

                       CheckTestingTask();
                       var q = from n in db.tblSIte select n;
                       foreach (tblSIte site in q)
                       {


                           try
                           {
                               byte status1, status2;
                               int cnt;
                               bool success;
                               lock (App.Kenwood)
                                   success = App.Kenwood.GetPlayStatus(site.SITE_ID, out status1, out status2, out cnt);
                               System.Collections.BitArray array = new System.Collections.BitArray(new byte[] { status1, status2 });
                               site.AC = array.Get((int)StatusIndex.AC);
                               site.DC = array.Get((int)StatusIndex.DC);
                               site.DoorOpen = array.Get((int)StatusIndex.Door);
                               site.Amp = array.Get((int)StatusIndex.AMP);
                               site.Speaker = array.Get((int)StatusIndex.SPEAKER);
                               if (!success)
                                   site.Comm = true;
                               else
                                   site.Comm = false;
                               if (site.InTest && (!success ||  success  && !array.Get((int)StatusIndex.BUSY)))
                               {
                                   site.InTest = false;
                                   db.tblTestLog.AddObject(
                                       
                                         new tblTestLog(){ AC=site.AC, AMP=site.Amp, Comm=site.Comm, DateTime=DateTime.Now, 
                                              DC=site.DC, DOOR=site.DoorOpen, SITE_ID=site.SITE_ID, SPK=site.Speaker}
                                       );
                               }

                           }
                           catch (Exception ex)
                           {
                               MessageBox.Show(ex.Message);
                           }





                       }
                       try
                       {
                           db.SaveChanges();
                       }
                       catch (Exception ex)
                       {
                           MessageBox.Show(ex.Message);
                       }
                   }
                   InTmr = false;

               }


               );

           tmr.Change(0, 10 * 1000);
          
#if DEBUG
           return;
#endif
          
           
       }


       static void CheckTestingTask()
       {
           wpfBroadcast.BroadcastEntities entity = new BroadcastEntities();
           foreach (tblSchedule schd in entity.tblSchedule)
           {
               DateTime sd=new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day,schd.TimeStamp.Hour,schd.TimeStamp.Minute,0);
               DateTime? td= schd.TestDate;

               if (td != null && (td > sd || sd - td < TimeSpan.FromHours(24) && (DateTime.Now < sd || DateTime.Now - sd > TimeSpan.FromMinutes(5)))) 
                   continue;
               if (td == null && (DateTime.Now < sd  ||  DateTime.Now-sd >TimeSpan.FromMinutes(5))   )
                   continue;
                     foreach(tblSIte site in db.tblSIte)
                     {
                             
                       lock (App.Kenwood)
                          site.Comm=     !App.Kenwood.Test(site.SITE_ID,  schd.IsMute,1);

                       site.InTest = true;
                     }

                     schd.TestDate = DateTime.Now;
           }

           db.SaveChanges();
           entity.SaveChanges();
         

       }


     

    }
}
