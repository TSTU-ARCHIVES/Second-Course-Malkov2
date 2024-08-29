using ClassLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Interface;
/// <summary>
/// Класс, используемый для обработки формы
/// </summary>
internal class Handler
{
    /// <summary>
    /// Максимальное отображаемое количество слов
    /// </summary>
    const int MAX_AMOUNT_OF_WORDS = 500;
    /// <summary>
    /// Лексический анализатор
    /// </summary>
    TextAnalyzer _textAnalyzer;
    /// <summary>
    /// Открывает файл
    /// </summary>
    /// <param name="path">Путь к файлу</param>
    public void OpenFile(string path)
    {
        var sourceFile = new FileInfo(path);
        var reader = new StreamReader(sourceFile.FullName);
        _textAnalyzer = new(reader.ReadToEnd());
        reader.Close();
    }
    /// <summary>
    /// Перечисляет по слову весь текст в файле
    /// </summary>
    /// <returns>Весь текст в файле</returns>
    public IEnumerable<string> GetText() =>
        _textAnalyzer == null ? [] : _textAnalyzer.Words;
    /// <summary>
    /// Заменяет одно слово в тексте на другое
    /// </summary>
    /// <param name="old">Заменяемое слово</param>
    /// <param name="newWord">Новое слово</param>
    public void ReplaceWord(string old, string newWord)
    {
        if (_textAnalyzer == null)
        {
            return;
        }

        _textAnalyzer.ReplaceWord(old, newWord);
    }
    /// <summary>
    /// Возвращает информацию о слове
    /// </summary>
    /// <param name="word">Проверяемое слово</param>
    /// <returns>Информация о слове</returns>
    public string ShowWordStat(string word)
    {
        if (_textAnalyzer == null)
            return "Text does not defined";
        StringBuilder stringBuilder = new($"Статистика слова {word}: \n");
        var stat = _textAnalyzer.GetWordInfo(word);
        if (stat.Count == 0)
        {
            stringBuilder.Append("Слово не найдено");
            return stringBuilder.ToString();
        }
        stringBuilder.Append($"Встречается {stat.Count} раз, на позициях: \n");
        foreach (var kv in stat)
        {
            stringBuilder.Append($"{kv.Position} ");
        }

        return stringBuilder.ToString();
    }
    /// <summary>
    /// Перечисляет текст, выделяя необходимые слов с помощью символов _ с каждой стороны
    /// </summary>
    /// <param name="n">N самых популярных слов которые будут выделены</param>
    /// <returns>Текст, выделяя необходимые слов с помощью символов _ с каждой стороны</returns>
    public IEnumerable<string> GetHighlightedText(int n)
    {
        if (_textAnalyzer == null)
            yield break;
        var topWords = _textAnalyzer.GetTopPopularWords(n).Select(wp => wp.Item1).ToHashSet();
        foreach (var word in _textAnalyzer.Words)
        {
            if (topWords.Contains(word))
                yield return $"_{word}_";
            else
                yield return word;
        }
        yield break;
    }
    /// <summary>
    /// Сохраняет текущий текст в данный файл
    /// </summary>
    /// <param name="path">Путь к данному файлу</param>
    public void Save(string path)
    {
        if (_textAnalyzer == null)
            return ;
        var sourceFile = new FileInfo(path);
        var writer = new StreamWriter(sourceFile.FullName);
        foreach (var word in _textAnalyzer.Words)
            writer.Write(word + " ");


        writer.Close();
    }
    /// <summary>
    /// Сохраняет полную информацию о словах в тексте в данный файл
    /// </summary>
    /// <param name="path">Путь к данному файлу</param>
    public void SaveStats(string path)
    {
        if (_textAnalyzer == null)
            return;
        var sourceFile = new FileInfo(path);
        var writer = new StreamWriter(sourceFile.FullName);
        foreach(var stat in _textAnalyzer.GetFullStatistics())
            writer.WriteLine(stat);


        writer.Close();
    }
    /// <summary>
    /// Сохраняет в файл статистику о самых популярных слов
    /// </summary>
    /// <param name="path">Путь к данному файлу</param>
    /// <param name="n">Количество самых популярных слов</param>
    public void SaveTopStats(string path, int n)
    {
        if (_textAnalyzer == null)
            return;
        var sourceFile = new FileInfo(path);
        var writer = new StreamWriter(sourceFile.FullName);
        foreach (var stat in _textAnalyzer.GetTopPopularWords(n))
            writer.WriteLine($"{stat.Item1} - {stat.Item2} раз");


        writer.Close();
    }

}
