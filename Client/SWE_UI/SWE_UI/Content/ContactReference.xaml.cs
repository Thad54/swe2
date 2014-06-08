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
     

        public XmlExchange.contact SelectedItem
        {
            get
            {
                return model.CompanyID_Edit;
               // return (XmlExchange.contact)GetValue(SelectedItemProperty);
            }
            set {
                model.CompanyID_Edit = value;
                //SetValue(SelectedItemProperty, value);                          
            }
        }

        public static readonly DependencyProperty SelectedItemProperty =
         DependencyProperty.Register("SelectedItem", typeof(XmlExchange.contact),
        typeof(ContactReference), new UIPropertyMetadata(null));

        public List<XmlExchange.contact> ItemSource
        {
            get
            {
                return model.EmployingCompanyData_Edit;
               // return (List<XmlExchange.contact>)GetValue(ItemSourceProperty);
            }
            set
            {
                model.EmployingCompanyData_Edit = value;
               // SetValue(ItemSourceProperty, value);
            }
        }

        public static readonly DependencyProperty ItemSourceProperty =
         DependencyProperty.Register("ItemSource", typeof(List<XmlExchange.contact>),
        typeof(ContactReference), new UIPropertyMetadata(null));


        public string Search
        {
            get
            {
                //return model.Search;
                return (string)GetValue(SearchProperty);
            }
            set
            {
                //model.Search = value;
                //OnPropertyChanged("Search");
                SetValue(SearchProperty, value);
            }
        }

        public static readonly DependencyProperty SearchProperty =
         DependencyProperty.Register("Search", typeof(string),
        typeof(ContactReference), new FrameworkPropertyMetadata("Nope", new PropertyChangedCallback(OnSearchPropertyChanged)));

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
    }
}
