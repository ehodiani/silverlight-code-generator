using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;

namespace SITGenerateFramework
{
    public class Repository
    {
        public void generateRepositories(string constr, string outputDir, string namesp)
        {
            //try
            //{
            //    Directory.CreateDirectory(outputDir + "\\Repository\\");
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

                classStr += getUsingSection(namesp);
                classStr += getHeader(namesp, dsTables.Tables[0].Rows[i]["Name"].ToString() + "_Repository");

                sql = "select * from information_schema.columns  where table_name = '" + dsTables.Tables[0].Rows[i]["Name"].ToString() + "'";
                DataSet ds = new DataSet();
                m = cls.getData(sql, ref ds);

                classStr += getMethods(ds, dsTables.Tables[0].Rows[i]["Name"].ToString());
                classStr += "\n     }\n}";

                TextWriter tw = new StreamWriter(outputDir + "\\" + dsTables.Tables[0].Rows[i]["Name"].ToString() + "\\" + dsTables.Tables[0].Rows[i]["Name"].ToString() + "_Repository" + ".cs");
                tw.WriteLine(classStr);
                tw.Close();
            }

        }

        public string getMethods(DataSet dsTableDefenation, string TableName)
        {
            string Methods = "        #region Methods\n{0}\n        #endregion";

            string str = getInserMethod(dsTableDefenation, TableName);
            str += "\n\n" + getUpdateMethod(dsTableDefenation, TableName);
            str += "\n\n" + getDeleteMethod(dsTableDefenation, TableName);
            str += "\n\n" + getFindByIdMethod(dsTableDefenation, TableName);
            str += "\n\n" + getFindAllMethod(dsTableDefenation, TableName);


            string sql = @"SELECT 
                        FK_Table  = FK.TABLE_NAME, 
                        FK_Column = CU.COLUMN_NAME, 
                        PK_Table  = PK.TABLE_NAME, 
                        PK_Column = PT.COLUMN_NAME, 
                        Constraint_Name = C.CONSTRAINT_NAME 
                    FROM 
                        INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS C 
                        INNER JOIN 
                        INFORMATION_SCHEMA.TABLE_CONSTRAINTS FK 
                            ON C.CONSTRAINT_NAME = FK.CONSTRAINT_NAME 
                        INNER JOIN 
                        INFORMATION_SCHEMA.TABLE_CONSTRAINTS PK 
                            ON C.UNIQUE_CONSTRAINT_NAME = PK.CONSTRAINT_NAME 
                        INNER JOIN 
                        INFORMATION_SCHEMA.KEY_COLUMN_USAGE CU 
                            ON C.CONSTRAINT_NAME = CU.CONSTRAINT_NAME 
                        INNER JOIN 
                        ( 
                            SELECT 
                                i1.TABLE_NAME, i2.COLUMN_NAME 
                            FROM 
                                INFORMATION_SCHEMA.TABLE_CONSTRAINTS i1 
                                INNER JOIN 
                                INFORMATION_SCHEMA.KEY_COLUMN_USAGE i2 
                                ON i1.CONSTRAINT_NAME = i2.CONSTRAINT_NAME 
                                WHERE i1.CONSTRAINT_TYPE = 'PRIMARY KEY' 
                        ) PT 
                        ON PT.TABLE_NAME = PK.TABLE_NAME 
                    -- optional: 
                    where FK.TABLE_NAME= '" + TableName + "'  ORDER BY  1,2,3,4";

            DataSet dsFK = new DataSet();
            DataAccess cls = new DataAccess();
            string m = cls.getData(sql, ref dsFK);
            for (int j = 0; j < dsFK.Tables[0].Rows.Count; j++)
            {
                if (dsFK.Tables[0].Rows[j]["FK_Column"].ToString() != "Created_By" && dsFK.Tables[0].Rows[j]["FK_Column"].ToString() != "Updated_By")
                {
                    str += "\n\n" + getFindByMethod(dsTableDefenation, TableName, dsFK.Tables[0].Rows[j]["FK_Column"].ToString());
                }
            }


            Methods = System.String.Format(Methods, str);
            return Methods;
        }

        public string getUsingSection(string namesp)
        {
            string usingSection = @"using System;";
            usingSection += "\nusing System.Collections.Generic;";
            usingSection += "\nusing System.Linq;";
            usingSection += "\nusing System.Text;";
            usingSection += "\nusing System.Data;";
            usingSection += "\nusing System.Data.SqlClient;";
            usingSection += "\nusing " + namesp + ".Entities;";
            return usingSection;
        }

