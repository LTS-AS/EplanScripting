using System.Windows.Forms;
using Eplan.EplApi.ApplicationFramework;
using Eplan.EplApi.Base;
using Eplan.EplApi.Scripting;

public class Class
{
    [Start]
    public void Function() //Unable to make this action export data from eplan professional. Is this a EC1 feature??
    {
        string strProjectpath =
            PathMap.SubstitutePath("$(PROJECTPATH)") + @"\";

        CommandLineInterpreter oCLI = new CommandLineInterpreter();
        ActionCallingContext acc = new ActionCallingContext();

        acc.AddParameter("TYPE", "EXPORT");
        acc.AddParameter("EXPORTFILE", strProjectpath + "Devicelist.txt");
//        acc.AddParameter("FORMAT", "XDLTxtImporterExporter");

        oCLI.Execute("devicelist", acc);

        MessageBox.Show("Action executed");

        return;
    }
}