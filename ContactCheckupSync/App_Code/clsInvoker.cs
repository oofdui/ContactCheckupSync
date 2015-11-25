using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

class clsInvoker
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="control"></param>
    /// <param name="maxValue"></param>
    /// <param name="value"></param>
    /// <example>
    /// clsInvoker.setProgressBar(pbDefault, dtMobile.Rows.Count, 0);
    /// </example>
    public void setProgressBar(ProgressBar control, int maxValue, int value)
    {
        if (control.InvokeRequired)
        {
            control.Invoke(new MethodInvoker(delegate
            {
                control.Maximum = maxValue;
                control.Value = value;
            }));
        }
        else
        {
            control.Maximum = maxValue;
            control.Value = value;
        }
    }
    public void setLabel(Label control, string text)
    {
        if (control.InvokeRequired)
        {
            control.Invoke(new MethodInvoker(delegate
            {
                control.Text = text;
            }));
        }
        else
        {
            control.Text = text;
        }
    }
    public void setButton(Button control, bool enable)
    {
        if (control.InvokeRequired)
        {
            control.Invoke(new MethodInvoker(delegate
            {
                control.Enabled = enable;
            }));
        }
        else
        {
            control.Enabled = enable;
        }
    }
	private void setComboBox(ComboBox control, int index)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(new MethodInvoker(delegate
                {
                    control.SelectedIndex = index;
                }));
            }
            else
            {
                control.SelectedIndex = index;
            }
        }
    public void setPictureBox(PictureBox control, bool visible)
    {
        if (control.InvokeRequired)
        {
            control.Invoke(new MethodInvoker(delegate
            {
                control.Visible = visible;
            }));
        }
        else
        {
            control.Visible = visible;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="control"></param>
    /// <param name="color"></param>
    /// <param name="columnFullWidth"></param>
    /// <param name="value"></param>
    /// <example>
    /// clsInvoker.setListView(
    ///     lvDefault, 
    ///     Color.Green, 
    ///     99,
    ///     new string[] {
    ///     DateTime.Now.ToString("dd/MM/yyyy HH:mm"), "Cancel", "", "Cancel by user." }
    ///);
    /// </example>
    public void setListView(ListView control, Color? color = null, int columnFullWidth = 99, params string[] value)
    {
        if (control.InvokeRequired)
        {
            control.Invoke(new MethodInvoker(delegate
            {
                control.Items.Add(new ListViewItem(value));
                if (color != null)
                {
                    control.Items[control.Items.Count - 1].ForeColor = (Color)color;
                }
                control.EnsureVisible(control.Items.Count - 1);
                setListViewResizeColumn(control, columnFullWidth);
            }));
        }
        else
        {
            control.Items.Add(new ListViewItem(value));
            if (color != null)
            {
                control.Items[control.Items.Count - 1].ForeColor = (Color)color;
            }
            control.EnsureVisible(control.Items.Count - 1);
            setListViewResizeColumn(control, columnFullWidth);
        }
    }
    public void setListViewResizeColumn(ListView control, int column = 99)
    {
        if (control.InvokeRequired)
        {
            #region Invoke
            control.Invoke(new MethodInvoker(delegate
            {
                var totalColumnWidth = 0;
                var calculateColumnWidth = 0;
                for (int i = 0; i < control.Columns.Count; i++)
                {
                    if (column == 99)
                    {
                        if (i < control.Columns.Count - 1)
                        {
                            control.AutoResizeColumn(i, ColumnHeaderAutoResizeStyle.ColumnContent);
                        }
                    }
                    else
                    {
                        if (column != i)
                        {
                            control.AutoResizeColumn(i, ColumnHeaderAutoResizeStyle.ColumnContent);
                            totalColumnWidth += control.Columns[i].Width;
                        }
                    }
                }
                #region FullFill
                if (column == 99)
                {
                    control.Columns[control.Columns.Count - 1].Width = -2;
                }
                else
                {
                    calculateColumnWidth = control.Width - totalColumnWidth - (control.Width / 10);//ลบด้วย 10% ของความกว้างทั้งหมดอีกรอบกันเลย
                    control.Columns[column].Width = calculateColumnWidth;
                    control.Columns[control.Columns.Count - 1].Width = -2;
                }
                #endregion
            }));
            #endregion
        }
        else
        {
            var totalColumnWidth = 0;
            var calculateColumnWidth = 0;
            for (int i = 0; i < control.Columns.Count; i++)
            {
                if (column == 99)
                {
                    if (i < control.Columns.Count - 1)
                    {
                        control.AutoResizeColumn(i, ColumnHeaderAutoResizeStyle.ColumnContent);
                    }
                }
                else
                {
                    if (column != i)
                    {
                        control.AutoResizeColumn(i, ColumnHeaderAutoResizeStyle.ColumnContent);
                        totalColumnWidth += control.Columns[i].Width;
                    }
                }
            }
            #region FullFill
            if (column == 99)
            {
                control.Columns[control.Columns.Count - 1].Width = -2;
            }
            else
            {
                calculateColumnWidth = control.Width - totalColumnWidth - (control.Width / 10);//ลบด้วย 10% ของความกว้างทั้งหมดอีกรอบกันเลย
                control.Columns[column].Width = calculateColumnWidth;
                control.Columns[control.Columns.Count - 1].Width = -2;
            }
            #endregion
        }
    }
}