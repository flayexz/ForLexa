# Extending Unity Test Framework
It is possible to extend the Unity Test Framework (UTF) in many ways, for custom workflows for your projects and for other packages to build on top of UTF.

These extensions are a supplement to the ones already offered by [NUnit](https://github.com/nunit/docs/wiki/Framework-Extensibility).

Some workflows for extending UTF include:
* [How to split the build and run process for standalone Play Mode tests](./reference-attribute-testplayerbuildmodifier.md#split-build-and-run-for-player-mode-tests)
* [How to run tests programmatically](./extension-run-tests.md)
* [How to get test results](./extension-get-test-results.md)
* [How to retrieve the list of tests](./extension-retrieve-test-list.md)                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            # How to get test results
You can receive callbacks when the active test run, or individual tests, starts and finishes. You can register callbacks by invoking `RegisterCallbacks` on the [TestRunnerApi](./reference-test-runner-api.md) with an instance of a class that implements [ICallbacks](./reference-icallbacks.md). There are four `ICallbacks` methods for the start and finish of both the whole run and each level of the test tree. 

## Example
An example of how listeners can be set up: 

> **Note**: Listeners receive callbacks from all test runs, regardless of the registered `TestRunnerApi` for that instance.

``` C#
public void SetupListeners()
{
   var api = ScriptableObject.CreateInstance<TestRunnerApi>();
   api.RegisterCallbacks(new MyCallbacks());
}

private class MyCallbacks : ICallbacks
{
    public void RunStarted(ITestAdaptor testsToRun)
    {
       
    }

    public void RunFinished(ITestResultAdaptor result)
    {
       
    }

    public void TestStarted(ITestAdaptor test)
    {
       
    }

    public void TestFinished(ITestResultAdaptor result)
    {
        if (!result.HasChildren && result.ResultState != "Passed")
        {
            Debug.Log(string.Format("Test {0} {1}", result.Test.Name, result.ResultState));
        }
    }
}
```

> **Note**: The registered callbacks are not persisted on domain reloads. So it is necessary to re-register the callback after a domain reloads, usually with [InitializeOnLoad](https://docs.unity3d.com/Manual/RunningEditorCodeOnLaunch.html).

It is possible to provide a `priority` as an integer as the second argument when registering a callback. This influences the invocation order of different callbacks. The def