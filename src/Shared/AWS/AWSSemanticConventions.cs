// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.ComponentModel;
using System.Diagnostics;

#if INSTRUMENTATION_AWSLAMBDA
using OpenTelemetry.Instrumentation.AWSLambda;
#elif INSTRUMENTATION_AWS
using OpenTelemetry.Instrumentation.AWS;
#elif RESOURCES_AWS
using OpenTelemetry.Resources.AWS;
#endif

// disable Style Warnings to improve readability of this specific file.
#pragma warning disable SA1124
#pragma warning disable SA1005
#pragma warning disable SA1514
#pragma warning disable SA1201
#pragma warning disable SA1623

namespace OpenTelemetry.AWS;

/// <summary>
/// Abstracts the complexities of honoring <see cref="SemanticConventionVersion"/>.
/// <para/>
/// Classes emitting attributes can use the extension methods in this class to build
/// a List of <see cref="KeyValuePair{K,V}"/>s containing
/// Attribute Name and Value without needing to know which version of the
/// Semantic Convention to use.
/// <example>
/// Below is a hypothetical example showing how attributes can be constructed.  It is not necessary
/// for this consumer to accommodate differing behavior based on <see cref="SemanticConventionVersion"/>,
/// as that will be handled by the extension methods
/// (ie <see cref="AttributeBuilderImpl.AddAttributeCloudResourceId"/>) themselves.
/// <code>
/// <![CDATA[
/// var resourceAttributes =
///     awsSemanticConventions
///         .AttributeBuilder
///         .AddAttributeCloudResourceId(containerArn)
///         .AddAttributeFoo("value 1")
///         .AddAttributeBar("value 2")
///         .Build();
/// ]]>
/// </code>
/// </example>
/// </summary>
internal partial class AWSSemanticConventions
{
    /// <summary>
    /// Options classes should use this value for initialization.
    /// <para />
    /// Example:
    /// <code>
    /// <![CDATA[
    /// public class AWSLambdaInstrumentationOptions
    /// {
    ///    /// <inheritdoc cref="OpenTelemetry.AWS.SemanticConventionVersion"/>
    ///    public SemanticConventionVersion SemanticConventionVersion { get; set; } = AWSSemanticConventions.DefaultSemanticConventionVersion;
    /// }
    /// ]]>
    /// </code>
    /// </summary>
    /// <remarks>
    /// Per SemanticConventionVersion, default should stay as
    /// <see cref="SemanticConventionVersion.V1_28_0"/> until next major version bump.
    /// </remarks>
    internal const SemanticConventionVersion DefaultSemanticConventionVersion = SemanticConventionVersion.V1_28_0;

    private readonly SemanticConventionVersion semanticConventionVersion;

    /// <inheritdoc cref="AttributeBuilderImpl"/>
    public AttributeBuilderImpl AttributeBuilder { get; }

    /// <inheritdoc cref="ParameterMappingBuilderImpl"/>
    public ParameterMappingBuilderImpl ParameterMappingBuilder { get; }

    /// <inheritdoc cref="TagBuilderImpl"/>
    public TagBuilderImpl TagBuilder { get; }

    /// <inheritdoc cref="TagExtractorImpl"/>
    public TagExtractorImpl TagExtractor { get; }

    /// <inheritdoc cref="AWSSemanticConventions"/>
    /// <param name="semanticConventionVersion">
    /// Sets the <see cref="SemanticConventionVersion"/> that will be used to resolve attribute names.
    /// </param>
    public AWSSemanticConventions(SemanticConventionVersion semanticConventionVersion = DefaultSemanticConventionVersion)
    {
        this.semanticConventionVersion = semanticConventionVersion;
        this.AttributeBuilder = new(this);
        this.ParameterMappingBuilder = new(this);
        this.TagBuilder = new(this);
        this.TagExtractor = new(this);
    }

    /// <summary>
    /// Build a Dictionary of Attribute Names.
    /// </summary>
    public class ParameterMappingBuilderImpl
    {
        private readonly AWSSemanticConventions awsSemanticConventions;
        private Dictionary<string, string> state = [];

        public ParameterMappingBuilderImpl(AWSSemanticConventions semanticConventions)
        {
            this.awsSemanticConventions = semanticConventions;
        }

        public void Add(string key, string value) => this.state.Add(key, value);

        public IDictionary<string, string> Build()
        {
            var builtState = this.state;

            this.state = [];

            return builtState;
        }

