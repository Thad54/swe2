﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using SWE_UI.ViewModel;

namespace SWE_UI.Content
{
    class ContactReferenceViewModel: BaseViewModel
    {

        public ContactReference control;
        private proxy _proxy  = new SWE_UI.proxy();

        private readonly DelegateCommand<string> _LoadAllCompaniesCommand;
        private readonly DelegateCommand<string> _SearchCompanyRefCommand;



        public ContactReferenceViewModel()
        {

            _LoadAllCompaniesCommand = new DelegateCommand<string>(
                (s) =>
                {
                    bool hold;

                    EmployingCompanyData_Edit = _EmployingCompanyData_Edit;
                    CompanyID_Edit = _CompanyID_Edit;

                    foreach (var item in control.ItemSource)
                    {
                        hold = item.Equals(_CompanyID_Edit);
                    }

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
        public DelegateCommand<string> SearchCompanyRef_Edit
        {
            get { return _SearchCompanyRefCommand; }
        }

        public DelegateCommand<string> LoadAllCompanies_Edit
        {
            get { return _LoadAllCompaniesCommand; }
        }

        private bool _CompanySelectorOpen;
        public bool CompanySelectorOpen
        {
            set
            {
                _CompanySelectorOpen = value;
                OnPropertyChanged("CompanySelectorOpen");
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
                control.SelectedItem = value;
                OnPropertyChanged("CompanyID_Edit");
                OnPropertyChanged("SelectedItem");
            }
        }

        private List<XmlExchange.contact> _EmployingCompanyData_Edit;
        public List<XmlExchange.contact> EmployingCompanyData_Edit
        {
            set
            {
                _EmployingCompanyData_Edit = value;
                
                if (!_EmployingCompanyData_Edit.Contains(null))
                {
                    var empty = new XmlExchange.contact();
                    empty.id = null;
                    _EmployingCompanyData_Edit.Add(empty);
                }
                
                control.ItemSource = _EmployingCompanyData_Edit;
                OnPropertyChanged("EmployingCompanyData_Edit");
            }
            get
            {
                return _EmployingCompanyData_Edit;
            }
        }

        private string _CompanyName_Edit;
        public string CompanyName_Edit
        {
            get { return _CompanyName_Edit; }
            set
            {
                _CompanyName_Edit = value;    
                OnPropertyChanged("CompanyName_Edit");
            }
        }

        private string _Search;
        public string Search
        {
            get { return _Search; }
            set
            {
                _Search = value;
                control.Search = _Search;
                OnPropertyChanged("Search");
            }
        }
    }
}