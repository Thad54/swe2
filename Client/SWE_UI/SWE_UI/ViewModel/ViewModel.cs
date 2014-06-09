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
        private readonly DelegateCommand<string> _SearchBillCommand;

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
                    if (!string.IsNullOrWhiteSpace(_FirstName) || !string.IsNullOrWhiteSpace(_LastName) || !string.IsNullOrWhiteSpace(_CompanyName) || !string.IsNullOrWhiteSpace(_UID))
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

            _SearchBillCommand = new DelegateCommand<string>(
                (s) =>
                {
                    var result = new List<XmlExchange.bill>();
                    //EmployingCompanyData_Edit = result;


                    try
                    {
                        if (_BillContact != null)
                        {
                            result = _proxy.searchBill(_BillContact.id, _DateFrom, _DateTo, _BillingAmount);
                        }
                        else
                        {
                            result = _proxy.searchBill(null, _DateFrom, _DateTo, _BillingAmount);
                        }
                    }
                    catch (Exception)
                    {
                        return;
                    }
                    if (result.Count == 0)
                    {
                        Contacts = "No Bill Found";
                        return;
                    }

                    if (result.Count == 1)
                    {
                        /*
                        clearEdit();
                        fillEdit(result.Last());*/

                        EditBillWindow _editBillWindow = new EditBillWindow();
                        EditBillViewModel _editViewModel = new EditBillViewModel();

                        _editBillWindow.DataContext = _editViewModel;
                        _editViewModel.setMainViewModel(this);
                        _editViewModel.setWindow(_editBillWindow);
                        _editViewModel.fillEdit(result.Last());

                        _editBillWindow.Show();
                        _BillData = result;
                        return;
                    }

                    _BillData = result;
                    
                }, //Execute
                (s) =>
                {
                    if (_BillContact != null || _DateFrom != null || _DateTo != null || _BillingAmount != null)
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
        public DelegateCommand<string> SearchBillClicked
        {
            get { return _SearchBillCommand; }
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
                if (string.IsNullOrWhiteSpace(_FirstName) && string.IsNullOrWhiteSpace(_LastName) && string.IsNullOrWhiteSpace(_CompanyName) && string.IsNullOrWhiteSpace(_UID))
                {
                    return null;
                }
                return !string.IsNullOrWhiteSpace(_CompanyName) || !string.IsNullOrWhiteSpace(_UID);
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

        #region SearchBill

        private List<XmlExchange.bill> _BillData;
        public List<XmlExchange.bill> BillData
        {
            get { return _BillData; }
            set
            {
                _BillData = value;
                //_SearchCommand.RaiseCanExecuteChanged();
                OnPropertyChanged("BillData");
            }
        }

        private XmlExchange.contact _BillContact;
        public XmlExchange.contact BillContact
        {
            get { return _BillContact; }
            set
            {
                _BillContact = value;
                _SearchBillCommand.RaiseCanExecuteChanged();
                //_SearchCommand.RaiseCanExecuteChanged();
                OnPropertyChanged("BillContact");
            }
        }

        private List<XmlExchange.contact> _BillContactDataSource;
        public List<XmlExchange.contact> BillContactDataSource
        {
            get { return _BillContactDataSource; }
            set
            {
                _BillContactDataSource = value;
                _SearchBillCommand.RaiseCanExecuteChanged();
                //_SearchCommand.RaiseCanExecuteChanged();
                OnPropertyChanged("BillContactDataSource");
            }
        }

        private DateTime _DateFrom;
        public DateTime DateFrom
        {
            get { return _DateFrom; }
            set
            {
                _DateFrom = value;
                _SearchBillCommand.RaiseCanExecuteChanged();
                //_SearchCommand.RaiseCanExecuteChanged();
                OnPropertyChanged("DateFrom");
            }
        }

        private DateTime _DateTo;
        public DateTime DateTo
        {
            get { return _DateTo; }
            set
            {
                _DateTo = value;
                _SearchBillCommand.RaiseCanExecuteChanged();
                //_SearchCommand.RaiseCanExecuteChanged();
                OnPropertyChanged("DateTo");
            }
        }

        private decimal? _BillingAmount;
        public decimal? BillingAmount
        {
            get { return _BillingAmount; }
            set
            {
                try
                {
                    _BillingAmount = Convert.ToDecimal(value);
                }
                catch (Exception e)
                {
                    _BillingAmount = null;
                }
                _SearchBillCommand.RaiseCanExecuteChanged();
                //_SearchCommand.RaiseCanExecuteChanged();
                OnPropertyChanged("BillingAmount");
            }
        }

        #endregion

    }
}
