using scHOOL.Context;
using scHOOL.Models;
using scHOOL.UsersLogic.StudentLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace scHOOL.UsersLogic.AdminLogic
{
    public class AdminLogic : IAdminLogic
    {
        public string AddAtTheEndTimetable(string groupNum, int idDay, string subjectName)
        {
            using (SchooolContext ctx = new())
            {
                string result = "";

                //Проверяем группу
                var groups = ctx.Groupps.Where(group => group.GroupNum.ToLower() == groupNum.ToLower());
                if (groups.ToList().Count() == 0)
                {
                    result = "Группы с таким названием нет.";
                    return result;
                }

                //Проверяем название предмета
                var subjects = ctx.Subjectts.Where(subject => subject.SubjectName.ToLower() == subjectName.ToLower()).ToList();
                if (subjects.Count() == 0)
                {
                    result = "Предмета с таким названием нет.";
                    return result;
                }

                //Получаем расписание и id предмета
                string timetable = groups.First().Timetable;
                int idSub = subjects.First().Id;

                //Проверяем есть ли предмет в списке предметов группы
                string subjList = groups.First().SubjectList;
                string idBuff = idSub.ToString();
                if (subjList.IndexOf(idBuff[0]) == -1)
                {
                    result = "В списке предметов группы нет такого предмета.";
                    return result;
                }

                //Получаем нужный день
                string day = "";
                foreach (var el in timetable)
                {
                    day += el;

                    if (el == ';')
                    {
                        if (int.Parse(day[..1]) == idDay)
                            break;
                        else
                            day = "";
                    }
                }

                //Добавляем id предмета в строку
                if (day.Length > 3)
                    day = day.Replace(";", "," + idSub + ";");
                else
                    day = day.Replace(";", idSub + ";");

                //Внедряем строку обратно в расписание
                string resultTimetable = "";
                string dayBuff = "";
                foreach (var el in timetable)
                {
                    dayBuff += el;

                    if (el == ';')
                    {
                        if (int.Parse(dayBuff[..1]) == idDay)
                            dayBuff = day;
                        resultTimetable += dayBuff;
                        dayBuff = "";
                    }
                }

                //Сохраняем изменения в бд
                groups.First().Timetable = resultTimetable;
                ctx.SaveChanges();
                result = "Предмет '" + subjectName + "' успешно добавлен.";
                return result;
            }
        }

        public string DeleteAtTheEndTimetable(string groupNum, int idDay)
        {
            using (SchooolContext ctx = new())
            {
                string result = "";

                //Проверяем группу
                var groups = ctx.Groupps.Where(group => group.GroupNum.ToLower() == groupNum.ToLower());
                if (groups.ToList().Count() == 0)
                {
                    result = "Группы с таким названием нет.";
                    return result;
                }

                //Получаем расписание
                string timetable = groups.First().Timetable;

                //Получаем нужный день
                string day = "";
                foreach (var el in timetable)
                {
                    day += el;

                    if (el == ';')
                    {
                        if (int.Parse(day[..1]) == idDay)
                            break;
                        else
                            day = "";
                    }
                }
                
                //If void
                if (day.Length == 3)
                {
                    result = "В этот день уроков нет.";
                    return result;
                }

                //Удаляем last id предмета
                while ((day.Last() != ',') && (day.Last() != ':'))
                    day = day[..^1];

                if (day.Last() == ',')
                    day = day[..^1];

                day = day + ';';

                //Внедряем строку обратно в расписание
                string resultTimetable = "";
                string dayBuff = "";
                foreach (var el in timetable)
                {
                    dayBuff += el;

                    if (el == ';')
                    {
                        if (int.Parse(dayBuff[..1]) == idDay)
                            dayBuff = day;
                        resultTimetable += dayBuff;
                        dayBuff = "";
                    }
                }

                //Сохраняем изменения в бд
                groups.First().Timetable = resultTimetable;
                ctx.SaveChanges();
                result = "Последний предмет успешно удален.";
                return result;
            }
        }

        public string CheckTimetable(string groupNum)
        {
            Dictionary<int, string> subjects = new Dictionary<int, string>();
            string result = "";

            //Получаем строку расписания из бд
            using(SchooolContext ctx = new())
            {
                //По группе получаем расписание
                string timetable = "";
                try
                {
                    timetable = ctx.Groupps.Where(group => group.GroupNum == groupNum).First().Timetable;
                }
                catch (InvalidOperationException ex) 
                {
                    result = "Пустое поле группы.";
                    return result;
                }

                //Меняем id предметов и дней недели на их название
                for (int i = 0; i < timetable.Length; ++i)
                {
                    string day = "";

                    while (timetable[i] != ';')
                    {
                        day += timetable[i];
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
                            if ((subject == "") && (el == ';'))      //!
                            {
                                result += el;
                                break;
                            }
                            else
                                elId = int.Parse(subject);

                            try
                            {
                                //Ищем в кеше(словаре) предметов
                                subject = subjects[elId];
                            }
                            catch (KeyNotFoundException ex)
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

                        subject += el;
                    }
                }

                return result;
            }
        }

        public string CreateGroup(string groupNum, List<string> subjectList)
        {
            using (SchooolContext ctx = new())
            {
                string result = "";

                //Проверяем номер группы
                if (ctx.Groupps.Where(group => group.GroupNum.ToLower() == groupNum.ToLower()).ToList().Count() > 0)
                {
                    result = "Группа с таким названием уже существует.";
                    return result;
                }

                //Проверяем предметы
                //На дубли в списке
                int count = 0;
                foreach (var el in subjectList)
                {
                    foreach (var el1 in subjectList)
                    {
                        if (el.ToLower() == el1.ToLower())
                            count++;
                    }

                    if (count > 1)
                    {
                        result = "В списке предметов не должны содержаться дубли.";
                        return result;
                    }

                    count = 0;
                }

                //На дубли в бд
                string subjList = "";
                foreach (var el in subjectList)
                {
                    var subjectBuff = ctx.Subjectts.Where(subject => subject.SubjectName.ToLower() == el.ToLower());
                    var control = subjectBuff.First().Id;
                    if (subjectBuff.ToList().Count() == 0)
                    {
                        result = "Предмета '" + el + "' нет в базе данных.";
                        return result;
                    }
                    else
                        subjList += subjectBuff.First().Id + ",";
                }
                subjList = subjList[..^1];

                //Пустой список расписания
                string voidTimetable = "0:;1:;2:;3:;4:;5:;";

                //Создаем новую группу и записываем ее в бд
                Groupp group = new Groupp() { GroupNum=groupNum, SubjectList=subjList, Timetable=voidTimetable };
                ctx.Groupps.Add(group);
                ctx.SaveChanges();
                result = "Группа успешно добавлена.";
                return result;
            }
        }

        public string AddSubject(string subjectName)
        {
            using (SchooolContext ctx = new())
            {
                string result = "";
                //Проверяем нет ли уже такого предмета
                if (ctx.Subjectts.Where(subject => subject.SubjectName.ToLower() == subjectName.ToLower()).ToList().Count() > 0)
                {
                    result = "Предмет с таким названием уже существует.";
                    return result;
                }

                Subjectt subject = new() { SubjectName = subjectName };
                ctx.Subjectts.Add(subject);
                ctx.SaveChanges();
                result = "Предмет успешно добавлен.";
                return result;
            }
        }

        public string DeleteStudent(long phoneNum)
        {
            using (SchooolContext ctx = new())
            {
                string result = "";
                //Проверяем есть ли такой ученик
                var student = ctx.Students.Where(student => student.PhoneNum == phoneNum);
                if (student.ToList().Count() == 0)
                {
                    result = "Ученика с таким телефоном нет.";
                    return result;
                }

                ctx.Students.Remove(student.First());
                ctx.SaveChanges();
                result = "Ученик успешно удален.";
                return result;
            }
        }

        public string DeleteTeacher(long phoneNum)
        {
            using (SchooolContext ctx = new())
            {
                string result = "";
                //Проверяем есть ли такой учитель
                var teacher = ctx.Teachers.Where(teacher => teacher.PhoneNum == phoneNum);
                if (teacher.ToList().Count() == 0)
                {
                    result = "Учителя с таким телефоном нет.";
                    return result;
                }

                ctx.Teachers.Remove(teacher.First());
                ctx.SaveChanges();
                result = "Учитель успешно удален.";
                return result;
            }
        }

        public string GetTeacherInformation(long phoneNum)
        {
            using (SchooolContext ctx = new())
            {
                string result = "";
                //Проверяем есть ли такой учитель
                var teacher = ctx.Teachers.Where(teacher => teacher.PhoneNum == phoneNum);
                if (teacher.ToList().Count() == 0)
                {
                    result = "Учителя с таким телефоном нет.";
                    return result;
                }

                result += "Фамилия: " + teacher.First().Surname + '\n' + "Имя: " + teacher.First().Name + '\n' + "Телефон: "
                     + teacher.First().PhoneNum + '\n' + "Предметы: " + teacher.First().Subject;
                return result;
            }
        }

        public string RegistrationStudent(string name, string surname, long phoneNum, string pasw, string group)
        {
            using (SchooolContext ctx = new())
            {
                string result = "";

                //Есть ли студент с таким номером
                if (ctx.Students.Where(student => student.PhoneNum == phoneNum).ToList().Count() > 0)
                {
                    result = "Студент с таким номером телефона уже существует.";
                    return result;
                }

                //Есть ли такая группа
                var groupBuff = ctx.Groupps.Where(grp => grp.GroupNum == group).ToList();
                if (groupBuff.Count() == 0)
                {
                    result = "Такой группы не существует.";
                    return result;
                }

                //Генерируем пустой список оценок
                string subjectList = groupBuff.First().SubjectList;
                string marks = "";
                string subject = "";

                foreach (var el in subjectList)
                {
                    if (el == ',')
                    {
                        marks += subject + ":;";
                        subject = "";
                        continue;
                    }

                    subject += el;
                }
                marks += subject + ":;";

                //Создаем нового студента и добавляем его в бд
                Student student = new Student() { Name=name, Surname=surname, PhoneNum=phoneNum, Pasw=pasw, Group=group, Marks=marks };
                ctx.Students.Add(student);
                ctx.SaveChanges();
                result = "Ученик успешно зарегестрирован.";
                return result;
            }
        }

        public string RegistrationTeacher(string name, string surname, long phoneNum, string pasw, string subject)
        {
            using (SchooolContext ctx = new())
            {
                string result = "";
                //Check phone num
                if (ctx.Teachers.Where(teacher => teacher.PhoneNum == phoneNum).ToList().Count() != 0)
                {
                    result = "Учитель с таким телефоном уже зарегестрирован.";
                    return result;
                }

                //Создаем учителя и добавляем его в бд
                Teacher teacher = new Teacher() { Name=name,Surname=surname,PhoneNum=phoneNum,Pasw=pasw,Subject=subject };
                ctx.Teachers.Add(teacher);
                ctx.SaveChanges();
                result = "Учитель успешно зарегестрирован.";
                return result;
            }
        }
    }
}