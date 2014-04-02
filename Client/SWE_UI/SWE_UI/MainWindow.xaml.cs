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

namespace SWE_UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
  //      private proxy _proxy;

        public MainWindow()
        {
            InitializeComponent();
 //           _proxy = new proxy();

          this.DataContext = new ViewModel.ViewModel();
        }

  /*      private void Button_Click(object sender, RoutedEventArgs e)
        {
          var contacts = _proxy.searchContacts(contactSearch.Text);

          if (contacts.Count == 0)
          {
              SearchResult.Text = "No Contacts found";
          }

            foreach(var con in contacts){
                SearchResult.Text += con.name + " | " + con.lastName +"\n";
            }

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var contacts = _proxy.searchContacts(contactSearch.Text);

            if (contacts.Count == 0)
            {
                SearchResult.Text = "No Contacts found";
            }

            foreach (var con in contacts)
            {
                SearchResult.Text += con.name + " | " + con.lastName + "\n";
            }
        }*/
    }
}
