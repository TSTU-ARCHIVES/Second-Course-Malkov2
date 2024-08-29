using System.Text;

namespace ClassLib;
/// <summary>
/// Класс, позволяющий осуществлять лексический анализ текста
///  с помощью универсального и совршенного хэширования
/// </summary>
public class TextAnalyzer
{
    /// <summary>
    /// Хэш-таблица лежащая во главе всего
    /// </summary>

    private PerfectHashTable<string, MyList<WordInfo>> _entries;
    /// <summary>
    /// Массив слов в тексте
    /// </summary>
    public string[] Words { get; private set; }
    /// <summary>
    /// Создает новый экземпляр лексического анализатора от данного текста
    /// </summary>
    /// <param name="text"></param>

    public TextAnalyzer(string text)
    {
        Words = text.Split(' ').Select(w => w.Trim()).ToArray();
        InitTable();
    }
    /// <summary>
    /// Инициализирует хэш-таблицу
    /// </summary>
    private void InitTable()
    {
        var tempHashTable = new UniHashTable<string, MyList<WordInfo>>();
        int position = 0;
        foreach(var word in Words)
        {
            if (tempHashTable.TryGetValue(word, out var list))
            {
                list.Add(new(position));
            }
            else
            {
                tempHashTable.Add(word, [new(position)]);
            }

            position ++;
        }

        _entries = new PerfectHashTable<string, MyList<WordInfo>>(tempHashTable.GetKeyValuePairs());
        return;
    }
    /// <summary>
    /// Получает список информаций о каждом вхождении слова в текст
    /// </summary>
    /// <param name="word">Проверяемое слово</param>
    /// <returns>Список информаций о каждом вхождении слова в текст либо пустой массив если оное отсутсвует</returns>
    public MyList<WordInfo> GetWordInfo(string word)
    {
        if (_entries.TryGetValue(word, out var list))
            return list;
        return [];
    }
    /// <summary>
    /// Заменяет одно слово в тексте на другое
    /// </summary>
    /// <param name="oldWord">Заменяемое слово</param>
    /// <param name="newWord">Слово-замена</param>
    public void ReplaceWord(string oldWord, string newWord)
    {
        if(_entries.TryGetValue(oldWord, out var list))
        {
            if (!_entries.TryGetValue(newWord, out var newWordList))
            {
                newWordList = [];
                _entries.Add(newWord, newWordList);
            }

            foreach (var wordInfo in list)
            {
                Words[wordInfo.Position] = newWord;
                newWordList.Add(wordInfo);
            }
            _entries.DeleteKey(oldWord);
        }

    }
    /// <summary>
    /// Перечисляет n самых часто встречающихся слов в тексте
    /// а именно их самих и количество их вхождений в текст
    /// </summary>
    /// <param name="n">Необходимое количество слов</param>
    /// <returns>n самых часто встречающихся слов в тексте
    /// а именно их самих и количество их вхождений в текст</returns>
    public IEnumerable<(string, int)> GetTopPopularWords(int n)
    {
        var top = new PriorityQueue<string, int>();
        foreach (var wordEntry in _entries.GetKeyValuePairs())
        {
            top.Enqueue(wordEntry.Key, wordEntry.Value.Count);
            if (top.Count > n)
                top.Dequeue();
        }


        while (top.Count > 0)
        {
            var word = top.Dequeue();
            yield return (word, _entries.Get(word).Count);
        }
        yield break;
    }
    /// <summary>
    /// Возвращает полную статистику по всем словам
    /// </summary>
    /// <returns>Перечисление строк для каждого из слов</returns>
    public IEnumerable<string> GetFullStatistics()
    {
        foreach(var kv in _entries.GetKeyValuePairs())
        {
            var wordString = new StringBuilder(kv.Key);
            wordString.Append(": ");
            foreach(var entry in kv.Value)
            {
                wordString.Append(entry.ToString() + " ");
            }
            wordString.Append(';');
            yield return wordString.ToString();
        }
        yield break;
    }

}
