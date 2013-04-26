using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;

namespace SITGenerateFramework
{
    public class View
    {
        public void generateView(string constr, string outputDir, string namesp)
        {
            //try
            //{
            //    Directory.CreateDirectory(outputDir + "\\Views\\");
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
                TextWriter tw = new StreamWriter(outputDir + "\\" + dsTables.Tables[0].Rows[i]["Name"].ToString() + "\\" + dsTables.Tables[0].Rows[i]["Name"].ToString() + "Page.xaml");
                tw.WriteLine(classStr);
                tw.Close();
            }

            for (int i = 0; i < dsTables.Tables[0].Rows.Count; i++)
            {
                string classStr = "";
                classStr += getCobeBehindMainPage(namesp, dsTables.Tables[0].Rows[i]["Name"].ToString());
                TextWriter tw = new StreamWriter(outputDir + "\\" + dsTables.Tables[0].Rows[i]["Name"].ToString() + "\\" + dsTables.Tables[0].Rows[i]["Name"].ToString() + "Page.xaml.cs");
                tw.WriteLine(classStr);
                tw.Close();
            }

            for (int i = 0; i < dsTables.Tables[0].Rows.Count; i++)
            {
                string classStr = "";
                classStr += getAddEdit(namesp, dsTables.Tables[0].Rows[i]["Name"].ToString());
                TextWriter tw = new StreamWriter(outputDir + "\\" + dsTables.Tables[0].Rows[i]["Name"].ToString() + "\\" + dsTables.Tables[0].Rows[i]["Name"].ToString() + "AddEdit.xaml");
                tw.WriteLine(classStr);
                tw.Close();
            }
            for (int i = 0; i < dsTables.Tables[0].Rows.Count; i++)
            {
                string classStr = "";
                classStr += getCobeBehindAddEdit(namesp, dsTables.Tables[0].Rows[i]["Name"].ToString());
                TextWriter tw = new StreamWriter(outputDir + "\\" + dsTables.Tables[0].Rows[i]["Name"].ToString() + "\\" + dsTables.Tables[0].Rows[i]["Name"].ToString() + "AddEdit.xaml.cs");
                tw.WriteLine(classStr);
                tw.Close();
            }

        }

