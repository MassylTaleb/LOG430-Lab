using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOG430_TP
{
    public class Aggregator
    {
        //index for finding the data in the txt file
        private const int TopicIndex = 1;
        private const int PayloadIndex = 2;
        private string testTargetUrl;

        private string Average()
        {

            return "";
        }

        private string StandardDeviation()
        {
            return "";
        }

        private string Median()
        {
            return "";
        }

        private string TestData()
        {
            //to modify
            testTargetUrl = @"D:\Users\Nicolas\Documents\ETS\Session8\LOG430\Lab1\Données de test - Détecteurs-20200916\sample.txt";
            string[] samples = System.IO.File.ReadAllLines(testTargetUrl);
            return "";
        }

    }
}
