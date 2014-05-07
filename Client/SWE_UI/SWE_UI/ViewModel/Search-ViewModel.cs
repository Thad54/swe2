using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWE_UI.ViewModel
{
    class Search_ViewModel : BaseViewModel
    {
        private ViewModel _mainViewModel;
        public void setMainViewModel(ViewModel view)
        {
            _mainViewModel = view;
        }

        private SearchResults _window;
        public void setWindow(SearchResults window)
        {
            _window = window;
        }

        

        private List<XmlExchange.contact> _contacts;
        public List<XmlExchange.contact> contacts
        {
            set{
                _contacts = value;
                OnPropertyChanged("contacts");
            }
            get
            {
                return _contacts;
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

        private readonly DelegateCommand<string> _SelectionCommand;
        public DelegateCommand<string> ContactSelected
        {
            get { return _SelectionCommand; }
        }

        public Search_ViewModel()
        {
            _SelectionCommand = new DelegateCommand<string>(
                (s) =>
                {
                    _mainViewModel.clearEdit();
                    _mainViewModel.fillEdit(SelectedContact);
                    _window.Close();
                }, //Execute
                (s) =>
                {
                    return true;
                } //CanExecute
                );
        }
    }
}
