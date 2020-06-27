using System;
using System.Collections.Generic;
using Microsoft.ML;
using Microsoft.ML.Data;
using static Microsoft.ML.Data.DataDebuggerPreview;

namespace myMLNET
{
    class IntroductionToMLNET
    {
        static void Main(string[] args)
        {
            var mlContext = new MLContext();
            IDataView trainingDataView = mlContext.Data.LoadFromTextFile<ChocolateData>(path: "data/chocolate-data.txt", hasHeader: true);

            var preview = trainingDataView.Preview(10);
            Console.WriteLine($"******************************************");
            Console.WriteLine($"Loaded training data: {preview}");
            Console.WriteLine($"******************************************");

            foreach (var columnInfo in preview.ColumnView)
            {
                Console.Write($"{columnInfo.Column.Name},");
            }
            Console.WriteLine();


            foreach (RowInfo rowInfo in preview.RowView)
            {
                foreach(KeyValuePair<string, object> row in rowInfo.Values)
                {
                    Console.Write($"{row.Value} ");
                }
                Console.WriteLine();
            }
            Console.ReadKey();
        }
    }

    public class ChocolateData
    {
        [LoadColumn(0)]
        public int Weight;

        [LoadColumn(1)]
        public int CocoaPercent;

        [LoadColumn(2)]
        public int SugarPercent;

        [LoadColumn(3)]
        public int MilkPercent;

        [LoadColumn(4)]
        public int CustomerHappiness;
    }
}
