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

        #region commands
        private readonly DelegateCommand<string> _SearchCommand;
        private readonly DelegateCommand<string> _EditCommand;

        public ViewModel()
        {
            _SearchCommand = new DelegateCommand<string>(
                (s) =>
                {

                    var proxy = new proxy();
                    var result = new List<XmlExchange.contact>();
                    try
                    {
                        if (CanSearchCompany == true)
                        {
                            result = proxy.searchCompany(_CompanyName, _UID);
                        }
                        else
                        {
                            result = proxy.searchPerson(_FirstName, _LastName);
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
                    var sb = new StringBuilder();

                    foreach (var con in result)
                    {
                        sb.AppendFormat("{0} | {1} | \n", con.name, con.lastName);
                    }
                    Contacts = sb.ToString();

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

            _EditCommand = new DelegateCommand<string>(
                (s) =>
                {

                    var proxy = new proxy();
                    var result = new XmlExchange.message();
                    var contact = new XmlExchange.contact();

                    if (string.IsNullOrWhiteSpace(_UID_Edit))
                    {
                        contact.title = _Title_Edit;
                        contact.name = _FirstName_Edit;
                        contact.lastName = _LastName_Edit;
                        contact.Suffix = _Suffix_Edit;
                        contact.uid = string.Empty;
                        contact.companyID = _CompanyID_Edit;
                    }
                    else
                    {
                        contact.title = string.Empty;
                        contact.Suffix = string.Empty;
                        contact.name = _CompanyName_Edit;
                        contact.uid = _UID_Edit;
                        contact.lastName = string.Empty;
                        contact.companyID = null;

                    }

                    contact.creationDate = _CreationDate_Edit;
                    contact.address = _Address_Edit;
                    contact.billingAddress = _BillingAddress_Edit;
                    contact.shippingAddress = _ShippingAddress_Edit;
                    contact.id = _ContactID;

                    try
                    {
                        result = proxy.EditContact(contact);
                    }
                    catch (Exception)
                    {
                        return;
                    }
                    if (result.error)
                    {
                        System.Windows.MessageBox.Show(result.text);
                    }

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

        public DelegateCommand<string> EditContactClicked
        {
            get { return _EditCommand; }
        }
        #endregion

        #region SearchContact
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

        private void NotifyChange_Search()
        {
            OnPropertyChanged("IsCompany");
            OnPropertyChanged("CanSearchCompany");
            OnPropertyChanged("CanSearchPerson");
        }

        #endregion

        #region EditContact

        private int? _ContactID = 2;

        private int? _CompanyID_Edit = 0;

        private DateTime _CreationDate_Edit = DateTime.Now;
        public DateTime CreationDate_Edit
        {
            get { return _CreationDate_Edit; }
            set
            {
                _CreationDate_Edit = value;
                //           _clickCommand.RaiseCanExecuteChanged();
            }
        }

        private string _Title_Edit;
        public string Title_Edit
        {
            get { return _Title_Edit; }
            set
            {
                _Title_Edit = value;
                _EditCommand.RaiseCanExecuteChanged();
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
                _EditCommand.RaiseCanExecuteChanged();
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
                _EditCommand.RaiseCanExecuteChanged();
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
                _EditCommand.RaiseCanExecuteChanged();
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
                _EditCommand.RaiseCanExecuteChanged();
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
                _EditCommand.RaiseCanExecuteChanged();
                NotifyChange_Edit();
                //           _clickCommand.RaiseCanExecuteChanged();
            }
        }

        private string _Address_Edit;
        public string Address_Edit
        {
            get { return _Address_Edit; }
            set
            {
                _Address_Edit = value;
            }
        }

        private string _ShippingAddress_Edit;
        public string ShippingAddress_Edit
        {
            get { return _ShippingAddress_Edit; }
            set
            {
                _ShippingAddress_Edit = value;
            }
        }

        private string _BillingAddress_Edit;
        public string BillingAddress_Edit
        {
            get { return _BillingAddress_Edit; }
            set
            {
                _BillingAddress_Edit = value;
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
