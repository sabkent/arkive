using System;
using System.Collections.Generic;
using System.Text;

namespace pws.Shared.Domain
{
    public sealed class Worker
    {
        public Worker(string name)
        {
            Name = name;
        }
        public string Name { get; private set; }
        public List<WorkItem> WorkItems { get; private set; }
    }
}
