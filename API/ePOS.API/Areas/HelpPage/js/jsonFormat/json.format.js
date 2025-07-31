$(document).ready(function () {
    var currentDate = new Date();
    var tick = currentDate.getTime();
    
    $.ajax({

        type: "GET",
        url: "../../../example-json/" + $("#sampleResponse").attr("rel") + "/" + $("#responeJson").attr("rel") + ".html?" + tick,
        success: function (data) {
            var result = data.split("{{ErrorCode}}");
            
            $("#responeJson").html("<h4>Example Response JSON</h4><textarea class='hide RawJson_0'>" + result[0].trim() + "</textarea><div class='Canvas_0' style='overflow:auto;'></div>");
            window.formatter = new QuickJSONFormatter({
                RawJsonId: $("#responeJson").find(".RawJson_0"),
                CanvasId: $("#responeJson").find(".Canvas_0"),
                TabSize: 1,
            });
            formatter.Process();

            if (result.length > 1) {
                $("#errorResponeJson").html("<h4>Example Error Response JSON</h4>");
                for (i = 1; i < result.length; i++) {
                    $("#errorResponeJson").append("<textarea class='hide RawJson_" + i + "'>" + result[i].trim() + "</textarea><div class='Canvas_" + i + "' style='overflow:auto; border-top:1px solid #fff'></div>");
                    window.formatter = new QuickJSONFormatter({
                        RawJsonId: $("#errorResponeJson").find(".RawJson_" + i),
                        CanvasId: $("#errorResponeJson").find(".Canvas_" + i),
                        TabSize: 1,
                    });
                    formatter.Process();
                }
            }
        },
        error: function (error) {
            $("#errorResponeJson").html("No data");
        }
    });

    //Format request Json
    if ($("#requestJson").html() != null) {
        var requestJsonContent = $("#requestJson").html();
        if (requestJsonContent.length > 0) {
            $("#requestJson").html("<h4>Example Request JSON</h4><textarea class='hide RawJson_0'>" + requestJsonContent + "</textarea><div class='Canvas_0' style='overflow:auto;'></div>");
            window.formatter = new QuickJSONFormatter({
                RawJsonId: $("#requestJson").find(".RawJson_0"),
                CanvasId: $("#requestJson").find(".Canvas_0"),
                TabSize: 1,
            });
            formatter.Process();
        }
    }
    
    
});