        public string getHeader(string namespaces, string classname)
        {
            string str = "\n\n\nnamespace " + namespaces + ".Repository" + "\n{\n";
            str += "    public class " + classname + "\n";
            str += "    {\n";
            return str;
        }

        public string getInserMethod(DataSet dsTableDefenation, string TableName)
        {
            string Statment = "         public string Insert(ref " + TableName + " entity)\n";
            Statment += "         {\n";
            Statment += "             Data.DataAccess cls = new Data.DataAccess();\n";
            Statment += "             DataTable dataTable = new DataTable();\n\n";
            Statment += "             dataTable.Columns.Add(\"ParameterName\");\n";
            Statment += "             dataTable.Columns.Add(\"ParameterValue\");\n\n";

            string str = "";
            for (int i = 0; i < dsTableDefenation.Tables[0].Rows.Count; i++)
            {
                string Column_name = "";
                if (dsTableDefenation.Tables[0].Rows[i]["Column_name"] != DBNull.Value)
                    Column_name = dsTableDefenation.Tables[0].Rows[i]["Column_name"].ToString();

                string Data_Type = "";
                if (dsTableDefenation.Tables[0].Rows[i]["Data_Type"] != DBNull.Value)
                    Data_Type = dsTableDefenation.Tables[0].Rows[i]["Data_Type"].ToString();

                string dType = "";
                switch (Data_Type)
                {

                    case "int": dType = " != 0"; break;
                    case "datetime": dType = ".ToString() != \"1/1/0001 12:00:00 AM\""; break;
                    case "float": dType = " != 0.0"; break;
                    case "money": dType = " != 0.0"; break;
                    default: dType = " != null"; break;
                }

                if (i != 0)
                {

                    Statment += "            if (entity." + Column_name + dType + ")\n";
                    //Statment += "                 dataTable.Rows.Add(\"@" + Column_name + "\", DBNull.Value);\n";
                    //Statment += "            else\n";
                    Statment += "                 dataTable.Rows.Add(\"@" + Column_name + "\", entity." + Column_name + ");\n\n";
                }
            }
            Statment += "\n";

            Statment += "            int id = 0;\n";
            Statment += "            string m= cls.Update(\"sp_Add_" + TableName + "\", dataTable, \"@" + dsTableDefenation.Tables[0].Rows[0]["Column_name"].ToString() + "\", ref id);\n";
            Statment += "            entity." + dsTableDefenation.Tables[0].Rows[0]["Column_name"].ToString() + "= id;\n\n";
            Statment += "            return m;\n";
            Statment += "         }";


            return Statment;
        }

        public string getUpdateMethod(DataSet dsTableDefenation, string TableName)
        {
            string Statment = "         public string Update(ref " + TableName + " entity)\n";
            Statment += "         {\n";
            Statment += "             Data.DataAccess cls = new Data.DataAccess();\n";
            Statment += "             DataTable dataTable = new DataTable();\n\n";
            Statment += "             dataTable.Columns.Add(\"ParameterName\");\n";
            Statment += "             dataTable.Columns.Add(\"ParameterValue\");\n\n";

            string str = "";
            for (int i = 0; i < dsTableDefenation.Tables[0].Rows.Count; i++)
            {
                string Column_name = "";
                if (dsTableDefenation.Tables[0].Rows[i]["Column_name"] != DBNull.Value)
                    Column_name = dsTableDefenation.Tables[0].Rows[i]["Column_name"].ToString();

                string Data_Type = "";
                if (dsTableDefenation.Tables[0].Rows[i]["Data_Type"] != DBNull.Value)
                    Data_Type = dsTableDefenation.Tables[0].Rows[i]["Data_Type"].ToString();

                string dType = "";
                switch (Data_Type)
                {

                    case "int": dType = " != 0"; break;
                    case "datetime": dType = ".ToString() != \"1/1/0001 12:00:00 AM\""; break;
                    case "float": dType = " != 0.0"; break;
                    case "money": dType = " != 0.0"; break;
                    default: dType = " != null"; break;
                }

                if (Column_name != "Date_Created" && Column_name != "Created_By" && Column_name != "Is_Deleted")
                {
                    Statment += "            if (entity." + Column_name + dType + ")\n";
                    //Statment += "                 dataTable.Rows.Add(\"@" + Column_name + "\", DBNull.Value);\n";
                    //Statment += "            else\n";
                    Statment += "                 dataTable.Rows.Add(\"@" + Column_name + "\", entity." + Column_name + ");\n\n";
                }

            }


            Statment += "\n";
            Statment += "            int id = 0;\n";
            Statment += "            string m= cls.Update(\"sp_Update_" + TableName + "\", dataTable, \"\", ref id);\n";

            Statment += "            return m;\n";
            Statment += "         }";
            return Statment;
        }


