using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PackingClassLibrary;

namespace Packing_Net.Classes
{
    /// <summary>
    /// Singleton Class.
    /// </summary>
    public static class SMController
    {
        static SMController(){}

        /// <summary>
        /// Singleton client object.
        /// </summary>
        public static void InitializeClientInstance()
        {
            Global.controller = new smController();
        }  
    }
}
