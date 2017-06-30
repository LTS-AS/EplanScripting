using Eplan.EplApi.ApplicationFramework;
using Eplan.EplApi.Scripting;

public class Class
{
    [Start]
    public void Function()
    {
        CommandLineInterpreter oCLI = new CommandLineInterpreter();
        ActionCallingContext acc = new ActionCallingContext();
        acc.AddParameter("DEVICENAME", "=+A1-S1");
        acc.AddParameter("FORMAT", "XDLTxtImporterExporter");

        oCLI.Execute("edit", acc);

        return;
    }
}