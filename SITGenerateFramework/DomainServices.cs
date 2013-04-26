using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;

namespace SITGenerateFramework
{
    public class DomainServices
    {
        public void generateDomainServices(string constr, string outputDir, string namesp)
        {
            try
            {
                Directory.CreateDirectory(outputDir + "\\Services\\");
            }
            catch { }


            DataAccess cls = new DataAccess();
            DataAccess.strConn = constr;
            DataSet dsTables = new DataSet();

            string sql = "select table_name as Name from INFORMATION_SCHEMA.Tables where TABLE_TYPE ='BASE TABLE' and table_name <> 'sysdiagrams'";
            string m = cls.getData(sql, ref dsTables);


            string classStr = "";

            classStr += getHeader(namesp, "GloblDomainService");

            for (int i = 0; i < dsTables.Tables[0].Rows.Count; i++)
            {


                
                
                

                //sql = "select * from information_schema.columns  where table_name = '" + dsTables.Tables[0].Rows[i]["Name"].ToString() + "'";
                //DataSet ds = new DataSet();
                //m = cls.getData(sql, ref ds);

                classStr += "        #region " + dsTables.Tables[0].Rows[i]["Name"].ToString() + "\n";
                classStr += getMethods(dsTables.Tables[0].Rows[i]["Name"].ToString());

                classStr += "        #endregion\n\n";
                

               
            }

            classStr += "\n     }\n}";
            TextWriter tw = new StreamWriter(outputDir + "\\Services\\GloblDomainService" + ".cs");
            tw.WriteLine(classStr);
            tw.Close();

        }

        public string getMethods( string TableName)
        {
            string Methods = "{0}";

            string str = getInserMethod(TableName);
            str += "\n\n" + getUpdateMethod(TableName);
            str += "\n\n" + getDeleteMethod(TableName);
            str += "\n\n" + getMethod(TableName);
         
//            string sql = @"SELECT 
//                        FK_Table  = FK.TABLE_NAME, 
//                        FK_Column = CU.COLUMN_NAME, 
//                        PK_Table  = PK.TABLE_NAME, 
//                        PK_Column = PT.COLUMN_NAME, 
//                        Constraint_Name = C.CONSTRAINT_NAME 
//                    FROM 
//                        INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS C 
//                        INNER JOIN 
//                        INFORMATION_SCHEMA.TABLE_CONSTRAINTS FK 
//                            ON C.CONSTRAINT_NAME = FK.CONSTRAINT_NAME 
//                        INNER JOIN 
//                        INFORMATION_SCHEMA.TABLE_CONSTRAINTS PK 
//                            ON C.UNIQUE_CONSTRAINT_NAME = PK.CONSTRAINT_NAME 
//                        INNER JOIN 
//                        INFORMATION_SCHEMA.KEY_COLUMN_USAGE CU 
//                            ON C.CONSTRAINT_NAME = CU.CONSTRAINT_NAME 
//                        INNER JOIN 
//                        ( 
//                            SELECT 
//                                i1.TABLE_NAME, i2.COLUMN_NAME 
//                            FROM 
//                                INFORMATION_SCHEMA.TABLE_CONSTRAINTS i1 
//                                INNER JOIN 
//                                INFORMATION_SCHEMA.KEY_COLUMN_USAGE i2 
//                                ON i1.CONSTRAINT_NAME = i2.CONSTRAINT_NAME 
//                                WHERE i1.CONSTRAINT_TYPE = 'PRIMARY KEY' 
//                        ) PT 
//                        ON PT.TABLE_NAME = PK.TABLE_NAME 
//                    -- optional: 
//                    where FK.TABLE_NAME= '" + TableName + "'  ORDER BY  1,2,3,4";

//            DataSet dsFK = new DataSet();
//            DataAccess cls = new DataAccess();
//            string m = cls.getData(sql, ref dsFK);
//            for (int j = 0; j < dsFK.Tables[0].Rows.Count; j++)
//            {
//                if (dsFK.Tables[0].Rows[j]["FK_Column"].ToString() != "Created_By" && dsFK.Tables[0].Rows[j]["FK_Column"].ToString() != "Updated_By")
//                {
//                    str += "\n\n" + getFK_Method(TableName, dsFK.Tables[0].Rows[j]["FK_Column"].ToString());
//                }
//            }
            Methods = System.String.Format(Methods, str);
            return Methods;
        }

      

