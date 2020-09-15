using System.Collections.Generic;

namespace ICM_Prediction_POC
{
    public class UserData
    {
        public bool IsPartner { get; set; }
        public bool ProfileExistsInAAD { get; set; } 
        public bool ProfileExistyInOP { get; set; }
        public bool ClaimsExists { get; set; }
        public bool ResultCodeZero { get; set; }
        public bool LoginExpiredMessage { get; set; }
        public bool AADSTS525Issue { get; set; }
        public string Issue { get; set; }

        public static Dictionary<string, uint> IssueLabel = new Dictionary<string, uint>
        {
            { "N/W Issue", 1 },
            { "Claims not Assigned", 2 },
            { "Self-Registration Not Done", 3 },
            { "Security Issue", 4 },
            { "Multiple Login", 5 }
        };

        public static Dictionary<string, string> IssueDetails = new Dictionary<string, string>
        {
            { "N/W Issue", "There is most probably a network issue at user's end. Ask the user to reconnect Router." },
            { "Claims not Assigned", "Ask One Profile team to re-scope claims for the user." },
            { "Self-Registration Not Done", "Ask partner user to go to https://aka.ms/esxpinvite and complete the registration process." },
            { "Security Issue", "The issue is likely due to 3rd party cookies getting blocked by the user's browser. Ask user to whitelist https://login.microsoftonline.com." },
            { "Multiple Login", "Ask use to launch browser in in-cognito (chrome) or in-private (edge) mode." }
        };

        public DataPoint ConvertToDataPoint()
        {
            return new DataPoint()
            {
                Features = new float[] 
                {
                    IsPartner ? 1 : 0,
                    ProfileExistsInAAD ? 1: 0,
                    ProfileExistyInOP ? 1: 0,
                    ClaimsExists ? 1: 0,
                    ResultCodeZero ? 1: 0,
                    LoginExpiredMessage ? 1: 0,
                    AADSTS525Issue ? 1: 0,
                },
                Label = !string.IsNullOrWhiteSpace(Issue) && IssueLabel.ContainsKey(Issue) ? IssueLabel[Issue] : 0,
            };
        }
    }
}
