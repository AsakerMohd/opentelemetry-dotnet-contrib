// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using Amazon.Runtime;
using OpenTelemetry.AWS;

namespace OpenTelemetry.Instrumentation.AWS.Implementation;

internal class AWSServiceHelper
{
    public AWSServiceHelper(AWSSemanticConventions semanticConventions)
    {
        this.ParameterAttributeMap =
            semanticConventions
                .ParameterMappingBuilder
                .AddAttributeAWSDynamoTableName("TableName")
                .AddAttributeAWSSQSQueueUrl("QueueUrl")
                .AddAttributeGenAiModelId("ModelId")
                .AddAttributeAWSBedrockAgentId("AgentId")
                .AddAttributeAWSBedrockDataSourceId("DataSourceId")
                .AddAttributeAWSBedrockGuardrailId("GuardrailId")
                .AddAttributeAWSBedrockKnowledgeBaseId("KnowledgeBaseId")
                .AddAttributeAWSSQSQueueName("QueueName")
                .AddAttributeAWSS3BucketName("BucketName")
                .AddAttributeAWSKinesisStreamName("StreamName")
                .AddAttributeAWSSNSTopicArn("TopicArn")
                .AddAttributeAWSSecretsManagerSecretArn("ARN")
                .AddAttributeAWSSecretsManagerSecretArn("SecretId")
                .AddAttributeAWSStepFunctionsActivityArn("ActivityArn")
                .AddAttributeAWSStepFunctionsStateMachineArn("StateMachineArn")
                .AddAttributeAWSLambdaResourceMappingId("UUID")
                .Build();
    }

    internal static IReadOnlyDictionary<string, List<string>> ServiceRequestParameterMap { get; } = new Dictionary<string, List<string>>()
    {
        { AWSServiceType.DynamoDbService, ["TableName"] },
        { AWSServiceType.SQSService, ["QueueUrl", "QueueName"] },
        { AWSServiceType.BedrockAgentService, ["AgentId", "KnowledgeBaseId", "DataSourceId"] },
        { AWSServiceType.BedrockAgentRuntimeService, ["AgentId", "KnowledgeBaseId"] },
        { AWSServiceType.BedrockRuntimeService, ["ModelId"] },
        { AWSServiceType.S3Service, ["BucketName"] },
        { AWSServiceType.KinesisService, ["StreamName"] },
        { AWSServiceType.LambdaService, ["UUID"] },
        { AWSServiceType.SecretsManagerService, ["SecretId"] },
        { AWSServiceType.SNSService, ["TopicArn"] },
        { AWSServiceType.StepFunctionsService, ["ActivityArn", "StateMachineArn"] },
    };

    internal static IReadOnlyDictionary<string, List<string>> ServiceResponseParameterMap { get; } = new Dictionary<string, List<string>>()
    {
        { AWSServiceType.BedrockService, ["GuardrailId"] },
        { AWSServiceType.BedrockAgentService, ["AgentId", "DataSourceId"] },
        { AWSServiceType.SecretsManagerService, ["ARN"] },
        { AWSServiceType.SQSService, ["QueueUrl"] },
    };

    internal IDictionary<string, string> ParameterAttributeMap { get; }

    internal static string GetAWSServiceName(IRequestContext requestContext)
        => Utils.RemoveAmazonPrefixFromServiceName(requestContext.ServiceMetaData.ServiceId);

    internal static string GetAWSOperationName(IRequestContext requestContext)
    {
        var completeRequestName = requestContext.OriginalRequest.GetType().Name;
        var suffix = "Request";
        var operationName = Utils.RemoveSuffix(completeRequestName, suffix);
        return operationName;
    }
}
