   M S B A M L   `   `   `  ����  Appl    Appl.HardwareConfigView   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      .��5      �   � ColumnDefinitions  5	   
   �� 5
      $	��380 �6   �� 5      $	��300 �6   .+�5   
   �� $��1��6   .+�5      ��  �� Orientation$ 
Horizontal=�6   .+�5      �� $��	Start cam��6   ��6-   �� *CommandStartCam+�� 5      $
��10,0q�6   $��Stop cam��6'   ��6:   �� *CommandStopCam+5      �� 5      $ 
Horizontal=�6   $
��0,15q�62   .+�5      �� $��Temps execution : ��6   �� 5      ��6   �� *TimeExecuteCiclic+5      �� 5       �� SelectedIndex 6   �� *SelectedTabIndex+.@�5      �� $��Entrée��6   .��5      �� .+�5      �� $
��0,20q�6    $	��300��6.   $	��300��6:   $��Center=�6J   ��6g   �� *ImageEntryZone+�� 5      $ 
Horizontal=�6%   .+�5      �� $ ��Size image zone entrée : ��6(   �� 5      ��6(   �� *SizeEntryZone+5       5!      5"      �� 5#      $��Piece��6   .��5$      �� .+�5%      �� $
��0,20q�6    $	��300��6.   $	��300��6:   $��Center=�6J   ��6g   �� *
ImagePiece+�� 5&      $ 
Horizontal=�6%   .+�5'      �� $��Size image : ��6(   �� 5(      ��6(   �� *SizeImagePiece+5)      5*      5+      �� 5,      $	��Fin��6   .��5-      �� .+�5.      �� $	��300��6    $	��300��6,   $��Center=�6<   ��6Y   �� *ImageEndConv+�� 5/      $ 
Horizontal=�6%   .+�50      �� $��Size image : ��6(   �� 51      ��6(   �� *SizeImageEndConv+52      53      54      55      56      �� 58   
   $��0,20,0,0q�6   $��Stretch=�6'   .+�59      �� $ 
Horizontal=�6   .+�5:      �� $��Convoyeur start��6   $	��100��63   ��6?   �� *CommandStartConvoyeur+�� 5;      $
��10,0q�6   $��Convoyeur stop��6'   $	��100��6@   ��6L   �� *CommandConvoyeurStop+5<      �� 5=      $
��0,20q�6   $ 
Horizontal=�6'   .+�5>      �� $��	Go To Pos��6   ��6-   �� *CommandPlateauGoTo+�� 5?      $
��10,0q�6   $	��100��6F   ��6(   �� *NextPosition+6F   �� 5@      $��20��6B   ��6   �� *Hard.PlateauPosition+6B   5A      �� 5B      $��Eject��6   $	��100��6%   ��61   �� *CommandEjectPiece+�� 5D      $
��0,20q�6   $ 
Horizontal=�6'   .+�5E      �� -   5        $��Plateau��5E      $��Plateau move��6(   $	��100��6?   6K   5F      �� 5H      -   5        $	��deb��5H      A� 5J       A� Columns$ 2��6   .+�5K      �� $	��0,0q�6   .+�5L      �� $ 
Horizontal=�6!   .+�5M      �� $��In3 D0, Plateau ref : ��6$   �� 5N      ��6$   �� *Hard.SwitchPlateauRef0+5O      �� 5P      $ 
Horizontal=�6!   .+�5Q      �� $��In0 D1, Plateau pos : ��6$   �� 5R      ��6$   �� *Hard.SwitchPlateauPosition+5S      �� 5T      $ 
Horizontal=�6!   .+�5U      �� $��In1 D2, Pce Det : ��6$   �� 5V      ��6$   �� *Hard.IsPieceDetected+5W      �� 5X      $ 
Horizontal=�6!   .+�5Y      �� $��In2 D4, Eject : ��6$   �� 5Z      ��6$   �� *Hard.SwitchPush+5[      5\      �� 5]      $��1��6   .+�5^      �� $ 
Horizontal=�6!   .+�5_      �� $��Eject : ��6$   �� 5`      ��6$   �� *Hard.IsPushEnable+5a      �� 5b      $ 
Horizontal=�6!   .+�5c      �� $��
Plateau : ��6$   �� 5d      ��6$   �� *Hard.IsPlateauEnable+5e      �� 5f      $ 
Horizontal=�6!   .+�5g      �� $��Convoyeur : ��6$   �� 5h      ��6$   �� *Hard.IsConvoyeurEnable+5i      5j      5k      5m      5n      5o      