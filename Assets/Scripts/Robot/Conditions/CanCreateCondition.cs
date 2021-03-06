﻿using HK.Framework.Text;
using UnityEngine;

namespace HK.Nanka.RobotSystems.Conditions
{
    /// <summary>
    /// アイテムを生成できるか判断するクラス
    /// </summary>
    [CreateAssetMenu(menuName = "MasterData/Robot/Condition/CanCreate")]
    public sealed class CanCreateCondition : Condition
    {
        [SerializeField]
        private StringAsset.Finder itemName;

        public override bool Can
        {
            get
            {
                var gameController = GameController.Instance;
                var itemSpec = gameController.Recipes.List.Find(r => r.ProductItemName == itemName);
                return itemSpec.CanCreate(gameController.Player.Inventory);
            }
        }
    }
}
