namespace WebExStatsSvc
{
    partial class ProjectInstaller
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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SvcProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.SvcInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // SvcProcessInstaller
            // 
            this.SvcProcessInstaller.Password = null;
            this.SvcProcessInstaller.Username = null;
            // 
            // SvcInstaller
            // 
            this.SvcInstaller.Description = "WebEx Statistic Service to call Api";
            this.SvcInstaller.DisplayName = "WebEx Stats";
            this.SvcInstaller.ServiceName = "DataSvc";
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.SvcProcessInstaller,
            this.SvcInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller SvcProcessInstaller;
        private System.ServiceProcess.ServiceInstaller SvcInstaller;
    }
}