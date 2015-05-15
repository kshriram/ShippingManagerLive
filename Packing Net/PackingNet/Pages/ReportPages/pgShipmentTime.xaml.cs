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
using System.Windows.Navigation;
using System.Windows.Shapes;
using PackingClassLibrary;
using PackingClassLibrary.CustomEntity;
using PackingClassLibrary.CustomEntity.ReportEntitys;
using Packing_Net.Classes;
using Packing_Net;

namespace Packing_Net.Pages.ReportPages
{
    /// <summary>
    /// Interaction logic for pgShipmentTime.xaml
    /// </summary>
    public partial class pgShipmentTime : Page
    {

        protected static Guid StationID;

        public pgShipmentTime()
        {
            //////if (Global.newWindowThread.IsAlive)
            //////{
            //////    Global.newWindowThread.Abort();
            //////}
            StationID = Global.controller.GetStationMaster(Global.controller.getDeviceMAC())[0].StationID;
            InitializeComponent();
            FilldgvShipmentPackedList();
            FillUserNameCmb();
            FillStatusCombo();
           
            lblDShipmentID.Content = "";
            lblDStatus.Content = "";
            lblDTime.Content = "";
            lblDItem.Content = "";
            lblDUserName.Content = "";
            lblDLocation.Content = "";
            lblDoverride.Content = "";
        }

        public void SetLabels(List<cstPackingTime> lsPackingTime)
        {
            try
            {
                lblTotalPacked.Content = lsPackingTime.Count();
                TimeSpan sumTime = TimeSpan.FromSeconds(0);
                foreach (var lsitem in lsPackingTime)
                {
                    TimeSpan t2 = new TimeSpan();
                    TimeSpan.TryParse(lsitem.TimeSpend, out t2);
                    sumTime = sumTime.Add(t2);
                }

                lblTime.Content = String.Format("{0:D2}-Day {1:D2}H:{2:D2}M:{3:D2}S", sumTime.Days, sumTime.Hours, sumTime.Minutes, sumTime.Seconds);
            }
            catch (Exception)
            { }
        }

        public void FilldgvShipmentPackedList()
        {
            try
            {
               // WindowThread.start();
                List<cstPackingTime> lspackingTime = Global.controller.GetPackingTimeQuantity_ByStation(StationID);
                dgvShipmentPackedList.ItemsSource = lspackingTime;
                //combox bind
                cmbShipmentID.ItemsSource = lspackingTime;
                cmbShipmentID.Items.Refresh();
                //Set Labels
                SetLabels(lspackingTime);
                //if (Global.newWindowThread.IsAlive)
                //{
                //    Global.newWindowThread.Abort();
                //}
            }
            catch (Exception)
            { }
        }

        public void FillUserNameCmb()
        {
            try
            {
                List<cstUserMasterTbl> lsUserMaser = Global.controller.GetUserInfoList();
                cmbUserNames.ItemsSource = lsUserMaser;
            }
            catch (Exception)
            { }
        }

        public void FillStatusCombo()
        {
            List<PackingStat> lspackingStatus = new List<PackingStat>();
            PackingStat Status = new PackingStat();
            Status.ID = 0;
            Status.Status = "Packed";
            lspackingStatus.Add(Status);
            PackingStat Status1 = new PackingStat();
            Status1.ID = 1;
            Status1.Status = "Partially Packed";
            lspackingStatus.Add(Status1);
            cmbShipmentStatus.ItemsSource = lspackingStatus;
            cmbShipmentStatus.Items.Refresh();
        }

        private void cmbUserNames_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           // WindowThread.start();
            lblCShipmentID.Visibility = System.Windows.Visibility.Hidden;
            lblShipmentID.Visibility = System.Windows.Visibility.Hidden; 
            cmbShipmentID.Visibility = System.Windows.Visibility.Hidden;

