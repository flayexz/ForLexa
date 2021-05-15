using System;
using System.Linq;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal.Filters;
using UnityEngine;

namespace UnityEditor.TestTools.TestRunner.Api
{
    /// <summary>
    /// A set of execution settings defining how to run tests, using the <see cref="TestRunnerApi"/>.
    /// </summary>
    [Serializable]
    public class ExecutionSettings
    {
        /// <summary>
        /// Creates an instance with a given set of filters, if any.
        /// </summary>
        /// <param name="filtersToExecute">Set of filters</param>
        public ExecutionSettings(params Filter[] filtersToExecute)
        {
            filters = filtersToExecute;
        }
        
        [SerializeField]
        internal BuildTarget? targetPlatform;

        /// <summary>
        /// An instance of <see cref="ITestRunSettings"/> to set up before running tests on a Player.
        /// </summary>
        // Note: Is not available after serialization
        public ITestRunSettings overloadTestRunSettings;
        
        [SerializeField]
        internal Filter filter;
        ///<summary>
        ///A collection of <see cref="Filter"/> to execute tests on.
        ///</summary>
        [SerializeField]
        public Filter[] filters;
        /// <summary>
        ///  Note that this is only supported for EditMode tests, and that tests which take multiple frames (i.e. [UnityTest] tests, or tests with [UnitySetUp] or [UnityTearDown] scaffolding) will be filtered out.
        /// </summary>
        /// <returns>If true, the call to Execute() will run tests synchronously, guaranteeing that all tests have finished running by the time the call returns.</returns>
        [SerializeField]
        public bool runSynchronously;
        /// <summary>
        /// The time, in seconds, the editor should wait for heartbeats after starting a test run on a player. This defaults to 10 minutes.
        /// </summary>
        [SerializeField]
        public int playerHeartbeatTimeout = 60*10;

        internal bool EditModeIncluded()
        {
            return filters.Any(f => IncludesTestMode(f.testMode, TestMode.EditMode));
        }
        
        internal bool PlayModeInEditorIncluded()
        {
            return filters.Any(f => IncludesTestMode(f.testMode, TestMode.PlayMode) && targetPlatform == null);
        }

        internal bool PlayerIncluded()
        {
            return filters.Any(f => IncludesTestMode(f.testMode, TestMode.PlayMode) && targetPlatform != null);
        }

        private static bool IncludesTestMode(TestMode testMode, TestMode modeToCheckFor)
        {
            return (testMode & modeToCheckFor) == modeToCheckFor;
        }
        
        internal ITestFilter BuildNUnitFilter()
        {
            return new OrFilter(filters.Select(f => f.ToRuntimeTestRunnerFilter(runSynchronously).BuildNUnitFilter()).ToArray());
        }
    }
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                  using System;
using UnityEngine;
using UnityEngine.TestTools.TestRunner.GUI;

namespace UnityEditor.TestTools.TestRunner.Api
{
    /// <summary>
    /// The filter class provides the <see cref="TestRunnerApi"/> with a specification of what tests to run when [running tests programmatically](https://docs.unity3d.com/Packages/com.unity.test-framework@1.1/manual/extension-run-tests.html).
    /// </summary>
    [Serializable]
    public class Filter
    {
        /// <summary>
        /// An enum flag that specifies if Edit Mode or Play Mode tests should run.
        ///</summary>
        [SerializeField]
        public TestMode testMode;
        /// <summary>
        /// The full name of the tests to match the filter. This is usually in the format FixtureName.TestName. If the test has test arguments, then include them in parenthesis. E.g. MyTestClass2.MyTestWithMultipleValues(1).
        /// </summary>
        [SerializeField]
        public string[] testNames;
        /// <summary>
        /// The same as testNames, except that it allows for Regex. This is useful for running specific fixtures or namespaces. E.g. "^MyNamespace\\." Runs any tests where the top namespace is MyNamespace.
        /// </summary>
        [SerializeField]
        public string[] groupNames;
        /// <summary>
        /// The name of a [Category](https://nunit.org/docs/2.2.7/category.html) to include in the run. Any test or fixtures runs that have a Category matching the string.
        /// </summary>
        [SerializeField]
        public string[] categoryNames;
        /// <summary>
        /// The name of assemblies included in the run. That is the assembly name, without the .dll file extension. E.g., MyTestAssembly
        /// </summary>
        [SerializeField]
        public string[] assemblyNames;
        /// <summary>
        /// The <see cref="BuildTarget"/> platform to run the test on. If set to null, then the Editor is the target for the tests.
        /// </summary>
        [SerializeField]
        public BuildTarget? targetPlatform;

