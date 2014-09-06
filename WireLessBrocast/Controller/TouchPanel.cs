using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Ports;
//using System.Linq;
using System.Text;
using System.Threading;

namespace Controller
{

    public enum LayerConst
    {
        Layer1,
        Layer2
    }

    public class ReturnData
    {
        public byte cmd { get; set; }
        public byte state { get; set; }
        public byte reference { get; set; } 
    }

    public class DisplayJob
    {
        public LayerConst layer{get;set;}
        public int lineno {get;set;}
        public string Text{get;set;}
        public string JobType { get; set; }
    }

    public delegate void TouchEventHandle(int x,int y);
   public  class TouchPanel
    {
       public Queue<int[]> ReceiveCmdQueue = new Queue<int[]>();
       public Queue<byte[]> SendQueue = new Queue<byte[]>();
       private event TouchEventHandle OnTouchEvent;
       public  IPanel CurrentPanel;
       public const byte START_BYTE = 0x5a;
       public const byte END_BYTE = 0x69;
       public static   int FontSize = 32;
       public static int FontType = 01;
       public string ComPort;
       SerialPort serialPort;
       Thread thRecevieTask;
       ReturnData retdata;

     
       public TouchPanel(string ComPort)
       {

           this.ComPort = ComPort;
           serialPort = new SerialPort(ComPort, 115200, Parity.None, 8, StopBits.One);
           serialPort.Open();
           int cnt = serialPort.BytesToRead;
           for (int i = 0; i < cnt; i++)
               serialPort.ReadByte();

           EnableTouchPanel(true);
           thRecevieTask = new System.Threading.Thread(ReceiveTask);
           thRecevieTask.Start();
           new System.Threading.Thread(this.SendTask).Start();
           new System.Threading.Thread(ReceiveCmdTask).Start();
           this.OnTouchEvent += TouchPanel_OnTouchEvent;

           init_Panl();
        
       }


       void ReceiveCmdTask()
       {
           while (true)
           {
               lock (this.ReceiveCmdQueue)
               {
                   if (ReceiveCmdQueue.Count == 0)
                       System.Threading.Monitor.Wait(ReceiveCmdQueue);
                   while (ReceiveCmdQueue.Count > 0)
                   {
                       int[] data = ReceiveCmdQueue.Dequeue();
                       if (this.OnTouchEvent != null)
                           this.OnTouchEvent(data[0],data[1]);
                       //switch (job.JobType)
                       //{
                       //    case "A": //alert
                       //        Alert(job.Text);
                       //        break;
                       //    case "S":  //select text
                       //        TextSelect(job.layer, job.lineno, job.Text);
                       //        break;

                       //    case  "U": //unselect
                       //        TextUnSelect(job.layer, job.lineno, job.Text);
                       //        break;
                       //}
                   }
               }
           }
       }
       void SendTask()
       {

           while (true)
           {

               lock (SendQueue)
               {
                   if (SendQueue.Count == 0)
                       System.Threading.Monitor.Wait(SendQueue);
               }
                   while (true)
                   {
                       byte[] data;
                       lock(SendQueue)
                       {
                           if(SendQueue.Count==0)
                               break;
                           data=   SendQueue.Dequeue();
                       }
                      
                       lock (this)
                       {
                           serialPort.Write(data, 0, data.Length);

                           System.Threading.Monitor.Wait(this, 1000);
                       }
                   }
                
               }
           }
       
