using System;
using System.Collections.Generic;

namespace scHOOL.Models;

public partial class Groupp
{
    public string GroupNum { get; set; } = null!;

    public string? Timetable { get; set; }

    public string? SubjectList { get; set; }

    public virtual ICollection<Student> Students { get; } = new List<Student>();

    public virtual ICollection<Task> Tasks { get; } = new List<Task>();
}
