using System;
using System.Linq;
using System.Collections.Generic;

namespace ICM_Prediction_POC.Model
{
    public class ModelRunner
    {
        private readonly IModel _model;
        private readonly IDataPointFactory _dataPointFactory;

        public ModelRunner(IModel model, IDataPointFactory dataPointFactory)
        {
            _model = model;
            _dataPointFactory = dataPointFactory;
        }

        public void Prepare()
        {
            List<DataPoint> dataPoints = _dataPointFactory.GenerateData();
            List<DataPoint> trainingData = new List<DataPoint>();
            List<DataPoint> testData = new List<DataPoint>();

            var randomGenerator = new Random();
            foreach (var dataPoint in dataPoints)
            {
                int random = randomGenerator.Next(1, 10);
                if (random <= 8)
                    trainingData.Add(dataPoint);
                else
                    testData.Add(dataPoint);
            }

            _model.Train(trainingData);
            _model.Test(testData, printConfusionMatrix: false);
        }

        public string PredictIssue(UserData data)
        {
            DataPoint dataPoint = data.ConvertToDataPoint();
            uint label = _model.Predict(dataPoint);
            if (UserData.IssueLabel.ContainsValue(label))
            {
                string issue = UserData.IssueLabel.FirstOrDefault(issueLabel => issueLabel.Value == label).Key;
                return $"{issue} - {UserData.IssueDetails[issue]}";
            }
            return "UN-PREDICTABLE";
        }
    }
}
