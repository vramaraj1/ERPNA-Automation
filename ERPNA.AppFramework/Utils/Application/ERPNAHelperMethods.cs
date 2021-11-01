﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading;
using Browser.Core.Framework;
using OpenQA.Selenium;


namespace ERPNA.AppFramework
{
    public class ERPNAHelperMethods
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userType"></param>
        /// <param name="customerID"></param>
        public dynamic Login(IWebDriver Browser)
        {
            
            return null;
        }

        public bool AreTablesTheSame(DataTable tbl1, DataTable tbl2)
        {
            if (tbl1.Rows.Count != tbl2.Rows.Count || tbl1.Columns.Count != tbl2.Columns.Count)
                return false;


            for (int i = 0; i < tbl1.Rows.Count; i++)
            {
                for (int c = 0; c < tbl1.Columns.Count; c++)
                {
                    if (!Equals(tbl1.Rows[i][c], tbl2.Rows[i][c]))
                        return false;
                }
            }
            return true;
        }

       



    }
}
