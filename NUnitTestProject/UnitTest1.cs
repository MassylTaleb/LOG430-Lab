using NUnit.Framework;
using LOG430_TP;
using System.Linq;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Text.RegularExpressions;
using LOG430_TP.Models.StatisticComputers;
using System;
using System.IO;

namespace NUnitTestProject
{
    public class Tests
    {
        private const string dataFileName = @"..\..\..\detector1.txt";
        private const int numberOfData = 7;
        private List<float> values;
        private const int TopicIndex = 1;
        private const int PayloadIndex = 2;
        private const string TargetTopic = "worldcongress2017/pilot_resologi/odtf1/ca/qc/mtl/mobil/traf/detector/det0/det-00721-01/lane1/measure0/bike/85-per-speed";
        

        [SetUp]
        public void Setup()
        {
            var counter = 0;
            var endOfFile = false;
            StreamReader file = new StreamReader(dataFileName);
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
            var meanComputer = new MeanComputer();
            Assert.That(meanComputer.Compute(values) == 46.5);
        }

        [Test]
        public void MedianTest()
        {
            var medianComputer = new MedianComputer();
            var ans = medianComputer.Compute(values);
            Assert.That(medianComputer.Compute(values) == 73.5);
        }

        [Test]
        public void StandardDeviationTest()
        {
            var stdDeviationComputer = new StandardDeviationComputer();
            var stdDeviation = stdDeviationComputer.Compute(values);
            Assert.That(Math.Round(stdDeviation, 2) == 51.97);
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