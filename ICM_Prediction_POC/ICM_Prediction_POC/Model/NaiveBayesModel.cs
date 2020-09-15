using System;
using System.Linq;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers;
using ICM_Prediction_POC.Data;
using System.Collections.Generic;

namespace ICM_Prediction_POC.Model
{
    public class NaiveBayesModel : IModel
    {
        private TransformerChain<MulticlassPredictionTransformer<NaiveBayesMulticlassModelParameters>> _trainedModel = null;
        public bool IsModelTrained => _trainedModel != null;
        private readonly MLContext _mlContext;

        public NaiveBayesModel()
        {
            _mlContext = new MLContext();
        }

        public void Train(List<DataPoint> trainingDataPoints, bool reTrain = false)
        {
            if (_trainedModel != null && !reTrain)
                return;

            IDataView trainingData = _mlContext.Data.LoadFromEnumerable(trainingDataPoints);

            var pipeline = _mlContext
                .Transforms.Conversion.MapValueToKey(nameof(DataPoint.Label))
                .Append(_mlContext.MulticlassClassification.Trainers.NaiveBayes());

            _trainedModel = pipeline.Fit(trainingData);
        }

        public List<PredictionModel> Test(List<DataPoint> testDataPoints, bool printConfusionMatrix = true)
        {
            if (!IsModelTrained)
                throw new Exception("Cannot test on untrained model");

            IDataView testData = _mlContext.Data.LoadFromEnumerable(testDataPoints);
            IDataView transformedTestData = _trainedModel.Transform(testData);

            List<PredictionModel> predictions = _mlContext.Data
                .CreateEnumerable<PredictionModel>(transformedTestData, reuseRowObject: false).ToList();

            if (printConfusionMatrix)
                PrintMetrics(_mlContext.MulticlassClassification.Evaluate(transformedTestData));

            return predictions;
        }

        public uint Predict(DataPoint dataPoint)
        {
            IDataView data = _mlContext.Data.LoadFromEnumerable(new List<DataPoint> { dataPoint });
            IDataView transformedData = _trainedModel.Transform(data);
            //return transformedData.GetColumn<uint>(nameof(DataPoint.Label)).First();
            var prediction = _mlContext.Data.CreateEnumerable<PredictionModel>(transformedData, reuseRowObject: false).ToList().FirstOrDefault();
            return prediction.PredictedLabel;
        }

        private void PrintMetrics(MulticlassClassificationMetrics metrics)
        {
            Console.WriteLine(metrics.ConfusionMatrix.GetFormattedConfusionTable());
        }
    }
}