       void TouchPanel_OnTouchEvent(int x, int y)
       {
            Menu lastmenu ;
           if (CurrentPanel == null)
               return;
           int lastMenuid = this.CurrentPanel.LastHitMenuID;
           if(lastMenuid!=-1)
                  lastmenu = CurrentPanel.GetMenu(lastMenuid);

           int menuid = this.CurrentPanel.HitTest(x, y-FontSize);
           if (menuid == -1|| menuid>=CurrentPanel.GetMenuCount() || lastMenuid==menuid)
               return;
           DisplayJob job;
           bool CanInvoke = true;
          
              // this.TextSelect(LayerConst.Layer2, menuid + 1, CurrentPanel.GetMenuText(menuid));
               if (lastMenuid == -1 && CurrentPanel.GetMenu(menuid).MenuType == EnumMenuType.Confirm)
               {
                   //  DisplayCmdQueue.Enqueue(new DisplayJob() { JobType = "A", Text = "請先選取項目", lineno = 7, layer = LayerConst.Layer2 });
                   Alert("請先選取項目");
                  
                   CanInvoke = false;
               }
               else
               {
                //   DisplayCmdQueue.Enqueue(new DisplayJob() { JobType = "A", Text = "".PadLeft(FontSize, ' '), lineno = 7, layer = LayerConst.Layer2 });
                  
                   Alert("".PadLeft(100, ' '));
               }
               if (this.CurrentPanel.GetMenu(menuid).MenuType == EnumMenuType.Normal)
               {

                   //  job = new DisplayJob() { layer = LayerConst.Layer2, lineno = menuid + 1, Text = CurrentPanel.GetMenuText(menuid), JobType = "S" };
                   //  DisplayCmdQueue.Enqueue(job);
                   TextSelect(LayerConst.Layer2, menuid + 1, CurrentPanel.GetMenuText(menuid));

                   if (lastMenuid != -1)
                   {
                       //job = new DisplayJob() { layer = LayerConst.Layer2, lineno = lastMenuid + 1, Text = CurrentPanel.GetMenuText(lastMenuid), JobType = "U" };
                       TextUnSelect(LayerConst.Layer2, lastMenuid + 1, CurrentPanel.GetMenuText(lastMenuid));
                       //  DisplayCmdQueue.Enqueue(job);
                   }
                   //}

               }
               else
               {
                   if(lastMenuid!=-1)
                   TextUnSelect(LayerConst.Layer2, lastMenuid + 1, CurrentPanel.GetMenuText(lastMenuid));
                   Beep();
               }
        //       System.Threading.Monitor.Pulse(DisplayCmdQueue);
        
       
           if(CanInvoke)
                 CurrentPanel.InvokeSelectEvent(menuid);
           
       }

     
       void init_Panl()
       {
           this.EnableTouchPanel(true);
           this.ClearScreen(LayerConst.Layer2);
           this.ClearScreen(LayerConst.Layer1);

     
       
           System.Threading.Timer tmr = new System.Threading.Timer((a) =>
           {

               this.ShowString(LayerConst.Layer2, 0,
                   string.Format("{0}年{1:00}月{2:00}日{3:00}時{4:00}分{5:00}秒", DateTime.Now.Year - 1911, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second).PadLeft(28),

                   Color.Yellow, Color.Black);

           }
          );
           tmr.Change(0, 1000);
       }

       public void EnableTouchPanel(bool enable)
       {
           byte[] comdata;
           if (enable)
               comdata = new byte[] { 0x5A, 0x04, 0x20, 0xFF, 0xDB, 0x69 };
           else
               comdata = new byte[] { 0x5A, 0x04, 0x20, 0x00, 0x24, 0x69 };

           lock (this)
               serialPort.Write(comdata, 0, comdata.Length);
       }

