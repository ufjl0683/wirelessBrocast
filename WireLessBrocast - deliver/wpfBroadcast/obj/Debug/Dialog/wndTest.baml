   M S B A M L   `   `   `  ����  wpfBroadcast!    wpfBroadcast.Dialog.wndTest   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35n jSAPBusinessObjects.WPF.ViewerShared, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -   5          � Title$  wndTest��5   	   $	��600��6   $	��700��6&   $��18(�6d   .��5      �    NormalBrush#��[  6    � ColumnDefinitions 5   
   �� 5      .+�5	   
   &   System.Windows.Controls.DataGrid  -   5          Name$ grdSite��5	         HeadersVisibility$ Column=�6$   $��18(�6@      AutoGenerateColumns .  $��0,0,0.6,56.4q�6k      ColumnHeaderHeight$ 50Q�6�   $	��460��6�   $��Bottom=�6�      Columns 5
      4  .System.Windows.Controls.DataGridTemplateColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$ 選取��6.      CellTemplate 5      �� 5      .R�5      �� -   5        $��5,5,0,0q�5   V   	 d� 	IsChecked	 6(   �� *
IsSelected+
 �� Mode$
 TwoWay=�6h   5      5      0  *System.Windows.Controls.DataGridTextColumn  5         
IsReadOnly . $ 站號��6b   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6%   �� *	SITE_ID+$
 TwoWay=�6b     5       .    Width5  /System.Windows.Controls.DataGridLengthConverter$	 200 6b   $ 站名��6o    6%   �� *	SITE_NAME+$
 TwoWay=�6o     5       . $ 傳送��6?    5      �� 5      .R�5      ��  J� IsHitTestVisible .  $��5,5,0,0q�6k   	 6A   �� *IsSend+$
 TwoWay=�6k   5      5        5       . $ 次數��6d    6%   �� *	RepeatCnt+$
 TwoWay=�6d     5       . $ 忙碌��6?    5      �� 5      .R�5      ��  .  $��5,5,0,0q�6k   	 6A   �� *IsBusy+$
 TwoWay=�6k   5       5"      5+      �� 5,   
   -   5        $��全選��5,      $
��Left=�6   $��32��69   $��	10,15,0,0q�6E   $	��Top=�6X   $��90��6p   6{   �� 5-   
   
�������$
��Left=�6)   $��24��6D   $��
356,20,0,0q�6P   $
��Wrap=�6d   $��播音次數:��6x   $	��Top=�6�   $��72��6�   $y�	Collapsed=�6�   �� 5.   
   -   5         ��Name$ 	txtRepeat��5.      $
��Left=�6%   $��27��6@   $��
433,17,0,0q�6L    �� TextWrapping$
 Wrap=�6`   $	��Top=�6t   $	��111��6�   $��1��6�   $y�	Collapsed=�6�   �� 5/   
   -   5        $��開始��5/      $
��Left=�6"   $��37��6=   $��
572,12,0,0q�6I   $	��Top=�6]   $��97��6u   6�   �� 50   
   -   5        $��緊急廣播��50      $��0,0,39.6,10.4q�6#   $��Right=�6Q   $	��126��6m   $��41��6y   $��Bottom=�6�   �� 51   
   -   5         ��Name$ txtTitle��51      $
��Left=�6&   $��27��6A   $��
139,12,0,0q�6M   $
��Wrap=�6a   $��	TextBlock��6u   $	��Top=�6�   $	��344��6�   ���	#FFFEF6F6$��24(�6�   $
��Bold"�6�   53      54      