        #region Service Parameter Mapping
        /// <inheritdoc cref="AWSSemanticConventionsBase.AttributeAWSDynamoTableName"/>
        public ParameterMappingBuilderImpl AddAttributeAWSDynamoTableName(string value)
            => this.awsSemanticConventions.AddDic(this, x => x.AttributeAWSDynamoTableName, value);

        /// <inheritdoc cref="AWSSemanticConventionsBase.AttributeAWSSQSQueueUrl"/>
        public ParameterMappingBuilderImpl AddAttributeAWSSQSQueueUrl(string value)
            => this.awsSemanticConventions.AddDic(this, x => x.AttributeAWSSQSQueueUrl, value);

        /// <inheritdoc cref="AWSSemanticConventionsBase.AttributeGenAiModelId"/>
        public ParameterMappingBuilderImpl AddAttributeGenAiModelId(string value)
            => this.awsSemanticConventions.AddDic(this, x => x.AttributeGenAiModelId, value);

        /// <inheritdoc cref="AWSSemanticConventionsBase.AttributeAWSBedrockAgentId"/>
        public ParameterMappingBuilderImpl AddAttributeAWSBedrockAgentId(string value)
            => this.awsSemanticConventions.AddDic(this, x => x.AttributeAWSBedrockAgentId, value);

        /// <inheritdoc cref="AWSSemanticConventionsBase.AttributeAWSBedrockDataSourceId"/>
        public ParameterMappingBuilderImpl AddAttributeAWSBedrockDataSourceId(string value)
            => this.awsSemanticConventions.AddDic(this, x => x.AttributeAWSBedrockDataSourceId, value);

        /// <inheritdoc cref="AWSSemanticConventionsBase.AttributeAWSBedrockGuardrailId"/>
        public ParameterMappingBuilderImpl AddAttributeAWSBedrockGuardrailId(string value)
            => this.awsSemanticConventions.AddDic(this, x => x.AttributeAWSBedrockGuardrailId, value);

        /// <inheritdoc cref="AWSSemanticConventionsBase.AttributeAWSBedrockKnowledgeBaseId"/>
        public ParameterMappingBuilderImpl AddAttributeAWSBedrockKnowledgeBaseId(string value)
            => this.awsSemanticConventions.AddDic(this, x => x.AttributeAWSBedrockKnowledgeBaseId, value);

        /// <inheritdoc cref="AWSSemanticConventionsBase.AttributeAWSSQSQueueName"/>
        public ParameterMappingBuilderImpl AddAttributeAWSSQSQueueName(string value)
            => this.awsSemanticConventions.AddDic(this, x => x.AttributeAWSSQSQueueName, value);

        /// <inheritdoc cref="AWSSemanticConventionsBase.AttributeAWSS3BucketName"/>
        public ParameterMappingBuilderImpl AddAttributeAWSS3BucketName(string value)
            => this.awsSemanticConventions.AddDic(this, x => x.AttributeAWSS3BucketName, value);

        /// <inheritdoc cref="AWSSemanticConventionsBase.AttributeAWSKinesisStreamName"/>
        public ParameterMappingBuilderImpl AddAttributeAWSKinesisStreamName(string value)
            => this.awsSemanticConventions.AddDic(this, x => x.AttributeAWSKinesisStreamName, value);

        /// <inheritdoc cref="AWSSemanticConventionsBase.AttributeAWSSNSTopicArn"/>
        public ParameterMappingBuilderImpl AddAttributeAWSSNSTopicArn(string value)
            => this.awsSemanticConventions.AddDic(this, x => x.AttributeAWSSNSTopicArn, value);

        /// <inheritdoc cref="AWSSemanticConventionsBase.AttributeAWSSecretsManagerSecretArn"/>
        public ParameterMappingBuilderImpl AddAttributeAWSSecretsManagerSecretArn(string value)
            => this.awsSemanticConventions.AddDic(this, x => x.AttributeAWSSecretsManagerSecretArn, value);

        /// <inheritdoc cref="AWSSemanticConventionsBase.AttributeAWSStepFunctionsActivityArn"/>
        public ParameterMappingBuilderImpl AddAttributeAWSStepFunctionsActivityArn(string value)
            => this.awsSemanticConventions.AddDic(this, x => x.AttributeAWSStepFunctionsActivityArn, value);

