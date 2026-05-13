using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTS2026_GrupaB.Test
{
    public enum Grade
    {
        Bad,
        Average,
        Good
    }
    internal class Parser
    {
        public static IEnumerable<TestCaseData> GetTestCasesData(string fileName)
        {
            string path = $@"{AppDomain.CurrentDomain.BaseDirectory}{fileName}";
            string[] lines = File.ReadAllLines(path);

            List<TestCaseData> testCases = new List<TestCaseData>();
            int skipStart = 3;

            foreach (string line in lines)
            {
                if (skipStart > 0) { skipStart--; continue; }
                if (string.IsNullOrWhiteSpace(line)) continue;

                string[] chars = line.Split('\t');
                int gold = Convert.ToInt32(chars[0]);
                int hiddenGold = Convert.ToInt32(chars[1]);
                bool canUncover = bool.Parse(chars[2]);
                Grade grade = Enum.Parse(typeof(Grade), chars[3]);

                testCases.Add(new TestCaseData(gold, hiddenGold, canUncover, grade));
            }
            return testCases;
        }
    }
}
