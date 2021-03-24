namespace LittleGameSever
{
    partial class Form1
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btn_StartSever = new System.Windows.Forms.Button();
            this.txtBox_Log = new System.Windows.Forms.TextBox();
            this.btn_StopSever = new System.Windows.Forms.Button();
            this.btn_StartGame = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.btn_StopGame = new System.Windows.Forms.Button();
            this.btn_Exit = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_StartSever
            // 
            this.btn_StartSever.Location = new System.Drawing.Point(12, 386);
            this.btn_StartSever.Name = "btn_StartSever";
            this.btn_StartSever.Size = new System.Drawing.Size(143, 52);
            this.btn_StartSever.TabIndex = 0;
            this.btn_StartSever.Text = "Start Sever";
            this.btn_StartSever.UseVisualStyleBackColor = true;
            this.btn_StartSever.Click += new System.EventHandler(this.btn_StartSever_Click);
            // 
            // txtBox_Log
            // 
            this.txtBox_Log.Location = new System.Drawing.Point(12, 12);
            this.txtBox_Log.Multiline = true;
            this.txtBox_Log.Name = "txtBox_Log";
            this.txtBox_Log.ReadOnly = true;
            this.txtBox_Log.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtBox_Log.Size = new System.Drawing.Size(432, 332);
            this.txtBox_Log.TabIndex = 1;
            // 
            // btn_StopSever
            // 
            this.btn_StopSever.Enabled = false;
            this.btn_StopSever.Location = new System.Drawing.Point(161, 386);
            this.btn_StopSever.Name = "btn_StopSever";
            this.btn_StopSever.Size = new System.Drawing.Size(143, 52);
            this.btn_StopSever.TabIndex = 2;
            this.btn_StopSever.Text = "Stop Sever";
            this.btn_StopSever.UseVisualStyleBackColor = true;
            this.btn_StopSever.Click += new System.EventHandler(this.btn_StopSever_Click);
            // 
            // btn_StartGame
            // 
            this.btn_StartGame.Enabled = false;
            this.btn_StartGame.Location = new System.Drawing.Point(310, 386);
            this.btn_StartGame.Name = "btn_StartGame";
            this.btn_StartGame.Size = new System.Drawing.Size(143, 52);
            this.btn_StartGame.TabIndex = 4;
            this.btn_StartGame.Text = "Start Game";
            this.btn_StartGame.UseVisualStyleBackColor = true;
            this.btn_StartGame.Click += new System.EventHandler(this.button_StartGame_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(450, 12);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowTemplate.Height = 27;
            this.dataGridView1.Size = new System.Drawing.Size(338, 332);
            this.dataGridView1.TabIndex = 5;
            // 
            // btn_StopGame
            // 
            this.btn_StopGame.Enabled = false;
            this.btn_StopGame.Location = new System.Drawing.Point(459, 386);
            this.btn_StopGame.Name = "btn_StopGame";
            this.btn_StopGame.Size = new System.Drawing.Size(143, 52);
            this.btn_StopGame.TabIndex = 6;
            this.btn_StopGame.Text = "Stop Game";
            this.btn_StopGame.UseVisualStyleBackColor = true;
            this.btn_StopGame.Click += new System.EventHandler(this.btn_StopGame_Click);
            // 
            // btn_Exit
            // 
            this.btn_Exit.Location = new System.Drawing.Point(608, 386);
            this.btn_Exit.Name = "btn_Exit";
            this.btn_Exit.Size = new System.Drawing.Size(143, 52);
            this.btn_Exit.TabIndex = 7;
            this.btn_Exit.Text = "Exit";
            this.btn_Exit.UseVisualStyleBackColor = true;
            this.btn_Exit.Click += new System.EventHandler(this.btn_Exit_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btn_Exit);
            this.Controls.Add(this.btn_StopGame);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.btn_StartGame);
            this.Controls.Add(this.btn_StopSever);
            this.Controls.Add(this.txtBox_Log);
            this.Controls.Add(this.btn_StartSever);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_StartSever;
        private System.Windows.Forms.TextBox txtBox_Log;
        private System.Windows.Forms.Button btn_StopSever;
        private System.Windows.Forms.Button btn_StartGame;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button btn_StopGame;
        private System.Windows.Forms.Button btn_Exit;
    }
}

