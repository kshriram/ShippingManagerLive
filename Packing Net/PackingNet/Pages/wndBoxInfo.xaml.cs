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

namespace PackingNet.Pages
{
    /// <summary>
    /// Interaction logic for wndBoxInfo.xaml
    /// </summary>
    public partial class wndBoxInfo : Window
    {
        cmdCartonInfo carton = new cmdCartonInfo();
        List<cstCartonInfo> lscarton = new List<cstCartonInfo>();

      
        public wndBoxInfo()
        {
            InitializeComponent();
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            try
            {
                fillGrid(Global.ShippingNumber);
                txtBoxNumberScanned.Focus();
                txtWH.Text = CheckWH(Global.ShippingNumber).Trim();
            }
            catch (Exception Ex)
            {
                ErrorLoger.save("wndBoxInfo - Page_load", "[" + DateTime.UtcNow.ToString() + "]" + Ex.ToString(), DateTime.UtcNow, Global.LoggedUserId);
            }
           
        }

        public String CheckWH(string ShippingNumber)
        {
            String Retutn = "";
            Retutn = Global.controller.GetShippingInfoFromSage(ShippingNumber, "LTL").First().ToAddressLine3;
            return Retutn;
        }


        public void fillGrid(string ShipmentNum)
        {

            try
            {
                lscarton = carton.GetCartonByShipmentNumberAndLocation(ShipmentNum, cmdLocalFile.ReadString("Location"));
                List<Gridclass> lsgrid = new List<Gridclass>();
                foreach (var item in lscarton)
                {
                    Gridclass check = new Gridclass();
                    check.boxnumber = item.BOXNumber;
                    if (item.Printed == 0)
                        check.Status = "Not Printed";
                    else
                        check.Status = "Printed";
                    lsgrid.Add(check);
                }

                grdContent.ItemsSource = lsgrid;
                txtBoxNumberScanned.Focus();
            }
            catch (Exception Ex)
            {
                ErrorLoger.save("wndBoxInfo - FillGrid", "[" + DateTime.UtcNow.ToString() + "]" + Ex.ToString(), DateTime.UtcNow, Global.LoggedUserId);
            }

          
        }

        private void txtBoxNumberScanned_KeyDown(object sender, KeyEventArgs e)
        {

            try
            {
               
                if (txtBoxNumberScanned.Text.Trim() != "" && txtBoxNumberScanned.Text.Trim() != null && e.Key == Key.Enter)
                { 
                    Global.counter = 0;
                    List<cstPackageDetails> _packingDetails = Global.controller.GetPackingDetailTbl(txtBoxNumberScanned.Text);

                    foreach (var item in _packingDetails)
                    {
                        Global.BoxNumberScanned = txtBoxNumberScanned.Text;
                        wndWayfair wayFairlabel = new wndWayfair();
                        wayFairlabel.Show();
                        Global.counter = Global.counter + 1;
                    }


                    SavePrinted(txtBoxNumberScanned.Text);



                    this.Dispatcher.Invoke(new Action(() =>
                        {
                            foreach (DataGridRow row in GetDataGridRows(grdContent))
                            {
                                TextBlock txtBoxNum = grdContent.Columns[0].GetCellContent(row) as TextBlock;
                                if (txtBoxNum.Text == txtBoxNumberScanned.Text)
                                {
                                    TextBlock txtstatus = grdContent.Columns[1].GetCellContent(row) as TextBlock;
                                    txtstatus.Text = "Printed";
                                }
                            }
                        }));
                    txtBoxNumberScanned.Text = "";
                    if (CanClose())
                    {
                        this.Close();
                    }
                }
            }
            catch (Exception Ex)
            {
                ErrorLoger.save("wndBoxInfo - FillGrid", "[" + DateTime.UtcNow.ToString() + "]" + Ex.ToString(), DateTime.UtcNow, Global.LoggedUserId);
            }

           
        }
        public void SavePrinted(String BoxNumber)
        {
            try
            {
                cmdCartonInfo car = new cmdCartonInfo();
                cstCartonInfo _cartonBox = Global.controller.GetAllCartonInfoByBoxNumber(BoxNumber).FirstOrDefault();
                _cartonBox.Printed = 1;
                car.UpdateCartonInfo(_cartonBox);
            }
            catch (Exception Ex)
            {
                ErrorLoger.save("wndBoxInfo - SavePrinted", "[" + DateTime.UtcNow.ToString() + "]" + Ex.ToString(), DateTime.UtcNow, Global.LoggedUserId);
            }

        }

        private void btnAddNewBox_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

      public  class Gridclass
        {
            public string boxnumber { get; set; }
            public string Status { get; set; }
        
        }

         /// <summary>
        /// This is to all return DataGridRows Object
        /// </summary>
        /// <param name="grid"> Grid View object</param>
        /// <returns>DataGridRow</returns>
      public IEnumerable<DataGridRow> GetDataGridRows(DataGrid grid)
      {
          var itemsSource = grid.ItemsSource as IEnumerable;
          if (null == itemsSource) yield return null;
          foreach (var item in itemsSource)
          {
              var row = grid.ItemContainerGenerator.ContainerFromItem(item) as DataGridRow;
              if (null != row) yield return row;
          }
      }

      private void txtWH_TextChanged_1(object sender, TextChangedEventArgs e)
      {
          if (txtWH.Text.Trim() == "")
          {
              txtBoxNumberScanned.IsEnabled = false;
          }
          else
          {
              Global.WH = txtWH.Text;
              txtBoxNumberScanned.IsEnabled = true;
             // txtBoxNumberScanned.Text = "";
          }
      }


      public Boolean CanClose()
      {
          Boolean flag = false;
          try
          {
              int i = GetDataGridRows(grdContent).Count();
              int printedrow = 0;

              foreach (DataGridRow row in GetDataGridRows(grdContent))
              {
                  TextBlock txtBoxNum = grdContent.Columns[1].GetCellContent(row) as TextBlock;
                  if (txtBoxNum.Text.ToUpper() == "Printed".ToUpper())
                  {
                      printedrow++;
                  }
              }
              if (i == printedrow)
                  flag = true;
          }
          catch (Exception)
          {
          }
          return flag;
      }

      private void txtBoxNumberScanned_GotFocus(object sender, RoutedEventArgs e)
      {
          txtBoxNumberScanned.Text = "";
      }
     
    }
}