       DateTime LastTouchTime;
       void ReceiveTask()
       {

           Stream stream = serialPort.BaseStream;
           byte cmd, state, reference;
           int data;
           while (true)
           {
               try
               {
                   while ((data = stream.ReadByte()) != 0x5a)

                       continue;
#if DEBUG
                   Console.Write("{0:X2} ", data);
#endif
                   cmd = (byte)stream.ReadByte();
                   if (cmd == 0x21)
                   {
                       byte xh, xl, yh, yl;
                       xh = (byte)stream.ReadByte();
                       xl = (byte)stream.ReadByte();
                       yh = (byte)stream.ReadByte();
                       yl = (byte)stream.ReadByte();
                       stream.ReadByte();  //cks
                       stream.ReadByte();  //0x69
                       int x = xh * 256 + xl;
                       int y = yh * 256 + yl;

                       lock (ReceiveCmdQueue)
                       {
#if DEBUG
                           Console.WriteLine("x:{0},y:{1}", x, y);
#endif
                           if (!(LastTouchTime != null && DateTime.Now.Subtract(LastTouchTime) < TimeSpan.FromSeconds(0.5)))
                           {
                               ReceiveCmdQueue.Enqueue(new int[] { x, y });
                               System.Threading.Monitor.Pulse(ReceiveCmdQueue);
                           }
                           LastTouchTime = DateTime.Now;
                         
                           
                          
                       }


                       //if (this.OnTouchEvent != null)
                       //    this.OnTouchEvent(x, y);
                       continue;
                   }

#if DEBUG
                   Console.Write("{0:X2} ", cmd);
#endif
                   state = (byte)stream.ReadByte();
#if DEBUG
                   Console.Write("{0:X2} ", state);
#endif
                   reference = (byte)stream.ReadByte();
#if DEBUG
                   Console.Write("{0:X2} ", reference);
                   Console.WriteLine("{0:X2}", stream.ReadByte());
#endif
                   retdata = new ReturnData() { cmd = cmd, reference = reference, state = state };
                   lock (this)
                   {
                       System.Threading.Monitor.Pulse(this);
                   }
               }
               catch (Exception ex)
               {
                   Console.WriteLine(ex.Message + "," + ex.StackTrace);
               }
           }
       }
     
       public void Send(byte[]comdata)
       {

           lock (this.SendQueue)
           {
               SendQueue.Enqueue(comdata);
               System.Threading.Monitor.Pulse(SendQueue);
           }
         //  this.serialPort.Write(comdata, 0, comdata.Length);
         //  System.Threading.Monitor.Wait(this, 1000);
           
           
       }

       public void ClearScreen(LayerConst layer)
       {
           byte[] data;
           if (layer == LayerConst.Layer1)
           {
               data = new byte[] { 0x5A, 0x04, 0x01, 0x00, 0x05, 0x69 };
           }
           else
           {
               data = new byte[] { 0x5A, 0x04, 0x01, 0x01, 0x04, 0x69 };
           }

             Send(data);
           //if (ret.reference != 0)
           //    throw new Exception("return data error");
          
       }

       public void TextSelect(LayerConst layer,int lineno, string text)
       {
           string pad = "".PadRight(64, ' ');
           ShowString(layer, lineno, pad, Color.Black, Color.White);
          ShowString(layer, lineno, text, Color.Black, Color.White);
           Beep();
         //
       }

      public  void Alert(string message)
       {
           ShowSmallString(LayerConst.Layer1, 8, message, Color.Yellow, Color.Black);
       }
       public void TextUnSelect(LayerConst layer, int lineno, string text)
       {
           string pad = "".PadRight(64, ' ');
           ShowString(layer, lineno, pad, Color.White, Color.Black);
           ShowString(layer, lineno, text, Color.White, Color.Black);
       }
       public void Beep()
       {
           byte[] comdata=new byte[]{0x5A, 0x06, 0x38, 0x0A, 0x0A, 0x01, 0x3F, 0x69 };
           Send(comdata);

       }
       //public void Beep(int beepcnt)
       //{
       //    byte[] comdata = new byte[] { 0x5A, 0x06, 0x38, 0x0A, 0x0A, (byte)beepcnt, 0x3F, 0x69 };
       //    Send(comdata);

       //}

