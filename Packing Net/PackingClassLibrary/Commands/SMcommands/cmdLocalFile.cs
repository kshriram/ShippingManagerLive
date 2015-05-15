using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace PackingClassLibrary.Commands
{
    public static class cmdLocalFile
    {

        /// <summary>
        /// Avinash
        /// Read the previously Stored setting From the file.
        /// </summary>
        /// <param name="WhtReturn">what u want to read Location or LogoutTime</param>
        /// <returns>Depending on selection it returns the String value</returns>
        public static string ReadString(String WhtReturn)
        {
            String _return = null;
            try
            {
                string Location = "";
                String Timestring = "";
                String Language = "English";
                String IsBarcodeShow="";
                String[] Lines = File.ReadAllLines(Environment.CurrentDirectory + "\\LocalSetting.txt");
                foreach (String line in Lines)
                {
                    var word = line.Split(new char[] { '#' });
                    Location = word[0].ToString();
                    Timestring = word[1].ToString();
                    Language = word[2].ToString ();
                    IsBarcodeShow=word[3].ToString();
                }
                switch (WhtReturn)
                {
                    case "Location":
                        _return = Location.ToString();
                        break;
                    case "LogoutTime":
                        _return = Timestring.ToString();
                        break;
                    case "Language":
                        _return = Language;
                        break;
                    case "ISBarcodeShow":
                        _return = IsBarcodeShow;
                        break;
                    default:
                        break;
                }
            }
            catch (Exception Ex)
            {
                Error_Loger.elAction.save("LocalSettingToFromFlatFile.ReadString()", Ex.Message.ToString());
            }
            return _return;
        }

        /// <summary>
        /// Avinash
        /// Weite paramenter values to the file Localsetting.txt;
        /// Overrite the previous Lines.
        /// </summary>
        /// <param name="Location">String variabel</param>
        /// <param name="AutoLogOutTime">String Veriabel</param>
        /// <returns>Boolean Value tru if no error else False</returns>
        public static Boolean WriteString(string Location, String AutoLogOutTime,String Language,String ISBarcodeShow)
        {
            Boolean _return = false;
            try
            {
                String _Line = Location + "#" + AutoLogOutTime + "#" + Language + "#" + ISBarcodeShow;
                File.WriteAllText(Environment.CurrentDirectory + "\\LocalSetting.txt", _Line);
                _return = true;

            }
            catch (Exception Ex)
            {
                Error_Loger.elAction.save("LocalSettingToFromFlatFile.WriteString()", Ex.Message.ToString());
            }
            return _return;
        }



    }
}