        private string getCobeBehindAddEdit(string namesp, string tableName)
        {
            string str = "";

            str += "using System;\n";
            str += "using System.Collections.Generic;\n";
            str += "using System.Linq;\n";
            str += "using System.Net;\n";
            str += "using System.Windows;\n";
            str += "using System.Windows.Controls;\n";
            str += "using System.Windows.Documents;\n";
            str += "using System.Windows.Input;\n";
            str += "using System.Windows.Media;\n";
            str += "using System.Windows.Media.Animation;\n";
            str += "using System.Windows.Shapes;\n\n";


            str += "namespace " + namesp + ".Views\n";
            str += "{\n";
            str += "    public partial class " + tableName + "AddEdit : ChildWindow\n";
            str += "    {\n";
            str += "        public " + tableName + "AddEdit()\n";
            str += "        {\n";
            str += "            InitializeComponent();\n";
            str += "        }\n\n";

            str += "        private void OKButton_Click(object sender, RoutedEventArgs e)\n";
            str += "        {\n";
            str += "            this.DialogResult = true;\n";
            str += "        }\n\n";
            str += "        private void CancelButton_Click(object sender, RoutedEventArgs e)\n";
            str += "        {\n";
            str += "            this.DialogResult = false;\n";
            str += "        }\n\n";
            str += "        private void Button_Click(object sender, RoutedEventArgs e)\n";
            str += "        {\n";
            str += "            bool result = false;\n";
            str += "            foreach (var ctrl in LayoutRoot.Children)\n";
            str += "            {\n";
            str += "                if (Validation.GetErrors(ctrl).Count > 0)\n";
            str += "                {\n";
            str += "                    result = true;\n";
            str += "                }\n";
            str += "            }\n";
            str += "            if (result == false)\n";
            str += "            {\n";
            str += "                this.DialogResult = true;\n";
            str += "            }\n";
            str += "        }\n";
            str += "\n";



            str += "        private void ChildWindow_Closed(object sender, EventArgs e)\n";
            str += "        {\n";
            str += "            MainPage m = (MainPage)Application.Current.RootVisual;\n";
            str += "            if (DialogResult == true)\n";
            str += "            {\n";
            str += "                if (m._context.HasChanges)\n";
            str += "                {\n";
            str += "                    m._context.SubmitChanges();\n";
            str += "                }\n";
            str += "            }\n";
            str += "            else\n";
            str += "            {\n";
            str += "                m._context.RejectChanges();\n";
            str += "            }\n";
            str += "\n";

            str += "    }\n";
            str += "}\n";
            str += "}\n";

            return str;
        }
        private string getAddEdit(string namesp, string tableName)
        {

            int Height = 171;


            DataAccess cls = new DataAccess();
            string sql = "select * from information_schema.columns  where table_name = '" + tableName + "'";
            DataSet dsColumns = new DataSet();
            string m = cls.getData(sql, ref dsColumns);

            Height = ((dsColumns.Tables[0].Rows.Count - 4) / 2 * 33) + 80;

            string str = "";
            str += "<sdk:ChildWindow\n";
            str += "           xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" \n";
            str += "           xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\" \n";
            str += "           xmlns:controls=\"clr-namespace:System.Windows.Controls;assembly=System.Windows.Controls\"\n";

            str += "           xmlns:my=\"clr-namespace:" + namesp + ".Views\" \n";
            str += "           xmlns:sdk=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation/sdk\"\n";
            str += "           xmlns:toolkit=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation/toolkit\"\n";
            str += "           xmlns:d=\"http://schemas.microsoft.com/expression/blend/2008\"\n";
            str += "           xmlns:mc=\"http://schemas.openxmlformats.org/markup-compatibility/2006\"\n";
            str += "           mc:Ignorable=\"d\"\n";
            str += "           x:Class=\"" + namesp + ".Views." + tableName + "AddEdit\"\n";
            str += "           Width=\"620\" Height=\"" + Height + "\"\n";
            str += "           Title=\"" + tableName.Replace("_", " ") + " Information\"   Closed=\"ChildWindow_Closed\">\n\n";


            str += "    <Grid x:Name=\"LayoutRoot\" Margin=\"2\">\n\n";

            //====================================RowDefinitions
            str += "        <Grid.RowDefinitions>\n";
            for (int i = 0; i < (dsColumns.Tables[0].Rows.Count - 4) / 2; i++)
            {
                str += "            <RowDefinition Height=\"33\" />\n";
            }
            str += "            <RowDefinition Height=\"50*\" />\n";
            str += "        </Grid.RowDefinitions>\n";

            //=================================== ColumnDefinitions
            str += "        <Grid.ColumnDefinitions>\n";
            str += "            <ColumnDefinition Width=\"100\" />\n";
            str += "            <ColumnDefinition Width=\"200\" />\n";
            str += "            <ColumnDefinition Width=\"100\" />\n";
            str += "            <ColumnDefinition Width=\"200\" />\n";
            str += "        </Grid.ColumnDefinitions>\n\n";

            //======================================================
            for (int j = 0; j < dsColumns.Tables[0].Rows.Count; j++)
            {
                if (dsColumns.Tables[0].Rows[j]["COLUMN_NAME"].ToString() != "Is_Deleted" && dsColumns.Tables[0].Rows[j]["COLUMN_NAME"].ToString() != "Created_By" && dsColumns.Tables[0].Rows[j]["COLUMN_NAME"].ToString() != "Updated_By" && dsColumns.Tables[0].Rows[j]["COLUMN_NAME"].ToString() != "Date_Created" && dsColumns.Tables[0].Rows[j]["COLUMN_NAME"].ToString() != "Date_Updated")
                {

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

                    string dType = "100";
                    switch (Data_Type)
                    {
                        case "int": dType = "100"; break;
                        case "datetime": dType = "100"; break;
                        case "bit": dType = "80"; break;
                        case "real": dType = "80"; break;
                        case "money": dType = "80"; break;
                        case "float": dType = "80"; break;
                        default: dType = "200"; break;
                    }

                    if (dsFK.Tables[0].Rows.Count == 0)
                    {
                        int row = j / 2;

                        if (dsColumns.Tables[0].Rows[j]["COLUMN_NAME"].ToString() == "Id")
                        {
                            str += "        <sdk:Label Content=\"" + dsColumns.Tables[0].Rows[j]["COLUMN_NAME"].ToString().Replace("_", " ") + ":\"  Grid.Column=\"0\" Grid.Row=\"" + row + "\"   Margin=\"10,3,10,3\" />\n";
                            str += "        <TextBox Text=\"{Binding Selected" + tableName + "." + dsColumns.Tables[0].Rows[j]["COLUMN_NAME"].ToString() + ", Mode=TwoWay}\" Grid.Column=\"1\"  Grid.Row=\"" + row + "\" IsReadOnly=\"True\" Margin=\"10,3,10,3\"/>\n";
                        }
                        else
                        {
                            switch (Data_Type)
                            {
                                case "datetime":
                                    {
                                        if (j % 2 == 0)
                                        {
                                            str += "        <sdk:Label Content=\"" + dsColumns.Tables[0].Rows[j]["COLUMN_NAME"].ToString().Replace("_", " ") + ":\"  Grid.Column=\"0\" Grid.Row=\"" + row + "\" Margin=\"10,3,10,3\" />\n";
                                            str += "        <sdk:DatePicker SelectedDate=\"{Binding Selected" + tableName + "." + dsColumns.Tables[0].Rows[j]["COLUMN_NAME"].ToString() + ", Mode=TwoWay}\" Grid.Column=\"1\"  Grid.Row=\"" + row + "\"  Margin=\"10,3,10,3\"/>\n";
                                        }
                                        else
                                        {
                                            str += "        <sdk:Label Content=\"" + dsColumns.Tables[0].Rows[j]["COLUMN_NAME"].ToString().Replace("_", " ") + ":\"  Grid.Column=\"2\" Grid.Row=\"" + row + "\" Margin=\"10,3,10,3\" />\n";
                                            str += "        <sdk:DatePicker SelectedDate=\"{Binding Selected" + tableName + "." + dsColumns.Tables[0].Rows[j]["COLUMN_NAME"].ToString() + ", Mode=TwoWay}\" Grid.Column=\"3\"  Grid.Row=\"" + row + "\"  Margin=\"10,3,10,3\"/>\n";
                                        }
                                        str += "\n";
                                    } break;
                                case "bit":
                                    {
                                        if (j % 2 == 0)
                                        {
                                            str += "        <sdk:Label Content=\"" + dsColumns.Tables[0].Rows[j]["COLUMN_NAME"].ToString().Replace("_", " ") + ":\"  Grid.Column=\"0\" Grid.Row=\"" + row + "\" Margin=\"10,3,10,3\" />\n";
                                            str += "        <CheckBox IsChecked=\"{Binding Selected" + tableName + "." + dsColumns.Tables[0].Rows[j]["COLUMN_NAME"].ToString() + ", Mode=TwoWay}\" Grid.Column=\"1\"  Grid.Row=\"" + row + "\"  Margin=\"10,3,10,3\"/>\n";
                                        }
                                        else
                                        {
                                            str += "        <sdk:Label Content=\"" + dsColumns.Tables[0].Rows[j]["COLUMN_NAME"].ToString().Replace("_", " ") + ":\"  Grid.Column=\"2\" Grid.Row=\"" + row + "\" Margin=\"10,3,10,3\" />\n";
                                            str += "        <CheckBox IsChecked=\"{Binding Selected" + tableName + "." + dsColumns.Tables[0].Rows[j]["COLUMN_NAME"].ToString() + ", Mode=TwoWay}\" Grid.Column=\"3\"  Grid.Row=\"" + row + "\"  Margin=\"10,3,10,3\"/>\n";
                                        }
                                        str += "\n";
                                    } break;
                                default:
                                    {
                                        if (j % 2 == 0)
                                        {
                                            str += "        <sdk:Label Content=\"" + dsColumns.Tables[0].Rows[j]["COLUMN_NAME"].ToString().Replace("_", " ") + ":\"  Grid.Column=\"0\" Grid.Row=\"" + row + "\" Margin=\"10,3,10,3\" />\n";
                                            str += "        <TextBox Text=\"{Binding Selected" + tableName + "." + dsColumns.Tables[0].Rows[j]["COLUMN_NAME"].ToString() + ", Mode=TwoWay}\" Grid.Column=\"1\"  Grid.Row=\"" + row + "\"  Margin=\"10,3,10,3\"/>\n";
                                        }
                                        else
                                        {
                                            str += "        <sdk:Label Content=\"" + dsColumns.Tables[0].Rows[j]["COLUMN_NAME"].ToString().Replace("_", " ") + ":\"  Grid.Column=\"2\" Grid.Row=\"" + row + "\" Margin=\"10,3,10,3\" />\n";
                                            str += "        <TextBox Text=\"{Binding Selected" + tableName + "." + dsColumns.Tables[0].Rows[j]["COLUMN_NAME"].ToString() + ", Mode=TwoWay}\" Grid.Column=\"3\"  Grid.Row=\"" + row + "\"  Margin=\"10,3,10,3\"/>\n";
                                        }
                                        str += "\n";
                                    } break;

                            }


                        }
                    }
                    else
                    {
                        str += "\n";
                        int row = j / 2;

                        string PK_Table = dsFK.Tables[0].Rows[0]["PK_Table"].ToString();
                        string adds = PK_Table.ElementAt(PK_Table.Length - 1) + "s";
                        if (PK_Table.ElementAt(PK_Table.Length - 1) == 'y')
                        {
                            adds = "ies";
                        }
                        if (PK_Table.ElementAt(PK_Table.Length - 1) == 's')
                        {
                            adds = "s";
                        }

                        if (j % 2 == 0)
                        {
                            str += "        <sdk:Label Content=\"" + dsFK.Tables[0].Rows[0]["FK_Column"].ToString().Replace("_", " ").Replace("Id", "") + ":\"  Grid.Column=\"0\" Grid.Row=\"" + row + "\" Margin=\"10,3,10,3\" />\n";
                            str += "        <ComboBox Grid.Column=\"1\"   Grid.Row=\"" + row + "\"  Margin=\"10,3,10,3\"\n";
                            str += "                                     ItemsSource=\"{Binding " + dsFK.Tables[0].Rows[0]["CONSTRAINT_NAME"].ToString() + "}\"\n";
                            str += "                                     DisplayMemberPath=\"Name\"\n";
                            str += "                                     SelectedValuePath =\"" + dsFK.Tables[0].Rows[0]["PK_Column"].ToString() + "\"\n";
                            str += "                                     SelectedValue=\"{Binding Selected" + tableName + "." + dsFK.Tables[0].Rows[0]["FK_Column"].ToString() + ", Mode=TwoWay}\"\n";
                            str += "                                      />\n";
                        }
                        else
                        {
                            str += "        <sdk:Label Content=\"" + dsFK.Tables[0].Rows[0]["FK_Column"].ToString().Replace("_", " ").Replace("Id", "") + ":\"  Grid.Column=\"2\" Grid.Row=\"" + row + "\" Margin=\"10,3,10,3\" />\n";
                            str += "        <ComboBox Grid.Column=\"3\"   Grid.Row=\"" + row + "\"  Margin=\"10,3,10,3\"\n";
                            str += "                                     ItemsSource=\"{Binding " + dsFK.Tables[0].Rows[0]["CONSTRAINT_NAME"].ToString() + "}\"\n";
                            str += "                                     DisplayMemberPath=\"Name\"\n";
                            str += "                                     SelectedValuePath =\"" + dsFK.Tables[0].Rows[0]["PK_Column"].ToString() + "\"\n";
                            str += "                                     SelectedValue=\"{Binding Selected" + tableName + "." + dsFK.Tables[0].Rows[0]["FK_Column"].ToString() + ", Mode=TwoWay}\"\n";
                            str += "                                      />\n";

                        }
                    }
                }
            }

            int row2 = dsColumns.Tables[0].Rows.Count / 2 + 1;
            str += "        <Button  Content=\"Close\"  Click=\"CancelButton_Click\"  Grid.Row=\"" + row2 + "\" Grid.Column=\"3\" HorizontalAlignment=\"Right\" FontSize=\"9\" Margin=\"0,0,5,0\"  Height=\"25\" />\n";
            str += "        <Button  Content=\"Save\"  Grid.Row=\"" + row2 + "\" Grid.Column=\"3\" HorizontalAlignment=\"Left\" Margin=\"70,0,0,0\" Style=\"{StaticResource GreenButton}\" Click=\"Button_Click\" FontSize=\"9\"  Height=\"25\" />\n";
            str += "    </Grid>\n";
            str += "</sdk:ChildWindow>\n";

            return str;

        }

