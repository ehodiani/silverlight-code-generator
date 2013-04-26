using System;
namespace SITGenerateFramework
{
    interface IViewModel
    {
        void generateViewModel(string constr, string outputDir, string namesp);
        string getclass(System.Data.DataSet dsTableDefenation, string TableName);
        string getCommands(System.Data.DataSet dsTableDefenation, string TableName);
        string getHeader(string namespaces, string TableName);
        string getMethods(System.Data.DataSet dsTableDefenation, string TableName);
        string getMethods1(System.Data.DataSet dsTableDefenation, string TableName);
        string getProperties(System.Data.DataSet dsTableDefenation, string TableName);
        string getUsingSection(string namesp);
    }
}