            ClearShipmentDetailGrid();
            lblCUserName.Visibility = cmbUserNames.SelectedItem == null ? Visibility.Visible : Visibility.Hidden;
            if (cmbShipmentStatus.Text == "")
            {
                if (dtpFromDate.Text.ToString() == "" || dtpToDate.Text == "")
                {
                    try
                    {
                        Guid UserID = Guid.Empty;
                        Guid.TryParse (cmbUserNames.SelectedValue.ToString(), out UserID);
                        List<cstPackingTime> lspackingTime = Global.controller.GetPackingTimeQuantity_ByStation(UserID, StationID);
                        dgvShipmentPackedList.ItemsSource = lspackingTime;
                        dgvShipmentPackedList.Items.Refresh();
                        SetLabels(lspackingTime);
                        //combox bind
                        cmbShipmentID.ItemsSource = lspackingTime;
                        cmbShipmentID.Items.Refresh();
                    }
                    catch (Exception)
                    { }
                }
                else
                {
                    DateTime FromDate = Convert.ToDateTime(dtpFromDate.Text);
                    DateTime ToDate = Convert.ToDateTime(dtpToDate.Text);
                    Guid UserID =(Guid) cmbUserNames.SelectedValue;
                    List<cstPackingTime> lspackingTime = Global.controller.GetPackingTimeQuantity_ByStation(UserID, FromDate, ToDate, StationID);
                    dgvShipmentPackedList.ItemsSource = lspackingTime;
                    dgvShipmentPackedList.Items.Refresh();
                    SetLabels(lspackingTime);
                    //combox bind
                    cmbShipmentID.ItemsSource = lspackingTime;
               
                    cmbShipmentID.Items.Refresh();
                }
            }
            else
            {
                int PackingStatus = Convert.ToInt32(cmbShipmentStatus.SelectedValue.ToString());

                if (dtpFromDate.Text.ToString() == "" || dtpToDate.Text == "")
                {
                    try
                    {
                        Guid UserID =(Guid) cmbUserNames.SelectedValue;
                        List<cstPackingTime> lspackingTime = Global.controller.GetPackingTimeQuantity_ByStation(UserID, PackingStatus, StationID);
                        dgvShipmentPackedList.ItemsSource = lspackingTime;
                        dgvShipmentPackedList.Items.Refresh();
                        SetLabels(lspackingTime);
                        //combox bind
                        cmbShipmentID.ItemsSource = lspackingTime;
                   
                        cmbShipmentID.Items.Refresh();
                    }
                    catch (Exception)
                    { }
                }
                else
                {
                    DateTime FromDate = Convert.ToDateTime(dtpFromDate.Text);
                    DateTime ToDate = Convert.ToDateTime(dtpToDate.Text);
                    Guid UserID = (Guid)cmbUserNames.SelectedValue;
                    List<cstPackingTime> lspackingTime = Global.controller.GetPackingTimeQuantity_ByStation(UserID, FromDate, ToDate, PackingStatus, StationID);
                    dgvShipmentPackedList.ItemsSource = lspackingTime;
                    dgvShipmentPackedList.Items.Refresh();
                    SetLabels(lspackingTime);
                    //combox bind
                    cmbShipmentID.ItemsSource = lspackingTime;
               
                    cmbShipmentID.Items.Refresh();
                }
            }
            //if (Global.newWindowThread.IsAlive)
            //{
            //    Global.newWindowThread.Abort();
            //}
        }

