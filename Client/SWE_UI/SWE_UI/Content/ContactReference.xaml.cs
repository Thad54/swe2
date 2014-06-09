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
using System.ComponentModel;

namespace SWE_UI.Content
{
    /// <summary>
    /// Interaction logic for ContactReference.xaml
    /// </summary>
    public partial class ContactReference : UserControl
    {
        ContactReferenceViewModel model = new ContactReferenceViewModel();

        public List<XmlExchange.contact> ItemSource
        {
            get
            {
                return (List<XmlExchange.contact>)GetValue(ItemSourceProperty);
            }
            set
            {
                SetValue(ItemSourceProperty, value);
            }
        }

        public static readonly DependencyProperty ItemSourceProperty =
         DependencyProperty.Register("ItemSource", typeof(List<XmlExchange.contact>),
        typeof(ContactReference), new UIPropertyMetadata(null, new PropertyChangedCallback(OnItemSourcePropertyChanged)));

        private static void OnItemSourcePropertyChanged(DependencyObject source,
        DependencyPropertyChangedEventArgs e)
        {
            ContactReference control = source as ContactReference;
            control.model.EmployingCompanyData_Edit = (List<XmlExchange.contact>)e.NewValue;
        }

        public XmlExchange.contact SelectedItem
        {
            get
            {
               return (XmlExchange.contact)GetValue(SelectedItemProperty);
            }
            set {
                SetValue(SelectedItemProperty, value);                          
            }
        }

        public static readonly DependencyProperty SelectedItemProperty =
         DependencyProperty.Register("SelectedItem", typeof(XmlExchange.contact),
        typeof(ContactReference), new UIPropertyMetadata(null, new PropertyChangedCallback(OnItemSelectedPropertyChanged)));

        private static void OnItemSelectedPropertyChanged(DependencyObject source,
        DependencyPropertyChangedEventArgs e)
        {
            ContactReference control = source as ContactReference;
            control.model.CompanyID_Edit = (XmlExchange.contact)e.NewValue;
        }

        public string Search
        {
            get
            {
                return (string)GetValue(SearchProperty);
            }
            set
            {
                SetValue(SearchProperty, value);
            }
        }

        public static readonly DependencyProperty SearchProperty =
         DependencyProperty.Register("Search", typeof(string),
        typeof(ContactReference), new FrameworkPropertyMetadata("Search", new PropertyChangedCallback(OnSearchPropertyChanged)));

        private static void OnSearchPropertyChanged(DependencyObject source,
        DependencyPropertyChangedEventArgs e)
        {
            ContactReference control = source as ContactReference;
            control.model.Search = (string)e.NewValue;
        }

        public ContactReference()
        {
            InitializeComponent();
            model.control = this;
            this.DataContext = model;
        }

       /* private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           // SelectedItem = (XmlExchange.contact)e.AddedItems[0];
        }*/
    }
}
