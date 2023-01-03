using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;
using System.Globalization;

namespace ClassLibrary_BPC.hrfocus.utility
{
    public class cls_StyleDataGridView
    {

        private static DataGridTableStyle TableStyleTemp;
        private static string HearderText = string.Empty;

        public void setHeader(string mapping_name, string header_name, int width, bool visible)
        {
            HearderText = header_name;
            TableStyleTemp = new DataGridTableStyle();
            TableStyleTemp.AlternatingBackColor = Color.Lavender;
            TableStyleTemp.BackColor = Color.White;
            TableStyleTemp.MappingName = mapping_name;
            TableStyleTemp.RowHeadersVisible = visible;
            TableStyleTemp.RowHeaderWidth = width;
        }


        public void addColumnStyles(string field_name, string text, int width, HorizontalAlignment align)
        {
            DataGridTextBoxColumn obj_cs = new DataGridTextBoxColumn();

            obj_cs.MappingName = field_name;
            obj_cs.HeaderText = text;
            obj_cs.Width = width;
            obj_cs.Alignment = align;
            if (field_name.EndsWith("Day") || field_name.EndsWith("Date") || field_name.EndsWith("Birth") || field_name.StartsWith("Date") || field_name.StartsWith("Day"))
                obj_cs.Format = System.String.Format("dd MMM yyyy", obj_cs.TextBox);
            TableStyleTemp.GridColumnStyles.Add(obj_cs);
        }

        public void addColumnStyles(string field_name, string text, int width, HorizontalAlignment align, string field_type, string field_format)
        {
            DataGridTextBoxColumn obj_cs = new DataGridTextBoxColumn();

            obj_cs.MappingName = field_name;
            obj_cs.HeaderText = text;
            obj_cs.Width = width;
            obj_cs.Alignment = align;
            switch (field_type)
            {
                case ("System.DateTime"):
                    obj_cs.Format = System.String.Format(field_format, obj_cs.TextBox);
                    break;
                case ("DateTimeEng"):
                    obj_cs.Format = System.String.Format(field_format, obj_cs.TextBox);
                    obj_cs.FormatInfo = DateTimeFormatInfo.InvariantInfo;
                    break;
                case ("System.Decimal"):
                    obj_cs.Format = System.String.Format(field_format, obj_cs.TextBox);
                    break;
                case ("System.Single"):
                    obj_cs.Format = System.String.Format(field_format, obj_cs.TextBox);
                    break;
                case ("System.Int32"):
                    obj_cs.Format = System.String.Format(field_format, obj_cs.TextBox);
                    break;
                case ("System.Int64"):
                    obj_cs.Format = System.String.Format(field_format, obj_cs.TextBox);
                    break;
            }
            TableStyleTemp.GridColumnStyles.Add(obj_cs);
        }

        public void addColumnStylesDateTime(string field_name, string text, int width, HorizontalAlignment align)
        {
            DataGridTextBoxColumn obj_cs = new DataGridTextBoxColumn();

            obj_cs.MappingName = field_name;
            obj_cs.HeaderText = text;
            obj_cs.Width = width;
            obj_cs.Alignment = align;
            obj_cs.Format = System.String.Format("dd/MM/yyyy", obj_cs.TextBox);
            obj_cs.FormatInfo = DateTimeFormatInfo.InvariantInfo;
            TableStyleTemp.GridColumnStyles.Add(obj_cs);
        }

        
        public void showDataGrid(ref DataGrid datagrid, bool allow_sort)
        {
            TableStyleTemp.AllowSorting = allow_sort;

            datagrid.TableStyles.Clear();
            datagrid.CaptionFont = new Font("Tahoma", 10, FontStyle.Regular);
            datagrid.CaptionText = HearderText;
            datagrid.FlatMode = true;
            datagrid.TableStyles.Add(TableStyleTemp);
        }

        public DataGridTableStyle getTableStyle
        {
            get { return TableStyleTemp; }
        }


        public void setColumnStyle(ref DataGridView dgv, string filed_name, string text, string align, int width)
        {
            switch (align)
            {
                case "left": dgv.Columns[filed_name].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft; break;
                case "center": dgv.Columns[filed_name].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter; break;
                case "right": dgv.Columns[filed_name].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight; break;
            }

            if (width == 0)
                dgv.Columns[filed_name].Visible = false;

            dgv.Columns[filed_name].Width = width;
            dgv.Columns[filed_name].HeaderText = text;
            dgv.Columns[filed_name].ReadOnly = true;       
            
        }
    }
}