        internal RuntimeTestRunnerFilter ToRuntimeTestRunnerFilter(bool synchronousOnly)
        {
            return new RuntimeTestRunnerFilter()
            {
                testNames = testNames,
                categoryNames = categoryNames,
                groupNames = groupNames,
                assemblyNames = assemblyNames,
                synchronousOnly = synchronousOnly
            };
        }
    }
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                          namespace UnityEditor.TestTools.TestRunner.Api
{
    /// <summary>
    /// Callbacks in the <see cref="TestRunnerApi"/> for the test stages when running tests. 
    /// </summary>
    public interface ICallbacks
    {
        /// <summary>
        /// A callback invoked when a test run is started.
        /// </summary>
        /// <param name="testsToRun">The full loaded test tree.</param>
        void RunStarted(ITestAdaptor testsToRun);
        /// <summary>
        /// A callback invoked when a test run is finished.
        /// </summary>
        /// <param name="result">The result of the test run.</param>
        void RunFinished(ITestResultAdaptor result);
        /// <summary>
        /// A callback invoked when each individual node of the test tree has started executing.
        /// </summary>
        /// <param name="test">The test node currently executed.</param>
        void TestStarted(ITestAdaptor test);
        /// <summary>
        /// A callback invoked when each individual node of the test tree has finished executing.
        /// </summary>
        /// <param name="result">The result of the test tree node after it had been executed.</param>
        void TestFinished(ITestResultAdaptor result);
    }
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                             using System.Collections.Generic;
using NUnit.Framework.Interfaces;

namespace UnityEditor.TestTools.TestRunner.Api
{
    /// <summary>
    /// ```ITestAdaptor``` is a representation of a node in the test tree implemented as a wrapper around the [NUnit](http://www.nunit.org/) [ITest](https://github.com/nunit/nunit/blob/master/src/NUnitFramework/framework/Interfaces/ITest.cs)  interface.
    /// </summary>
    public interface ITestAdaptor
    {
        /// <summary> 
        /// The ID of the test tree node. The ID can change if you add new tests to the suite. Use UniqueName, if you want to have a more permanent point of reference. 
        /// </summary>
        string Id { get; }
        /// <summary> 
        /// The name of the test. E.g.,```MyTest```. 
        /// </summary>
        string Name { get; }
        /// <summary> 
        /// The full name of the test. E.g., ```MyNamespace.MyTestClass.MyTest```.
        /// </summary>
        string FullName { get; }
        /// <summary> 
        /// The total number of test cases in the node and all sub-nodes.
        /// </summary>
        int TestCaseCount { get; }
        /// <summary> 
        /// Whether the node has any children.
        /// </summary>
        bool HasChildren { get; }
        /// <summary>
        /// True if the node is a test suite/fixture, false otherwise.
        /// </summary>
        bool IsSuite { get; }
        /// <summary>
        /// The child nodes.
        /// </summary>
        IEnumerable<ITestAdaptor> Children { get; }
        /// <summary> 
        /// The parent node, if any.
        /// </summary>
        ITestAdaptor Parent { get; }
        /// <summary>
        /// The test case timeout in milliseconds. Note that this value is only available on TestFinished.
        /// </summary>
        int TestCaseTimeout { get; }
        /// <summary>
        /// The type of test class as an ```NUnit``` <see cref="ITypeInfo"/>. If the node is not a test class, then the value is null.
        /// </summary>
        ITypeInfo TypeInfo { get; }
        /// <summary>
        /// The Nunit <see cref="IMethodInfo"/> of the test method. If the node is not a test method, then the value is null.
        /// </summary>
        IMethodInfo Method { get; }
        /// <summary>
        /// An array of the categories applied to the test or fixture.
        /// </summary>
        string[] Categories { get; }
        /// <summary>
        /// Returns true if the node represents a test assembly, false otherwise.
        /// </summary>
        bool IsTestAssembly { get; }
        /// <summary>
        /// The run state of the test node. Either ```NotRunnable```, ```Runnable```, ```Explicit```, ```Skipped```, or ```Ignored```.
        /// </summary>
        RunState RunState { get; }
        /// <summary>
        /// The description of the test.
        /// </summary>
        string Description { get; }
        /// <summary>
        /// The skip reason. E.g., if ignoring the test.
        /// </summary>
        string SkipReason { get; }
        /// <summary>
        /// The ID of the parent node.
        /// </summary>
        string ParentId { get; }
        /// <summary>
        /// The full name of the parent node.
        /// </summary>
        string ParentFullName { get; }
        /// <summary>
        /// A unique generated name for the test node. E.g., ```Tests.dll/MyNamespace/MyTestClass/[Tests][MyNamespace.MyTestClass.MyTest]```.
        /// </summary>
        string UniqueName { get; }
        /// <summary>
        /// A unique name of the parent node. E.g., ```Tests.dll/MyNamespace/[Tests][MyNamespace.MyTestClass][suite]```.
        /// </summary>
        string ParentUniqueName { get; }
        /// <summary>
        /// The child index of the node in its parent.
        /// </summary>
        int ChildIndex { get; }
        /// <summary>
        /// The mode of the test. Either **Edit Mode** or **Play Mode**.
        /// </summary>
        TestMode TestMode { get; }
    }
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                using System.Collections.Generic;
using NUnit.Framework.Interfaces;
using UnityEngine.TestRunner.TestLaunchers;

namespace UnityEditor.TestTools.TestRunner.Api
{
    internal interface ITestAdaptorFactory
    {
        ITestAdaptor Create(ITest test);
        ITestAdaptor Create(RemoteTestData testData);
        ITestResultAdaptor Create(ITestResult testResult);
        ITestResultAdaptor Create(RemoteTestResultData testResult, RemoteTestResultDataWithTestData allData);
        ITestAdaptor BuildTree(RemoteTestResultDataWithTestData data);
        IEnumerator<ITestAdaptor> BuildTreeAsync(RemoteTestResultDataWithTestData data);
        void ClearResultsCache();
        void ClearTestsCache();
    }
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                         using System;
using System.Collections.Generic;
using NUnit.Framework.Interfaces;

namespace UnityEditor.TestTools.TestRunner.Api
{
    /// <summary>
    /// The `ITestResultAdaptor` is the representation of the test results for a node in the test tree implemented as a wrapper around the [NUnit](http://www.nunit.org/) [ITest](https://github.com/nunit/nunit/blob/master/src/NUnitFramework/framework/Interfaces/ITestResults.cs) interface.
    /// </summary>
    public interface ITestResultAdaptor
    {
        /// <summary>
        /// The test details of the test result tree node as a <see cref="TestAdaptor"/>
        /// </summary>
        ITestAdaptor Test { get; }
        ///<summary>
        ///The name of the test node. 
        ///</summary>
        string Name { get; }
        /// <summary>
        /// Gets the full name of the test result
        /// </summary>
        ///<returns> 
        ///The name of the test result.
        ///</returns>
        string FullName { get; }
        ///<summary>
        ///Gets the state of the result as a string.
        ///</summary>
        ///<returns>
        ///It returns one of these values: `Inconclusive`, `Skipped`, `Skipped:Ignored`, `Skipped:Explicit`, `Passed`, `Failed`, `Failed:Error`, `Failed:Cancelled`, `Failed:Invalid`
        ///</returns>
        string ResultState { get; }
        ///<summary>
        ///Gets the status of the test as an enum.
        ///</summary>
        ///<returns>
        ///It returns one of these values:`Inconclusive`, `Skipped`, `Passed`, or `Failed` 
        ///</returns>
        TestStatus TestStatus { get; }
        /// <summary>
        /// Gets the elapsed time for running the test in seconds
        /// </summary>
        /// <returns>
        /// Time in seconds.
        /// </returns>
        double Duration { get; }
        /// <summary>
        /// Gets or sets the time the test started running.
        /// </summary>
        ///<returns>
        ///A DataTime object.
        ///</returns>
        DateTime StartTime { get; }
        ///<summary>
        ///Gets or sets the time the test finished running.
        ///</summary>
        ///<returns>
        ///A DataTime object.
        ///</returns>
        DateTime EndTime { get; }

        /// <summary>
        /// The message associated with a test failure or with not running the test
        /// </summary>
        string Message { get; }

        /// <summary>
        /// Any stacktrace associated with an error or failure. Not available in the Compact Framework 1.0.
        /// </summary>
        string StackTrace { get; }

        /// <summary>
        /// The number of asserts executed when running the test and all its children.
        /// </summary>
        int AssertCount { get; }

        /// <summary>
        /// The number of test cases that failed when running the test and all its children.
        /// </summary>
        int FailCount { get; }

        /// <summary>
        /// The number of test cases that passed when running the test and all its children.
        /// </summary>
        int PassCount { get; }

        /// <summary>
        /// The number of test cases that were skipped when running the test and all its children.
        /// </summary>
        in