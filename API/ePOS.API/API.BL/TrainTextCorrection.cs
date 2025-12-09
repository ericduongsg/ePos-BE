using Microsoft.ML;
using System;
using System.Collections.Generic;
using System.Linq;
using Web.BO;

namespace API.BL
{
    public static class TypoCorrector
    {
        //public static ITransformer TrainCorrectionModel(MLContext mlContext, IEnumerable<CorrectionData> data)
        //{
        //    var trainingData = mlContext.Data.LoadFromEnumerable(data);

        //    var pipeline = mlContext.Transforms.Text.FeaturizeText("Features", nameof(CorrectionData.InputText))
        //        .Append(mlContext.Transforms.Conversion.MapValueToKey("Label", nameof(CorrectionData.CorrectedText)))
        //        .Append(mlContext.MulticlassClassification.Trainers.SdcaMaximumEntropy("Label", "Features"))
        //        .Append(mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabelDecoded", "PredictedLabel"));

        //    return pipeline.Fit(trainingData);
        //}
        //public static string PredictCorrection(MLContext mlContext, ITransformer model, string input)
        //{
        //    var predictor = mlContext.Model.CreatePredictionEngine<CorrectionData, CorrectionPrediction>(model);

        //    var result = predictor.Predict(new CorrectionData { InputText = input });
        //    return result.PredictedText;
        //}

    }


}