   M S B A M L   `   `   `  ����  Appl    Appl.TrieurControl   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      .��5      ]� .+�5
   
   �� $��10q�6     �� Orientation$  
Horizontal=�6!   $	��Top=�6:   .+�5      �� $��Switch state auto��6    d� 	IsChecked 63   �� *SwitchAutoNextState+�� 5      $	��Log��6    6%   �� *	LogEnable+5      �� 5   
   $	��200��6   $��Right=�6    ��67   �� *Log+A� 5   
    A� Columns$ 3��6   $	��Top=�6"   $��40��67   .+�5      �� $��80��6   $��Start��6    ��60   �� *CommandStartSystem+�� 5      $��80��6   $
��Stop��6!   ��60   �� *CommandStopSystem+�� 5      $��80��6   $��Suiv ->��6    ��62   �� *CommandSwitchNextState+5      A� 5   
   $	��Top=�6   $ 2��6+   .+�5      �� $  
Horizontal=�6   $��40��63   .+�5      �� $��
Machine : ��6   $��Center=�6.   �� 5      $��Center=�69   ��6   �� *StateSystem+69   5      �� 5      $  
Horizontal=�6   $��40��62   .+�5      �� $��Next state : ��6   $��Center=�61   �� 5      $��Center=�67   ��6   �� *	NextState+67   5       5!      �� 5#   
   $	��Top=�6   $  
Horizontal=�6*   $
��0,15q�6C   .+�5$      �� $��Temps execution : ��6   �� 5%      ��6   �� *TimeExecuteCiclic+5&      �� 5(   
   $	��Top=�6   $  
Horizontal=�6*   $��40��6C   .+�5)      �� $��Infos : ��6   $��Center=�6(   �� 5*      $��Center=�63   ��6   �� *	TextInfos+63   5+      �� 5-   
   $  
Horizontal=�6   $	��300��6.   .+�5.      �� $	��300��6   ��6    �� *ImageAtester.ImageSource+�� 5/      $	��300��6   ��6    �� *ImageRef.ImageSource+50      53      54      