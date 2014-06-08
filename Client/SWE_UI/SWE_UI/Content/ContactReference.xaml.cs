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
using SWE_UI.Content;

namespace SWE_UI.Content
{
    /// <summary>
    /// Interaction logic for ContactReference.xaml
    /// </summary>
    public partial class ContactReference : UserControl
    {
        ContactReferenceViewModel model = new ContactReferenceViewModel();

        public XmlExchange.contact SelectedItem
        {
            get {
                return (XmlExchange.contact)GetValue(SelectedItemProperty);
            }
            set {
                SetValue(SelectedItemProperty, value); 
            }
        }

        public static readonly DependencyProperty SelectedItemProperty =
         DependencyProperty.Register("SelectedItem", typeof(XmlExchange.contact),
        typeof(ContactReference), new PropertyMetadata(null));

        public ContactReference()
        {
            InitializeComponent();
            this.DataContext = model; 
        }

    }
}