        public string getHeader(string namespaces, string classname)
        {
            string str = "namespace " + namespaces + ".Web.Services\n";
            str += "{\n";
            str += "    using System;\n";
            str += "    using System.Collections.Generic;\n";
            str += "    using System.ComponentModel;\n";
            str += "    using System.ComponentModel.DataAnnotations;\n";
            str += "    using System.Linq;\n";
            str += "    using System.ServiceModel.DomainServices.Hosting;\n";
            str += "    using System.ServiceModel.DomainServices.Server;\n\n";

            str += "    using "+ namespaces +".Entities;\n";
            str += "    using " + namespaces + ".Repository;\n\n";

            str += "    // TODO: Create methods containing your application logic.\n";
            str += "    [EnableClientAccess()]\n";
            str += "    public class GloblDomainService : DomainService\n";
            str += "    {\n\n";
          
            return str;
        }
        public string getInserMethod(string tableName)
        {
            string str = "        public void Insert" + tableName + "(" + tableName + " entity)\n";
            str += "        {\n";
            str += "            " + tableName + "_Repository entity_rep = new " + tableName + "_Repository();\n";
            str += "            string m = entity_rep.Insert(ref entity);\n";
            str += "        }\n";
            str += "\n";
           
            return str;
        }
        public string getUpdateMethod(string tableName)
        {
            string str = "        public void Update" + tableName + "(" + tableName + " entity)\n";
            str += "        {\n";
            str += "            " + tableName + "_Repository entity_rep = new " + tableName + "_Repository();\n";
            str += "            string m = entity_rep.Update(ref entity);\n";
            str += "        }\n";
            str += "\n";

            return str;
        }
        public string getDeleteMethod(string tableName)
        {
            string str = "        public void Delete" + tableName + "(" + tableName + " entity)\n";
            str += "        {\n";
            str += "            " + tableName + "_Repository entity_rep = new " + tableName + "_Repository();\n";
            str += "            string m = entity_rep.Delete(ref entity);\n";
            str += "        }\n";
            str += "\n";

            return str;
        }

        public string getMethod(string tableName)
        {

            string adds = tableName.ElementAt(tableName.Length - 1) + "s";
            if (tableName.ElementAt(tableName.Length - 1) == 'y')
            {
                adds = "ies";
            }
            if (tableName.ElementAt(tableName.Length - 1) == 's')
            {
                adds = "s";
            }

            string str = "        public IEnumerable<" + tableName + "> get" + tableName.Substring(0, tableName.Length - 1) + adds + "()\n";
            str += "        {\n";
            str += "            List<" + tableName + "> entities = new List<" + tableName + ">();\n";
            str += "            " + tableName + "_Repository entity_rep = new " + tableName + "_Repository();\n";
            str += "            entities = entity_rep.FindAll();\n";
            str += "            return entities; \n";
            str += "        }\n";
            str += "\n";
            return str;
        }

        public string getFK_Method(string tableName, string FK_Column)
        {
            string[] by = FK_Column.Split('_');
            string str = "        public IEnumerable<" + tableName + "> get" + tableName + "s_By" + by[0] + "(" + tableName + " entity)\n";
            str += "        {\n";
            str += "            List<" + tableName + "> entities = new List<" + tableName + ">();\n";
            str += "            " + tableName + "_Repository entity_rep = new " + tableName + "_Repository();\n";
            str += "            entities = entity_rep.FindBy" + by[0] + "(ref entity);\n";
            str += "            return entities; \n";
            str += "        }\n";
            str += "\n";
            return str;
        }

        public string getFK_Method(string tableName)
        {
            string str = "";
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
                    where FK.TABLE_NAME= '" + tableName + "'  ORDER BY  1,2,3,4";
            DataSet ds = new DataSet();
            DataAccess cls = new DataAccess();
            string m = cls.getData(sql, ref ds);

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                str += "        public IEnumerable<" + ds.Tables[0].Rows[i]["PK_Table"].ToString() + "> get" + ds.Tables[0].Rows[i]["PK_Table"].ToString() + "()\n";
                str += "        {\n";
                str += "            List<" + ds.Tables[0].Rows[i]["PK_Table"].ToString() + "> entities = new List<" + ds.Tables[0].Rows[i]["PK_Table"].ToString() + ">();\n";
                str += "            " + ds.Tables[0].Rows[i]["PK_Table"].ToString() + "_Repository entity_rep = new " + ds.Tables[0].Rows[i]["PK_Table"].ToString() + "_Repository();\n";
                str += "            entities = entity_rep.FindAll();\n";
                str += "            return entities; \n";
                str += "        }\n";
                str += "\n\n";
            }

                return str;
        }



    }
}
