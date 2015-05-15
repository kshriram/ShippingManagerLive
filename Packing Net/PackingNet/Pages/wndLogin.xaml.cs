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
using PackingClassLibrary;
using PackingClassLibrary.CustomEntity;
using PackingClassLibrary.Models;

namespace Packing_Net.Pages
{
    public partial class Login : Window
    {

        #region Declarations 
        //blank User model Object.
        model_User modelUser = Global.controller.getModelUser();

        //Mac Address of machin;
        String MACAddresss = Global.controller.getDeviceMAC();
        #endregion
        int rdoFlag = 0;
        #region Events
        /// <summary>
        /// Author: Avinash.
        /// Version: Alfa.
        /// Login Page Initialze event method
        /// </summary>
        public Login()
        {
            #region initialization page

            InitializeComponent();
            try
            {
                //Logger add log.
                SaveUserLogsTbl.logThis(csteActionType.Login_PageStart.ToString());


                //set language for the application.
                _setLanguage();

                // if AutoLogout occur.
                BErrorMsg.Visibility = System.Windows.Visibility.Hidden;

                //session expired
                if (Global.ISTimerRaised == true)
                {
                    _scrollMsg(Global.controller.ConvetLanguage("Session expired. Please login again to Continue...", Global.LanguageFileName), Color.FromRgb(222, 87, 24));
                }

                #region Message
                //Set Error message on the Sctrip and Visible it to animate.
                _scrollMsg("Please scan your badge.", Color.FromRgb(19, 136, 49));
                #endregion
            }
            catch (Exception Ex)
            {
                //Log the Error to the Error Log table
               /// ErrorLoger.save("wndLogin - public Login()", "[" + DateTime.UtcNow.ToString() + "]" + Ex.ToString());
                ErrorLoger.save("wndLogin - public Login()", "[" +TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time" + "]" + Ex.ToString()));
              /////  DateTime dt= TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time");

            }

            #endregion
        }

        /// <summary>
        /// Page Load event 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            try
            {
                //Pause the Session till Login successfull
                SessionManager.Autotimer.Stop();

                txtUserName.Focus();

                //Convert screen language to set application language
                WindowLanguages.Convert(this);
                rdoFlag = 1;
            }
            catch (Exception Ex)
            {
                //Log the Error to the Error Log table
              ///  ErrorLoger.save("wndLogin - Window_Loaded_1", "[" + DateTime.UtcNow.ToString() + "]" + Ex.ToString());
              ///  
                ErrorLoger.save("wndLogin - Window_Loaded_1", "[" + TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time" + "]" + Ex.ToString()));
            }
        }