       public void ShowSmallString(LayerConst layer, int lineno, string text, Color fcolor, Color bcolor)
       {
           System.IO.MemoryStream ms = new System.IO.MemoryStream();
           byte len;
           byte cmd = 0x09;
           byte fonttype = 0x00; //FontSizexFontSize
           byte yh = (byte)(lineno * FontSize / 256);
           byte yl = (byte)(lineno * FontSize % 256);
           byte fcolorh, fcolorl, bcolorh, bcolorl;
           fcolorh = (byte)((fcolor.R & 0xf8) | (fcolor.G / 32));
           fcolorl = (byte)(((fcolor.G / 8 & 0x07) << 5) | fcolor.B / 8);
           bcolorh = (byte)((bcolor.R & 0xf8) | (bcolor.G / 32));
           bcolorl = (byte)(((bcolor.G / 8 & 0x07) << 5) | bcolor.B / 8);
           byte xh = (byte)((layer == LayerConst.Layer1) ? 0x80 : 0x40);
           byte xl = 0;
           byte[] code = System.Text.Encoding.Unicode.GetBytes(text);
           byte cks = 0; ;
           len = (byte)(code.Length + 10 + 2);
           ms.WriteByte(0x5a);
           ms.WriteByte(len);
           cks ^= len;
           ms.WriteByte(cmd);
           cks ^= cmd;
           ms.WriteByte(fonttype);
           cks ^= fonttype;
           ms.WriteByte(xh);
           cks ^= xh;
           ms.WriteByte(xl);
           cks ^= xl;
           ms.WriteByte(yh);
           cks ^= yh;
           ms.WriteByte(yl);
           cks ^= yl;
           ms.WriteByte(fcolorh);
           cks ^= fcolorh;
           ms.WriteByte(fcolorl);
           cks ^= fcolorl;
           ms.WriteByte(bcolorh);
           cks ^= bcolorh;
           ms.WriteByte(bcolorl);
           cks ^= bcolorl;
           for (int i = 0; i < code.Length / 2; i++)
           {
               ms.WriteByte(code[i * 2 + 1]);
               cks ^= code[i * 2 + 1];
               ms.WriteByte(code[i * 2]);
               cks ^= code[i * 2];
           }

           ms.WriteByte(cks);
           ms.WriteByte(0x69);

           byte[] data = ms.ToArray();
           Send(data);
       }
       public void ShowString(LayerConst layer, int lineno,string text,Color fcolor,Color bcolor)
       {
         //  0x5A,Length>=12,0x07,FontType,(Layer+x_H),x_L,y_H,y_L,Fcolor H,FcolorL,BcolorH,BcolorL, Uni-code, checksum,0x69 0x5A,Length>=12,0x07,FontType,(Layer+x_H),x_L,y_H,y_L,Fcolor H,FcolorL,BcolorH,BcolorL, Uni-code, checksum,0x69 
           System.IO.MemoryStream ms = new System.IO.MemoryStream();
           byte len;
           byte cmd = 0x09;
           byte fonttype = (byte)FontType; //FontSizexFontSize
           byte yh = (byte)(lineno * FontSize/256);
           byte yl =(byte)( lineno * FontSize % 256);
           byte fcolorh, fcolorl,bcolorh,bcolorl;
           fcolorh =(byte) ((fcolor.R & 0xf8)|(fcolor.G/32));
           fcolorl = (byte)(((fcolor.G / 8 & 0x07) << 5) | fcolor.B / 8);
           bcolorh = (byte)((bcolor.R & 0xf8) | (bcolor.G / 32));
           bcolorl = (byte)(((bcolor.G / 8 & 0x07) << 5) | bcolor.B / 8);
           byte xh = (byte)((layer== LayerConst.Layer1)?0x80:0x40);
           byte xl=0;
           byte[] code = System.Text.Encoding.Unicode.GetBytes(text);
           byte cks = 0; ;
           len =(byte)( code.Length + 10 + 2);
           ms.WriteByte(0x5a);
           ms.WriteByte(len);
           cks^=len;
           ms.WriteByte(cmd);
           cks ^= cmd;
           ms.WriteByte(fonttype);
           cks ^= fonttype;
           ms.WriteByte(xh);
           cks ^= xh;
           ms.WriteByte(xl);
           cks ^= xl;
           ms.WriteByte(yh);
           cks ^= yh;
           ms.WriteByte(yl);
           cks ^= yl;
           ms.WriteByte(fcolorh);
           cks ^= fcolorh;
           ms.WriteByte(fcolorl);
           cks ^= fcolorl;
           ms.WriteByte(bcolorh);
           cks ^= bcolorh;
           ms.WriteByte(bcolorl);
           cks ^= bcolorl;
           for (int i = 0; i < code.Length/2; i++)
           {
               ms.WriteByte(code[i*2+1]);
               cks^=code[i*2+1];
               ms.WriteByte(code[i*2 ]);
               cks ^= code[i*2];
           }

           ms.WriteByte(cks);
           ms.WriteByte(0x69);

           byte[] data = ms.ToArray();
             Send(data);
          //if (ret.reference != 0)
          //    throw new Exception("return data error");
       //    byte[] ret = new byte[5];
         //  serialPort.Write(data,0,data.Length);
           //for (int i = 0; i < 5; i++)
           //    ret[i] =(byte) serialPort.ReadByte();
       }

