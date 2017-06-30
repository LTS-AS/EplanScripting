using System.Windows.Forms;
using Eplan.EplApi.Scripting;

public class Class
{

    [DeclareAction("MenuAction")]
    public void ActionFunction()
    {
        MessageBox.Show("Action!");

        return;
    }

    [DeclareMenu]
    public void MenuFunction()
    {
        Eplan.EplApi.Gui.Menu oMenu = new Eplan.EplApi.Gui.Menu();

        oMenu.AddMenuItem(
            "Meny item in utilities", // Name: Menüpunkt
            "MenuAction" // Name: Action
            );

        return;
    }
}