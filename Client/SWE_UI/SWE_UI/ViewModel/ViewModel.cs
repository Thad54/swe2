using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.ComponentModel;

namespace SWE_UI.ViewModel
{
    public class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string prop)
        {
            var temp = PropertyChanged;
            if (temp != null)
            {
                temp(this, new PropertyChangedEventArgs(prop));
            }
        }

        private readonly DelegateCommand<string> _clickCommand;

        public ViewModel()
        {
            _clickCommand = new DelegateCommand<string>(
                (s) => {
                    var proxy = new proxy();
                    var result = new List<XmlExchange.contact>();
                    try
                    {
                        result = proxy.searchContacts(_searchText);
                    }
                    catch(Exception)
                    {
                        return;
                    }
                    if (result.Count == 0)
                    {
                        Contacts = "No Contact Found";
                        return;
                    }
                    var sb = new StringBuilder();

                    foreach (var con in result)
                    {
                        sb.AppendFormat("{0} | {1} | \n", con.name, con.lastName);
                    }
                    Contacts = sb.ToString();

                }, //Execute
                (s) => { return !string.IsNullOrEmpty(_searchText); } //CanExecute
                );
        }

        public DelegateCommand<string> SearchContactClicked
        {
            get { return _clickCommand; }
        }

        private string _contacts;
        public string Contacts
        {
            set
            {
                _contacts = value;
                OnPropertyChanged("Contacts");
            }

            get
            {             
                return _contacts;                         
            }
        }

        private string _searchText;
        public string SearchText
        {
            get { return _searchText; }
            set
            {
                _searchText = value;
                _clickCommand.RaiseCanExecuteChanged();
            }
        }
    }
}
