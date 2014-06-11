using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.ComponentModel;
using System.Collections.ObjectModel;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using PdfSharp.Pdf;


namespace SWE_UI.ViewModel
{
    public class ViewModel : BaseViewModel
    {
        private proxy _proxy = new proxy(); 

        #region commands
        private readonly DelegateCommand<string> _SearchCommand;
        private readonly DelegateCommand<string> _SelectionCommand;
        private readonly DelegateCommand<string> _SearchBillCommand;
        private readonly DelegateCommand<string> _BillSelectionCommand;
        private readonly DelegateCommand<string> _EditBillCommand;
        private readonly DelegateCommand<string> _RestoreEditCommand;
        private readonly DelegateCommand<string> _PrintCommand;
        private readonly DelegateCommand<string> _RemoveBillingPositionCommand;
        private readonly DelegateCommand<string> _AddBillingPositionCommand;
        private readonly DelegateCommand<string> _EditBillingPositionCommand;

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

            _BillSelectionCommand = new DelegateCommand<string>(
                (s) =>
                {
                    EditBillWindow _editBillWindow = new EditBillWindow();
                    EditBillViewModel _editViewModel = new EditBillViewModel();

                    _editBillWindow.DataContext = _editViewModel;
                    _editViewModel.setMainViewModel(this);
                    _editViewModel.setWindow(_editBillWindow);
                    _editViewModel.fillEdit(SelectedBill);

                    _editBillWindow.Show();
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
                            result = _proxy.searchBill(_BillContact.id, _DateFrom, _DateTo, _BillingAmountFrom, _BillingAmountTo);
                        }
                        else
                        {
                            result = _proxy.searchBill(null, _DateFrom, _DateTo, _BillingAmountFrom, _BillingAmountTo);
                        }
                    }
                    catch (Exception)
                    {
                        return;
                    }
                    if (result.Count == 0)
                    {
                        //Contacts = "No Bill Found";
                        BillData = new List<XmlExchange.bill>();
                        return;
                    }

                    if (result.Count == 1)
                    {

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

                    BillData = result;
                    
                }, //Execute
                (s) =>
                {
                    if (_BillContact != null || _DateFrom != null || _DateTo != null || _BillingAmountFrom != null || _BillingAmountTo != null)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                } //CanExecute
                );

            _EditBillCommand = new DelegateCommand<string>(
                (s) =>
                {
                    var bill = new XmlExchange.bill();

                    bill.BillingDate = _BillDate;
                    bill.billingPositions = _BillingPositionData;
                    bill.comment = _Comment;
                    bill.contact = _BillContactNew;
                    if (_BillContactNew != null)
                    {
                        bill.contactId = _BillContactNew.id;
                    }
                    else
                    {
                        bill.contactId = null;
                    }
                    bill.DueByDate = _DueDate;
                    bill.ID = null;
                    bill.message = _Message;
                    bill.billAmount = Brutto;

                    _originalBill = bill;
                    _PrintCommand.RaiseCanExecuteChanged();
                    

                    var message = _proxy.AddBill(bill);

                    if(message.error){
                        MessageBox.Show(message.text);
                        return;
                    }

                }, //Execute
                (s) =>
                {
                    if (BillingPositionData == null)
                    {
                        return false;
                    }
                    if(BillingPositionData.Count == 0){
                        return false;
                    }
                    if(BillDate != null && DueDate != null){
                        return true;
                    } else {
                        return false;
                    }
                } //CanExecute
                );

            _RestoreEditCommand = new DelegateCommand<string>(
                (s) =>
                {
                    clearEdit();
                    _SearchBillCommand.RaiseCanExecuteChanged();
                }, //Execute
                (s) =>
                {
                    return true;

                } //CanExecute
                );

