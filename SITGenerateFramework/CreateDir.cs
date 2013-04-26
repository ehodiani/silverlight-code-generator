using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;

namespace SITGenerateFramework
{
    public class CreateDir
    {
        public void generateDir(string constr, string outputDir, string namesp)
        {
           


            DataAccess cls = new DataAccess();
            DataAccess.strConn = constr;
            DataSet dsTables = new DataSet();

            string sql = "select table_name as Name from INFORMATION_SCHEMA.Tables where TABLE_TYPE ='BASE TABLE' and table_name <> 'sysdiagrams'";
            string m = cls.getData(sql, ref dsTables);

            for (int i = 0; i < dsTables.Tables[0].Rows.Count; i++)
            {
                string classStr = "";

                try
                {
                    Directory.CreateDirectory(outputDir + "\\" + dsTables.Tables[0].Rows[i]["Name"].ToString() + "\\");
                }
                catch { }

               
            }
        }
    }
}
