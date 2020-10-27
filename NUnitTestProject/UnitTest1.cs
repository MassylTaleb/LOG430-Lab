using NUnit.Framework;
using LOG430_TP;
using System.Linq;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace NUnitTestProject
{
    public class Tests
    {

        private const string sampleURL = @"D:\Users\Nicolas\Documents\ETS\Session8\LOG430\Lab1\Données de test - Détecteurs-20200916\sample.txt";
        private const int numberOfData = 7;
        private List<float> values;
        private const int TopicIndex = 1;
        private const int PayloadIndex = 2;
        private const string TargetTopic = "worldcongress2017/pilot_resologi/odtf1/ca/qc/mtl/mobil/traf/detector/det0/det-00721-01/lane0/measure0/pedest/85-per-speed";
        

        [SetUp]
        public void Setup()
        {
            var counter = 0;
            var endOfFile = false;
            System.IO.StreamReader file = new System.IO.StreamReader(sampleURL);
            values = new List<float>();
            
            while (counter <= numberOfData && !endOfFile)
            {
                var data = file.ReadLine();
                if (data == null)
                {
                    endOfFile = true;
                    continue;
                }

                var topic = data.Split(';')[TopicIndex];
                if (!topic.Equals(TargetTopic))
                {
                    continue;
                }

                values.Add(GetValue(data));
                counter++;
            }
        }

        [Test]
        public void MeanTest()
        {
            var average = values.Average();
            Assert.That(values.Average() == 10.0);
        }


        private float GetValue(string dataLine)
        {
            var payloadString = dataLine.Split(';')[PayloadIndex]; 
            var payload = JsonSerializer.Deserialize<PayloadModel>(payloadString);
            var value = payload.Value.ToString();

            if (float.TryParse(value,out float result))
            {
                return result;
            }

            return 0;
        }

    }
}