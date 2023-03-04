using scHOOL.UsersLogic;
using scHOOL.Forms.StudentForms;
using scHOOL.Forms.TeacherForms;

namespace scHOOL
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new MainForm());

            switch(UserInformation.role)
            {
                //Role - Admin
                case 0:
                    Application.Run(new AdminMainForm());
                    break;
                //Role - Student
                case 1:
                    Application.Run(new StudentMainForm());
                    break;
                //Role - Teacher
                case 2:
                    Application.Run(new TeacherMainForm());
                    break;
            }
        }
    }
}