        private string getCobeBehindMainPage(string namesp, string tableName)
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



           

            str += "using System;\n";
            str += "using System.Collections.Generic;\n";
            str += "using System.Linq;\n";
            str += "using System.Net;\n";
            str += "using System.Windows;\n";
            str += "using System.Windows.Controls;\n";
            str += "using System.Windows.Documents;\n";
            str += "using System.Windows.Input;\n";
            str += "using System.Windows.Media;\n";
            str += "using System.Windows.Media.Animation;\n";
            str += "using System.Windows.Shapes;\n";
            str += "using System.Windows.Navigation;\n";
            str += "using SilverlightReport;\n";
            str += "using " + namesp + ".ViewModel;\n";
            str += "using " + namesp + ".Entities;\n";
            str += "using System.Windows.Data;\n";
            str += "using System.Reflection;\n\n\n\n";


            str += "namespace " + namesp + ".Views\n";
            str += "{\n";
            str += "    public partial class " + tableName + "Page : Page\n";
            str += "    {\n";
            str += "        public " + tableName + "ViewModel emv { set; get; }\n\n";
            str += "        public " + tableName + "Page()\n";
            str += "        {\n";
            str += "            InitializeComponent();\n";
            str += "            emv = new " + tableName + "ViewModel();\n";
            str += "            this.DataContext = emv;\n";

