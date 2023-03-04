using scHOOL.Forms.StudentForms;
using scHOOL.UsersLogic.AdminLogic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace scHOOL.Forms.AdminForms
{
    public partial class AddGroup : Form
    {
        public AddGroup(AdminMainForm mainForm)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            admin = new();
            subjectList = new();
        }

        //Back
        private void button1_Click(object sender, EventArgs e)
        {
            Close();
            mainForm.Show();
        }

        //Add subject to subject list
        private void button8_Click(object sender, EventArgs e)
        {
            subjectList.Add(textBox2.Text);
            textBox1.Text += textBox2.Text + ',';
        }

        //Create new group
        private void button4_Click(object sender, EventArgs e)
        {
            MessageBox.Show(admin.CreateGroup(textBox3.Text, subjectList), "");
            subjectList.Clear();
            //textBox1.Text = "";
        }

        private AdminMainForm mainForm;
        private AdminLogic admin;
        private List<string> subjectList;
    }
}