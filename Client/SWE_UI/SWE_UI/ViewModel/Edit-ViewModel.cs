using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace SWE_UI.ViewModel
{
    class Edit_ViewModel : BaseViewModel
    {
        private proxy _proxy  = new SWE_UI.proxy();

        void WindowClosing(object sender, CancelEventArgs e){
            if (ContactAltered() && !_submit)
            {
                MessageBoxResult result =
                MessageBox.Show("You are about to abandon the changes made to the Contact " + _originalContact.name + ". \nDo you wish to continue?",
                "", MessageBoxButton.OKCancel);

                if (result == MessageBoxResult.OK)
                {
                    e.Cancel = false;
                    return;
                }
                else
                {
                    e.Cancel = true;
                    return;
                }
            }
            else
            {
                e.Cancel = false;
            }
        }

        private ViewModel _mainViewModel;
        public void setMainViewModel(ViewModel view)
        {
            _mainViewModel = view;

        }

        private bool _submit = false;
        private EditContactWIndow _window;
        public void setWindow(EditContactWIndow window)
        {
            _window = window;
            _window.Closing += new CancelEventHandler(WindowClosing);
        }

        private readonly DelegateCommand<string> _EditCommand;
        private readonly DelegateCommand<string> _RestoreEditCommand;
        private readonly DelegateCommand<string> _LoadAllCompaniesCommand;
        private readonly DelegateCommand<string> _SearchCompanyRefCommand;

        public Edit_ViewModel()
        {

            _EditCommand = new DelegateCommand<string>(
                (s) =>
                {

                    var result = new XmlExchange.message();
                    var contact = new XmlExchange.contact();

                    if (string.IsNullOrWhiteSpace(_UID_Edit))
                    {
                        contact.title = _Title_Edit;
                        contact.name = _FirstName_Edit;
                        contact.lastName = _LastName_Edit;
                        contact.Suffix = _Suffix_Edit;
                        contact.uid = string.Empty;
                        contact.companyID = _CompanyID_Edit.id;
                        contact.company = _CompanyID_Edit.name;
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
                        result = _proxy.EditContact(contact);
                    }
                    catch (Exception)
                    {
                        return;
                    }
                    if (result.error)
                    {
                        System.Windows.MessageBox.Show(result.text);
                    }

                    var searchResults = _mainViewModel.contactData;
                    var newList = new List<XmlExchange.contact>();

                    foreach (var elem in searchResults)
                    {
                        if (elem.id == contact.id)
                        {
                            elem.setData(contact);
                        }
                        newList.Add(elem);
                    }

                    _mainViewModel.contactData = newList;

                    _submit = true;
                    _window.Close();
                   /* EmployingCompanyData_Edit = _proxy.searchCompany("", "");
                    clearEdit();*/

                }, //Execute
                (s) =>
                {
                    return ContactAltered();
                } //CanExecute
                );

            _RestoreEditCommand = new DelegateCommand<string>(
                (s) =>
                {
                    if (ContactAltered())
                    {
                        RestoreEdit();
                    }
                    _EditCommand.RaiseCanExecuteChanged();
                }, //Execute
                (s) =>
                {
                    return true;

                } //CanExecute
                );

            _LoadAllCompaniesCommand = new DelegateCommand<string>(
                (s) =>
                {
                    EmployingCompanyData_Edit = _proxy.searchCompany("", "");
                }, //Execute
                (s) =>
                {
                    return true;

                } //CanExecute
                );

            _SearchCompanyRefCommand = new DelegateCommand<string>(
                (s) =>
                {
                    var result = _proxy.searchCompany(_CompanyRefName, "");
                    if (result.Count == 0)
                    {
                        CompanyRefName = "No Company found";
                        CompanyID_Edit = null;
                        return;

                    }
                    else if (result.Count == 1)
                    {
                        EmployingCompanyData_Edit = result;
                        CompanyID_Edit = _EmployingCompanyData_Edit[0];
                        return;
                    }
                    EmployingCompanyData_Edit = result;
                    CompanySelectorOpen = true;
                }, //Execute
                (s) =>
                {
                    return true;

                } //CanExecute
                );
        } 

        public DelegateCommand<string> EditContactClicked
        {
            get { return _EditCommand; }
        }

        public DelegateCommand<string> RestoreEditClicked
        {
            get { return _RestoreEditCommand; }
        }

        public DelegateCommand<string> SearchCompanyRef_Edit
        {
            get { return _SearchCompanyRefCommand; }
        }

        public DelegateCommand<string> LoadAllCompanies_Edit
        {
            get { return _LoadAllCompaniesCommand; }
        }

        public void clearEdit()
        {
            _ContactID = null;
            Address_Edit = string.Empty;
            BillingAddress_Edit = string.Empty;
            ShippingAddress_Edit = string.Empty;
            CreationDate_Edit = DateTime.Now;
            Title_Edit = string.Empty;
            FirstName_Edit = string.Empty;
            LastName_Edit = string.Empty;
            Suffix_Edit = string.Empty;
            CompanyName_Edit = string.Empty;
            UID_Edit = string.Empty;
            CompanyID_Edit = null;
            _EditCommand.RaiseCanExecuteChanged();
        }
        public void fillEdit(XmlExchange.contact con)
        {

            _originalContact = con;

            if (con.isCompany == false)
            {
                Title_Edit = con.title;
                FirstName_Edit = con.name;
                LastName_Edit = con.lastName;
                Suffix_Edit = con.Suffix;

                EmployingCompanyData_Edit = _proxy.searchCompany("", "");

                foreach (var item in _EmployingCompanyData_Edit)
                {
                    if (item.id == con.companyID)
                    {
                        CompanyID_Edit = item;
                    }
                }
            }
            else
            {
                CompanyName_Edit = con.name;
                UID_Edit = con.uid;
            }


            _ContactID = con.id;
            _EditCommand.RaiseCanExecuteChanged();
            Address_Edit = con.address;
            BillingAddress_Edit = con.billingAddress;
            ShippingAddress_Edit = con.shippingAddress;

            CreationDate_Edit = con.creationDate;

        }

        private void RestoreEdit(){
            if (_originalContact.isCompany == false)
            {
                Title_Edit = _originalContact.title;
                FirstName_Edit = _originalContact.name;
                LastName_Edit = _originalContact.lastName;
                Suffix_Edit = _originalContact.Suffix;

                EmployingCompanyData_Edit = _proxy.searchCompany("", "");

                foreach (var item in _EmployingCompanyData_Edit)
                {
                    if (item.id == _originalContact.companyID)
                    {
                        CompanyID_Edit = item;
                    }
                }
            }
            else
            {
                CompanyName_Edit = _originalContact.name;
                UID_Edit = _originalContact.uid;
            }

            _EditCommand.RaiseCanExecuteChanged();
            Address_Edit = _originalContact.address;
            BillingAddress_Edit = _originalContact.billingAddress;
            ShippingAddress_Edit = _originalContact.shippingAddress;

            CreationDate_Edit = _originalContact.creationDate;
        }

        private bool ContactAltered()
        {
            if (_originalContact.address != Address_Edit)
            {
                return true;
            }
            if (_originalContact.billingAddress != BillingAddress_Edit)
            {
                return true;
            }
            if (_originalContact.creationDate != CreationDate_Edit)
            {
                return true;
            }
            if (_originalContact.isCompany == false)
            {
                if (_originalContact.lastName != LastName_Edit)
                {
                    return true;
                }
                if (CompanyID_Edit == null)
                {
                    if (_originalContact.companyID == null)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                } else if (_originalContact.companyID != CompanyID_Edit.id)
                {
                    return true;
                }
                if (_originalContact.name != FirstName_Edit)
                {
                    return true;
                }
                if (_originalContact.title != Title_Edit)
                {
                    return true;
                }
                if (_originalContact.Suffix != Suffix_Edit)
                {
                    return true;
                }
            }
            else if (_originalContact.isCompany == true)
            {
                if (_originalContact.name != CompanyName_Edit)
                {
                    return true;
                }
                if (_originalContact.uid != UID_Edit)
                {
                    return true;
                }
            }
            return false;
        }




        private int? _ContactID;

        private XmlExchange.contact _originalContact;

        private bool _CompanySelectorOpen;
        public bool CompanySelectorOpen
        {
            set
            {
                _CompanySelectorOpen = value;
                OnPropertyChanged("CompanySelectorOpen");
                _EditCommand.RaiseCanExecuteChanged();
            }
            get
            {
                return _CompanySelectorOpen;
            }
        }

        private string _CompanyRefName;
        public string CompanyRefName
        {
            set
            {
                _CompanyRefName = value;
                OnPropertyChanged("CompanyRefName");
                _EditCommand.RaiseCanExecuteChanged();
            }
            get
            {
                return _CompanyRefName;
            }
        }

        private XmlExchange.contact _CompanyID_Edit;
        public XmlExchange.contact CompanyID_Edit
        {
            get
            {
                return _CompanyID_Edit;
            }

            set
            {
                _CompanyID_Edit = value;
                OnPropertyChanged("CompanyID_Edit");
                _EditCommand.RaiseCanExecuteChanged();
            }
        }

        private List<XmlExchange.contact> _EmployingCompanyData_Edit;
        public List<XmlExchange.contact> EmployingCompanyData_Edit
        {
            set
            {
                _EmployingCompanyData_Edit = value;
                var empty = new XmlExchange.contact();
                empty.id = null;
                _EmployingCompanyData_Edit.Add(empty);
                OnPropertyChanged("EmployingCompanyData_Edit");
                _EditCommand.RaiseCanExecuteChanged();
            }
            get
            {
                //var list = new List<string>();

                /*_EmployingCompanyData_Edit = _proxy.searchCompany("", "");

                var empty = new XmlExchange.contact();
                empty.id = null;
                _EmployingCompanyData_Edit.Add(empty);*/


                return _EmployingCompanyData_Edit;
            }
        }

        private DateTime _CreationDate_Edit = DateTime.Now;
        public DateTime CreationDate_Edit
        {
            get { return _CreationDate_Edit; }
            set
            {
                _CreationDate_Edit = value;
                OnPropertyChanged("CreationDate_Edit");
                _EditCommand.RaiseCanExecuteChanged();
            }
        }

        private string _Title_Edit;
        public string Title_Edit
        {
            get { return _Title_Edit; }
            set
            {
                _Title_Edit = value;
                NotifyChange_Edit();
                OnPropertyChanged("Title_Edit");
                _EditCommand.RaiseCanExecuteChanged();
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
                OnPropertyChanged("FirstName_Edit");
                _EditCommand.RaiseCanExecuteChanged();
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
                OnPropertyChanged("LastName_Edit");
                _EditCommand.RaiseCanExecuteChanged();
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
                OnPropertyChanged("Suffix_Edit");
                _EditCommand.RaiseCanExecuteChanged();
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
                OnPropertyChanged("CompanyName_Edit");
                _EditCommand.RaiseCanExecuteChanged();
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
                OnPropertyChanged("UID_Edit");
                _EditCommand.RaiseCanExecuteChanged();
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
                OnPropertyChanged("Address_Edit");
                _EditCommand.RaiseCanExecuteChanged();
            }
        }

        private string _ShippingAddress_Edit;
        public string ShippingAddress_Edit
        {
            get { return _ShippingAddress_Edit; }
            set
            {
                _ShippingAddress_Edit = value;
                OnPropertyChanged("ShippingAddress_Edit");
                _EditCommand.RaiseCanExecuteChanged();
            }
        }

        private string _BillingAddress_Edit;
        public string BillingAddress_Edit
        {
            get { return _BillingAddress_Edit; }
            set
            {
                _BillingAddress_Edit = value;
                OnPropertyChanged("BillingAddress_Edit");
                _EditCommand.RaiseCanExecuteChanged();
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
    }
}
