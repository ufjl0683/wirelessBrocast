   M S B A M L   `   `   `  ����   wpfBroadcast!    wpfBroadcast.Dialog.wndTest   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35n jSAPBusinessObjects.WPF.ViewerShared, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     -     � Title$  wndTest��$	��600��$	��700��$��18(�.���    NormalBrush#��[   � ColumnDefinitions �� .+�&   System.Windows.Controls.DataGrid  -     Name$ grdSite��   HeadersVisibility$ Column=�$��18(�   AutoGenerateColumns .  $��0,0,0.6,56.4q�   ColumnHeaderHeight$ 50Q�$	��460��$��Bottom=�   Columns 4  .System.Windows.Controls.DataGridTemplateColumn  ,  &System.Windows.Controls.DataGridColumn   Header$ 選取��   CellTemplate �� .R��� -   $��5,5,0,0q�	 d� 	IsChecked	 �� *
IsSelected+
 �� Mode$
 TwoWay=�0  *System.Windows.Controls.DataGridTextColumn     
IsReadOnly . $ 站號��1  +System.Windows.Controls.DataGridBoundColumn   Binding �� *	SITE_ID+$
 TwoWay=�   .    Width5  /System.Windows.Controls.DataGridLengthConverter$	 200 $ 站名�� �� *	SITE_NAME+$
 TwoWay=�   . $ 傳送�� �� .R���  J� IsHitTestVisible .  $��5,5,0,0q�	 �� *IsSend+$
 TwoWay=�   . $ 次數�� �� *	RepeatCnt+$
 TwoWay=�   . $ 忙碌�� �� .R���  .  $��5,5,0,0q�	 �� *IsBusy+$
 TwoWay=��� -   $��全選��$
��Left=�$��32��$��	10,15,0,0q�$	��Top=�$��90���� 
�������$
��Left=�$��24��$��
356,20,0,0q�$
��Wrap=�$��播音次數:��$	��Top=�$��72��$y�	Collapsed=��� -    ��Name$ 	txtRepeat��$
��Left=�$��27��$��
433,17,0,0q� �� TextWrapping$
 Wrap=�$	��Top=�$	��111��$��1��$y�	Collapsed=��� -   $��開始��$
��Left=�$��37��$��
572,12,0,0q�$	��Top=�$��97���� -   $��緊急廣播��$��0,0,39.6,10.4q�$��Right=�$	��126��$��41��$��Bottom=��� -    ��Name$ txtTitle��$
��Left=�$��27��$��
139,12,0,0q�$
��Wrap=�$��	TextBlock��$	��Top=�$	��344�����	#FFFEF6F6$��24(�$
��Bold"