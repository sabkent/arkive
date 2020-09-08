using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace pws.Shared.Domain
{
    public sealed class WorkItem
    {
        public Guid Id { get; set; }
        public string Description { get; private set; }
    }
}