            str += "            MainPage m = (MainPage)Application.Current.RootVisual;\n";
            str += "            if (m.cmboxBoxLang.SelectedIndex == 1)\n";
            str += "            {\n";
            str += "                Helpers.LanguageTool.TranslateChildWindow(this, \"ar_JO\");\n";
            str += "            }\n";
            str += "            else\n";
            str += "            {\n";
            str += "                Helpers.LanguageTool.TranslateChildWindow(this, \"en_US\");\n";
            str += "            }\n";
            str += "\n";
            str += "        }\n";
            str += "\n";
            str += "        // Executes when the user navigates to this page.\n";
            str += "        protected override void OnNavigatedTo(NavigationEventArgs e)\n";
            str += "        {\n";
            str += "        }\n";


            str += "        private void txtSearch_KeyUp(object sender, KeyEventArgs e)\n";
            str += "        {\n";
            str += "            if (e.Key == Key.Enter)\n";
            str += "            {\n";
            str += "                Predicate<object> predicate = null;\n";
            str += "                predicate = p => (Helpers.FullTextSearch.Matches(txtSearch.Text.ToLower().Split(' '), p));\n";
            str += "                PagedCollectionView view = new PagedCollectionView(emv." + tableName.Substring(0, tableName.Length - 1) + adds + ");\n";
            str += "                view.Filter = null;\n";
            str += "                view.Filter = predicate;\n";
            str += "                grd" + tableName + ".ItemsSource = view;\n";
            str += "            }\n";
            str += "        }\n";
            str += "\n";
            str += "\n";