        /// <inheritdoc cref="AWSSemanticConventionsBase.AttributeAWSStepFunctionsStateMachineArn"/>
        public ParameterMappingBuilderImpl AddAttributeAWSStepFunctionsStateMachineArn(string value)
            => this.awsSemanticConventions.AddDic(this, x => x.AttributeAWSStepFunctionsStateMachineArn, value);

        /// <inheritdoc cref="AWSSemanticConventionsBase.AttributeAWSLambdaResourceMappingId"/>
        public ParameterMappingBuilderImpl AddAttributeAWSLambdaResourceMappingId(string value)
            => this.awsSemanticConventions.AddDic(this, x => x.AttributeAWSLambdaResourceMappingId, value);
        #endregion
    }

    /// <summary>
    /// Build a List of Attribute KeyValuePairs.
    /// </summary>
    public class AttributeBuilderImpl
    {
        private readonly AWSSemanticConventions awsSemanticConventions;
        private List<KeyValuePair<string, object>> state = [];

        public AttributeBuilderImpl(AWSSemanticConventions semanticConventions)
        {
            this.awsSemanticConventions = semanticConventions;
        }

        public void Add(string key, object value) => this.state.Add(new(key, value));

        public List<KeyValuePair<string, object>> Build()
        {
            var builtState = this.state;

            this.state = [];

            return builtState;
        }

        #region Cloud Attributes
        /// <inheritdoc cref="AWSSemanticConventionsBase.AttributeCloudAccountID"/>
        public AttributeBuilderImpl AddAttributeCloudAccountID(object? value, bool addIfEmpty = false) =>
            this.awsSemanticConventions.Add(this, x => x.AttributeCloudAccountID, value, addIfEmpty);

        /// <inheritdoc cref="AWSSemanticConventionsBase.AttributeCloudAvailabilityZone"/>
        public AttributeBuilderImpl AddAttributeCloudAvailabilityZone(object? value, bool addIfEmpty = false) =>
            this.awsSemanticConventions.Add(this, x => x.AttributeCloudAvailabilityZone, value, addIfEmpty);

        /// <inheritdoc cref="AWSSemanticConventionsBase.AttributeCloudPlatform"/>
        public AttributeBuilderImpl AddAttributeCloudPlatformIsAwsEc2() =>
            this.awsSemanticConventions.Add(this, x => x.AttributeCloudPlatform, x => x.CloudPlatformValuesAwsEc2);

        /// <inheritdoc cref="AWSSemanticConventionsBase.AttributeCloudPlatform"/>
        public AttributeBuilderImpl AddAttributeCloudPlatformIsAwsEcs() =>
            this.awsSemanticConventions.Add(this, x => x.AttributeCloudPlatform, x => x.CloudPlatformValuesAwsEcs);

        /// <inheritdoc cref="AWSSemanticConventionsBase.AttributeCloudPlatform"/>
        public AttributeBuilderImpl AddAttributeCloudPlatformIsAwsEks() =>
            this.awsSemanticConventions.Add(this, x => x.AttributeCloudPlatform, x => x.CloudPlatformValuesAwsEks);

        /// <inheritdoc cref="AWSSemanticConventionsBase.AttributeCloudPlatform"/>
        public AttributeBuilderImpl AddAttributeCloudPlatformIsAwsElasticBeanstalk() =>
            this.awsSemanticConventions.Add(this, x => x.AttributeCloudPlatform, x => x.CloudPlatformValuesAwsElasticBeanstalk);

        /// <inheritdoc cref="AWSSemanticConventionsBase.AttributeCloudProvider"/>
        public AttributeBuilderImpl AddAttributeCloudProviderIsAWS() =>
            this.awsSemanticConventions.Add(this, x => x.AttributeCloudProvider, x => x.CloudProviderValuesAws);

        /// <inheritdoc cref="AWSSemanticConventionsBase.AttributeCloudRegion"/>
        public AttributeBuilderImpl AddAttributeCloudRegion(object? value, bool addIfEmpty = false) =>
            this.awsSemanticConventions.Add(this, x => x.AttributeCloudRegion, value, addIfEmpty);

        /// <inheritdoc cref="AWSSemanticConventionsBase.AttributeCloudResourceId"/>
        public AttributeBuilderImpl AddAttributeCloudResourceId(object? value, bool addIfEmpty = false) =>
            this.awsSemanticConventions.Add(this, x => x.AttributeCloudResourceId, value, addIfEmpty);
        #endregion

