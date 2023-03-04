using scHOOL.Forms.StudentForms;
using scHOOL.UsersLogic.AdminLogic;
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
using System.Xml;

namespace scHOOL.Forms.AdminForms
{
    public partial class ChangeTimetable : Form
    {
        public ChangeTimetable(AdminMainForm mainForm)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            admin = new();
        }

        //Back
        private void button1_Click(object sender, EventArgs e)
        {
            Close();
            mainForm.Show();
        }

        //Delete last
        private void button2_Click(object sender, EventArgs e)
        {
            int idDay = GetDayId(textBox3.Text);
            if (idDay == -1)
                return;
            MessageBox.Show(admin.DeleteAtTheEndTimetable(textBox2.Text, idDay), "РАСПИСАНИЕ");
        }

        //Check timetable
        private void button8_Click(object sender, EventArgs e)
        {

            string timetable = admin.CheckTimetable(textBox2.Text);
            if (timetable == "Пустое поле группы.")
            {
                MessageBox.Show(timetable, "РАСПИСАНИЕ");
                return;
            }

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
                    result += '\n';
                    //Reset
                    day = "";
                    continue;
                }

                day += el;
            }

            MessageBox.Show(result, "РАСПИСАНИЕ");
        }

        //Add subject at the end
        private void button3_Click(object sender, EventArgs e)
        {
            int idDay = GetDayId(textBox3.Text);
            if (idDay == -1)
                return;
            MessageBox.Show(admin.AddAtTheEndTimetable(textBox2.Text, idDay, textBox1.Text), "РАСПИСАНИЕ");
        }

        private int GetDayId(string day)
        {
            int idDay = 0;
            //Проверяем день недели
            switch (day.ToLower())
            {
                case "понедельник":
                    idDay = 0;
                    break;
                case "пн":
                    idDay = 0;
                    break;
                case "вторник":
                    idDay = 1;
                    break;
                case "вт":
                    idDay = 1;
                    break;
                case "среда":
                    idDay = 2;
                    break;
                case "ср":
                    idDay = 2;
                    break;
                case "четверг":
                    idDay = 3;
                    break;
                case "чт":
                    idDay = 3;
                    break;
                case "пятница":
                    idDay = 4;
                    break;
                case "пт":
                    idDay = 4;
                    break;
                case "суббота":
                    idDay = 5;
                    break;
                case "сб":
                    idDay = 5;
                    break;
                default:
                    MessageBox.Show("День недели введен неправильно.", "РАСПИСАНИЕ");
                    return -1;
            }

            return idDay;
        }

        private AdminMainForm mainForm;
        private AdminLogic admin;
    }
}