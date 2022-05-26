public interface ICoins
{
    int coins { get; set; }
    void Initialize();
    void GetFreeCoins();
    void GetExtraCoins();
    void SpendOneCoin();
    // void SetGambleCoins(int coinsToAdd);
    // void Gamble();
}
