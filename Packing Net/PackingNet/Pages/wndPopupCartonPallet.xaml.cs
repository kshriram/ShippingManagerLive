using Packing_Net.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PackingNet.Pages
{
    /// <summary>
    /// Interaction logic for wndPopupCartonPallet.xaml
    /// </summary>
    public partial class wndPopupCartonPallet : Window
    {
        public wndPopupCartonPallet()
        {
            InitializeComponent();
            //this.Closing += new System.ComponentModel.CancelEventHandler(MyWindow_Closing);
        }
        //void MyWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        //{
        //    if (true)
        //    {

        //    }
        //    else
        //    {
        //        e.Cancel = true;
        //    }

            
        //}
        private void rdoCarton_Checked_1(object sender, RoutedEventArgs e)
        {
           // Global.CartonPallet = "Carton";
            WindowThread.start();
            this.Close();
            wndBoxInfoForPallet pallet = new wndBoxInfoForPallet();
            pallet.ShowDialog();

           
        }

        private void rdoPallet_Checked_1(object sender, RoutedEventArgs e)
        {
            //Global.CartonPallet = "Pallet";
            WindowThread.start();
            this.Close();
            wndPalletPrintStatus palletstatus = new wndPalletPrintStatus();
            palletstatus.ShowDialog();

           
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
