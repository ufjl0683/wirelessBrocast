   M S B A M L   `   `   `  ����a  ]PresentationFramework.Aero, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35gJclr-namespace:Microsoft.Windows.Themes;assembly=PresentationFramework.AeroMicrosoft.Windows.Themes  5   	    wpfBroadcast&    wpfBroadcast.Dialog.wndBroadcast   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35n jSAPBusinessObjects.WPF.ViewerShared, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   ]ThemesJclr-namespace:Microsoft.Windows.Themes;assembly=PresentationFramework.Aero     5   	   -   5          � Title$  wndBroadcast��5   	   $��626.087��6   $��700.79��6/   $��16(�6>    � 	Resources 5      ��%    5      .��5      � $��0,0,0.4,-0.2q�6      NormalBrush#��[  6!    � ColumnDefinitions 5   
   �� 5      .+�5   
   &   System.Windows.Controls.DataGrid  -   5          Name$ grdSite��5      $��18(�6$      HeadersVisibility$ Column=�63      AutoGenerateColumns .  $��
0,66,0.4,0q�6k      ColumnHeaderHeight$ 50Q�6   $��Stretch=�6�      Columns 5      4  .System.Windows.Controls.DataGridTemplateColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$ 選取��6.   	   CellTemplate	 5      �� 5      .R�5      �� -   5        $��5,5,0,0q�5   V   
 d� 	IsChecked
 6(   �� *
IsSelected+ �� Mode$ TwoWay=�6h   5      5      0  *System.Windows.Controls.DataGridTextColumn  5         
IsReadOnly . $ 站號��6b   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6%   �� *	SITE_ID+$ TwoWay=�6b     5        .    Width5  /System.Windows.Controls.DataGridLengthConverter$	 200 6b   $ 站名��6o    6%   �� *	SITE_NAME+$ TwoWay=�6o     5!       . $ 傳送��6?   	 5"      �� 5#      .R�5$      ��  J� IsHitTestVisible .  $��5,5,0,0q�6k   
 6A   �� *IsSend+$ TwoWay=�6k   5%      5'        5(       . $ 次數��6W    6%   �� *	RepeatCnt+6W     5)       . $ 忙碌��6?   	 5*      �� 5+      .R�5,      ��  .  $��5,5,0,0q�6k   
 6A   �� *IsBusy+$ TwoWay=�6k   5-      5/        50      $ 監聽��6)   $ 50 65   	 51      �� 52      .R�53      �� -   5          ButtonStyle1#��[ 53   %   $��監聽��6j   {�6K   �� *	CanEcho+6w   54      56      58      �� 59   
   -   5        $��全選��59      $
��Left=�6   $��32��69   $��	10,15,0,0q�6E   $	��Top=�6X   $��90��6p   6{   �� 5:   
   $
��Left=�6   $��24��6/   $��
356,20,0,0q�6;   $
��Wrap=�6O   $��播音次數:��6c   $	��Top=�6p   $��72��6�   ���	#FFF9F7F76�   �� 5;   
   $
��Left=�6   $��24��6/   $��
132,20,0,0q�6;   $
��Wrap=�6O   $��
預錄詞:��6c   $	��Top=�6o   $��63��6�   ���	#FFF9F7F76�   �� 5<   
   -   5         ��Name$ txtCount��5<      $
��Left=�6$   $��27��6?   $��
433,17,0,0q�6K    �� TextWrapping$
 Wrap=�6_   $	��Top=�6s   $	��111��6�   $��1��6�   �� 5=   
   -   5         ��Name$ cbRecordSound��5=      $
��Left=�6*   $��34��6E   $��
200,15,0,0q�6Q   $	��Top=�6e   $	��125��6}   ��5>      �� 5?      .R�5@      �� ��6!   �� *Name+5A      5C      �� 5D   
   -   5        $��開始��5D      $
��Left=�6!   $��37��6<   $��
572,12,0,0q�6H   $	��Top=�6\   $��97��6t   6   5F      5G      