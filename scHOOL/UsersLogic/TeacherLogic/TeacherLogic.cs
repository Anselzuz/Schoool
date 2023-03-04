using scHOOL.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scHOOL.UsersLogic.TeacherLogic
{
    class TeacherLogic : ITeacherLogic
    {
        public string AddMarkStudent(int idStudent, string subject, int mark)
        {
            using (SchooolContext ctx = new())
            {
                string result = "";

                int subjectId = 0;
                //Проверяем предмет + получаем его id
                try
                {
                    subjectId = ctx.Subjectts.Where(subj => subj.SubjectName.ToLower() == subject.ToLower()).First().Id;
                }
                catch (InvalidOperationException ex)
                {
                    result = "Предмета с таким названием нет.";
                    return result;
                }

                string marks = "";
                dynamic student;
                //Проверяем студента + получаем его оценки
                try
                {
                    student = ctx.Students.Where(student => student.Id == idStudent).First();
                    marks = student.Marks;
                }
                catch (InvalidOperationException ex)
                {
                    result = "Ученика с таким id нет.";
                    return result;
                }

                //Есть ли такой предмет в группе студента
                string studGroup = ctx.Students.Where(stud => stud.Id == idStudent).First().Group;
                string subjectsGroup = ctx.Groupps.Where(grp => grp.GroupNum == studGroup).First().SubjectList;
                string idSub = "";
                bool inGroup = false;
                foreach (var el in subjectsGroup)
                {
                    if (el == ',')
                    {
                        if (int.Parse(idSub) == subjectId)
                            inGroup = true;

                        idSub = "";
                        continue;
                    }

                    idSub += el;
                }
                if (!inGroup)
                {
                    result = "В группе такого предмета нет.";
                    return result;
                }

                string subMarks = "";
                string newMarks = "";
                //Добавляем новую оценку
                foreach (var el in marks)
                {
                    subMarks += el;

                    if (el == ';')
                    {
                        bool flag = false;
                        string sub = "";
                        //Определяем предмет
                        foreach (var el1 in subMarks)
                        {
                            if (el1 == ':')
                            {
                                //Тот ли предмет
                                if (int.Parse(sub) == subjectId)
                                {
                                    flag = true;
                                    break;
                                }
                                else
                                    break;
                            }
                            sub += el1;
                        }

                        if (flag)
                            subMarks = subMarks[..^1] + mark + ';';

                        newMarks += subMarks;
                        subMarks = "";
                    }
                }

                student.Marks = newMarks;
                ctx.SaveChanges();
                result = "Оценка успешно добавлена.";
                return result;
            }
        }

        public string GetStudents(string group)
        {
            using (SchooolContext ctx = new())
            {
                string result = "";

                //Получаем список учеников группы
                var students = ctx.Students.Where(student => student.Group.ToLower() == group.ToLower()).ToList();
                if (students.Count() == 0)
                {
                    result = "Группы с таким названием не существует.";
                    return result;
                }

                foreach (var el in students)
                {
                    result += "Id: " + el.Id + "\tSurname: " + el.Surname + "\tName: " + el.Name + '\n';
                }

                return result;
            }
        }

        public string IssueTaskGroup(string group, string subject, string task)
        {
            using (SchooolContext ctx = new())
            {
                string result = "";
                //Проверяем группу
                try
                {
                    string groupBuff = ctx.Groupps.Where(gr => gr.GroupNum.ToLower() == group.ToLower()).First().GroupNum;
                }
                catch (InvalidOperationException ex)
                {
                    result = "Группы с таким названием нет.";
                    return result;
                }

                int subjectId = 0;
                //Проверяем предмет
                try
                {
                    subjectId = ctx.Subjectts.Where(subj => subj.SubjectName.ToLower() == subject.ToLower()).First().Id;
                }
                catch (InvalidOperationException ex)
                {
                    result = "Предмета с таким названием нет.";
                    return result;
                }

                //Есть ли такой предмет в группе студента
                string subjectsGroup = ctx.Groupps.Where(grp => grp.GroupNum == group).First().SubjectList;
                string idSub = "";
                bool inGroup = false;
                foreach (var el in subjectsGroup)
                {
                    if (el == ',')
                    {
                        if (int.Parse(idSub) == subjectId)
                            inGroup = true;

                        idSub = "";
                        continue;
                    }

                    idSub += el;
                }
                if (!inGroup)
                {
                    result = "В группе такого предмета нет.";
                    return result;
                }

                //Проверяем есть ли задание для группы и предмета
                try
                {
                    var taskBuff = ctx.Tasks.Where(tsk => (tsk.IdSub == subjectId) && (tsk.Group.ToLower() == group.ToLower())).First();
                    //Изменяем задание и добавляем в бд
                    taskBuff.Task1 = task;
                    ctx.SaveChanges();
                    result = "Задание успешно добавлено.";
                    return result;
                }
                catch (InvalidOperationException ex)
                {
                    //Создаем новый объект задания и добавляем в бд
                    Task tsk = new() { Task1 = task, Group = group, IdSub = subjectId };
                    ctx.Tasks.Add(tsk);
                    ctx.SaveChanges();
                    result = "Задание успешно добавлено.";
                    return result;
                }
            }
        }

        public string GetTaskGroup(string group, string subject)
        {
            using (SchooolContext ctx = new())
            {
                string result = "";
                //Проверяем группу
                try
                {
                    string groupBuff = ctx.Groupps.Where(gr => gr.GroupNum.ToLower() == group.ToLower()).First().GroupNum;
                }
                catch (InvalidOperationException ex)
                {
                    result = "Группы с таким названием нет.";
                    return result;
                }

                int subjectId = 0;
                //Проверяем предмет
                try
                {
                    subjectId = ctx.Subjectts.Where(subj => subj.SubjectName.ToLower() == subject.ToLower()).First().Id;
                }
                catch (InvalidOperationException ex)
                {
                    result = "Предмета с таким названием нет.";
                    return result;
                }

                //Есть ли такой предмет в группе студента
                string subjectsGroup = ctx.Groupps.Where(grp => grp.GroupNum == group).First().SubjectList;
                string idSub = "";
                bool inGroup = false;
                foreach (var el in subjectsGroup)
                {
                    if (el == ',')
                    {
                        if (int.Parse(idSub) == subjectId)
                            inGroup = true;

                        idSub = "";
                        continue;
                    }

                    idSub += el;
                }
                if (!inGroup)
                {
                    result = "В группе такого предмета нет.";
                    return result;
                }

                //Проверяем есть ли задание для группы и предмета
                try
                {
                    string task = ctx.Tasks.Where(tsk => (tsk.IdSub == subjectId) && (tsk.Group.ToLower() == group.ToLower())).First().Task1;
                    result = task;
                    return result;
                }
                catch (InvalidOperationException ex)
                {
                    result = "Задания нет.";
                    return result;
                }
            }
        }
    }
}