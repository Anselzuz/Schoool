using System;
using System.Collections.Generic;
using scHOOL.Models;

namespace scHOOL;

public partial class Task
{
    public int Id { get; set; }

    public string? Task1 { get; set; }

    public int? IdSub { get; set; }

    public string? Group { get; set; }

    public virtual Groupp? GroupNavigation { get; set; }

    public virtual Subjectt? IdSubNavigation { get; set; }
}
