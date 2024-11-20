using UnityEditor;
using UnityEditor.TestTools.TestRunner.Api;
using System.IO;
using UnityEngine;

public class TestReportGenerator
{
    public static void RunTestsAndGenerateReport()
    {
        string testResultsPath = "Builds/TestResults";
        CreateDirectory(testResultsPath);

        string testReportFile = $"{testResultsPath}/TestReport.xml";

        // Configura os testes
        var testRunnerApi = ScriptableObject.CreateInstance<TestRunnerApi>();
        var filter = new Filter
        {
            testMode = TestMode.EditMode
        };

        var executionSettings = new ExecutionSettings(filter)
        {
            runSynchronously = true // Executa os testes de forma síncrona
        };

        // Adiciona os callbacks
        testRunnerApi.RegisterCallbacks(new TestCallbacks(testReportFile));

        // Executa os testes
        testRunnerApi.Execute(executionSettings);
    }

    private static void CreateDirectory(string path)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }

    private class TestCallbacks : ICallbacks
    {
        private readonly string _reportFilePath;

        public TestCallbacks(string reportFilePath)
        {
            _reportFilePath = reportFilePath;
        }

        public void RunStarted(ITestAdaptor testsToRun)
        {
            Debug.Log("Test execution started...");
        }

        public void RunFinished(ITestResultAdaptor result)
        {
            Debug.Log("Test execution finished. Writing report...");

            // Gera o relatório XML
            string xmlReport = result.ToXml().OuterXml;
            File.WriteAllText(_reportFilePath, xmlReport);

            Debug.Log($"Test report saved to {_reportFilePath}");
        }

        public void TestStarted(ITestAdaptor test)
        {
            Debug.Log($"Test started: {test.Name}");
        }

        public void TestFinished(ITestResultAdaptor result)
        {
            Debug.Log($"Test finished: {result.Name}, Result: {result.ResultState}");
        }
    }
}