        #region Container
        /// <inheritdoc cref="AWSSemanticConventionsBase.AttributeContainerID"/>
        public AttributeBuilderImpl AddAttributeContainerId(object? value, bool addIfEmpty = false) =>
            this.awsSemanticConventions.Add(this, x => x.AttributeContainerID, value, addIfEmpty);
        #endregion

        #region AWS
        /// <inheritdoc cref="AWSSemanticConventionsBase.AttributeEcsContainerArn"/>
        public AttributeBuilderImpl AddAttributeEcsContainerArn(object? value, bool addIfEmpty = false) =>
            this.awsSemanticConventions.Add(this, x => x.AttributeEcsContainerArn, value, addIfEmpty);

        /// <inheritdoc cref="AWSSemanticConventionsBase.AttributeEcsClusterArn"/>
        public AttributeBuilderImpl AddAttributeEcsClusterArn(object? value, bool addIfEmpty = false) =>
            this.awsSemanticConventions.Add(this, x => x.AttributeEcsClusterArn, value, addIfEmpty);

        /// <inheritdoc cref="AWSSemanticConventionsBase.AttributeEcsLaunchtype"/>
        public AttributeBuilderImpl AddAttributeEcsLaunchtypeIsEc2() =>
            this.awsSemanticConventions.Add(this, x => x.AttributeEcsLaunchtype, x => x.ValueEcsLaunchTypeEc2);

        /// <inheritdoc cref="AWSSemanticConventionsBase.AttributeEcsLaunchtype"/>
        public AttributeBuilderImpl AddAttributeEcsLaunchtypeIsFargate() =>
            this.awsSemanticConventions.Add(this, x => x.AttributeEcsLaunchtype, x => x.ValueEcsLaunchTypeFargate);

        /// <inheritdoc cref="AWSSemanticConventionsBase.AttributeEcsTaskArn"/>
        public AttributeBuilderImpl AddAttributeEcsTaskArn(object? value, bool addIfEmpty = false) =>
            this.awsSemanticConventions.Add(this, x => x.AttributeEcsTaskArn, value, addIfEmpty);

        /// <inheritdoc cref="AWSSemanticConventionsBase.AttributeEcsTaskFamily"/>
        public AttributeBuilderImpl AddAttributeEcsTaskFamily(object? value, bool addIfEmpty = false) =>
            this.awsSemanticConventions.Add(this, x => x.AttributeEcsTaskFamily, value, addIfEmpty);

        /// <inheritdoc cref="AWSSemanticConventionsBase.AttributeEcsTaskRevision"/>
        public AttributeBuilderImpl AddAttributeEcsTaskRevision(object? value, bool addIfEmpty = false) =>
            this.awsSemanticConventions.Add(this, x => x.AttributeEcsTaskRevision, value, addIfEmpty);

        /// <inheritdoc cref="AWSSemanticConventionsBase.AttributeLogGroupNames"/>
        public AttributeBuilderImpl AddAttributeLogGroupNames(object? value, bool addIfEmpty = false) =>
            this.awsSemanticConventions.Add(this, x => x.AttributeLogGroupNames, value, addIfEmpty);

        /// <inheritdoc cref="AWSSemanticConventionsBase.AttributeLogGroupArns"/>
        public AttributeBuilderImpl AddAttributeLogGroupArns(object? value, bool addIfEmpty = false) =>
            this.awsSemanticConventions.Add(this, x => x.AttributeLogGroupArns, value, addIfEmpty);

        /// <inheritdoc cref="AWSSemanticConventionsBase.AttributeLogStreamNames"/>
        public AttributeBuilderImpl AddAttributeLogStreamNames(object? value, bool addIfEmpty = false) =>
            this.awsSemanticConventions.Add(this, x => x.AttributeLogStreamNames, value, addIfEmpty);

        /// <inheritdoc cref="AWSSemanticConventionsBase.AttributeLogStreamArns"/>
        public AttributeBuilderImpl AddAttributeLogStreamArns(object? value, bool addIfEmpty = false) =>
            this.awsSemanticConventions.Add(this, x => x.AttributeLogStreamArns, value, addIfEmpty);

        #endregion

        #region Faas
        /// <inheritdoc cref="AWSSemanticConventionsBase.AttributeFaasID"/>
        public AttributeBuilderImpl AddAttributeFaasID(object? value, bool addIfEmpty = false) =>
            this.awsSemanticConventions.Add(this, x => x.AttributeFaasID, value, addIfEmpty);

