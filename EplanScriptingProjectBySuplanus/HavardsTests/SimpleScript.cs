public class SimpleScript{
	
    private string getProjectPath(int nStrippLevels){
		//Execute a "selectionset" action to obtain the currently selected project's full path
		Eplan.EplApi.ApplicationFramework.ActionManager oMngr = new Eplan.EplApi.ApplicationFramework.ActionManager();
		Eplan.EplApi.ApplicationFramework.Action oSelSetAction = oMngr.FindAction("selectionset");
		string myPath = "";
		if (oSelSetAction != null){
			Eplan.EplApi.ApplicationFramework.ActionCallingContext ctx = new Eplan.EplApi.ApplicationFramework.ActionCallingContext();
			ctx.AddParameter("TYPE", "PROJECT");
			bool bRet = oSelSetAction.Execute(ctx);
			if (bRet){
				ctx.GetParameter("PROJECT", ref myPath);
				for(int i=0;((myPath != "")&&(i<nStrippLevels));i++)
				{
					myPath = System.IO.Path.GetDirectoryName(myPath);//pealing off last folder
				}
			}
		}
		return myPath;
    }
	
	
	public void PDFexport(string sZielDatei)
	{
		//Progressbar ein
		Eplan.EplApi.Base.Progress oProgress = new Eplan.EplApi.Base.Progress("SimpleProgress");
		oProgress.ShowImmediately();

		ActionCallingContext pdfContext = new ActionCallingContext();
		pdfContext.AddParameter("type", "PDFPROJECTSCHEME"); //PDFPROJECTSCHEME = Projekt im PDF-Format exportieren
		//pdfContext.AddParameter("exportscheme", "NAME_SCHEMA"); //verwendetes Schema
		pdfContext.AddParameter("exportfile", sZielDatei); //Name export.Projekt, Vorgabewert: Projektname
		pdfContext.AddParameter("exportmodel", "0"); //0 = keine Modelle ausgeben
		pdfContext.AddParameter("blackwhite", "1"); //1 = PDF wird schwarz-weiss
		pdfContext.AddParameter("useprintmargins", "1"); //1 = Druckränder verwenden
		pdfContext.AddParameter("readonlyexport", "2"); //1 = PDF wird schreibgeschützt
		pdfContext.AddParameter("usesimplelink", "1"); //1 = einfache Sprungfunktion
		pdfContext.AddParameter("usezoomlevel", "1"); //Springen in Navigationsseiten
		pdfContext.AddParameter("fastwebview", "1"); //1 = schnelle Web-Anzeige
		pdfContext.AddParameter("zoomlevel", "1"); //wenn USEZOOMLEVEL auf 1 dann hier Zoomstufe in mm

		CommandLineInterpreter cmdLineItp = new CommandLineInterpreter();
		cmdLineItp.Execute("export", pdfContext);

		//'Progressbar aus
		oProgress.EndPart(true);
		return;
	}
    public bool generateReports()
	{
		bool bResult = false;
		string strProjectName = getProjectPath(0);
		ActionCallingContext context1 = new ActionCallingContext ();
		context1.AddParameter("ProjectName",strProjectName);
		if (strProjectName != "")
		{
			context1.AddParameter("type","PROJECT");
			bResult = new CommandLineInterpreter().Execute("reports",context1);
		}
		else
		{
			MessageBox.Show("Not able to generate report projects. Select ONE project.", "ERROR");
		}
		return bResult;
	}
	
	 [DeclareAction("ShowMessage")]
     public void showMessageFunction()
	{
		MessageBox.Show(DateTime.Now.ToString("yyyy-MM-dd"), "Useless date function");
    }
	 
	 [DeclareAction("updateAndExportPDF")]
     public void updateAndExportPDFn()
	 {
		bool result = generateReports();
	    PDFexport(getProjectPath(2) + "\\P2014-096 padde2\\" + DateTime.Now.ToString("yyyy-MM-dd") + "-export.pdf");
		return;
     }

	 
	 [DeclareAction("OpenProjectFolder")]
	 public void openProjectFolderFunction(){
		string projectFolder = "";
		projectFolder = getProjectPath(2);
		if(projectFolder == string.Empty){
			MessageBox.Show("Not able to open path. Select ONE project.", "ERROR");
		}
		else{
			System.Diagnostics.Process.Start("explorer.exe", projectFolder);
		}
    }

	[DeclareMenu]
	public void SecondMenuFunction(){
           Eplan.EplApi.Gui.Menu oMenu = new Eplan.EplApi.Gui.Menu();
		   oMenu.AddMenuItem("Custom","ShowMessage");
		   oMenu.AddMenuItem("Update and export PDFs","updateAndExportPDF");
		   oMenu.AddMenuItem("Open project folder","OpenProjectFolder");
     }
}