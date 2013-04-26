using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;

namespace SITGenerateFramework
{
    public class ViewModel : SITGenerateFramework.IViewModel
    {
        public void generateViewModel(string constr, string outputDir, string namesp)
        {
            //try
            //{
            //    Directory.CreateDirectory(outputDir + "\\ViewModel\\");
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
                classStr += getHeader(namesp, dsTables.Tables[0].Rows[i]["Name"].ToString());

                sql = "select * from information_schema.columns  where table_name = '" + dsTables.Tables[0].Rows[i]["Name"].ToString() + "'";
                DataSet ds = new DataSet();
                m = cls.getData(sql, ref ds);

                classStr += getMethods(ds, dsTables.Tables[0].Rows[i]["Name"].ToString());
                classStr += "\n     \n}";

                TextWriter tw = new StreamWriter(outputDir + "\\" + dsTables.Tables[0].Rows[i]["Name"].ToString() + "\\" + dsTables.Tables[0].Rows[i]["Name"].ToString() + "ViewModel" + ".cs");
                tw.WriteLine(classStr);
                tw.Close();
            }
        }



        public string getUsingSection(string namesp)
        {
            string usingSection = "using System;\n";
            usingSection += "using System.Net;\n";
            usingSection += "using System.Windows;\n";
            usingSection += "using System.Windows.Controls;\n";
            usingSection += "using System.Windows.Documents;\n";
            usingSection += "using System.Windows.Ink;\n";
            usingSection += "using System.Windows.Input;\n";
            usingSection += "using System.Windows.Media;\n";
            usingSection += "using System.Windows.Media.Animation;\n";
            usingSection += "using System.Windows.Shapes;\n";
            usingSection += "using System.ComponentModel;\n";
            usingSection += "using " + namesp + ".Web.Services;\n";
            usingSection += "using " + namesp + ".Entities;\n";
            usingSection += "using System.ServiceModel.DomainServices.Client;\n";
            usingSection += "using System.Linq;\n";
            usingSection += "using System.Collections.ObjectModel;\n";
            usingSection += "\n";


            return usingSection;
        }

        public string getHeader(string namespaces, string TableName)
        {
            string str = "\n\n\nnamespace " + namespaces + ".ViewModel" + "\n{\n";
            str += "    public class " + TableName + "ViewModel : INotifyPropertyChanged\n";
            str += "    {\n";
            str += "        MainPage m = (MainPage)Application.Current.RootVisual;\n";


            return str;
        }

