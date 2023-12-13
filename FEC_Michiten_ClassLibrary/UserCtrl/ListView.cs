using FEC_Michiten_ClassLibrary.Models;
using FEC_Michiten_ClassLibrary.Pairs;
using FEC_Michiten_ClassLibrary.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FEC_Michiten_ClassLibrary.UserCtrl
{
    public partial class ListView : UserControl
    {
        public ListView()
        {
            InitializeComponent();
            
        }

        public DataGridViewColumnCollection DgvColmuns
        {
            get
            {
                return dgvItemList.Columns;
            }
            
        }

        public event EventHandler ListSelectChanged;
        public event DataGridViewCellEventHandler ListcCellClickEvent;
        public event DataGridViewCellEventHandler ListcCellDClickEvent;

        public delegate void ListCellEndEditHandler(SignItem item, string value);
        public event ListCellEndEditHandler ListCellEndEditEvent;

        public void Init(List<SignItem> pairs, string rootDir)
        {
            listFunc = new ListFunc(dgvItemList, rootDir);

            listFunc.UpdateList(pairs, Define.PairsMode.Lot);

            dispList = pairs;

            if (pairs.Count > 0)
                this.SetSelectedItem(pairs[0]);
        }

        private List<SignItem> dispList;
        private ListFunc listFunc;

        public SignItem GetSelectedItem()
        {

            return listFunc.GetSelectedItem(dispList);

        }

        public void SetSelectedItem(SignItem item)
        {
            int idx = listFunc.GetRowIndex(item.No);

            if(idx != -1)
            {
                dgvItemList.Rows[idx].Selected= true;
            }
            listFunc.Scroll();
        }

        private void dgvItemList_SelectionChanged(object sender, EventArgs e)
        {
            ListSelectChanged?.Invoke(sender, e);
        }

        private void dgvItemList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            ListcCellClickEvent?.Invoke(sender, e);
        }

        private void dgvItemList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            ListcCellDClickEvent?.Invoke(sender, e);
        }

        private void dgvItemList_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {

            SignItem item = listFunc.GetSelectedItem(dispList);

            string value = string.Empty;
            if(dgvItemList.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
            {
                value = dgvItemList.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
            }
            ListCellEndEditEvent?.Invoke(item, value);

        }
    }
}