        private void dtpDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)

        {
           // WindowThread.start();
            lblCShipmentID.Visibility = System.Windows.Visibility.Hidden;
            lblShipmentID.Visibility = System.Windows.Visibility.Hidden;
            cmbShipmentID.Visibility = System.Windows.Visibility.Hidden;

            ClearShipmentDetailGrid();
            if (cmbShipmentStatus.Text == "")
            {
                try
                {


                    if (cmbUserNames.Text != "")
                    {
                        DateTime FromDate = Convert.ToDateTime(dtpFromDate.Text);
                        DateTime ToDate = Convert.ToDateTime(dtpToDate.Text);
                        Guid UserID =(Guid)cmbUserNames.SelectedValue;
                        List<cstPackingTime> lspackingTime = Global.controller.GetPackingTimeQuantity_ByStation(UserID, FromDate, ToDate, StationID);
                        dgvShipmentPackedList.ItemsSource = lspackingTime;
                        dgvShipmentPackedList.Items.Refresh();
                        SetLabels(lspackingTime);
                        //combox bind
                        cmbShipmentID.ItemsSource = lspackingTime;
                     
                        cmbShipmentID.Items.Refresh();
                    }
                    else
                    {
                        DateTime FromDate = Convert.ToDateTime(dtpFromDate.Text);
                        DateTime ToDate = Convert.ToDateTime(dtpToDate.Text);
                        long UserID = Convert.ToInt64(cmbUserNames.SelectedValue);
                        List<cstPackingTime> lspackingTime = Global.controller.GetPackingTimeQuantity_ByStation(FromDate, ToDate, StationID);
                        dgvShipmentPackedList.ItemsSource = lspackingTime;
                        dgvShipmentPackedList.Items.Refresh();
                        SetLabels(lspackingTime);
                        //combox bind
                        cmbShipmentID.ItemsSource = lspackingTime;
                   
                        cmbShipmentID.Items.Refresh();
                    }
                }

                catch (Exception)
                {
                }
            }
            else
            {
                int PackingStatus = Convert.ToInt32(cmbShipmentStatus.SelectedValue.ToString());
                try
                {


                    if (cmbUserNames.Text != "")
                    {
                        DateTime FromDate = Convert.ToDateTime(dtpFromDate.Text);
                        DateTime ToDate = Convert.ToDateTime(dtpToDate.Text);
                        Guid UserID = (Guid)cmbUserNames.SelectedValue;
                        List<cstPackingTime> lspackingTime = Global.controller.GetPackingTimeQuantity_ByStation(UserID, FromDate, ToDate, PackingStatus, StationID);
                        dgvShipmentPackedList.ItemsSource = lspackingTime;
                        dgvShipmentPackedList.Items.Refresh();
                        SetLabels(lspackingTime);
                        //combox bind
                        cmbShipmentID.ItemsSource = lspackingTime;
                   
                        cmbShipmentID.Items.Refresh();
                    }
                    else
                    {
                        DateTime FromDate = Convert.ToDateTime(dtpFromDate.Text);
                        DateTime ToDate = Convert.ToDateTime(dtpToDate.Text);
                        long UserID = Convert.ToInt64(cmbUserNames.SelectedValue);
                        List<cstPackingTime> lspackingTime = Global.controller.GetPackingTimeQuantity_ByStation(FromDate, ToDate, PackingStatus, StationID);
                        dgvShipmentPackedList.ItemsSource = lspackingTime;
                        dgvShipmentPackedList.Items.Refresh();
                        SetLabels(lspackingTime);
                        //combox bind
                        cmbShipmentID.ItemsSource = lspackingTime;
                   
                        cmbShipmentID.Items.Refresh();
                    }
                }

                catch (Exception)
                {
                }

            }
            //if (Global.newWindowThread.IsAlive)
            //{
            //    Global.newWindowThread.Abort();
            //}
        }

        private void dgvShipmentPackedList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
               // WindowThread.start();
                cstPackingTime packingItem = (cstPackingTime)dgvShipmentPackedList.SelectedItem;
                List<cstPackageDetails> lsPackingDetails = Global.controller.GetPackingDetailTbl(packingItem.PackingID);

              //////16-01-2015
                List<cstPackageDetails> lsPackingDetailsForGridBind = Global.controller.GetPackingDetailsByBOX(packingItem.PackingID);

                dgvPackedDetailsList.ItemsSource = lsPackingDetailsForGridBind;
                dgvPackedDetailsList.Items.Refresh();
                lblDShipmentID.Content = Global.controller.GetShippingNumber(lsPackingDetails[0].PackingId);
                lblDStatus.Content = "Packed";
                TimeSpan span;
                if (TimeSpan.TryParse(packingItem.TimeSpend.Replace("M", "").Replace("H", "").Replace("S", ""), out span))

                    lblDTime.Content = String.Format("{0:D2}min {1:D2}sec", span.Minutes, span.Seconds);
                lblDItem.Content = packingItem.Quantity.ToString();
                String Location = lsPackingDetails[0].ShipmentLocation.ToString();
                String UserName = Global.controller.GetSelcetedUserMaster((Global.controller.GetPackingTbl().SingleOrDefault(i=> i.PackingId==lsPackingDetails[0].PackingId)).UserID).First().UserFullName.ToString();

                int MangerOverride = Global.controller.GetPackingTbl().SingleOrDefault(i => i.PackingId == lsPackingDetails[0].PackingId).MangerOverride;
                lblDoverride.Content = "No";
                if (MangerOverride ==1)
                {
                    lblDoverride.Content = "Manager";    
                }
                else if(MangerOverride ==2)
                {
                    lblDoverride.Content = "Self";
                }

                foreach(var LocationItem in lsPackingDetails)
                {
                    lblDUserName.Content = UserName;
                    lblDLocation.Content = Location;
                    if (Location != LocationItem.ShipmentLocation)
                    {
                        Location = LocationItem.ShipmentLocation;
                        UserName = Global.controller.GetSelcetedUserMaster((Global.controller.GetPackingTbl().SingleOrDefault(i => i.PackingId == lsPackingDetails[0].PackingId)).UserID).First().UserFullName.ToString();
                        lblDLocation.Content = Location + " & " + lblDLocation.Content;
                        lblDUserName.Content = UserName + " & " + lblDUserName.Content;
                    }
                }
                //if (Global.newWindowThread.IsAlive)
                //{
                //    Global.newWindowThread.Abort();
                //}
            }
            catch (Exception)
            { }
        }

        private void cmbShipmentID_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           // WindowThread.start();
            lblCShipmentID.Visibility = cmbShipmentID.SelectedItem == null ? Visibility.Visible : Visibility.Hidden;
            lblUserName.Visibility = System.Windows.Visibility.Hidden;
            cmbUserNames.Visibility = System.Windows.Visibility.Hidden;
            lblCUserName.Visibility = System.Windows.Visibility.Hidden;
            lblCShipmentStaus.Visibility = System.Windows.Visibility.Hidden;
            lblStatus.Visibility = System.Windows.Visibility.Hidden;
            cmbShipmentStatus.Visibility = System.Windows.Visibility.Hidden;
            lblFrom.Visibility = System.Windows.Visibility.Hidden;
            lblTo.Visibility = System.Windows.Visibility.Hidden;
            dtpFromDate.Visibility = System.Windows.Visibility.Hidden;
            dtpToDate.Visibility = System.Windows.Visibility.Hidden;
            cstPackingTime cmbItem = (cstPackingTime)cmbShipmentID.SelectedItem;
            List<cstPackingTime> lsNpackingTime = new List<cstPackingTime>();
            List<cstPackingTime> lspackingTime = Global.controller.GetPackingTimeQuantity_ByStation(StationID);
            foreach (cstPackingTime t in lspackingTime)
            {
                if (t.ShippingNumber == cmbItem.ShippingNumber)
                {
                    lsNpackingTime.Add(t);
                }
            }

            cstPackingTime packingItem = (cstPackingTime)cmbShipmentID.SelectedItem;
            List<cstPackageDetails> lsPackingDetails = Global.controller.GetPackingDetailTbl(packingItem.PackingID);
            dgvPackedDetailsList.ItemsSource = lsPackingDetails;
            dgvPackedDetailsList.Items.Refresh();

            

          //  dgvShipmentPackedList.ItemsSource = lsNpackingTime;
           // dgvShipmentPackedList.Items.Refresh();

            lblDShipmentID.Content = Global.controller.GetShippingNumber(lsPackingDetails[0].PackingId);
            lblDStatus.Content = "Packed";
            TimeSpan span;
            if (TimeSpan.TryParse(packingItem.TimeSpend.Replace("M", "").Replace("H", "").Replace("S", ""), out span))

                lblDTime.Content = String.Format("{0:D2}min {1:D2}sec", span.Minutes, span.Seconds);
            lblDItem.Content = packingItem.Quantity.ToString();
            String Location = lsPackingDetails[0].ShipmentLocation.ToString();
            String UserName = Global.controller.GetSelcetedUserMaster((Global.controller.GetPackingTbl().SingleOrDefault(i => i.PackingId == lsPackingDetails[0].PackingId)).UserID).First().UserFullName.ToString();

            int MangerOverride = Global.controller.GetPackingTbl().SingleOrDefault(i => i.PackingId == lsPackingDetails[0].PackingId).MangerOverride;
            lblDoverride.Content = "No";
            if (MangerOverride == 1)
            {
                lblDoverride.Content = "Manager";
            }
            else if (MangerOverride == 2)
            {
                lblDoverride.Content = "Self";
            }

            foreach (var LocationItem in lsPackingDetails)
            {
                lblDUserName.Content = UserName;
                lblDLocation.Content = Location;
                if (Location != LocationItem.ShipmentLocation)
                {
                    Location = LocationItem.ShipmentLocation;
                    UserName = Global.controller.GetSelcetedUserMaster((Global.controller.GetPackingTbl().SingleOrDefault(i => i.PackingId == lsPackingDetails[0].PackingId)).UserID).First().UserFullName.ToString();
                    lblDLocation.Content = Location + " & " + lblDLocation.Content;
                    lblDUserName.Content = UserName + " & " + lblDUserName.Content;
                }
            }
            //if (Global.newWindowThread.IsAlive)
            //{
            //    Global.newWindowThread.Abort();
            //}

        }

        private void cmbShipmentStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

          //  WindowThread.start();
            lblCShipmentID.Visibility = System.Windows.Visibility.Hidden;
            lblShipmentID.Visibility = System.Windows.Visibility.Hidden;
            cmbShipmentID.Visibility = System.Windows.Visibility.Hidden;

            ClearShipmentDetailGrid();
            lblCShipmentStaus.Visibility = cmbShipmentStatus.SelectedItem == null ? Visibility.Visible : Visibility.Hidden;
            int PackingStatus = Convert.ToInt32(cmbShipmentStatus.SelectedValue.ToString());

            if ((cmbShipmentStatus.Text != "" && dtpFromDate.Text != "") && dtpToDate.Text != "")
            {
                DateTime FromDate = Convert.ToDateTime(dtpFromDate.Text);
                DateTime ToDate = Convert.ToDateTime(dtpToDate.Text);
                Guid UserID = Guid.Empty;
                Guid.TryParse (cmbUserNames.SelectedValue.ToString(), out UserID);
                List<cstPackingTime> lspackingTime = Global.controller.GetPackingTimeQuantity_ByStation(UserID, FromDate, ToDate, PackingStatus, StationID);
                dgvShipmentPackedList.ItemsSource = lspackingTime;
                dgvShipmentPackedList.Items.Refresh();
                SetLabels(lspackingTime);
                //combox bind
                cmbShipmentID.ItemsSource = lspackingTime;
           
                cmbShipmentID.Items.Refresh();
            }
            else if ((dtpFromDate.Text != "" && dtpToDate.Text != "") && cmbUserNames.Text == "")
            {
                DateTime FromDate = Convert.ToDateTime(dtpFromDate.Text);
                DateTime ToDate = Convert.ToDateTime(dtpToDate.Text);
                List<cstPackingTime> lspackingTime = Global.controller.GetPackingTimeQuantity_ByStation(FromDate, ToDate, PackingStatus,StationID);
                dgvShipmentPackedList.ItemsSource = lspackingTime;
                dgvShipmentPackedList.Items.Refresh();
                SetLabels(lspackingTime);
                //combox bind
                cmbShipmentID.ItemsSource = lspackingTime;
                cmbShipmentID.Items.Refresh();
            }
            else if ((dtpFromDate.Text == "" || dtpToDate.Text == "") && cmbUserNames.Text == "")
            {
                List<cstPackingTime> lspackingTime = Global.controller.GetPackingTimeQuantity_ByStation(PackingStatus, true, StationID);
                dgvShipmentPackedList.ItemsSource = lspackingTime;
                dgvShipmentPackedList.Items.Refresh();
                SetLabels(lspackingTime);
                //combox bind
                cmbShipmentID.ItemsSource = lspackingTime;
           
                cmbShipmentID.Items.Refresh();
            }
            else if ((dtpFromDate.Text == "" || dtpToDate.Text == "") && cmbUserNames.Text != "")
            {
                Guid UserID = Guid.Empty;
                Guid.TryParse(cmbUserNames.SelectedValue.ToString(), out UserID);
                List<cstPackingTime> lspackingTime = Global.controller.GetPackingTimeQuantity_ByStation(UserID, PackingStatus, StationID);
                dgvShipmentPackedList.ItemsSource = lspackingTime;
                dgvShipmentPackedList.Items.Refresh();
                SetLabels(lspackingTime);
                //combox bind
                cmbShipmentID.ItemsSource = lspackingTime;
           
                cmbShipmentID.Items.Refresh();
            }
            //if (Global.newWindowThread.IsAlive)
            //{
            //    Global.newWindowThread.Abort();
            //}
        }

        private void ClearShipmentDetailGrid()
        {
            try
            {
                lblDShipmentID.Content = "";
                lblDStatus.Content = "";
                lblDTime.Content = "";
                lblDItem.Content = "";
                lblDUserName.Content = "";
                lblDLocation.Content = "";
                lblDoverride.Content = "";
                lblCShipmentID.Visibility = Visibility.Hidden;
                List<cstPackageDetails> lsPackingDetails = new List<cstPackageDetails>();
                dgvPackedDetailsList.ItemsSource = lsPackingDetails;
                dgvPackedDetailsList.Items.Refresh();
            }
            catch (Exception)
            { }
        }

    }


    class PackingStat
    {
        public int ID { get; set; }
        public string Status { get; set; }
    }

}
