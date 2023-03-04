using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scHOOL.UsersLogic.TeacherLogic
{
    interface ITeacherLogic
    {
        string GetStudents(string group);
        string IssueTaskGroup(string group, string subject, string task);
        string AddMarkStudent(int idStudent, string subject, int mark);

        string GetTaskGroup(string group, string subject);
    }
}