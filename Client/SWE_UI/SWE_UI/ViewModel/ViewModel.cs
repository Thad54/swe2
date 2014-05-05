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

        #region SearchContact
        private readonly DelegateCommand<string> _clickCommand;

        public ViewModel()
        {
            _clickCommand = new DelegateCommand<string>(
                (s) =>
                {
                    var proxy = new proxy();
                    var result = new List<XmlExchange.contact>();
                    try
                    {
                        result = proxy.searchContacts(_FirstName);
                    }
                    catch (Exception)
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
                (s) => {
                    if (!string.IsNullOrEmpty(_FirstName) || !string.IsNullOrEmpty(_LastName) || !string.IsNullOrEmpty(_CompanyName) || !string.IsNullOrEmpty(_UID))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                
                } //CanExecute
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
                //  var newW = new SearchResults();
                //  newW.Show();
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

        private string _LastName;
        public string LastName
        {
            get { return _LastName; }
            set
            {
                _LastName = value;
                _clickCommand.RaiseCanExecuteChanged();
                OnPropertyChanged("LastName");
                NotifyChange();
            }
        }

        private string _FirstName;
        public string FirstName
        {
            get { return _FirstName; }
            set
            {
                _FirstName = value;
                _clickCommand.RaiseCanExecuteChanged();
                OnPropertyChanged("FirstName");
                NotifyChange();
            }
        }

        private string _CompanyName;
        public string CompanyName
        {
            get { return _CompanyName; }
            set
            {
                _CompanyName = value;
                _clickCommand.RaiseCanExecuteChanged();
                OnPropertyChanged("CompanyName");
                NotifyChange();
            }
        }

        private string _UID;
        public string UID
        {
            get { return _UID; }
            set
            {
                _UID = value;
                _clickCommand.RaiseCanExecuteChanged();
                OnPropertyChanged("UID");
                NotifyChange();
            }
        }

        public bool? IsCompany
        {
            get
            {
                if (string.IsNullOrWhiteSpace(FirstName) && string.IsNullOrWhiteSpace(LastName) && string.IsNullOrWhiteSpace(CompanyName))
                {
                    return null;
                }
                return !string.IsNullOrWhiteSpace(CompanyName);

            }
        }

        public bool CanEditCompany
        {
            get
            {
                return IsCompany == null || IsCompany == true;
            }
        }

        public bool CanEditPerson
        {
            get
            {
                return IsCompany == null || IsCompany == false;
            }
        }

       private void NotifyChange(){
           OnPropertyChanged("IsCompany");
           OnPropertyChanged("CanEditCompany");
           OnPropertyChanged("CanEditPerson");
       }

        #endregion
        #region EditContact

        #endregion
    }
}
