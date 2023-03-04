using scHOOL.Context;
using scHOOL.UsersLogic.TeacherLogic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace scHOOL.Forms.TeacherForms
{
    public partial class IssueGroup : Form
    {
        public IssueGroup(TeacherMainForm mainForm)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            teacher = new();
        }

        //Get task
        private void button2_Click(object sender, EventArgs e)
        {
            string result = teacher.GetTaskGroup(textBox4.Text, textBox3.Text);
            switch(result)
            {
                case "Группы с таким названием нет.":
                    MessageBox.Show(result, "ЗАДАНИЕ");
                    break;
                case "Предмета с таким названием нет.":
                    MessageBox.Show(result, "ЗАДАНИЕ");
                    break;
                case "В группе такого предмета нет.":
                    MessageBox.Show(result, "ЗАДАНИЕ");
                    break;
                default:
                    richTextBox2.Text = result;
                    break;
            }
        }

        //Issue new task
        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show(teacher.IssueTaskGroup(textBox4.Text, textBox3.Text, richTextBox2.Text), "ЗАДАНИЕ");
        }

        //Back
        private void button4_Click(object sender, EventArgs e)
        {
            Close();
            mainForm.Show();
        }

        private TeacherMainForm mainForm;
        private TeacherLogic teacher;
    }
}