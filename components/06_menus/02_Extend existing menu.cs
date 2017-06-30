using System.Windows.Forms;
using Eplan.EplApi.Scripting;

public class Class
{

    [DeclareAction("MenuAction")]
    public void ActionFunction()
    {
        MessageBox.Show("Action has been executed!");

        return;
    }

    [DeclareMenu]
    public void MenuFunction()
    {
        Eplan.EplApi.Gui.Menu oMenu = new Eplan.EplApi.Gui.Menu();

        oMenu.AddMenuItem(
            "Snabelstoffmeny med renter",// Name: menu item
            "MenuAction", // Name: Action
            "Statustext", // Statustext
            37024 , // menu ID: Insert / Smart Window ...
            1 , // 1 = behind menu item, 0 = before menu item
            false , // display separator before
            false  // display separator behind
            );

        return;
    }
}