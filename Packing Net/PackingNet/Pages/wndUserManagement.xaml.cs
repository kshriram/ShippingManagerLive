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
using System.Windows.Shapes;
using Packing_Net.Classes;
using PackingClassLibrary.CustomEntity;
using PackingClassLibrary;
using System.Windows.Threading;
namespace Packing_Net.Pages
{
    /// <summary>
    /// Interaction logic for UserManagement.xaml
    /// </summary>
    public partial class UserManagement : Window
    {
        public UserManagement()
        {
            InitializeComponent();
           
            //Sow UserName at the Top
            lblUserTop.Content = Global.WindowTopUserName;
            //fill user list at the left
            ShowUserListLeft();

            
        }

        private void ShowUserListLeft()
        {
            try
            {
                
               var _lsUserInfo =Global.controller.GetUserInfoList();
               List<string> _lsUserFullName = new List<string>();
               grdUserList.ItemsSource = _lsUserInfo;

            }
            catch (Exception Ex)
            {
                ErrorLoger.save("UserManagement.ShowUserListLeft()", Ex.Message.ToString());
            }
        }

        private void btnNewUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //log 
                SaveUserLogsTbl.logThis(csteActionType.UserManagement_NewUserClicked.ToString());
                NewUserRegistration _pgNewUser = new NewUserRegistration();
                frmMain.Content = _pgNewUser;
            }
            catch (Exception Ex)
            {
                ErrorLoger.save("UserManagement.btnNewUser_Click()", Ex.Message.ToString());
            }
        }

        private void btnSelectUsre_Click(object sender, RoutedEventArgs e)
        {
            Button btnShowUserinfo = (Button)e.Source;
            DataGridRow _Row = btnShowUserinfo.TemplatedParent.FindParent<DataGridRow>();
            TextBlock txtUserID = grdUserList.Columns[0].GetCellContent(_Row) as TextBlock;
            Guid TGuid = Guid.Empty;
            Guid.TryParse(txtUserID.Text, out TGuid);
            UserInfoPage.UserID = TGuid;
            PgUsedInfoDisplay _UserInfoPage = new PgUsedInfoDisplay();
            frmMain.Content = _UserInfoPage;
        }

        

        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //log 
                SaveUserLogsTbl.logThis(csteActionType.UserManagement_HomeClicked.ToString());

                HomeScreen _home = new HomeScreen();
                _home.Show();
                this.Close();

            }
            catch (Exception Ex)
            {
                ErrorLoger.save("UserManagement.btnHome_Click()", Ex.Message.ToString());
            }
        }

        private void Label_TouchDown_1(object sender, TouchEventArgs e)
        {
           
        }

        private void lblRole_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            frmMain.Navigate(new RoleUI());
        }

        /// <summary>
        /// Open role page.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRole_Click(object sender, RoutedEventArgs e)
        {
            //log 
            SaveUserLogsTbl.logThis(csteActionType.UserManagement_NewRoleClicked.ToString());
            frmMain.Navigate(new RoleUI());
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            //log 
            SaveUserLogsTbl.logThis(csteActionType.UserManagement_ScreenLoaded.ToString());
            //Auto Timer Restart..
            SessionManager.StartTime();

            //Convert To Language
            WindowLanguages.Convert(this);
        }

        private void grdUserList_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            grdUserList.Items.Refresh();
        }
    }
}
