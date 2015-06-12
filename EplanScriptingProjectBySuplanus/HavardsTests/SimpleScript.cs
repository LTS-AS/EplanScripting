public class SimpleScript
{
	 [DeclareAction("ShowMessage")]
     public void ShowMessageFunction()
     {
           MessageBox.Show("MyFunctionAsAction was called!", "RegisterScriptMenu");
           return;
     }
	 
	 [DeclareAction("ShowMessages")]
     public void ShowMessagesFunction()
     {
           ShowMessageFunction();
           ShowMessageFunction();
		   return;
     }

	[DeclareMenu]
	public void SecondMenuFunction()
     {
           Eplan.EplApi.Gui.Menu oMenu = new Eplan.EplApi.Gui.Menu();
		   oMenu.AddMenuItem("Custom","ShowMessage");
		   oMenu.AddMenuItem("Custom double","ShowMessages");
     }
}