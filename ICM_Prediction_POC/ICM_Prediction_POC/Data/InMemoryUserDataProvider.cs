using System.Collections.Generic;

namespace ICM_Prediction_POC
{
    public class InMemoryUserDataProvider: IUserDataProvider
    {
        public List<UserData> Get()
        {
            var baseData = new List<UserData>
            {
                new UserData
                {
                    IsPartner = false,
                    ProfileExistsInAAD = true,
                    ProfileExistyInOP = true,
                    ClaimsExists = true,
                    ResultCodeZero = true,
                    LoginExpiredMessage = false,
                    AADSTS525Issue = false,
                    Issue = "N/W Issue"
                },
                new UserData
                {
                    IsPartner = false,
                    ProfileExistsInAAD = true,
                    ProfileExistyInOP = true,
                    ClaimsExists = false,
                    ResultCodeZero = false,
                    LoginExpiredMessage = false,
                    AADSTS525Issue = false,
                    Issue = "Claims not Assigned"
                },
                new UserData
                {
                    IsPartner = true,
                    ProfileExistsInAAD = false,
                    ProfileExistyInOP = true,
                    ClaimsExists = false,
                    ResultCodeZero = false,
                    LoginExpiredMessage = false,
                    AADSTS525Issue = false,
                    Issue = "Self-Registration Not Done"
                },
                new UserData
                {
                    IsPartner = false,
                    ProfileExistsInAAD = true,
                    ProfileExistyInOP = true,
                    ClaimsExists = true,
                    ResultCodeZero = false,
                    LoginExpiredMessage = true,
                    AADSTS525Issue = false,
                    Issue = "Security Issue"
                },
                new UserData
                {
                    IsPartner = true,
                    ProfileExistsInAAD = true,
                    ProfileExistyInOP = true,
                    ClaimsExists = true,
                    ResultCodeZero = false,
                    LoginExpiredMessage = false,
                    AADSTS525Issue = true,
                    Issue = "Multiple Login"
                }
            };
            var pumpedData = PumpData(baseData, pumpFactor: 5);
            return pumpedData;
        }

        private List<UserData> PumpData(List<UserData> baseData, int pumpFactor = 5)
        {
            var pumpedData = new List<UserData>();
            foreach(var data in baseData)
            {
                for (int pumpIdx = 1; pumpIdx <= pumpFactor; pumpIdx++)
                {
                    pumpedData.Add(data);
                }
            }
            return pumpedData;
        }
    }
}
