   M S B A M L   `   `   `  ���� q  mSystem.Windows.Controls.Layout.Toolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35�`clr-namespace:System.Windows.Controls.Primitives;assembly=System.Windows.Controls.Layout.Toolkit"System.Windows.Controls.Primitives  U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35C0clr-namespace:System.Windows;assembly=WPFToolkitSystem.Windows O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem G CwpfBroadcast, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null?$clr-namespace:wpfBroadcast.ConverterwpfBroadcast.Converter qUclr-namespace:System.Windows.Controls;assembly=System.Windows.Controls.Layout.ToolkitSystem.Windows.Controls  +clr-namespace:wpfBroadcastwpfBroadcast    wpfBroadcast.Main   o
Primitives`clr-namespace:System.Windows.Controls.Primitives;assembly=System.Windows.Controls.Layout.Toolkit  \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35n jSAPBusinessObjects.WPF.ViewerShared, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @vsm0clr-namespace:System.Windows;assembly=WPFToolkit     6sys&clr-namespace:System;assembly=mscorlib     4con$clr-namespace:wpfBroadcast.Converter     jControlsUclr-namespace:System.Windows.Controls;assembly=System.Windows.Controls.Layout.Toolkit     ;esri*http://schemas.esri.com/arcgis/client/2009     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     ,localclr-namespace:wpfBroadcast     -     � Title$
  Main��$��461.756��$��629.154�� � WindowStartupLocation$ CenterScreen=� � WindowState$ 	Maximized=�$��18(� � Icon$ Mark.png�� � 	Resources 1  +wpfBroadcast.Converter.BoolToColorConverter   BoolToColorConverter  HyperLinkButtonStyle �� 
TargetType �� Property �� Value �� 
TargetType	 �� Triggers
 P� Property P� Value+   %System.Windows.Controls.AccordionItem'  !System.Windows.VisualStateManager   VisualStateGroups%  System.Windows.VisualStateGroup  Name   Transitions%  System.Windows.VisualTransition   GeneratedDuration   States   System.Windows.VisualState  Name   
Storyboard ��Name g� 	BeginTime �� 
TargetName �� TargetProperty O� KeyTime �� 	KeySpline O� Value g� Duration R� KeyTime R� Value  System.Windows.Visibility ��Name �� Padding �� CornerRadius  � RowDefinitions! ��Name" � ColumnDefinitions# ��Name8   2System.Windows.Controls.Primitives.AccordionButton$  Name%   AccordionButtonStyle&   
IsSelected' d� 	IsCheckedA	   ;System.Windows.Controls.Primitives.ExpandableContentControl( 	 Name) 	  
Percentage*   ExpandDirection+ 	  
RevealMode$,   ExpandableContentControlStyle  tblSIteViewSource��%�  &	        &	      '  �     &	 �      �� " ��.���  � �� �� " ��.R��� $��	Underline{�$��
23.4,0,0,0q�#��z���� .F��� $��Segoe UI)���  � ��
 �����	 P� 
 �@J�IsMouseOver . . ���  � ��
 ��ح���  �@�Cursor$
 Hand���� "  .���  � �� �� "  .R�� 
������ #��z��#��z��   $ CommonStates��   $ 0D�.   $ Normal��  $ Pressed��  $ 	MouseOver��  $ Disabled��  $ FocusStates��.   $ Focused��  $ 	Unfocused��  $ ExpansionStates��   $ 0D�.   $ 	Collapsed��. �� $&  AccordionItemCollapsedStoryboard��.��U� $ 00:00:00X�$ 
ExpandSite��$6 0(Primitives:ExpandableContentControl.Percentage)�.\��� $ 
00:00:00.3��$ 	0.2,0,0,1��$ 0Q�T� $ 
00:00:00.3X�$ 00:00:00D�$ 
ExpandSite��$ (UIElement.Visibility)�..�i� $ 00:00:00��  	Collapsed=�  $ Expanded��. �� $% AccordionItemExpandedStoryboard��.��U� $ 00:00:00X�$ 
ExpandSite��$6 0(Primitives:ExpandableContentControl.Percentage)�.\��� $ 
00:00:00.3��$ 	0.2,0,0,1��$ 1Q�  $ LockedStates��   $ 0D�.   $ Locked��. �� .��T� $ 0D�$ ExpanderButton��$ 	IsEnabled�..�i� $ 0�� ��	False��  $ Unlocked��. �� .��T� $ 0D�$ ExpanderButton��$ 	IsEnabled�..�i� $ 0�� ��True��  $ ExpandDirectionStates��   $ 0D�.   $ 
ExpandDown��. �� .��T� $ 0D�$	 rd1��$ Height�..�i� $ 0�� �* �T� $ 0D�$	 cd0��$ Width�..�i� $ 0�� �* �  $ ExpandUp��. �� .��T� $ 0D�$ ExpanderButton��$ 
(Grid.Row)�..�i� $ 0�� ��1��T� $ 0D�$ 
ExpandSite��$ 
(Grid.Row)�..�i� $ 0�� ��0��T� $ 0D�$	 rd0��$ Height�..�i� $ 0�� �* �T� $ 0D�$	 cd0��$ Width�..�i� $ 0�� �* �  $ 
ExpandLeft��. �� .��T� $ 0D�$ ExpanderButton��$ (Grid.ColumnSpan)�..�i� $ 0�� ��1��T� $ 0D�$ 
ExpandSite��$ (Grid.ColumnSpan)�..�i� $ 0�� ��1��T� $ 0D�$ ExpanderButton��$ (Grid.RowSpan)�..�i� $ 0�� ��2��T� $ 0D�$ 
ExpandSite��$ (Grid.RowSpan)�..�i� $ 0�� ��2��T� $ 0D�$ ExpanderButton��$ (Grid.Column)�..�i� $ 0�� ��1��T� $ 0D�$ 
ExpandSite��$ 
(Grid.Row)�..�i� $ 0�� ��0��T� $ 0D�$	 rd0��$ Height�..�i� $ 0�� �* �T� $ 0D�$	 cd0��$ Width�..�i� $ 0�� �* �  $ ExpandRight��. �� .��T� $ 0D�$ ExpanderButton��$ (Grid.ColumnSpan)�..�i� $ 0�� ��1��T� $ 0D�$ 
ExpandSite��$ (Grid.ColumnSpan)�..�i� $ 0�� ��1��T� $ 0D�$ ExpanderButton��$ (Grid.RowSpan)�..�i� $ 0�� ��2��T� $ 0D�$ 
ExpandSite��$ (Grid.RowSpan)�..�i� $ 0�� ��2��T� $ 0D�$ 
ExpandSite��$ 
(Grid.Row)�..�i� $ 0�� ��0��T� $ 0D�$ 
ExpandSite��$ (Grid.Column)�..�i� $ 0�� ��1��T� $ 0D�$	 rd0��$ Height�..�i� $ 0�� �* �T� $ 0D�$	 cd1��$ Width�..�i� $ 0�� �* �.+��� $ 
Background��# z��#��z��#��z��$ 1,1,1,1��._��   �� $	! rd0��$
��Auto ��� $	! rd1��$
��Auto �" �� $	# cd0��$
��Auto ��� $	# cd1��$
��Auto �.+�  $$ ExpanderButton��#��z% #��z��#��z��#' z& ��. $��0��$��0,0,0,0q�$��0,0,0,0q�#��z��#��z��#��z��#��z��#��z��#��z��#��z��#��z��#��z��#��z��#��z��	  $( 
ExpandSite��$��1����.  $) 0Q�#+ z* #��z��#��z��$��0,0,0,0q�#��z, #��z��#��z��#��z��#��z��#��z��#��z��#��z��#��z��#��z��#��z���� .��� - � DataContext#- [ .+�'
   !System.Windows.Controls.Accordion
  -   . 
 Name$. accdMenu��  NormalBrush#��[ $
��Left=�$��Stretch=�$	��224��$��24(�.@�  ���   
ShineBrush#��[ .+��� / �� Orientation$/ 
Horizontal=�.+��� $��32��$��32��$��icon/setting.png���� $��系統設定��.���� .+��� -   $	��168��#��[ $��10q�$��24(�$��使用者管理���� -   $	��168��#��[ $��10q�$��24(�$��警報站管理���� -   $	��168��#��[ $��10q�$��24(�$��預錄詞管理���� -   $	��168��#��[ $��10q�$��24(�$��測試排程管理��  ���� $/ 
Horizontal=�.+��� $��32��$��32��$��icon/broadcast.png���� $��系統廣播��.���� .+��� -   $	��168��#��[ $��10q�$��24(�$��預錄詞廣播���� -   $	��168��#��[ $��10q�$��24(�$��口語廣播��  ���� $/ 
Horizontal=�.+��� $��32��$��32��$��UniformToFill=�$��icon/test.jpg���� $��系統測試��.���� .+��� -	   $	��168��#��[ $��10q�$��24(�$��一般測試���� -
   $	��168��#��[ $��10q�$��24(�$��靜音測試��  ���� $/ 
Horizontal=�.+��� $��32��$��32��$��UniformToFill=�$��icon/report.png���� $��報表��.���� .+��� -   $	��168��#��[ $��10q�$��24(�$��操作紀錄���� -   $	��168��#��[ $��10q�$��24(�$��事件紀錄���� -   $	��168��#��[ $��10q�$��24(�$��排程測試紀錄��&   System.Windows.Controls.DataGrid  -   0  Name$0 tblSIteDataGrid��1   HeadersVisibility$1 Column=�2   
IsReadOnly2 . 3   RowDetailsVisibilityMode$3 VisibleWhenSelected=�$��229,0,0.4,0q�4   EnableRowVirtualization4 . 5   AutoGenerateColumns5 .  $��18(�6   ColumnHeaderHeight$6 50Q�7   	RowHeight$7 30Q�$��Stretch=����� 8   Columns8 0  *System.Windows.Controls.DataGridTextColumn  -   sITE_IDColumn��,  &System.Windows.Controls.DataGridColumn9   Width5  /System.Windows.Controls.DataGridLengthConverter$9 SizeToHeader :   Header$: 編號��1  +System.Windows.Controls.DataGridBoundColumn;   Binding; �� *	SITE_ID+  -   sITE_NAMEColumn��$	9 200 $: 站名��; �� *	SITE_NAME+4  .System.Windows.Controls.DataGridTemplateColumn  $: 通訊��<   CellTemplate< �� .R�A� $��20��$��20������ *Comm+= �� 	Converter#= [    $: AC��< �� .R�A� $��20��$��20������ *AC+#= [    $: DC��< �� .R�A� $��20��$��20������ *DC+#= [    $: 	擴大機��< �� .R�A� $��20��$��20������ *Amp+#= [    $: 喇吧��< �� .R�A� $��20��$��20������ *	Speaker+#= [    $: 箱門��< �� .R�A� $��20��$��20������ *
DoorOpen+#= [  �� -   $
��Test��$��0,0,25.4,18q�$��Right=�$��66��$��34��$��Bottom=�$y�	Collapsed=