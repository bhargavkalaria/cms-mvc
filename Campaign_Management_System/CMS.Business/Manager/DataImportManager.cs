using AutoMapper;
using CMS.BE.ViewModels;
using CMS.BL.Interface;
using CMS.Data.Database;
using CMS.DL.Interface;
using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web;

namespace CMS.BL.Manager
{
    public class DataImportManager : IDataImportManager
    {
        private ICustomerRepository _icustomerRepository;
        public DataImportManager(ICustomerRepository customerRepository)
        {
            _icustomerRepository = customerRepository;
        }

        public List<CustomerViewModel> GetAllCustomers()
        {
            List<CustomerViewModel> customerViewModel = new List<CustomerViewModel>();
            var customers = _icustomerRepository.GetAllCustomers();
            foreach (var user in customers)
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Customer, CustomerViewModel>();
                });

                IMapper mapper = config.CreateMapper();
                var source = new Customer();
                source = user;
                var dest = mapper.Map<Customer, CustomerViewModel>(source);
                customerViewModel.Add(dest);
            }
            return customerViewModel;
        }

        public string ReadAndSaveExcel(HttpPostedFileBase httpPostedFile)
        {
            Stream stream = httpPostedFile.InputStream;

            IExcelDataReader reader = null;

            CustomerViewModel customerViewModel = new CustomerViewModel();

            if (httpPostedFile.FileName.EndsWith(".xls"))
            {
                reader = ExcelReaderFactory.CreateBinaryReader(stream);
            }
            else
            {
                reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
            }

            int fieldcount = reader.FieldCount;
            int rowcount = reader.RowCount;
            DataTable dt = new DataTable();
            DataRow row;
            DataTable dt_ = new DataTable();
            List<Customer> cst = new List<Customer>();
            try
            {
                dt_ = reader.AsDataSet().Tables[0];
                for (int i = 0; i < dt_.Columns.Count; i++)
                {
                    dt.Columns.Add(dt_.Rows[0][i].ToString());
                }
                int rowcounter = 0;
                for (int row_ = 1; row_ < dt_.Rows.Count; row_++)
                {
                    row = dt.NewRow();

                    for (int col = 0; col < dt_.Columns.Count; col++)
                    {
                        row[col] = dt_.Rows[row_][col].ToString();

                        if (col == 0)
                        {
                            if (!string.IsNullOrEmpty(row[col].ToString()))
                            {
                                if (IsNumber(row[col].ToString()))
                                {
                                    customerViewModel.CustomerName = row[col].ToString();
                                }
                                else
                                {
                                    return "Name can't be number at line " + (row_ + 1);
                                }
                            }
                            else
                            {
                                return "Please fill customer name at line " + (row_ + 1);
                            }
                        }
                        if (col == 1)
                        {
                            if (!string.IsNullOrEmpty(row[col].ToString()))
                            {
                                if (IsValidEmailAddress(row[col].ToString()))
                                {
                                    customerViewModel.Email = row[col].ToString();
                                }
                                else
                                {
                                    return "Please enter valid email at line " + (row_ + 1);
                                }
                            }
                            else
                            {
                                return "Please fill email at line " + (row_ + 1);
                            }
                        }
                        if (col == 2)
                        {
                            if (!string.IsNullOrEmpty(row[col].ToString()))
                            {
                                if (IsNumber(row[col].ToString()))
                                {
                                    customerViewModel.City = row[col].ToString();
                                }
                                else
                                {
                                    return "City can't be number at line " + (row_ + 1);
                                }
                            }
                            else
                            {
                                return "Please fill city at line " + (row_ + 1);
                            }
                        }
                        if (col == 3)
                        {
                            if (!string.IsNullOrEmpty(row[col].ToString()))
                            {
                                string Mobilepattern = @"^(\d{10})$";
                                Regex rgx = new Regex(Mobilepattern);
                                if (rgx.IsMatch(row[col].ToString()))
                                {
                                    customerViewModel.Mobile = row[col].ToString();
                                }
                                else
                                {
                                    return "Please fill valid number at line " + (row_ + 1);
                                }
                            }
                            else
                            {
                                return "Please fill mobile number at line " + (row_ + 1);
                            }
                        }
                        if (col == 4)
                        {
                            if (!string.IsNullOrEmpty(row[col].ToString()))
                            {
                                string AgePattern = @"^0*(?:[1-9][0-9]?|100)$";
                                Regex rgx = new Regex(AgePattern);
                                if (rgx.IsMatch(row[col].ToString()))
                                {
                                    customerViewModel.Age = Convert.ToInt32(row[col]);
                                }
                                else
                                {
                                    return "Please fill valid age at line " + (row_ + 1);
                                }

                            }
                            else
                            {
                                return "Please fill age at line " + (row_ + 1);
                            }
                        }
                        if (col == 5)
                        {
                            if (!string.IsNullOrEmpty(row[col].ToString()))
                            {
                                if (IsNumber(row[col].ToString()))
                                {
                                    customerViewModel.State = row[col].ToString();
                                }
                                else
                                {
                                    return "State can't be number at line " + (row_ + 1);
                                }
                            }
                            else
                            {
                                return "Please fill state at line " + (row_ + 1);
                            }
                        }
                        if (col == 6)
                        {
                            if (!string.IsNullOrEmpty(row[col].ToString()))
                            {
                                if (IsNumber(row[col].ToString()))
                                {
                                    customerViewModel.Country = row[col].ToString();
                                }
                                else
                                {
                                    return "Country can't be number at line " + (row_ + 1);
                                }
                            }
                            else
                            {
                                return "Please fill country at line " + (row_ + 1);
                            }
                        }
                        if (col == 7)
                        {
                            if (!string.IsNullOrEmpty(row[col].ToString()))
                            {
                                if (IsNumber(row[col].ToString()))
                                {
                                    customerViewModel.Address = row[col].ToString();
                                }
                                else
                                {
                                    return "Address can't be number at line " + (row_ + 1);
                                }
                            }
                            else
                            {
                                return "Please fill address at line " + (row_ + 1);
                            }
                        }

                        rowcounter++;
                    }
                    dt.Rows.Add(row);
                    cst.Add(new Customer
                    {
                        CustomerID = customerViewModel.CustomerID,
                        CustomerName = customerViewModel.CustomerName,
                        Email = customerViewModel.Email,
                        City = customerViewModel.City,
                        Mobile = customerViewModel.Mobile,
                        Age = customerViewModel.Age,
                        State = customerViewModel.State,
                        Country = customerViewModel.Country,
                        Address = customerViewModel.Address
                    });
                }
                for (var i = 0; i < cst.Count; i++)
                {
                    _icustomerRepository.ReadAndSaveExcel(cst[i]);
                }
                return "success";
            }
            catch (Exception)
            {
                return "Something went wrong";
                // ModelState.AddModelError("File", "Unable to Upload file!");
                // return View();
            }
        }

        public bool IsValidEmailAddress(string emailaddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailaddress);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        public static bool IsNumber(String s)
        {
            if (Regex.IsMatch(s, @"^[a-zA-Z]+(?:[\s-][a-zA-Z]+)*$"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
