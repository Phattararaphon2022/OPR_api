using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Globalization;
using System.IO; 
using System.Text;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;

namespace HRFocus.Inquiry
{
	/// <summary>
	/// Summary description for ExportData.
	/// </summary>
	public class frmExportData : System.Windows.Forms.Form
	{
		private DataGrid dgd;
		private string strPath;
		private int ColumnCount = 0;
		private int RowCount = 0;
		private StreamWriter sw;
		private int CountTime = 0;
		private bool FirstRow = false;

		private Thread trdExport;

		private System.Windows.Forms.ProgressBar progressBar1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label labPath;
		private System.Windows.Forms.Timer timer1;
		private System.ComponentModel.IContainer components;

		public frmExportData()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(frmExportData));
			this.progressBar1 = new System.Windows.Forms.ProgressBar();
			this.label1 = new System.Windows.Forms.Label();
			this.labPath = new System.Windows.Forms.Label();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.SuspendLayout();
			// 
			// progressBar1
			// 
			this.progressBar1.Location = new System.Drawing.Point(8, 68);
			this.progressBar1.Name = "progressBar1";
			this.progressBar1.Size = new System.Drawing.Size(356, 20);
			this.progressBar1.TabIndex = 258;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(136, 20);
			this.label1.TabIndex = 259;
			this.label1.Text = "Please wait...";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// labPath
			// 
			this.labPath.Location = new System.Drawing.Point(8, 32);
			this.labPath.Name = "labPath";
			this.labPath.Size = new System.Drawing.Size(356, 28);
			this.labPath.TabIndex = 260;
			this.labPath.Text = "D:\\Documents and Settings\\Administrator\\Desktop\\Document_HRM";
			this.labPath.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// timer1
			// 
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// frmExportData
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(225)), ((System.Byte)(230)), ((System.Byte)(255)));
			this.ClientSize = new System.Drawing.Size(370, 96);
			this.Controls.Add(this.labPath);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.progressBar1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmExportData";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "IN-0003 Export Data";
			this.Load += new System.EventHandler(this.frmExportData_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void frmExportData_Load(object sender, System.EventArgs e)
		{
			CountTime = 0;
			timer1.Enabled = true;			
		}

		public void SetExportData(DataGrid pDataGrid, int pColumnCount,  int pRowCount, bool pFirstRow, string pPath)
		{
			dgd = pDataGrid;				// DataGrid
			strPath = pPath;				// Path File
			ColumnCount = pColumnCount;		// จำนวน Column
			RowCount = pRowCount;		// จำนวน Row
			FirstRow = pFirstRow;
			
			labPath.Text = strPath;
		}

		public void ExportData()
		{
			this.Cursor = Cursors.WaitCursor;

			string strMss = "";
			string strFile = strPath;
			int intDotStart = strFile.IndexOf(".");

			++intDotStart;
			strFile = strFile.Substring(intDotStart,strFile.Length-intDotStart);
			strFile = strFile.ToLower();

			try
			{
				switch (strFile)
				{
					case "txt": 
						ExportTXT();
						break;
					case "csv":
						ExportCSV();
						break;
					case "xls":
						ExportXLS();
						break;
					default : 
						strFile = "Error";
						break;
				}

				if (strFile == "Error")
				{
					//if (Initial.Language == "T")
					//	strMss = "คุณไม่สามารถโอนข้อมูล !";
					//else
						strMss = "You can't export data !";
				}
				else
				{
					//if (Initial.Language == "T")
					//	strMss = "ทำการโอนข้อมูล เรียบร้อยแล้ว !";
					//else
						strMss = "Export data complete !";
				}

				MessageBox.Show(strMss,"HR-Focus",MessageBoxButtons.OK,MessageBoxIcon.Information);
			}
			catch (Exception ep)
			{
				//if (Initial.Language == "T")
				//	strMss = "คุณไม่สามารถโอนข้อมูล !";
				//else
					strMss = "You can't export data !";

				MessageBox.Show(ep.ToString(),"HR-Focus",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
				MessageBox.Show(strMss,"HR-Focus",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
			}
			finally
			{
				this.Cursor = Cursors.Default;
				this.Close();
			}
		}

		private void ExportTXT()
		{
			sw = new StreamWriter(strPath, false, System.Text.Encoding.UTF8);

			int iColCount = ColumnCount;
			string strData = string.Empty;
			string td = Convert.ToChar(34).ToString();		//	"
			string tu = Convert.ToChar(44).ToString();		//  ,
			string en = Convert.ToChar(13).ToString();		//  Enter

			progressBar1.Maximum = RowCount;
			progressBar1.Value = 0;

			if (FirstRow)
			{
				foreach(DataGridTableStyle TableStyle in dgd.TableStyles)
				{
					int k = 0;
					strData = string.Empty;
					foreach(DataGridColumnStyle ColumnStyle in TableStyle.GridColumnStyles)
					{
						strData += td+ ColumnStyle.MappingName +td;
						if ( k < iColCount - 1)
							strData += tu;
						++k;
					}	
					sw.Write(strData);
					sw.Write(sw.NewLine);
				}
			}

			for(int i=0; i < RowCount; i++) 
			{
				progressBar1.Value = i+1;

				strData = string.Empty;
				for(int j=0; j < ColumnCount; j++) 
				{
					if (!Convert.IsDBNull(dgd[i,j]))
						strData += td+ dgd[i,j].ToString().TrimEnd() +td;
					else
						strData += td+""+td;
					
					if ( j < iColCount - 1)
						strData += tu;
				}
				sw.Write(strData);

				if (i < RowCount-1)		
					sw.Write(sw.NewLine);
			}

			sw.Close();
		}

		private void ExportCSV()
		{
			sw = new StreamWriter(strPath, false, System.Text.Encoding.GetEncoding("ISO-8859-11"));

			int iColCount = ColumnCount;

			string strData = string.Empty;
			string td = Convert.ToChar(34).ToString();		//	"
			string tu = Convert.ToChar(44).ToString();		//  ,
			string en = Convert.ToChar(13).ToString();		//  Enter

			progressBar1.Maximum = RowCount;
			progressBar1.Value = 0;

			if (FirstRow)
			{
				foreach(DataGridTableStyle TableStyle in dgd.TableStyles)
				{
					int k = 0;
					strData = string.Empty;
					foreach(DataGridColumnStyle ColumnStyle in TableStyle.GridColumnStyles)
					{
						strData += td+ ColumnStyle.MappingName +td;
						if ( k < iColCount - 1)
							strData += tu;

						++k;
					}	
					sw.Write(strData);
					sw.Write(sw.NewLine);
				}
			}

			for(int i=0; i < RowCount; i++) 
			{
				progressBar1.Value = i+1;

				strData = string.Empty;
				for(int j=0; j < ColumnCount; j++) 
				{
					if (!Convert.IsDBNull(dgd[i,j]))
						strData += td+ dgd[i,j].ToString().TrimEnd() +td;
					else
						strData += td+""+td;
					
					if ( j < iColCount - 1)
						strData += tu;
				}
				sw.Write(strData);

				if (i < RowCount-1)		
					sw.Write(sw.NewLine);
			}

			sw.Close();
		}

		private void ExportXLS()
		{
			Microsoft.Office.Interop.Excel.Application application;
			Microsoft.Office.Interop.Excel.Workbook book;
			Microsoft.Office.Interop.Excel.Worksheet sheet;

            //-- OLD
			//application = new Microsoft.Office.Interop.Excel.ApplicationClass();

            application = new Microsoft.Office.Interop.Excel.Application();
			
			System.Globalization.CultureInfo oldCI = System.Threading.Thread.CurrentThread.CurrentCulture ;
			System.Threading.Thread.CurrentThread.CurrentCulture =  new System.Globalization.CultureInfo("en-US");
			object M = System.Reflection.Missing.Value;

			application.Visible = false;
			application.AlertBeforeOverwriting = false;
			
			book = application.Workbooks.Add(Type.Missing);

			sheet = (Microsoft.Office.Interop.Excel.Worksheet)book.Worksheets[1];
			sheet.Name = "Data";

			string strData = string.Empty;
			int colIndex = 0;
			int rowIndex = 0;
			DateTime DateData;

			progressBar1.Maximum = RowCount;
			progressBar1.Value = 0;
			
			Microsoft.Office.Interop.Excel.Range range;

			if (FirstRow)
			{
				rowIndex = 1;

				Microsoft.Office.Interop.Excel.Style style = book.Styles.Add("StyleBold", M);
				style.Font.Bold = true;
				style.NumberFormat = "Text";

				foreach(DataGridTableStyle TableStyle in dgd.TableStyles)
				{
					foreach(DataGridColumnStyle ColumnStyle in TableStyle.GridColumnStyles)
					{
						colIndex++;	
						
						range = ((Microsoft.Office.Interop.Excel.Range)sheet.Cells[rowIndex, colIndex]);
						range.Style = "StyleBold";
						range.Value2 = ColumnStyle.MappingName;
						range.Columns.AutoFit();
					}	
				}
			}

			for (int i = 0; i < RowCount; i++)
			{
				progressBar1.Value = i+1;

				for (int j = 0; j < ColumnCount; j++)
				{
					if (!Convert.IsDBNull(dgd[i,j]))
					{
						if (IsDateTime(dgd[i,j]))
						{
							DateData = Convert.ToDateTime(dgd[i,j], DateTimeFormatInfo.CurrentInfo);
							if (DateData.Hour == 0 && DateData.Minute == 0)
								strData = DateData.ToString("dd/MM/yyyy");
							else 
								strData = DateData.ToString("dd/MM/yyyy HH:mm");
						}
						else
							strData = dgd[i,j].ToString().TrimEnd();
					}
					else
						strData = "";
					
					range = ((Microsoft.Office.Interop.Excel.Range)sheet.Cells[i + 1 + rowIndex, j + 1]);
					range.NumberFormat = "@";
					range.Value2 = strData;
				}
			}

			book.SaveAs(strPath,M,M,M,M,M,Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange,M,M,M,M,M);

			// Finally close the Workbook and save it
//			book.Close(true, M, M );

			application.Quit();

			System.Threading.Thread.CurrentThread.CurrentCulture = oldCI;
		}

		private void timer1_Tick(object sender, System.EventArgs e)
		{
			++ CountTime;
			if (CountTime > 3)
			{
				timer1.Enabled = false;

                //-- F Edit 08/05/2013
                //OLD
                //trdExport = new Thread(new ThreadStart(ExportData));
                trdExport = new Thread(new ThreadStart(StartExportData));
				trdExport.Start();                
			}
		
		}

        //-- F Edit 08/05/2013
        private delegate void MyDelegate();
        void StartExportData()
        {
            this.Invoke((MyDelegate)delegate
            {
                ExportData();
            });
        }

		private bool IsDateTime(object Value)
		{
			try
			{
				Convert.ToDateTime(Value, DateTimeFormatInfo.CurrentInfo);
				return true;
			}
			catch 
			{
				return false;
			}
		}


	}
}
