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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Packing_Net.Classes;
using PackingClassLibrary.CustomEntity;

namespace Packing_Net.Pages
{
    /// <summary>
    /// Interaction logic for NewUserRegistration.xaml
    /// </summary>
    public partial class NewUserRegistration : Page
    {
        
        /// <summary>
        /// permission set
        /// </summary> 
        string _permissionSet;

        /// <summary>
        /// Initialize
        /// </summary>
        public NewUserRegistration()
        {
            InitializeComponent();
            fillRole();
            _permissionSet = "";
         
        }

        /// <summary>
        /// fill role on new user page.
        /// </summary>
        public void fillRole()
        {
            List<cstRoleTbl> keyValue = Global.controller.GetRole();
            cmbRole.ItemsSource = keyValue;
        }
        
       
        /// <summary>
        /// Iterator for getting permission sets.
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="index"></param>
        public void iteratePermissionSet(Grid grid, int index)
        {
            if (grid.Children.Count  == index)
            {
                _permissionSet = _permissionSet.Remove(_permissionSet.Length - 1);
                return;
            }
            else
            {
                if (grid.Children[index] is CheckBox)
                {
                    CheckBox checkBox = grid.Children[index] as CheckBox;
                    _permissionSet = _permissionSet + checkBox.IsChecked.ToString() + "-" ;
                }
                iteratePermissionSet(grid, ++index);              
            }
        }

       
        

        /// <summary>
        /// Set all permissions sets
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="action"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public void iteratePermissionSet(Grid grid,string[] action, int index)
        {
            if (grid.Children.Count == index)
            {                
                return;
            }
            else
            {
                if (grid.Children[index] is CheckBox)
                {
                    CheckBox checkBox = grid.Children[index] as CheckBox;
                    checkBox.IsChecked = Boolean.Parse(action[index]);
                }
                iteratePermissionSet(grid, action, ++index);
            }            
        }
        private void cmbRole_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            txtSelectTeam.Visibility = cmbRole.SelectedItem == null ? Visibility.Visible : Visibility.Hidden;
        }

        private void Page_Loaded_1(object sender, RoutedEventArgs e)
        {
           // ControlConvertToLanguages.LabelConvert(this);
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<cstUserMasterTbl> _lsUserInformation = new List<cstUserMasterTbl>();
                cstUserMasterTbl _UserMasterTbl = new cstUserMasterTbl();
                //add list values to the custom class boject
                _UserMasterTbl.UserID = Guid.Empty;
                _UserMasterTbl.UserName =txtUserName.Text; 
                _UserMasterTbl.UserAddress = txtAddress.Text;
                _UserMasterTbl.Password = txtPass.Password.ToString();
                _UserMasterTbl.JoiningDate = Convert.ToDateTime(dtpJoiningDate.Text);
                _UserMasterTbl.UserFullName = txtFullName.Text;
                cstRoleTbl RoleItem = (cstRoleTbl)cmbRole.SelectedItem;
                _UserMasterTbl.Role = RoleItem.RoleId;
                _lsUserInformation.Add(_UserMasterTbl);
                if (Global.controller.SetUserMaster(_lsUserInformation))
                {
                    MsgBox.Show("Ok", "Save", "Save successfully!");
                    clearAll();
                }
                
            }
            catch (Exception)
            {}
        }


        public void clearAll()
        {
            txtUserName.Clear();
            txtAddress.Clear();
            txtConfirmPass.Clear();
            txtFullName.Clear();
            txtPass.Clear();
            cmbRole.SelectedIndex = -1;
            cmbRole.Text = "";
            txtSelectTeam.Visibility = System.Windows.Visibility.Visible;
            txtSelectTeam.Text = "-- Select Designation --";
            dtpJoiningDate.Text = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time").ToShortDateString();

           
        }

        private void txtConfirmPass_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtPass.Password .ToString() == txtConfirmPass.Password.ToString())
            {
                imgCpassword.Visibility = System.Windows.Visibility.Visible;
                imgRpassword.Visibility = System.Windows.Visibility.Hidden;
            }
            else
            {
                imgCpassword.Visibility = System.Windows.Visibility.Hidden;
                imgRpassword.Visibility = System.Windows.Visibility.Visible;
                txtConfirmPass.Clear();
                txtPass.Clear();
            }
        }

    }
    class Station
    {
        public int StationID { get; set; }
        public String StationName { get; set; }
    }
}
