public class SimpleScript{
	
    private string getProjectFolder(){
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
				if(myPath != ""){
					myPath = System.IO.Path.GetDirectoryName(myPath);//pealing off filename
					myPath = System.IO.Path.GetDirectoryName(myPath);//pealing off last folder
				}
			}
		}
		return myPath;
    }
	
	 [DeclareAction("ShowMessage")]
     public void showMessageFunction(){
         MessageBox.Show(getProjectFolder());
         return;
     }
	 
	 [DeclareAction("ShowMessages")]
     public void showMessagesFunction()
     {
		 showMessageFunction();
		 showMessageFunction();
		 return;
     }
	 
	 [DeclareAction("OpenProjectFolder")]
	 public void openProjectFolderFunction(){
		string projectFolder = "";
		projectFolder = getProjectFolder();
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
		   oMenu.AddMenuItem("Custom double","ShowMessages");
		   oMenu.AddMenuItem("Open project folder","OpenProjectFolder");
     }
}