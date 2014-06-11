using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using PdfSharp.Pdf;

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

        public EditBillViewModel()
        {

            _EditBillCommand = new DelegateCommand<string>(
                (s) =>
                {
                    var bill = new XmlExchange.bill();

                    bill.BillingDate = _BillDate;
                    bill.billingPositions = _BillingPositionData;
                    bill.comment = _Comment;
                    bill.contact = _BillContact;
                    if (_BillContact != null)
                    {
                        bill.contactId = _BillContact.id;
                    }
                    else
                    {
                        bill.contactId = null;
                    }
                    bill.DueByDate = _DueDate;
                    bill.ID = _originalBill.ID;
                    bill.message = _Message;

                    var message = _proxy.EditBill(bill);

                    if(message.error){
                        MessageBox.Show(message.text);
                    }

                    _submit = true;
                    _window.Close();
                   /* EmployingCompanyData_Edit = _proxy.searchCompany("", "");
                    clearEdit();*/

                }, //Execute
                (s) =>
                {
                    if (_BillingPositionData.Count == 0)
                    {
                        return false;
                    }
                    if (_BillDate == null || _DueDate == null)
                    {
                        return false;
                    }
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
                        _PrintCommand.RaiseCanExecuteChanged();
                    }
                    _EditBillCommand.RaiseCanExecuteChanged();
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
                    _BillingPositionEdited = true;
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
                    return !ContactAltered();

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
  /*      public DelegateCommand<string> SelectBillingPositionClicked
        {
            get { return _SelectBillingPositionCommand; }
        }*/
        public void clearEdit()
        {
        }

        private bool _BillingPositionEdited = false;

        private bool BillingPositionsChanged()
        {
            int match = 0;

            if (_BillingPositionEdited)
            {
                return true;
            }
            if(_originalBill.billingPositions.Count != BillingPositionData.Count){
                return true;
            }

            foreach (var bp in _originalBill.billingPositions)
            {
                if (BillingPositionData.Contains(bp))
                {
                    match++;
                }
            }
            if (match == _originalBill.billingPositions.Count)
            {
                return false;
            }
            else
            {
                return true;
            }

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

           // BillingPositionData = bill.billingPositions;
            var bp = new ObservableCollection<XmlExchange.billingPosition>();

            foreach (var elem in bill.billingPositions)
            {
                bp.Add(elem);
            }

            BillingPositionData = bp;
            calculateBillingAmount();
            _BillingPositionEdited = false;

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
                return false;
            }
            if (_originalBill.DueByDate != _DueDate)
            {
                return true;
            }
            if (_originalBill.message != _Message)
            {
                return true;
            }
            if (BillingPositionsChanged())
            {
                return true;
            }
            return false;
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

/*        private bool _CanAddBillingPosition;
        public bool CanAddBillingPosition
        {
            set
            {
                _CanAddBillingPosition = value;
                OnPropertyChanged("CanAddBillingPosition");
            }
            get
            {
                return _EditActive;
            }
        }
*/
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
