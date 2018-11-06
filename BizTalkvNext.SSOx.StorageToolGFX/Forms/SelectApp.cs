// Decompiled with JetBrains decompiler
// Type: BizTalk.SSO.SelectApp
// Assembly: BizTalk.SSOConfigStoreTool, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D651EB7A-4D45-41E7-906A-9969F573FBD1
// Assembly location: C:\Users\Administrator\Desktop\SSO ToolV2.0beta\SSO ToolV2.0beta\BizTalk.SSOConfigStoreTool.exe

using BizTalkvNext.SSOx;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace BizTalkvNext.SSOxStorageToolGFX
{
  public class SelectApp : Form
  {
    private IDictionary<string, string> apps;
    private IContainer components;
    private ListBox listboxApplications;
    private Button btnOk;
    private Button btnCancel;

    public string ApplicationName
    {
      get
      {
        return this.listboxApplications.SelectedItem.ToString();
      }
    }

    public string ApplicationDescription
    {
      get
      {
        return this.apps[this.listboxApplications.SelectedItem.ToString()];
      }
    }

    public ListBox.SelectedObjectCollection AppNamesList
    {
      get
      {
        return this.listboxApplications.SelectedItems;
      }
    }

    public SelectionMode SetListBoxSelectMode
    {
      set
      {
        this.listboxApplications.SelectionMode = value;
      }
    }

    public SelectApp()
    {
      this.InitializeComponent();
    }

    private void SelectApp_Load(object sender, EventArgs e)
    {
      this.apps = SSOConfigManager.GetApplications();
      if (this.apps.Count - this.apps.Where<KeyValuePair<string, string>>((Func<KeyValuePair<string, string>, bool>) (x => x.Key.StartsWith("{"))).Count<KeyValuePair<string, string>>() == 0)
      {
        this.btnOk.Enabled = false;
        this.listboxApplications.Items.Add((object) "No applications found.");
      }
      else
      {
        this.listboxApplications.BeginUpdate();
        foreach (string key in (IEnumerable<string>) this.apps.Keys)
        {
          if (!key.StartsWith("{"))
            this.listboxApplications.Items.Add((object) key);
        }
        this.listboxApplications.EndUpdate();
      }
    }

    private void listboxApplications_DoubleClick(object sender, EventArgs e)
    {
      if (this.listboxApplications.IndexFromPoint(this.listboxApplications.PointToClient(Cursor.Position)) == -1)
        return;
      this.DialogResult = DialogResult.OK;
      this.Close();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.listboxApplications = new ListBox();
      this.btnOk = new Button();
      this.btnCancel = new Button();
      this.SuspendLayout();
      this.listboxApplications.FormattingEnabled = true;
      this.listboxApplications.Location = new Point(13, 13);
      this.listboxApplications.Name = "listboxApplications";
      this.listboxApplications.Size = new Size(267, 160);
      this.listboxApplications.TabIndex = 0;
      this.btnOk.DialogResult = DialogResult.OK;
      this.btnOk.Location = new Point(13, 194);
      this.btnOk.Name = "btnOk";
      this.btnOk.Size = new Size(75, 23);
      this.btnOk.TabIndex = 1;
      this.btnOk.Text = "Ok";
      this.btnOk.UseVisualStyleBackColor = true;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(204, 193);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 2;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(292, 229);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnOk);
      this.Controls.Add((Control) this.listboxApplications);
      this.FormBorderStyle = FormBorderStyle.FixedSingle;
      this.Name = "SelectApp";
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Select Application";
      this.Load += new EventHandler(this.SelectApp_Load);
      this.ResumeLayout(false);
    }
  }
}
