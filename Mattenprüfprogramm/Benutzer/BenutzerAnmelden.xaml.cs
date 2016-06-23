using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Mattenprüfprogramm.Benutzer
{
    /// <summary>
    /// Interaktionslogik für BenutzerAnmelden.xaml
    /// </summary>
    public partial class BenutzerAnmelden : Window
    {
        MainWindow w;


        public BenutzerAnmelden(MainWindow w)
        {
            InitializeComponent();
            this.Topmost = true;
            this.w = w;
        }

        private void button_Login_Click(object sender, RoutedEventArgs e)
        {
            DatabaseConnectionDataContext db = new DatabaseConnectionDataContext();

            var usr = from u in db.User
                      where u.Name == textBox_Name.Text && u.Passwort == passwordBox.Password
                      select u;

            if (usr.Count() > 0)
            {
                w.user = usr.First();
                w.ActivateMenu(usr.First(),true);
                this.Close();
            }
            else
            {
                label_message.Content = "Nutzer oder Passwort nicht bekannt!";
                
            }
        }
    }
}
