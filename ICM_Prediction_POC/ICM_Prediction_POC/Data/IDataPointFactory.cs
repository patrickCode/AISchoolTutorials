using System.Collections.Generic;

namespace ICM_Prediction_POC
{
    public interface IDataPointFactory
    {
        List<DataPoint> GenerateData();
    }
}