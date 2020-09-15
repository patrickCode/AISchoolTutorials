using System;
using System.IO;
using System.Linq;
using Microsoft.ML;
using Microsoft.ML.Data;
using myMLNET.Common.Models;
using System.Collections.Generic;
using myMLNET.Common.Utils;
using static Microsoft.ML.Data.DataDebuggerPreview;

namespace myMLNET
{
    class LinearRegression
    {
        private static readonly string TrainDataPath = Path.Combine(Environment.CurrentDirectory, "data", "chocolate-data.txt");
        private const string CocoaPercent = "CocoaPercent";

        static void Main(string[] args)
        {
            var mlContext = new MLContext();

            // Creates a loader which will help format the data
            var reader = mlContext.Data.CreateTextLoader(
                columns: new TextLoader.Column[]
                {
                    new TextLoader.Column(CocoaPercent, DataKind.Single, 1),
                    new TextLoader.Column("Label", DataKind.Single, 4) // Customer happiness is the label. The predicted features is called the Label.
                },
                hasHeader: true // First line of the data file is a header and not data row
            );

            IDataView trainingData = reader.Load(TrainDataPath);
            PreviewUtil.Show(trainingData);

            // Creates a pipeline - All operations like data operations, transformation, training are bundled as a pipeline
            var pipeline =
                mlContext.Transforms.Concatenate(outputColumnName: "Features", inputColumnNames: CocoaPercent)
                .Append(mlContext.Regression.Trainers.LbfgsPoissonRegression());

            // Train the model
            var trainingModel = pipeline.Fit(trainingData);

            // Using the training for one-time prediction
            var predictionEngine = mlContext.Model.CreatePredictionEngine< ChocolateInput, ChocolateOutput>(trainingModel);

            // Get the prediction
            ChocolateOutput prediction = predictionEngine.Predict(new ChocolateInput() { CocoaPercent = 100 });

            Console.WriteLine($"Predicted happiness: {prediction.CustomerHappiness}");

            ChartGeneratorUtil.PlotRegressionChart(new PlotChartGeneratorModel()
            {
                Title = "Chocolate Consumer Happiness Prediction",
                LabelX = "Cocoa Percent",
                LabelY = "Customer Happiness",
                ImageName = "CocoaPercentToHappiness.png",
                PointsList = new List<PlotChartPointsList>
                {
                    new PlotChartPointsList {Points = ChartGeneratorUtil.GetChartPointsFromFile(TrainDataPath, 1, 4).ToList()}
                },
                MaxLimitY = ChartGeneratorUtil.GetMaxColumnValueFromFile(TrainDataPath, 4) + 10
            });

            Console.ReadKey();
        }

        public class ChocolateInput
        {
            public float CocoaPercent { get; set; }

            public float Weight { get; set; }
        }

        public class ChocolateOutput
        {
            [ColumnName("Score")]
            public float CustomerHappiness { get; set; }
        }
    }
}
