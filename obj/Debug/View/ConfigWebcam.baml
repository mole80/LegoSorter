   M S B A M L   `   `   `  ����O  Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem  5      [ WSystem.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089Y@clr-namespace:System.Windows.Forms;assembly=System.Windows.FormsSystem.Windows.Forms 5      ? ;Appl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:ApplAppl 5         Appl.ConfigWebcam   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      9System&clr-namespace:System;assembly=mscorlib     5      UWinForms@clr-namespace:System.Windows.Forms;assembly=System.Windows.Forms     5      $localclr-namespace:Appl     5      -   5          � 	Resources  5       �� 
TargetType   StyleStackParamCam �� 	Resources �� Property �� Value  StyleStackParamCamAqc �� BasedOn �� DataType  Appl.WindowsAquisition �� Orientation �� Mode	 �� Triggers
 �� Value �� Binding �� 
TargetName��%�  &	        &	     7   (�� �    5!      *, +)7  7  7  7  �� 5   
   " ��6    5      ���� 5      '��       " ��6   .�5      ��  � ��$ 70��6.   5      �� 5      '��       " ��6   .�5      ��  � ��$	 200��6.   �� 5       �@��AutoToolTipPlacement$ TopLeft=�6=   5      5      5      �� 5   
   " ��6   8    5      ���� 5      '��       " ��6   .�5      ��  � ��$	 100��6.   5      5      5      �� 5!   
   "  6   .R�5"      �� $��borderTempl��6)   
��� � �$��0q�6<   ._�5#      �� $	��290��6   .+�5$      �� $��Center=�6    $
��0,10q�6=   $��16(�6b   ��6K   �� *Title+6b   �� 5&      $ 
Horizontal=�6!   8��  .+�5'      �� $
��Left��6$   �� 5(      $��TextBoxLeft��6"   ��65   �� *PosX+$ TwoWay=�5)      �� 5*      $ 
Horizontal=�6!   8�� .+�5+      �� $	��Top��6$   �� 5,      $��
TextBoxTop��6"   ��64   �� *PosY+$ TwoWay=�5-      �� 5.      $ 
Horizontal=�6!   8�� .+�5/      �� $��Width��6$   �� 50      $��TextBoxWidth��6"   ��66   �� *Width+$ TwoWay=�51      �� 52      $ 
Horizontal=�6!   8�� .+�53      �� $��Height��6$   �� 54      $��TextBoxHeight��6"   ��67   �� *Height+$ TwoWay=�55      56      57      	 59      �� 5:      $

 True��6=    6   �� *
IsSelected+.a�5;      �� $ borderTempl��6    � ��$ 2q�6Q   5<      5>      5@      .��5B      � -   5        .+�5C   
   ]� .+�5D      �� $
��0,10q�6   $	��Top=�6'   .+�5E      �� $ 
Horizontal=�6   .+�5F      �� $��Start��6   ��6-   �� *CommandStartWebCam+�� 5G      $��	10,0,25,0q�6   $
��Stop��60   ��6?   �� *CommandStopWebCam+�� 5I      $��
Monochrome��6    d� 	IsChecked 64   �� *	GrayImage+�� 5J      $
��25,0q�6   $��Use background��6-    6F   �� *UseBackground+�� 5L      $��Update Background��6   ��69   �� *CommandCaptureBackground+�� 5N      $
��25,0q�6   $��
Use filter��6-    6B   �� *	UseFilter+5P      � 5R       � ColumnDefinitions 5S      �� 5T      $	��300 �6+   �� 5U      $	��200 �6+   �� 5V      $	��200 �6+   �� 5W      $	��200 �6+   .+�5Z      �� -   5        $��1��5Z   %   ��65   �� *Cam.AquBrickImage+6[   �� 5[      -   5        $��2��5[   %   ��65   �� *Cam.AquEndConvWindow+6^   �� 5\      -   5        $��3��5\   %   ��65   �� *Cam.AquEntryZone+6Z   �� 5^      .+�5_      �� $��10q�6$   $��Webcam��60   $��16(�6>   �� 5`      $ 
Horizontal=�6%   #��[  6>   .+�5a      �� $��
Brightness��6(   �� 5b      -   5         � Minimum$ 50Q�5b   S    � Maximum$	 100Q�6`    �� AutoToolTipPlacement$ TopLeft=�6�    � Value 6%   �� *Cam.Britghness+$ TwoWay=�6�   5c      �� 5d      $ 
Horizontal=�6%   #��[  6>   .+�5e      �� $��
Saturation��6(   �� 5f      -   5        $ 0Q�5f   S   $	 150Q�6_    6%   �� *Cam.Saturation+$ TwoWay=�6m   5g      �� 5h      $ 
Horizontal=�6%   #��[  6>   $y�	Collapsed=�6j   .+�5i      �� $��Gamma��6(   �� 5j      -   5        $ 0Q�5j   N   $	 255Q�6Z    6%   �� *	Cam.Gamma+$ TwoWay=�6h   5k      �� 5l      $ 
Horizontal=�6%   #��[  6>   .+�5m      �� $
��Gain��6(   �� 5n      -	   5        $ 0Q�5n   M   $ 30Q�6Y    6%   �� *
Cam.Gain+$ TwoWay=�6f   5o      �� 5p      $ 
Horizontal=�6%   #��[  6>   .+�5q      �� $��Exposure��6(   �� 5r      -
   5        $	 -10Q�5r   Q   $ 0Q�6_    �� TickFrequency$ 1Q�6k    6%   �� *Cam.Exposure+$ TwoWay=�6}   5s      �� 5t      $ 
Horizontal=�6%   #��[  6>   .+�5u      �� $��Contrast��6(   �� 5v      -   5        $ 30Q�5v   Q   $	 100Q�6^    6%   �� *Cam.Contrast+$ TwoWay=�6l   5w      �� 5x      $ 
Horizontal=�6%   #��[  6>   $y�	Collapsed=�6j   .+�5y      �� $��Focus��6(   �� 5z      -   5        $
 -100Q�5z   N   $	 100Q�6]    6%   �� *	Cam.Focus+$ TwoWay=�6k   5{      �� 5}      $
��0,20q�6%   $ 
Horizontal=�63   #��[  6L   .+�5~      �� $��Delta BG��6(   �� 5      -   5        $ 0Q�5   S   $	 200Q�6_    6%   �� *DeltaIntensity+$ TwoWay=�6m   5�      �� 5�      $��0,10,0,0q�6$   ��66   �� *CursorPositionImage.PosX+ �� StringFormat$ Image X : {0:F0}���� 5�      ��6$   �� *CursorPositionImage.PosY+$ Image Y : {0:F0}���� 5�      ��6$   �� *CursorPositionCanvas.PosX+$ Canvas X : {0:F0}���� 5�      ��6$   �� *CursorPositionCanvas.PosY+$ Canvas Y : {0:F0}���� 5�      ��6$   �� *CursorPositionCanvas.PosY+$ Moyenne fond : {0:F0}��5�      5�      5�      A� 5�       A� Columns$ 2��6   .+�5�      �� -   5        $��CanvasRectAqc��5�   %   $	��500��6   $��Stretch=�6:   $��Stretch=�6X   .+�5�      �� -   5        $��ImageWebCam��5�   (   $	��500��6   $
��Left=�6;   ��6V   �� *ImageWebSource+6�   �� 5�      -   5        $��RectangleWindows��5�       ���	#FF51FF00$��2��6K   5�      �� 5�      .+�5�      7  1System.Windows.Forms.Integration.WindowsFormsHost  -   5        $��WinHostHistoControl��5�   '   $	��200��6B   �� 5�      $	��200��6   ��6(   �� *ImageBackground+5�      5�      5�      5�      5�      