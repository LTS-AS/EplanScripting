public class SimpleScript
{
	
	 [DeclareAction("MyScriptActionWithMenu")]
     public void MyFunctionAsAction()
     {

           MessageBox.Show("MyFunctionAsAction was called!", "RegisterScriptMenu");
           return;

     }

	
	[DeclareMenu]
	public void MenuFunction()
     {

           Eplan.EplApi.Gui.Menu oMenu = new Eplan.EplApi.Gui.Menu();
           oMenu.AddMenuItem("Custom","MyScriptActionWithMenu");

     }

}