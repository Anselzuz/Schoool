using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scHOOL.UsersLogic.StudentLogic
{
    interface IStudentLogic
    {
        string CheckTimetable();
        string CheckTask();
        string CheckAllMarks();
        string CheckMarks(string subject);
    }
}
