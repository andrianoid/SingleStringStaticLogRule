using System.Collections.Generic;
using System.Linq;
using UiPath.Studio.Activities.Api;
using UiPath.Studio.Activities.Api.Analyzer;
using UiPath.Studio.Activities.Api.Analyzer.Rules;
using UiPath.Studio.Analyzer.Models;

namespace SingleStringStaticLogRule
{
    public class SingleStringStaticLogRule : IRegisterAnalyzerConfiguration
    {
        public void Initialize(IAnalyzerConfigurationService workflowAnalyzerConfigService)
        {

            // Verify that the correct Studio Analyzer version is used
            if (!workflowAnalyzerConfigService.HasFeature("WorkflowAnalyzerV4"))
            {
                // Only allow V4
                return;
            }

            // Create the rule definition
            var logMessageConstraint = new Rule<IActivityModel>("SingleStringStaticLogs", "ST-USG-099", SingleStringStaticLog);
            logMessageConstraint.DefaultErrorLevel = System.Diagnostics.TraceLevel.Error;
            logMessageConstraint.Parameters.Add("whitelist_keywords", new Parameter()
            {
                DefaultValue = "",
                Key = "whitelist_keywords",
                LocalizedDisplayName = "Whitelist of Keywords"
            });

            // Add the rule to the config service
            workflowAnalyzerConfigService.AddRule<IActivityModel>(logMessageConstraint);
        }

        private InspectionResult SingleStringStaticLog(IActivityModel activityToInspect, Rule configuredRule)
        {
            var messageList = new List<InspectionMessage>();

            var whitelist_string = configuredRule.Parameters["whitelist_keywords"]?.Value;
            var whitelist = whitelist_string.Split(',');

            var error_level = configuredRule.ErrorLevel;

            foreach (var activityArgument in activityToInspect.Arguments)
            {
                if (activityArgument.DisplayName == "Message")
                {
                    // check against whitelist
                    foreach(var item in whitelist)
                    {
                        if (activityArgument.DefinedExpression.Contains(item))
                        {
                            // It violates the basic rule, but allow it as an exception because it's in the whitelist
                            error_level = System.Diagnostics.TraceLevel.Warning;
                        }
                    }
                    // get count of double-quotes
                    int count = activityArgument.DefinedExpression.Count(f => f == '"');

                    //// Validate rules

                    // Require exactly two quotes
                    if (!count.Equals(2))
                    {
                        messageList.Add(new InspectionMessage()
                        {
                            Message = $"Only static log messages are permitted: {activityArgument.DefinedExpression}"
                        });
                        continue;
                    }
                    // Require first character to be a quote
                    if (!activityArgument.DefinedExpression.First().Equals('"'))
                    {
                        messageList.Add(new InspectionMessage()
                        {
                            Message = $"First character must be a double-quote: {activityArgument.DefinedExpression}"
                        });
                    }
                    // Require last character to be a quote
                    if (!activityArgument.DefinedExpression.Last().Equals('"'))
                    {
                        messageList.Add(new InspectionMessage()
                        {
                            Message = $"Last character must be a double-quote: {activityArgument.DefinedExpression}"
                        });
                    }
                }
            }

            if (messageList.Count > 0)
            {
                return new InspectionResult()
                {
                    HasErrors = true,
                    InspectionMessages = messageList,
                    RecommendationMessage = $"Only static log messages are permitted",
                    ErrorLevel = error_level
                };
            }

            return new InspectionResult() { HasErrors = false };
        }
    }
}
