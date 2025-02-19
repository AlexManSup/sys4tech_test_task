﻿/**********************************
 * Title:       DataGridViewColumn Hosting MaskedTextBox
 * Author:      Juergen Thomas, Berlin (Germany)
 * Email:       post@vs-polis.de
 * Environment: WinXP, NET 2.0
 * Keywords:    DataGridViewColumn, 
 *   DataGridViewMaskedTextColumn, 
 *   DataGridViewMaskedTextCell, 
 *   DataGridViewMaskedTextControl,
 *   DataGridViewTextBoxColumn, 
 *   DataGridViewTextBoxCell, 
 *   DataGridViewTextBoxEditingControl,
 *   DataGridView, 
 *   MaskedTextBox,
 *   EditingControl,
 *   IDataGridViewEditingControl,
 *   Host Controls
 * Description: An article to host a MaskedTextBox in a DataGridViewColumn
 * Section      Desktop Development
 * SubSection   Grid & Data Controls >> .NET - DataGrid and DataView
 * 
 * Contents
 * --------
 * This solution contains the following classes:
 *   DataGridViewMaskedTextColumn, 
 *   DataGridViewMaskedTextCell, 
 *   DataGridViewMaskedTextControl,  
 * 
 * Additionally, the DataGridViewMaskedTextColumn class uses the
 * ReferencedDescriptionAttribute.
 * 
 * (C) Juergen Thomas
 * The Code Project Open License (CPOL) 
 * http://www.codeproject.com/info/EULA.aspx
 * 
 * History information is posted in DataGridViewMaskedTextColumn.cs 
 **********************************/

/*********************************
 * History
 * -------
 * 05/04/2008  First Version
 * 
 *********************************/

#region Usings
using System;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;
#endregion

namespace JThomas.Controls
{
	/// <summary>
	/// DataGridViewMaskedTextColumn hosts a DGV-MaskedTextCell collection
	/// containing a Mask property.
	/// </summary>
	[System.Drawing.ToolboxBitmap(typeof(System.Windows.Forms.MaskedTextBox))]
	public class DataGridViewMaskedTextColumn : DataGridViewColumn
	{
		
		#region Fields
    private static Type columnType = typeof(DataGridViewMaskedTextColumn);
		#endregion

		#region Constructors
		
		/// <summary>
		/// Standard constructor without arguments
		/// </summary>
		public DataGridViewMaskedTextColumn() 
			: this(String.Empty)
		{
		}

		/// <summary>
		/// Constructor using a Mask string
		/// </summary>
		/// <param name="maskString">Mask string used in the EditingControl</param>
		public DataGridViewMaskedTextColumn(string maskString)
			: base(new DataGridViewMaskedTextCell())
		{
			SortMode = DataGridViewColumnSortMode.Automatic;
			Mask = maskString;
		}

		#endregion
		
		#region Methods
		
		/// <summary>
		/// Converting the current DGV-MaskedTextColumn instance to a string value.
		/// </summary>
		/// <returns>String value of the instance containing the name 
		/// and column index.</returns>
		public override string ToString()
		{
			StringBuilder builder = new StringBuilder(0x40);
			builder.Append("DataGridViewMaskedTextColumn { Name=");
			builder.Append(base.Name);
			builder.Append(", Index=");
			builder.Append(base.Index.ToString());
			builder.Append(" }");
			return builder.ToString();
		}

		/// <summary>
		/// Creates a copy of a DGV-MaskedTextColumn containing the DGV-Column properties.
		/// </summary>
		/// <returns>Instance of a DGV-MaskedTextColumn using the Mask string.</returns>
		public override object Clone() 
		{
			DataGridViewMaskedTextColumn col = (DataGridViewMaskedTextColumn)base.Clone();
			col.Mask = Mask;
			col.CellTemplate = (DataGridViewMaskedTextCell)this.CellTemplate.Clone();
			return col;
		}

		#endregion

		#region Derived properties
		
		[Browsable(false), 
		 DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override DataGridViewCell CellTemplate
		{
			get {	return base.CellTemplate;			}
			set	{
				if ((value != null) && !(value is DataGridViewMaskedTextCell))
				{
					throw new InvalidCastException("DataGridView: WrongCellTemplateType, must be DataGridViewMaskedTextCell");
				}
				base.CellTemplate = value;
			}
		}

		[DefaultValue(1)]
		public new DataGridViewColumnSortMode SortMode
		{
			get	{	return base.SortMode;		}
			set	{	base.SortMode = value;	}
		}

		private DataGridViewMaskedTextCell MaskedTextCellTemplate
		{
			get	{	return (DataGridViewMaskedTextCell)CellTemplate;			}
		}
		
		#endregion
		
		#region Mask property
		
		/// <summary>
		/// Input String that rules the possible input values in each cell of the column.
		/// </summary>
		[Category("Masking")]
		[JThomas.Extensions.ReferencedDescription(typeof(System.Windows.Forms.MaskedTextBox), "Mask")]
		public string Mask 
		{
			get
			{
				if (MaskedTextCellTemplate == null)
				{
					throw new InvalidOperationException("DataGridViewColumn: CellTemplate required");
				}
				return MaskedTextCellTemplate.Mask;
			}
			set
			{
				if (Mask != value)
				{
					/// If the mask is changed, the cell template has to be changed,
					/// and each cell of the column has to be adapted.
					MaskedTextCellTemplate.Mask = value;
					if (base.DataGridView != null)
					{
						DataGridViewRowCollection rows = base.DataGridView.Rows;
						int count = rows.Count;
						for (int i = 0; i < count; i++)
						{
							DataGridViewMaskedTextCell cell = rows.SharedRow(i).Cells[base.Index] as DataGridViewMaskedTextCell;
							if (cell != null)
							{
								cell.Mask = value;
							}
						}
					}
				}
			}
		}
		
		#endregion
				
	}
	
}
