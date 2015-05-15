using PackingClassLibrary.CustomEntity;
using PackingClassLibrary.CustomEntity.SMEntitys;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace Packing_Net.Classes
{
   public class PrintSlip
    {
        String PackingNumber;
        String ShippingNumber;
        String BoxNumber;

        System.Drawing.Image imgBoxNumber;
        System.Drawing.Image imgPackingNumber;
        System.Drawing.Image imgShippingNumber;
        #region Print packing slip
        /// <summary>
        /// Print packing slip of barcode that has packing number and its respective shipping number;
        /// </summary>
        public void PrintPckingSlip(Guid BoxID)
        {
            try
            {
                cstBoxPackage _boxInfo = Global.controller.GetBoxPackageByBoxID(BoxID);
                BoxNumber = _boxInfo.BOXNUM;
                cstPackageTbl packing = Global.controller.GetPackingTbl().SingleOrDefault(i => i.PackingId == _boxInfo.PackingID);
                ShippingNumber = packing.ShippingNum;

                BarcodeLib.Barcode b = new BarcodeLib.Barcode();

                PackingNumber = packing.PCKROWID;

                try
                {
                    imgPackingNumber = b.Encode(BarcodeLib.TYPE.CODE128, packing.PCKROWID, System.Drawing.Color.Black, System.Drawing.Color.White, 300, 60);
                    imgShippingNumber = b.Encode(BarcodeLib.TYPE.CODE128, ShippingNumber, System.Drawing.Color.Black, System.Drawing.Color.White, 300, 60);
                    imgBoxNumber = b.Encode(BarcodeLib.TYPE.CODE128, BoxNumber, System.Drawing.Color.Black, System.Drawing.Color.White, 300, 60);
                }
                catch (Exception Ex)
                {
                    //Log the Error to the Error Log table
                    ErrorLoger.save("wndShipmentDetailPage - PrintPckingSlip_sub1", "[" + DateTime.UtcNow.ToString() + "]" + Ex.ToString(), DateTime.UtcNow, Global.LoggedUserId);
                }
                PrintDocument pd = new PrintDocument();
                pd.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);
                pd.OriginAtMargins = false;
                pd.DefaultPageSettings.Landscape = true;

                //Business card paper size
                pd.DefaultPageSettings.PaperSize = new PaperSize("BC", 330, 220);
                pd.PrintPage += new PrintPageEventHandler(pd_PrintPage);
                pd.Print();
            }
            catch (Exception Ex)
            {
                //Log the Error to the Error Log table
                ErrorLoger.save("wndShipmentDetailPage - PrintPckingSlip", "[" + DateTime.UtcNow.ToString() + "]" + Ex.ToString(), DateTime.UtcNow, Global.LoggedUserId);
            }
        }

        /// <summary>
        /// Adjest Print Page settinges here.
        /// Add Bodcodes, Lines spaces etc
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void pd_PrintPage(object sender, PrintPageEventArgs e)
        {
            try
            {
                System.Drawing.Font drawFont = new System.Drawing.Font("Arial", 10);
                System.Drawing.SolidBrush drawBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);

                //Box Number Image Borcode
                e.Graphics.DrawImage(imgBoxNumber, 10, 10, (float)200, 50);
                //Box Number
                e.Graphics.DrawString(BoxNumber.ToString(), drawFont, drawBrush, (float)50, (float)60.5);

                //packing Number and Barcode
                e.Graphics.DrawImage(imgPackingNumber, 10, 100, (float)200, (float)50);
                //Packing Number String
                e.Graphics.DrawString(PackingNumber.ToString(), drawFont, drawBrush, (float)50, (float)150.5);

                //shipment Number and its barcode
                e.Graphics.DrawImage(imgShippingNumber, 10, 190, (float)200, (float)50);
                //Shipment Number String
                e.Graphics.DrawString(ShippingNumber, drawFont, drawBrush, (float)50, (float)240.5);

                //Border to paper boundry
                System.Drawing.Pen pen = new System.Drawing.Pen(drawBrush);
                e.Graphics.DrawRectangle(pen, (float)0.0, (float)0.0, (float)288.0, (float)432.0);
            }
            catch (Exception Ex)
            {
                //Log the Error to the Error Log table
                ErrorLoger.save("wndShipmentDetailPage - pd_PrintPage", "[" + DateTime.UtcNow.ToString() + "]" + Ex.ToString(), DateTime.UtcNow, Global.LoggedUserId);
            }
        }
        #endregion

    }
}
