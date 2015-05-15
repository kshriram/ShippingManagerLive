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
using PackingClassLibrary;
using System.Text.RegularExpressions;
using Packing_Net.validations;
using PackingClassLibrary.Models;


namespace Packing_Net.Pages
{
    /// <summary>
    /// Interaction logic for PgUsedInfoDisplay.xaml
    /// </summary>
    public partial class PgUsedInfoDisplay : Page
    {
        public static Guid UserID = Guid.Empty;
        public PgUsedInfoDisplay()
        {
            InitializeComponent();
            fillRole();
        }

        public void fillRole()
        {
            List<cstRoleTbl> keyValue = Global.controller.GetRole();
            cmbRole.ItemsSource = keyValue;
        }
        
        private void cvsMain_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Guid _UserID = UserInfoPage.UserID;
                model_User User = new model_User(_UserID);
                List<cstUserMasterTbl> _lsUserMaseter = Global.controller.GetSelcetedUserMaster(_UserID);

                foreach (var _UserItem in _lsUserMaseter)
                {
                    UserID = _UserItem.UserID;
                    lblVUserName.Content = _UserItem.UserName;
                    txtFullName.Text = _UserItem.UserFullName;
                    txtAddress.Text = _UserItem.UserAddress;
                    txtUserName.Text = _UserItem.UserName;
                    dtpJoiningDate.Text = _UserItem.JoiningDate.ToShortDateString();
                    lblCRole.Text = _UserItem.RoleName;
                    cmbRole.Text = _UserItem.RoleName;
                    lblVUserFullName.Content = _UserItem.UserFullName;
                    lblVJoinigDate.Content = _UserItem.JoiningDate.ToShortDateString();
                    lblVAddress.Content = _UserItem.UserAddress;

                }
            }
            catch (Exception Ex)
            {
                ErrorLoger.save("PgUserinfoDisplay.cvsMain_Loaded()", Ex.ToString());
            }
        }
        #region Combobox Visiblitychanges
        private void cmbRole_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            lblCRole.Visibility = cmbRole.SelectedItem == null ? Visibility.Visible : Visibility.Hidden;
        }

       
        #endregion
        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //ComboBoxItem _CmbStatio = (ComboBoxItem)ddlStation.SelectedItem;

                List<cstUserMasterTbl> _lsUserMaster = new List<cstUserMasterTbl>();
                cstUserMasterTbl _userInfo = new cstUserMasterTbl();
                _userInfo.UserName = txtUserName.Text;
                _userInfo.UserAddress = txtAddress.Text;
                _userInfo.Password = txtPass.Password.ToString();
                _userInfo.JoiningDate = Convert.ToDateTime(dtpJoiningDate.Text);
                if (cmbRole.Text == "" || cmbRole.Text == null)
                {
                    _userInfo.RoleName = lblCRole.Text;//if Role Combobox not selected.
                }
                else
                {
                    _userInfo.RoleName = cmbRole.Text;//if Role Combobox  selected.
                }
                _userInfo.UserFullName = txtFullName.Text;
                if (cmbRole.SelectedItem != null)
                {
                    cstRoleTbl Role = (cstRoleTbl)cmbRole.SelectedItem;
                    _userInfo.Role = Role.RoleId;
                }
                else
                {
                    _userInfo.Role = Guid.Empty;
                }

                _lsUserMaster.Add(_userInfo);
                Boolean _return = Global.controller.SetUserMaster(_lsUserMaster, UserID);
                if (_return == true)
                {
                 MsgBox.Show("Ok", "Update", Environment.NewLine + "Record updated successfully!");
                    ClearAllForms();
                    
                }
                else
                {
                   MsgBox.Show("Warning", "Update", Environment.NewLine + "Record update fail.");
                }
            }
            catch (Exception Ex)
            {
                ErrorLoger.save("PgUserinfoDisplay.btnUpdate_Click()", Ex.ToString());
            }
        }
        /// <summary>
        /// This function clear the all values from the page.
        /// </summary>
        private void ClearAllForms()
        {
            try
            {
                txtFullName.Text = "";
                txtAddress.Text = "";
                txtUserName.Text = "";
                txtPass.Clear();
                txtConfirmPass.Clear();
                dtpJoiningDate.Text = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time").ToShortDateString();
                cmbRole.SelectedIndex = -1;
                lblCRole.Text = "";
            }
            catch (Exception Ex)
            {
                ErrorLoger.save("PgUserinfoDisplay.ClearAllForms()", Ex.ToString());
            }
        }
        private void txtFullName_TextChanged(object sender, TextChangedEventArgs e)
        {
           
        }

        private void txtFullName_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {

                //string _input = (sender as TextBox).Text;
                //                if (_input.CharOnly())
                //{
                //    e.Handled = true;
                //}


            }
            catch (Exception Ex)
            {
                ErrorLoger.save("PgUserinfoDisplay.txtFullName_KeyDown()", Ex.ToString());
            }
        }   


    }

    
    
  
}
