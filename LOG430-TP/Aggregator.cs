using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LOG430_TP
{
    public class Aggregator
    {

        //public string GetAverage(string dateDbut, string dateFin, string topic)
        //{
        //    //var applicationMessages = DatabaseController.GetTopic(dateDebut,dateFin,Topic)   Donne un array d'applicationMessages **************

        //    ApplicationMessage[] applicationMessages;
        //    List<PayloadModel> payloads = new List<PayloadModel>();

        //    foreach (ApplicationMessage applicationMessage in applicationMessages)
        //    {
        //        payloads.Add(JsonSerializer.Deserialize<PayloadModel>(applicationMessage.Payload));
        //    }

        //    return AverageCalculator(payloads);
        //}

        //public string GetStandardDeviation(string dateDbut, string dateFin, string topic)
        //{
        //    //var applicationMessages = DatabaseController.GetTopic(dateDebut,dateFin,Topic)   Donne un array d'applicationMessages **************

        //    ApplicationMessage[] applicationMessages;
        //    List<PayloadModel> payloads = new List<PayloadModel>();

        //    foreach (ApplicationMessage applicationMessage in applicationMessages)
        //    {
        //        payloads.Add(JsonSerializer.Deserialize<PayloadModel>(applicationMessage.Payload));
        //    }

        //    return StandardDeviation(payloads);
        //}

        //public string GetMedian(string dateDbut, string dateFin, string topic)
        //{
        //    //var applicationMessages = DatabaseController.GetTopic(dateDebut,dateFin,Topic)   Donne un array d'applicationMessages **************

        //    ApplicationMessage[] applicationMessages;
        //    List<PayloadModel> payloads = new List<PayloadModel>();

        //    foreach (ApplicationMessage applicationMessage in applicationMessages)
        //    {
        //        payloads.Add(JsonSerializer.Deserialize<PayloadModel>(applicationMessage.Payload));
        //    }

        //    return Median(payloads);
        //}

        private string AverageCalculator(List<PayloadModel> payloads)
        {
            PayloadModel firstPayload = payloads.First();
            if ((firstPayload.Value as double?) == null)
            {
                return "Bad Format";
            }

            return payloads.Average(p => Convert.ToDouble(p.Value)) + firstPayload.Unit;
        }

        private string StandardDeviation(List<PayloadModel> payloads)
        {
            PayloadModel firstPayload = payloads.First();
            if ((firstPayload.Value as double?) == null)
            {
                return "Bad Format";
            }

            var average = double.Parse(AverageCalculator(payloads));
            var valueMinusAverageTotal = 0.0;
            foreach (PayloadModel payload in payloads)
            {
                valueMinusAverageTotal += Math.Pow(Convert.ToDouble(payload.Value) - average, 2);
            }


            return Math.Sqrt(valueMinusAverageTotal / payloads.Count) + firstPayload.Unit;
        }

        private string Median(List<PayloadModel> payloads)
        {
            PayloadModel firstPayload = payloads.First();
            if ((firstPayload.Value as double?) == null)
            {
                return "Bad Format";
            }

            List<double> values = new List<double>();
            foreach (PayloadModel payload in payloads)
            {
                values.Add(Convert.ToDouble(payload.Value));
            }

            values.Sort();

            if (values.Count == 1)
            {
                return values.First() + firstPayload.Unit;
            }

            if (values.Count % 2 == 0)
            {
                var firstValue = values[values.Count / 2];
                var secondValue = values[values.Count / 2 + 1];
                return (firstValue + secondValue) / 2.0 + firstPayload.Unit;
            }

            return values[(values.Count + 1) / 2] + firstPayload.Unit;

        }
    }
}