        public string getDeleteMethod(DataSet dsTableDefenation, string TableName)
        {
            string Statment = "         public string Delete(ref " + TableName + " entity)\n";
            Statment += "         {\n";
            Statment += "             Data.DataAccess cls = new Data.DataAccess();\n";
            Statment += "             DataTable dataTable = new DataTable();\n\n";
            Statment += "             dataTable.Columns.Add(\"ParameterName\");\n";
            Statment += "             dataTable.Columns.Add(\"ParameterValue\");\n\n";

            string str = "";

            string Column_name = "";
            if (dsTableDefenation.Tables[0].Rows[0]["Column_name"] != DBNull.Value)
                Column_name = dsTableDefenation.Tables[0].Rows[0]["Column_name"].ToString();


            Statment += "             dataTable.Rows.Add(\"@" + Column_name + "\", entity." + Column_name + ");\n";



            Statment += "\n";
            Statment += "            int id = 0;\n";
            Statment += "            string m= cls.Update(\"sp_Delete_" + TableName + "\", dataTable, \"\", ref id);\n";

            Statment += "            return m;\n";
            Statment += "         }";
            return Statment;
        }

        public string getFindByIdMethod(DataSet dsTableDefenation, string TableName)
        {
            string Statment = "         public void FindById(ref " + TableName + " entity)\n";
            Statment += "         {\n";
            Statment += "             Data.DataAccess cls = new Data.DataAccess();\n";
            Statment += "             DataTable dataTable = new DataTable();\n\n";
            Statment += "             dataTable.Columns.Add(\"ParameterName\");\n";
            Statment += "             dataTable.Columns.Add(\"ParameterValue\");\n\n";

            string str = "";

            string Column_name = "";
            if (dsTableDefenation.Tables[0].Rows[0]["Column_name"] != DBNull.Value)
                Column_name = dsTableDefenation.Tables[0].Rows[0]["Column_name"].ToString();


            Statment += "             dataTable.Rows.Add(\"@" + Column_name + "\", entity." + Column_name + ");\n";



            Statment += "\n";
            Statment += "             DataSet ds = new DataSet();\n";
            Statment += "             string m = cls.FillDataSet(\"sp_GetById_" + TableName + "\", dataTable, ref ds);\n";

            Statment += "             if (ds.Tables[0].Rows.Count != 0)\n";
            Statment += "             {\n";
            for (int i = 0; i < dsTableDefenation.Tables[0].Rows.Count; i++)
            {

                string pColumn_name = "";
                if (dsTableDefenation.Tables[0].Rows[i]["Column_name"] != DBNull.Value)
                    pColumn_name = dsTableDefenation.Tables[0].Rows[i]["Column_name"].ToString();

                string Data_Type = "";
                if (dsTableDefenation.Tables[0].Rows[i]["Data_Type"] != DBNull.Value)
                    Data_Type = dsTableDefenation.Tables[0].Rows[i]["Data_Type"].ToString();



                string dType = "";
                switch (Data_Type)
                {

                    case "int": dType = "ToInt32"; break;
                    case "datetime": dType = "ToDateTime"; break;
                    case "bit": dType = "ToBoolean"; break;

                    case "real": dType = "ToDouble"; break;
                    case "money": dType = "ToDouble"; break;
                    case "float": dType = "ToDouble"; break;
                    default: dType = "ToString"; break;
                }

                Statment += "                 if (ds.Tables[0].Rows[0][\"" + pColumn_name + "\"] != DBNull.Value)\n";
                Statment += "                     entity." + pColumn_name + " = Convert." + dType + "(ds.Tables[0].Rows[0][\"" + pColumn_name + "\"]);\n\n";
                Statment += "";
            }
            Statment += "             }\n";

            Statment += "         }";
            return Statment;
        }

