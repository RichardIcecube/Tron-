using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tron_
{
    public partial class Form1 : Form
    {
        //Todo:
        //implement power ups - the more "item boxes" you get, the higher your power becomes. you gain permanent abilities like jumping and shooting. think of some more
        //idk sometime shit crashes the game, try to figure out if there is a consistent cause.
        //Implement shooting
        List<Player> players = new List<Player>();
        //The max size of the board. Figured out through trial and error
        const int maxX = 39;
        const int maxY = 30;
        Random random = new Random();
        int itemX;
        int itemY;
        readonly Keys[] keys = { Keys.W, Keys.S, Keys.A, Keys.D, Keys.Space, Keys.NumPad8, Keys.NumPad5, Keys.NumPad4, Keys.NumPad6, Keys.Right, Keys.R };
        public Form1()
        {
            //when the form loads, both players are created, both 5 units away from their respective corners. For now, there are only two, but the code is
            // flexible enough to have as many players as needed. 
            InitializeComponent();
            Player temp = new Player(5,5,1, Player.Direction.Right);
            players.Add(temp);
            temp = new Player(maxX - 5, maxY - 5,2, Player.Direction.Left);
            players.Add(temp);
            //Sets the speed of the game
            uxTimer.Interval = 1000 / temp.speed;
            //starts the timer whose events run the game.
            uxTimer.Start();
            //starts a separate timer that allows for an input to be read at all times that resets the game.
            uxResetTimer.Start();
            itemX = random.Next(0, maxX);
            itemY = random.Next(0, maxY);
            players[0].direction = Player.Direction.Right;
            players[1].direction = Player.Direction.Left;
        }
        private void UpdateScreen(object sender, EventArgs e)
        {
            //Checks for the input of both players and adjusts the direction and jump state of them.
            if (Input.KeyPressed(Keys.W) && players[0].direction != Player.Direction.Down) players[0].direction = Player.Direction.Up;
            else if (Input.KeyPressed(Keys.S) && players[0].direction != Player.Direction.Up) players[0].direction = Player.Direction.Down;
            else if (Input.KeyPressed(Keys.A) && players[0].direction != Player.Direction.Right) players[0].direction = Player.Direction.Left;
            else if (Input.KeyPressed(Keys.D) && players[0].direction != Player.Direction.Left) players[0].direction = Player.Direction.Right;
            if (Input.KeyPressed(Keys.Space) && players[0].level > 1) players[0].jumping = true;
            if (Input.KeyPressed(Keys.NumPad8) && players[1].direction != Player.Direction.Down) players[1].direction = Player.Direction.Up;
            else if (Input.KeyPressed(Keys.NumPad5) && players[1].direction != Player.Direction.Up) players[1].direction = Player.Direction.Down;
            else if (Input.KeyPressed(Keys.NumPad4) && players[1].direction != Player.Direction.Right) players[1].direction = Player.Direction.Left;
            else if (Input.KeyPressed(Keys.NumPad6) && players[1].direction != Player.Direction.Left) players[1].direction = Player.Direction.Right;
            if (Input.KeyPressed(Keys.Right) && players[1].level > 1) players[1].jumping = true;
            //Calls the MovePlayer() method which adjusts the positions of each player based on their directions. 
            MovePlayer();
            //Calls uxBoard.Invalidate() which clears the current pictureBox then calls the Paint event of the picture box
            uxBoard.Invalidate();
        }
        private void uxBoard_Paint(object sender, PaintEventArgs e)
        {
            //This works with graphics. I'm not completely sure how this works, but I was able to figure it out by just following parameters and trial and error.
            for(int j = 0; j < players.Count; j++)
            {
                //Converts the picturebox into a graphics data type. By doing this, this allows specific "draw" commands.
                Graphics board = e.Graphics;
                if (!players[j].dead)
                {
                    Brush wallColor;
                    if (j == 0) wallColor = Brushes.Red;
                    else wallColor = Brushes.Blue;
                    Pen wallPen = new Pen(wallColor);
                    Point[] points = new Point[4];
                    for (int i = 1; i < players[j].walls.Count; i++)
                    {
                        board.DrawLine(wallPen, new Point((players[j].walls[i - 1].x * 16) + 8, (players[j].walls[i - 1].y * 16) + 8), new Point((players[j].walls[i].x * 16) + 8, (players[j].walls[i].y * 16) + 8));
                    }
                    //The numbers and modifiers applied to the x and y points are the result of trial and error. This just ensures that the visuals on the screen allign
                    // with each other cleanly. Without them, the walls and the bike shapes get really wonky. 
                    //The split between horizontal and vertical directions determine the shape of the bike. If it is facing left and right, the left and right sides of 
                    // the bike are longer than the up and down sides. Same applies to up and down.
                    if (players[j].direction == Player.Direction.Left || players[j].direction == Player.Direction.Right)
                    {
                        points[0] = new Point((players[j].x) * 16 - 10, (players[j].y) * 16 + 8);
                        points[1] = new Point(((players[j].x) * 16) + 7, ((players[j].y) * 16) + 4);
                        points[2] = new Point(((players[j].x) * 16) + 22, (players[j].y) * 16 + 8);
                        points[3] = new Point(((players[j].x) * 16) + 7, ((players[j].y) * 16) + 12);
                    }
                    else if (players[j].direction == Player.Direction.Up || players[j].direction == Player.Direction.Down)
                    {
                        points[0] = new Point((players[j].x) * 16 + 4, (players[j].y) * 16 + 7);
                        points[1] = new Point(((players[j].x) * 16 + 8), ((players[j].y) * 16) - 10);
                        points[2] = new Point(((players[j].x) * 16) + 12, (players[j].y) * 16 + 7);
                        points[3] = new Point(((players[j].x) * 16) + 8, ((players[j].y) * 16) + 22);
                    }
                    //This takes the points made above and creates a shape.
                    board.DrawClosedCurve(new Pen(players[j].playerColor), points);
                    board.DrawEllipse(new Pen(Brushes.Black), new Rectangle(itemX * 16, itemY * 16, 16, 16));
                }
            }
        }
        private void MovePlayer()
        {
            foreach (Player player in players)
            {
                //If the player is currently jumping, the jump duration is progressed.
                if (player.jumping) player.jumpTicks++;
                //This shifts the player up a space to visualize the jump on the first tick of the jump.
                if (player.jumping && player.jumpTicks == 1)
                {
                    if (player.direction.Equals(Player.Direction.Up) || player.direction.Equals(Player.Direction.Down)) player.x--;
                    else player.y--;
                }
                //At the end of the jump, the player's position is adjusted to land from the jump and the jump variables are reset.
                if (player.jumping && player.jumpTicks > 3)
                {
                    if (player.direction.Equals(Player.Direction.Up) || player.direction.Equals(Player.Direction.Down)) player.x++;
                    else player.y++;
                    player.jumping = false;
                    player.jumpTicks = 0;
                }
                //The player's position is adjusted based on their direction. Their previous position is added to the list of walls right before.
                //If the player is jumping, that is recorded in a parallel list to the wall list. This later ensures that walls created while jumping
                //collide with jumping players but allow grounded players to pass through.
                switch (player.direction)
                {
                    case Player.Direction.Up:
                        player.walls.Add(new Wall(player.x, player.y));
                        player.jumps.Add(player.jumping);
                        player.y--;
                        break;
                    case Player.Direction.Down:
                        player.walls.Add(new Wall(player.x, player.y));
                        player.jumps.Add(player.jumping);
                        player.y++;
                        break;
                    case Player.Direction.Left:
                        player.walls.Add(new Wall(player.x, player.y));
                        player.jumps.Add(player.jumping);
                        player.x--;
                        break;
                    case Player.Direction.Right:
                        player.walls.Add(new Wall(player.x, player.y));
                        player.jumps.Add(player.jumping);
                        player.x++;
                        break;
                    case Player.Direction.Neutral:
                        break;
                }
                //If the wall gets to a certain length, the first wall in the list is removed to maintain that maximum length. The corresponding item in the jump list
                // is removed.
                if (player.walls.Count > 100)
                {
                    player.walls.RemoveAt(0);
                    player.jumps.RemoveAt(0);
                }
                //This checks if the player is within the bounds of the arena. If not, the game ends. 
                if (player.x < 0 || player.x > 39 || player.y < 0 || player.y > 30)
                {
                    GameOver(player);
                }
                else
                {
                    //This checks if the player collides with any of the walls created. If so, the game ends. 
                    for(int j = 0; j < players.Count; j++)
                    {
                        for (int i = 0; i < players[j].walls.Count; i++)
                        {
                            if (player.x == players[j].walls[i].x && player.y == players[j].walls[i].y)
                            {
                                if (player.jumping == player.jumps[i])
                                {
                                    GameOver(player);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            foreach(Player player in players)
            {
                if (player.x == itemX && player.y == itemY && !player.jumping)
                {
                    player.level++;
                    itemX = random.Next(0, maxX);
                    itemY = random.Next(0, maxY);
                    switch(player.level)
                    {
                        case 1:
                            player.playerColor = Brushes.Black;
                            break;
                        case 2:
                            player.playerColor = Brushes.Purple;
                            break;
                        case 3:
                            player.playerColor = Brushes.Gold;
                            break;
                        default:
                            break;
                    }
                }
            }
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            //This was pulled from the tutorial video I watched, so I can't fully explain how this works. 
            Input.ChangeState(e.KeyCode, true);
        }
        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            //This was pulled from the tutorial video I watched, so I can't fully explain how this works. 
            Input.ChangeState(e.KeyCode, false);
        }
        private void GameOver(Player player)
        {
            //Ends the game
            uxTimer.Stop();
            MessageBox.Show("Player " + player.playerNumber + " died.");
        }
        private void Reset(object sender, EventArgs e)
        {
            //resets the game
            if (Input.KeyPressed(Keys.R)) StartGame();
        }
        //resets game
        private void StartGame()
        {
            //Deactivates each key. Without this, there is a chance that a key is stuck as active upon reset.
            foreach(Keys key in keys)
            {
                Input.ChangeState(key, false);
            }
            //restarts the parameters of the game
            players = new List<Player>();
            Player temp = new Player(5,5,1, Player.Direction.Right);
            players.Add(temp);
            temp = new Player(maxX - 5, maxY - 5,2, Player.Direction.Left);
            players.Add(temp);
            uxTimer.Interval = 1000 / temp.speed;
            uxTimer.Start();
            uxBoard.Image = null;
            uxBoard.InitialImage = null;
            players[0].direction = Player.Direction.Right;
            players[1].direction = Player.Direction.Left;
            players[0].level = 1;
            players[1].level = 1;
        }
    }
}
