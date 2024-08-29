namespace ClassLib;
/// <summary>
/// Структура, используемая для вычисления значения универсальной
/// хэш-функции
/// </summary>
internal struct UniHashFunction
{
    /// <summary>
    /// Коэфициент А
    /// </summary>
    public int A { get; set; }
    /// <summary>
    /// Коэфициент B 
    /// </summary>
    public int B { get; set; }
    /// <summary>
    /// Константное простое число Р
    /// </summary>
    public const int P = 65535;
    /// <summary>
    /// Создает новую универсальную хэш-функцию с А и В равными нулю
    /// </summary>
    public UniHashFunction() { }
    /// <summary>
    /// Создает новую универсальную хэш-функцию с данными значениями А и В
    /// </summary>
    /// <param name="a">Коэфициент А</param>
    /// <param name="b">Коэфициент В</param>
    public UniHashFunction(int a, int b)
    {
        A = a;
        B = b;
    }
    /// <summary>
    /// Вычисляет значение универсальной хэш функции от данного объекта
    /// </summary>
    /// <typeparam name="T">Тип объекта</typeparam>
    /// <param name="value">Объект</param>
    /// <returns>Значение универсальной хэш функции от данного объекта</returns>
    public readonly int GetHashCode<T>(T value) 
        => Math.Abs((A * value.GetHashCode() + B) % P);

}