        /// <inheritdoc cref="AWSSemanticConventionsBase.AttributeFaasExecution"/>
        public AttributeBuilderImpl AddAttributeFaasExecution(object? value, bool addIfEmpty = false) =>
            this.awsSemanticConventions.Add(this, x => x.AttributeFaasExecution, value, addIfEmpty);

        /// <inheritdoc cref="AWSSemanticConventionsBase.AttributeFaasName"/>
        public AttributeBuilderImpl AddAttributeFaasName(object? value, bool addIfEmpty = false) =>
            this.awsSemanticConventions.Add(this, x => x.AttributeFaasName, value, addIfEmpty);

        /// <inheritdoc cref="AWSSemanticConventionsBase.AttributeFaasVersion"/>
        public AttributeBuilderImpl AddAttributeFaasVersion(object? value, bool addIfEmpty = false) =>
            this.awsSemanticConventions.Add(this, x => x.AttributeFaasVersion, value, addIfEmpty);

        /// <inheritdoc cref="AWSSemanticConventionsBase.AttributeFaasTrigger"/>
        public AttributeBuilderImpl AddAttributeFaasTrigger(object? value, bool addIfEmpty = false) =>
            this.awsSemanticConventions.Add(this, x => x.AttributeFaasTrigger, value, addIfEmpty);

        /// <inheritdoc cref="AWSSemanticConventionsBase.AttributeFaasColdStart"/>
        public AttributeBuilderImpl AddAttributeFaasColdStart(object? value, bool addIfEmpty = false) =>
            this.awsSemanticConventions.Add(this, x => x.AttributeFaasColdStart, value, addIfEmpty);

        #endregion

        #region Host
        /// <inheritdoc cref="AWSSemanticConventionsBase.AttributeHostID"/>
        public AttributeBuilderImpl AddAttributeHostID(object? value, bool addIfEmpty = false) =>
            this.awsSemanticConventions.Add(this, x => x.AttributeHostID, value, addIfEmpty);

        /// <inheritdoc cref="AWSSemanticConventionsBase.AttributeHostType"/>
        public AttributeBuilderImpl AddAttributeHostType(object? value, bool addIfEmpty = false) =>
            this.awsSemanticConventions.Add(this, x => x.AttributeHostType, value, addIfEmpty);

        /// <inheritdoc cref="AWSSemanticConventionsBase.AttributeHostName"/>
        public AttributeBuilderImpl AddAttributeHostName(object? value, bool addIfEmpty = false) =>
            this.awsSemanticConventions.Add(this, x => x.AttributeHostName, value, addIfEmpty);

        #endregion

        #region Http

        /// <inheritdoc cref="AWSSemanticConventionsBase.AttributeHttpScheme"/>
        [Obsolete("Replaced by <c>url.scheme</c> instead.")]
        public AttributeBuilderImpl AddAttributeHttpScheme(object? value, bool addIfEmpty = false) =>
            this.awsSemanticConventions.Add(this, x => x.AttributeHttpScheme, value, addIfEmpty);

        /// <inheritdoc cref="AWSSemanticConventionsBase.AttributeHttpTarget"/>
        [Obsolete("Split to <c>url.path</c> and `url.query.")]
        public AttributeBuilderImpl AddAttributeHttpTarget(object? value, bool addIfEmpty = false) =>
            this.awsSemanticConventions.Add(this, x => x.AttributeHttpTarget, value, addIfEmpty);

        /// <inheritdoc cref="AWSSemanticConventionsBase.AttributeHttpMethod"/>
        [Obsolete("Replaced by <c>http.request.method</c>.")]
        public AttributeBuilderImpl AddAttributeHttpMethod(object? value, bool addIfEmpty = false) =>
            this.awsSemanticConventions.Add(this, x => x.AttributeHttpMethod, value, addIfEmpty);

        /// <inheritdoc cref="AWSSemanticConventionsBase.AttributeHttpRequestMethod"/>
        public AttributeBuilderImpl AddAttributeHttpRequestMethod(object? value, bool addIfEmpty = false) =>
            this.awsSemanticConventions.Add(this, x => x.AttributeHttpRequestMethod, value, addIfEmpty);
        #endregion

