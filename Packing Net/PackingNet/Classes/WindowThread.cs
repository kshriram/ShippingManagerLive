using Packing_Net.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Packing_Net.Classes
{
    public static class WindowThread
    {
        /// <summary>
        /// Start new thread of the window in the application for wait screen.
        /// </summary>
        public static void start()
        {
          Global.newWindowThread = new Thread(new ThreadStart(() =>
            {
                // Create and show the Window
                wndWait tempWindow = new wndWait();
                tempWindow.Activate();
                tempWindow.Topmost = false;
                tempWindow.Focus();
                tempWindow.ShowActivated = true;
                tempWindow.Show();

                // Start the Dispatcher Processing
                System.Windows.Threading.Dispatcher.Run();
            }));
            // Set the apartment state
           Global.newWindowThread.SetApartmentState(ApartmentState.STA);
            // Make the thread a background thread
           Global.newWindowThread.IsBackground = true;
            // Start the thread
           Global.newWindowThread.Start();

        }

    }
}
