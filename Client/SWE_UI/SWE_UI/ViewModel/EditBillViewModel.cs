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

    }
}
