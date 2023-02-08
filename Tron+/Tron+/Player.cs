using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tron_
{
    class Player
    {
        public int playerNumber { get; set; }
        public enum Direction { Up, Down, Left, Right, Neutral };
        public int x { get; set; }
        public int y { get; set; }
        public int speed { get; set; }
        public int level { get; set; }
        public bool dead { get; set; }
        public Direction direction { get; set; }
        public bool jumping { get; set; }
        public int jumpTicks { get; set; }
        public Brush playerColor { get; set; }
        public List<Wall> walls { get; set; }
        public List<bool> jumps { get; set; }
        public Player(int ex, int why, int number, Direction dir)
        {
            x = ex;
            y = why;
            speed = 20;
            dead = false;
            //direction = dir;
            direction = Direction.Neutral;
            jumping = false;
            jumpTicks = 0;
            level = 1;
            playerNumber = number;
            jumps = new List<bool>();
            walls = new List<Wall>();
            playerColor = Brushes.Black;
        }
    }
}
