using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;

namespace SITGenerateFramework
{
    public class StoredProcedures
    {
        public void generateStoredProcedures(string constr, string outputDir, string namesp)
        {
            //try
            //{
            //    Directory.CreateDirectory(outputDir + "\\StoredProcedures\\");
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


                sql = "select * from information_schema.columns  where table_name = '" + dsTables.Tables[0].Rows[i]["Name"].ToString() + "'";
                DataSet ds = new DataSet();
                m = cls.getData(sql, ref ds);

                classStr += getMethods(ds, dsTables.Tables[0].Rows[i]["Name"].ToString());
                classStr += "";

                TextWriter tw = new StreamWriter(outputDir + "\\" + dsTables.Tables[0].Rows[i]["Name"].ToString() + "\\" + dsTables.Tables[0].Rows[i]["Name"].ToString() + ".sql");
                tw.WriteLine(classStr);
                tw.Close();
            }
        }

        public string getMethods(DataSet dsTableDefenation, string TableName)
        {
            string Methods = "{0}";


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

            string str = "/****** Object:  StoredProcedure [dbo].[sp_Add_" + TableName + "]    Script Date: " + DateTime.Now + " ******/\n";
            str += "IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Add_" + TableName + "]') AND type in (N'P', N'PC'))\n";
            str += "DROP PROCEDURE [dbo].[sp_Add_" + TableName + "]\n";
            str += "Go\n\n";


            str += "/****** Object:  StoredProcedure [dbo].[sp_Update_" + TableName + "]    Script Date: " + DateTime.Now + " ******/\n";
            str += "IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Update_" + TableName + "]') AND type in (N'P', N'PC'))\n";
            str += "DROP PROCEDURE [dbo].[sp_Update_" + TableName + "]\n";
            str += "Go\n\n";


            str += "/****** Object:  StoredProcedure [dbo].[sp_Delete_" + TableName + "]    Script Date: " + DateTime.Now + " ******/\n";
            str += "IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Delete_" + TableName + "]') AND type in (N'P', N'PC'))\n";
            str += "DROP PROCEDURE [dbo].[sp_Delete_" + TableName + "]\n";
            str += "Go\n\n";


            str += "/****** Object:  StoredProcedure [dbo].[sp_GetById_" + TableName + "]    Script Date: " + DateTime.Now + " ******/\n";
            str += "IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_GetById_" + TableName + "]') AND type in (N'P', N'PC'))\n";
            str += "DROP PROCEDURE [dbo].[sp_GetById_" + TableName + "]\n";
            str += "Go\n\n";



            str += "/****** Object:  StoredProcedure [dbo].[sp_FindAll_" + TableName + "]    Script Date: " + DateTime.Now + " ******/\n";
            str += "IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_FindAll_" + TableName + "]') AND type in (N'P', N'PC'))\n";
            str += "DROP PROCEDURE [dbo].[sp_FindAll_" + TableName + "]\n";
            str += "Go\n\n";


            for (int j = 0; j < dsFK.Tables[0].Rows.Count; j++)
            {
                str += "/****** Object:  StoredProcedure [dbo].[sp_GetBy" + "_" + TableName + "_" + dsFK.Tables[0].Rows[j]["FK_Column"].ToString() + "]    Script Date: " + DateTime.Now + " ******/\n";
                str += "IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_GetBy" + "_" + TableName + "_" + dsFK.Tables[0].Rows[j]["FK_Column"].ToString() + "]') AND type in (N'P', N'PC'))\n";
                str += "DROP PROCEDURE [dbo].[sp_GetBy" + "_" + TableName + "_" + dsFK.Tables[0].Rows[j]["FK_Column"].ToString() + "]\n";
                str += "Go\n\n";
            }

            str += "\n\n--****** Object:  StoredProcedure [dbo].[sp_Add_" + TableName + "]    Script Date: " + DateTime.Now + " ******\n\n";
            str += InsertStatment(dsTableDefenation, TableName);
            str += "\n\n--****** Object:  StoredProcedure [dbo].[sp_Update_" + TableName + "]    Script Date: " + DateTime.Now + " ******\n\n";
            str += "\n\n" + UpdateStatment(dsTableDefenation, TableName);
            str += "\n\n--****** Object:  StoredProcedure [dbo].[sp_Delete_" + TableName + "]    Script Date: " + DateTime.Now + " ******\n\n";
            str += "\n\n" + DeleteStatment(dsTableDefenation, TableName);
            str += "\n\n--****** Object:  StoredProcedure [dbo].[sp_GetById_" + TableName + "]    Script Date: " + DateTime.Now + " ******\n\n";
            str += "\n\n" + GetStatmentById(dsTableDefenation, TableName);
            str += "\n\n--****** Object:  StoredProcedure [dbo].[sp_FindAll_" + TableName + "]    Script Date: " + DateTime.Now + " ******\n\n";
            str += "\n\n" + GetStatmenFindAll(dsTableDefenation, TableName);


            

                for (int j = 0; j < dsFK.Tables[0].Rows.Count; j++)
                {
                    if (dsFK.Tables[0].Rows[j]["FK_Column"].ToString() != "Created_By" && dsFK.Tables[0].Rows[j]["FK_Column"].ToString() != "Updated_By")
                    {
                        str += "\n\n--****** Object:  StoredProcedure [dbo].[sp_GetBy" + "_" + TableName + "_" + dsFK.Tables[0].Rows[j]["FK_Column"].ToString() + "]    Script Date: " + DateTime.Now + " ******\n\n";
                        str += "\n\n" + GetStatmentByFK_Column(TableName, dsFK.Tables[0].Rows[j]["FK_Column"].ToString());
                    }
                }




            Methods = System.String.Format(Methods, str);
            return Methods;
        }
        public string GetStatmentByFK_Column(string TableName, string FK_Column)
        {
            string Statment = "CREATE PROCEDURE [dbo].[sp_GetBy" + "_" + TableName + "_" + FK_Column + "]\n";
            Statment += "(\n   @" + FK_Column + "  int\n)\n\nAS\n\nSET NOCOUNT ON\n\n";
            Statment += "SELECT * FROM [dbo].[" + TableName + "] \n";


            Statment += "\n\nWhere Is_Deleted = 0 and " + FK_Column + " = @" + FK_Column;

            Statment += "\nGo";
            return Statment;
        }
        public string GetStatmenFindAll(DataSet dsTableDefenation, string TableName)
        {
            string Statment = "CREATE PROCEDURE [dbo].[sp_FindAll_" + TableName + "]\n";
            Statment += "\nAS\n\nSET NOCOUNT ON\n\n";
            Statment += "SELECT * FROM [dbo].[" + TableName + "] \n";


            Statment += "\n\nWhere Is_Deleted = 0 ";

            Statment += "\nGo";
            return Statment;
        }
        public string GetStatmentById(DataSet dsTableDefenation, string TableName)
        {
            string Statment = "CREATE PROCEDURE [dbo].[sp_GetById_" + TableName + "]\n";
            Statment += "(\n   @" + dsTableDefenation.Tables[0].Rows[0]["Column_name"].ToString() + "  int\n)\n\nAS\n\nSET NOCOUNT ON\n\n";
            Statment += "SELECT * FROM [dbo].[" + TableName + "] \n";


            Statment += "\n\nWhere Is_Deleted = 0 and " + dsTableDefenation.Tables[0].Rows[0]["Column_name"].ToString() + " = @" + dsTableDefenation.Tables[0].Rows[0]["Column_name"].ToString();

            Statment += "\nGo";
            return Statment;
        }
        public string DeleteStatment(DataSet dsTableDefenation, string TableName)
        {
            string Statment = "CREATE PROCEDURE [dbo].[sp_Delete_" + TableName + "]\n";
            Statment += "(\n   @" + dsTableDefenation.Tables[0].Rows[0]["Column_name"].ToString() + "  int\n)\n\nAS\n\n";
            Statment += "Update [dbo].[" + TableName + "] \n";
            Statment += "Set \n   Is_Deleted = 1";

            Statment += "\n\nWhere " + dsTableDefenation.Tables[0].Rows[0]["Column_name"].ToString() + " = @" + dsTableDefenation.Tables[0].Rows[0]["Column_name"].ToString();

            Statment += "\nGo";
            return Statment;
        }
        public string InsertStatment(DataSet dsTableDefenation, string TableName)
        {
            string Statment = "CREATE PROCEDURE [dbo].[sp_Add_" + TableName + "]\n";
            Statment += "(\n{0}\n)\n\nAS\n\n ";
            Statment += "INSERT INTO [dbo].[" + TableName + "] \n";
            Statment += "(\n{1}\n)";
            Statment += "\nValues\n(\n{2}\n)";
            Statment += "\nSET @" + dsTableDefenation.Tables[0].Rows[0]["Column_name"].ToString() + "=@@IDENTITY";
            string Parameters = "";
            string fields = "";
            string values = "";

            for (int i = 0; i < dsTableDefenation.Tables[0].Rows.Count; i++)
            {
                string Column_name = "";
                if (dsTableDefenation.Tables[0].Rows[i]["Column_name"] != DBNull.Value)
                    Column_name = dsTableDefenation.Tables[0].Rows[i]["Column_name"].ToString();

                string Data_Type = "";
                if (dsTableDefenation.Tables[0].Rows[i]["Data_Type"] != DBNull.Value)
                    Data_Type = dsTableDefenation.Tables[0].Rows[i]["Data_Type"].ToString();

                string Character_maximum_length = "";
                if (dsTableDefenation.Tables[0].Rows[i]["Character_maximum_length"] != DBNull.Value)
                    Character_maximum_length = dsTableDefenation.Tables[0].Rows[i]["Character_maximum_length"].ToString();

                if (i == 0)
                {
                    Parameters += "          @" + Column_name + " " + Data_Type + " output";

                }
                else
                {
                    Parameters += "         ,@" + Column_name + " " + Data_Type + " ";

                   

                    if (i == 1)
                    {
                        fields += "\n      " + Column_name;
                        values += "\n      @" + Column_name;

                    }
                    else
                    {
                        fields += "\n     ," + Column_name;
                        values += "\n     ,@" + Column_name;
                    }
                }

                if (Character_maximum_length != "")
                {
                    if (Character_maximum_length == "-1")
                        Parameters += "(MAX)";
                    else
                        Parameters += "(" + Character_maximum_length + ")";
                }
                if (dsTableDefenation.Tables[0].Rows[i]["IS_NULLABLE"].ToString().ToLower() == "yes")
                {
                    Parameters += " = null";
                }
                Parameters += "\n";

            }


            Statment = System.String.Format(Statment, Parameters, fields, values);
            Statment += "\nGo";
            return Statment;
        }