        /// <summary>
        /// KeyPress event of User Name textbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtUserName_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == System.Windows.Input.Key.Enter)
                {
                    #region Login enter perssed
                    if (txtUserName.Text != "")
                    {
                        //Same User login for session again. when session expired.
                        if (Global.TimeOutUserName == txtUserName.Text && Global.ISTimerRaised == true)
                        {
                            //Logger add log.
                            SaveUserLogsTbl.logThis(csteActionType.SessionReLogin.ToString());

                            //Session start..
                            SessionManager.StartTime();

                            //Close this window 
                            this.Close();
                        }
                        else if ((Global.TimeOutUserName == "" || Global.TimeOutUserName == null) && Global.ISTimerRaised == false)
                        {
                            //if session not expired then login is first time.
                            login_Prorgam();
                        }
                        else
                        {
                            //Worong username or password

                            txtUserName.Text = "";
                            txtMask.Text = "";

                            //Logger add log.
                            SaveUserLogsTbl.logThis(csteActionType.Login_Invalid_User_00.ToString());

                            //Show msg strip.
                            _scrollMsg(Global.controller.ConvetLanguage("You are not previously logged User.", Global.LanguageFileName), Color.FromRgb(222, 87, 24));
                        }
                    }
                    else
                    {
                        e.Handled = false;
                    }
                    #endregion
                }
            }
            catch (Exception Ex)
            {
                //Log the Error to the Error Log table
              //  ErrorLoger.save("wndLogin - txtUserName_KeyDown", "[" + DateTime.UtcNow.ToString() + "]" + Ex.ToString());
                ErrorLoger.save("wndLogin - txtUserName_KeyDown", "[" + TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time" + "]" + Ex.ToString()));
            }

            // if key pressed at the textbox is enter the this will go on. - avinash6jun2013


        }
        
        
        /// <summary>
        /// Show barcode chagractor for the login textbox
        /// </summary>
        private void txtUserName_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key != Key.Back && e.Key != Key.Delete && e.Key != Key.Enter)
                {
                    txtMask.Text = "‖" + txtMask.Text;
                }
                else if (e.Key == Key.Back || e.Key != Key.Delete)
                {
                    txtMask.Text = "";
                    txtUserName.Text = "";
                }
            }
            catch (Exception Ex)
            {
                //Log the Error to the Error Log table
              //  ErrorLoger.save("wndLogin - txtUserName_KeyUp", "[" + DateTime.UtcNow.ToString() + "]" + Ex.ToString());
                ErrorLoger.save("wndLogin - txtUserName_KeyUp", "[" + TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time" + "]" + Ex.ToString()));
            }

            
        }

        #endregion

        #region Methods

        #region Login Code
        /// <summary>
        /// Login Code 
        /// </summary>
        public void login_Prorgam()
        {
            List<cstUserMasterTbl> _lsUseMaster = new List<cstUserMasterTbl>();
            try
            {
                //find the username and password.
                _lsUseMaster = modelUser.UserFunctions.GetUserMaster(txtUserName.Text);
            }
            catch (Exception Ex)
            {
                //Log the Error to the Error Log table
               /// ErrorLoger.save("wndLogin - login_Prorgam_Sub1", "[" + DateTime.UtcNow.ToString() + "]" + Ex.ToString());
                ErrorLoger.save("wndLogin - txtUserName_KeyUp", "[" + TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time" + "]" + Ex.ToString()));
            }

            try
            {
                if (_lsUseMaster.Count() > 0)
                {
                    modelUser = new model_User(_lsUseMaster[0].UserID);

                    Global.LoggedUserModel = modelUser;
                    
                    Global.LoggedUserId = modelUser.UserInfo.UserID;
                                        
                    //Set UserID for created and update responsible operations, in all application for Audit table and Error log table.
                    Global.controller.SetCreatedOrUpdatedUserID(Global.LoggedUserId);
                    
                    Global.WindowTopUserName = modelUser.UserInfo.UserFullName;
                    Global.LastLoginDateTime = modelUser.LastLoginTime;
            ////    Global.LastLoginDateTime=TimeZoneInfo.ConvertTimeBySystemTimeZoneId(modelUser.LastLoginTime, "Eastern Standard Time" + "]");
                        
                    //Admin User Check and rediract to the Home screen.
                        if (Global.controller.IsSuperUser(Global.LoggedUserId))
                        {
                            if (CheckStationRegistred.IsRegistered() == false)
                            {
                                wndStationMaster _Station = new wndStationMaster();
                                this.Dispatcher.Invoke(new Action(() => { _Station.ShowDialog(); }));
                                try
                                {
                                    _addLogUserStationtable();
                                }
                                catch (Exception Ex)
                                {
                                    //Log the Error to the Error Log table
                                   /// ErrorLoger.save("wndLogin - login_Prorgam_Sub2", "[" + DateTime.UtcNow.ToString() + "]" + Ex.ToString());
                                    ErrorLoger.save("wndLogin - login_Prorgam_Sub2", "[" + TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time" + "]" + Ex.ToString()));
                                }
                            }
                            else if (CheckStationRegistred.IsRegistered())
                            {
                                _addLogUserStationtable();
                            }

                            txtUserName.Text = "";
                            txtMask.Text = "";

                            //save User Log
                            SaveUserLogsTbl.logThis(csteActionType.Login.ToString());

                            HomeScreen _Home = new HomeScreen();
                            this.Dispatcher.Invoke(new Action(() => { _Home.Show(); }));
                            this.Close();
                        }
                        else
                        {
                            #region station Registration Check
                            if (CheckStationRegistred.IsRegistered() == false)
                            {
                                wndStationMaster _Station = new wndStationMaster();
                                this.Dispatcher.Invoke(new Action(() => { _Station.ShowDialog(); }));
                                try
                                {
                                    //save new station.
                                    _addLogUserStationtable();
                                }
                                catch (Exception Ex)
                                {
                                    //Log the Error to the Error Log table
                                ///    ErrorLoger.save("wndLogin - login_Prorgam_Sub3", "[" + DateTime.UtcNow.ToString() + "]" + Ex.ToString());
                                    ErrorLoger.save("wndLogin - login_Prorgam_Sub2", "[" + TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time" + "]" + Ex.ToString()));
                                }
                            }
                            else
                            {
                                _addLogUserStationtable();
                                Global.StationModel = new model_Station(MACAddresss);
                            }
                            #endregion

                            txtUserName.Text = "";
                            txtMask.Text = "";
                            //Set Error message on the Sctrip and Visible it to animate.
                            _scrollMsg("Welcome " + Global.LoggedUserModel.UserInfo.UserFullName, Color.FromRgb(38, 148, 189));
                            //save User Log
                            SaveUserLogsTbl.logThis(csteActionType.Login.ToString());

                            //Open admin home page for admin.
                            MainWindow ScanWindow = new MainWindow();
                            this.Dispatcher.Invoke(new Action(() => { ScanWindow.Show(); }));

                            //close this window
                            this.Close();
                        }
                }
                else
                {
                    txtUserName.Text = "";
                    txtMask.Text = "";

                    //Set Error message on the Sctrip and Visible it to animate.
                    _scrollMsg("Warning : Access Denied. Incorrect User Name.", Color.FromRgb(222, 87, 24));
                }
            }
            catch (Exception Ex)
            {
                //Log the Error to the Error Log table
            ///    ErrorLoger.save("wndLogin - login_Prorgam_Sub3", "[" + DateTime.UtcNow.ToString() + "]" + Ex.ToString());
                ErrorLoger.save("wndLogin - login_Prorgam_Sub3", "[" + TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time" + "]" + Ex.ToString()));
            }
        }
        #endregion

        #region Station save
        /// <summary>
        /// save new station to station table.
        /// </summary>
        private void _addLogUserStationtable()
        {
            try
            {
               
                List<cstStationMasterTbl> lsstationID = Global.controller.GetStationMaster(MACAddresss);
                List<cstUserStationTbl> lsUserStation = new List<cstUserStationTbl>();
                cstUserStationTbl Ustaion = new cstUserStationTbl();
                Ustaion.UserStationID = Guid.Empty;
                Ustaion.StationID = lsstationID[0].StationID;
                Ustaion.UserID = Global.LoggedUserId;
                //Ustaion.LoginDateTime = DateTime.UtcNow;
                ///12-08-2014
                Ustaion.LoginDateTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time");
                lsUserStation.Add(Ustaion);
                Global.controller.SaveUserStation(lsUserStation);
                
                Global.StationName = lsstationID[0].StationName.ToString();
                Global.StationModel = new model_Station(MACAddresss);
            }
            catch (Exception Ex)
            {
                //Log the Error to the Error Log table
                //ErrorLoger.save("wndLogin - saveUserStation", "[" + DateTime.UtcNow.ToString() + "]" + Ex.ToString());
                ErrorLoger.save("wndLogin - saveUserStation", "[" + TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time" + "]" + Ex.ToString()));
            }
        }
        #endregion

        #region Language Set
       /// <summary>
       /// Set Application Langage.
       /// </summary>
        private void _setLanguage()
        {
            try
            {
                String LanguageString = Global.controller.ReadFromLocalFile("Language");
                SetGlobalFileName(LanguageString);
            }
            catch (Exception Ex)
            {
                //Log the Error to the Error Log table
                ErrorLoger.save("wndLogin - _setLanguage", "[" + DateTime.UtcNow.ToString() + "]" + Ex.ToString());
            }
        }

        /// <summary>
        /// Select and set language to Global File
        /// </summary>
        /// <param name="LanguageString">String language select</param>
        private void SetGlobalFileName(String LanguageString)
        {
            switch (LanguageString)
            {
                case "English":
                    Global.LanguageFileName = Environment.CurrentDirectory + "\\Language\\English_US.sys";
                    break;
                case "Chinese":
                    Global.LanguageFileName = Environment.CurrentDirectory + "\\Language\\Chinese.sys";
                    break;
                case "Russian":
                    Global.LanguageFileName = Environment.CurrentDirectory + "\\Language\\Russian.sys";
                    break;

                default:
                    break;
            }
        }
        #endregion
        #endregion

        #region Scroll message set
        /// <summary>
        /// Scroll Message show.
        /// </summary>
        /// <param name="Message">String message.</param>
        /// <param name="clr">Color of the message.</param>
        private void _scrollMsg(string Message, Color clr)
        {
            Dispatcher.Invoke(new Action(() =>
                {

                    try
                    {
                        BErrorMsg.Visibility = System.Windows.Visibility.Hidden;
                        BErrorMsg.Visibility = System.Windows.Visibility.Visible;
                        lblErrorMsg.Foreground = new SolidColorBrush(clr);
                       /// lblErrorMsg.Text = "Login -[" + (DateTime.UtcNow.ToString("hh:mm:ss tt")) + "]▶ " + Message.ToString();
                        /// TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time")
                        lblErrorMsg.Text = "Login -[" + (TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time")) + "]▶ " + Message.ToString();
                        txtblStack.Inlines.Add(new Run { Text = lblErrorMsg.Text + Environment.NewLine, Foreground = new SolidColorBrush(clr) });
                        Global.lsScroll.Add(new Run { Text = lblErrorMsg.Text + Environment.NewLine, Foreground = new SolidColorBrush(clr) });
                        svStack.ScrollToBottom();
                    }
                    catch (Exception Ex)
                    {
                        //Log the Error to the Error Log table
                      //  ErrorLoger.save("wndLogin - _scrollMsg", "[" + DateTime.UtcNow.ToString() + "]" + Ex.ToString());
                        ErrorLoger.save("wndLogin - _scrollMsg", "[" + TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Eastern Standard Time" + "]" + Ex.ToString()));
                    }
                }));
        }
        #endregion

        private void RadioButton_Checked_1(object sender, RoutedEventArgs e)
        {
            if (rdoFlag == 1)
            {
                btnltl.IsChecked = false;
            }
            Global.LoginType = "UPSFEDEX";
            txtUserName.Focus();
        }

        private void RadioButton_Checked_2(object sender, RoutedEventArgs e)
        {
            btnupsandfedex.IsChecked = false;
            Global.LoginType = "LTL";
            txtUserName.Focus();
        }

        private void txtMask_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}

