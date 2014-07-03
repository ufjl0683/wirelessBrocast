using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace WirelessBrocast
{
    public class Recoder
    {
       public string uribase = "http://192.168.1.100/vlansys";

       public void SetRTCNow()
       {
           WebClient client = new WebClient();

           client.Credentials = new NetworkCredential("vlansd", "1234");
           string res = client.UploadString("http://192.168.1.100/vlansys/syscgi?SetRTC ",
                "RTCClick=" + DateTime.Now.ToString("yyyy/M/d H:m:s"));
       //    Console.WriteLine(DateTime.Now.ToString("yyyy/M/d H:m:s" + res));
       }

       public System.Collections.Generic.List<RecordInfo> GetRecordInfo(DateTime BeginTime)

       {

            
           WebClient client = new WebClient();
           client.Credentials = new NetworkCredential("vlansd", "1234");
           DateTime now=DateTime.Now;
           string param =string.Format( 
               "cDateTime=on&StartYear={0}&StartMonth={1}&StartDay={2}&StartHour={3}&StartMinute={4}&StartSecond={5}&StopYear={6}&StopMonth={7}&StopDay={8}&StopHour={9}&StopMinute={10}&StopSecond={11}&tCallerID=&tDTMF=&tRings=&tRecLength=",
               BeginTime.Year,BeginTime.Month,BeginTime.Day,BeginTime.Hour,BeginTime.Minute,BeginTime.Second,
               now.Year,now.Month,now.Day,now.Hour,now.Minute,now.Second
               );

           string res = client.UploadString("http://192.168.1.100/vlansys/vlaninquiry?",
               param);
           Regex regex = new Regex(@"RecLength=(\d+).*StartTime=(.*?),");
           MatchCollection collection = regex.Matches(res);

           System.Collections.Generic.List<RecordInfo> list = new List<RecordInfo>();
           for (int i = 0; i < collection.Count; i++)
           {
               list.Add(
                   new RecordInfo()
                   {
                       TimeStamp = DateTime.Parse(collection[i].Groups[2].Value),
                       RecordSeconds = int.Parse(collection[i].Groups[1].Value)
                   }
                   );
            //   Console.WriteLine(collection[i].Groups[2].Value);
           }
           return list;
       }

       public int GetRecordTime(DateTime DateTimebegin)
       {
           return this.GetRecordInfo(DateTimebegin).Sum(n => n.RecordSeconds);
       }

    }

    public class RecordInfo
    {
        public DateTime TimeStamp { get; set; }
        public int RecordSeconds { get; set; }

        public override string ToString()
        {
            //return base.ToString();

            return "TimeStamp:" + this.TimeStamp.ToString() + "RecordSeconds:" + RecordSeconds;
        }

       
    }

}