        public string UpdateStatment(DataSet dsTableDefenation, string TableName)
        {
            string Statment = "CREATE PROCEDURE [dbo].[sp_Update_" + TableName + "]\n";
            Statment += "(\n{0}\n)\n\nAS\n\n";
            Statment += "Update [dbo].[" + TableName + "] \n";
            Statment += "Set \n{1}";

            Statment += "\n\nWhere " + dsTableDefenation.Tables[0].Rows[0]["Column_name"].ToString() + " = @" + dsTableDefenation.Tables[0].Rows[0]["Column_name"].ToString();
            string Parameters = "";
            string fields = "";
            string values = "";

            for (int i = 0; i < dsTableDefenation.Tables[0].Rows.Count; i++)
            {
                string Column_name = "";
                if (dsTableDefenation.Tables[0].Rows[i]["Column_name"] != DBNull.Value)
                    Column_name = dsTableDefenation.Tables[0].Rows[i]["Column_name"].ToString();

                string Data_Type = "";
                if (dsTableDefenation.Tables[0].Rows[i]["Data_Type"] != DBNull.Value)
                    Data_Type = dsTableDefenation.Tables[0].Rows[i]["Data_Type"].ToString();

                string Character_maximum_length = "";
                if (dsTableDefenation.Tables[0].Rows[i]["Character_maximum_length"] != DBNull.Value)
                    Character_maximum_length = dsTableDefenation.Tables[0].Rows[i]["Character_maximum_length"].ToString();

                if (i == 0)
                {
                    if (Column_name != "Is_Deleted" && Column_name != "Date_Created" && Column_name != "Created_By")
                        Parameters += "          @" + Column_name + " " + Data_Type + " output";

                }
                else
                {
                    if (Column_name != "Is_Deleted" && Column_name != "Date_Created" && Column_name != "Created_By")
                        Parameters += "         ,@" + Column_name + " " + Data_Type + " ";
                    if (i == 1)
                    {
                        if (Column_name != "Is_Deleted" && Column_name != "Date_Created" && Column_name != "Created_By")
                            fields += "\n      " + Column_name + " =@" + Column_name;

                    }
                    else
                    {
                        if (Column_name != "Is_Deleted" && Column_name != "Date_Created" && Column_name != "Created_By")
                            fields += "\n     ," + Column_name + " =@" + Column_name;
                    }
                }

                if (Character_maximum_length != "")
                {
                    if (Character_maximum_length == "-1")
                        Parameters += "(MAX)";
                    else
                        Parameters += "(" + Character_maximum_length + ")";
                }

                if (Column_name != "Is_Deleted" && Column_name != "Date_Created" && Column_name != "Created_By")
                {
                    if (dsTableDefenation.Tables[0].Rows[i]["IS_NULLABLE"].ToString().ToLower() == "yes")
                    {
                        Parameters += " = null";
                    }
                    Parameters += "\n";
                   
                }

            }


            Statment = System.String.Format(Statment, Parameters, fields);

            Statment += "\nGo";
            return Statment;
        }
    }
}
