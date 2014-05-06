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
        private readonly DelegateCommand<string> _SearchCommand;

        public ViewModel()
        {
            _SearchCommand = new DelegateCommand<string>(
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
            get { return _SearchCommand; }
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

        private string _LastName;
        public string LastName
        {
            get { return _LastName; }
            set
            {
                _LastName = value;
                _SearchCommand.RaiseCanExecuteChanged();
                OnPropertyChanged("LastName");
                NotifyChange_Search();
            }
        }

        private string _FirstName;
        public string FirstName
        {
            get { return _FirstName; }
            set
            {
                _FirstName = value;
                _SearchCommand.RaiseCanExecuteChanged();
                OnPropertyChanged("FirstName");
                NotifyChange_Search();
            }
        }

        private string _CompanyName;
        public string CompanyName
        {
            get { return _CompanyName; }
            set
            {
                _CompanyName = value;
                _SearchCommand.RaiseCanExecuteChanged();
                OnPropertyChanged("CompanyName");
                NotifyChange_Search();
            }
        }

        private string _UID;
        public string UID
        {
            get { return _UID; }
            set
            {
                _UID = value;
                _SearchCommand.RaiseCanExecuteChanged();
                OnPropertyChanged("UID");
                NotifyChange_Search();
            }
        }

        public bool? IsCompany
        {
            get
            {
                if (string.IsNullOrWhiteSpace(FirstName) && string.IsNullOrWhiteSpace(LastName) && string.IsNullOrWhiteSpace(CompanyName) && string.IsNullOrWhiteSpace(UID))
                {
                    return null;
                }
                return !string.IsNullOrWhiteSpace(CompanyName) || !string.IsNullOrWhiteSpace(UID);

            }
        }

        public bool CanSearchCompany
        {
            get
            {
                return IsCompany == null || IsCompany == true;
            }
        }

        public bool CanSearchPerson
        {
            get
            {
                return IsCompany == null || IsCompany == false;
            }
        }

       private void NotifyChange_Search(){
           OnPropertyChanged("IsCompany");
           OnPropertyChanged("CanSearchCompany");
           OnPropertyChanged("CanSearchPerson");
       }

        #endregion
        #region EditContact

       private string _Title_Edit;
       public string Title_Edit
       {
           get { return _Title_Edit; }
           set
           {
               _Title_Edit = value;
               NotifyChange_Edit();
    //           _clickCommand.RaiseCanExecuteChanged();
           }
       }

       private string _FirstName_Edit;
       public string FirstName_Edit
       {
           get { return _FirstName_Edit; }
           set
           {
               _FirstName_Edit = value;
               NotifyChange_Edit();
               //           _clickCommand.RaiseCanExecuteChanged();
           }
       }

       private string _LastName_Edit;
       public string LastName_Edit
       {
           get { return _LastName_Edit; }
           set
           {
               _LastName_Edit = value;
               NotifyChange_Edit();
               //           _clickCommand.RaiseCanExecuteChanged();
           }
       }

       private string _Suffix_Edit;
       public string Suffix_Edit
       {
           get { return _Suffix_Edit; }
           set
           {
               _Suffix_Edit = value;
               NotifyChange_Edit();
               //           _clickCommand.RaiseCanExecuteChanged();
           }
       }

       private string _CompanyName_Edit;
       public string CompanyName_Edit
       {
           get { return _CompanyName_Edit; }
           set
           {
               _CompanyName_Edit = value;
               NotifyChange_Edit();
               //           _clickCommand.RaiseCanExecuteChanged();
           }
       }

       private string _UID_Edit;
       public string UID_Edit
       {
           get { return _UID_Edit; }
           set
           {
               _UID_Edit = value;
               NotifyChange_Edit();
               //           _clickCommand.RaiseCanExecuteChanged();
           }
       }

       public bool? IsPerson_Edit
       {
           get
           {
               if (string.IsNullOrWhiteSpace(FirstName_Edit) && string.IsNullOrWhiteSpace(LastName_Edit) && string.IsNullOrWhiteSpace(Title_Edit) && string.IsNullOrWhiteSpace(CompanyName_Edit) && string.IsNullOrWhiteSpace(Suffix_Edit) && string.IsNullOrWhiteSpace(UID_Edit))
               {
                   return null;
               }
               return !string.IsNullOrWhiteSpace(CompanyName_Edit) || !string.IsNullOrWhiteSpace(UID_Edit);
               
           }
       }

       public bool CanEditCompany
       {
           get
           {
               return IsPerson_Edit == null || IsPerson_Edit == true;
           }
       }

       public bool CanEditPerson
       {
           get
           {
               return IsPerson_Edit == null || IsPerson_Edit == false;
           }
       }

       private void NotifyChange_Edit()
       {
           OnPropertyChanged("IsPerson");
           OnPropertyChanged("CanEditCompany");
           OnPropertyChanged("CanEditPerson");
       }

        #endregion
    }
}
