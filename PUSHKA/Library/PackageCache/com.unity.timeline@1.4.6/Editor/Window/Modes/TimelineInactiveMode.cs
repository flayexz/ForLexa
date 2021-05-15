 If a player build fails, the test specific build settings should be cleaned up and the original values restored as intended (DS-1001).
- Added better error message when using TestRunCallbackAttribute and the implementation is stripped away (DS-454).
- Fixed an issue where the test results xml would have a zero end-time for tests executed before a domain reload (DSTR-63).
- Fixed OpenSource in case of a Test in a nested class (DSTR-6)
- UnityTests with a domain reload now works correctly in combination with Retry and Repeat attributes (DS-428).
- Fixed OpenSource in case of Tests located inside a package (DS-432)

## [1.1.19] - 2020-11-17
- Command line runs with an inconclusive test result now exit with exit code 2 (case DS-951).
- Fixed timeout during UnitySetUp which caoused test to pass instead of failing due to wrong time format.
- Timeout exeption thrown when timeout time is exeded in the UnitySetup when using `WaitForSeconds(n)`.
- Updating `com.unity.ext.nunit` version
- Method marked with UnityTest that are not returning IEnumerator is now giving a proper error (DS-1059).

## [1.1.18] - 2020-10-07
- Fixed issue of timeout during UnitySetUp which wasn't detected and allowed the test to pass instead of failing (case DSTR-21)

## [1.1.17] - 2020-10-05
- Fixed an issue where the Wai