       public void Attatch(IPanel i_panel)
       {
        //   this.ClearScreen(LayerConst.Layer1);
          // this.ClearScreen(LayerConst.Layer2);
           for (int i = 1; i <= 7; i++)
               ShowString(LayerConst.Layer2 , i, "".PadLeft(64, ' '), Color.White, Color.Black);
           this.CurrentPanel = i_panel;
           i_panel.IsActive = true;
           for (int i = 0; i < i_panel.GetMenuCount(); i++)
           {
               this.ShowString(LayerConst.Layer2, i + 1, i_panel.GetMenuText(i), Color.White, Color.Black);
           }
       }
     


    }

    public  enum EnumMenuType
{
        Normal,
        Confirm,
        Cancel
}

    public  delegate void OnMenuSelectHandler (int menuid);
   public class Panel:IPanel
   {
     
       public          Menu[] menuArray;
       public Panel(Menu[] menuArray)
       {
           LastHitMenuID = -1;
           this.menuArray = menuArray;
          
       }


       public Menu  GetMenu(int menuid)
       {
         
           return this.menuArray[menuid];
       }




       public int HitTest(int x, int y)
       {
           int menuid = y / TouchPanel.FontSize;

           if (menuid < menuArray.Length)
           {
               LastHitMenuID = menuid;
               return menuid;
           }
           else
               return -1;

       }

       bool _IsActive;
       public bool IsActive
       {
           get
           {
               return _IsActive;
           }
           set
           {
               _IsActive = value;
           }
       }




       Menu[] IPanel.GetAllMenu()
       {
           return menuArray;
       }

      



       public int GetMenuCount()
       {
           return this.menuArray.Length;
       }

       public string GetMenuText(int menuid)
       {
           
           return menuArray[menuid].Text;
       }

       int _LastHitMenuID;
       public int LastHitMenuID
       {
           get
           {
               return _LastHitMenuID;
           }
           set
           {
               _LastHitMenuID = value;
           }
       }


       public void InvokeSelectEvent(int menuid)
       {
           if (this.OnMenuSelect != null)
               this.OnMenuSelect(menuid);
       }

       public void Close()
       {
         //  throw new NotImplementedException();
       }





       public event OnMenuSelectHandler OnMenuSelect;
   }

   public interface IPanel
   {
       Menu[] GetAllMenu();
       Menu GetMenu(int menuid);
       int HitTest(int x, int y);
       bool IsActive { get; set; }
       int GetMenuCount();
       string GetMenuText(int menuid);
       int LastHitMenuID { get; set; }
       void InvokeSelectEvent(int menuid);
       void Close();
         event OnMenuSelectHandler OnMenuSelect;
   }

   //public class Dialog:Panel
   //{
   //    public Dialog(Menu[] menuary):base(menuary)
   //    {

   //    }
   //    public  int ShowDialog(TouchPanel tPanel)
   //    {
   //        throw   new NotImplementedException();

   //    }
       
      
   //}

   public class Menu
   {   
       public string Text { get; set; }
       public EnumMenuType MenuType { get; set; }
   }
}
