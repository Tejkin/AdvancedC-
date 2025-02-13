using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thomas_Chen_Task_Manager
{
    public class TaskDescriptionComparer : IComparer<Task>
    {
        public int Compare(Task x, Task y)
        {
            return x.description.CompareTo(y.description);
        }
    }
}