        public string getMethods(DataSet dsTableDefenation, string TableName)
        {
            string Methods = "{0}";

            string str = getProperties(dsTableDefenation, TableName);
            str += getCommands(dsTableDefenation, TableName);
            str += getMethods1(dsTableDefenation, TableName);
            str += "    }\n" + getclass(dsTableDefenation, TableName);
            //str += "\n\n" + getFindAllMethod(dsTableDefenation, TableName);

            Methods = System.String.Format(Methods, str);
            return Methods;
        }
        public string getProperties(DataSet dsTableDefenation, string TableName)
        {
            string Statment = "\n";

            Statment += "        #region Properties\n";
            Statment += "\n";
            Statment += "        public event PropertyChangedEventHandler PropertyChanged;\n";
            Statment += "        private void RaisePropertyChanged(string propertyname)\n";
            Statment += "        {\n";
            Statment += "            if (PropertyChanged != null)\n";
            Statment += "            {\n";
            Statment += "                try\n";
            Statment += "                {\n";
            Statment += "                    PropertyChanged(this, new PropertyChangedEventArgs(propertyname));\n";
            Statment += "                }\n";
            Statment += "                catch { }\n";
            Statment += "            }\n";
            Statment += "        }\n\n";



            Statment += "        private bool _isBusy;\n";
            Statment += "        public bool IsBusy\n";
            Statment += "        {\n";
            Statment += "            get\n";
            Statment += "            {\n";
            Statment += "                return _isBusy;\n";
            Statment += "            }\n";
            Statment += "            set\n";
            Statment += "            {\n";
            Statment += "                _isBusy = value;\n";
            Statment += "                RaisePropertyChanged(\"IsBusy\");\n";
            Statment += "            }\n";
            Statment += "        }\n\n";

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
                    string PK_Table = dsFK.Tables[0].Rows[j]["PK_Table"].ToString();
                    string adds = PK_Table.ElementAt(PK_Table.Length - 1) + "s";
                    if (PK_Table.ElementAt(PK_Table.Length - 1) == 'y')
                    {
                        adds = "ies";
                    }
                    if (PK_Table.ElementAt(PK_Table.Length - 1) == 's')
                    {
                        adds = "s";
                    }
                    Statment += "        public ObservableCollection<" + PK_Table + "> _" + dsFK.Tables[0].Rows[j]["CONSTRAINT_NAME"].ToString() + ";\n";
                    Statment += "        public ObservableCollection<" + PK_Table + "> " + dsFK.Tables[0].Rows[j]["CONSTRAINT_NAME"].ToString() + "\n";
                    Statment += "        {\n";
                    Statment += "            get\n";
                    Statment += "            {\n";
                    Statment += "                return _" + dsFK.Tables[0].Rows[j]["CONSTRAINT_NAME"].ToString() + ";\n";
                    Statment += "            }\n";
                    Statment += "        }\n\n";
                }
                else
                {
                    string PK_Table = dsFK.Tables[0].Rows[j]["PK_Table"].ToString();
                    string FK_Column = dsFK.Tables[0].Rows[j]["FK_Column"].ToString();

                    Statment += "        public ObservableCollection<" + PK_Table + "> _" + FK_Column.ToString().Substring(0, 1).ToLower() + FK_Column.Substring(1, FK_Column.ToString().Length - 1).ToLower() + ";\n";
                    Statment += "        public ObservableCollection<" + PK_Table + "> " + FK_Column + "\n";
                    Statment += "        {\n";
                    Statment += "            get\n";
                    Statment += "            {\n";
                    Statment += "                return _" + FK_Column.ToString().Substring(0, 1).ToLower() + FK_Column.Substring(1, FK_Column.ToString().Length - 1).ToLower() + ";\n";
                    Statment += "            }\n";
                    Statment += "        }\n\n";
                }
            }

            Statment += "        private " + TableName + " _selected" + TableName + " = null;\n";
            Statment += "        public " + TableName + " Selected" + TableName + "\n";
            Statment += "        {\n";
            Statment += "            get\n";
            Statment += "            {\n";
            Statment += "                return _selected" + TableName + ";\n";
            Statment += "            }\n";
            Statment += "            set\n";
            Statment += "            {\n";
            Statment += "                _selected" + TableName + " = value;\n";
            Statment += "                RaisePropertyChanged(\"Selected" + TableName + "\");\n";
            Statment += "            }\n";
            Statment += "        }\n\n";


            string addss = TableName.ElementAt(TableName.Length - 1) + "s";
            if (TableName.ElementAt(TableName.Length - 1) == 'y')
            {
                addss = "ies";
            }
            if (TableName.ElementAt(TableName.Length - 1) == 's')
            {
                addss = "s";
            }

            Statment += "        public ObservableCollection<" + TableName + "> _" + TableName.Substring(0, TableName.Length - 1) + addss + ";\n";
            Statment += "        public ObservableCollection<" + TableName + "> " + TableName.Substring(0, TableName.Length - 1) + addss + "\n";
            Statment += "        {\n";
            Statment += "            get\n";
            Statment += "            {\n";
            Statment += "                return _" + TableName.Substring(0, TableName.Length - 1) + addss + ";\n";
            Statment += "            }\n";
            Statment += "        }\n";
            Statment += "        #endregion\n";

            return Statment;
        }

        public string getCommands(DataSet dsTableDefenation, string TableName)
        {


            string adds = TableName.ElementAt(TableName.Length - 1) + "s";
            if (TableName.ElementAt(TableName.Length - 1) == 'y')
            {
                adds = "ies";
            }
            if (TableName.ElementAt(TableName.Length - 1) == 's')
            {
                adds = "s";
            }

            string Statment = "\n";

            //=========================================== EditCommand
            Statment += "        #region Commands\n";
            Statment += "        public ICommand EditCommand\n";
            Statment += "        {\n";
            Statment += "            get\n";
            Statment += "            {\n";
            Statment += "                return new " + TableName + "Command(this, \"Edit\");\n";
            Statment += "            }\n";
            Statment += "        }\n";
            Statment += "        public void Edit()\n";
            Statment += "        {\n";

            Statment += "            Selected" + TableName + ".Date_Updated = DateTime.Now;\n";
            Statment += "            Selected" + TableName + ".Updated_By = m.UserId;\n";
            Statment += "\n";

            Statment += "            Views." + TableName + "AddEdit frm = new Views." + TableName + "AddEdit();\n";
            Statment += "            frm.Title = \"Edit " + TableName.Replace("_", " ") + " Information\";\n";
            Statment += "            frm.DataContext = this;\n";
            Statment += "            frm.Show();\n";
            Statment += "        }\n\n";

            //=========================================== RefreshCommand 
            Statment += "        public ICommand RefreshCommand\n";
            Statment += "        {\n";
            Statment += "            get\n";
            Statment += "            {\n";
            Statment += "                return new " + TableName + "Command(this, \"Refresh\");\n";
            Statment += "            }\n";
            Statment += "        }\n";
            Statment += "        public void Refresh()\n";
            Statment += "        {\n";
            Statment += "            m._context." + TableName.Substring(0, TableName.Length - 1) + adds + ".Clear();\n";
            Statment += "            m._context.Load(m._context.get" + TableName.Substring(0, TableName.Length - 1) + adds + "Query(), Load" + TableName.Substring(0, TableName.Length - 1) + adds + "Completed, null);\n";
            Statment += "        }\n";
            Statment += "\n";

            //=========================================== SaveCommand
            Statment += "        public ICommand SaveCommand\n";
            Statment += "        {\n";
            Statment += "            get\n";
            Statment += "            {\n";
            Statment += "                return new " + TableName + "Command(this, \"Save\");\n";
            Statment += "            }\n";
            Statment += "        }\n";
            Statment += "        public void Save()\n";
            Statment += "        {\n";
            Statment += "            if (m._context.HasChanges)\n";
            Statment += "            {\n";
            Statment += "                m._context.SubmitChanges();\n";
            Statment += "                RaisePropertyChanged(\"" + TableName.Substring(0, TableName.Length - 1) + adds + "\");\n";
            Statment += "            }\n";
            Statment += "        }\n\n";

            //=========================================== InsertCommand
            Statment += "        public ICommand InsertCommand\n";
            Statment += "        {\n";
            Statment += "            get\n";
            Statment += "            {\n";
            Statment += "                return new " + TableName + "Command(this, \"Insert\");\n";
            Statment += "            }\n";
            Statment += "        }\n";
            Statment += "        public void Insert()\n";



            Statment += "        {\n";
            Statment += "            " + TableName + " entity = new " + TableName + "();\n";

            Statment += "            entity.Updated_By = m.UserId;\n";
            Statment += "            entity.Created_By = m.UserId;\n";
            //Statment += "            entity.Date_Created = DateTime.Now;\n";
            //Statment += "            entity.Date_Updated = DateTime.Now;\n";
            Statment += "            entity.Name = \"\";\n";

            for (int i = 0; i < dsTableDefenation.Tables[0].Rows.Count; i++)
            {
                string Data_Type = "";
                if (dsTableDefenation.Tables[0].Rows[i]["Data_Type"] != DBNull.Value)
                    Data_Type = dsTableDefenation.Tables[0].Rows[i]["Data_Type"].ToString();
                
                if (Data_Type == "datetime")
                {
                    Statment += "            entity." + dsTableDefenation.Tables[0].Rows[i]["COLUMN_NAME"].ToString() + " = DateTime.Now;\n";
                }
            }


            Statment += "            m._context." + TableName.Substring(0, TableName.Length - 1) + adds + ".Add(entity);\n";
            Statment += "            _" + TableName.Substring(0, TableName.Length - 1) + adds + ".Add(entity);\n";
            Statment += "            RaisePropertyChanged(\"" + TableName.Substring(0, TableName.Length - 1) + adds + "\");\n";
            Statment += "            Selected" + TableName + " = entity;\n\n";
            Statment += "            Views." + TableName + "AddEdit frm = new Views." + TableName + "AddEdit();\n";
            Statment += "            frm.Title = \"Add New " + TableName.Replace("_", " ") + "\";\n";
            Statment += "            frm.DataContext = this;\n";
            Statment += "            frm.Show();\n";
            Statment += "            frm.Closed += new EventHandler(frm_Closed);\n";
            Statment += "        }\n\n";

            Statment += "        void frm_Closed(object sender, EventArgs e)\n";
            Statment += "        {\n";
            Statment += "            ChildWindow window = sender as ChildWindow;\n";
            Statment += "            if (window.DialogResult == false)\n";
            Statment += "            {\n";
            Statment += "                try\n";
            Statment += "                {\n";
            //Statment += "                    Countries.Remove(SelectedCountry);\n";
            Statment += "                _" + TableName.Substring(0, TableName.Length - 1) + adds + ".Remove(Selected" + TableName + ");\n";
            Statment += "                RaisePropertyChanged(\"" + TableName.Substring(0, TableName.Length - 1) + adds + "\");\n";
            Statment += "                }\n";
            Statment += "                catch { }\n";
            Statment += "            }\n";
            Statment += "        }\n";
            Statment += "\n";

            //=========================================== CancelCommand
            Statment += "        public ICommand CancelCommand\n";
            Statment += "        {\n";
            Statment += "            get\n";
            Statment += "            {\n";
            Statment += "                return new " + TableName + "Command(this, \"Cancel\");\n";
            Statment += "            }\n";
            Statment += "        }\n";
            Statment += "        public void Cancel()\n";



            Statment += "        {\n";
            Statment += "            m._context.RejectChanges();\n";
            Statment += "            RaisePropertyChanged(\"" + TableName.Substring(0, TableName.Length - 1) + adds + "\");\n";

            Statment += "        }\n\n";

            //=========================================== DeleteCommand

            Statment += "        public ICommand DeleteCommand\n";
            Statment += "        {\n";
            Statment += "            get\n";
            Statment += "            {\n";
            Statment += "                return new " + TableName + "Command(this, \"Delete\");\n";
            Statment += "            }\n";
            Statment += "        }\n";
            Statment += "        public void Delete()\n";
            Statment += "        {\n";
            //Statment += "            if (MessageBox.Show(\"Are you sure to want to delete this " + TableName + "?\", \"Delete\", MessageBoxButton.OKCancel) == MessageBoxResult.OK)\n";
            Statment += "            if (MessageBox.Show(\"Are you sure to want to delete this record?\", \"Delete\", MessageBoxButton.OKCancel) == MessageBoxResult.OK)\n";
            Statment += "            {\n";
            Statment += "                m._context." + TableName.Substring(0, TableName.Length - 1) + adds + ".Remove(Selected" + TableName + ");\n";
            Statment += "                _" + TableName.Substring(0, TableName.Length - 1) + adds + ".Remove(Selected" + TableName + ");\n";
            Statment += "                m._context.SubmitChanges();\n";
            Statment += "                RaisePropertyChanged(\"" + TableName.Substring(0, TableName.Length - 1) + adds + "\");\n";
            Statment += "            }\n";
            Statment += "        }\n";
            Statment += "        #endregion\n";




            return Statment;
        }

        public string getMethods1(DataSet dsTableDefenation, string TableName)
        {
            string Statment = "\n";
            Statment += "        #region Methods\n";
            Statment += "        public " + TableName + "ViewModel()\n";
            Statment += "        {\n";
            Statment += "            IsBusy = true;\n";

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


            bool foundCreatedBy = false;
            for (int j = 0; j < dsFK.Tables[0].Rows.Count; j++)
            {

                if (dsFK.Tables[0].Rows[j]["FK_Column"].ToString() != "Created_By" && dsFK.Tables[0].Rows[j]["FK_Column"].ToString() != "Updated_By")
                {
                    string PK_Table = dsFK.Tables[0].Rows[j]["PK_Table"].ToString();
                    string adds = PK_Table.ElementAt(PK_Table.Length - 1) + "s";
                    if (PK_Table.ElementAt(PK_Table.Length - 1) == 'y')
                    {
                        adds = "ies";
                    }
                    if (PK_Table.ElementAt(PK_Table.Length - 1) == 's')
                    {
                        adds = "s";
                    }


                    Statment += "            if (m._context." + PK_Table.Substring(0, PK_Table.ToString().Length - 1) + "" + adds + ".Count == 0)\n";
                    Statment += "            {\n";
                    Statment += "                m._context.Load(m._context.get" + PK_Table.Substring(0, PK_Table.ToString().Length - 1) + "" + adds + "Query(), Load" + dsFK.Tables[0].Rows[j]["CONSTRAINT_NAME"].ToString() + "Completed, null);\n";
                    Statment += "            }\n";
                    Statment += "            else\n";
                    Statment += "            {\n";
                    Statment += "                _" + dsFK.Tables[0].Rows[j]["CONSTRAINT_NAME"].ToString() + " = new ObservableCollection<" + PK_Table + ">(m._context." + PK_Table.Substring(0, PK_Table.Length - 1) + "" + adds + ");\n";
                    Statment += "                RaisePropertyChanged(\"" + dsFK.Tables[0].Rows[j]["CONSTRAINT_NAME"].ToString() + "\");\n";
                    Statment += "            }\n\n";
                }
                else
                {
                    foundCreatedBy = true;
                }

            }

            if (foundCreatedBy == true)
            {
                Statment += "            if (m._context.Users.Count == 0)\n";
                Statment += "            {\n";
                Statment += "                m._context.Load(m._context.getUsersQuery(), LoadUsersCompleted, null);\n";
                Statment += "            }\n";
                Statment += "            else\n";
                Statment += "            {\n";
                Statment += "                _updated_by = new ObservableCollection<User>(m._context.Users);\n\n";
                Statment += "                RaisePropertyChanged(\"Updated_By\");\n";
                Statment += "                _created_by = new ObservableCollection<User>(m._context.Users);\n";
                Statment += "                RaisePropertyChanged(\"Created_By\");\n";
                Statment += "            }\n";
            }


            string addss = TableName.ElementAt(TableName.Length - 1) + "s";
            if (TableName.ElementAt(TableName.Length - 1) == 'y')
            {
                addss = "ies";
            }
            if (TableName.ElementAt(TableName.Length - 1) == 's')
            {
                addss = "s";
            }

            Statment += "            if (m._context." + TableName.Substring(0, TableName.Length - 1) + addss + ".Count == 0)\n";
            Statment += "            {\n";
            Statment += "                m._context.Load(m._context.get" + TableName.Substring(0, TableName.Length - 1) + addss + "Query(), Load" + TableName.Substring(0, TableName.Length - 1) + addss + "Completed, null);\n";
            Statment += "            }\n";
            Statment += "            else\n";
            Statment += "            {\n";
            Statment += "                _" + TableName.Substring(0, TableName.Length - 1) + addss + " = new ObservableCollection<" + TableName + ">(m._context." + TableName.Substring(0, TableName.Length - 1) + addss + ");\n";
            Statment += "                RaisePropertyChanged(\"" + TableName.Substring(0, TableName.Length - 1) + addss + "\");\n";
            Statment += "                IsBusy = false;\n";
            Statment += "            }\n";
            Statment += "        }\n\n";

            //=================================================================== Load Copleted
            for (int j = 0; j < dsFK.Tables[0].Rows.Count; j++)
            {
                if (dsFK.Tables[0].Rows[j]["FK_Column"].ToString() != "Created_By" && dsFK.Tables[0].Rows[j]["FK_Column"].ToString() != "Updated_By")
                {
                    string PK_Table = dsFK.Tables[0].Rows[j]["PK_Table"].ToString();
                    string adds = PK_Table.ElementAt(PK_Table.Length - 1) + "s";
                    if (PK_Table.ElementAt(PK_Table.Length - 1) == 'y')
                    {
                        adds = "ies";
                    }
                    if (PK_Table.ElementAt(PK_Table.Length - 1) == 's')
                    {
                        adds = "s";
                    }
                    Statment += "        private void Load" + dsFK.Tables[0].Rows[j]["CONSTRAINT_NAME"].ToString() + "Completed(LoadOperation<" + PK_Table + "> lo)\n";
                    Statment += "        {\n";
                    Statment += "            _" + dsFK.Tables[0].Rows[j]["CONSTRAINT_NAME"].ToString() + " = new ObservableCollection<" + PK_Table + ">(m._context." + PK_Table.Substring(0, PK_Table.Length - 1) + "" + adds + ");\n";
                    Statment += "            RaisePropertyChanged(\"" + dsFK.Tables[0].Rows[j]["CONSTRAINT_NAME"].ToString() + "\");\n";
                    Statment += "        }\n\n";
                }
            }


            if (foundCreatedBy == true)
            {
                Statment += "        private void LoadUsersCompleted(LoadOperation<User> lo)\n";
                Statment += "        {\n";
                Statment += "            _updated_by = new ObservableCollection<User>(m._context.Users);\n";
                Statment += "            RaisePropertyChanged(\"Updated_By\");\n";
                Statment += "\n";
                Statment += "            _created_by = new ObservableCollection<User>(m._context.Users);\n";
                Statment += "            RaisePropertyChanged(\"Created_By\");\n";
                Statment += "        }\n";

            }
            Statment += "        private void Load" + TableName.Substring(0, TableName.Length - 1) + addss + "Completed(LoadOperation<" + TableName + "> lo)\n";
            Statment += "        {\n";
            Statment += "            _" + TableName.Substring(0, TableName.Length - 1) + addss + " = new ObservableCollection<" + TableName + ">(m._context." + TableName.Substring(0, TableName.Length - 1) + addss + ");\n";
            Statment += "            RaisePropertyChanged(\"" + TableName.Substring(0, TableName.Length - 1) + addss + "\");\n";
            Statment += "            IsBusy = false;\n";
            Statment += "        }\n";
            Statment += "        #endregion\n";

            Statment += "\n";

            return Statment;
        }

        public string getclass(DataSet dsTableDefenation, string TableName)
        {
            string Statment = "\n";

            Statment += "    #region " + TableName + " Commands Class\n";
            Statment += "    public class " + TableName + "Command : ICommand\n";
            Statment += "    {";
            Statment += "        public event EventHandler CanExecuteChanged;\n";
            Statment += "        private " + TableName + "ViewModel _evm = null;\n";
            Statment += "        private string _cmd = null;\n";
            Statment += "\n";
            Statment += "        public " + TableName + "Command(" + TableName + "ViewModel evm, string cmd)\n";
            Statment += "        {\n";
            Statment += "            _evm = evm;\n";
            Statment += "            _cmd = cmd;\n";
            Statment += "        }\n";
            Statment += "        public bool CanExecute(object parameter)\n";
            Statment += "        {\n";
            Statment += "            return true;\n";
            Statment += "        }\n";
            Statment += "        public void Execute(object parameter)\n";
            Statment += "        {\n";
            Statment += "            switch (_cmd)\n";
            Statment += "            {";
            Statment += "                case \"Delete\":\n";
            Statment += "                    {\n";
            Statment += "                        _evm.Delete();\n";
            Statment += "                    } break;\n";
            Statment += "                case \"Insert\":\n";
            Statment += "                    {\n";
            Statment += "                        _evm.Insert();\n";
            Statment += "                    } break;\n";
            Statment += "                case \"Save\":\n";
            Statment += "                    {\n";
            Statment += "                        _evm.Save();\n";
            Statment += "                    } break;\n";
            Statment += "                case \"Edit\":\n";
            Statment += "                    {\n";
            Statment += "                        _evm.Edit();\n";
            Statment += "                    } break;\n";

            Statment += "                case \"Cancel\":\n";
            Statment += "                    {\n";
            Statment += "                        _evm.Cancel();\n";
            Statment += "                    } break;\n";

            Statment += "                case \"Refresh\":\n";
            Statment += "                    {\n";
            Statment += "                        _evm.Refresh();\n";
            Statment += "                    } break;\n";


            Statment += "            }\n";
            Statment += "        }\n";
            Statment += "    }\n";
            Statment += "    #endregion\n";


            return Statment;
        }
    }
}
