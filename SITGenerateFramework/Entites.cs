using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Forms;
using System.IO;

namespace SITGenerateFramework
{
    public class Entites
    {
        public void generateEntities(string constr, string outputDir, string namesp)
        {

            //try
            //{
            //    Directory.CreateDirectory(outputDir + "\\Entities\\");
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

                classStr += "using System;\n";
                classStr += "using System.Collections.Generic;\n";
                classStr += "using System.Linq;\n";
                classStr += "using System.Text;\n";
                //classStr += "using System.Text.RegularExpressions;\n";
                classStr += "using System.ComponentModel.DataAnnotations;\n\n";

                classStr += "namespace " + namesp + ".Entities\n";
                classStr += "{\n";
                classStr += "    public class " + dsTables.Tables[0].Rows[i]["Name"].ToString() + "\n";
                classStr += "    {\n";
                // classStr += "        private static Regex RegexEmail = new Regex(@\"^([\\w\\-\\.]+)@((\\[([0-9]{1,3}\\.){3}[0-9]{1,3}\\])|(([\\w\\-]+\\.)+)([a-zA-Z]{2,4}))$\", RegexOptions.Multiline | RegexOptions.IgnoreCase);";


                //===========================================================================================


                DataSet dsColumns = new DataSet();

                sql = "select * from information_schema.columns  where table_name = '" + dsTables.Tables[0].Rows[i]["Name"].ToString() + "'";
                m = cls.getData(sql, ref dsColumns);

                for (int j = 0; j < dsColumns.Tables[0].Rows.Count; j++)
                {

                    sql = @"SELECT i.name AS IndexName,
                         OBJECT_NAME(ic.OBJECT_ID) AS TableName,
                         COL_NAME(ic.OBJECT_ID,ic.column_id) AS ColumnName
                         FROM sys.indexes AS i
                         INNER JOIN sys.index_columns AS ic
                         ON i.OBJECT_ID = ic.OBJECT_ID
                         AND i.index_id = ic.index_id
                         WHERE i.is_primary_key = 1 and OBJECT_NAME(ic.OBJECT_ID) ='" + dsTables.Tables[0].Rows[i]["Name"].ToString() + "' and COL_NAME(ic.OBJECT_ID,ic.column_id)='" + dsColumns.Tables[0].Rows[j]["COLUMN_NAME"].ToString() + "'";

                    DataSet dsKey = new DataSet();
                    m = cls.getData(sql, ref dsKey);

                    if (dsKey.Tables[0].Rows.Count != 0)
                    {
                        classStr += "        [Key]\n";

                    }

                    string Data_Type = "";
                    if (dsColumns.Tables[0].Rows[j]["Data_Type"] != DBNull.Value)
                        Data_Type = dsColumns.Tables[0].Rows[j]["Data_Type"].ToString();

                    string dType = "";
                    switch (Data_Type)
                    {
                        case "int": dType = "int"; break;
                        case "datetime": dType = "DateTime"; break;
                        case "bit": dType = "bool"; break;
                        case "real": dType = "double"; break;
                        case "money": dType = "double"; break;
                        case "float": dType = "double"; break;
                        default: dType = "string"; break;
                    }
                    if (dsColumns.Tables[0].Rows[j]["IS_NULLABLE"].ToString() != "YES" && dsColumns.Tables[0].Rows[j]["COLUMN_NAME"].ToString() != "Id")
                    {
                        classStr += "        [Required]\n";
                    }

                    if (Data_Type == "nvarchar")
                    {
                        if (dsColumns.Tables[0].Rows[j]["CHARACTER_MAXIMUM_LENGTH"].ToString() == "-1")
                        {
                            classStr += "        [StringLength(4000)]\n";
                        }
                        else
                        {
                            classStr += "        [StringLength(" + dsColumns.Tables[0].Rows[j]["CHARACTER_MAXIMUM_LENGTH"].ToString() + ")]\n";
                        }
                    }
                    classStr += "        public " + dType + " " + dsColumns.Tables[0].Rows[j]["COLUMN_NAME"].ToString() + " { get; set; }\n";



                }

                classStr += "\n\n";

                sql = @"SELECT 
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
                    where FK.TABLE_NAME= '" + dsTables.Tables[0].Rows[i]["Name"].ToString() + "'  ORDER BY  1,2,3,4";

                DataSet dsFK = new DataSet();
                m = cls.getData(sql, ref dsFK);

                for (int j = 0; j < dsFK.Tables[0].Rows.Count; j++)
                {
                    if (dsFK.Tables[0].Rows[j]["FK_Column"].ToString() != "Created_By" && dsFK.Tables[0].Rows[j]["FK_Column"].ToString() != "Updated_By")
                    {
                        classStr += "        private " + dsFK.Tables[0].Rows[j]["PK_Table"].ToString() + " _" + dsFK.Tables[0].Rows[j]["Constraint_Name"].ToString().Substring(0, 1).ToLower() + dsFK.Tables[0].Rows[j]["Constraint_Name"].ToString().Substring(1, dsFK.Tables[0].Rows[j]["Constraint_Name"].ToString().Length - 1).ToLower() + ";\n";
                        classStr += "        [Association(\"" + dsFK.Tables[0].Rows[j]["Constraint_Name"].ToString() + "\",\"" + dsFK.Tables[0].Rows[j]["FK_Column"].ToString() + "\", \"" + dsFK.Tables[0].Rows[j]["PK_Column"].ToString() + "\", IsForeignKey = true)]\n";
                        classStr += "        public " + dsFK.Tables[0].Rows[j]["PK_Table"].ToString() + " " + dsFK.Tables[0].Rows[j]["Constraint_Name"].ToString() + "\n";
                        classStr += "        {\n";
                        classStr += "            get\n";
                        classStr += "            {\n";
                        classStr += "                return _" + dsFK.Tables[0].Rows[j]["Constraint_Name"].ToString().Substring(0, 1).ToLower() + dsFK.Tables[0].Rows[j]["Constraint_Name"].ToString().Substring(1, dsFK.Tables[0].Rows[j]["Constraint_Name"].ToString().Length - 1).ToLower() + ";\n";
                        classStr += "            }\n";
                        classStr += "            set\n";
                        classStr += "            {\n";
                        classStr += "                _" + dsFK.Tables[0].Rows[j]["Constraint_Name"].ToString().Substring(0, 1).ToLower() + dsFK.Tables[0].Rows[j]["Constraint_Name"].ToString().Substring(1, dsFK.Tables[0].Rows[j]["Constraint_Name"].ToString().Length - 1).ToLower() + "= value;\n";
                        classStr += "            }\n";
                        classStr += "        }\n";
                    }
                    else
                    {
                        classStr += "        private " + dsFK.Tables[0].Rows[j]["PK_Table"].ToString() + " _" + dsFK.Tables[0].Rows[j]["PK_Table"].ToString().Substring(0, 1).ToLower() + dsFK.Tables[0].Rows[j]["PK_Table"].ToString().Substring(1, dsFK.Tables[0].Rows[j]["PK_Table"].ToString().Length - 1).ToLower() + "_" + dsFK.Tables[0].Rows[j]["FK_Column"].ToString() + ";\n";
                        classStr += "        [Association(\"" + dsFK.Tables[0].Rows[j]["Constraint_Name"].ToString() + "\",\"" + dsFK.Tables[0].Rows[j]["FK_Column"].ToString() + "\", \"" + dsFK.Tables[0].Rows[j]["PK_Column"].ToString() + "\", IsForeignKey = true)]\n";
                        classStr += "        public " + dsFK.Tables[0].Rows[j]["PK_Table"].ToString() + " " + dsFK.Tables[0].Rows[j]["PK_Table"].ToString() + "_" + dsFK.Tables[0].Rows[j]["FK_Column"].ToString() + "\n";
                        classStr += "        {\n";
                        classStr += "            get\n";
                        classStr += "            {\n";
                        classStr += "                return _" + dsFK.Tables[0].Rows[j]["PK_Table"].ToString().Substring(0, 1).ToLower() + dsFK.Tables[0].Rows[j]["PK_Table"].ToString().Substring(1, dsFK.Tables[0].Rows[j]["PK_Table"].ToString().Length - 1).ToLower() + "_" + dsFK.Tables[0].Rows[j]["FK_Column"].ToString() + ";\n";
                        classStr += "            }\n";
                        classStr += "            set\n";
                        classStr += "            {\n";
                        classStr += "                _" + dsFK.Tables[0].Rows[j]["PK_Table"].ToString().Substring(0, 1).ToLower() + dsFK.Tables[0].Rows[j]["PK_Table"].ToString().Substring(1, dsFK.Tables[0].Rows[j]["PK_Table"].ToString().Length - 1).ToLower() + "_" + dsFK.Tables[0].Rows[j]["FK_Column"].ToString() + "= value;\n";
                        classStr += "            }\n";
                        classStr += "        }\n";
                    }

                }

                classStr += "    }\n";
                classStr += "}";

                //RichTextBox rich = new RichTextBox();
                //rich.Text = classStr;

                TextWriter tw = new StreamWriter(outputDir + "\\" + dsTables.Tables[0].Rows[i]["Name"].ToString() + "\\" + dsTables.Tables[0].Rows[i]["Name"].ToString() + ".cs");
                tw.WriteLine(classStr);
                tw.Close();


                //rich.SaveFile(outputDir + "\\Entities\\" + dsTables.Tables[0].Rows[i]["Name"].ToString()+  ".cs", RichTextBoxStreamType.PlainText);

            }




        }


