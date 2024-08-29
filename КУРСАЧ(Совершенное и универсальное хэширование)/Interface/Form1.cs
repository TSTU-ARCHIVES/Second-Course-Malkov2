using static System.Net.Mime.MediaTypeNames;

namespace Interface;

public partial class Form1 : Form
{
    Handler handler;
    public Form1()
    {
        handler = new Handler();
        InitializeComponent();
    }

    private void button1_Click(object sender, EventArgs e)
    {
        openFileDialog1 = new();
        openFileDialog1.Multiselect = false;
        openFileDialog1.Filter = "txt files(*.txt) | *.txt";
        if (openFileDialog1.ShowDialog() == DialogResult.OK)
        {
            handler.OpenFile(openFileDialog1.FileName);
            textBox1.Text = string.Join(' ', handler.GetText());
        }
    }

    private void button3_Click(object sender, EventArgs e)
    {

        var inputForm = new InputForm();
        inputForm.CreateLabeles(["Искомое слово"]);
        inputForm.ShowDialog();

        MessageBox.Show(handler.ShowWordStat(inputForm.TextBoxes["Искомое слово"].Text));
    }

    private void button4_Click(object sender, EventArgs e)
    {
        var inputForm = new InputForm();
        inputForm.CreateLabeles(["Заменяемое слово", "Новое слово"]);
        inputForm.ShowDialog();
        handler.ReplaceWord(
            inputForm.TextBoxes["Заменяемое слово"].Text,
            inputForm.TextBoxes["Новое слово"].Text
            );
        textBox1.Text = string.Join(' ', handler.GetText());

    }

    private void button2_Click(object sender, EventArgs e)
    {
        textBox1.Text = string.Join(' ', handler.GetHighlightedText(5));
    }

    private void button6_Click(object sender, EventArgs e)
    {
        saveFileDialog1 = new();
        saveFileDialog1.Filter = "txt files(*.txt) | *.txt";
        if (saveFileDialog1.ShowDialog() == DialogResult.OK)
        {
            handler.SaveStats(saveFileDialog1.FileName);
        }
    }

    private void button5_Click(object sender, EventArgs e)
    {
        textBox1.Text = string.Join(' ', handler.GetText());
    }

    private void button7_Click(object sender, EventArgs e)
    {
        var inputForm = new InputForm();
        inputForm.CreateLabeles(["Количество самых популярных слов"]);
        inputForm.ShowDialog();
        if (!int.TryParse(inputForm.TextBoxes["Количество самых популярных слов"].Text, out var n))
            return;

        saveFileDialog1 = new();
        saveFileDialog1.Filter = "txt files(*.txt) | *.txt";
        if (saveFileDialog1.ShowDialog() == DialogResult.OK)
        {
            handler.SaveTopStats(saveFileDialog1.FileName, n);
        }
    }

    private void button8_Click(object sender, EventArgs e)
    {
        saveFileDialog1 = new();
        saveFileDialog1.Filter = "txt files(*.txt) | *.txt";
        if (saveFileDialog1.ShowDialog() == DialogResult.OK)
        {
            handler.Save(saveFileDialog1.FileName);
        }
    }
}
