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
        private readonly DelegateCommand<string> _EditCommand;
        private readonly DelegateCommand<string> _ClearContactCommand;


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
                    BillSaved = true;


                }, //Execute
                (s) =>
                {
                    if (_originalBill != null || BillSaved == true)
                    {
                        return false;
                    }
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
                    BillSaved = false;
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
                        if (_CompanyID_Edit == null)
                        {
                            contact.companyID = null;
                            contact.company = string.Empty;
                        }
                        else
                        {
                            contact.companyID = _CompanyID_Edit.id;
                            contact.company = _CompanyID_Edit.name;
                        }
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

                    try
                    {
                        result = _proxy.AddContact(contact);
                    }
                    catch (Exception)
                    {
                        return;
                    }
                    if (result.error)
                    {
                        System.Windows.MessageBox.Show(result.text);
                        return;
                    }

                }, //Execute
                (s) =>
                {
                    if (CanEditPerson && CanEditCompany)
                    {
                        return false;
                    }
                    if (CanEditPerson)
                    {
                        if (string.IsNullOrWhiteSpace(FirstName_Edit) || string.IsNullOrWhiteSpace(LastName_Edit))
                        {
                            return false;
                        }
                        if (string.IsNullOrWhiteSpace(Address_Edit) || string.IsNullOrWhiteSpace(BillingAddress_Edit) || string.IsNullOrWhiteSpace(ShippingAddress_Edit))
                        {
                            return false;
                        }
                        if (CreationDate_Edit == null)
                        {
                            return false;
                        }
                        return true;
                    }
                    else
                    {
                        if (string.IsNullOrWhiteSpace(CompanyName_Edit) || string.IsNullOrWhiteSpace(UID_Edit))
                        {
                            return false;
                        }
                        if (string.IsNullOrWhiteSpace(Address_Edit) || string.IsNullOrWhiteSpace(BillingAddress_Edit) || string.IsNullOrWhiteSpace(ShippingAddress_Edit))
                        {
                            return false;
                        }
                        if (CreationDate_Edit == null)
                        {
                            return false;
                        }
                        return true;
                    }

                    return true;

                } //CanExecute
                );

            _ClearContactCommand = new DelegateCommand<string>(
                (s) =>
                {
                    clearNewContact();
                    _EditCommand.RaiseCanExecuteChanged();
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
        public DelegateCommand<string> EditContactClicked
        {
            get { return _EditCommand; }
        }

        public DelegateCommand<string> ClearContactClicked
        {
            get { return _ClearContactCommand; }
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


        private bool BillSaved = false;
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

        public void clearNewContact()
        {
            Address_Edit = null;
            BillingAddress_Edit = null;
            ShippingAddress_Edit = null;
            CreationDate_Edit = DateTime.Now;
            Title_Edit = null;
            FirstName_Edit = null;
            LastName_Edit = null;
            Suffix_Edit = null;
            CompanyName_Edit = null;
            UID_Edit = null;
            CompanyID_Edit = null;
            _EditCommand.RaiseCanExecuteChanged();
        }

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
                OnPropertyChanged("EmployingCompanyData_Edit");
                _EditCommand.RaiseCanExecuteChanged();
            }
            get
            {

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

        #endregion
    }
}
