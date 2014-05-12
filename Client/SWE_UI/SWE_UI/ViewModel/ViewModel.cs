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
    public class ViewModel : BaseViewModel
    {
        private proxy _proxy = new proxy(); 

        #region commands
        private readonly DelegateCommand<string> _SearchCommand;
        private readonly DelegateCommand<string> _SelectionCommand;

        public ViewModel()
        {
            

            _SearchCommand = new DelegateCommand<string>(
                (s) =>
                {       

                    var result = new List<XmlExchange.contact>();
                    //EmployingCompanyData_Edit = result;


                    try
                    {
                        if (CanSearchCompany == true)
                        {
                            result = _proxy.searchCompany(_CompanyName, _UID);
                        }
                        else
                        {
                            result = _proxy.searchPerson(_FirstName, _LastName);
                        }
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

                    if (result.Count == 1)
                    {
                        /*
                        clearEdit();
                        fillEdit(result.Last());*/

                        EditContactWIndow _editContactWindow = new EditContactWIndow();
                        Edit_ViewModel _editViewModel = new Edit_ViewModel();

                        _editContactWindow.DataContext = _editViewModel;
                        _editViewModel.setMainViewModel(this);
                        _editViewModel.setWindow(_editContactWindow);
                        _editViewModel.fillEdit(result.Last());

                        _editContactWindow.Show();

                        contactData = result;
                        return;
                    }

                    contactData = result;

                }, //Execute
                (s) =>
                {
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

            _SelectionCommand = new DelegateCommand<string>(
                (s) =>
                {
                    EditContactWIndow _editContactWindow = new EditContactWIndow();
                    Edit_ViewModel _editViewModel = new Edit_ViewModel();

                    _editContactWindow.DataContext = _editViewModel;
                    _editViewModel.setMainViewModel(this);
                    _editViewModel.setWindow(_editContactWindow);
                    _editViewModel.fillEdit(SelectedContact);

                    _editContactWindow.Show();
                }, //Execute
                (s) =>
                {
                    return true;
                } //CanExecute
                );
       
        }

        public DelegateCommand<string> SearchContactClicked
        {
            get { return _SearchCommand; }
        }
        public DelegateCommand<string> ContactSelected
        {
            get { return _SelectionCommand; }
        }

        #endregion

        #region tabControl

        private int _tabItem;
        public int tabItem
        {
            set
            {
                _tabItem = value;
                OnPropertyChanged("tabItem");
            }
            get
            {
                return _tabItem;     
            }
        }

        #endregion

        #region SearchContact

        private List<XmlExchange.contact> _contactData;
        public List<XmlExchange.contact> contactData
        {
            set
            {
                _contactData = value;
                OnPropertyChanged("contactData");
            }
            get
            {
                return _contactData;
            }
        }

        private XmlExchange.contact _SelectedContact;
        public XmlExchange.contact SelectedContact
        {
            set
            {
                _SelectedContact = value;
                OnPropertyChanged("SelectedContact");
            }
            get
            {
                return _SelectedContact;
            }
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

        private void NotifyChange_Search()
        {
            OnPropertyChanged("IsCompany");
            OnPropertyChanged("CanSearchCompany");
            OnPropertyChanged("CanSearchPerson");
        }

        #endregion

    }
}
