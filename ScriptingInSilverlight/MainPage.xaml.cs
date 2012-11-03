using System;
using System.Windows;
using Microsoft.Scripting.Hosting;

namespace ScriptingInSilverlight
{
    public partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();

            // call web service to get Python script, for this demo we are hard coding the script here

            Python.Text = "def calculateCommission(start, end):" + Environment.NewLine +
                          "  if (end < start.AddMonths(3)):" + Environment.NewLine +
                          "    return 1" + Environment.NewLine +
                          "  elif (end >= start.AddMonths(3) and end < start.AddMonths(6)):" + Environment.NewLine +
                          "    return 2" + Environment.NewLine +
                          "  elif (end >= start.AddMonths(6) and end <= start.AddMonths(12)):" + Environment.NewLine +
                          "    return 3";

        }

        private void PythonButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ScriptEngine engine = IronPython.Hosting.Python.CreateEngine();
                ScriptScope scope = engine.CreateScope();
                engine.Execute(Python.Text, scope);

                Func<DateTime, DateTime, int> function;
                if (!scope.TryGetVariable("calculateCommission", out function))
                {
                    PythonResult.Text = "No function calculateCommission defined";
                    return;
                }

                var result = function(new DateTime(2012, 11, 02), new DateTime(2012, 12, 31));

                PythonResult.Text = "RESULT = " + result + " % commission";
            }
            catch (Exception ex)
            {
                PythonResult.Text = string.Format("Exception running script: {0}", ex.Message);
            }   
        }
    }
}
