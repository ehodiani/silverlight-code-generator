using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;

namespace SITGenerateFramework
{
    public class Reports
    {
        public void generateReports(string constr, string outputDir, string namesp)
        {
            //try
            //{
            //    Directory.CreateDirectory(outputDir + "\\Reports\\");
            //}
            //catch { }


            DataAccess cls = new DataAccess();
            DataAccess.strConn = constr;
            DataSet dsTables = new DataSet();

            string sql = "select table_name as Name from INFORMATION_SCHEMA.Tables where TABLE_TYPE ='BASE TABLE' and table_name <> 'sysdiagrams'";
            string m = cls.getData(sql, ref dsTables);

            for (int i = 0; i < dsTables.Tables[0].Rows.Count; i++)
            {
                string classStr = "";
                classStr += getMainPage(namesp, dsTables.Tables[0].Rows[i]["Name"].ToString());
                TextWriter tw = new StreamWriter(outputDir + "\\" + dsTables.Tables[0].Rows[i]["Name"].ToString() + "\\" + dsTables.Tables[0].Rows[i]["Name"].ToString() + "Report.xaml");
                tw.WriteLine(classStr);
                tw.Close();
            }

            

        }

        private string getMainPage(string namesp, string tableName)
        {
            string str = "";

            DataAccess cls = new DataAccess();
            string sql = "select * from information_schema.columns  where table_name = '" + tableName + "'";
            DataSet dsColumns = new DataSet();
            string m = cls.getData(sql, ref dsColumns);

            //=================================================
            string adds = tableName.ElementAt(tableName.Length - 1) + "s";
            if (tableName.ElementAt(tableName.Length - 1) == 'y')
            {
                adds = "ies";
            }
            if (tableName.ElementAt(tableName.Length - 1) == 's')
            {
                adds = "s";
            }

            //========================================================

            str += "<sr:Report\n";
            str += "    xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" \n";
            str += "    xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\"\n";
            str += "    xmlns:sr=\"clr-namespace:SilverlightReport;assembly=SilverlightReport\">\n";
            
            
            str += "    <sr:ReportBand Kind=\"ReportHeader\">\n";
            str += "        <TextBlock FontSize=\"20\" FontWeight=\"Bold\" Margin=\"10\" HorizontalAlignment=\"Center\">\n";
            str += "                " + tableName.Replace("_"," ").Substring(0, tableName.Length - 1) + adds + " List</TextBlock>\n";
            str += "    </sr:ReportBand>\n";






            str += "    <sr:Report.DataGrid>\n";
            str += "        <sr:CDataGrid HeaderHorizontalAlignment=\"Center\">\n";

            for (int j = 0; j < dsColumns.Tables[0].Rows.Count; j++)
            {
                if (dsColumns.Tables[0].Rows[j]["COLUMN_NAME"].ToString().Contains("Is_Deleted") || dsColumns.Tables[0].Rows[j]["COLUMN_NAME"].ToString().Contains("Date_Created") || dsColumns.Tables[0].Rows[j]["COLUMN_NAME"].ToString().Contains("Date_Updated"))
                {
                    continue;
                }
                sql = @"SELECT        FK.TABLE_NAME AS FK_Table, CU.COLUMN_NAME AS FK_Column, PK.TABLE_NAME AS PK_Table, PT.COLUMN_NAME AS PK_Column, 
                         C.CONSTRAINT_NAME AS Constraint_Name
                            FROM            INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS AS C INNER JOIN
                         INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS FK ON C.CONSTRAINT_NAME = FK.CONSTRAINT_NAME INNER JOIN
                         INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS PK ON C.UNIQUE_CONSTRAINT_NAME = PK.CONSTRAINT_NAME INNER JOIN
                         INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS CU ON C.CONSTRAINT_NAME = CU.CONSTRAINT_NAME INNER JOIN
                             (SELECT        i1.TABLE_NAME, i2.COLUMN_NAME
                                FROM            INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS i1 INNER JOIN
                                                         INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS i2 ON i1.CONSTRAINT_NAME = i2.CONSTRAINT_NAME
                                WHERE        (i1.CONSTRAINT_TYPE = 'PRIMARY KEY')) AS PT ON PT.TABLE_NAME = PK.TABLE_NAME
                    WHERE        (FK.TABLE_NAME = '" + tableName + "') AND (CU.COLUMN_NAME = '" + dsColumns.Tables[0].Rows[j]["COLUMN_NAME"].ToString() + "')  ORDER BY FK_Table, FK_Column, PK_Table, PK_Column";

                DataSet dsFK = new DataSet();
                m = cls.getData(sql, ref dsFK);

                string Data_Type = "";
                if (dsColumns.Tables[0].Rows[j]["Data_Type"] != DBNull.Value)
                    Data_Type = dsColumns.Tables[0].Rows[j]["Data_Type"].ToString();

                string widthCol = "100";
                switch (Data_Type)
                {
                    case "int": widthCol = "0.5"; break;
                    case "datetime": widthCol = "1.3"; break;
                    case "bit": widthCol = "0.5"; break;
                    case "real": widthCol = "0.5"; break;
                    case "money": widthCol = "0.5"; break;
                    case "float": widthCol = "0.5"; break;
                    default: widthCol = "2.2"; break;
                }

                if (dsFK.Tables[0].Rows.Count == 0)
                {

                    str += "            <sr:CDataGridColumn Header=\"" + dsColumns.Tables[0].Rows[j]["COLUMN_NAME"].ToString().Replace("_", " ") + "\" Binding=\"{Binding " + dsColumns.Tables[0].Rows[j]["COLUMN_NAME"].ToString() + "}\" Width=\"" + widthCol + "\"></sr:CDataGridColumn>\n";
                }
                else
                {
                    if (dsFK.Tables[0].Rows[0]["FK_Column"].ToString() != "Created_By" && dsFK.Tables[0].Rows[0]["FK_Column"].ToString() != "Updated_By")
                    {

                        str += "            <sr:CDataGridColumn Header=\"" + dsFK.Tables[0].Rows[0]["FK_Column"].ToString().Replace("_", " ").Replace("Id", "") + "\" Binding=\"{Binding " + dsFK.Tables[0].Rows[0]["Constraint_Name"].ToString() + ".Name" + "}\" Width=\"" + widthCol + "\"></sr:CDataGridColumn>\n";
                    }
                    else
                    {
                        //str += "            <sr:CDataGridColumn Header=\"" + dsColumns.Tables[0].Rows[j]["COLUMN_NAME"].ToString().Replace("_", " ") + "\" Binding=\"{Binding " + dsFK.Tables[0].Rows[0]["PK_Table"].ToString().Replace("_", " ") + "_" + dsFK.Tables[0].Rows[0]["FK_Column"].ToString() + ".Name" + "}\" Width=\"" + widthCol + "\"></sr:CDataGridColumn>\n";
                    }
                }
            }

          
            str += "        </sr:CDataGrid>\n";
            str += "    </sr:Report.DataGrid>\n";
            str += "</sr:Report>\n";
        


            return str;
        }
    }
}