            _RemoveBillingPositionCommand = new DelegateCommand<string>(
                (s) =>
                {
                    BillingPositionData.Remove(_SelectedBillingPosition);
                    SelectedBillingPosition = null;
                    calculateBillingAmount();
                    _EditBillCommand.RaiseCanExecuteChanged();
                    _PrintCommand.RaiseCanExecuteChanged();

                }, //Execute
                (s) =>
                {
                    if (_SelectedBillingPosition != null)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                } //CanExecute
                );

            _EditBillingPositionCommand = new DelegateCommand<string>(
                (s) =>
                {

                    var bp = new XmlExchange.billingPosition();

                    bp.name = _EditBillingPositionName;
                    bp.price = _EditBillingPositionPrice;
                    bp.amount = _EditBillingPositionAmount;
                    bp.tax = _EditBillingPositionTax;

                    BillingPositionData.Remove(SelectedBillingPosition);
                    BillingPositionData.Add(bp);

                    OnPropertyChanged("BillingPositionData");

                    SelectedBillingPosition = null;
                    calculateBillingAmount();
                    _EditBillCommand.RaiseCanExecuteChanged();
                    _PrintCommand.RaiseCanExecuteChanged();

                }, //Execute
                (s) =>
                {
                    if(_SelectedBillingPosition == null){
                        return false;
                    }
                    if (string.IsNullOrWhiteSpace(_EditBillingPositionName) || _EditBillingPositionAmount == null || _EditBillingPositionPrice == null || _EditBillingPositionTax == null)
                    {
                        return false;
                    }
                    if(_SelectedBillingPosition.name != _EditBillingPositionName || _SelectedBillingPosition.price != _EditBillingPositionPrice || _SelectedBillingPosition.tax != _EditBillingPositionTax || _SelectedBillingPosition.amount != _EditBillingPositionAmount)
                    {
                        return true;
                    } else {
                        return false;
                    }

                } //CanExecute
                );

            _AddBillingPositionCommand = new DelegateCommand<string>(
                (s) =>
                {
                    if (BillingPositionData == null)
                    {
                        BillingPositionData = new ObservableCollection<XmlExchange.billingPosition>();
                    }
                    var bp = new XmlExchange.billingPosition();
                    bp.name = _NewBillingPositionName;
                    bp.price = _NewBillingPositionPrice;
                    bp.tax = _NewBillingPositionTax;
                    bp.amount = _NewBillingPositionAmount;

                    BillingPositionData.Add(bp);
                    calculateBillingAmount();
                    clearNewBillingPosition();

                    _EditBillCommand.RaiseCanExecuteChanged();
                    _PrintCommand.RaiseCanExecuteChanged();

                }, //Execute
                (s) =>
                {
                    if( !string.IsNullOrWhiteSpace(_NewBillingPositionName) && _NewBillingPositionAmount != null && _NewBillingPositionPrice != null && _NewBillingPositionTax != null )
                    {
                        return true;
                    } else{
                        return false;
                    }


                } //CanExecute
                );

