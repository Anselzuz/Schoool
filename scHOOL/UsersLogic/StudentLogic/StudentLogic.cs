using Microsoft.EntityFrameworkCore;
using scHOOL.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scHOOL.UsersLogic.StudentLogic
{
    enum Week
    {
        Monday = 0,
        Tuesday = 1,
        Wednesday = 2,
        Thursday = 3,
        Friday = 4,
        Saturday = 5
    }
    class StudentLogic : IStudentLogic
    {
        public string CheckAllMarks()
        {
            using (SchooolContext ctx = new())
            {
                string result = "";
                //Получаем строку оценок
                string marks = ctx.Students.Where(student => student.PhoneNum == UserInformation.phoneNum).First().Marks;

                //Находим названия предметов по их id
                string subject = "";
                for (int i = 0; i < marks.Length; ++i)
                {
                    if (marks[i] == ':')
                    {
                        subject = ctx.Subjectts.Where(subj => subj.Id == int.Parse(subject)).First().SubjectName;
                        result += subject;

                        while (marks[i] != ';')
                        {
                            result += marks[i];
                            ++i;
                        }
                        result += ';';
                        subject = "";
                        continue;
                    }

                    subject += marks[i];
                }

                return result;
            }
        }

        public string CheckMarks(string subject)
        {
            using (SchooolContext ctx = new())
            {
                string result = "";
                int subjectId = -1;
                try
                {
                    //Проверяем есть ли такой предмет
                    subjectId = ctx.Subjectts.Where(subj => subj.SubjectName.ToLower() == subject.ToLower()).First().Id;
                    //Правильное название предмета
                    string trueSubName = ctx.Subjectts.Where(subj => subj.Id == subjectId).First().SubjectName;

                    //Получаем оценки
                    string marks = ctx.Students.Where(student => student.PhoneNum == UserInformation.phoneNum).First().Marks;

                    //Делаем выборку
                    string subjectMarks = "";
                    string idBuff = "";
                    foreach (var el in marks)
                    {
                        if (el == ';')
                        {
                            foreach (var el1 in subjectMarks)
                            {
                                if (el1 == ':')
                                {
                                    if (int.Parse(idBuff) == subjectId)
                                    {
                                        result += trueSubName + ": " + subjectMarks.Replace(idBuff+':', "");
                                        return result;
                                    }
                                }
                                idBuff += el1;
                            }
                            idBuff = "";
                            subjectMarks = "";
                            continue;
                        }
                        subjectMarks += el;
                    }

                    result = "Предмета с таким названием у вас нет.";
                }
                catch(InvalidOperationException ex) 
                {
                    result = "Неправильное название предмета.";
                }

                return result;
            }
        }

        public string CheckTask()
        {
            using (SchooolContext ctx = new())
            {
                string result = "";

                //Получаем свою группу
                string groupNum = ctx.Students.Where(student => student.PhoneNum == UserInformation.phoneNum).First().Group;

                //Ищем задания для группы и переводим id предметов в названия
                var tasks = ctx.Tasks.Where(task => task.Group == groupNum).ToList();
                foreach (var el in tasks)
                    result += ctx.Subjectts.Where(subject => subject.Id == el.IdSub).First().SubjectName + ": " + el.Task1 + "\n\n";

                return result;
            }
        }

        public string CheckTimetable()
        {
            Dictionary<int, string> subjects = new Dictionary<int, string>();
            string result = "";

            //Получаем строку расписания из бд
            using(SchooolContext ctx = new())
            {
                //Получаем свою группу
                string groupNum = ctx.Students.Where(student => student.PhoneNum == UserInformation.phoneNum).First().Group;

                //По группе получаем расписание
                string timetable = ctx.Groupps.Where(group => group.GroupNum == groupNum).First().Timetable;

                //Меняем id предметов и дней недели на их название
                for (int i = 0; i < timetable.Length; ++i)
                {
                    string day = "";

                    while(timetable[i]!=';')
                    {
                        day+= timetable[i];
                        ++i;
                    }
                    day += ';';

                    //Меняем id дней недели на их название
                    int dayWeek = int.Parse(day[0..1]);
                    switch (dayWeek)
                    {
                        case (int)Week.Monday:
                            result += "-Понедельник-:";
                            break;
                        case (int)Week.Tuesday:
                            result += "-Вторник-:";
                            break;
                        case (int)Week.Wednesday:
                            result += "-Среда-:";
                            break;
                        case (int)Week.Thursday:
                            result += "-Четверг-:";
                            break;
                        case (int)Week.Friday:
                            result += "-Пятница-:";
                            break;
                        case (int)Week.Saturday:
                            result += "-Суббота-:";
                            break;
                    }

                    day = day.Remove(0, 2);
                    string subject = "";
                    int elId = 0;
                    foreach (var el in day)
                    {
                        if ((el == ',') || (el == ';'))
                        {
                            if((subject == "") && (el == ';'))      //!
                            {
                                result+= el;
                                break;
                            }
                            else
                                elId = int.Parse(subject);

                            try
                            {
                                //Ищем в кеше(словаре) предметов
                                subject = subjects[elId];
                            }
                            catch(KeyNotFoundException ex)
                            {
                                //Если не нашли обращаемся к бд
                                var subjectBuff = ctx.Subjectts.Where(subject => subject.Id == elId).First();
                                subject = subjectBuff.SubjectName;
                                subjects.Add(elId, subject);
                            }

                            if (el == ',')
                                result += subject + ',';
                            else
                                result += subject + ';';
                            subject = "";
                            continue;
                        }

                        subject+= el;
                    }
                }

                return result;
            }
        }
    }
}