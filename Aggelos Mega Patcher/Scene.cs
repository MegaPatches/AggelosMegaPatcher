using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aggelos_Mega_Patcher
{
    /************************************************************
     * Scene
     * 
     * This class represents all aspects of a scene that we
     * want to track for modification.
     ************************************************************/
    public class Scene
    {
        public string Name { get; set; }    //Name to display in the view
        public int ID { get; set; }         //Scene number used in the save file
        public int x { get; set; }          //X coordinate in the save file
        public int y { get; set; }          //y coordinate in the save file

        public Scene(string name, int id, int x, int y)
        {
            this.Name = name;
            this.ID = id;
            this.x = x;
            this.y = y;
        }
    }
}
