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
using HRManagement.View;
using HRManagement.Controller;

namespace HRManagement
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static MainWindow log;  //Single Object For Class (Static)

        private MainWindow()
        {
            InitializeComponent();
        }

        public static MainWindow Log
        {
            get
            {
                if (log == null)
                    log = new MainWindow();
                return log;
            }
        }
        private void BtnLogClick(object sender,RoutedEventArgs evt)
        {
            DBData db = new DBData();
            String Loglbl = Convert.ToString(LblLog.Content);   //To Convert Label Text To String

            if (Loglbl == "LOGIN")
            {
                String sql = "select * from Login where Uname='" + TxtUname.Text + "' And Password='" + TxtPwd.Password + "'";
                if (db.Find(sql) == true)
                {
                    var uriSource = new Uri(@"Assets\Unlock.png",UriKind.Relative);
                    LogImg.Source = new BitmapImage(uriSource);
                    MainMenu menu = new MainMenu();
                    menu.Show();
                    this.Hide();
                    LblLog.Content = "LOGOUT";
                }
                else
                {
                    sql = "select * from Login where Uname='" + TxtUname.Text + "'";
                    if (db.Find(sql) == true)                       //Mean UName is Available & Pwd wrong
                    {
                        TxtPwd.Password = "";
                    }
                    else                                            //Mean Password is wrong
                    {
                        TxtUname.Text = "";
                        TxtPwd.Password = "";
                    }
                }
            }
            else
            {
                var uriSource = new Uri(@"Assets\Lock.png", UriKind.Relative);
                LogImg.Source = new BitmapImage(uriSource);
                TxtPwd.Password = "";
                LblLog.Content = "LOGIN";
            }
        }
    }
}
