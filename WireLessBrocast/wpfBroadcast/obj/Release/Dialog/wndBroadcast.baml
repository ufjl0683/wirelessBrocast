   M S B A M L   `   `   `  ���� a  ]PresentationFramework.Aero, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35gJclr-namespace:Microsoft.Windows.Themes;assembly=PresentationFramework.AeroMicrosoft.Windows.Themes   wpfBroadcast&    wpfBroadcast.Dialog.wndBroadcast   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35n jSAPBusinessObjects.WPF.ViewerShared, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     X TXceed.Wpf.Toolkit, Version=2.3.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f44xctk)http://schemas.xceed.com/wpf/xaml/toolkit  WThemesJclr-namespace:Microsoft.Windows.Themes;assembly=PresentationFramework.Aero  -     � Title$  wndBroadcast��$��626.087��$��700.79��$��16(� � 	Resources ��%    .��� $��0,0,0.4,-0.2q�   NormalBrush#��[   � ColumnDefinitions �� .+�%  Xceed.Wpf.Toolkit.BusyIndicator  -     Name$ busyIndicator��&   System.Windows.Controls.DataGrid  -     Name$ grdSite��$��18(�   HeadersVisibility$ Column=�   AutoGenerateColumns .  $��0,66,0.4,53q�   ColumnHeaderHeight$ 50Q�$��Stretch=�   Columns 4  .System.Windows.Controls.DataGridTemplateColumn  ,  &System.Windows.Controls.DataGridColumn	   Header$	 選取��
   CellTemplate
 �� .R��� -   $��5,5,0,0q� d� 	IsChecked �� *
IsSelected+ �� Mode$ TwoWay=�0  *System.Windows.Controls.DataGridTextColumn     
IsReadOnly . $	 站號��1  +System.Windows.Controls.DataGridBoundColumn   Binding �� *	SITE_ID+$ TwoWay=�   .    Width5  /System.Windows.Controls.DataGridLengthConverter$	 200 $	 站名�� �� *	SITE_NAME+$ TwoWay=�   . $	 傳送��
 �� .R���  J� IsHitTestVisible .  $��5,5,0,0q� �� *IsSend+$ TwoWay=�   . $	 次數�� �� *	RepeatCnt+   . $	 忙碌��
 �� .R���  .  $��5,5,0,0q� �� *IsBusy+$ TwoWay=�  $	 監聽��$ 50 
 �� .R��� -     ButtonStyle1#��[ $��監聽��{��� *	CanEcho+�� -   $��全選��$
��Left=�$��32��$��	10,15,0,0q�$	��Top=�$��90���� $
��Left=�$��24��$��
356,20,0,0q�$
��Wrap=�$��播音次數:��$	��Top=�$��72�����	#FFF9F7F7�� $
��Left=�$��24��$��
132,20,0,0q�$
��Wrap=�$��
預錄詞:��$	��Top=�$��63�����	#FFF9F7F7�� -    ��Name$ txtCount��$
��Left=�$��27��$��
433,17,0,0q� �� TextWrapping$
 Wrap=�$	��Top=�$	��111��$��1���� -    ��Name$ cbRecordSound��$
��Left=�$��34��$��
200,15,0,0q�$	��Top=�$	��125������ .R��� ���� *Name+�� -	   $��開始��$
��Left=�$��37��$��
572,12,0,0q�$	��Top=�$��97���� -
    ��Name$ btnEmergency��$��緊急廣播��$��0,0,18.4,10q�$��Right=�$	��104��$��35��$��Bottom=