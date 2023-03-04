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
    public partial class TeacherMainForm : Form
    {
        public TeacherMainForm()
        {
            InitializeComponent();
            teacher = new TeacherLogic();
        }

        //Get students
        private void button3_Click(object sender, EventArgs e)
        {
            MessageBox.Show(teacher.GetStudents(textBox1.Text));
        }

        //Issue task to the group
        private void button1_Click(object sender, EventArgs e)
        {
            IssueGroup task = new(this);
            Hide();
            task.Show();
        }

        //Add marks
        private void button2_Click(object sender, EventArgs e)
        {
            int idStudent = 0;
            if (!int.TryParse(textBox2.Text, out idStudent))
            {
                MessageBox.Show("Введен неправильный id студента.", "ОЦЕНКА");
                return;
            }
            int mark = 0;
            if (!int.TryParse(textBox5.Text, out mark))
            {
                MessageBox.Show("Введена неправильная оценка.", "ОЦЕНКА");
                return;
            }
            if ((mark > 5) || (mark < 2))
            {
                MessageBox.Show("Оценка не может быть < 2 и > 5.", "ОЦЕНКА");
                return;
            }
            MessageBox.Show(teacher.AddMarkStudent(idStudent, textBox3.Text, mark));
        }

        TeacherLogic teacher;
    }
}