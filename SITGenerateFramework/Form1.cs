using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SITGenerateFramework
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            Entites en = new Entites();
            en.generateEntities(txtConnectStr.Text, txtOutputDir.Text, txtNamespace.Text);
          
            
        }

        private void btnGenerateRepositories_Click(object sender, EventArgs e)
        {
            Repository rep = new Repository();
            rep.generateRepositories(txtConnectStr.Text, txtOutputDir.Text, txtNamespace.Text);
        }

        private void btnStoredProcedures_Click(object sender, EventArgs e)
        {
            StoredProcedures sp = new StoredProcedures();
            sp.generateStoredProcedures(txtConnectStr.Text, txtOutputDir.Text, txtNamespace.Text);
        }

        private void btnGenerateDomainServices_Click(object sender, EventArgs e)
        {
            DomainServices ds = new DomainServices();
            ds.generateDomainServices(txtConnectStr.Text, txtOutputDir.Text, txtNamespace.Text);
        }

        private void btnViewModel_Click(object sender, EventArgs e)
        {
            ViewModel vm = new ViewModel();
            vm.generateViewModel(txtConnectStr.Text, txtOutputDir.Text, txtNamespace.Text);
        }

        private void btnViews_Click(object sender, EventArgs e)
        {
            View v = new View();
            v.generateView(txtConnectStr.Text, txtOutputDir.Text, txtNamespace.Text);
        }

        private void btnReports_Click(object sender, EventArgs e)
        {
            Reports rp = new Reports();
            rp.generateReports(txtConnectStr.Text, txtOutputDir.Text, txtNamespace.Text);
        }

        private void btnPartialEntities_Click(object sender, EventArgs e)
        {
            Entites en = new Entites();
            en.generatepartialEntities(txtConnectStr.Text, txtOutputDir.Text, txtNamespace.Text);
        }

        private void btnCreateDir_Click(object sender, EventArgs e)
        {
            CreateDir en = new CreateDir();
            en.generateDir(txtConnectStr.Text, txtOutputDir.Text, txtNamespace.Text);
        }
    }
}
