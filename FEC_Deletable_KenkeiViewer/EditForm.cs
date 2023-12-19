using FEC_Michiten_ClassLibrary.Models;
using FEC_Michiten_ClassLibrary.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FEC_Deletable_KenkeiViewer
{
    public partial class EditForm : Form
    {
        List<SignItem> items = new List<SignItem>();

        public delegate void SubmitEventHandler(string label);
        public event SubmitEventHandler SubmitEvent;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="items"></param>
        /// <param name="label"></param>
        public EditForm(List<SignItem> items, string label)
        {
            InitializeComponent();

            this.items = items;

            if (!string.IsNullOrEmpty(label))
                tbLabel.Text = label;

            this.TopMost = true;
            this.ActiveControl = tbLabel;
            tbLabel.SelectAll();
        }

        public string tbValue;

        /// <summary>
        /// 確定ボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btSubmit_Click(object sender, EventArgs e)
        {
            // 禁則文字チェック
            if (!UtilFunc.CheckFileNameChars(tbLabel.Text))
            {
                UtilFunc.ErrMsg("ファイル名に使用できない文字が含まれています。");
                return;
            }

            // 重複チェック
            var checkNo = items.Where(x => x.No.Equals(tbLabel.Text));
            if (checkNo.Count() != 0)
            {
                UtilFunc.ErrMsg("入力されたラベルは使用されています。");
                return;
            }

            tbValue  = tbLabel.Text;
            DialogResult = DialogResult.OK;
            this.Close();
        }

        /// <summary>
        /// キャンセルボタン押下 -> ラベル指定Dlg閉じる
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Enter押下で確定ボタンの動作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btSubmit_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Return) 
            {
                btSubmit_Click(sender, e);
            }
        }
    }
}
