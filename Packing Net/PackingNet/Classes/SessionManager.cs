using Packing_Net.Pages;
using PackingClassLibrary.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
namespace Packing_Net.Classes
{
    /// <summary>
    /// Author: Avinash.
    /// Version: Alfas
    /// Session Manager manges the session in the application.
    /// </summary>
   public static class SessionManager
    {
       public static DispatcherTimer Autotimer = new DispatcherTimer();

       /// <summary>
       /// Session initialized to the application specified time and strated.
       /// </summary>
       public static void StartTime()
       {
           try
           {
               Global.TimeOutUserName = Global.LoggedUserModel.UserInfo.UserName;
               Global.ISTimerRaised = false;
               int _Hour = Convert.ToInt32(GetHrorMinorSec("HH"));
               int _Min = Convert.ToInt32(GetHrorMinorSec("MM"));
               int _Sec = Convert.ToInt32(GetHrorMinorSec("SS"));

               //AtutoTimer
               if (Autotimer.IsEnabled)
               {
                   Autotimer.Stop();
               }
               Autotimer.Interval = new TimeSpan(_Hour, _Min, _Sec);
               Autotimer.Start();
           }
           catch (Exception Ex)
           {
               ErrorLoger.save("Classes\\AutologoutTime.StartTime()", Ex.InnerException.ToString());
           }
       }

       /// <summary>
       /// Avinash
       /// Split the Time returned from locationsetting File in to hours , Min, Sec 
       /// </summary>
       /// <param name="returnHHorMMorSS">String maintion HH to return Hour value same for Min-MM and for Sec value =SS</param>
       /// <returns>String depending on selection</returns>
       public static string GetHrorMinorSec(String returnHHorMMorSS)
       {
           String _retuen = "";
           try
           {
               String _Time = cmdLocalFile.ReadString("LogoutTime");
               if (_Time != null || _Time != "")
               {
                   var Str = _Time.Split(new Char[] { ':' });
                   switch (returnHHorMMorSS)
                   {
                       case "HH":
                           _retuen = Str[0];
                           break;
                       case "MM":
                           _retuen = Str[1];
                           break;
                       case "SS":
                           _retuen = Str[2];
                           break;
                       default:
                           break;
                   }
               }
           }
           catch (Exception Ex ) 
           {
               ErrorLoger.save("Classes\\AutoLogoutTime.GetHrorMinOrSec()", Ex.InnerException.ToString());
           }
           return _retuen;
       }

       
    }
}