        #region Net
        /// <inheritdoc cref="AWSSemanticConventionsBase.AttributeNetHostName"/>
        [Obsolete("Replaced by <c>server.address</c>.")]
        public AttributeBuilderImpl AddAttributeNetHostName(object? value, bool addIfEmpty = false) =>
            this.awsSemanticConventions.Add(this, x => x.AttributeNetHostName, value, addIfEmpty);

        /// <inheritdoc cref="AWSSemanticConventionsBase.AttributeNetHostPort"/>
        [Obsolete("Replaced by <c>server.port</c>.")]
        public AttributeBuilderImpl AddAttributeNetHostPort(object? value, bool addIfEmpty = false) =>
            this.awsSemanticConventions.Add(this, x => x.AttributeNetHostPort, value, addIfEmpty);
        #endregion

        #region K8s
        /// <inheritdoc cref="AWSSemanticConventionsBase.AttributeK8SClusterName"/>
        public AttributeBuilderImpl AddAttributeK8SClusterName(object? value, bool addIfEmpty = false) =>
            this.awsSemanticConventions.Add(this, x => x.AttributeK8SClusterName, value, addIfEmpty);
        #endregion

        #region Server
        /// <inheritdoc cref="AWSSemanticConventionsBase.AttributeServerAddress"/>
        public AttributeBuilderImpl AddAttributeServerAddress(object? value, bool addIfEmpty = false) =>
            this.awsSemanticConventions.Add(this, x => x.AttributeServerAddress, value, addIfEmpty);

        /// <inheritdoc cref="AWSSemanticConventionsBase.AttributeServerPort"/>
        public AttributeBuilderImpl AddAttributeServerPort(object? value, bool addIfEmpty = false) =>
            this.awsSemanticConventions.Add(this, x => x.AttributeServerPort, value, addIfEmpty);
        #endregion

        #region Service
        /// <inheritdoc cref="AWSSemanticConventionsBase.AttributeServiceName"/>
        public AttributeBuilderImpl AddAttributeServiceNameIsAwsElasticBeanstalk() =>
            this.awsSemanticConventions.Add(this, x => x.AttributeServiceName, x => x.ServiceNameValuesAwsElasticBeanstalk);

        /// <inheritdoc cref="AWSSemanticConventionsBase.AttributeServiceNamespace"/>
        public AttributeBuilderImpl AddAttributeServiceNamespace(object? value, bool addIfEmpty = false) =>
            this.awsSemanticConventions.Add(this, x => x.AttributeServiceNamespace, value, addIfEmpty);

        /// <inheritdoc cref="AWSSemanticConventionsBase.AttributeServiceInstanceID"/>
        public AttributeBuilderImpl AddAttributeServiceInstanceID(object? value, bool addIfEmpty = false) =>
            this.awsSemanticConventions.Add(this, x => x.AttributeServiceInstanceID, value, addIfEmpty);

        /// <inheritdoc cref="AWSSemanticConventionsBase.AttributeServiceVersion"/>
        public AttributeBuilderImpl AddAttributeServiceVersion(object? value, bool addIfEmpty = false) =>
            this.awsSemanticConventions.Add(this, x => x.AttributeServiceVersion, value, addIfEmpty);
        #endregion

        #region Url
        /// <inheritdoc cref="AWSSemanticConventionsBase.AttributeUrlPath"/>
        public AttributeBuilderImpl AddAttributeUrlPath(object? value, bool addIfEmpty = false) =>
            this.awsSemanticConventions.Add(this, x => x.AttributeUrlPath, value, addIfEmpty);

        /// <inheritdoc cref="AWSSemanticConventionsBase.AttributeServiceName"/>
        public AttributeBuilderImpl AddAttributeUrlQuery(object? value, bool addIfEmpty = false) =>
            this.awsSemanticConventions.Add(this, x => x.AttributeUrlQuery, value, addIfEmpty);

        /// <inheritdoc cref="AWSSemanticConventionsBase.AttributeUrlScheme"/>
        public AttributeBuilderImpl AddAttributeUrlScheme(object? value, bool addIfEmpty = false) =>
            this.awsSemanticConventions.Add(this, x => x.AttributeUrlScheme, value, addIfEmpty);
        #endregion
    }

    /// <summary>
    /// Add Attributes to <see cref="Activity"/>.
    /// </summary>
    public class TagBuilderImpl
    {
        private readonly AWSSemanticConventions awsSemanticConventions;

        public TagBuilderImpl(AWSSemanticConventions semanticConventions)
        {
            this.awsSemanticConventions = semanticConventions;
        }

