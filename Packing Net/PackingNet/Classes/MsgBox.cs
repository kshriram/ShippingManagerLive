using Packing_Net.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Packing_Net.Classes
{
   public static class MsgBox
    {
       public static void Show(String MessageBoxType, String MessageBoxTitle, String MessageBoxBody)
       {
           Global.MsgBoxType = MessageBoxType;
           Global.MsgBoxTitle = Global.controller.ConvetLanguage(MessageBoxTitle.Trim(), Global.LanguageFileName);
           Global.MsgBoxMessage = Global.controller.ConvetLanguage(MessageBoxBody.Trim(), Global.LanguageFileName);
           Umsgbox ShowMsgBox = new Umsgbox();
           ShowMsgBox.ShowDialog();
       }
    }
}