            str += "        private Report report;\n";
            str += "        private void btnPrint_Click(object sender, RoutedEventArgs e)\n";
            str += "        {\n";

            str += "            string name = \"" + tableName + "Report.xaml\";\n";
            str += "            string fullName = string.Format(\"/" + namesp + ";component/Reports/{0}\", name);\n";
            str += "            this.report = Report.LoadFromXaml(fullName);\n";
            //str += "            this.report.ItemsSource = emv." + tableName.Substring(0, tableName.Length - 1) + adds + ";\n";

            str += "            //==============================================================\n";
            str += "            List<Entities." + tableName + "> entities = new List<Entities." + tableName + ">();\n";
            str += "            for (int i = 0; i < " + tableName + "Pager.PageCount; i++)\n";
            str += "            {\n";
            str += "                " + tableName + "Pager.PageIndex = i;\n";
            str += "                foreach (Entities." + tableName + " data in " + tableName + "Pager.Source)\n";
            str += "                {\n";
            str += "                    entities.Add(data);\n";
            str += "                }\n";
            str += "            }\n";
            str += "            this.report.ItemsSource = entities;\n";
            str += "            //==============================================================\n";
            str += "\n";
            str += "            Button btn = sender as Button;\n";
            str += "            if (btn.Name == \"btnPrint\")\n";
            str += "            {\n";
            str += "                this.report.Print();\n";
            str += "            }\n";
            str += "            else\n";
            str += "            {\n";
            str += "                this.report.Preview();\n";
            str += "            }\n";
            str += "        }\n";

            str += "    }\n";
            str += "}\n";



            return str;
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

            str += "<navigation:Page x:Class=\"" + namesp + ".Views." + tableName + "Page\" \n";
            str += "           xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" \n";
            str += "           xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\"\n";
            str += "           xmlns:d=\"http://schemas.microsoft.com/expression/blend/2008\"\n";
            str += "           xmlns:mc=\"http://schemas.openxmlformats.org/markup-compatibility/2006\"\n";
            str += "           mc:Ignorable=\"d\"\n";
            str += "           xmlns:navigation=\"clr-namespace:System.Windows.Controls;assembly=System.Windows.Controls.Navigation\"\n";
            str += "           xmlns:jib=\"clr-namespace:Jib.Controls.DataGrid;assembly=Jib.Controls\"      \n";
            str += "                 xmlns:my=\"clr-namespace:SilverlightClassLibrary1;assembly=SilverlightClassLibrary1\"\n";
            str += "           d:DesignWidth=\"940\" d:DesignHeight=\"480\"\n";
            str += "           Title=\"" + tableName + " Page\"\n";
            str += "           xmlns:sdk=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation/sdk\" \n";
            str += "           xmlns:toolkit=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation/toolkit\">\n\n";


            str += "    <Grid x:Name=\"LayoutRoot\">\n";
            str += "        <Grid.RowDefinitions>\n";
            str += "            <RowDefinition Height=\"55\" />\n";
            str += "            <RowDefinition Height=\"5\" />\n";
            str += "            <RowDefinition Height=\"435*\" />\n";
            str += "            <RowDefinition Height=\"30\" />\n";
            str += "            <RowDefinition Height=\"5\" />\n";
            str += "        </Grid.RowDefinitions>\n\n";


            str += "        <StackPanel  Margin=\"0,0,0,0\"  Orientation=\"Horizontal\"  Grid.Row=\"0\">\n";
            str += "            <StackPanel.Effect>\n";
            str += "                <DropShadowEffect/>\n";
            str += "            </StackPanel.Effect>\n";