        #region AWS

        /// <inheritdoc cref="AWSSemanticConventionsBase.AttributeDbSystem"/>
        public Activity? SetTagAttributeDbSystemToDynamoDb(Activity? activity)
            => this.awsSemanticConventions.SetTag(activity, x => x.AttributeDbSystem, x => x.AttributeDynamoDb);

        /// <inheritdoc cref="AWSSemanticConventionsBase.AttributeGenAiSystem"/>
        public Activity? SetTagAttributeGenAiSystemToBedrock(Activity? activity)
            => this.awsSemanticConventions.SetTag(activity, x => x.AttributeGenAiSystem, x => x.AttributeAWSBedrock);

        /// <inheritdoc cref="AWSSemanticConventionsBase.AttributeAWSRequestId"/>
        public Activity? SetTagAttributeAWSRequestId(Activity? activity, string operationName)
            => this.awsSemanticConventions.SetTag(activity, x => x.AttributeAWSRequestId, operationName);
        #endregion

        #region GEN AI

        /// <inheritdoc cref="AWSSemanticConventionsBase.AttributeGenAiTopP"/>
        public Activity? SetTagAttributeGenAiTopP(Activity? activity, double topP)
            => this.awsSemanticConventions.SetTag(activity, x => x.AttributeGenAiTopP, topP);

        /// <inheritdoc cref="AWSSemanticConventionsBase.AttributeGenAiTemperature"/>
        public Activity? SetTagAttributeGenAiTemperature(Activity? activity, double temperature)
            => this.awsSemanticConventions.SetTag(activity, x => x.AttributeGenAiTemperature, temperature);

        /// <inheritdoc cref="AWSSemanticConventionsBase.AttributeGenAiMaxTokens"/>
        public Activity? SetTagAttributeGenAiMaxTokens(Activity? activity, int maxTokens)
            => this.awsSemanticConventions.SetTag(activity, x => x.AttributeGenAiMaxTokens, maxTokens);

        /// <inheritdoc cref="AWSSemanticConventionsBase.AttributeGenAiInputTokens"/>
        public Activity? SetTagAttributeGenAiInputTokens(Activity? activity, int inputTokens)
            => this.awsSemanticConventions.SetTag(activity, x => x.AttributeGenAiInputTokens, inputTokens);

        /// <inheritdoc cref="AWSSemanticConventionsBase.AttributeGenAiOutputTokens"/>
        public Activity? SetTagAttributeGenAiOutputTokens(Activity? activity, int outputTokens)
            => this.awsSemanticConventions.SetTag(activity, x => x.AttributeGenAiOutputTokens, outputTokens);

        /// <inheritdoc cref="AWSSemanticConventionsBase.AttributeGenAiFinishReasons"/>
        public Activity? SetTagAttributeGenAiFinishReasons(Activity? activity, string[] finishReasons)
            => this.awsSemanticConventions.SetTag(activity, x => x.AttributeGenAiFinishReasons, finishReasons);

        #endregion

        #region Http

        /// <inheritdoc cref="AWSSemanticConventionsBase.AttributeHttpStatusCode"/>
        [Obsolete("Replaced by <c>http.response.status_code</c>.")]
        public Activity? SetTagAttributeHttpStatusCode(Activity? activity, int value)
            => this.awsSemanticConventions.SetTag(activity, x => x.AttributeHttpStatusCode, value);

        /// <inheritdoc cref="AWSSemanticConventionsBase.AttributeHttpResponseStatusCode"/>
        public Activity? SetTagAttributeHttpResponseStatusCode(Activity? activity, int value)
            => this.awsSemanticConventions.SetTag(activity, x => x.AttributeHttpResponseStatusCode, value);

        /// <inheritdoc cref="AWSSemanticConventionsBase.AttributeHttpResponseHeaderContentLength"/>
        public Activity? SetTagAttributeHttpResponseHeaderContentLength(Activity? activity, long value)
            => this.awsSemanticConventions.SetTag(activity, x => x.AttributeHttpResponseHeaderContentLength, value);
        #endregion

        #region Cloud

        /// <inheritdoc cref="AWSSemanticConventionsBase.AttributeCloudRegion"/>
        public Activity? SetTagAttributeCloudRegion(Activity? activity, string operationName)
            => this.awsSemanticConventions.SetTag(activity, x => x.AttributeCloudRegion, operationName);
        #endregion
    }

