using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using VMS.TPS.Common.Model.API;
using VMS.TPS.Common.Model.Types;

// TODO: Replace the following version attributes by creating AssemblyInfo.cs. You can do this in the properties of the Visual Studio project.
[assembly: AssemblyVersion("1.0.0.1")]
[assembly: AssemblyFileVersion("1.0.0.1")]
[assembly: AssemblyInformationalVersion("1.0")]

// TODO: Uncomment the following line if the script requires write access.
// [assembly: ESAPIScript(IsWriteable = true)]

namespace VMS.TPS
{
  public class Script
  {
    public Script()
    {
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public void Execute(ScriptContext context, System.Windows.Window window/*, ScriptEnvironment environment*/)
    {

        //If no dose in patient show error.
        if (context.PlanSetup.Dose == null)
        {
            MessageBox.Show("Dose is not calculated for this plan, please check the plan and try again.", "No dose found");
            return;
        }
        //Create object of MainWindow and pass context parameter
        var mainView = new BED_Calc.MainView(context);
        window.Title = "BED/EQD2 Calculator";
        window.Content = mainView; //contents to be displayed on window

        }
    }
}
