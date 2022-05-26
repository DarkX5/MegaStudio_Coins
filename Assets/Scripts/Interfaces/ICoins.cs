public interface ICoins
{
    int coins { get; set; }
    void Initialize();
    void GetFreeCoins();
    void GetExtraCoins();
    void Gamble();
}