            str += "\n";
            str += "            <Button Width=\"80\" Cursor=\"Hand\" Style=\"{StaticResource GreenButton}\" Command=\"{Binding InsertCommand}\"  Name=\"btnAdd\"  Margin=\"5,0,0,0\">\n";
            str += "                <StackPanel  Orientation=\"Vertical\" >\n";
            str += "                    <Image Source=\"/" + namesp + ";component/Images/Add.png\"  HorizontalAlignment=\"Center\" Height=\"30\"  Width=\"30\"/>\n";
            str += "                    <TextBlock  Text=\"Add New\"  HorizontalAlignment=\"Center\"  FontSize=\"10\"/>\n";
            str += "                </StackPanel>\n";
            str += "            </Button>\n";


            str += "\n";
            str += "            <Button Width=\"80\" Cursor=\"Hand\" Style=\"{StaticResource GreenButton}\" Command=\"{Binding EditCommand}\"  Name=\"btnEdit\"  Margin=\"5,0,0,0\">\n";
            str += "                <StackPanel  Orientation=\"Vertical\" >\n";
            str += "                    <Image Source=\"/" + namesp + ";component/Images/Edit.png\"  HorizontalAlignment=\"Center\" Height=\"30\"  Width=\"30\"/>\n";
            str += "                    <TextBlock  Text=\"Edit\"  HorizontalAlignment=\"Center\"  FontSize=\"10\"/>\n";
            str += "                </StackPanel>\n";
            str += "            </Button>\n";


            str += "\n";
            str += "            <Button Width=\"80\" Cursor=\"Hand\" Style=\"{StaticResource GreenButton}\" Command=\"{Binding DeleteCommand}\"  Name=\"btnDelete\"  Margin=\"5,0,0,0\">\n";
            str += "                <StackPanel  Orientation=\"Vertical\" >\n";
            str += "                    <Image Source=\"/" + namesp + ";component/Images/Delete.png\"  HorizontalAlignment=\"Center\" Height=\"30\"  Width=\"30\"/>\n";
            str += "                    <TextBlock  Text=\"Delete\"  HorizontalAlignment=\"Center\"  FontSize=\"10\"/>\n";
            str += "                </StackPanel>\n";
            str += "            </Button>\n";


            str += "\n";
            str += "            <Button Width=\"80\" Cursor=\"Hand\" Style=\"{StaticResource GreenButton}\" Command=\"{Binding RefreshCommand}\"  Name=\"btnRefresh\"  Margin=\"5,0,0,0\">\n";
            str += "                <StackPanel  Orientation=\"Vertical\" >\n";
            str += "                    <Image Source=\"/" + namesp + ";component/Images/Refresh.png\"  HorizontalAlignment=\"Center\" Height=\"30\"  Width=\"30\"/>\n";
            str += "                    <TextBlock  Text=\"Refresh\"  HorizontalAlignment=\"Center\"  FontSize=\"10\"/>\n";
            str += "                </StackPanel>\n";
            str += "            </Button>\n";


          

            str += "\n";
            str += "            <Button Width=\"80\" Cursor=\"Hand\" Style=\"{StaticResource GreenButton}\"   Name=\"btnPrint\"  Margin=\"5,0,0,0\" Click=\"btnPrint_Click\">\n";
            str += "                <StackPanel  Orientation=\"Vertical\" >\n";
            str += "                    <Image Source=\"/" + namesp + ";component/Images/Print.png\"  HorizontalAlignment=\"Center\" Height=\"30\"  Width=\"30\"/>\n";
            str += "                    <TextBlock  Text=\"Print\"  HorizontalAlignment=\"Center\"  FontSize=\"10\"/>\n";
            str += "                </StackPanel>\n";
            str += "            </Button>\n";

            str += "\n";
            str += "            <Button Width=\"80\" Cursor=\"Hand\" Style=\"{StaticResource GreenButton}\"  Name=\"btnPrintPreview\"  Margin=\"5,0,0,0\" Click=\"btnPrint_Click\">\n";
            str += "                <StackPanel  Orientation=\"Vertical\" >\n";
            str += "                    <Image Source=\"/" + namesp + ";component/Images/print-preview.png\"  HorizontalAlignment=\"Center\" Height=\"30\"  Width=\"30\"/>\n";
            str += "                    <TextBlock  Text=\"Preview\"  HorizontalAlignment=\"Center\"  FontSize=\"10\"/>\n";
            str += "                </StackPanel>\n";
            str += "            </Button>\n";
          
            str += "\n";
            str += "            <StackPanel.Background>\n";
            str += "                <LinearGradientBrush EndPoint=\"0.5,1\" StartPoint=\"0.5,0\">\n";
            str += "                    <GradientStop Color=\"#FF0E78BA\" Offset=\"0\" />\n";
            str += "                    <GradientStop Color=\"#FF013D98\" Offset=\"1\" />\n";
            str += "                </LinearGradientBrush>\n";
            str += "            </StackPanel.Background>\n";
            str += "        </StackPanel>\n";
            str += "\n";


