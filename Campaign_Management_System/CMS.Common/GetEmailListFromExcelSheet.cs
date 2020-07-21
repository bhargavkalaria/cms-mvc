using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web;

namespace CMS.Common
{
    public class GetEmailListFromExcelSheet
    {
        public List<string> GetEmailsList(HttpPostedFileBase httpPostedFile)
        {
            List<string> emails = new List<string>();
            Stream stream = httpPostedFile.InputStream;

            IExcelDataReader reader = null;


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
                        if (col == 2)
                        {
                            emails.Add(row[col].ToString());
                        }
                        rowcounter++;
                    }
                    dt.Rows.Add(row);
                }

                return emails;
            }
            catch (Exception)
            {
                return null;

            }
        }
    }
}
