using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Thomas_Chen_Task_Manager
{
    public class ParserTester
    {
        public void RunTests()
        {
            var parser = new NaturalLanguageParser();

            var testCases = new[]
            {
                "Call Mom, three PM, Wednesday",
                "Meeting tomorrow at 10 AM",
                "Call Rob on Wednesday at three PM",
                "Doctor’s appointment next Tuesday at nine pm",
                "Gym at 6 PM on Friday",
                "Dinner party next Saturday at 7 PM",
                "Project deadline this Thursday",
                "Dentist appointment at 3pm",
                "Meeting next Wednesday",
                "Read book",
                "Visit Grandma, Wednesday, three PM",
                "Groceries 4 PM today",
            };

            foreach (var testCase in testCases)
            {
                var result = parser.ParseTask(testCase);
                Debug.WriteLine($"Original: {testCase}");
                Debug.WriteLine($"Task: {result.Task}");
                Debug.WriteLine($"Date: {result.Date?.ToString("dd/MM/yyyy") ?? "N/A"}");
                Debug.WriteLine($"Time: {result.Time?.ToString() ?? "N/A"}");
                Debug.WriteLine("--------------------------------------------------");
            }
        }
    }
}

