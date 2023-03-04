using scHOOL.UsersLogic;
using scHOOL.Context;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace scHOOL
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        //Enter
        private void button1_Click(object sender, EventArgs e)
        {
            //Get phone number
            if (!long.TryParse(textBox1.Text, out UserInformation.phoneNum))
            {
                MessageBox.Show("Введен некорректный номер телефона.");
                return;
            }

            using (SchooolContext ctx = new())
            {
                string name = "";

                //Check student
                var students = ctx.Students.Where(student => (student.PhoneNum == UserInformation.phoneNum) && (student.Pasw == textBox2.Text)).ToList();
                if (students.Count != 0)
                {
                    name = students[0].Name;
                    UserInformation.role = 1;
                    MessageBox.Show("Добро пожаловать ученик " + name + '!');
                    Close();
                    return;
                }

                //Check teacher
                var teachers = ctx.Teachers.Where(teacher => (teacher.PhoneNum == UserInformation.phoneNum) && (teacher.Pasw == textBox2.Text)).ToList();
                if (teachers.Count != 0)
                {
                    name = teachers[0].Name;
                    UserInformation.role = 2;
                    MessageBox.Show("Добро пожаловать учитель " + name + '!');
                    Close();
                    return;
                }

                //Check administrator
                var admins = ctx.Administrators.Where(admin => (admin.PhoneNum == UserInformation.phoneNum) && (admin.Pasw == textBox2.Text)).ToList();
                if (admins.Count != 0)
                {
                    name = admins[0].Name;
                    UserInformation.role = 0;
                    MessageBox.Show("Добро пожаловать администратор " + name + '!');
                    Close();
                    return;
                }

                //If fail authorization
                MessageBox.Show("Введен неправильный номер телефона или пароль.");
            }
        }
    }
}
