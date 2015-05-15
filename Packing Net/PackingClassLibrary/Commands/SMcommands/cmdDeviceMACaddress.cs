using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace PackingClassLibrary.Commands.SMcommands
{
   public class cmdDeviceMACaddress
    {
        /// <summary>
        /// Returns Devide Unique ID(MAC Address)
        /// </summary>
        /// <returns>String DeviceID</returns>
        public String DeviceNumber()
        {
            String _return = "";
            try
            {
                NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
                String sMacAddress = string.Empty;
                foreach (NetworkInterface adapter in nics)
                {
                    if (sMacAddress == String.Empty)// only return MAC Address from first card  
                    {
                        IPInterfaceProperties properties = adapter.GetIPProperties();
                        sMacAddress = adapter.GetPhysicalAddress().ToString();
                    }
                }
                _return = sMacAddress;

            }
            catch (Exception)
            { }
            return _return;
        }
    }
}
