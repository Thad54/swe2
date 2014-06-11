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

        private readonly DelegateCommand<string> _EditBillCommand;
        private readonly DelegateCommand<string> _RestoreEditCommand;
        private readonly DelegateCommand<string> _PrintCommand;
        private readonly DelegateCommand<string> _RemoveBillingPositionCommand;
        private readonly DelegateCommand<string> _AddBillingPositionCommand;
        private readonly DelegateCommand<string> _EditBillingPositionCommand;
        private readonly DelegateCommand<string> _SelectBillingPositionCommand;

        public EditBillViewModel()
        {

            _EditBillCommand = new DelegateCommand<string>(
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
                        //RestoreEdit();
                        fillEdit(_originalBill);
                    }
                    _EditBillCommand.RaiseCanExecuteChanged();
                }, //Execute
                (s) =>
                {
                    return true;

                } //CanExecute
                );
        } 

        public DelegateCommand<string> EditBillClicked
        {
            get { return _EditBillCommand; }
        }

        public DelegateCommand<string> RestoreEditClicked
        {
            get { return _RestoreEditCommand; }
        }

        public DelegateCommand<string> PrintClicked
        {
            get { return _PrintCommand; }
        }
        public DelegateCommand<string> RemoveBillingPositionClicked
        {
            get { return _RemoveBillingPositionCommand; }
        }
        public DelegateCommand<string> AddBillingPositionClicked
        {
            get { return _AddBillingPositionCommand; }
        }
        public DelegateCommand<string> EditBillingPositionClicked
        {
            get { return _EditBillingPositionCommand; }
        }
        public DelegateCommand<string> SelectBillingPositionClicked
        {
            get { return _SelectBillingPositionCommand; }
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
            if (_originalBill.BillingDate != _BillDate)
            {
                return true;
            }
            if (_originalBill.comment != _Comment)
            {
                return true;
            }
            if (_BillContact != null)
            {
                if (_originalBill.contactId != _BillContact.id)
                {
                    return true;
                }
            }
            else
            {
                return true;
            }

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
                _EditBillCommand.RaiseCanExecuteChanged();
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
                _EditBillCommand.RaiseCanExecuteChanged();
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
                _EditBillCommand.RaiseCanExecuteChanged();
            }
        }

        private List<XmlExchange.contact> _BillContactData;
        public List<XmlExchange.contact> BillContactData
        {
            set
            {
                _BillContactData = value;
                OnPropertyChanged("BillContactData");
                _EditBillCommand.RaiseCanExecuteChanged();
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
                _EditBillCommand.RaiseCanExecuteChanged();
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
                _EditBillCommand.RaiseCanExecuteChanged();
            }
        }

        private ObservableCollection<XmlExchange.billingPosition> _BillingPositionData;
        public ObservableCollection<XmlExchange.billingPosition> BillingPositionData
        {
            set
            {
                _BillingPositionData = value;
                OnPropertyChanged("BillingPositionData");
                _EditBillCommand.RaiseCanExecuteChanged();
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
                _EditBillCommand.RaiseCanExecuteChanged();
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
                _EditBillCommand.RaiseCanExecuteChanged();
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
                    _EditBillCommand.RaiseCanExecuteChanged();
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
                    _EditBillCommand.RaiseCanExecuteChanged();
                }
            }
            get
            {
                return _DueDate;
            }
        }

        private string _NewBillingPositionName;
        public string NewBillingPositionName
        {
            set
            {
                _NewBillingPositionName = value;
                OnPropertyChanged("NewBillingPositionName");
                _EditBillCommand.RaiseCanExecuteChanged();
            }
            get
            {
                return _NewBillingPositionName;
            }
        }

        private string _EditBillingPositionName;
        public string EditBillingPositionName
        {
            set
            {
                _EditBillingPositionName = value;
                OnPropertyChanged("EditBillingPositionName");
                _EditBillCommand.RaiseCanExecuteChanged();
            }
            get
            {
                return _EditBillingPositionName;
            }
        }

        private decimal? _NewBillingPositionPrice;
        public decimal? NewBillingPositionPrice
        {
            set
            {
                _NewBillingPositionPrice = value;
                OnPropertyChanged("NewBillingPositionPrice");
                _EditBillCommand.RaiseCanExecuteChanged();
            }
            get
            {
                return _NewBillingPositionPrice;
            }
        }

        private decimal? _EditBillingPositionPrice;
        public decimal? EditBillingPositionPrice
        {
            set
            {
                _EditBillingPositionPrice = value;
                OnPropertyChanged("EditBillingPositionPrice");
                _EditBillCommand.RaiseCanExecuteChanged();
            }
            get
            {
                return _EditBillingPositionPrice;
            }
        }

        private decimal? _EditBillingPositionTax;
        public decimal? EditBillingPositionTax
        {
            set
            {
                _EditBillingPositionTax = value;
                OnPropertyChanged("EditBillingPositionTax");
                _EditBillCommand.RaiseCanExecuteChanged();
            }
            get
            {
                return _EditBillingPositionTax;
            }
        }

        private decimal? _NewBillingPositionTax;
        public decimal? NewBillingPositionTax
        {
            set
            {
                _NewBillingPositionTax = value;
                OnPropertyChanged("NewBillingPositionTax");
                _EditBillCommand.RaiseCanExecuteChanged();
            }
            get
            {
                return _NewBillingPositionTax;
            }
        }

        private int? _NewBillingPositionAmount;
        public int? NewBillingPositionAmount
        {
            set
            {
                _NewBillingPositionAmount = value;
                OnPropertyChanged("NewBillingPositionAmount");
                _EditBillCommand.RaiseCanExecuteChanged();
            }
            get
            {
                return _NewBillingPositionAmount;
            }
        }

        private int? _EditBillingPositionAmount;
        public int? EditBillingPositionAmount
        {
            set
            {
                _EditBillingPositionAmount = value;
                OnPropertyChanged("EditBillingPositionAmount");
                _EditBillCommand.RaiseCanExecuteChanged();
            }
            get
            {
                return _EditBillingPositionAmount;
            }
        }

        private bool _EditActive;
        public bool EditActive
        {
            set
            {
                _EditActive = value;
                OnPropertyChanged("EditActive");
                _EditBillCommand.RaiseCanExecuteChanged();
            }
            get
            {
                return _EditActive;
            }
        }

        private bool _CanRemoveBillingPosition;
        public bool CanRemoveBillingPosition
        {
            set
            {
                _CanRemoveBillingPosition = value;
                OnPropertyChanged("CanRemoveBillingPosition");
                _EditBillCommand.RaiseCanExecuteChanged();
            }
            get
            {
                return _CanRemoveBillingPosition;
            }
        }
    }
}