            str += "        <my:QueryTextBox Height=\"25\" Name=\"txtSearch\" Width=\"200\" HorizontalAlignment=\"Right\" Margin=\"0,15,15,10\" KeyUp=\"txtSearch_KeyUp\" />";
            str += "\n";
            str += "\n";
            //str += "        <StackPanel  Margin=\"0,0,0,0\" Name=\"stackPanel1\" Orientation=\"Horizontal\"  Grid.Row=\"0\" Background=\"#FFD3D3D8\" >\n";
            //str += "            <StackPanel.Effect>\n";
            //str += "                <DropShadowEffect/>\n";
            //str += "            </StackPanel.Effect>\n";
            //str += "            <Button Command=\"{Binding InsertCommand}\" Content=\"Add New\"  Name=\"btnAdd\" Height=\"25\" Margin=\"5,0,0,0\" FontSize=\"9\" />\n";
            //str += "            <Button Content=\"Edit\"  Name=\"btnEdit\" Command=\"{Binding EditCommand}\" Height=\"25\" FontSize=\"9\" />\n";
            //str += "            <Button Command=\"{Binding DeleteCommand}\" Content=\"Delete\"  Name=\"btnDelete\"  Height=\"25\" FontSize=\"9\" />\n";
            //str += "            <Button Command=\"{Binding RefreshCommand}\" Content=\"Refresh\"  Name=\"btnRefresh\"  Height=\"25\" FontSize=\"9\" />\n";
            //str += "        </StackPanel>\n\n";

            // str += "        <sdk:DataPager HorizontalContentAlignment=\"Right\" x:Name=\"myPager\" Source=\"{Binding Path=ItemsSource, ElementName=grd" + tableName + "}\" PageSize=\"20\" Grid.Row=\"0\"  Width=\"200\" Margin=\"5\" HorizontalAlignment=\"Right\" Height=\"25\" />\n\n";
            //str += "        <sdk:DataPager HorizontalContentAlignment=\"Right\" x:Name=\"" + tableName + "Pager\" Source=\"{Binding Path=ItemsSource, ElementName=grd" + tableName + "}\" PageSize=\"20\" Grid.Row=\"0\"  Width=\"400\" Margin=\"5\" HorizontalAlignment=\"Right\" Height=\"25\" Padding=\"1\" DisplayMode=\"FirstLastPreviousNextNumeric\" NumericButtonCount=\"10\" Cursor=\"Hand\" />\n";

            str += "        <jib:JibGrid AutoGenerateColumns=\"False\"  IsReadOnly=\"True\" Name=\"grd" + tableName + "\"  FilteredItemsSource=\"{Binding " + tableName.Substring(0, tableName.Length - 1) + adds + ", Mode=TwoWay}\" Grid.Row=\"2\"\n";
            str += "            SelectedItem=\"{Binding Selected" + tableName + ", Mode=TwoWay}\" RowBackground=\"#FFD2D3D4\" Background=\"#FFE0E2E5\"  >\n";
            str += "            <sdk:DataGrid.Columns>\n";


            for (int j = 0; j < dsColumns.Tables[0].Rows.Count; j++)
            {
                if (dsColumns.Tables[0].Rows[j]["COLUMN_NAME"].ToString().Contains("Is_Deleted"))
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

                string dType = "100";
                switch (Data_Type)
                {
                    case "int": dType = "80"; break;
                    case "datetime": dType = "150"; break;
                    case "bit": dType = "80"; break;
                    case "real": dType = "80"; break;
                    case "money": dType = "80"; break;
                    case "float": dType = "80"; break;
                    default: dType = "250"; break;
                }

                if (dsFK.Tables[0].Rows.Count == 0)
                {

                    str += "                <sdk:DataGridTextColumn  Header=\"" + dsColumns.Tables[0].Rows[j]["COLUMN_NAME"].ToString().Replace("_", " ") + "\" Binding=\"{Binding " + dsColumns.Tables[0].Rows[j]["COLUMN_NAME"].ToString() + "}\"    Width=\"" + dType + "\" Foreground=\"Black\" />\n";
                }
                else
                {
                    if (dsFK.Tables[0].Rows[0]["FK_Column"].ToString() != "Created_By" && dsFK.Tables[0].Rows[0]["FK_Column"].ToString() != "Updated_By")
                    {

                        str += "                <sdk:DataGridTextColumn  Header=\"" + dsFK.Tables[0].Rows[0]["FK_Column"].ToString().Replace("_", " ").Replace("Id", "") + "\" Binding=\"{Binding " + dsFK.Tables[0].Rows[0]["FK_Column"].ToString().Replace("_Id", "_Name").Replace("_", "") + "" + "}\"    Width=\"" + 200 + "\" Foreground=\"Black\" />\n";
                    }
                    else
                    {
                        str += "                <sdk:DataGridTextColumn  Header=\"" + dsColumns.Tables[0].Rows[j]["COLUMN_NAME"].ToString().Replace("_", " ") + "\" Binding=\"{Binding " + dsFK.Tables[0].Rows[0]["FK_Column"].ToString().Replace("_Id", "_Name").Replace("_", "") + "" + "}\"    Width=\"" + dType + "\" Foreground=\"Black\" />\n";
                        //str += "                <sdk:DataGridTextColumn  Header=\"" + dsColumns.Tables[0].Rows[j]["COLUMN_NAME"].ToString().Replace("_", " ") + "\" Binding=\"{Binding " + dsFK.Tables[0].Rows[0]["PK_Table"].ToString().Replace("_", " ") + "_" + dsFK.Tables[0].Rows[0]["FK_Column"].ToString() + ".Name" + "}\"    Width=\"" + dType + "\" Foreground=\"Black\" />\n";
                    }
                }
            }

