using Packing_Net.Classes;
using PackingClassLibrary.Commands;
using PackingClassLibrary.Commands.SMcommands;
using PackingClassLibrary.CustomEntity;
using PackingClassLibrary.CustomEntity.SMEntitys;
using System;
using System.Collections;
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
using System.Windows.Threading;
namespace PackingNet.Pages
{
    /// <summary>
    /// Interaction logic for wndShipmentForPallet.xaml
    /// </summary>
    public partial class wndShipmentForPallet : Window
    {

        DispatcherTimer dtLoadUpdate;

        
        
 
        public wndShipmentForPallet()
        {
            InitializeComponent();
        }

        private void btnAdd_Click_1(object sender, RoutedEventArgs e)
        {

          //  Global.ShipmentList lstshipment = new Global.ShipmentList;

            cstShipmentList lstshipment = new cstShipmentList();

            lstshipment.Shipment = txtShipmentNumber.Text;



           Global.ListShipmnet.Add(lstshipment);

            txtShipmentNumber.Text = "";

          //  grdShipment.ItemsSource = ListShipmnet;


            dtLoadUpdate = new DispatcherTimer();
            dtLoadUpdate.Interval = new TimeSpan(0, 0, 0, 0, 10);
            dtLoadUpdate.Tick += dtLoadUpdate_Tick;
            //start the dispacher.
            dtLoadUpdate.Start();


            
        }

        private void dtLoadUpdate_Tick(object sender, EventArgs e)
        {
            grdShipment.ItemsSource = "";

            grdShipment.ItemsSource = Global.ListShipmnet;
            dtLoadUpdate.Stop();
        }



        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            //grdShipment.ItemsSource = Global.ShipmentList;
        }

        private void btnNext_Click_1(object sender, RoutedEventArgs e)
        {
            wndBoxInfoForPallet wnd = new wndBoxInfoForPallet();
            wnd.ShowDialog();
        }
    }
}
