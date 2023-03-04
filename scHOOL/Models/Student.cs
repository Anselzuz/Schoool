using System;
using System.Collections.Generic;
using scHOOL.Models;

namespace scHOOL;

public partial class Student
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Surname { get; set; }

    public long PhoneNum { get; set; }

    public string? Pasw { get; set; }

    public string? Group { get; set; }

    public string? Marks { get; set; }

    public virtual Groupp? GroupNavigation { get; set; }
}
