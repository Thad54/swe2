using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace SWE_UI.ViewModel
{
    class EditBillViewModel : BaseViewModel
    {
        private proxy _proxy  = new SWE_UI.proxy();

        void WindowClosing(object sender, CancelEventArgs e){
            if (ContactAltered() && !_submit)
            {
                MessageBoxResult result =
                MessageBox.Show("You are about to abandon the changes made to the Bill. \nDo you wish to continue?",
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
        private EditBillWindow _window;
        public void setWindow(EditBillWindow window)
        {
            _window = window;
            _window.Closing += new CancelEventHandler(WindowClosing);
        }

        private readonly DelegateCommand<string> _EditCommand;
        private readonly DelegateCommand<string> _RestoreEditCommand;

        public EditBillViewModel()
        {

            _EditCommand = new DelegateCommand<string>(
                (s) =>
                {
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
        } 

        public DelegateCommand<string> EditContactClicked
        {
            get { return _EditCommand; }
        }

        public DelegateCommand<string> RestoreEditClicked
        {
            get { return _RestoreEditCommand; }
        }

        public void clearEdit()
        {
        }
        public void fillEdit(XmlExchange.bill bill)
        {
            _originalBill = bill;
            Message = bill.message;
            Comment = bill.comment;
            BillDate = bill.BillingDate;
            DueDate = bill.DueByDate;

            var part1 = _proxy.searchCompany("","");
            var part2 = _proxy.searchPerson("", "");
            part1.AddRange(part2);

            BillContactData = part1;
            foreach (var item in _BillContactData)
            {
                if (item.id == bill.contactId)
                {
                    BillContact = item;
                }
            }

            BillingPositionData = bill.billingPositions;
            calculateBillingAmount();

        }

        private void calculateBillingAmount()
        {
            decimal? net = 0;
            decimal? brut = 0;

            foreach (var bp in _BillingPositionData)
            {
                net += bp.price * bp.amount;
                brut += bp.price * bp.amount * bp.tax;
            }
            Netto = net;
            Brutto = brut;
        }

        private void RestoreEdit(){
        }

        private bool ContactAltered()
        {
            /*if (_originalContact.address != Address_Edit)
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
            }*/
            return false;
        }




        //private int? _ContactID;

        private XmlExchange.bill _originalBill;

        private string _Message;
        public string Message
        {
            set
            {
                _Message = value;
                OnPropertyChanged("Message");
                _EditCommand.RaiseCanExecuteChanged();
            }
            get
            {
                return _Message;
            }
        }

        private string _Comment;
        public string Comment
        {
            set
            {
                _Comment = value;
                OnPropertyChanged("Comment");
                _EditCommand.RaiseCanExecuteChanged();
            }
            get
            {
                return _Comment;
            }
        }

        private XmlExchange.contact _BillContact;
        public XmlExchange.contact BillContact
        {
            get
            {
                return _BillContact;
            }

            set
            {
                _BillContact = value;
                OnPropertyChanged("BillContact");
                _EditCommand.RaiseCanExecuteChanged();
            }
        }

        private List<XmlExchange.contact> _BillContactData;
        public List<XmlExchange.contact> BillContactData
        {
            set
            {
                _BillContactData = value;
                OnPropertyChanged("BillContactData");
                _EditCommand.RaiseCanExecuteChanged();
            }
            get
            {

                return _BillContactData;
            }
        }

        private XmlExchange.billingPosition _NewBillingPosition;
        public XmlExchange.billingPosition NewBillingPosition
        {
            get
            {
                return _NewBillingPosition;
            }

            set
            {
                _NewBillingPosition = value;
                OnPropertyChanged("NewBillingPosition");
                _EditCommand.RaiseCanExecuteChanged();
            }
        }

        private XmlExchange.billingPosition _SelectedBillingPosition;
        public XmlExchange.billingPosition SelectedBillingPosition
        {
            get
            {
                return _SelectedBillingPosition;
            }

            set
            {
                _SelectedBillingPosition = value;
                OnPropertyChanged("SelectedBillingPosition");
                _EditCommand.RaiseCanExecuteChanged();
            }
        }

        private ObservableCollection<XmlExchange.billingPosition> _BillingPositionData;
        public ObservableCollection<XmlExchange.billingPosition> BillingPositionData
        {
            set
            {
                _BillingPositionData = value;
                OnPropertyChanged("BillingPositionData");
                _EditCommand.RaiseCanExecuteChanged();
            }
            get
            {

                return _BillingPositionData;
            }
        }

        private decimal? _Netto = 0;
        public decimal? Netto
        {
            set
            {
                _Netto = value;
                OnPropertyChanged("Netto");
                _EditCommand.RaiseCanExecuteChanged();
            }
            get
            {
                return _Netto;
            }
        }

        private decimal? _Brutto = 0;
        public decimal? Brutto
        {
            set
            {
                _Brutto = value;
                OnPropertyChanged("Brutto");
                _EditCommand.RaiseCanExecuteChanged();
            }
            get
            {
                return _Brutto;
            }
        }

        private DateTime? _BillDate;
        public DateTime? BillDate
        {
            set
            {
                if (BillDate != null || !BillDate.Equals(value))
                {
                    _BillDate = value;
                    OnPropertyChanged("BillDate");
                    _EditCommand.RaiseCanExecuteChanged();
                }
            }
            get
            {
                return _BillDate;
            }
        }

        private DateTime? _DueDate;
        public DateTime? DueDate
        {
            set
            {
                if (DueDate != null || !DueDate.Equals(value))
                {
                    _DueDate = value;
                    OnPropertyChanged("DueDate");
                    _EditCommand.RaiseCanExecuteChanged();
                }
            }
            get
            {
                return _DueDate;
            }
        }
    }
}
