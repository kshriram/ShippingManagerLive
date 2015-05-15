using Packing_Net.Classes;
using PackingClassLibrary.CustomEntity;
using PackingClassLibrary.CustomEntity.ReportEntitys;
using PackingClassLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Packing_Net.Pages.ReportPages
{
    /// <summary>
    /// Interaction logic for wndRecentPacked.xaml
    /// </summary>
    public partial class wndRecentPacked : Window
    {
        public wndRecentPacked()
        {
            InitializeComponent();
            FillAll();
        }

        private void FillAll()
        {
            try
            {
               
                List<cstPackingTime> packingTime = Global.controller.GetPackingTimeQuantity();
                List<cstPackingTime> lspackingTime = new List<cstPackingTime>();
                foreach (var Listitem in packingTime)
                {
                    cstPackingTime Timels = new cstPackingTime();
                    Timels.Quantity = Listitem.Quantity;
                    Timels.PackingID = Listitem.PackingID;
                    Timels.ShippingNumber = Listitem.ShippingNumber;
                   Timels.TimeSpend = Listitem.TimeSpend;
                   lspackingTime.Add(Timels);
                }
                Guid RecentPackedID = Global.RecentlyPackedID;
                cstPackingTime packingItem = lspackingTime.SingleOrDefault(i => i.PackingID == RecentPackedID);
                List<cstPackageDetails> lsPackingDetails = Global.controller.GetPackingDetailTbl(RecentPackedID);
                dgvPackedDetailsList.ItemsSource = lsPackingDetails;
                dgvPackedDetailsList.Items.Refresh();
                lblDShipmentID.Content =Global.controller.GetShippingNumber( lsPackingDetails[0].PackingId);
                lblDStatus.Content = "Packed";
              
                lblDTime.Content = packingItem.TimeSpend.ToString();
                lblDItem.Content = packingItem.Quantity.ToString();
                String Location = lsPackingDetails[0].ShipmentLocation.ToString();
                String UserName =Global.controller.GetSelcetedUserMaster(Global.controller.GetPackingList(RecentPackedID, true).UserID).First().UserFullName.ToString();

                foreach (var LocationItem in lsPackingDetails)
                {
                    lblDUserName.Content = UserName;
                    lblDLocation.Content = Location;
                    if (Location != LocationItem.ShipmentLocation)
                    {
                        Location = LocationItem.ShipmentLocation;
                        UserName = Global.controller.GetSelcetedUserMaster(Global.controller.GetPackingList(RecentPackedID, true).UserID).First().UserFullName.ToString();
                        lblDLocation.Content = Location + " & " + lblDLocation.Content;
                        lblDUserName.Content = UserName + " & " + lblDUserName.Content;
                    }
                }
            }
            catch (Exception)
            { }
        }

        private void btnContinue_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception)
            {}
        }
    }
}
