using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scHOOL.UsersLogic.AdminLogic
{
    public interface IAdminLogic
    {
        string RegistrationStudent(string name, string surname, long phoneNum, string pasw, string group);
        string RegistrationTeacher(string name, string surname, long phoneNum, string pasw, string subject);
        string DeleteStudent(long phoneNum);
        string DeleteTeacher(long phoneNum);
        string GetTeacherInformation(long phoneNum);
        string CreateGroup(string groupNum, List<string> subjectList);
        string AddSubject(string subjectName);
        string CheckTimetable(string groupNum);
        string AddAtTheEndTimetable(string groupNum, int idDay, string subjectName);
        string DeleteAtTheEndTimetable(string groupNum, int idDay);
    }
}