            str += "            </sdk:DataGrid.Columns>\n";
            str += "            <toolkit:ContextMenuService.ContextMenu>\n";
            str += "                <toolkit:ContextMenu>\n";

            str += "                    <toolkit:MenuItem Header=\"Add New\" Command=\"{Binding InsertCommand}\"  >\n";
            str += "                        <toolkit:MenuItem.Icon>\n";
            str += "                            <Image Width=\"16\" Height=\"16\" Source=\"/" + namesp + ";component/Images/Add.png\" />\n";
            str += "                        </toolkit:MenuItem.Icon>\n";
            str += "                    </toolkit:MenuItem>\n";
            str += "\n";


            
            str += "                    <toolkit:MenuItem Header=\"Edit\" Command=\"{Binding EditCommand}\"  >\n";
            str += "                        <toolkit:MenuItem.Icon>\n";
            str += "                            <Image Width=\"16\" Height=\"16\" Source=\"/" + namesp + ";component/Images/Edit.png\" />\n";
            str += "                        </toolkit:MenuItem.Icon>\n";
            str += "                    </toolkit:MenuItem>\n";
            str += "\n";


            
            str += "                    <toolkit:MenuItem Header=\"Delete\" Command=\"{Binding DeleteCommand}\"  >\n";
            str += "                        <toolkit:MenuItem.Icon>\n";
            str += "                            <Image Width=\"16\" Height=\"16\" Source=\"/" + namesp + ";component/Images/Delete.png\" />\n";
            str += "                        </toolkit:MenuItem.Icon>\n";
            str += "                    </toolkit:MenuItem>\n";
            str += "\n";


            str += "                    <toolkit:MenuItem Header=\"Refresh\" Command=\"{Binding RefreshCommand}\"  >\n";
            str += "                        <toolkit:MenuItem.Icon>\n";
            str += "                            <Image Width=\"16\" Height=\"16\" Source=\"/" + namesp + ";component/Images/Refresh.png\" />\n";
            str += "                        </toolkit:MenuItem.Icon>\n";
            str += "                    </toolkit:MenuItem>\n";
            str += "\n";


            str += "                </toolkit:ContextMenu>\n";
            str += "            </toolkit:ContextMenuService.ContextMenu>\n";
            str += "\n";


            str += "        </jib:JibGrid>\n";
            
            str += "        <sdk:DataPager x:Name=\"" + tableName + "Pager\" Source=\"{Binding Path=ItemsSource, ElementName=grd" + tableName + "}\" PageSize=\"20\" Grid.Row=\"3\"   Cursor=\"Hand\"   />\n";
            str += "        <toolkit:BusyIndicator Grid.Row=\"0\" Grid.RowSpan=\"4\" Name=\"busy" + tableName + "\" IsBusy=\"{Binding IsBusy,Mode=TwoWay}\"  />\n";
            //str += "        <sdk:DataPager HorizontalContentAlignment=\"Right\" x:Name=\"" + tableName + "Pager\" Source=\"{Binding Path=ItemsSource, ElementName=grd" + tableName + "}\" PageSize=\"20\" Grid.Row=\"0\"  Width=\"400\" Margin=\"5\" HorizontalAlignment=\"Right\" Height=\"25\" Padding=\"1\" DisplayMode=\"FirstLastPreviousNextNumeric\" NumericButtonCount=\"10\" Cursor=\"Hand\" />\n";
            str += "\n";
            str += "    </Grid>\n";
            str += "</navigation:Page>\n";



            return str;
        }
    }
}
