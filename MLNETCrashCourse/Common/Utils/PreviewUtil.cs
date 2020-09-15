using System;
using Microsoft.ML;
using System.Collections.Generic;
using static Microsoft.ML.Data.DataDebuggerPreview;

namespace myMLNET.Common.Utils
{
    public static class PreviewUtil
    {
        public static void Show(IDataView traningData, int maxRows = 10)
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
    }
}
