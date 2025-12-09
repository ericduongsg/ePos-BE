using Microsoft.ML.Data;

namespace Web.BO
{
    public class CorrectionData
    {
        public string Original { get; set; }
        public string Corrected { get; set; }
    }

    public class CorrectionPrediction
    {
        public string Corrected { get; set; }
    }

}
