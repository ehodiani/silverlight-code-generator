namespace SITGenerateFramework
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.txtConnectStr = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtOutputDir = new System.Windows.Forms.TextBox();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.txtNamespace = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnGenerateRepositories = new System.Windows.Forms.Button();
            this.btnStoredProcedures = new System.Windows.Forms.Button();
            this.btnGenerateDomainServices = new System.Windows.Forms.Button();
            this.btnViewModel = new System.Windows.Forms.Button();
            this.btnViews = new System.Windows.Forms.Button();
            this.btnReports = new System.Windows.Forms.Button();
            this.btnPartialEntities = new System.Windows.Forms.Button();
            this.btnCreateDir = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(28, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Connection String:";
            // 
            // txtConnectStr
            // 
            this.txtConnectStr.Location = new System.Drawing.Point(130, 37);
            this.txtConnectStr.Name = "txtConnectStr";
            this.txtConnectStr.Size = new System.Drawing.Size(591, 20);
            this.txtConnectStr.TabIndex = 1;
            this.txtConnectStr.Text = "Data Source=localhost\\sql2008;Initial Catalog=Logistics;Persist Security Info=Tru" +
    "e;User ID=sa;Password=aiads2000";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(28, 77);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Output Dir:";
            // 
            // txtOutputDir
            // 
            this.txtOutputDir.Location = new System.Drawing.Point(130, 77);
            this.txtOutputDir.Name = "txtOutputDir";
            this.txtOutputDir.Size = new System.Drawing.Size(591, 20);
            this.txtOutputDir.TabIndex = 3;
            this.txtOutputDir.Text = "D:\\My Projects\\Silverlight\\Logistics\\Framework";
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(650, 129);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(71, 23);
            this.btnGenerate.TabIndex = 4;
            this.btnGenerate.Text = "Entities";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // txtNamespace
            // 
            this.txtNamespace.Location = new System.Drawing.Point(130, 103);
            this.txtNamespace.Name = "txtNamespace";
            this.txtNamespace.Size = new System.Drawing.Size(591, 20);
            this.txtNamespace.TabIndex = 6;
            this.txtNamespace.Text = "Logistics";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(28, 103);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Namespace:";
            // 
            // btnGenerateRepositories
            // 
            this.btnGenerateRepositories.Location = new System.Drawing.Point(552, 129);
            this.btnGenerateRepositories.Name = "btnGenerateRepositories";
            this.btnGenerateRepositories.Size = new System.Drawing.Size(92, 23);
            this.btnGenerateRepositories.TabIndex = 7;
            this.btnGenerateRepositories.Text = "Repositories";
            this.btnGenerateRepositories.UseVisualStyleBackColor = true;
            this.btnGenerateRepositories.Click += new System.EventHandler(this.btnGenerateRepositories_Click);
            // 
            // btnStoredProcedures
            // 
            this.btnStoredProcedures.Location = new System.Drawing.Point(436, 129);
            this.btnStoredProcedures.Name = "btnStoredProcedures";
            this.btnStoredProcedures.Size = new System.Drawing.Size(110, 23);
            this.btnStoredProcedures.TabIndex = 8;
            this.btnStoredProcedures.Text = "Stored Procedures";
            this.btnStoredProcedures.UseVisualStyleBackColor = true;
            this.btnStoredProcedures.Click += new System.EventHandler(this.btnStoredProcedures_Click);
            // 
            // btnGenerateDomainServices
            // 
            this.btnGenerateDomainServices.Location = new System.Drawing.Point(329, 129);
            this.btnGenerateDomainServices.Name = "btnGenerateDomainServices";
            this.btnGenerateDomainServices.Size = new System.Drawing.Size(101, 23);
            this.btnGenerateDomainServices.TabIndex = 9;
            this.btnGenerateDomainServices.Text = "Domain Services";
            this.btnGenerateDomainServices.UseVisualStyleBackColor = true;
            this.btnGenerateDomainServices.Click += new System.EventHandler(this.btnGenerateDomainServices_Click);
            // 
            // btnViewModel
            // 
            this.btnViewModel.Location = new System.Drawing.Point(234, 129);
            this.btnViewModel.Name = "btnViewModel";
            this.btnViewModel.Size = new System.Drawing.Size(89, 23);
            this.btnViewModel.TabIndex = 10;
            this.btnViewModel.Text = "View Model";
            this.btnViewModel.UseVisualStyleBackColor = true;
            this.btnViewModel.Click += new System.EventHandler(this.btnViewModel_Click);
            // 
            // btnViews
            // 
            this.btnViews.Location = new System.Drawing.Point(166, 129);
            this.btnViews.Name = "btnViews";
            this.btnViews.Size = new System.Drawing.Size(62, 23);
            this.btnViews.TabIndex = 11;
            this.btnViews.Text = "View";
            this.btnViews.UseVisualStyleBackColor = true;
            this.btnViews.Click += new System.EventHandler(this.btnViews_Click);
            // 
            // btnReports
            // 
            this.btnReports.Location = new System.Drawing.Point(166, 158);
            this.btnReports.Name = "btnReports";
            this.btnReports.Size = new System.Drawing.Size(62, 23);
            this.btnReports.TabIndex = 12;
            this.btnReports.Text = "Reports";
            this.btnReports.UseVisualStyleBackColor = true;
            this.btnReports.Click += new System.EventHandler(this.btnReports_Click);
            // 
            // btnPartialEntities
            // 
            this.btnPartialEntities.Location = new System.Drawing.Point(234, 158);
            this.btnPartialEntities.Name = "btnPartialEntities";
            this.btnPartialEntities.Size = new System.Drawing.Size(196, 23);
            this.btnPartialEntities.TabIndex = 13;
            this.btnPartialEntities.Text = "Partial Entities";
            this.btnPartialEntities.UseVisualStyleBackColor = true;
            this.btnPartialEntities.Click += new System.EventHandler(this.btnPartialEntities_Click);
            // 
            // btnCreateDir
            // 
            this.btnCreateDir.Location = new System.Drawing.Point(525, 158);
            this.btnCreateDir.Name = "btnCreateDir";
            this.btnCreateDir.Size = new System.Drawing.Size(196, 23);
            this.btnCreateDir.TabIndex = 14;
            this.btnCreateDir.Text = "Create Dir";
            this.btnCreateDir.UseVisualStyleBackColor = true;
            this.btnCreateDir.Click += new System.EventHandler(this.btnCreateDir_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(753, 206);
            this.Controls.Add(this.btnCreateDir);
            this.Controls.Add(this.btnPartialEntities);
            this.Controls.Add(this.btnReports);
            this.Controls.Add(this.btnViews);
            this.Controls.Add(this.btnViewModel);
            this.Controls.Add(this.btnGenerateDomainServices);
            this.Controls.Add(this.btnStoredProcedures);
            this.Controls.Add(this.btnGenerateRepositories);
            this.Controls.Add(this.txtNamespace);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.txtOutputDir);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtConnectStr);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtConnectStr;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtOutputDir;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.TextBox txtNamespace;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnGenerateRepositories;
        private System.Windows.Forms.Button btnStoredProcedures;
        private System.Windows.Forms.Button btnGenerateDomainServices;
        private System.Windows.Forms.Button btnViewModel;
        private System.Windows.Forms.Button btnViews;
        private System.Windows.Forms.Button btnReports;
        private System.Windows.Forms.Button btnPartialEntities;
        private System.Windows.Forms.Button btnCreateDir;
    }
}

