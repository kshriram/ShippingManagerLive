using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackingClassLibrary.Commands
{
    /// <summary>
    /// Author: Avinash
    /// Version: Alfa.
    /// UPC code Converter From and To sku Name
    /// </summary>
   public static class cmdUPCConverter
    {
       /// <summary>
       /// Convert UPC Code to the SKu name.
       /// </summary>
       /// <param name="UPCCode">11 or 12 Digit UPC Code.</param>
       /// <returns>String of Sku Name in error blank string.</returns>
       public static String UPCCodeToSKU(String UPCCode)
       {
            Sage_x3v6Entities Sage = new Sage_x3v6Entities();
           String _return = "";
           try
           {
               _return= Sage.ITMMASTERs.SingleOrDefault(i => i.EANCOD_0 == UPCCode).ITMDES1_0.ToString();
           }
           catch (Exception Ex)
           {
               Error_Loger.elAction.save("UPCCodeTOSKUName.UPCCodeToSKU()", Ex.Message.ToString());
           }
           return _return;
       }

       /// <summary>
       /// SKU Name to its UPC Code.
       /// </summary>
       /// <param name="SKUName">String SKU Name.</param>
       /// <returns>String UPC 13 Digit code in error "000000000000" code</returns>
       public static String SKUNameToSku(String SKUName)
       {
            Sage_x3v6Entities Sage = new Sage_x3v6Entities();
           string UPCACode = "000000000000";
           
           try
           {
              var vUPCACode = Sage.ExecuteStoreQuery<String>(@"SELECT TOP 1 [ITMMASTER].[EANCOD_0] AS UPCCode FROM [PRODUCTION].[ITMMASTER] WHERE [ITMMASTER].[ITMDES1_0] ='" + SKUName + "';").ToList();
             //  var vUPCACode = Sage.PRODUCTION.ITMMASTERs.SingleOrDefault(i => i.ITMDES1_0 == SKUName).EANCOD_0.ToList();

               if (vUPCACode.Count() == 0)
               {
                   var vUPCACode1 = Sage.ExecuteStoreQuery<String>(@"SELECT TOP 1  [ITMMASTER].[EANCOD_0] AS UPCCode
                                                                FROM [PRODUCTION].[SDELIVERYD]
                                                                  INNER JOIN [PRODUCTION].[SDELIVERY] ON [SDELIVERYD].[SDHNUM_0] = [SDELIVERY].[SDHNUM_0]
                                                                  LEFT JOIN [PRODUCTION].[ITMMASTER] ON [ITMMASTER].[ITMREF_0] = [SDELIVERYD].[ITMREF_0]
                                                                  INNER JOIN [PRODUCTION].[STOJOU] ON [STOJOU].[VCRNUM_0] = [SDELIVERY].[SDHNUM_0] AND [STOJOU].[ITMREF_0] = [ITMMASTER].[ITMREF_0] 
	                                                              INNER JOIN [PRODUCTION].[SORDER] ON [SORDER].[SOHNUM_0] = [SDELIVERY].[SOHNUM_0]
	                                                              WHERE [SDELIVERY].[SOHNUM_0] NOT IN(SELECT DISTINCT SORDER.SOHNUM_0 AS OrderID
                                                                  FROM PRODUCTION.SORDER INNER JOIN 
	                                                              PRODUCTION.SORDERP ON PRODUCTION.SORDER.SOHNUM_0 = PRODUCTION.SORDERP.SOHNUM_0 INNER JOIN
                                                                  PRODUCTION.SORDERQ ON PRODUCTION.SORDER.SOHNUM_0 = PRODUCTION.SORDERQ.SOHNUM_0
                                                                  AND PRODUCTION.SORDERP.LINTYP_0 <> 7 AND PRODUCTION.SORDERP.SOPLIN_0 = PRODUCTION.SORDERQ.SOPLIN_0 
                                                                  WHERE CASE WHEN (SORDER.ORDSTA_0 = 2 AND SORDERQ.SDHNUM_0 = '' AND SORDER.CCLREN_0 = 'CUS') THEN 'Order Cancelled'
                                                                  WHEN (SORDER.XB_HLDSTA_0 = 3) THEN 'Order on Hold' END IS NOT NULL)
                                                           AND [SDELIVERYD].[ITMDES1_0]  ='" + SKUName + "';").ToList();
                   foreach (var vitem in vUPCACode1)
                   {
                       long n;
                       UPCACode = vitem.ToString();
                       if ((long.TryParse(UPCACode, out n)) == false)
                       {
                           UPCACode = "000000000000";
                       }
                   }
               }
               else
               {
                   UPCACode = "";
                   foreach (var vitem in vUPCACode)
                   {
                       long n;
                       UPCACode =UPCACode+ vitem.ToString();
                       if ((long.TryParse(UPCACode, out n)) == false)
                       {
                           UPCACode = "000000000000";
                       }
                   }
               }
           }
           catch (Exception)
           {}
           return UPCACode;
       }
    }
}
