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
    public partial class RegStudent : Form
    {
        public RegStudent(AdminMainForm mainForm)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            admin = new();
        }

        //Back
        private void button4_Click(object sender, EventArgs e)
        {
            Close();
            mainForm.Show();
        }

        //Registration
        private void button2_Click(object sender, EventArgs e)
        {
            //Check input
            long phoneNum = 0;
            if (!long.TryParse(textBox1.Text, out phoneNum) || phoneNum > 99999999999 || phoneNum < 10000000000)
            {
                MessageBox.Show("Введен некорректный номер телефона.", "РЕГИСТРАЦИЯ УЧЕНИКА");
                return;
            }

            MessageBox.Show(admin.RegistrationStudent(textBox4.Text, textBox3.Text, phoneNum, textBox2.Text, textBox5.Text), "РЕГИСТРАЦИЯ УЧЕНИКА");
        }

        private AdminMainForm mainForm;
        private AdminLogic admin;
    }
}