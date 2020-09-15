using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Calibrators;
using Microsoft.ML.Trainers;
using myMLNET.Common.Models;
using myMLNET.Common.Utils;

namespace myMLNET
{
    class MultipleLinearRegression
    {
        private static string TrainingDataPath = Path.Combine(Environment.CurrentDirectory, "data", "chocolate-data-multiple-linear-regression.txt");
        
        static void Main(string[] args)
        {
            var mlContext = new MLContext();

            TextLoader reader = mlContext.Data.CreateTextLoader(
                columns: new TextLoader.Column[]
                {
                    new TextLoader.Column("Weight", DataKind.Single, 0),
                    new TextLoader.Column("CocoaPercent", DataKind.Single, 1),
                    new TextLoader.Column("Cost", DataKind.Single, 2),
                    new TextLoader.Column("Label", DataKind.Single, 3) // Label is customer happiness. The predicted feature is called the label
                },
                hasHeader: true);
            IDataView trainingData = reader.Load(TrainingDataPath);
            PreviewUtil.Show(trainingData);

            var pipeline =
                mlContext.Transforms.Concatenate("Features", "Weight")
                .Append(mlContext.Regression.Trainers.LbfgsPoissonRegression());

            var trainingModel = pipeline.Fit(trainingData);


            /* Get the graph data */
            
            /* Build the graph */

            /* Calculate metrics */

            /* Get the predictions */

            /* Show Graph 3D */
            // TODO: Add Graph, update instructions

            Console.ReadKey();
        }


        private static void PrintMetrics(MLContext mlContext, IDataView predictions)
        {

        }

        public class ChocolateInput
        {
            public float Weight { get; set; }
            public float CocoaPercent { get; set; } 
            public float Cost { get; set; } 
        }

        public class ChocolateOutput
        {
            [ColumnName("Score")]
            public float CustomerHappiness { get; set; }
        }
    }
}
