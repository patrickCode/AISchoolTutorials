using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Data.DataView;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers;
using myMLNET.Common.Models;
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
                    new TextLoader.Column("Label", DataKind.Single, 4)
                },
                hasHeader: true // First line of the data file is a header and not data row
            );

            IDataView trainingData = reader.Load(TrainDataPath);
            Preview(trainingData);

            // Creates a pipeline - All operations like data operations, transformation, training are bundled as a pipeline
            var pipeline =
                mlContext.Transforms.Concatenate(outputColumnName: "Features", inputColumnNames: CocoaPercent)
                .Append(mlContext.Regression.Trainers.PoissonRegression());

            // Train the model
            TransformerChain<RegressionPredictionTransformer<PoissonRegressionModelParameters>> model = pipeline.Fit(trainingData);

            // Using the training for one-time prediction
            var predictionEngine = model.CreatePredictionEngine<ChocolateInput, ChocolateOutput>(mlContext);

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

        static void Preview(IDataView traningData, int maxRows = 10)
        {
            var preview = traningData.Preview(maxRows);
            Console.WriteLine("**************************************");
            Console.WriteLine($"Loaded training data: {preview}");
            Console.WriteLine("**************************************");

            foreach (ColumnInfo columnInfo in preview.ColumnView)
            {
                Console.Write($"{columnInfo.Column.Name} ");
            }
            Console.WriteLine();

            foreach (RowInfo rowInfo in preview.RowView)
            {
                foreach (KeyValuePair<string, object> row in rowInfo.Values)
                {
                    Console.Write($"{row.Value} ");
                }
                Console.WriteLine();
            }
            Console.WriteLine("**************************************");
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
