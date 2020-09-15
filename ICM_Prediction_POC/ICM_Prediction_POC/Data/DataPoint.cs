using Microsoft.ML.Data;

namespace ICM_Prediction_POC
{
    public class DataPoint
    {
        public uint Label { get; set; }
        [VectorType(7)]
        public float[] Features { get; set; }
    }
}
