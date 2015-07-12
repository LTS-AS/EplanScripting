namespace MyOtherNameSpace{
public class ReportLib
{
	public string projectProperties()
	{
		ActionCallingContext pdfContext = new ActionCallingContext();
		return pdfContext.ToString();
	}
	
	public string getProjectPath(int nStrippLevels)
	{
		//Execute a "selectionset" action to obtain the currently selected project's full path
		Eplan.EplApi.ApplicationFramework.ActionManager oMngr = new Eplan.EplApi.ApplicationFramework.ActionManager();
		Eplan.EplApi.ApplicationFramework.Action oSelSetAction = oMngr.FindAction("selectionset");
		string myPath = "";
		if (oSelSetAction != null)
		{
			Eplan.EplApi.ApplicationFramework.ActionCallingContext ctx = new Eplan.EplApi.ApplicationFramework.ActionCallingContext();
			ctx.AddParameter("TYPE", "PROJECT");
			bool bRet = oSelSetAction.Execute(ctx);
			if (bRet)
			{
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
		Eplan.EplApi.Base.Progress oProgress = new Eplan.EplApi.Base.Progress("SimpleProgress");
		oProgress.ShowImmediately();
		ActionCallingContext pdfContext = new ActionCallingContext();
		pdfContext.AddParameter("type", "PDFPROJECTSCHEME");
		pdfContext.AddParameter("exportfile", sZielDatei);
		pdfContext.AddParameter("exportmodel", "0");
		pdfContext.AddParameter("blackwhite", "1");
		pdfContext.AddParameter("useprintmargins", "1");
		pdfContext.AddParameter("readonlyexport", "2");
		pdfContext.AddParameter("usesimplelink", "1");
		pdfContext.AddParameter("usezoomlevel", "1");
		pdfContext.AddParameter("fastwebview", "1");
		pdfContext.AddParameter("zoomlevel", "1");

		CommandLineInterpreter cmdLineItp = new CommandLineInterpreter();
		cmdLineItp.Execute("export", pdfContext);

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

	public void backup(string revisionType = "")
	{
	string strProjectname = PathMap.SubstitutePath("$(PROJECTNAME)");

	string strFullProjectname = PathMap.SubstitutePath("$(P)");
	if (strProjectname == "")
	{
		MessageBox.Show("No unique project selected", "Backup failed");
	}
	else
	{
		string strDestination = System.IO.Path.GetDirectoryName(strFullProjectname);//pealing off last folder
		string myTime = System.DateTime.Now.ToString("yyyy_MM_dd");
		string hour = System.DateTime.Now.Hour.ToString();
		string minute = System.DateTime.Now.Minute.ToString();
		Progress progress = new Progress("SimpleProgress");
		progress.BeginPart(100, "");
		progress.SetAllowCancel(true);
		if (!progress.Canceled())
		{
			progress.BeginPart(33, "Backup");
			ActionCallingContext backupContext = new ActionCallingContext();
			backupContext.AddParameter("BACKUPMEDIA", "DISK");
			backupContext.AddParameter("BACKUPMETHOD", "BACKUP");
			backupContext.AddParameter("COMPRESSPRJ", "0");
			backupContext.AddParameter("INCLEXTDOCS", "1");
			backupContext.AddParameter("BACKUPAMOUNT", "BACKUPAMOUNT_ALL");
			backupContext.AddParameter("INCLIMAGES", "1");
			backupContext.AddParameter("LogMsgActionDone", "true");
			backupContext.AddParameter("DESTINATIONPATH", strDestination);
			backupContext.AddParameter("PROJECTNAME", strFullProjectname);
			backupContext.AddParameter("TYPE", "PROJECT");
			if (revisionType=="")
			{
				backupContext.AddParameter("ARCHIVENAME", strProjectname + "_" + myTime + "_" + hour + "_" + minute);//if no revision type defined, just add timestamp
			}
			else
			{
				backupContext.AddParameter("ARCHIVENAME",  strProjectname +  "_" + revisionType);//TODO: add revision number based on previus files
			}
			new CommandLineInterpreter().Execute("backup", backupContext);
			progress.EndPart();		
		}
		progress.EndPart(true);
	}
	return;
	}
}

public class UtilitiesToolbar{
	ReportLib myLib = new ReportLib();
	
	[DeclareAction("ShowMessage")]
    public void showMessageFunction()
	{
		MessageBox.Show(PathMap.SubstitutePath("$(P)"));
		//MessageBox.Show(DateTime.Now.ToString("yyyy-MM-dd"), "Useless date function");
    }
	 
	 [DeclareAction("updateAndExportPDF")]
     public void updateAndExportPDFn()
	 {
		bool result = myLib.generateReports();
	    myLib.PDFexport(myLib.getProjectPath(2) + "\\P2014-096 padde2\\" + DateTime.Now.ToString("yyyy-MM-dd") + "-export.pdf");
		return;
     }

	 
	 [DeclareAction("OpenProjectFolder")]
	 public void openProjectFolderFunction(){
		string projectFolder = "";
		projectFolder = myLib.getProjectPath(2);
		if(projectFolder == string.Empty){
			MessageBox.Show("Not able to open path. Select ONE project.", "ERROR");
		}
		else{
			System.Diagnostics.Process.Start("explorer.exe", projectFolder);
		}
    }
	
	[DeclareAction("backupProject")]
	public void backupProject(){
		myLib.backup();
	}

	[DeclareMenu]
	public void SecondMenuFunction(){
           Eplan.EplApi.Gui.Menu oMenu = new Eplan.EplApi.Gui.Menu();
		   oMenu.AddMenuItem("Custom","ShowMessage");
		   oMenu.AddMenuItem("Back up project","backupProject");
		   oMenu.AddMenuItem("Update and export PDFs","updateAndExportPDF");
		   oMenu.AddMenuItem("Open project folder","OpenProjectFolder");
     }
}
}
//User supplementary field 1 = P2014-096