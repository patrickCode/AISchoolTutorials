using ICM_Prediction_POC.Data;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers;
using System.Collections.Generic;

namespace ICM_Prediction_POC.Model
{
    public interface IModel
    {
        bool IsModelTrained { get; }

        uint Predict(DataPoint dataPoint);
        List<PredictionModel> Test(List<DataPoint> testDataPoints, bool printConfusionMatrix = true);
        void Train(List<DataPoint> trainingDataPoints, bool reTrain = false);
    }
}