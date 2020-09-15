using System.Linq;
using System.Collections.Generic;

namespace ICM_Prediction_POC
{
    public class DataPointFactory : IDataPointFactory
    {
        private readonly IUserDataProvider _userDataFactory;

        public DataPointFactory(IUserDataProvider userDataFactory)
        {
            _userDataFactory = userDataFactory;
        }

        public List<DataPoint> GenerateData()
        {
            List<UserData> userDataList = _userDataFactory.Get();
            return userDataList.Select(userData => userData.ConvertToDataPoint()).ToList();
        }
    }
}
