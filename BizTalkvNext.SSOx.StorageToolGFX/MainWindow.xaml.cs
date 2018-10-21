using Microsoft.BizTalk.SSOClient.Interop;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;

namespace BizTalkvNext.SSOxStorageToolGFX
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void cbAutoPopulateSSOGroups_Checked(object sender, RoutedEventArgs e)
        {
            if (!this.cbAutoPopulateSSOGroups.IsChecked.Value)
                return;
            int flags;
            int auditAppDeleteMax;
            int auditMappingDeleteMax;
            int auditNtpLookupMax;
            int auditXpLookupMax;
            int ticketTimeout;
            int credCacheTimeout;
            string secretServer;
            string SSOAdminGroup;
            string affiliateAppMgrGroup;
            ((ISSOAdmin)new SSOAdmin()).GetGlobalInfo(out flags, out auditAppDeleteMax, out auditMappingDeleteMax, out auditNtpLookupMax, out auditXpLookupMax, out ticketTimeout, out credCacheTimeout, out secretServer, out SSOAdminGroup, out affiliateAppMgrGroup);
            this.tbSSOAdminGrp.Text = SSOAdminGroup;
            this.tbSSOAffliateAdminGrp.Text = affiliateAppMgrGroup;
        }

        private void miExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void miImportSSOConfig_Click(object sender, RoutedEventArgs e)
        {
            //THis is not present in WPF
            OpenFileDialog ofdConfigFile = new OpenFileDialog();

            ofdConfigFile.Filter = "SSO Config File(*.xml)|*.xml";

            //ofdConfigFile.

            ofdConfigFile.ShowDialog();
            //TODO - Verify Dialogresult
            if(DialogResult.HasValue && DialogResult.Value)
            {
                //TODO
                //if(DialogResult.Value != "OK")
                return;
            }
            if (this.ImportConfiguration(ofdConfigFile.FileName))
            {
                int num1 = (int)MessageBox.Show("Configuration imported successfully.", "Information", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
            else
            {
                int num2 = (int)MessageBox.Show("Import has been canceled.", "Information", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
        }

        private void returnDialogResult(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private bool ImportConfiguration(string path)
        {
            this.miClearALl_Click((object)null, (RoutedEventArgs)null);
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(path);
            this.tbApplicationName.Text = xmlDocument.SelectSingleNode("//application/@name").InnerText;
            this.tbApplicationDescription.Text = xmlDocument.SelectSingleNode("//description").InnerText;
            this.tbSSOAffliateAdminGrp.Text = xmlDocument.SelectSingleNode("//appUserAccount").InnerText;
            this.tbSSOAdminGrp.Text = xmlDocument.SelectSingleNode("//appAdminAccount").InnerText;
            foreach (XmlNode selectNode in xmlDocument.SelectNodes("//field"))
            {
                dgvKeyValue.Items.Add(new object[] { "Key", "Value", "Yes" });
                //DataGridViewRow row = this.dgvKeyValue.Rows[this.dgvKeyValue.Rows.Add()];
                //row.Cells["clKeyName"].Value = (object)selectNode.Attributes["label"].InnerText;
                //row.Cells["clValue"].Value = (object)selectNode.InnerText;
                //row.Cells["clIsMask"].Value = selectNode.Attributes["masked"].InnerText.ToLower() == "yes" ? (object)"Yes" : (object)"No";
            }
            return true;
        }

        private void miClearALl_Click(object sender, RoutedEventArgs e)
        {
            this.initializeFields();
        }
        private void initializeFields()
        {
            this.tbApplicationName.Text = "";
            this.tbApplicationDescription.Text = "";
            this.tbSSOAdminGrp.Text = "";
            this.tbSSOAffliateAdminGrp.Text = "";
            //this.dgvKeyValue.Rows.Clear();
            this.cbAutoPopulateSSOGroups.IsChecked = false;
            //foreach (DataGridViewRow row in (IEnumerable)this.dgvKeyValue.Rows)
            //{
            //    row.Cells["clKeyName"].Value = (object)"";
            //    row.Cells["clValue"].Value = (object)"";
            //    row.Cells["clIsMask"].Value = (object)"Yes";
            //}
        }
    }
}