    /// <summary>
    /// Get Attributes from <see cref="Activity"/>.
    /// </summary>
    public class TagExtractorImpl
    {
        private readonly AWSSemanticConventions awsSemanticConventions;

        public TagExtractorImpl(AWSSemanticConventions semanticConventions)
        {
            this.awsSemanticConventions = semanticConventions;
        }

        #region AWS
        /// <inheritdoc cref="AWSSemanticConventionsBase.AttributeAWSRequestId"/>
        public object? GetTagAttributeAWSRequestId(Activity? activity)
            => this.awsSemanticConventions.GetTag(activity, x => x.AttributeAWSRequestId);
        #endregion

        #region GEN AI
        /// <inheritdoc cref="AWSSemanticConventionsBase.AttributeGenAiModelId"/>
        public object? GetTagAttributeGenAiModelId(Activity? activity)
            => this.awsSemanticConventions.GetTag(activity, x => x.AttributeGenAiModelId);
        #endregion
    }

    private AttributeBuilderImpl Add(AttributeBuilderImpl attributes, Func<AWSSemanticConventionsBase, string> attributeNameFunc, Func<AWSSemanticConventionsBase, string> valueFunc) =>
        this.Add(attributes, attributeNameFunc, valueFunc(this.GetSemanticConventionVersion()));

    private AttributeBuilderImpl Add(AttributeBuilderImpl attributes, Func<AWSSemanticConventionsBase, string> attributeNameFunc, object? value, bool addIfEmpty = false)
    {
        var semanticConventionVersionImpl = this.GetSemanticConventionVersion();

        var attributeName = attributeNameFunc(semanticConventionVersionImpl);

        // if attributeName is empty or there is no value, exit
        if (string.IsNullOrEmpty(attributeName) ||
           (string.IsNullOrEmpty(value?.ToString()) && !addIfEmpty))
        {
            return attributes;
        }

        attributes.Add(attributeName, value ?? string.Empty);

        return attributes;
    }

    private Activity? SetTag(Activity? activity, Func<AWSSemanticConventionsBase, string> attributeNameFunc, Func<AWSSemanticConventionsBase, object?> valueFunc) =>
        this.SetTag(activity, attributeNameFunc, valueFunc(this.GetSemanticConventionVersion()));

    private Activity? SetTag(Activity? activity, Func<AWSSemanticConventionsBase, string> attributeNameFunc, object? value)
    {
        var semanticConventionVersionImpl = this.GetSemanticConventionVersion();

        var attributeName = attributeNameFunc(semanticConventionVersionImpl);

        // if attributeName is empty, exit
        if (string.IsNullOrEmpty(attributeName))
        {
            return activity;
        }

        activity?.SetTag(attributeName, value);

        return activity;
    }

    private object? GetTag(Activity? activity, Func<AWSSemanticConventionsBase, string> attributeNameFunc)
    {
        var semanticConventionVersionImpl = this.GetSemanticConventionVersion();

        var attributeName = attributeNameFunc(semanticConventionVersionImpl);

        // if attributeName is empty, exit
        return string.IsNullOrEmpty(attributeName) ? null : activity?.GetTagItem(attributeName);
    }

    private ParameterMappingBuilderImpl AddDic(ParameterMappingBuilderImpl dict, Func<AWSSemanticConventionsBase, string> attributeNameFunc, string value)
    {
        var semanticConventionVersionImpl = this.GetSemanticConventionVersion();

        var attributeName = attributeNameFunc(semanticConventionVersionImpl);

        if (!string.IsNullOrEmpty(attributeName))
        {
            dict.Add(value, attributeName);
        }

        return dict;
    }

    private AWSSemanticConventionsBase GetSemanticConventionVersion()
    {
#pragma warning disable IDE0066 // Convert switch statement to expression
        switch (this.semanticConventionVersion)
        {
            case SemanticConventionVersion.Latest:
            case SemanticConventionVersion.V1_29_0:
                return new AWSSemanticConventions_V1_29_0();

            case SemanticConventionVersion.V1_28_0:
                return new AWSSemanticConventions_V1_28_0();

            default:
                throw new InvalidEnumArgumentException(
                    argumentName: nameof(SemanticConventionVersion),
                    (int)this.semanticConventionVersion,
                    typeof(SemanticConventionVersion));
        }
#pragma warning restore IDE0066 // Convert switch statement to expression
    }
}
