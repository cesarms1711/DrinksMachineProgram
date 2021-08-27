using DrinksMachineProgram.Entities;
using DrinksMachineProgram.Resources;

using System;
using System.Collections.Generic;
using System.Linq;

namespace DrinksMachineProgram.BusinessLayer
{

    public class CoinsBL : IEntityBL<Coin, short>
    {

        #region Private Attributes

        private short MaxId = 0;

        private static List<Coin> Coins = new();

        #endregion Private Attributes

        #region CTOR

        private CoinsBL() { }

        #endregion CTOR

        #region Singleton Instance

        private static CoinsBL _instance;

        public static CoinsBL Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new CoinsBL();
                }

                return _instance;
            }
        }

        #endregion Singleton Instance

        #region Public Methods

        public List<Coin> List()
        {
            return Coins
                .OrderByDescending(c => c.Value)
                .ToList();
        }

        public Coin Detail(short id)
        {
            Coin coin = Coins.First(c => c.Id == id);

            if (coin == null) throw new Exception(TextResources.MessageErrorRecordDoesNotExist);

            return coin;
        }

        public void Create(Coin coin)
        {
            MaxId++;

            coin.Id = MaxId;

            Coins.Add(coin);
        }

        public void Edit(Coin coin)
        {
            Coin modifiedCoin = Coins.First(c => c.Id == coin.Id);

            modifiedCoin.Name = coin.Name;
            modifiedCoin.Value = coin.Value;
            modifiedCoin.QuantityAvailable = coin.QuantityAvailable;
        }

        public void Delete(short id)
        {
            Coins = Coins
                .Where(c => c.Id != id)
                .ToList();
        }

        #endregion Public Methods

    }

}