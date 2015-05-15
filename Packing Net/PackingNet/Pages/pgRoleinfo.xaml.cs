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
    /// Interaction logic for RoleUI.xaml
    /// </summary>
    public partial class RoleUI : Page
    {


        RoleDataMode _mode;
        /// <summary>
        /// permission set
        /// </summary> 
        string _permissionSet;


        public RoleUI()
        {
            InitializeComponent();
            fillRoleList();
            _permissionSet = "";
            _mode = RoleDataMode.New;
        }

        /// <summary>
        /// Save role
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="index"></param>
        private void btnSaveRole_Click(object sender, RoutedEventArgs e)
        {
            iteratePermissionSet(grdUserPermissionGrid, 0);
            _permissionSet = _permissionSet + "&";
            iteratePermissionSet(grdShipmentPermissionGrid, 0);
            cstRoleTbl role = new cstRoleTbl();
            role.Name = txtRoleName.Text;
            role.Action = _permissionSet;
            if (_mode == RoleDataMode.New)
            {
                Global.controller.UpdateRole(role, csteActionenum.New);
                fillRoleList();
                _mode = RoleDataMode.New;
                btnSaveRole.Content = "Save";
                txtRoleName.Text = "";
                txtRoleName.Focus();
                unCheckAllPermissions(grdShipmentPermissionGrid, 0);
                unCheckAllPermissions(grdUserPermissionGrid, 0);
                MsgBox.Show("Ok", "Save", Environment.NewLine + "Setting save successfully.");
            }
            else if (_mode == RoleDataMode.Edit)
            {
                role.RoleId = (Guid)lstRole.SelectedValue;
                Global.controller.UpdateRole(role, csteActionenum.Update);
                MsgBox.Show("Ok", "Update", Environment.NewLine + "Setting updated successfully.");
            }
        }

        /// <summary>
        /// Iterator for getting permission sets.
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="index"></param>
        public void iteratePermissionSet(Grid grid, int index)
        {
            if (grid.Children.Count == index)
            {
                _permissionSet = _permissionSet.Remove(_permissionSet.Length - 1);
                return;
            }
            else
            {
                if (grid.Children[index] is CheckBox)
                {
                    CheckBox checkBox = grid.Children[index] as CheckBox;
                    _permissionSet = _permissionSet + checkBox.IsChecked.ToString() + "-";
                }
                iteratePermissionSet(grid, ++index);
            }
        }

        /// <summary>
        /// Fill List box control with roles.
        /// </summary>
        private void fillRoleList()
        {
            List<cstRoleTbl> lsRoles = Global.controller.GetRole();
            lstRole.ItemsSource = lsRoles;
        }

        /// <summary>
        /// Initialize Page.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page_Initialized_1(object sender, EventArgs e)
        {
            fillRoleList();
        }


        /// <summary>
        /// On List Box item selection display all Roles in edit mode.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lstRole_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _permissionSet = "";
            this._mode = RoleDataMode.Edit;
            btnSaveRole.Content = "Update";
            if (((ListBox)sender).SelectedValue != null)
            {
                List<cstRoleTbl> list = Global.controller.GetRoleById((Guid)((ListBox)sender).SelectedValue, csteActionenum.Get);
                if (list.Count > 0)
                {
                    cstRoleTbl objRole = list[0];
                    txtRoleName.Text = objRole.Name;
                    unCheckAllPermissions(grdShipmentPermissionGrid, 0);
                    unCheckAllPermissions(grdUserPermissionGrid, 0);
                    if (objRole.Action != null)
                    {
                        string[] strAction = objRole.Action.Split(new char[] { '&' });
                        iteratePermissionSet(grdUserPermissionGrid, strAction[0].Split(new char[] { '-' }), 0);
                        iteratePermissionSet(grdShipmentPermissionGrid, strAction[1].Split(new char[] { '-' }), 0);
                    }
                }
            }
        }

        /// <summary>
        /// Un check all permission sets.
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="action"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public void unCheckAllPermissions(Grid grid, int index)
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
                    checkBox.IsChecked = false;
                }
                unCheckAllPermissions(grid, ++index);
            }
        }

        /// <summary>
        /// Set all permissions sets
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="action"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public void iteratePermissionSet(Grid grid, string[] action, int index)
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

        /// <summary>
        /// On new role button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNewRole_Click(object sender, RoutedEventArgs e)
        {
            _mode = RoleDataMode.New;
            btnSaveRole.Content = "Save";
            txtRoleName.Text = "";
            txtRoleName.Focus();
            unCheckAllPermissions(grdShipmentPermissionGrid, 0);
            unCheckAllPermissions(grdUserPermissionGrid, 0);
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {

        }        
    }
}
