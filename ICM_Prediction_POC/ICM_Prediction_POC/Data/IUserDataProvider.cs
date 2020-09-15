using System.Collections.Generic;

namespace ICM_Prediction_POC
{
    public interface IUserDataProvider
    {
        List<UserData> Get();
    }
}
