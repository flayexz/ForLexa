ontSaveInEditor hideFlags would result in an error on cleanup (case 1136883).
- Now ITestPlayerBuildModifier.ModifyOptions is called as expected when running tests on a device (case 1213845)

## [1.1.11] - 2020-01-16
- Fixed test runner dlls got included into player build (case 1211624)
- Passing a non-full-path of XML file for -testResults in Unity Batchmode issue resolved, now passing "result.xml" creates the result file in the project file directory (case 959078)
- Respect Script Debugging build setting when running tests

## [1.1.10] - 2019-12-19
- Introduced PostSuccessfulLaunchAction callback
- Fixed an issue where canceling a UnityTest while it was running would incorrectly mark it as passed instead of canceled.
- Added command line argument for running tests synchronously.
- The test search bar now handles null values correctly.
- The test output pane now retains its size on domain reloads.

## [1.1.9] - 2019-12-12
- Rolled back refactoring to the test run system, as it caused issues in some corner cases.

## [1.1.8] - 2019-11-15
- Ensured that a resumed test run is continued instantly. 

## [1.1.7] - 2019-11-14
- Fixed an issue with test runs after domain reload.

## [1.1.6] - 2019-11-12
- Building a player for test will no longer look in unrelated assemblies for relevant attributes.

## [1.1.5] - 2019-10-23
- Fixed a regression to synchronous runs introduced in 1.1.4.

## [1.1.4] - 2019-10-15
- Running tests in batch mode now correctly returns error code 3 (RunError) when a timeout or a build error occurs.
- Fixed an issue where a test run in a player would time out, if the player takes longer than 10 minutes to run.
- Added command line argum