            _PrintCommand = new DelegateCommand<string>(
                (s) =>
                    {
                    var bill = _originalBill;

                    XmlExchange.contact Kunde = bill.contact;
                    Document document = new Document();
                    Section section = document.AddSection();
                    section.AddParagraph("Rechnung "+bill.ID.ToString() +" für "+ bill.BillingDate.ToString());
                    section.AddParagraph();
                    if (Kunde.isCompany == true)//Kunde ist eine Firma
                    {
                        section.AddParagraph(Kunde.company + " " + Kunde.uid + "\n" + Kunde.billingAddress);
                    }else{
                        section.AddParagraph(Kunde.title + " " + Kunde.name + " " + Kunde.lastName + Kunde.Suffix + "\n" + Kunde.billingAddress + "\n");

                    }
                    section.AddParagraph();
                    section.AddParagraph("Rechnung über folgenge Posten.");
                    section.AddParagraph();

                    decimal netto = 0;
                    decimal nettogesamt = 0;
                    decimal brutto = 0;
                    decimal bruttogesamt = 0;
                    foreach (XmlExchange.billingPosition zeile in bill.billingPositions)
                    {


                        netto = (int)zeile.amount * (decimal)zeile.price;
                        brutto = netto + (decimal)zeile.tax;
                        section.AddParagraph(zeile.amount + "x " + zeile.name + " Netto: " + (zeile.price * zeile.amount) + " Brutto: " + (zeile.price * zeile.amount * zeile.tax));
   //                     section.AddParagraph(zeile.amount.ToString() + "\t á " + zeile.price.ToString() + "\t:" + netto.ToString());
                        nettogesamt += netto;
                        bruttogesamt += brutto;
                    }
                    section.AddParagraph();
                    section.AddParagraph("Gesammtpreis (ohne Steuer): € " + nettogesamt.ToString() + "\nGesamtpreis (inkl. Steuer): € " + bruttogesamt.ToString());
                    section.AddParagraph();

                    DateTime date = (DateTime)bill.DueByDate;

                    section.AddParagraph("Der Rechnungsbetrag ist bis inkl. " + date.ToString("dd.MM.yyyy") + " zu bezahlen");
                    section.AddParagraph();

                    section.AddParagraph(bill.message);
                    section.AddParagraph();

                    section.AddParagraph(bill.comment );
                    section.AddParagraph();

                    PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer(false, PdfFontEmbedding.Always);

                    DateTime formatDate = (DateTime)bill.BillingDate;
                    pdfRenderer.Document = document;
                    pdfRenderer.RenderDocument();
                    string filename = "Bill" + bill.ID + "_" + Kunde.name + "_" + formatDate.ToString("dd.MM.yyyy");// + ".pdf";//hier gehört der pfad hin

                    if (System.IO.File.Exists(filename + ".pdf"))
                    {
                        filename = filename + "_Newer.pdf";
                    }
                    else
                    {
                        filename = filename + ".pdf";
                    }
                    if (System.IO.File.Exists(filename + "_Newer.pdf"))
                    {
                        System.IO.File.Delete(filename + "_Newer.pdf");
                        filename = filename + "_Newer.pdf";
                    }
                    pdfRenderer.PdfDocument.Save(filename);
                    System.Diagnostics.Process.Start(filename);

                }, //Execute
                (s) =>
                {
                    if(_originalBill == null)
                    {
                        return false;
                    } else {
                        return true;
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
        public DelegateCommand<string> BillSelected
        {
            get { return _BillSelectionCommand; }
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

        private XmlExchange.bill _SelectedBill;
        public XmlExchange.bill SelectedBill
        {
            set
            {
                _SelectedBill = value;
                OnPropertyChanged("SelectedBill");
            }
            get
            {
                return _SelectedBill;
            }
        }

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

        private DateTime? _DateFrom;
        public DateTime? DateFrom
        {
            get { return _DateFrom; }
            set
            {
                if (DateFrom != null || !DateFrom.Equals(value))
                {
                    _DateFrom = value;
                    _SearchBillCommand.RaiseCanExecuteChanged();
                    //_SearchCommand.RaiseCanExecuteChanged();
                    OnPropertyChanged("DateFrom");
                }
            }
        }

        private DateTime? _DateTo;
        public DateTime? DateTo
        {
            get { return _DateTo; }
            set
            {
                if (DateTo != null || !DateTo.Equals(value))
                {
                    _DateTo = value;
                    _SearchBillCommand.RaiseCanExecuteChanged();
                    //_SearchCommand.RaiseCanExecuteChanged();
                    OnPropertyChanged("DateTo");
                }
            }
        }

        private decimal? _BillingAmountFrom;
        public decimal? BillingAmountFrom
        {
            get { return _BillingAmountFrom; }
            set
            {
               /* try
                {
                    _BillingAmountFrom = Convert.ToDecimal(value);
                }
                catch
                {
                    _BillingAmountFrom = null;
                }*/
                _BillingAmountFrom = value;
                _SearchBillCommand.RaiseCanExecuteChanged();
                //_SearchCommand.RaiseCanExecuteChanged();
                OnPropertyChanged("BillingAmountFrom");
            }
        }

        private decimal? _BillingAmountTo;
        public decimal? BillingAmountTo
        {
            get { return _BillingAmountTo; }
            set
            {
                try
                {
                    _BillingAmountTo = Convert.ToDecimal(value);
                }
                catch
                {
                    _BillingAmountTo = null;
                }
                _SearchBillCommand.RaiseCanExecuteChanged();
                //_SearchCommand.RaiseCanExecuteChanged();
                OnPropertyChanged("BillingAmountTo");
            }
        }
        #endregion

        #region NewBill

        
//        private readonly DelegateCommand<string> _SelectBillingPositionCommand;

        private void fillBillingPositionEdit()
        {
            if (_SelectedBillingPosition != null)
            {
                EditActive = true;
                EditBillingPositionName = _SelectedBillingPosition.name;
                EditBillingPositionPrice = _SelectedBillingPosition.price;
                EditBillingPositionTax = _SelectedBillingPosition.tax;
                EditBillingPositionAmount = _SelectedBillingPosition.amount;
            }
            else
            {

                EditActive = false;
                EditBillingPositionName = null;
                EditBillingPositionPrice = null;
                EditBillingPositionTax = null;
                EditBillingPositionAmount = null;
            }
        }

        private void clearNewBillingPosition()
        {
            NewBillingPositionName = null;
            NewBillingPositionPrice = null;
            NewBillingPositionTax = null;
            NewBillingPositionAmount = null;
        }
        public void clearEdit()
        {
            _originalBill = null;
            Message = null;
            Comment = null;
            BillDate = null;
            DueDate = null;

            var part1 = _proxy.searchCompany("","");
            var part2 = _proxy.searchPerson("", "");
            part1.AddRange(part2);

            BillContactData = part1;
            BillContact = null;
            var bp = new ObservableCollection<XmlExchange.billingPosition>();
            BillingPositionData = new ObservableCollection<XmlExchange.billingPosition>();
            calculateBillingAmount();

            _EditBillCommand.RaiseCanExecuteChanged();
            

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


        private XmlExchange.bill _originalBill;

        private string _Message;
        public string Message
        {
            set
            {
                _Message = value;
                OnPropertyChanged("Message");
                _EditBillCommand.RaiseCanExecuteChanged();
                _PrintCommand.RaiseCanExecuteChanged();
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
                _PrintCommand.RaiseCanExecuteChanged();
            }
            get
            {
                return _Comment;
            }
        }

        private XmlExchange.contact _BillContactNew;
        public XmlExchange.contact BillContactNew
        {
            get
            {
                return _BillContactNew;
            }

            set
            {
                _BillContactNew = value;
                OnPropertyChanged("BillContactNew");
                _EditBillCommand.RaiseCanExecuteChanged();
                _PrintCommand.RaiseCanExecuteChanged();
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
                fillBillingPositionEdit();
                OnPropertyChanged("SelectedBillingPosition");
                _RemoveBillingPositionCommand.RaiseCanExecuteChanged();
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
                    _PrintCommand.RaiseCanExecuteChanged();
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
                    _PrintCommand.RaiseCanExecuteChanged();
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
                _AddBillingPositionCommand.RaiseCanExecuteChanged();
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
                _EditBillingPositionCommand.RaiseCanExecuteChanged();
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
                _AddBillingPositionCommand.RaiseCanExecuteChanged();
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
                _EditBillingPositionCommand.RaiseCanExecuteChanged();
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
                _EditBillingPositionCommand.RaiseCanExecuteChanged();
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
                _AddBillingPositionCommand.RaiseCanExecuteChanged();
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
                _AddBillingPositionCommand.RaiseCanExecuteChanged();
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
                _EditBillingPositionCommand.RaiseCanExecuteChanged();
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
    

        #endregion

        #region NewContact

        #endregion
    }
}
