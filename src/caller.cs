namespace EplanScripts
{	
	public class ActionClass
	{
		include program;
		ScriptTest st = new ScriptTest();
		[DeclareAction("action")]
		public void openProjectFolderFunction()
		{
			st.sugg();
		}

		[DeclareMenu]
		public void SecondMenuFunction()
		{
           Eplan.EplApi.Gui.Menu oMenu = new Eplan.EplApi.Gui.Menu();
		   oMenu.AddMenuItem("Action","action");
		}
	}
}