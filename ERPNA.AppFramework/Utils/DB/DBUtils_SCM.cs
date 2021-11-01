using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Browser.Core.Framework;

namespace ERPNA.AppFramework
{
    
        /// <summary>
        /// Queries/scripts for Mainpro
        /// </summary>
        public static class DBUtils_SCM
        {
            #region properties

            private static DataAccess _WebAppDbAccess = new DataAccess(new SqlServerDataAccessProvider(AppSettings.Config["SQLConnectionString"]));

            //private static DataAccess _WebAppDbAccess = null;

            private static string p_SQLconnString = string.Empty;
            public static string SQLconnString
            {
                get
                {
                    return p_SQLconnString;
                }
                set
                {
                    p_SQLconnString = value;
                    _WebAppDbAccess = new DataAccess(new SqlServerDataAccessProvider(value));

                }
            }

            #endregion properties

            #region methods

            /// <summary>
            /// Sets the CFPCMainPro+ GracePeriodEndDate to yesterday's date, so that if the user complete the current cycle, 
            /// it will be automatically rollover to next applicable cycle  
            /// NOTE: We can NOT not use this method for UAT /Production Environemnts. And everytime we use this query, 
            /// afterward we have to set the date back to the default date. See the method titled 
            /// SetGracePeriodEndDateToDefaultDate to set it back. That method will be called at the test class level
            /// after every test that uses this method
            /// </summary>       
            public static decimal AWB_AllSpendAnalysis_ServiceSum()
            {
                

            string query = string.Format(@"SELECT SUM(TotalAmt)
FROM dsSpendDetailMthAll udlTotSpendDetailAct (nolock)
INNER JOIN dsOrgCatalog (nolock)
ON dsOrgCatalog.Catalog = udlTotSpendDetailAct.Catalog
AND dsOrgCatalog.Org = udlTotSpendDetailAct.Org
INNER JOIN udlSpendItem (nolock)
ON udlTotSpendDetailAct.SpendItemID = udlSpendItem.ID
LEFT JOIN udlVendorLoc
ON udlVendorLoc.VendorLoc = udlTotSpendDetailAct.VendorLoc
AND udlVendorLoc.VendorSet = udlTotSpendDetailAct.VendorSet
AND udlVendorLoc.DeletedYN = 0
WHERE ISNULL(udlVendorLoc.ServicesOnlyYN,0) = 1
AND DATEDIFF( MONTH, udlTotSpendDetailAct.TranBeginDate, GETDATE()) BETWEEN 1 AND 12");

           decimal serviceValue = _WebAppDbAccess.GetDataValue<decimal>(query, 90);

            return serviceValue;
          
            }

            public static decimal AWB_AllSpendAnalysis_RegisteredSum()
            {


            string query = string.Format(@"SELECT SUM(TotalAmt)
FROM dsSpendDetailMthAll udlTotSpendDetailAct (nolock)
INNER JOIN dsOrgCatalog (nolock)
ON dsOrgCatalog.Catalog = udlTotSpendDetailAct.Catalog
AND dsOrgCatalog.Org = udlTotSpendDetailAct.Org
INNER JOIN udlSpendItem (nolock)
ON udlTotSpendDetailAct.SpendItemID = udlSpendItem.ID
LEFT JOIN udlVendorLoc
ON udlVendorLoc.VendorLoc = udlTotSpendDetailAct.VendorLoc
AND udlVendorLoc.VendorSet = udlTotSpendDetailAct.VendorSet
AND udlVendorLoc.DeletedYN = 0
WHERE udlSpendItem.VendorEntityGuid <> ''
AND ISNULL(udlVendorLoc.ServicesOnlyYN,0) = 0
AND DATEDIFF( MONTH, udlTotSpendDetailAct.TranBeginDate, GETDATE()) BETWEEN 1 AND 12");

            decimal serviceValue = _WebAppDbAccess.GetDataValue<decimal>(query, 90);

            return serviceValue;

        }

        public static decimal AWB_AllSpendAnalysis_UnRegisteredSum()
        {


            string query = string.Format(@"SELECT SUM(TotalAmt)
FROM dsSpendDetailMthAll udlTotSpendDetailAct (nolock)
INNER JOIN dsOrgCatalog (nolock)
ON dsOrgCatalog.Catalog = udlTotSpendDetailAct.Catalog
AND dsOrgCatalog.Org = udlTotSpendDetailAct.Org
INNER JOIN udlSpendItem (nolock)
ON udlTotSpendDetailAct.SpendItemID = udlSpendItem.ID
LEFT JOIN udlVendorLoc
ON udlVendorLoc.VendorLoc = udlTotSpendDetailAct.VendorLoc
AND udlVendorLoc.VendorSet = udlTotSpendDetailAct.VendorSet
AND udlVendorLoc.DeletedYN = 0
WHERE udlSpendItem.VendorEntityGuid = ''
AND ISNULL(udlVendorLoc.ServicesOnlyYN,0) = 0
AND DATEDIFF( MONTH, udlTotSpendDetailAct.TranBeginDate, GETDATE()) BETWEEN 1 AND 12");

            decimal serviceValue = _WebAppDbAccess.GetDataValue<decimal>(query, 90);

            return serviceValue;

        }

        public static decimal AWB_AllSpendAnalysis_KeywordSpendSum()
        {


            string query = string.Format(@"SELECT
sum(CASE WHEN ISNULL(udlVendorLoc.ServicesOnlyYN,0) = 0 And udlSpendItem.VendorEntityGUID <> '' THEN TotalAmt ELSE 0 END) as TotalSpend
FROM dsSpendDetailMthAll udlTotSpendDetailAct (nolock)
INNER JOIN dsOrgCatalog (nolock)
ON dsOrgCatalog.Catalog = udlTotSpendDetailAct.Catalog
AND dsOrgCatalog.Org = udlTotSpendDetailAct.Org
INNER JOIN udlSpendItem (nolock)
ON udlTotSpendDetailAct.SpendItemID = udlSpendItem.ID
Inner Join (Select DISTINCT DPC.TableID, DPC.RowID From DPC with(nolock) WHERE (DPC.TableID = 'USI' ) AND (DPC.Level = '1' And DPC.TableID = 'USI' AND DPC.Source = '1' AND DPC.Phrase = 'Stent') ) as KEY_TBL on Key_Tbl.RowID = udlSpendItem.ID
LEFT JOIN udlVendorLoc
ON udlVendorLoc.VendorLoc = udlTotSpendDetailAct.VendorLoc
AND udlVendorLoc.VendorSet = udlTotSpendDetailAct.VendorSet
AND udlVendorLoc.DeletedYN = 0
WHERE udlSpendItem.VendorEntityGuid <> ''
AND ISNULL(udlVendorLoc.ServicesOnlyYN,0) = 0
AND DATEDIFF( MONTH, udlTotSpendDetailAct.TranBeginDate, GETDATE()) BETWEEN 1 AND 12");

            decimal serviceValue = _WebAppDbAccess.GetDataValue<decimal>(query, 90);

            return serviceValue;

        }

        public static Int32 AWB_AllSpendAnalysis_ItemKeywordMfrMINCount()
        {


            string query = string.Format(@"SELECT COUNT(*) FROM (
SELECT PIGEntityName, PIGItemNo, Count = COUNT(*)
FROM dsSpendDetailMthAll udlTotSpendDetailAct (nolock)
INNER JOIN dsOrgCatalog (nolock)
ON dsOrgCatalog.Catalog = udlTotSpendDetailAct.Catalog
AND dsOrgCatalog.Org = udlTotSpendDetailAct.Org
INNER JOIN udlSpendItem (nolock)
ON udlTotSpendDetailAct.SpendItemID = udlSpendItem.ID
Inner Join (Select DISTINCT DPC.TableID, DPC.RowID From DPC with(nolock) WHERE DPC.Level = '1' And DPC.TableID = 'USI' AND DPC.Source = '1' AND DPC.Phrase = 'Stent') as KEY_TBL on Key_Tbl.RowID = udlSpendItem.ID
LEFT JOIN udlVendorLoc
ON udlVendorLoc.VendorLoc = udlTotSpendDetailAct.VendorLoc
AND udlVendorLoc.VendorSet = udlTotSpendDetailAct.VendorSet
AND udlVendorLoc.DeletedYN = 0
WHERE udlSpendItem.VendorEntityGuid <> ''
AND ISNULL(udlVendorLoc.ServicesOnlyYN,0) = 0
AND DATEDIFF( MONTH, udlTotSpendDetailAct.TranBeginDate, GETDATE()) BETWEEN 1 AND 12
AND udlSpendItem.PIGItemNo <> ''
--And udlSpendItem.ItemNo <> ''
GROUP BY PIGEntityName, PIGItemNo
) d");

            Int32 serviceValue = _WebAppDbAccess.GetDataValue<Int32>(query, 90);

            return serviceValue;

        }

        public static decimal AWB_AllSpendAnalysis_ItemUNSPSCCodeSpendSum()
        {


            string query = string.Format(@"SELECT SUM(TotalAmt)
FROM dsSpendDetailMthAll udlTotSpendDetailAct (nolock)
INNER JOIN dsOrgCatalog (nolock)
ON dsOrgCatalog.Catalog = udlTotSpendDetailAct.Catalog
AND dsOrgCatalog.Org = udlTotSpendDetailAct.Org
INNER JOIN udlSpendItem (nolock)
ON udlTotSpendDetailAct.SpendItemID = udlSpendItem.ID
LEFT JOIN udlVendorLoc
ON udlVendorLoc.VendorLoc = udlTotSpendDetailAct.VendorLoc
AND udlVendorLoc.VendorSet = udlTotSpendDetailAct.VendorSet
AND udlVendorLoc.DeletedYN = 0
WHERE udlSpendItem.VendorEntityGuid <> ''
AND ISNULL(udlVendorLoc.ServicesOnlyYN,0) = 0
AND DATEDIFF( MONTH, udlTotSpendDetailAct.TranBeginDate, GETDATE()) BETWEEN 1 AND 12
AND udlSpendItem.ID IN ( SELECT RowID FROM DPC WHERE TableID = 'USI' AND Level = 1 AND Source = 2 AND Phrase = '42321605' )");

            decimal serviceValue = _WebAppDbAccess.GetDataValue<decimal>(query, 90);

            return serviceValue;

        }

        public static Int32 AWB_AllSpendAnalysis_ItemUNSPSCCodeMfrMINCount()
        {


            string query = string.Format(@"SELECT COUNT(*) FROM (
SELECT PIGEntityName, PIGItemNo, Count = COUNT(*)
FROM dsSpendDetailMthAll udlTotSpendDetailAct (nolock)
INNER JOIN dsOrgCatalog (nolock)
ON dsOrgCatalog.Catalog = udlTotSpendDetailAct.Catalog
AND dsOrgCatalog.Org = udlTotSpendDetailAct.Org
INNER JOIN udlSpendItem (nolock)
ON udlTotSpendDetailAct.SpendItemID = udlSpendItem.ID
LEFT JOIN udlVendorLoc
ON udlVendorLoc.VendorLoc = udlTotSpendDetailAct.VendorLoc
AND udlVendorLoc.VendorSet = udlTotSpendDetailAct.VendorSet
AND udlVendorLoc.DeletedYN = 0
WHERE udlSpendItem.VendorEntityGuid <> ''
AND ISNULL(udlVendorLoc.ServicesOnlyYN,0) = 0
AND DATEDIFF( MONTH, udlTotSpendDetailAct.TranBeginDate, GETDATE()) BETWEEN 1 AND 12
AND udlSpendItem.ID IN ( SELECT RowID FROM DPC WHERE TableID = 'USI' AND Level = 1 AND Source = 2 AND Phrase = '42321605' )
AND udlSpendItem.PIGItemNo <> ''
GROUP BY PIGEntityName, PIGItemNo
) d");

            Int32 serviceValue = _WebAppDbAccess.GetDataValue<Int32>(query, 90);

            return serviceValue;

        }

        public static decimal AWB_AllSpendAnalysis_ItemManufacturerChildSpendSum()
        {


            string query = string.Format(@"SELECT SUM(TotalAmt)
FROM dsSpendDetailMthAll udlTotSpendDetailAct (nolock)
INNER JOIN dsOrgCatalog (nolock)
ON dsOrgCatalog.Catalog = udlTotSpendDetailAct.Catalog
AND dsOrgCatalog.Org = udlTotSpendDetailAct.Org
INNER JOIN udlSpendItem (nolock)
ON udlTotSpendDetailAct.SpendItemID = udlSpendItem.ID
LEFT JOIN udlVendorLoc
ON udlVendorLoc.VendorLoc = udlTotSpendDetailAct.VendorLoc
AND udlVendorLoc.VendorSet = udlTotSpendDetailAct.VendorSet
AND udlVendorLoc.DeletedYN = 0
WHERE udlSpendItem.VendorEntityGuid <> ''
AND ISNULL(udlVendorLoc.ServicesOnlyYN,0) = 0
AND DATEDIFF( MONTH, udlTotSpendDetailAct.TranBeginDate, GETDATE()) BETWEEN 1 AND 12
AND udlSpendItem.PIGEntityName = 'United States Surgical Corporation'");

            decimal serviceValue = _WebAppDbAccess.GetDataValue<decimal>(query, 90);

            return serviceValue;

        }

        public static Int32 AWB_AllSpendAnalysis_ItemManufacturerChildSpendCount()
        {


            string query = string.Format(@"SELECT COUNT(*) FROM (
SELECT PIGEntityName, PIGItemNo, Count = COUNT(*)
FROM dsSpendDetailMthAll udlTotSpendDetailAct (nolock)
INNER JOIN dsOrgCatalog (nolock)
ON dsOrgCatalog.Catalog = udlTotSpendDetailAct.Catalog
AND dsOrgCatalog.Org = udlTotSpendDetailAct.Org
INNER JOIN udlSpendItem (nolock)
ON udlTotSpendDetailAct.SpendItemID = udlSpendItem.ID
LEFT JOIN udlVendorLoc
ON udlVendorLoc.VendorLoc = udlTotSpendDetailAct.VendorLoc
AND udlVendorLoc.VendorSet = udlTotSpendDetailAct.VendorSet
AND udlVendorLoc.DeletedYN = 0
WHERE udlSpendItem.VendorEntityGuid <> ''
AND ISNULL(udlVendorLoc.ServicesOnlyYN,0) = 0
AND DATEDIFF( MONTH, udlTotSpendDetailAct.TranBeginDate, GETDATE()) BETWEEN 1 AND 12
AND udlSpendItem.PIGEntityName = 'United States Surgical Corporation'
AND udlSpendItem.PIGItemNo <> ''
GROUP BY PIGEntityName, PIGItemNo
) d");

            Int32 serviceValue = _WebAppDbAccess.GetDataValue<Int32>(query, 90);

            return serviceValue;

        }


        public static decimal AWB_AllSpendAnalysis_ItemManufacturerParentSpendSum()
        {


            string query = string.Format(@"SELECT SUM(TotalAmt)
FROM dsSpendDetailMthAll udlTotSpendDetailAct (nolock)
INNER JOIN dsOrgCatalog (nolock)
ON dsOrgCatalog.Catalog = udlTotSpendDetailAct.Catalog
AND dsOrgCatalog.Org = udlTotSpendDetailAct.Org
INNER JOIN udlSpendItem (nolock)
ON udlTotSpendDetailAct.SpendItemID = udlSpendItem.ID
inner join (select Distinct RowID, RefRowID from vDPCRollup WHERE TableID = 'USI' AND Level = '2' AND Source = 23 AND Phrase = 'Covidien LTD') DPC on
DPC.RowID = udlSpendItem.ID
LEFT JOIN udlVendorLoc
ON udlVendorLoc.VendorLoc = udlTotSpendDetailAct.VendorLoc
AND udlVendorLoc.VendorSet = udlTotSpendDetailAct.VendorSet
AND udlVendorLoc.DeletedYN = 0
WHERE udlSpendItem.VendorEntityGuid <> ''
AND ISNULL(udlVendorLoc.ServicesOnlyYN,0) = 0
AND DATEDIFF( MONTH, udlTotSpendDetailAct.TranBeginDate, GETDATE()) BETWEEN 1 AND 12");

            decimal serviceValue = _WebAppDbAccess.GetDataValue<decimal>(query, 90);

            return serviceValue;

        }

        public static Int32 AWB_AllSpendAnalysis_ItemManufacturerParentSpendCount()
        {


            string query = string.Format(@"SELECT COUNT(*) FROM (
SELECT PIGEntityName, PIGItemNo, Count = COUNT(*)
FROM dsSpendDetailMthAll udlTotSpendDetailAct (nolock)
INNER JOIN dsOrgCatalog (nolock)
ON dsOrgCatalog.Catalog = udlTotSpendDetailAct.Catalog
AND dsOrgCatalog.Org = udlTotSpendDetailAct.Org
INNER JOIN udlSpendItem (nolock)
ON udlTotSpendDetailAct.SpendItemID = udlSpendItem.ID
inner join (select Distinct RowID, RefRowID from vDPCRollup WHERE TableID = 'USI' AND Level = '2' AND Source = 23 AND Phrase = 'Covidien LTD') DPC on
DPC.RowID = udlSpendItem.ID
LEFT JOIN udlVendorLoc
ON udlVendorLoc.VendorLoc = udlTotSpendDetailAct.VendorLoc
AND udlVendorLoc.VendorSet = udlTotSpendDetailAct.VendorSet
AND udlVendorLoc.DeletedYN = 0
WHERE udlSpendItem.VendorEntityGuid <> ''
AND ISNULL(udlVendorLoc.ServicesOnlyYN,0) = 0
AND DATEDIFF( MONTH, udlTotSpendDetailAct.TranBeginDate, GETDATE()) BETWEEN 1 AND 12
AND udlSpendItem.PIGItemNo <> ''
GROUP BY PIGEntityName, PIGItemNo
) d");

            Int32 serviceValue = _WebAppDbAccess.GetDataValue<Int32>(query, 90);

            return serviceValue;

        }

        public static decimal AWB_AllSpendAnalysis_VendorTotalChildSpendSum()
        {


            string query = string.Format(@"SELECT SUM(TotalAmt)
FROM dsSpendDetailMthAll udlTotSpendDetailAct (nolock)
INNER JOIN dsOrgCatalog (nolock)
ON dsOrgCatalog.Catalog = udlTotSpendDetailAct.Catalog
AND dsOrgCatalog.Org = udlTotSpendDetailAct.Org
INNER JOIN udlSpendItem (nolock)
ON udlTotSpendDetailAct.SpendItemID = udlSpendItem.ID
LEFT JOIN udlVendorLoc
ON udlVendorLoc.VendorLoc = udlTotSpendDetailAct.VendorLoc
AND udlVendorLoc.VendorSet = udlTotSpendDetailAct.VendorSet
AND udlVendorLoc.DeletedYN = 0
WHERE udlSpendItem.VendorEntityGuid <> ''
AND ISNULL(udlVendorLoc.ServicesOnlyYN,0) = 0
AND DATEDIFF( MONTH, udlTotSpendDetailAct.TranBeginDate, GETDATE()) BETWEEN 1 AND 12
AND udlSpendItem.VendorEntityName = 'Depuy Spine Inc'");

            decimal serviceValue = _WebAppDbAccess.GetDataValue<decimal>(query, 90);

            return serviceValue;

        }

        public static Int32 AWB_AllSpendAnalysis_VendorTotalChildSpendCount()
        {


            string query = string.Format(@"SELECT COUNT(*) FROM (
SELECT PIGEntityName, PIGItemNo, Count = COUNT(*)
FROM dsSpendDetailMthAll udlTotSpendDetailAct (nolock)
INNER JOIN dsOrgCatalog (nolock)
ON dsOrgCatalog.Catalog = udlTotSpendDetailAct.Catalog
AND dsOrgCatalog.Org = udlTotSpendDetailAct.Org
INNER JOIN udlSpendItem (nolock)
ON udlTotSpendDetailAct.SpendItemID = udlSpendItem.ID
LEFT JOIN udlVendorLoc
ON udlVendorLoc.VendorLoc = udlTotSpendDetailAct.VendorLoc
AND udlVendorLoc.VendorSet = udlTotSpendDetailAct.VendorSet
AND udlVendorLoc.DeletedYN = 0
WHERE udlSpendItem.VendorEntityGuid <> ''
AND ISNULL(udlVendorLoc.ServicesOnlyYN,0) = 0
AND DATEDIFF( MONTH, udlTotSpendDetailAct.TranBeginDate, GETDATE()) BETWEEN 1 AND 12
AND udlSpendItem.VendorEntityName = 'Depuy Spine Inc'
AND udlSpendItem.PIGItemNo <> ''
GROUP BY PIGEntityName, PIGItemNo
) d");

            Int32 serviceValue = _WebAppDbAccess.GetDataValue<Int32>(query, 90);

            return serviceValue;

        }


        public static decimal AWB_AllSpendAnalysis_VendorTotalParentSpendSum()
        {


            string query = string.Format(@"SELECT SUM(TotalAmt)
FROM dsSpendDetailMthAll udlTotSpendDetailAct (nolock)
INNER JOIN dsOrgCatalog (nolock)
ON dsOrgCatalog.Catalog = udlTotSpendDetailAct.Catalog
AND dsOrgCatalog.Org = udlTotSpendDetailAct.Org
INNER JOIN udlSpendItem (nolock)
ON udlTotSpendDetailAct.SpendItemID = udlSpendItem.ID
LEFT JOIN udlVendorLoc
ON udlVendorLoc.VendorLoc = udlTotSpendDetailAct.VendorLoc
AND udlVendorLoc.VendorSet = udlTotSpendDetailAct.VendorSet
AND udlVendorLoc.DeletedYN = 0
WHERE udlSpendItem.VendorEntityGuid <> ''
AND ISNULL(udlVendorLoc.ServicesOnlyYN,0) = 0
AND DATEDIFF( MONTH, udlTotSpendDetailAct.TranBeginDate, GETDATE()) BETWEEN 1 AND 12
AND udlSpendItem.VendorMatchName = 'Johnson & Johnson'");

            decimal serviceValue = _WebAppDbAccess.GetDataValue<decimal>(query, 90);

            return serviceValue;

        }

        public static Int32 AWB_AllSpendAnalysis_VendorTotalParentSpendCount()
        {


            string query = string.Format(@"SELECT COUNT(*) FROM (
SELECT PIGEntityName, PIGItemNo, Count = COUNT(*)
FROM dsSpendDetailMthAll udlTotSpendDetailAct (nolock)
INNER JOIN dsOrgCatalog (nolock)
ON dsOrgCatalog.Catalog = udlTotSpendDetailAct.Catalog
AND dsOrgCatalog.Org = udlTotSpendDetailAct.Org
INNER JOIN udlSpendItem (nolock)
ON udlTotSpendDetailAct.SpendItemID = udlSpendItem.ID
LEFT JOIN udlVendorLoc
ON udlVendorLoc.VendorLoc = udlTotSpendDetailAct.VendorLoc
AND udlVendorLoc.VendorSet = udlTotSpendDetailAct.VendorSet
AND udlVendorLoc.DeletedYN = 0
WHERE udlSpendItem.VendorEntityGuid <> ''
AND ISNULL(udlVendorLoc.ServicesOnlyYN,0) = 0
AND DATEDIFF( MONTH, udlTotSpendDetailAct.TranBeginDate, GETDATE()) BETWEEN 1 AND 12
AND udlSpendItem.VendorMatchName = 'Johnson & Johnson'
AND udlSpendItem.PIGItemNo <> ''
GROUP BY PIGEntityName, PIGItemNo
) d");

            Int32 serviceValue = _WebAppDbAccess.GetDataValue<Int32>(query, 90);

            return serviceValue;

        }

        public static decimal AWB_AllSpendAnalysis_ItemContractAllSpendSum()
        {


            string query = string.Format(@"SELECT SUM(TotalAmt)
FROM dsSpendDetailMthAll udlTotSpendDetailAct (nolock)
INNER JOIN dsOrgCatalog (nolock)
ON dsOrgCatalog.Catalog = udlTotSpendDetailAct.Catalog
AND dsOrgCatalog.Org = udlTotSpendDetailAct.Org
INNER JOIN udlSpendItem (nolock)
ON udlTotSpendDetailAct.SpendItemID = udlSpendItem.ID
LEFT JOIN udlVendorLoc
ON udlVendorLoc.VendorLoc = udlTotSpendDetailAct.VendorLoc
AND udlVendorLoc.VendorSet = udlTotSpendDetailAct.VendorSet
AND udlVendorLoc.DeletedYN = 0
WHERE udlSpendItem.VendorEntityGuid <> ''
AND ISNULL(udlVendorLoc.ServicesOnlyYN,0) = 0
AND DATEDIFF( MONTH, udlTotSpendDetailAct.TranBeginDate, GETDATE()) BETWEEN 1 AND 12
AND udlSpendItem.ID IN ( SELECT RowID FROM DPC WHERE TableID = 'USI' AND Level = 1 AND Source = 26 AND Phrase = 'LN-OR-000455')");

            decimal serviceValue = _WebAppDbAccess.GetDataValue<decimal>(query, 90);

            return serviceValue;

        }

        public static Int32 AWB_AllSpendAnalysis_ItemContractAllSpendCount()
        {


            string query = string.Format(@"SELECT COUNT(*) FROM (
SELECT PIGEntityName, PIGItemNo, Count = COUNT(*)
FROM dsSpendDetailMthAll udlTotSpendDetailAct (nolock)
INNER JOIN dsOrgCatalog (nolock)
ON dsOrgCatalog.Catalog = udlTotSpendDetailAct.Catalog
AND dsOrgCatalog.Org = udlTotSpendDetailAct.Org
INNER JOIN udlSpendItem (nolock)
ON udlTotSpendDetailAct.SpendItemID = udlSpendItem.ID
LEFT JOIN udlVendorLoc
ON udlVendorLoc.VendorLoc = udlTotSpendDetailAct.VendorLoc
AND udlVendorLoc.VendorSet = udlTotSpendDetailAct.VendorSet
AND udlVendorLoc.DeletedYN = 0
WHERE udlSpendItem.VendorEntityGuid <> ''
AND ISNULL(udlVendorLoc.ServicesOnlyYN,0) = 0
AND DATEDIFF( MONTH, udlTotSpendDetailAct.TranBeginDate, GETDATE()) BETWEEN 1 AND 12
AND udlSpendItem.ID IN ( SELECT RowID FROM DPC WHERE TableID = 'USI' AND Level = 1 AND Source = 26 AND Phrase = 'LN-OR-000455' )
AND udlSpendItem.PIGItemNo <> ''
GROUP BY PIGEntityName, PIGItemNo
) d");

            Int32 serviceValue = _WebAppDbAccess.GetDataValue<Int32>(query, 90);

            return serviceValue;

        }

        public static DataTable AWB_AllSpendAnalysis_ItemDepartmentAllSpendSum()
        {        

         
            string query = string.Format(@"SELECT Org,Dept,SUM(TotalAmt) as DeptSpend
FROM udlDeptIssueSpend(nolock)
INNER JOIN udlSpendItem(nolock) ON
udlSpendItem.ID = udlDeptIssueSpend.SpendItemID
LEFT JOIN udlVendorLoc with(nolock) ON
udlVendorLoc.VendorLoc = udlDeptIssueSpend.VendorLoc AND
udlVendorLoc.VendorSet = udlDeptIssueSpend.VendorSet AND
udlVendorLoc.DeletedYN = 0
inner join(select RowID, RefRowID from DPC WHERE TableID = 'USI' AND Source = 7 AND Phrase = '6650-00') DPC on
DPC.RowID = udlSpendItem.ID and
DPC.RefRowID = udlDeptIssueSpend.DepartmentID
WHERE ISNULL(udlVendorLoc.ServicesOnlyYN, 0) = 0 AND
DATEDIFF(MONTH, udlDeptIssueSpend.TranBeginDate, GETDATE()) BETWEEN 1 AND 12
AND udlDeptIssueSpend.Org in ('1010', '1020', '1030', '1040', '1050')
Group by Dept, Org
order by Org");
            DataTable accountsTable = _WebAppDbAccess.GetDataTable(query);
            return accountsTable;

        }

        public static DataTable AWB_AllSpendAnalysis_ItemDepartmentAllSpendCount()
        {


            string query = string.Format(@"SELECT Org,Dept, count (distinct case when udlSpendITem.PIGItemNo = '' then null else PIGEntityGUID+PIGItemNo end) as ItemCount
FROM udlDeptIssueSpend (nolock)
INNER JOIN udlSpendItem (nolock) ON
udlSpendItem.ID = udlDeptIssueSpend.SpendItemID
LEFT JOIN udlVendorLoc with(nolock) ON
udlVendorLoc.VendorLoc = udlDeptIssueSpend.VendorLoc AND
udlVendorLoc.VendorSet = udlDeptIssueSpend.VendorSet AND
udlVendorLoc.DeletedYN = 0
inner join (select RowID, RefRowID from DPC WHERE TableID = 'USI' AND Source = 7 AND Phrase = '6650-00') DPC on
DPC.RowID = udlSpendItem.ID and
DPC.RefRowID = udlDeptIssueSpend.DepartmentID
WHERE ISNULL(udlVendorLoc.ServicesOnlyYN,0) = 0 AND
DATEDIFF( MONTH, udlDeptIssueSpend.TranBeginDate, GETDATE()) BETWEEN 1 AND 12
AND udlDeptIssueSpend.Org in ('1010','1020','1030','1040','1050')
Group by Dept, Org
order by Org");

            DataTable accountsTable = _WebAppDbAccess.GetDataTable(query);
            return accountsTable;

        }

        public static decimal AWB_FileAnalysis_FileitemsSpendRegisteredSum()
        {


            string query = string.Format(@"SELECT SUM(TotalAmt)
FROM dsSpendDetailMthAll udlTotSpendDetailAct (nolock)
INNER JOIN dsOrgCatalog (nolock)
ON dsOrgCatalog.Catalog = udlTotSpendDetailAct.Catalog
AND dsOrgCatalog.Org = udlTotSpendDetailAct.Org
INNER JOIN udlSpendItem (nolock)
ON udlTotSpendDetailAct.SpendItemID = udlSpendItem.ID
LEFT JOIN udlVendorLoc
ON udlVendorLoc.VendorLoc = udlTotSpendDetailAct.VendorLoc
AND udlVendorLoc.VendorSet = udlTotSpendDetailAct.VendorSet
AND udlVendorLoc.DeletedYN = 0
WHERE udlSpendItem.VendorEntityGuid <> ''
AND ISNULL(udlVendorLoc.ServicesOnlyYN,0) = 0
AND DATEDIFF( MONTH, udlTotSpendDetailAct.TranBeginDate, GETDATE()) BETWEEN 1 AND 12
AND udlTotSpendDetailAct.ItemNo <> ''");

            decimal sumValue = _WebAppDbAccess.GetDataValue<decimal>(query, 90);

            return sumValue;

        }

        public static decimal AWB_FileAnalysis_FileitemsSpendUnRegisteredSum()
        {


            string query = string.Format(@"SELECT SUM(TotalAmt)
FROM dsSpendDetailMthAll udlTotSpendDetailAct (nolock)
INNER JOIN dsOrgCatalog (nolock)
ON dsOrgCatalog.Catalog = udlTotSpendDetailAct.Catalog
AND dsOrgCatalog.Org = udlTotSpendDetailAct.Org
INNER JOIN udlSpendItem (nolock)
ON udlTotSpendDetailAct.SpendItemID = udlSpendItem.ID
LEFT JOIN udlVendorLoc
ON udlVendorLoc.VendorLoc = udlTotSpendDetailAct.VendorLoc
AND udlVendorLoc.VendorSet = udlTotSpendDetailAct.VendorSet
AND udlVendorLoc.DeletedYN = 0
WHERE udlSpendItem.VendorEntityGuid = ''
AND ISNULL(udlVendorLoc.ServicesOnlyYN,0) = 0
AND DATEDIFF( MONTH, udlTotSpendDetailAct.TranBeginDate, GETDATE()) BETWEEN 1 AND 12
AND udlTotSpendDetailAct.ItemNo <> ''");

            decimal sumValue = _WebAppDbAccess.GetDataValue<decimal>(query, 90);

            return sumValue;

        }

        public static decimal AWB_FileAnalysis_FileitemsSpendServiceSum()
        {


            string query = string.Format(@"SELECT SUM(TotalAmt)
FROM dsSpendDetailMthAll udlTotSpendDetailAct (nolock)
INNER JOIN dsOrgCatalog (nolock)
ON dsOrgCatalog.Catalog = udlTotSpendDetailAct.Catalog
AND dsOrgCatalog.Org = udlTotSpendDetailAct.Org
INNER JOIN udlSpendItem (nolock)
ON udlTotSpendDetailAct.SpendItemID = udlSpendItem.ID
LEFT JOIN udlVendorLoc
ON udlVendorLoc.VendorLoc = udlTotSpendDetailAct.VendorLoc
AND udlVendorLoc.VendorSet = udlTotSpendDetailAct.VendorSet
AND udlVendorLoc.DeletedYN = 0
WHERE ISNULL(udlVendorLoc.ServicesOnlyYN,0) = 1
AND DATEDIFF( MONTH, udlTotSpendDetailAct.TranBeginDate, GETDATE()) BETWEEN 1 AND 12
AND udlTotSpendDetailAct.ItemNo <> ''");

            decimal sumValue = _WebAppDbAccess.GetDataValue<decimal>(query, 90);

            return sumValue;

        }

        public static decimal AWB_FileAnalysis_KeywordSpendSum()
        {


            string query = string.Format(@"SELECT SUM(TotalAmt)
FROM dsSpendDetailMthAll udlTotSpendDetailAct (nolock)
INNER JOIN dsOrgCatalog (nolock)
ON dsOrgCatalog.Catalog = udlTotSpendDetailAct.Catalog
AND dsOrgCatalog.Org = udlTotSpendDetailAct.Org
INNER JOIN udlSpendItem (nolock)
ON udlTotSpendDetailAct.SpendItemID = udlSpendItem.ID
Inner Join (Select DISTINCT DPC.TableID, DPC.RowID From DPC with(nolock) WHERE (DPC.TableID = 'USI' )
AND (DPC.Level = '1' And DPC.TableID = 'USI' AND DPC.Source = '1'
AND DPC.Phrase = 'Stent'
)) as KEY_TBL on Key_Tbl.RowID = udlSpendItem.ID
LEFT JOIN udlVendorLoc
ON udlVendorLoc.VendorLoc = udlTotSpendDetailAct.VendorLoc
AND udlVendorLoc.VendorSet = udlTotSpendDetailAct.VendorSet
AND udlVendorLoc.DeletedYN = 0
WHERE udlSpendItem.VendorEntityGuid <> ''
AND ISNULL(udlVendorLoc.ServicesOnlyYN,0) = 0
AND DATEDIFF( MONTH, udlTotSpendDetailAct.TranBeginDate, GETDATE()) BETWEEN 1 AND 12
AND udlTotSpendDetailAct.ItemNo <> ''");

            decimal serviceValue = _WebAppDbAccess.GetDataValue<decimal>(query, 90);

            return serviceValue;

        }

        public static Int32 AWB_FileAnalysis_ItemKeywordMfrMINCount()
        {


            string query = string.Format(@"SELECT COUNT(*) FROM (
SELECT PIGEntityName, PIGItemNo, Count = COUNT(*)
FROM dsSpendDetailMthAll udlTotSpendDetailAct (nolock)
INNER JOIN dsOrgCatalog (nolock)
ON dsOrgCatalog.Catalog = udlTotSpendDetailAct.Catalog
AND dsOrgCatalog.Org = udlTotSpendDetailAct.Org
INNER JOIN udlSpendItem (nolock)
ON udlTotSpendDetailAct.SpendItemID = udlSpendItem.ID
Inner Join (Select DISTINCT DPC.TableID, DPC.RowID From DPC with(nolock) WHERE (DPC.TableID = 'USI' )
AND (DPC.Level = '1' And DPC.TableID = 'USI' AND DPC.Source = '1'
AND DPC.Phrase = 'Stent'
)) as KEY_TBL on Key_Tbl.RowID = udlSpendItem.ID
LEFT JOIN udlVendorLoc
ON udlVendorLoc.VendorLoc = udlTotSpendDetailAct.VendorLoc
AND udlVendorLoc.VendorSet = udlTotSpendDetailAct.VendorSet
AND udlVendorLoc.DeletedYN = 0
WHERE udlSpendItem.VendorEntityGuid <> ''
AND ISNULL(udlVendorLoc.ServicesOnlyYN,0) = 0
AND DATEDIFF( MONTH, udlTotSpendDetailAct.TranBeginDate, GETDATE()) BETWEEN 1 AND 12
AND udlTotSpendDetailAct.ItemNo <> ''
AND udlSpendItem.PIGItemNo <> ''
GROUP BY PIGEntityName, PIGItemNo
) d");

            Int32 serviceValue = _WebAppDbAccess.GetDataValue<Int32>(query, 90);

            return serviceValue;

        }

        public static decimal AWB_FileAnalysis_ItemUNSPSCCodeSpendSum()
        {


            string query = string.Format(@"SELECT SUM(TotalAmt)
FROM dsSpendDetailMthAll udlTotSpendDetailAct (nolock)
INNER JOIN dsOrgCatalog (nolock)
ON dsOrgCatalog.Catalog = udlTotSpendDetailAct.Catalog
AND dsOrgCatalog.Org = udlTotSpendDetailAct.Org
INNER JOIN udlSpendItem (nolock)
ON udlTotSpendDetailAct.SpendItemID = udlSpendItem.ID
LEFT JOIN udlVendorLoc
ON udlVendorLoc.VendorLoc = udlTotSpendDetailAct.VendorLoc
AND udlVendorLoc.VendorSet = udlTotSpendDetailAct.VendorSet
AND udlVendorLoc.DeletedYN = 0
WHERE udlSpendItem.VendorEntityGuid <> ''
AND ISNULL(udlVendorLoc.ServicesOnlyYN,0) = 0
AND DATEDIFF( MONTH, udlTotSpendDetailAct.TranBeginDate, GETDATE()) BETWEEN 1 AND 12
AND udlTotSpendDetailAct.ItemNo <> ''
AND udlSpendItem.ID IN ( SELECT RowID FROM DPC WHERE TableID = 'USI' AND Level = 1 AND Source = 2 AND Phrase = '42321605' )");

            decimal serviceValue = _WebAppDbAccess.GetDataValue<decimal>(query, 90);

            return serviceValue;

        }

        public static Int32 AWB_FileAnalysis_ItemUNSPSCCodeMfrMINCount()
        {


            string query = string.Format(@"SELECT COUNT(*) FROM (
SELECT PIGEntityName, PIGItemNo, Count = COUNT(*)
FROM dsSpendDetailMthAll udlTotSpendDetailAct (nolock)
INNER JOIN dsOrgCatalog (nolock)
ON dsOrgCatalog.Catalog = udlTotSpendDetailAct.Catalog
AND dsOrgCatalog.Org = udlTotSpendDetailAct.Org
INNER JOIN udlSpendItem (nolock)
ON udlTotSpendDetailAct.SpendItemID = udlSpendItem.ID
LEFT JOIN udlVendorLoc
ON udlVendorLoc.VendorLoc = udlTotSpendDetailAct.VendorLoc
AND udlVendorLoc.VendorSet = udlTotSpendDetailAct.VendorSet
AND udlVendorLoc.DeletedYN = 0
WHERE udlSpendItem.VendorEntityGuid <> ''
AND ISNULL(udlVendorLoc.ServicesOnlyYN,0) = 0
AND DATEDIFF( MONTH, udlTotSpendDetailAct.TranBeginDate, GETDATE()) BETWEEN 1 AND 12
AND udlTotSpendDetailAct.ItemNo <> ''
AND udlSpendItem.ID IN ( SELECT RowID FROM DPC WHERE TableID = 'USI' AND Level = 1 AND Source = 2 AND Phrase = '42321605' )
AND udlSpendItem.PIGItemNo <> ''
GROUP BY PIGEntityName, PIGItemNo
) d");

            Int32 serviceValue = _WebAppDbAccess.GetDataValue<Int32>(query, 90);

            return serviceValue;

        }

       
        public static Int32 AWB_FileAnalysis_ItemManufacturerChildSpendCount()
        {


            string query = string.Format(@"SELECT COUNT(*) FROM (
SELECT PIGEntityName, PIGItemNo, Count = COUNT(*)
FROM dsSpendDetailMthAll udlTotSpendDetailAct (nolock)
INNER JOIN dsOrgCatalog (nolock)
ON dsOrgCatalog.Catalog = udlTotSpendDetailAct.Catalog
AND dsOrgCatalog.Org = udlTotSpendDetailAct.Org
INNER JOIN udlSpendItem (nolock)
ON udlTotSpendDetailAct.SpendItemID = udlSpendItem.ID
LEFT JOIN udlVendorLoc
ON udlVendorLoc.VendorLoc = udlTotSpendDetailAct.VendorLoc
AND udlVendorLoc.VendorSet = udlTotSpendDetailAct.VendorSet
AND udlVendorLoc.DeletedYN = 0
WHERE udlSpendItem.VendorEntityGuid <> ''
AND ISNULL(udlVendorLoc.ServicesOnlyYN,0) = 0
AND DATEDIFF( MONTH, udlTotSpendDetailAct.TranBeginDate, GETDATE()) BETWEEN 1 AND 12
AND udlTotSpendDetailAct.ItemNo <> ''
AND udlSpendItem.PIGEntityName = 'Covidien LTD'
AND udlSpendItem.PIGItemNo <> ''
GROUP BY PIGEntityName, PIGItemNo
) d");

            Int32 serviceValue = _WebAppDbAccess.GetDataValue<Int32>(query, 90);

            return serviceValue;

        }


        
        public static Int32 AWB_FileAnalysis_ItemManufacturerParentSpendCount()
        {


            string query = string.Format(@"SELECT COUNT(*) FROM (
SELECT PIGEntityName, PIGItemNo, Count = COUNT(*)
FROM dsSpendDetailMthAll udlTotSpendDetailAct (nolock)
INNER JOIN dsOrgCatalog (nolock)
ON dsOrgCatalog.Catalog = udlTotSpendDetailAct.Catalog
AND dsOrgCatalog.Org = udlTotSpendDetailAct.Org
INNER JOIN udlSpendItem (nolock)
ON udlTotSpendDetailAct.SpendItemID = udlSpendItem.ID
inner join (select Distinct RowID, RefRowID from DPC WHERE TableID = 'USI' AND Level = '2' AND Source = 23 AND Phrase = 'Covidien LTD') DPC on
DPC.RowID = udlSpendItem.ID
LEFT JOIN udlVendorLoc
ON udlVendorLoc.VendorLoc = udlTotSpendDetailAct.VendorLoc
AND udlVendorLoc.VendorSet = udlTotSpendDetailAct.VendorSet
AND udlVendorLoc.DeletedYN = 0
WHERE udlSpendItem.VendorEntityGuid <> ''
And udlSpendItem.PIGEntityGUID <> ''
AND ISNULL(udlVendorLoc.ServicesOnlyYN,0) = 0
AND DATEDIFF( MONTH, udlTotSpendDetailAct.TranBeginDate, GETDATE()) BETWEEN 1 AND 12
AND udlTotSpendDetailAct.ItemNo <> ''
AND udlSpendItem.PIGItemNo <> ''
GROUP BY PIGEntityName, PIGItemNo
) d");

            Int32 serviceValue = _WebAppDbAccess.GetDataValue<Int32>(query, 90);

            return serviceValue;

        }

        public static decimal AWB_FileAnalysis_VendorTotalChildSpendSum()
        {


            string query = string.Format(@"SELECT SUM(TotalAmt)
FROM dsSpendDetailMthAll udlTotSpendDetailAct (nolock)
INNER JOIN dsOrgCatalog (nolock)
ON dsOrgCatalog.Catalog = udlTotSpendDetailAct.Catalog
AND dsOrgCatalog.Org = udlTotSpendDetailAct.Org
INNER JOIN udlSpendItem (nolock)
ON udlTotSpendDetailAct.SpendItemID = udlSpendItem.ID
LEFT JOIN udlVendorLoc
ON udlVendorLoc.VendorLoc = udlTotSpendDetailAct.VendorLoc
AND udlVendorLoc.VendorSet = udlTotSpendDetailAct.VendorSet
AND udlVendorLoc.DeletedYN = 0
WHERE udlSpendItem.VendorEntityGuid <> ''
AND ISNULL(udlVendorLoc.ServicesOnlyYN,0) = 0
AND DATEDIFF( MONTH, udlTotSpendDetailAct.TranBeginDate, GETDATE()) BETWEEN 1 AND 12
AND udlTotSpendDetailAct.ItemNo <> ''
AND udlSpendItem.VendorEntityName = 'Depuy Spine Inc'");

            decimal serviceValue = _WebAppDbAccess.GetDataValue<decimal>(query, 90);

            return serviceValue;

        }

        public static Int32 AWB_FileAnalysis_VendorTotalChildSpendCount()
        {


            string query = string.Format(@"SELECT COUNT(*) FROM (
SELECT PIGEntityName, PIGItemNo, Count = COUNT(*)
FROM dsSpendDetailMthAll udlTotSpendDetailAct (nolock)
INNER JOIN dsOrgCatalog (nolock)
ON dsOrgCatalog.Catalog = udlTotSpendDetailAct.Catalog
AND dsOrgCatalog.Org = udlTotSpendDetailAct.Org
INNER JOIN udlSpendItem (nolock)
ON udlTotSpendDetailAct.SpendItemID = udlSpendItem.ID
LEFT JOIN udlVendorLoc
ON udlVendorLoc.VendorLoc = udlTotSpendDetailAct.VendorLoc
AND udlVendorLoc.VendorSet = udlTotSpendDetailAct.VendorSet
AND udlVendorLoc.DeletedYN = 0
WHERE udlSpendItem.VendorEntityGuid <> ''
AND ISNULL(udlVendorLoc.ServicesOnlyYN,0) = 0
AND DATEDIFF( MONTH, udlTotSpendDetailAct.TranBeginDate, GETDATE()) BETWEEN 1 AND 12
AND udlTotSpendDetailAct.ItemNo <> ''
AND udlSpendItem.VendorEntityName = 'Depuy Spine Inc'
AND udlSpendItem.PIGItemNo <> ''
GROUP BY PIGEntityName, PIGItemNo
) d");

            Int32 serviceValue = _WebAppDbAccess.GetDataValue<Int32>(query, 90);

            return serviceValue;

        }


        public static decimal AWB_FileAnalysis_VendorTotalParentSpendSum()
        {


            string query = string.Format(@"SELECT SUM(TotalAmt)
FROM dsSpendDetailMthAll udlTotSpendDetailAct (nolock)
INNER JOIN dsOrgCatalog (nolock)
ON dsOrgCatalog.Catalog = udlTotSpendDetailAct.Catalog
AND dsOrgCatalog.Org = udlTotSpendDetailAct.Org
INNER JOIN udlSpendItem (nolock)
ON udlTotSpendDetailAct.SpendItemID = udlSpendItem.ID
LEFT JOIN udlVendorLoc
ON udlVendorLoc.VendorLoc = udlTotSpendDetailAct.VendorLoc
AND udlVendorLoc.VendorSet = udlTotSpendDetailAct.VendorSet
AND udlVendorLoc.DeletedYN = 0
WHERE udlSpendItem.VendorEntityGuid <> ''
AND ISNULL(udlVendorLoc.ServicesOnlyYN,0) = 0
AND DATEDIFF( MONTH, udlTotSpendDetailAct.TranBeginDate, GETDATE()) BETWEEN 1 AND 12
AND udlTotSpendDetailAct.ItemNo <> ''
AND udlSpendItem.VendorMatchName = 'Johnson & Johnson'");

            decimal serviceValue = _WebAppDbAccess.GetDataValue<decimal>(query, 90);

            return serviceValue;

        }

        public static Int32 AWB_FileAnalysis_VendorTotalParentSpendCount()
        {


            string query = string.Format(@"SELECT COUNT(*) FROM (
SELECT PIGEntityName, PIGItemNo, Count = COUNT(*)
FROM dsSpendDetailMthAll udlTotSpendDetailAct (nolock)
INNER JOIN dsOrgCatalog (nolock)
ON dsOrgCatalog.Catalog = udlTotSpendDetailAct.Catalog
AND dsOrgCatalog.Org = udlTotSpendDetailAct.Org
INNER JOIN udlSpendItem (nolock)
ON udlTotSpendDetailAct.SpendItemID = udlSpendItem.ID
LEFT JOIN udlVendorLoc
ON udlVendorLoc.VendorLoc = udlTotSpendDetailAct.VendorLoc
AND udlVendorLoc.VendorSet = udlTotSpendDetailAct.VendorSet
AND udlVendorLoc.DeletedYN = 0
WHERE udlSpendItem.VendorEntityGuid <> ''
AND ISNULL(udlVendorLoc.ServicesOnlyYN,0) = 0
AND DATEDIFF( MONTH, udlTotSpendDetailAct.TranBeginDate, GETDATE()) BETWEEN 1 AND 12
AND udlTotSpendDetailAct.ItemNo <> ''
AND udlSpendItem.VendorMatchName = 'Johnson & Johnson'
AND udlSpendItem.PIGItemNo <> ''
GROUP BY PIGEntityName, PIGItemNo
) d");

            Int32 serviceValue = _WebAppDbAccess.GetDataValue<Int32>(query, 90);

            return serviceValue;

        }

        public static decimal AWB_FileAnalysis_ItemContractAllSpendSum()
        {


            string query = string.Format(@"SELECT SUM(TotalAmt)
FROM dsSpendDetailMthAll udlTotSpendDetailAct (nolock)
INNER JOIN dsOrgCatalog (nolock)
ON dsOrgCatalog.Catalog = udlTotSpendDetailAct.Catalog
AND dsOrgCatalog.Org = udlTotSpendDetailAct.Org
INNER JOIN udlSpendItem (nolock)
ON udlTotSpendDetailAct.SpendItemID = udlSpendItem.ID
LEFT JOIN udlVendorLoc
ON udlVendorLoc.VendorLoc = udlTotSpendDetailAct.VendorLoc
AND udlVendorLoc.VendorSet = udlTotSpendDetailAct.VendorSet
AND udlVendorLoc.DeletedYN = 0
WHERE udlSpendItem.VendorEntityGuid <> ''
AND ISNULL(udlVendorLoc.ServicesOnlyYN,0) = 0
AND DATEDIFF( MONTH, udlTotSpendDetailAct.TranBeginDate, GETDATE()) BETWEEN 1 AND 12
AND udlTotSpendDetailAct.ItemNo <> ''
AND udlSpendItem.ID IN ( SELECT RowID FROM DPC WHERE TableID = 'USI' AND Level = 1 AND Source = 26 AND Phrase = 'PP-IV-133' )");

            decimal serviceValue = _WebAppDbAccess.GetDataValue<decimal>(query, 90);

            return serviceValue;

        }

        public static Int32 AWB_FileAnalysis_ItemContractAllSpendCount()
        {


            string query = string.Format(@"SELECT COUNT(*) FROM (
SELECT PIGEntityName, PIGItemNo, Count = COUNT(*)
FROM dsSpendDetailMthAll udlTotSpendDetailAct (nolock)
INNER JOIN dsOrgCatalog (nolock)
ON dsOrgCatalog.Catalog = udlTotSpendDetailAct.Catalog
AND dsOrgCatalog.Org = udlTotSpendDetailAct.Org
INNER JOIN udlSpendItem (nolock)
ON udlTotSpendDetailAct.SpendItemID = udlSpendItem.ID
LEFT JOIN udlVendorLoc
ON udlVendorLoc.VendorLoc = udlTotSpendDetailAct.VendorLoc
AND udlVendorLoc.VendorSet = udlTotSpendDetailAct.VendorSet
AND udlVendorLoc.DeletedYN = 0
WHERE udlSpendItem.VendorEntityGuid <> ''
AND ISNULL(udlVendorLoc.ServicesOnlyYN,0) = 0
AND DATEDIFF( MONTH, udlTotSpendDetailAct.TranBeginDate, GETDATE()) BETWEEN 1 AND 12
AND udlTotSpendDetailAct.ItemNo <> ''
AND udlSpendItem.ID IN ( SELECT RowID FROM DPC WHERE TableID = 'USI' AND Level = 1 AND Source = 26 AND Phrase = 'PP-IV-133' )
AND udlSpendItem.PIGItemNo <> ''
GROUP BY PIGEntityName, PIGItemNo
) d");

            Int32 serviceValue = _WebAppDbAccess.GetDataValue<Int32>(query, 90);

            return serviceValue;

        }

        public static DataTable AWB_FileAnalysis_ItemDepartmentAllSpendSum()
        {


            string query = string.Format(@"SELECT Org,Dept,SUM(TotalAmt) as DeptSpend
FROM udlDeptIssueSpend (nolock)
INNER JOIN udlSpendItem (nolock) ON
udlSpendItem.ID = udlDeptIssueSpend.SpendItemID
LEFT JOIN udlVendorLoc with(nolock) ON
udlVendorLoc.VendorLoc = udlDeptIssueSpend.VendorLoc AND
udlVendorLoc.VendorSet = udlDeptIssueSpend.VendorSet AND
udlVendorLoc.DeletedYN = 0
inner join (select RowID, RefRowID from DPC WHERE TableID = 'USI' AND Source = 7 AND Phrase = '5870-00') DPC on
DPC.RowID = udlSpendItem.ID and
DPC.RefRowID = udlDeptIssueSpend.DepartmentID
WHERE ISNULL(udlVendorLoc.ServicesOnlyYN,0) = 0 AND
DATEDIFF( MONTH, udlDeptIssueSpend.TranBeginDate, GETDATE()) BETWEEN 1 AND 12
AND udlDeptIssueSpend.ItemNo <> ''
AND udlDeptIssueSpend.Org in ('1010','1020','1030','1040','1050')
Group by Dept, Org
order by Org");
            DataTable accountsTable = _WebAppDbAccess.GetDataTable(query);
            return accountsTable;

        }

        public static DataTable AWB_FileAnalysis_ItemDepartmentAllSpendCount()
        {


            string query = string.Format(@"SELECT Org,Dept, count (distinct case when udlSpendITem.PIGItemNo = '' then null else PIGEntityGUID+PIGItemNo end) as ItemCount
FROM udlDeptIssueSpend (nolock)
INNER JOIN udlSpendItem (nolock) ON
udlSpendItem.ID = udlDeptIssueSpend.SpendItemID
LEFT JOIN udlVendorLoc with(nolock) ON
udlVendorLoc.VendorLoc = udlDeptIssueSpend.VendorLoc AND
udlVendorLoc.VendorSet = udlDeptIssueSpend.VendorSet AND
udlVendorLoc.DeletedYN = 0
inner join (select RowID, RefRowID from DPC WHERE TableID = 'USI' AND Source = 7 AND Phrase = '5870-00') DPC on
DPC.RowID = udlSpendItem.ID and
DPC.RefRowID = udlDeptIssueSpend.DepartmentID
WHERE ISNULL(udlVendorLoc.ServicesOnlyYN,0) = 0 AND
DATEDIFF( MONTH, udlDeptIssueSpend.TranBeginDate, GETDATE()) BETWEEN 1 AND 12
AND udlDeptIssueSpend.ItemNo <> ''
AND udlDeptIssueSpend.Org in ('1010','1020','1030','1040','1050')
Group by Dept, Org
order by Org");

            DataTable accountsTable = _WebAppDbAccess.GetDataTable(query);
            return accountsTable;

        }

        public static dynamic AWB_AllSpendAnalysis_GroupAllSpendSum()
        {


            string query = string.Format(@"SELECT
CASE WHEN SUM(TotalAmt) IS NULL THEN '$0.00'
WHEN SUM(TotalAmt) < 0THEN '($' + CONVERT(varchar(50), CAST(ABS(SUM(TotalAmt)) as MONEY), -1) +')'
ELSE '$' + CONVERT(varchar(50), CAST(ABS(SUM(TotalAmt)) as MONEY), -1)
END Amount
FROM dsSpendDetailMthAll udlTotSpendDetailAct (nolock)
INNER JOIN dsOrgCatalog (nolock)
ON dsOrgCatalog.Catalog = udlTotSpendDetailAct.Catalog
AND dsOrgCatalog.Org = udlTotSpendDetailAct.Org
INNER JOIN udlSpendItem (nolock)
ON udlTotSpendDetailAct.SpendItemID = udlSpendItem.ID
LEFT JOIN udlVendorLoc WITH(NOLOCK)
ON udlVendorLoc.VendorLoc = udlTotSpendDetailAct.VendorLoc
AND udlVendorLoc.VendorSet = udlTotSpendDetailAct.VendorSet
AND udlVendorLoc.DeletedYN = 0
WHERE udlSpendItem.VendorEntityGuid <> ''
AND ISNULL(udlVendorLoc.ServicesOnlyYN,0) = 0
AND DATEDIFF( MONTH, udlTotSpendDetailAct.TranBeginDate, GETDATE()) BETWEEN 1 AND 12
AND udlSpendItem.ID IN ( SELECT RowID FROM DPC WHERE TableID = 'USI' AND Level = 1 AND Source = 9 AND Phrase = 'Surgical Gloves' )");
            dynamic serviceValue = _WebAppDbAccess.GetDataValue<dynamic>(query, 90);

            return serviceValue;

        }

        public static Int32 AWB_AllSpendAnalysis_GroupAllSpendCount()
        {


            string query = string.Format(@"SELECT COUNT(*) FROM (
SELECT PIGEntityName, PIGItemNo, Count = COUNT(*)
FROM dsSpendDetailMthAll udlTotSpendDetailAct WITH(NOLOCK)
INNER JOIN dsOrgCatalog WITH(NOLOCK)
ON dsOrgCatalog.Catalog = udlTotSpendDetailAct.Catalog
AND dsOrgCatalog.Org = udlTotSpendDetailAct.Org
INNER JOIN udlSpendItem WITH(NOLOCK)
ON udlTotSpendDetailAct.SpendItemID = udlSpendItem.ID
LEFT JOIN udlVendorLoc WITH(NOLOCK)
ON udlVendorLoc.VendorLoc = udlTotSpendDetailAct.VendorLoc
AND udlVendorLoc.VendorSet = udlTotSpendDetailAct.VendorSet
AND udlVendorLoc.DeletedYN = 0
WHERE udlSpendItem.VendorEntityGuid <> ''
AND ISNULL(udlVendorLoc.ServicesOnlyYN,0) = 0
AND DATEDIFF( MONTH, udlTotSpendDetailAct.TranBeginDate, GETDATE()) BETWEEN 1 AND 12
AND udlSpendItem.ID IN ( SELECT RowID FROM DPC WITH(NOLOCK)WHERE TableID = 'USI' AND Level = 1 AND Source = 9 AND Phrase = 'Surgical Gloves' )
AND udlSpendItem.PIGItemNo <> ''
GROUP BY PIGEntityName, PIGItemNo
) d");

            Int32 serviceValue = _WebAppDbAccess.GetDataValue<Int32>(query, 90);

            return serviceValue;

        }

        public static List<string> AWB_AllSpendAnalysis_ClassSubSpendSum()
        {
            string query = string.Format(@"SELECT
CASE WHEN SUM(TotalAmt) IS NULL THEN '$0.00'
WHEN SUM(TotalAmt) < 0THEN '($' + CONVERT(varchar(50), CAST(ABS(SUM(TotalAmt)) as MONEY), -1) +')'
ELSE '$' + CONVERT(varchar(50), CAST(ABS(SUM(TotalAmt)) as MONEY), -1)
END Amount
FROM dsSpendDetailMthAll udlTotSpendDetailAct (nolock)
INNER JOIN dsOrgCatalog (nolock)
ON dsOrgCatalog.Catalog = udlTotSpendDetailAct.Catalog
AND dsOrgCatalog.Org = udlTotSpendDetailAct.Org
INNER JOIN udlSpendItem (nolock)
ON udlTotSpendDetailAct.SpendItemID = udlSpendItem.ID
INNER JOIN DPC WITH(NOLOCK)
ON udlSpendItem.ID = DPC.RowID
LEFT JOIN udlVendorLoc WITH(NOLOCK)
ON udlVendorLoc.VendorLoc = udlTotSpendDetailAct.VendorLoc
AND udlVendorLoc.VendorSet = udlTotSpendDetailAct.VendorSet
AND udlVendorLoc.DeletedYN = 0
LEFT OUTER JOIN udlItemSubClass WITH(NOLOCK)
ON udlItemSubClass.ID = DPC.RefRowID
WHERE udlSpendItem.VendorEntityGuid <> ''
AND ISNULL(udlVendorLoc.ServicesOnlyYN,0) = 0
AND DATEDIFF( MONTH, udlTotSpendDetailAct.TranBeginDate, GETDATE()) BETWEEN 1 AND 12
AND DPC.TableID = 'USI' AND DPC.Level = 1 AND DPC.Source = 14 AND DPC.Phrase = 'Active Humidification'
GROUP BY udlItemSubClass.ItemSubClassDesc");

            DataTable content = _WebAppDbAccess.GetDataTable(query, 90);

            return DataUtils.DataRowsToListString(content.Rows, null, true);
        }

        public static Int32 AWB_AllSpendAnalysis_ClassSubSpendCount()
        {


            string query = string.Format(@"SELECT COUNT(*) FROM (
SELECT PIGEntityName, PIGItemNo, Count = COUNT(*), udlItemSubClass.ItemSubClassDesc
FROM dsSpendDetailMthAll udlTotSpendDetailAct (nolock)
INNER JOIN dsOrgCatalog (nolock)
ON dsOrgCatalog.Catalog = udlTotSpendDetailAct.Catalog
AND dsOrgCatalog.Org = udlTotSpendDetailAct.Org
INNER JOIN udlSpendItem (nolock)
ON udlTotSpendDetailAct.SpendItemID = udlSpendItem.ID
INNER JOIN DPC WITH(NOLOCK)
ON udlSpendItem.ID = DPC.RowID
LEFT JOIN udlVendorLoc WITH(NOLOCK)
ON udlVendorLoc.VendorLoc = udlTotSpendDetailAct.VendorLoc
AND udlVendorLoc.VendorSet = udlTotSpendDetailAct.VendorSet
AND udlVendorLoc.DeletedYN = 0
LEFT OUTER JOIN udlItemSubClass WITH(NOLOCK)
ON udlItemSubClass.ID = DPC.RefRowID
WHERE udlSpendItem.VendorEntityGuid <> ''
AND ISNULL(udlVendorLoc.ServicesOnlyYN,0) = 0
AND DATEDIFF( MONTH, udlTotSpendDetailAct.TranBeginDate, GETDATE()) BETWEEN 1 AND 12
AND DPC.TableID = 'USI' AND DPC.Level = 1 AND DPC.Source = 14 AND DPC.Phrase = 'Active Humidification'
AND udlSpendItem.PIGItemNo <> ''
GROUP BY PIGEntityName, PIGItemNo, udlItemSubClass.ItemSubClassDesc
)d");

            Int32 serviceValue = _WebAppDbAccess.GetDataValue<Int32>(query, 90);

            return serviceValue;

        }

        public static dynamic AWB_AllSpendAnalysis_ClassSpendSum()
        {


            string query = string.Format(@"SELECT
CASE WHEN SUM(TotalAmt) IS NULL THEN '$0.00'
WHEN SUM(TotalAmt) < 0THEN '($' + CONVERT(varchar(50), CAST(ABS(SUM(TotalAmt)) as MONEY), -1) +')'
ELSE '$' + CONVERT(varchar(50), CAST(ABS(SUM(TotalAmt)) as MONEY), -1)
END Amount
FROM dsSpendDetailMthAll udlTotSpendDetailAct (nolock)
INNER JOIN dsOrgCatalog (nolock)
ON dsOrgCatalog.Catalog = udlTotSpendDetailAct.Catalog
AND dsOrgCatalog.Org = udlTotSpendDetailAct.Org
INNER JOIN udlSpendItem (nolock)
ON udlTotSpendDetailAct.SpendItemID = udlSpendItem.ID
LEFT JOIN udlVendorLoc
ON udlVendorLoc.VendorLoc = udlTotSpendDetailAct.VendorLoc
AND udlVendorLoc.VendorSet = udlTotSpendDetailAct.VendorSet
AND udlVendorLoc.DeletedYN = 0
WHERE udlSpendItem.VendorEntityGuid <> ''
AND ISNULL(udlVendorLoc.ServicesOnlyYN,0) = 0
AND DATEDIFF( MONTH, udlTotSpendDetailAct.TranBeginDate, GETDATE()) BETWEEN 1 AND 12
AND udlSpendItem.ID IN ( SELECT RowID FROM DPC WHERE TableID = 'USI' AND Level = 1 AND Source = 13 AND Phrase = 'Active Humidification' )");
            dynamic serviceValue = _WebAppDbAccess.GetDataValue<dynamic>(query, 90);

            return serviceValue;

        }

        public static Int32 AWB_AllSpendAnalysis_ClassSpendCount()
        {


            string query = string.Format(@"SELECT COUNT(*) FROM (
SELECT PIGEntityName, PIGItemNo, Count = COUNT(*)
FROM dsSpendDetailMthAll udlTotSpendDetailAct (nolock)
INNER JOIN dsOrgCatalog (nolock)
ON dsOrgCatalog.Catalog = udlTotSpendDetailAct.Catalog
AND dsOrgCatalog.Org = udlTotSpendDetailAct.Org
INNER JOIN udlSpendItem (nolock)
ON udlTotSpendDetailAct.SpendItemID = udlSpendItem.ID
LEFT JOIN udlVendorLoc
ON udlVendorLoc.VendorLoc = udlTotSpendDetailAct.VendorLoc
AND udlVendorLoc.VendorSet = udlTotSpendDetailAct.VendorSet
AND udlVendorLoc.DeletedYN = 0
WHERE udlSpendItem.VendorEntityGuid <> ''
AND ISNULL(udlVendorLoc.ServicesOnlyYN,0) = 0
AND DATEDIFF( MONTH, udlTotSpendDetailAct.TranBeginDate, GETDATE()) BETWEEN 1 AND 12
AND udlSpendItem.ID IN ( SELECT RowID FROM DPC WHERE TableID = 'USI' AND Level = 1 AND Source = 13 AND Phrase = 'Active Humidification' )
AND udlSpendItem.PIGItemNo <> ''
GROUP BY PIGEntityName, PIGItemNo
) d");

            Int32 serviceValue = _WebAppDbAccess.GetDataValue<Int32>(query, 90);

            return serviceValue;

        }

        public static dynamic AWB_AllSpendAnalysis_CategorySpendSum()
        {


            string query = string.Format(@"SELECT
CASE WHEN SUM(TotalAmt) IS NULL THEN '$0.00'
WHEN SUM(TotalAmt) < 0THEN '($' + CONVERT(varchar(50), CAST(ABS(SUM(TotalAmt)) as MONEY), -1) +')'
ELSE '$' + CONVERT(varchar(50), CAST(ABS(SUM(TotalAmt)) as MONEY), -1)
END Amount
FROM dsSpendDetailMthAll udlTotSpendDetailAct (nolock)
INNER JOIN dsOrgCatalog (nolock)
ON dsOrgCatalog.Catalog = udlTotSpendDetailAct.Catalog
AND dsOrgCatalog.Org = udlTotSpendDetailAct.Org
INNER JOIN udlSpendItem (nolock)
ON udlTotSpendDetailAct.SpendItemID = udlSpendItem.ID
LEFT JOIN udlVendorLoc
ON udlVendorLoc.VendorLoc = udlTotSpendDetailAct.VendorLoc
AND udlVendorLoc.VendorSet = udlTotSpendDetailAct.VendorSet
AND udlVendorLoc.DeletedYN = 0
WHERE udlSpendItem.VendorEntityGuid <> ''
AND ISNULL(udlVendorLoc.ServicesOnlyYN,0) = 0
AND DATEDIFF( MONTH, udlTotSpendDetailAct.TranBeginDate, GETDATE()) BETWEEN 1 AND 12
AND udlSpendItem.ID IN ( SELECT RowID FROM DPC WHERE TableID = 'USI' AND Level = 1 AND Source = 100 AND Phrase = 'Adhesive Skin Closures' )");
            dynamic serviceValue = _WebAppDbAccess.GetDataValue<dynamic>(query, 90);

            return serviceValue;

        }

        public static Int32 AWB_AllSpendAnalysis_CategorySpendCount()
        {


            string query = string.Format(@"SELECT COUNT(*) FROM (
SELECT PIGEntityName, PIGItemNo, Count = COUNT(*)
FROM dsSpendDetailMthAll udlTotSpendDetailAct WITH(NOLOCK)
INNER JOIN dsOrgCatalog WITH(NOLOCK)
ON dsOrgCatalog.Catalog = udlTotSpendDetailAct.Catalog
AND dsOrgCatalog.Org = udlTotSpendDetailAct.Org
INNER JOIN udlSpendItem WITH(NOLOCK)
ON udlTotSpendDetailAct.SpendItemID = udlSpendItem.ID
LEFT JOIN udlVendorLoc WITH(NOLOCK)
ON udlVendorLoc.VendorLoc = udlTotSpendDetailAct.VendorLoc
AND udlVendorLoc.VendorSet = udlTotSpendDetailAct.VendorSet
AND udlVendorLoc.DeletedYN = 0
WHERE udlSpendItem.VendorEntityGuid <> ''
AND ISNULL(udlVendorLoc.ServicesOnlyYN,0) = 0
AND DATEDIFF( MONTH, udlTotSpendDetailAct.TranBeginDate, GETDATE()) BETWEEN 1 AND 12
AND udlSpendItem.ID IN ( SELECT RowID FROM DPC WITH(NOLOCK)WHERE TableID = 'USI' AND Level = 1 AND Source = 100 AND Phrase = 'Adhesive Skin Closures' )
AND udlSpendItem.PIGItemNo <> ''
GROUP BY PIGEntityName, PIGItemNo
) d");

            Int32 serviceValue = _WebAppDbAccess.GetDataValue<Int32>(query, 90);

            return serviceValue;

        }

        public static DataTable AWB_AllSpendAnalysis_RegionOrgSpendSum()
        {


            string query = string.Format(@"Select HistoricalSpend.Org
,CASE WHEN SUM(TotalAmt) IS NULL THEN '$0.00'
WHEN SUM(TotalAmt) < 0THEN '($' + CONVERT(varchar(50), CAST(ABS(SUM(TotalAmt)) as MONEY), -1) +')'
ELSE '$' + CONVERT(varchar(50), CAST(ABS(SUM(TotalAmt)) as MONEY), -1)
END Amount
from dsSpendDetailMthAll HistoricalSpend with(nolock)
Inner Join udlSpendItem with(nolock) On
udlSpendItem.ID = HistoricalSpend.SpendItemID
left outer Join udlVendorLoc with(nolock) On
udlVendorLoc.VendorLoc = udlSpendItem.VendorLoc And
udlVendorLoc.VendorSet = udlSpendItem.VendorSet And
udlVendorLoc.DeletedYN = 0

Where udlSpendItem.VendorEntityGUID <> ''
And udlSpendItem.PIGEntityGUID <> ''
And ISNULL(udlVendorLoc.ServicesOnlyYN,0) = 0
AND DATEDIFF( MONTH, TranBeginDate, GETDATE()) BETWEEN 1 AND 12

GROUP BY HistoricalSpend.Org");
            DataTable accountsTable = _WebAppDbAccess.GetDataTable(query);
            return accountsTable;

        }

        public static DataTable AWB_NonFileAnalysis_RegionOrgSpendSum()
        {
            string query = string.Format(@"SELECT CASE WHEN SUM(TotalAmt) IS NULL THEN '$0.00'
WHEN SUM(TotalAmt) < 0 THEN '($' + CONVERT(varchar(50), CAST(ABS(SUM(TotalAmt)) as MONEY), -1) +')'
ELSE '$' + CONVERT(varchar(50), CAST(ABS(SUM(TotalAmt)) as MONEY), -1)END Amount
, udlDeptIssueSpend.Org
FROM udlDeptIssueSpend WITH(NOLOCK)
INNER JOIN udlSpendItem WITH(NOLOCK)
ON udlSpendItem.ID = udlDeptIssueSpend.SpendItemID
LEFT JOIN udlVendorLoc WITH(NOLOCK)
ON udlVendorLoc.VendorLoc = udlDeptIssueSpend.VendorLoc
AND udlVendorLoc.VendorSet = udlDeptIssueSpend.VendorSet
AND udlVendorLoc.DeletedYN = 0
inner join (select RowID, RefRowID from DPC WITH(NOLOCK) WHERE TableID = 'USI' AND Source = 7 AND Phrase = '1000-00') DPC
on DPC.RowID = udlSpendItem.ID
and DPC.RefRowID = udlDeptIssueSpend.DepartmentID
WHERE DATEDIFF( MONTH, udlDeptIssueSpend.TranBeginDate, GETDATE()) BETWEEN 1 AND 12
AND udlDeptIssueSpend.ItemNo = ''
GROUP BY udlDeptIssueSpend.Org
");
            DataTable accountsTable = _WebAppDbAccess.GetDataTable(query);
            return accountsTable;

        }

        public static DataTable AWB_NonFileAnalysis_RegionOrgSpendCount()
        {
            string query = string.Format(@"SELECT
count (distinct case when udlSpendITem.PIGItemNo = '' then null else PIGEntityGUID+PIGItemNo end) as ItemCount
, udlDeptIssueSpend.Org
FROM udlDeptIssueSpend WITH(NOLOCK)
INNER JOIN udlSpendItem WITH(NOLOCK)
ON udlSpendItem.ID = udlDeptIssueSpend.SpendItemID
LEFT JOIN udlVendorLoc WITH(NOLOCK)
ON udlVendorLoc.VendorLoc = udlDeptIssueSpend.VendorLoc
AND udlVendorLoc.VendorSet = udlDeptIssueSpend.VendorSet
AND udlVendorLoc.DeletedYN = 0
inner join (select RowID, RefRowID from DPC WITH(NOLOCK) WHERE TableID = 'USI' AND Source = 7 AND Phrase = '1000-00') DPC
on DPC.RowID = udlSpendItem.ID and DPC.RefRowID = udlDeptIssueSpend.DepartmentID
WHERE DATEDIFF( MONTH, udlDeptIssueSpend.TranBeginDate, GETDATE()) BETWEEN 1 AND 12
AND udlDeptIssueSpend.ItemNo = ''
GROUP BY udlDeptIssueSpend.Org
");
            DataTable accountsTable = _WebAppDbAccess.GetDataTable(query);
            return accountsTable;

        }

        public static dynamic AWB_SharedServices_AllSpendAnalysis_GroupServiceSpendSum()
        {


            string query = string.Format(@"SELECT
CASE WHEN SUM(TotalAmt) IS NULL THEN '$0.00'
WHEN SUM(TotalAmt) < 0 THEN '($' + CONVERT(varchar(50), CAST(ABS(SUM(TotalAmt)) as MONEY), -1) +')'
ELSE '$' + CONVERT(varchar(50), CAST(ABS(SUM(TotalAmt)) as MONEY), -1)
END Amount
FROM udlVendorLocSpendDetail HistoricalSpend WITH(NOLOCK)
LEFT OUTER JOIN udlVendorLoc WITH(NOLOCK)
ON udlVendorLoc.VendorLoc = HistoricalSpend.VendorLoc
AND udlVendorLoc.VendorSet = HistoricalSpend.VendorSet
AND udlVendorLoc.DeletedYN = 0
WHERE ISNULL(udlVendorLoc.ServicesOnlyYN,0) = 1
AND DATEDIFF( MONTH, HistoricalSpend.MonthSelectionDate, GETDATE()) BETWEEN 1 AND 12
AND udlVendorLoc.ID IN
( SELECT RowID FROM dsSAWBNonDeptExpenseDPC WITH(NOLOCK) WHERE TableID = 'UVE' AND Level = 1 AND Source = 44)");
            dynamic serviceValue = _WebAppDbAccess.GetDataValue<dynamic>(query, 90);

            return serviceValue;

        }

        public static DataTable AWB_SharedServices_AllSpendAnalysis_CategoryServiceSpendSum()
        {
            string query = string.Format(@"SELECT 
MAX(Phrase) as Category
, CASE WHEN SUM(TotalAmt) IS NULL THEN '$0.00' 
WHEN SUM(TotalAmt) < 0 THEN '($' + CONVERT(varchar(50), CAST(ABS(SUM(TotalAmt)) as MONEY), -1) +')' 
ELSE '$' + CONVERT(varchar(50), CAST(ABS(SUM(TotalAmt)) as MONEY), -1)
END Amount
FROM udlVendorLocSpendDetail HistoricalSpend WITH(NOLOCK)
LEFT OUTER JOIN udlVendorLoc WITH(NOLOCK) 
ON udlVendorLoc.VendorLoc = HistoricalSpend.VendorLoc 
AND udlVendorLoc.VendorSet = HistoricalSpend.VendorSet
AND udlVendorLoc.DeletedYN = 0
LEFT OUTER JOIN dsSAWBNonDeptExpenseDPC WITH(NOLOCK)
ON UdlVendorLoc.ID = dsSAWBNonDeptExpenseDPC.RowID
AND dsSAWBNonDeptExpenseDPC.TableID = 'UVE'
AND Level = 1 AND Source = 44
WHERE ISNULL(udlVendorLoc.ServicesOnlyYN,0) = 1
AND DATEDIFF( MONTH, HistoricalSpend.MonthSelectionDate, GETDATE()) BETWEEN 1 AND 12
GROUP BY dsSAWBNonDeptExpenseDPC.RefRowID
");
            DataTable accountsTable = _WebAppDbAccess.GetDataTable(query);
            return accountsTable;

        }

        public static DataTable AWB_SharedServices_AllSpendAnalysis_VendorServiceSpendSum()
        {
            string query = string.Format(@"SELECT
VendorLocName
, CASE WHEN SUM(TotalAmt) IS NULL THEN '$0.00'
WHEN SUM(TotalAmt) < 0THEN '($' + CONVERT(varchar(50), CAST(ABS(SUM(TotalAmt)) as MONEY), -1) +')'
ELSE '$' + CONVERT(varchar(50), CAST(ABS(SUM(TotalAmt)) as MONEY), -1)
END Amount
FROM udlVendorLocSpendDetail HistoricalSpend WITH(NOLOCK)
LEFT OUTER JOIN udlVendorLoc WITH(NOLOCK)
ON udlVendorLoc.VendorLoc = HistoricalSpend.VendorLoc
AND udlVendorLoc.VendorSet = HistoricalSpend.VendorSet
AND udlVendorLoc.DeletedYN = 0
WHERE ISNULL(udlVendorLoc.ServicesOnlyYN,0) = 1
AND DATEDIFF( MONTH, HistoricalSpend.MonthSelectionDate, GETDATE()) BETWEEN 1 AND 12
AND udlVendorLoc.ID IN ( SELECT RowID FROM dsSAWBNonDeptExpenseDPC WITH(NOLOCK) WHERE TableID = 'UVE' AND Level = 1 AND Source = 45)
GROUP BY VendorLocName
");
            DataTable accountsTable = _WebAppDbAccess.GetDataTable(query);
            return accountsTable;

        }

        public static DataTable AWB_SharedServices_AllSpendAnalysis_OrgServiceSpendSum()
        {
            string query = string.Format(@"SELECT
HistoricalSpend.Org
, CASE WHEN SUM(TotalAmt) IS NULL THEN '$0.00'
WHEN SUM(TotalAmt) < 0THEN '($' + CONVERT(varchar(50), CAST(ABS(SUM(TotalAmt)) as MONEY), -1) +')'
ELSE '$' + CONVERT(varchar(50), CAST(ABS(SUM(TotalAmt)) as MONEY), -1)
END Amount
FROM udlVendorLocSpendDetail HistoricalSpend with(nolock)
INNER JOIN udlVendorLoc with(nolock)
ON udlVendorLoc.VendorLoc = HistoricalSpend.VendorLoc
AND udlVendorLoc.VendorSet = HistoricalSpend.VendorSet
WHERE udlVendorLoc.DeletedYN = 0
AND udlVendorLoc.EntityXID <> udlVendorLoc.VendorLoc
AND udlVendorLoc.ServicesOnlyYN = 1
AND DATEDIFF( MONTH, MonthSelectionDate, GETDATE()) BETWEEN 1 AND 12
GROUP BY HistoricalSpend.Org
");
            DataTable accountsTable = _WebAppDbAccess.GetDataTable(query);
            return accountsTable;

        }

        public static DataTable AWB_SharedServices_AllSpendAnalysis_VendorServiceSpendSum_SCM4431()
        {
            string query = string.Format(@"SELECT
EntityAttributes.EntityName,
CASE WHEN SUM(TotalAmt) IS NULL THEN '$0.00'
WHEN SUM(TotalAmt) < 0THEN '($' + CONVERT(varchar(50), CAST(ABS(SUM(TotalAmt)) as MONEY), -1) +')'
ELSE '$' + CONVERT(varchar(50), CAST(ABS(SUM(TotalAmt)) as MONEY), -1)
END Amount

FROM udlVendorLocSpendDetail HistoricalSpend with(nolock)
INNER JOIN udlVendorLoc with(nolock)
ON udlVendorLoc.VendorLoc = HistoricalSpend.VendorLoc
AND udlVendorLoc.VendorSet = HistoricalSpend.VendorSet
INNER JOIN EntityAttributes EntityAttributes with(nolock)
ON EntityAttributes.GUID = udlVendorLoc.EntityXID
WHERE
udlVendorLoc.DeletedYN = 0
AND ISNULL(udlVendorLoc.ServicesOnlyYN,0) = 1
ANd udlVendorLoc.EntityXID <> udlVendorLoc.VendorLoc
AND DATEDIFF( MONTH, HistoricalSpend.MonthSelectionDate, GETDATE()) BETWEEN 1 AND 12
AND udlVendorLoc.ID IN ( SELECT RowID FROM dsSAWBNonDeptExpenseDPC WITH(NOLOCK) WHERE TableID = 'UVE' AND Source = 45)
GROUP BY EntityAttributes.EntityName
");
            DataTable accountsTable = _WebAppDbAccess.GetDataTable(query);
            return accountsTable;

        }

        public static DataTable AWB_SharedServices_AllSpendAnalysis_ContractServiceSpendSum()
        {
            string query = string.Format(@"SELECT
ContractNo
, CASE WHEN SUM(TotalAmt) IS NULL THEN '$0.00'
WHEN SUM(TotalAmt) < 0THEN '($' + CONVERT(varchar(50), CAST(ABS(SUM(TotalAmt)) as MONEY), -1) +')'
ELSE '$' + CONVERT(varchar(50), CAST(ABS(SUM(TotalAmt)) as MONEY), -1)
END Amount
FROM udlVendorLocSpendDetail HistoricalSpend WITH (NOLOCK)
INNER JOIN udlVendorLoc WITH (NOLOCK)
ON udlVendorLoc.VendorLoc = HistoricalSpend.VendorLoc
AND udlVendorLoc.VendorSet = HistoricalSpend.VendorSet
INNER JOIN dsSAWBNonDeptExpenseDPC DPC1 WITH (NOLOCK)
ON udlVendorLoc.ID = DPC1.RowID
AND DPC1.TableID = 'UVE'
AND DPC1.Source IN (46)
INNER JOIN EntityAttributes EntityAttributes WITH (NOLOCK)
ON EntityAttributes.GUID = udlVendorLoc.EntityXID
INNER JOIN ServiceContract WITH (NOLOCK)
ON ServiceContract.ID = DPC1.RefRowID
WHERE udlVendorLoc.DeletedYN = 0
AND udlVendorLoc.ServicesOnlyYN = 1
AND udlVendorLoc.EntityXID <> udlVendorLoc.VendorLoc
AND DATEDIFF( MONTH, HistoricalSpend.MonthSelectionDate, GETDATE()) BETWEEN 1 AND 12
GROUP BY ContractNo
");
            DataTable accountsTable = _WebAppDbAccess.GetDataTable(query);
            return accountsTable;

        }

        public static DataTable AWB_SharedServices_AllSpendAnalysis_RegionOrgServiceSpendSum()
        {
            string query = string.Format(@"SELECT
Org
, CASE WHEN SUM(TotalAmt) IS NULL THEN '$0.00'
WHEN SUM(TotalAmt) < 0THEN '($' + CONVERT(varchar(50), CAST(ABS(SUM(TotalAmt)) as MONEY), -1) +')'
ELSE '$' + CONVERT(varchar(50), CAST(ABS(SUM(TotalAmt)) as MONEY), -1)
END Amount
FROM udlVendorLocSpendDetail HistoricalSpend with(nolock)
INNER JOIN udlVendorLoc with(nolock)
ON udlVendorLoc.VendorLoc = HistoricalSpend.VendorLoc
AND udlVendorLoc.VendorSet = HistoricalSpend.VendorSet
WHERE udlVendorLoc.DeletedYN = 0
AND udlVendorLoc.EntityXID <> udlVendorLoc.VendorLoc
AND udlVendorLoc.ServicesOnlyYN = 1
AND DATEDIFF( MONTH, HistoricalSpend.MonthSelectionDate, GETDATE()) BETWEEN 1 AND 12
GROUP BY Org
");
            DataTable accountsTable = _WebAppDbAccess.GetDataTable(query);
            return accountsTable;

        }

        public static DataTable AWB_SharedServices_AllSpendAnalysis_DeptServiceSpendSum()
        {
            string query = string.Format(@"SELECT
Department.Dept
, CASE WHEN SUM(TotalAmt) IS NULL THEN '$0.00'
WHEN SUM(TotalAmt) < 0THEN '($' + CONVERT(varchar(50), CAST(ABS(SUM(TotalAmt)) as MONEY), -1) +')'
ELSE '$' + CONVERT(varchar(50), CAST(ABS(SUM(TotalAmt)) as MONEY), -1)
END Amount
FROM Department with(nolock)
INNER JOIN udlVendorLocSpendDetail HistoricalSpend with(nolock)
ON Department.Org = HistoricalSpend.Org
AND Department.Dept = HistoricalSpend.Dept
INNER JOIN udlVendorLoc with(nolock)
ON udlVendorLoc.VendorLoc = HistoricalSpend.VendorLoc
AND udlVendorLoc.VendorSet = HistoricalSpend.VendorSet
INNER JOIN Organization with(nolock)
ON Organization.Org = Department.Org
INNER JOIN dsSAWBDeptDPC DPC1 with(nolock)
ON udlVendorLoc.ID = DPC1.RowID
AND Department.ID = DPC1.RefRowID
AND DPC1.TableID = 'UVE'
AND DPC1.Source in (47)
WHERE udlVendorLoc.DeletedYN = 0
And udlVendorLoc.ServicesOnlyYN = 1
and HistoricalSpend.Dept <> ''
And udlVendorLoc.EntityXID <> udlVendorLoc.VendorLoc
AND DATEDIFF( MONTH, HistoricalSpend.MonthSelectionDate, GETDATE()) BETWEEN 1 AND 12
GROUP BY Department.Dept
");
            DataTable accountsTable = _WebAppDbAccess.GetDataTable(query);
            return accountsTable;

        }

        public static DataTable AWB_SharedServices_AllSpendAnalysis_ExpenseCodeServiceSpendSum()
        {
            string query = string.Format(@"SELECT
HistoricalSpend.ExpenseCode
, HistoricalSpend.Org
, CASE WHEN SUM(TotalAmt) IS NULL THEN '$0.00'
WHEN SUM(TotalAmt) < 0THEN '($' + CONVERT(varchar(50), CAST(ABS(SUM(TotalAmt)) as MONEY), -1) +')'
ELSE '$' + CONVERT(varchar(50), CAST(ABS(SUM(TotalAmt)) as MONEY), -1)
END Amount
FROM Expense with(nolock)
INNER JOIN udlVendorLocSpendDetail HistoricalSpend with(nolock)
ON Expense.Org = HistoricalSpend.Org
AND Expense.ExpenseCode = HistoricalSpend.ExpenseCode
INNER JOIN udlVendorLoc with(nolock)
ON udlVendorLoc.VendorLoc = HistoricalSpend.VendorLoc
AND udlVendorLoc.VendorSet = HistoricalSpend.VendorSet
INNER JOIN Organization with(nolock)
ON Organization.Org = Expense.Org
INNER JOIN dsSAWBExpenseCodeDPC DPC1 with(nolock)
ON udlVendorLoc.ID = DPC1.RowID
AND Expense.ID = DPC1.RefRowID
AND DPC1.TableID = 'UVE'
AND DPC1.Source in (48)
WHERE udlVendorLoc.DeletedYN = 0
AND udlVendorLoc.ServicesOnlyYN = 1
AND HistoricalSpend.ExpenseCode <> ''
AND udlVendorLoc.EntityXID <> udlVendorLoc.VendorLoc
AND DATEDIFF( MONTH, HistoricalSpend.MonthSelectionDate, GETDATE()) BETWEEN 1 AND 12
GROUP BY HistoricalSpend.ExpenseCode, HistoricalSpend.Org
");
            DataTable accountsTable = _WebAppDbAccess.GetDataTable(query);
            return accountsTable;

        }

        public static DataRow User_Note()
        {


            string query = string.Format(@"SELECT TOP 1 TableID, RowID, CreateUser FROM attachment WHERE atttype = 'UN'");

            DataRow FirstRow = _WebAppDbAccess.GetDataRow(query);

            return FirstRow;

        }

        public static DataRow Set_As_Replaced()
        {


            string query = string.Format(@"SELECT top 1 ID, ReplacedByContractNo, ContractName, ContractNo
FROM dbo.Contract
WHERE ReplacedByContractNo IS NULL");

            DataRow FirstRow = _WebAppDbAccess.GetDataRow(query, 90);

            return FirstRow;

        }

        public static DataRow System_Notes()
        {


            string query = string.Format(@"Select top 1 * from dbo.Attachment with(nolock) where AttType = 'SN' and TableID = 'CON'");

            DataRow FirstRow = _WebAppDbAccess.GetDataRow(query);

            return FirstRow;

        }

        #endregion methods
    }




}
