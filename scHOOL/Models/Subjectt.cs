using System;
using System.Collections.Generic;

namespace scHOOL;

public partial class Subjectt
{
    public int Id { get; set; }

    public string? SubjectName { get; set; }

    public virtual ICollection<Task> Tasks { get; } = new List<Task>();
}
