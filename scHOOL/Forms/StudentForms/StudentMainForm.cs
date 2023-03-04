using scHOOL.UsersLogic.StudentLogic;
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
    public partial class StudentMainForm : Form
    {
        public StudentMainForm()
        {
            InitializeComponent();
            student = new();
        }

        //Check timetable
        private void button1_Click(object sender, EventArgs e)
        {
            //Получаем строку расписания
            string timetable = student.CheckTimetable();

            //Парсим строку
            string result = "";
            string day = "";

            foreach (var el in timetable)
            {
                if (el == ';')
                {
                    int subNum = 1;
                    day += el;

                    //Get weekday
                    int i = 0;
                    while (day[i] != ':')
                    {
                        result += day[i];
                        ++i;
                    }
                    ++i;

                    //Check void day
                    if (day[i] == ';')
                    {
                        result += "\nУроков нет!\n\n";
                        day = "";
                        continue;
                    }

                    result += "\n" + subNum + ". ";

                    //Get subjects
                    while (i != day.Length)
                    {
                        if ((day[i] == ',') || (day[i] == ';'))
                        {
                            if (day[i] != ';')
                            {
                                subNum += 1;
                                result += '\n';
                                result += subNum + ". ";
                            }
                            else
                                result += '\n';
                            ++i;
                            continue;
                        }
                        result += day[i];
                        ++i;
                    }
                    result+= '\n';
                    //Reset
                    day = "";
                    continue;
                }

                day += el;
            }

            MessageBox.Show(result, "РАСПИСАНИЕ");
        }

        //Check task
        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show(student.CheckTask(), "ЗАДАНИЕ");
        }

        //Check all marks
        private void button4_Click(object sender, EventArgs e)
        {
            string result = student.CheckAllMarks();
            result = result.Replace(';', '\n');
            result = result.Replace(":", ":\t");
            for (int i = 1; i < 6; ++i)
            {
                result = result.Replace(i.ToString(), i.ToString() + ' ');
            }
            MessageBox.Show(result, "ОЦЕНКИ");
        }

        //Check marks
        private void button3_Click(object sender, EventArgs e)
        {
            string result = student.CheckMarks(textBox1.Text);
            for (int i = 1; i < 6; ++i)
            {
                result = result.Replace(i.ToString(), i.ToString() + ' ');
            }
            MessageBox.Show(result, "ОЦЕНКИ");
        }

        StudentLogic student;
    }
}