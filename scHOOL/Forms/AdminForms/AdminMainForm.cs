using scHOOL.Forms.AdminForms;
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

namespace scHOOL.Forms.StudentForms
{
    public partial class AdminMainForm : Form
    {
        public AdminMainForm()
        {
            InitializeComponent();
            admin = new();
        }

        //Student registration
        private void button1_Click(object sender, EventArgs e)
        {
            RegStudent regSt = new(this);
            Hide();
            regSt.Show();
        }

        //Teacher registration
        private void button2_Click(object sender, EventArgs e)
        {
            RegTeacher regT = new(this);
            Hide();
            regT.Show();
        }

        //Check information ab teacher
        private void button7_Click(object sender, EventArgs e)
        {
            //Check input
            long phoneNum = 0;
            if (!long.TryParse(textBox3.Text, out phoneNum) || phoneNum > 99999999999)
            {
                MessageBox.Show("Введен некорректный номер телефона.", "ИНФОРМАЦИЯ ОБ УЧИТЕЛЕ");
                return;
            }
            MessageBox.Show(admin.GetTeacherInformation(phoneNum), "ИНФОРМАЦИЯ ОБ УЧИТЕЛЕ");
        }

        //Create new group
        private void button3_Click(object sender, EventArgs e)
        {
            AddGroup addGr = new(this);
            Hide();
            addGr.Show();
        }

        //Change timetable for group
        private void button4_Click(object sender, EventArgs e)
        {
            ChangeTimetable chT = new(this);
            Hide();
            chT.Show();
        }

        //Delete student
        private void button6_Click(object sender, EventArgs e)
        {
            //Check input
            long phoneNum = 0;
            if (!long.TryParse(textBox1.Text, out phoneNum) || phoneNum > 99999999999)
            {
                MessageBox.Show("Введен некорректный номер телефона.", "УДАЛЕНИЕ УЧЕНИКА");
                return;
            }
            MessageBox.Show(admin.DeleteStudent(phoneNum), "УДАЛЕНИЕ УЧЕНИКА");
        }

        //Delete teacher
        private void button5_Click(object sender, EventArgs e)
        {
            //Check input
            long phoneNum = 0;
            if (!long.TryParse(textBox2.Text, out phoneNum) || phoneNum > 99999999999)
            {
                MessageBox.Show("Введен некорректный номер телефона.", "УДАЛЕНИЕ УЧИТЕЛЯ");
                return;
            }
            MessageBox.Show(admin.DeleteTeacher(phoneNum), "УДАЛЕНИЕ УЧИТЕЛЯ");
        }

        //Add new subject
        private void button8_Click(object sender, EventArgs e)
        {
            MessageBox.Show(admin.AddSubject(textBox4.Text), "НОВЫЙ ПРЕДМЕТ");
        }

        private AdminLogic admin;
    }
}
