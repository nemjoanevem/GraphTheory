   M S B A M L   `   `   `  ����   GraphTheory    GraphTheory.Settings   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006       � Title$  Gráf beállítások��$	��450��$	��600�� � 
ResizeMode$ NoResize=� � WindowStyle$
 None=�.���  � ColumnDefinitions �� �� �� �� $	��120 ���  � RowDefinitions �� �� �� �� �� .+��� -    ��Name$ nodesCounterlbl��$	��100��$��30��$��0��$��0,0,6,45q�$��0��.��Csúcsok száma:�� -    ��Name$ NodesCounterCB��$��90��$��30��$��0, 0, 10, 0q�$��0��$��0�� �� SelectedIndex$ 6��.@��� -   $
��cbi3��.��3�� -   $
��cbi4��.��4�� -   $
��cbi5��.��5�� -   $
��cbi6��.��6�� -   $
��cbi7��.��7�� -   $
��cbi8��.��8�� -	   $
��cbi9��.��9�� $��Élek száma: ��$
��Left=�$��Bottom=�$��1��$��0���� $��Center=�$��Bottom=�$��25, 0, 0, 0q�$��1��$��0������  �� Path$ 
EdgesCount��� -
   	 ��Name$	 TxtBoxX��$��0��$��3��$��25��$��10, 0, 10, 40q��� -   $	 TxtBoxY��$��0��$��3��$��25��$��10, 40, 10, 00q��� -   $	 TxtBoxValue��$��1��$��3��$��25��$��10, 0, 10, 52q��� -   $
 lblX��$��0��$��2��$��50��$��25��$��0, 0, 0, 44q�$��Right=�.��
X érték:�� -   $
 lblY��$��0��$��2��$��50��$��25��$��0, 36, 0, 0q�$��Right=�.��
Y érték:�� -   $ lblValue��$��1��$��2��$��35��$��25��$��0, 0, 0, 56q�$��Right=�.��Súly:�� -   
 ��Name$
 
AddEdgebtn��$��Él hozzáadás��   btnGray#��[  $��4��$��0��$��Center=�$	��Top=�$	��106��$��0,7,0,0q��� -   $
 EditEdgebtn��$��Él szerkesztés��#��[  $��4��$��0��$��Center=�$��Bottom=�$	��106��$��0,0,0,7q��� -   $
 RemoveEdgebtn��$��Él törlés��#��[  $��4��$��1��$��Center=�$	��Top=�$	��106��$��0,7,0,0q��� -   $
 Backbtn��$��Vissza��#��[  $��4��$��1��$��Center=�$��Bottom=�$	��106��$��0,0,0,7q��� -    ��Name$ ListView��$��2��$��3��$��5�� �� View �� .P���  �� Width$	 150��$��X�� �� DisplayMemberBinding �� *X+�� $	 150��$��Y�� �� *Y+�� $	 150��$��Súly�� �� *Value+