        public string getFindAllMethod(DataSet dsTableDefenation, string TableName)
        {
            string Statment = "        public  List<" + TableName + "> FindAll()\n";
            Statment += "        {\n";
            Statment += "            SqlConnection SQLConn = new SqlConnection(ConnectionHelper.GetConnectionString());\n";
            Statment += "            SqlCommand SQLcmd = new SqlCommand();\n";
            Statment += "            SQLcmd.Connection = SQLConn;\n";
            Statment += "            SQLcmd.CommandText = \"sp_FindAll_" + TableName + "\";\n";
            Statment += "            SQLcmd.CommandType = CommandType.StoredProcedure;\n";
            Statment += "            List<" + TableName + "> result = new List<" + TableName + ">();\n\n";
            Statment += "            SQLConn.Open();\n";
            Statment += "            SqlDataReader reader = SQLcmd.ExecuteReader(CommandBehavior.CloseConnection);\n";
            Statment += "            while (reader.Read())\n";
            Statment += "            {\n";
            Statment += "                " + TableName + " record = new " + TableName + "();\n\n";

            for (int i = 0; i < dsTableDefenation.Tables[0].Rows.Count; i++)
            {

                string pColumn_name = "";
                if (dsTableDefenation.Tables[0].Rows[i]["Column_name"] != DBNull.Value)
                    pColumn_name = dsTableDefenation.Tables[0].Rows[i]["Column_name"].ToString();

                string Data_Type = "";
                if (dsTableDefenation.Tables[0].Rows[i]["Data_Type"] != DBNull.Value)
                    Data_Type = dsTableDefenation.Tables[0].Rows[i]["Data_Type"].ToString();

                string dType = "";
                switch (Data_Type)
                {

                    case "int": dType = "ToInt32"; break;
                    case "datetime": dType = "ToDateTime"; break;
                    case "bit": dType = "ToBoolean"; break;

                    case "real": dType = "ToDouble"; break;
                    case "money": dType = "ToDouble"; break;
                    case "float": dType = "ToDouble"; break;
                    default: dType = "ToString"; break;
                }
                Statment += "                if (reader[\"" + pColumn_name + "\"] != DBNull.Value)\n";
                Statment += "                    record." + pColumn_name + " = Convert." + dType + "(reader[\"" + pColumn_name + "\"]);\n\n";
            }
            Statment += "                result.Add(record);\n";
            Statment += "            }\n";
            Statment += "            reader.Close();\n";
            Statment += "            SQLConn.Close();\n";
            Statment += "            return result;\n";
            Statment += "        }\n";
            Statment += "";

            return Statment;
        }

        public string getFindByMethod(DataSet dsTableDefenation, string TableName, string FK_Column)
        {
            string by =FK_Column.Replace("_Id","");
            string Statment = "        public  List<" + TableName + "> FindBy" + by + "(ref " + TableName + " entity)\n";
            Statment += "        {\n";
            Statment += "            SqlConnection SQLConn = new SqlConnection(ConnectionHelper.GetConnectionString());\n";
            Statment += "            SqlCommand SQLcmd = new SqlCommand();\n";
            Statment += "            SQLcmd.Connection = SQLConn;\n";
            Statment += "            SQLcmd.CommandText = \"sp_Find_" + TableName + "_" + FK_Column + "\";\n";
            Statment += "            SQLcmd.CommandType = CommandType.StoredProcedure;\n";
            Statment += "            SQLcmd.Parameters.Add(new SqlParameter(\"@" + FK_Column + "\", entity." + FK_Column + "));\n";
            Statment += "            List<" + TableName + "> result = new List<" + TableName + ">();\n\n";
            Statment += "            SQLConn.Open();\n";
            Statment += "            SqlDataReader reader = SQLcmd.ExecuteReader(CommandBehavior.CloseConnection);\n";
            Statment += "            while (reader.Read())\n";
            Statment += "            {\n";
            Statment += "                " + TableName + " record = new " + TableName + "();\n\n";

            for (int i = 0; i < dsTableDefenation.Tables[0].Rows.Count; i++)
            {

                string pColumn_name = "";
                if (dsTableDefenation.Tables[0].Rows[i]["Column_name"] != DBNull.Value)
                    pColumn_name = dsTableDefenation.Tables[0].Rows[i]["Column_name"].ToString();

                string Data_Type = "";
                if (dsTableDefenation.Tables[0].Rows[i]["Data_Type"] != DBNull.Value)
                    Data_Type = dsTableDefenation.Tables[0].Rows[i]["Data_Type"].ToString();

                string dType = "";
                switch (Data_Type)
                {

                    case "int": dType = "ToInt32"; break;
                    case "datetime": dType = "ToDateTime"; break;
                    case "bit": dType = "ToBoolean"; break;

                    case "real": dType = "ToDouble"; break;
                    case "money": dType = "ToDouble"; break;
                    case "float": dType = "ToDouble"; break;
                    default: dType = "ToString"; break;
                }
                Statment += "                if (reader[\"" + pColumn_name + "\"] != DBNull.Value)\n";
                Statment += "                    record." + pColumn_name + " = Convert." + dType + "(reader[\"" + pColumn_name + "\"]);\n\n";
            }
            Statment += "                result.Add(record);\n";
            Statment += "            }\n";
            Statment += "            reader.Close();\n";
            Statment += "            SQLConn.Close();\n";
            Statment += "            return result;\n";
            Statment += "        }\n";
            Statment += "";

            return Statment;
        }


    }
}
