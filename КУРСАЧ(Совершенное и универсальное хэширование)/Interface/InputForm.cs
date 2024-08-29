using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using TextBox = System.Windows.Forms.TextBox;

namespace Interface;

public partial class InputForm : Form
{
    public Dictionary<string, TextBox> TextBoxes { get; set; }

    private const int TOP_MARGIN               = 50;
    private const int LEFT_MARGIN              = 50;
    private const int TEXTBOX_WIDTH            = 100;
    private const int TEXTBOX_LENGTH           = 150;
    private const int MARGIN_BETWEEN_TEXTBOXES = 70;

    public InputForm()
    {

        InitializeComponent();
    }

    public void CreateLabeles(string[] labels)
    {
        TextBoxes = new();
        for (int i = 0; i < labels.Length; i++)
        {
            var textBox = new TextBox()
            {
                Multiline = true,
                PlaceholderText = labels[i],
                Top = TOP_MARGIN,
                Left = LEFT_MARGIN + (TEXTBOX_LENGTH + MARGIN_BETWEEN_TEXTBOXES) * i,
                Size = new(TEXTBOX_LENGTH, TEXTBOX_WIDTH)
            };
            TextBoxes.Add(labels[i], textBox);
        }
        foreach (var kv in TextBoxes)
        {
            this.Controls.Add(kv.Value);
            kv.Value.Show();
        }
    }

    private void button1_Click(object sender, EventArgs e)
    {
        this.Close();
    }
}
