using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data;

namespace BasaDan
{
    public partial class Form2 : Form
    {
        private SqlConnection sqlConnection = null;

        private SqlCommandBuilder sqlBuilder = null;

        private SqlDataAdapter sqlDataAdapter = null;

        private DataSet dataSet = null;

        private bool newRowadding = false;

        public Form2()
        {
            InitializeComponent();
        }

        private void LoadData()
        {
            try
            {
                sqlDataAdapter = new SqlDataAdapter("SELECT *, 'Delete' AS [Command] FROM Supplier", sqlConnection);

                sqlBuilder = new SqlCommandBuilder(sqlDataAdapter);

                sqlBuilder.GetInsertCommand();
                sqlBuilder.GetUpdateCommand();
                sqlBuilder.GetDeleteCommand();

                dataSet = new DataSet();//gg

                sqlDataAdapter.Fill(dataSet, "Supplier");

                dataGridView1.DataSource = dataSet.Tables["Supplier"];

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    DataGridViewLinkCell linkCell = new DataGridViewLinkCell();

                    dataGridView1[7, i] = linkCell;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ReloadData()
        {
            try
            {
                dataSet.Tables["Supplier"].Clear();

                sqlDataAdapter.Fill(dataSet, "Supplier");

                dataGridView1.DataSource = dataSet.Tables["Supplier"];

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    DataGridViewLinkCell linkCell = new DataGridViewLinkCell();

                    dataGridView1[7, i] = linkCell;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void Form2_Load(object sender, EventArgs e)
        {
            sqlConnection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\nikse\source\repos\BasaDan\BasaDan\Database1.mdf;Integrated Security=True");

            sqlConnection.Open();

            LoadData();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            ReloadData();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            try
            {
                if (e.ColumnIndex == 7)
                {
                    string task = dataGridView1.Rows[e.RowIndex].Cells[7].Value.ToString();

                    if (task == "Delete")
                    {
                        if (MessageBox.Show("Удалить эту строку?", "Удаление", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                            == DialogResult.Yes)
                        {
                            int rowIndex = e.RowIndex;

                            dataGridView1.Rows.RemoveAt(rowIndex);

                            dataSet.Tables["Supplier"].Rows[rowIndex].Delete();

                            sqlDataAdapter.Update(dataSet, "Supplier");
                        }

                    }
                    else if (task == "Insert")
                    {
                        int rowIndex = dataGridView1.Rows.Count - 2;

                        DataRow row = dataSet.Tables["Supplier"].NewRow();

                        row["Name"] = dataGridView1.Rows[rowIndex].Cells["Name"].Value;
                        row["INN"] = dataGridView1.Rows[rowIndex].Cells["INN"].Value;
                        row["City"] = dataGridView1.Rows[rowIndex].Cells["City"].Value;
                        row["Street"] = dataGridView1.Rows[rowIndex].Cells["Street"].Value;
                        row["DomAndKV"] = dataGridView1.Rows[rowIndex].Cells["DomAndKV"].Value;
                        row["Remove"] = dataGridView1.Rows[rowIndex].Cells["Remove"].Value;

                        dataSet.Tables["Supplier"].Rows.Add(row);

                        dataSet.Tables["Supplier"].Rows.RemoveAt(dataSet.Tables["Supplier"].Rows.Count - 1);

                        dataGridView1.Rows.RemoveAt(dataGridView1.Rows.Count - 2);

                        dataGridView1.Rows[e.RowIndex].Cells[7].Value = "Delete";

                        sqlDataAdapter.Update(dataSet, "Supplier");

                        newRowadding = false;
                    }
                    else if (task == "Update")
                    {
                        int r = e.RowIndex;

                        dataSet.Tables["Supplier"].Rows[r]["Name"] = dataGridView1.Rows[r].Cells["Name"].Value;
                        dataSet.Tables["Supplier"].Rows[r]["INN"] = dataGridView1.Rows[r].Cells["INN"].Value;
                        dataSet.Tables["Supplier"].Rows[r]["City"] = dataGridView1.Rows[r].Cells["City"].Value;
                        dataSet.Tables["Supplier"].Rows[r]["Street"] = dataGridView1.Rows[r].Cells["Street"].Value;
                        dataSet.Tables["Supplier"].Rows[r]["DomAndKV"] = dataGridView1.Rows[r].Cells["DomAndKV"].Value;
                        dataSet.Tables["Supplier"].Rows[r]["Remove"] = dataGridView1.Rows[r].Cells["Remove"].Value;

                        sqlDataAdapter.Update(dataSet, "Supplier");

                        dataGridView1.Rows[e.RowIndex].Cells[7].Value = "Delete";
                    }

                    ReloadData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            try
            {
                if (newRowadding == false)
                {
                    newRowadding = true;

                    int lastRow = dataGridView1.Rows.Count - 2;

                    DataGridViewRow row = dataGridView1.Rows[lastRow];

                    DataGridViewLinkCell linkCell = new DataGridViewLinkCell();

                    dataGridView1[7, lastRow] = linkCell;

                    row.Cells["Command"].Value = "Insert";

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (newRowadding == false)
                {
                    int rowIndex = dataGridView1.SelectedCells[0].RowIndex;

                    DataGridViewRow editingRow = dataGridView1.Rows[rowIndex];

                    DataGridViewLinkCell linkCell = new DataGridViewLinkCell();

                    dataGridView1[7, rowIndex] = linkCell;

                    editingRow.Cells["Command"].Value = "Update";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


       

        private void toolStripSeparator1_Click(object sender, EventArgs e)
        {
            //хз что это
        }

        private void файлToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void выходToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}