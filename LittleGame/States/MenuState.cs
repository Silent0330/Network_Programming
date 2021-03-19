using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LittleGame.State
{
    class MenuState : GameState
    {
        private System.Windows.Forms.Button joinButton;
        private System.Windows.Forms.Button exitButton;
        private System.Windows.Forms.TextBox textBox;

        public MenuState(GameStateManager gsm)
        {
            this.gsm = gsm;
            this.joinButton = new System.Windows.Forms.Button();
            this.exitButton = new System.Windows.Forms.Button();
            this.textBox = new System.Windows.Forms.TextBox();

            #region panel
            this.BackColor = System.Drawing.Color.LightBlue;
            this.BackgroundImage = global::LittleGame.Properties.Resources.menubg;
            this.BackgroundImageLayout = ImageLayout.Stretch;
            this.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "menu state";
            this.Size = new System.Drawing.Size(800, 600);
            this.TabIndex = 0;
            this.Controls.Add(joinButton);
            this.Controls.Add(exitButton);
            #endregion

            //textBox
            this.textBox.Location = new System.Drawing.Point(300, 200);
            this.textBox.Margin = new System.Windows.Forms.Padding(4);
            this.textBox.Name = "ip textBox";
            this.textBox.Size = new System.Drawing.Size(200, 50);
            this.textBox.TabIndex = 4;
            this.textBox.Text = "192.168.0.100";
            this.textBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox_KeyDown);
            
            //join button
            this.joinButton.BackColor = System.Drawing.Color.Transparent;
            this.joinButton.FlatAppearance.BorderSize = 0;
            this.joinButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.joinButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.joinButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.joinButton.Font = new System.Drawing.Font("標楷體", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.joinButton.ForeColor = System.Drawing.Color.Black;
            this.joinButton.Location = new System.Drawing.Point(350, 250);
            this.joinButton.Name = "join button";
            this.joinButton.Size = new System.Drawing.Size(100, 40);
            this.joinButton.TabIndex = 0;
            this.joinButton.Text = "JOIN";
            this.joinButton.UseVisualStyleBackColor = false;
            this.joinButton.Click += new System.EventHandler(this.join_Click);
            this.joinButton.MouseLeave += new System.EventHandler(this.join_MouseLeave);
            this.joinButton.MouseHover += new System.EventHandler(this.join_MouseHover);

            //exit button
            this.exitButton.BackColor = System.Drawing.Color.Transparent;
            this.exitButton.FlatAppearance.BorderSize = 0;
            this.exitButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.exitButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.exitButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.exitButton.Font = new System.Drawing.Font("標楷體", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.exitButton.ForeColor = System.Drawing.Color.Black;
            this.exitButton.Location = new System.Drawing.Point(350, 350);
            this.exitButton.Name = "exit button";
            this.exitButton.Size = new System.Drawing.Size(100, 40);
            this.exitButton.TabIndex = 0;
            this.exitButton.Text = "EXIT";
            this.exitButton.UseVisualStyleBackColor = false;
            this.exitButton.Click += new System.EventHandler(this.exit_Click);
            this.exitButton.MouseLeave += new System.EventHandler(this.exit_MouseLeave);
            this.exitButton.MouseHover += new System.EventHandler(this.exit_MouseHover);
        }
        
        public override void update()
        {

        }

        public override void keyDown(KeyEventArgs e)
        {

        }

        public override void keyUp(KeyEventArgs e)
        {

        }
        
        //textBox

        private void textBox_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                this.Controls.Remove(textBox);
            }
        }
        
        //join button
        private void join_Click(object sender, EventArgs e)
        {
            this.Controls.Add(textBox);
            this.textBox.Focus();
        }

        private void join_MouseHover(object sender, EventArgs e)
        {
            joinButton.ForeColor = System.Drawing.Color.Red;
        }

        private void join_MouseLeave(object sender, EventArgs e)
        {
            joinButton.ForeColor = System.Drawing.Color.Black;
        }

        //exit button
        private void exit_Click(object sender, EventArgs e)
        {
            gsm.form.Close();
        }

        private void exit_MouseHover(object sender, EventArgs e)
        {
            exitButton.ForeColor = System.Drawing.Color.Red;
        }

        private void exit_MouseLeave(object sender, EventArgs e)
        {
            exitButton.ForeColor = System.Drawing.Color.Black;
        }
        
    }
}
