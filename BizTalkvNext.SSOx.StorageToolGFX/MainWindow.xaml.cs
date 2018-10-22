using BizTalkvNext.SSOx;
using Microsoft.BizTalk.SSOClient.Interop;
using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
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
        //Added private member
        DataGridView dgvKeyValue = new DataGridView();

        public MainWindow()
        {
            InitializeComponent();
            this.initializeFields();

            DataGridViewTextBoxColumn dgtbc = new DataGridViewTextBoxColumn();
            dgtbc.HeaderText = "KeyName";
            dgtbc.Name = "clKeyName";
            dgtbc.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvKeyValue.Columns.Add(dgtbc);

            dgtbc = new DataGridViewTextBoxColumn();
            dgtbc.HeaderText = "Value";
            dgtbc.Name = "clValue";
            dgtbc.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvKeyValue.Columns.Add(dgtbc);

            DataGridViewComboBoxColumn dgvc = new DataGridViewComboBoxColumn();
            dgvc.Items.Add("Yes");
            dgvc.Items.Add("No");
            dgvc.HeaderText = "Is Masked?";
            dgvc.Width = 80;
            dgvc.Name = "clIsMask";
            dgvKeyValue.Columns.Add(dgvc);
            dgvKeyValue.AutoResizeColumns();

            winformshost.Child = dgvKeyValue;
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
            System.Windows.Forms.OpenFileDialog ofdConfigFile = new System.Windows.Forms.OpenFileDialog();

            ofdConfigFile.Filter = "SSO Config File(*.xml)|*.xml";           
           
            if (ofdConfigFile.ShowDialog() != System.Windows.Forms.DialogResult.OK)
            {               
                return;
            }
            if (this.ImportConfiguration(ofdConfigFile.FileName))
            {
                int num1 = (int)System.Windows.Forms.MessageBox.Show("Configuration imported successfully.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
                int num2 = (int)System.Windows.Forms.MessageBox.Show("Import has been canceled.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
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
                //dgvKeyValue.Items.Add(new object[] { "Key", "Value", "Yes" });
                DataGridViewRow row = this.dgvKeyValue.Rows[this.dgvKeyValue.Rows.Add()];
                row.Cells["clKeyName"].Value = (object)selectNode.Attributes["label"].InnerText;
                row.Cells["clValue"].Value = (object)selectNode.InnerText;
                row.Cells["clIsMask"].Value = selectNode.Attributes["masked"].InnerText.ToLower() == "yes" ? (object)"Yes" : (object)"No";
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
            this.dgvKeyValue.Rows.Clear();
            this.cbAutoPopulateSSOGroups.IsChecked = false;
            foreach (DataGridViewRow row in (IEnumerable)this.dgvKeyValue.Rows)
            {
                row.Cells["clKeyName"].Value = (object)"";
                row.Cells["clValue"].Value = (object)"";
                row.Cells["clIsMask"].Value = (object)"Yes";
            }
        }

        private void MiExportSSOConfig_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.tbApplicationName.Text))
            {
                System.Windows.Forms.SaveFileDialog sfdConfigFile = new System.Windows.Forms.SaveFileDialog();

                sfdConfigFile.Filter = "SSO Config File(*.xml)|*.xml";
                sfdConfigFile.FileName = "SSO_" + this.tbApplicationName.Text + ".xml";

                if (sfdConfigFile.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                {                  
                    return;
                }

                if (this.ExportXmlConfiguration(sfdConfigFile.FileName))
                {
                    int num1 = (int)System.Windows.Forms.MessageBox.Show("Configuration exported successfully.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                else
                {
                    int num2 = (int)System.Windows.Forms.MessageBox.Show("Export has been canceled.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
            }
            else
            {
                int num = (int)System.Windows.Forms.MessageBox.Show("Please browse and select the application", "Information", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        private bool ExportXmlConfiguration(string path)
        {
            XmlTextWriter xmlTextWriter = new XmlTextWriter(path, Encoding.UTF8);
            xmlTextWriter.WriteStartDocument();
            xmlTextWriter.WriteStartElement("sso");
            xmlTextWriter.WriteStartElement("application");
            xmlTextWriter.WriteAttributeString("name", this.tbApplicationName.Text);
            xmlTextWriter.WriteElementString("description", this.tbApplicationDescription.Text);
            xmlTextWriter.WriteElementString("appUserAccount", this.tbSSOAffliateAdminGrp.Text);
            xmlTextWriter.WriteElementString("appAdminAccount", this.tbSSOAdminGrp.Text);
            int num = 0;
            ArrayList arrayList = new ArrayList();
            foreach (DataGridViewRow row in (IEnumerable)this.dgvKeyValue.Rows)
            {
                if (row.Cells["clKeyName"].Value != null && row.Cells["clKeyName"].Value.ToString() != string.Empty)
                {
                    xmlTextWriter.WriteStartElement("field");
                    xmlTextWriter.WriteAttributeString("ordinal", num.ToString());
                    xmlTextWriter.WriteAttributeString("label", row.Cells["clKeyName"].Value.ToString());
                    xmlTextWriter.WriteAttributeString("masked", row.Cells["clIsMask"].Value.ToString().Replace("yes", "Yes"));
                    xmlTextWriter.WriteString(row.Cells["clValue"].Value.ToString());
                    xmlTextWriter.WriteEndElement();
                    arrayList.Add((object)row.Cells[0].Value.ToString());
                    ++num;
                }
            }
            xmlTextWriter.WriteStartElement("flags");
            xmlTextWriter.WriteAttributeString("configStoreApp", "yes");
            xmlTextWriter.WriteAttributeString("allowLocalAccounts", "yes");
            xmlTextWriter.WriteAttributeString("enableApp", "yes");
            xmlTextWriter.WriteEndElement();
            xmlTextWriter.WriteEndElement();
            xmlTextWriter.WriteEndElement();
            xmlTextWriter.Flush();
            xmlTextWriter.Close();
            return true;
        }

        private bool ExportXmlConfigurationByApplication(string appName, string path, string guid)
        {
            string appUserAcct = "";
            string appAdminAcct = "";
            string description = "";
            string contactInfo = "";
            HybridDictionary hybridDictionary = new HybridDictionary();
            if (appName != string.Empty)
            {
                try
                {
                    HybridDictionary configProperties = SSOConfigManager.GetConfigProperties(appName, out description, out contactInfo, out appUserAcct, out appAdminAcct);
                    XmlTextWriter xmlTextWriter = new XmlTextWriter(path + "\\SSO_" + appName + "_" + guid + ".xml", Encoding.UTF8);
                    xmlTextWriter.WriteStartDocument();
                    xmlTextWriter.WriteStartElement("sso");
                    xmlTextWriter.WriteStartElement("application");
                    xmlTextWriter.WriteAttributeString("name", appName);
                    xmlTextWriter.WriteElementString("description", description);
                    xmlTextWriter.WriteElementString("appUserAccount", appAdminAcct);
                    xmlTextWriter.WriteElementString("appAdminAccount", appUserAcct);
                    int num = 0;
                    ArrayList arrayList = new ArrayList();
                    foreach (DictionaryEntry dictionaryEntry in configProperties)
                    {
                        xmlTextWriter.WriteStartElement("field");
                        xmlTextWriter.WriteAttributeString("ordinal", num.ToString());
                        xmlTextWriter.WriteAttributeString("label", dictionaryEntry.Key.ToString());
                        xmlTextWriter.WriteAttributeString("masked", "yes");
                        xmlTextWriter.WriteString(dictionaryEntry.Value.ToString());
                        xmlTextWriter.WriteEndElement();
                        arrayList.Add((object)dictionaryEntry.Value.ToString());
                        ++num;
                    }
                    xmlTextWriter.WriteStartElement("flags");
                    xmlTextWriter.WriteAttributeString("configStoreApp", "yes");
                    xmlTextWriter.WriteAttributeString("allowLocalAccounts", "yes");
                    xmlTextWriter.WriteAttributeString("enableApp", "yes");
                    xmlTextWriter.WriteEndElement();
                    xmlTextWriter.WriteEndElement();
                    xmlTextWriter.WriteEndElement();
                    xmlTextWriter.Flush();
                    xmlTextWriter.Close();
                    return true;
                }
                catch (Exception ex)
                {
                    int num = (int)System.Windows.Forms.MessageBox.Show("Error Occured. Details: " + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
            return false;
        }

        private void miAboutMe_Click(object sender, RoutedEventArgs e)
        {
            int num = (int)System.Windows.Forms.MessageBox.Show("For any issues and suggestions contact :\r\nmanjunathp@ymail.com\r\nThis tool is based on Microsoft.EnterpriseSingleSignOn.Interop 9.0.1000.0");
        }

        private void btRefresh_Click(object sender, RoutedEventArgs e)
        {
            this.cbAutoPopulateSSOGroups.IsChecked = false;
            this.tbSSOAdminGrp.Text = "";
            this.tbSSOAffliateAdminGrp.Text = "";
            if (!(this.tbApplicationName.Text != string.Empty))
                return;
            try
            {
                string description;
                string contactInfo;
                string appUserAcct;
                string appAdminAcct;
                HybridDictionary configProperties = SSOConfigManager.GetConfigProperties(this.tbApplicationName.Text, out description, out contactInfo, out appUserAcct, out appAdminAcct);
                this.tbApplicationDescription.Text = description;
                this.tbSSOAdminGrp.Text = appAdminAcct;
                this.tbSSOAffliateAdminGrp.Text = appUserAcct;
                this.dgvKeyValue.Rows.Clear();
                foreach (DictionaryEntry dictionaryEntry in configProperties)
                {
                    DataGridViewRow row = this.dgvKeyValue.Rows[this.dgvKeyValue.Rows.Add()];
                    row.Cells["clKeyName"].Value = (object)dictionaryEntry.Key.ToString();
                    row.Cells["clValue"].Value = (object)dictionaryEntry.Value.ToString();
                    row.Cells["clIsMask"].Value = (object)"Yes";
                }
            }
            catch (Exception ex)
            {
                int num = (int)System.Windows.Forms.MessageBox.Show("Error Occured. Details: " + ex.Message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void btCreate_Click(object sender, RoutedEventArgs e)
        {
            if (this.tbApplicationName.Text != string.Empty)
            {
                SSOPropBag ssoPropBag = new SSOPropBag();
                ArrayList maskArray = new ArrayList();
                foreach (DataGridViewRow row in (IEnumerable)this.dgvKeyValue.Rows)
                {
                    if (row.Cells["clKeyName"].Value != null && row.Cells["clKeyName"].Value.ToString() != string.Empty)
                    {
                        object ptrVar = row.Cells["clValue"].Value;
                        ssoPropBag.Write(row.Cells["clKeyName"].Value.ToString(), ref ptrVar);
                        if (row.Cells["clIsMask"].FormattedValue.ToString().ToLower() == "yes")
                            maskArray.Add((object)268435456);
                        else
                            maskArray.Add((object)0);
                    }
                }
                try
                {
                    SSOConfigManager.CreateConfigStoreApplication(this.tbApplicationName.Text, this.tbApplicationDescription.Text, this.tbSSOAffliateAdminGrp.Text, this.tbSSOAdminGrp.Text, ssoPropBag, maskArray);
                    SSOConfigManager.SetConfigProperties(this.tbApplicationName.Text, ssoPropBag);
                    int num = (int)System.Windows.Forms.MessageBox.Show("Application Successfully Created", "Information", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                catch (Exception ex)
                {
                    int num = (int)System.Windows.Forms.MessageBox.Show("Error Occured.  Details: " + ex.Message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
            else
            {
                int num1 = (int)System.Windows.Forms.MessageBox.Show("Application name is required.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void btModify_Click(object sender, RoutedEventArgs e)
        {
            SSOPropBag ssoPropBag = new SSOPropBag();
            ArrayList maskArray = new ArrayList();
            foreach (DataGridViewRow row in (IEnumerable)this.dgvKeyValue.Rows)
            {
                if (row.Cells["clKeyName"].Value != null && row.Cells["clKeyName"].Value.ToString() != string.Empty)
                {
                    string propName = row.Cells["clKeyName"].Value.ToString();
                    object ptrVar = (object)row.Cells["clValue"].Value.ToString();
                    ssoPropBag.Write(propName, ref ptrVar);
                    if (row.Cells["clIsMask"].FormattedValue.ToString().ToLower() == "yes")
                        maskArray.Add((object)268435456);
                    else
                        maskArray.Add((object)0);
                }
            }
            string str1 = "";
            string str2 = "";
            bool flag1 = false;
            string empty1 = string.Empty;
            string empty2 = string.Empty;
            string empty3 = string.Empty;
            string description;
            string contactInfo;
            string appUserAcct;
            string appAdminAcct;
            HybridDictionary configProperties = SSOConfigManager.GetConfigProperties(this.tbApplicationName.Text, out description, out contactInfo, out appUserAcct, out appAdminAcct);
            string str3 = string.Empty;
            if (description != this.tbApplicationDescription.Text)
                str3 = "Application description, ";
            else if (appUserAcct != this.tbSSOAffliateAdminGrp.Text)
                str3 += "SSO User Group, ";
            else if (appAdminAcct != this.tbSSOAdminGrp.Text)
                str3 += "SSO Admin Group";
            if (str3 != string.Empty)                
                str1 = System.Windows.Forms.MessageBox.Show("Are you sure wish to modify: " + str3, "Confirm", System.Windows.Forms.MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK ? "CHANGE" : "DONTCHANGE";
            if (str1 != "DONTCHANGE")
            {
                foreach (DictionaryEntry property in ssoPropBag.properties)
                {
                    bool flag2 = false;
                    bool flag3 = false;
                    foreach (DictionaryEntry dictionaryEntry in configProperties)
                    {
                        if (dictionaryEntry.Key.ToString() == property.Key.ToString())
                        {
                            flag2 = true;
                            if (dictionaryEntry.Value.ToString() == property.Value.ToString())
                                flag3 = true;
                        }
                    }
                    empty1 += !flag2 ? property.Key.ToString() + ", " : "";
                    empty3 += !flag3 ? property.Key.ToString() + ", " : "";
                }
                foreach (DictionaryEntry dictionaryEntry in configProperties)
                {
                    bool flag2 = false;
                    flag1 = false;
                    foreach (DictionaryEntry property in ssoPropBag.properties)
                    {
                        if (dictionaryEntry.Key.ToString() == property.Key.ToString())
                            flag2 = true;
                    }
                    empty2 += !flag2 ? dictionaryEntry.Key.ToString() + ", " : "";
                }
                string str4 = empty2.TrimEnd(',', ' ');
                string str5 = empty1.TrimEnd(',', ' ');
                string str6 = empty3.TrimEnd(',', ' ');
                if (str5 != string.Empty && str4 != string.Empty)
                    str2 = System.Windows.Forms.MessageBox.Show("Are you sure wish to Add field/s' (" + str5 + ") and Delete field/s' (" + str4 + ") to this application? Existing settings will be overwritten.", "Confirm", System.Windows.Forms.MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK ? "CHANGE" : "DONTCHANGE";
                else if (str5 != string.Empty)
                    str2 = System.Windows.Forms.MessageBox.Show("Are you sure wish to Add field/s' (" + str5 + ") to this application? Existing settings will be overwritten.", "Confirm", System.Windows.Forms.MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK ? "CHANGE" : "DONTCHANGE";
                else if (str4 != string.Empty)
                    str2 = System.Windows.Forms.MessageBox.Show("Are you sure wish to Delete field/s' (" + str4 + ") to this application? Existing settings will be overwritten.", "Confirm", System.Windows.Forms.MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK ? "CHANGE" : "DONTCHANGE";
                else if (str6 != string.Empty)
                    str2 = System.Windows.Forms.MessageBox.Show("Are you sure wish to Update field/s' (" + str6 + ") to this application? Existing settings will be overwritten.", "Confirm", System.Windows.Forms.MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK ? "CHANGE" : "DONTCHANGE";
                try
                {
                    if ((str2 == "CHANGE" || str1 == "CHANGE") && str2 != "DONTCHANGE")
                    {
                        SSOConfigManager.DeleteApplication(this.tbApplicationName.Text);
                        SSOConfigManager.CreateConfigStoreApplication(this.tbApplicationName.Text, this.tbApplicationDescription.Text, this.tbSSOAffliateAdminGrp.Text, this.tbSSOAdminGrp.Text,  ssoPropBag, maskArray);
                        SSOConfigManager.SetConfigProperties(this.tbApplicationName.Text, ssoPropBag);
                        if (str6 != string.Empty && str1 == "")
                        {
                            int num1 = (int)System.Windows.Forms.MessageBox.Show("Only field values are updated.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        }
                        else if (str6 != string.Empty && str1 == "CHANGE")
                        {
                            int num2 = (int)System.Windows.Forms.MessageBox.Show("User details and Only field values are updated.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        }
                        else
                        {
                            int num3 = (int)System.Windows.Forms.MessageBox.Show("Application successfully modified.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        }
                    }
                    else
                    {
                        int num = (int)System.Windows.Forms.MessageBox.Show("No field values are updated.", "Exclamation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
                catch (Exception ex)
                {
                    int num = (int)System.Windows.Forms.MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
            else
            {
                int num4 = (int)System.Windows.Forms.MessageBox.Show("No field values are updated.", "Exclamation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void btDelete_Click(object sender, RoutedEventArgs e)
        {
            if (this.tbApplicationName.Text != null || this.tbApplicationName.Text != string.Empty)
            {
                if (System.Windows.Forms.MessageBox.Show("Are you sure you wish to delete this application? All its settings will be lost.", "Warning", System.Windows.Forms.MessageBoxButtons.YesNoCancel, System.Windows.Forms.MessageBoxIcon.Exclamation) != System.Windows.Forms.DialogResult.Yes)
                    return;
                try
                {
                    SSOConfigManager.DeleteApplication(this.tbApplicationName.Text);
                    int num = (int)System.Windows.Forms.MessageBox.Show("Application deleted.", "Information", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Asterisk);
                }
                catch (Exception ex)
                {
                    int num = (int)System.Windows.Forms.MessageBox.Show("Error Occured.  Details: " + ex.ToString(), "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Hand);
                }
            }
            else
            {
                int num1 = (int)System.Windows.Forms.MessageBox.Show("Please enter an application name for deletion.", "Warning", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
            }
        }
    }
}
