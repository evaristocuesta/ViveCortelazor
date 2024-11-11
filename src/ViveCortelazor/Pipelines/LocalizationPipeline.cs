﻿namespace ViveCortelazor.Pipelines;

public class LocalizationPipeline
{
    public void Configure(IApplicationBuilder app, RequestLocalizationOptions options)
    {
        app.UseRequestLocalization(options);
    }
}