        public void generatepartialEntities(string constr, string outputDir, string namesp)
        {

            //try
            //{
            //    Directory.CreateDirectory(outputDir + "\\Model\\");
            //}
            //catch { }
            DataAccess cls = new DataAccess();
            DataAccess.strConn = constr;
            DataSet dsTables = new DataSet();

            string sql = "select table_name as Name from INFORMATION_SCHEMA.Tables where TABLE_TYPE ='BASE TABLE' and table_name <> 'sysdiagrams'";
            string m = cls.getData(sql, ref dsTables);

            for (int i = 0; i < dsTables.Tables[0].Rows.Count; i++)
            {

                if (dsTables.Tables[0].Rows[i]["Name"].ToString() == "Contact")
                {
                    MessageBox.Show("hi");
                }

                string classStr = "";

                classStr += "using System;\n";
                classStr += "using System.Collections.Generic;\n";
                classStr += "using System.Linq;\n";
                classStr += "using System.Text;\n";
                classStr += "using System.Runtime.Serialization;\n";
                classStr += "using System.ComponentModel.DataAnnotations;\n\n";


                classStr += "namespace " + namesp + ".Entities\n";
                classStr += "{\n";
                classStr += "    public partial class " + dsTables.Tables[0].Rows[i]["Name"].ToString() + "\n";
                classStr += "    {\n";
                // classStr += "        private static Regex RegexEmail = new Regex(@\"^([\\w\\-\\.]+)@((\\[([0-9]{1,3}\\.){3}[0-9]{1,3}\\])|(([\\w\\-]+\\.)+)([a-zA-Z]{2,4}))$\", RegexOptions.Multiline | RegexOptions.IgnoreCase);";


                //===========================================================================================


                DataSet dsColumns = new DataSet();

                sql = "select * from information_schema.columns  where table_name = '" + dsTables.Tables[0].Rows[i]["Name"].ToString() + "'"; //and column_name like '%Name'";
                m = cls.getData(sql, ref dsColumns);

                //===================================================================================================================
                sql = @"SELECT 
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
                    where FK.TABLE_NAME= '" + dsTables.Tables[0].Rows[i]["Name"].ToString() + "'  ORDER BY  1,2,3,4";

                DataSet dsFK = new DataSet();
                m = cls.getData(sql, ref dsFK);

                for (int j = 0; j < dsFK.Tables[0].Rows.Count; j++)
                {
                    if (dsFK.Tables[0].Rows[j]["FK_Column"].ToString() != "Created_By" && dsFK.Tables[0].Rows[j]["FK_Column"].ToString() != "Updated_By")
                    {

                        classStr += "        #region " + dsFK.Tables[0].Rows[j]["FK_Column"].ToString().Replace("_Id", "_Name").Replace("_", "") + "\n";
                        classStr += "        partial void On" + dsFK.Tables[0].Rows[j]["FK_Column"].ToString().Replace("_Id", "_Name").Replace("_", "") + "Changing(string value);\n";
                        classStr += "        partial void On" + dsFK.Tables[0].Rows[j]["FK_Column"].ToString().Replace("_Id", "_Name").Replace("_", "") + "Changed();\n";

                      
                        classStr += "        private string  _" + dsFK.Tables[0].Rows[j]["FK_Column"].ToString().Replace("_Id", "_Name").Replace("_", "") + ";\n";
                        classStr += "        [DataMember()]\n";
                        classStr += "        public string " + dsFK.Tables[0].Rows[j]["FK_Column"].ToString().Replace("_Id", "_Name").Replace("_", "") + "\n";
                        classStr += "        {\n";
                        classStr += "            get\n";
                        classStr += "            {\n";

                        classStr += "                if (_" + dsFK.Tables[0].Rows[j]["FK_Column"].ToString().Replace("_Id", "_Name").Replace("_", "") + " == null)\n";
                        classStr += "                {\n";
                        classStr += "                    try\n";
                        classStr += "                    {\n";
                        classStr += "                        _" + dsFK.Tables[0].Rows[j]["FK_Column"].ToString().Replace("_Id", "_Name").Replace("_", "") + " = " + dsFK.Tables[0].Rows[j]["Constraint_Name"].ToString() + ".Name;\n";
                        classStr += "                    }\n";
                        classStr += "                    catch { }\n";
                        classStr += "                }\n";

                        classStr += "                return  _" + dsFK.Tables[0].Rows[j]["FK_Column"].ToString().Replace("_Id", "_Name").Replace("_", "") + ";\n";
                        classStr += "            }\n";

                        classStr += "            set\n";
                        classStr += "            {\n";
                        classStr += "                if ((this._" + dsFK.Tables[0].Rows[j]["FK_Column"].ToString().Replace("_Id", "_Name").Replace("_", "") + " != value))\n";
                        classStr += "                {\n";
                        classStr += "                    this.On" + dsFK.Tables[0].Rows[j]["FK_Column"].ToString().Replace("_Id", "_Name").Replace("_", "") + "Changing(value);\n";
                        classStr += "                    this.RaiseDataMemberChanging(\"" + dsFK.Tables[0].Rows[j]["FK_Column"].ToString().Replace("_Id", "_Name").Replace("_", "") + "\");\n";
                        classStr += "                    this.ValidateProperty(\"" + dsFK.Tables[0].Rows[j]["FK_Column"].ToString().Replace("_Id", "_Name").Replace("_", "") + "\", value);\n";
                        classStr += "                    this._" + dsFK.Tables[0].Rows[j]["FK_Column"].ToString().Replace("_Id", "_Name").Replace("_", "") + " = value;\n";
                        classStr += "                    this.RaiseDataMemberChanged(\"" + dsFK.Tables[0].Rows[j]["FK_Column"].ToString().Replace("_Id", "_Name").Replace("_", "") + "\");\n";
                        classStr += "                    this.On" + dsFK.Tables[0].Rows[j]["FK_Column"].ToString().Replace("_Id", "_Name").Replace("_", "") + "Changed();\n";
                        classStr += "                }\n";
                        classStr += "            }\n";
                     

                        classStr += "        }\n";
                        classStr += "        #endregion\n\n";
                    }
                    else
                    {
                        classStr += "        #region " + dsFK.Tables[0].Rows[j]["FK_Column"].ToString().Replace("_Id", "_Name").Replace("_", "") + "\n";
                        classStr += "        partial void On" + dsFK.Tables[0].Rows[j]["FK_Column"].ToString().Replace("_Id", "_Name").Replace("_", "") + "Changing(string value);\n";
                        classStr += "        partial void On" + dsFK.Tables[0].Rows[j]["FK_Column"].ToString().Replace("_Id", "_Name").Replace("_", "") + "Changed();\n";

                      

                        classStr += "        private string  _" + dsFK.Tables[0].Rows[j]["FK_Column"].ToString().Replace("_Id", "_Name").Replace("_", "") + ";\n";
                        classStr += "        [DataMember()]\n";
                        classStr += "        public string " + dsFK.Tables[0].Rows[j]["FK_Column"].ToString().Replace("_Id", "_Name").Replace("_", "") + "\n";
                        classStr += "        {\n";
                        classStr += "            get\n";
                        classStr += "            {\n";

                        classStr += "                if (_" + dsFK.Tables[0].Rows[j]["FK_Column"].ToString().Replace("_Id", "_Name").Replace("_", "") + " == null)\n";
                        classStr += "                {\n";
                        classStr += "                    try\n";
                        classStr += "                    {\n";
                        classStr += "                        _" + dsFK.Tables[0].Rows[j]["FK_Column"].ToString().Replace("_Id", "_Name").Replace("_", "") + " = " + dsFK.Tables[0].Rows[j]["PK_Table"].ToString() + "_" + dsFK.Tables[0].Rows[j]["FK_Column"].ToString() + ".Name;\n";
                        classStr += "                    }\n";
                        classStr += "                    catch { }\n";
                        classStr += "                }\n";

                        classStr += "                return  _" + dsFK.Tables[0].Rows[j]["FK_Column"].ToString().Replace("_Id", "_Name").Replace("_", "") + ";\n";
                        classStr += "            }\n";

                        classStr += "            set\n";
                        classStr += "            {\n";
                        classStr += "                if ((this._" + dsFK.Tables[0].Rows[j]["FK_Column"].ToString().Replace("_Id", "_Name").Replace("_", "") + " != value))\n";
                        classStr += "                {\n";
                        classStr += "                    this.On" + dsFK.Tables[0].Rows[j]["FK_Column"].ToString().Replace("_Id", "_Name").Replace("_", "") + "Changing(value);\n";
                        classStr += "                    this.RaiseDataMemberChanging(\"" + dsFK.Tables[0].Rows[j]["FK_Column"].ToString().Replace("_Id", "_Name").Replace("_", "") + "\");\n";
                        classStr += "                    this.ValidateProperty(\"" + dsFK.Tables[0].Rows[j]["FK_Column"].ToString().Replace("_Id", "_Name").Replace("_", "") + "\", value);\n";
                        classStr += "                    this._" + dsFK.Tables[0].Rows[j]["FK_Column"].ToString().Replace("_Id", "_Name").Replace("_", "") + " = value;\n";
                        classStr += "                    this.RaiseDataMemberChanged(\"" + dsFK.Tables[0].Rows[j]["FK_Column"].ToString().Replace("_Id", "_Name").Replace("_", "") + "\");\n";
                        classStr += "                    this.On" + dsFK.Tables[0].Rows[j]["FK_Column"].ToString().Replace("_Id", "_Name").Replace("_", "") + "Changed();\n";
                        classStr += "                }\n";
                        classStr += "            }\n";

                        classStr += "        }\n";
                        classStr += "        #endregion\n\n";
                    }
                    

                }
                //======================================================================================================================

                classStr += "        public override string ToString()\n";
                classStr += "        {\n";
                classStr += "            return Name;\n";
                classStr += "        }\n\n";
                classStr += "\n\n";
              
               

                classStr += "    }\n";
                classStr += "}";

                //RichTextBox rich = new RichTextBox();
                //rich.Text = classStr;

                if (dsColumns.Tables[0].Rows.Count != 0)
                {
                    TextWriter tw = new StreamWriter(outputDir + "\\" + dsTables.Tables[0].Rows[i]["Name"].ToString() + "\\" + dsTables.Tables[0].Rows[i]["Name"].ToString() + "Model.cs");
                    tw.WriteLine(classStr);
                    tw.Close();
                }


                //rich.SaveFile(outputDir + "\\Entities\\" + dsTables.Tables[0].Rows[i]["Name"].ToString()+  ".cs", RichTextBoxStreamType.PlainText);

            }




        }
    }
}
