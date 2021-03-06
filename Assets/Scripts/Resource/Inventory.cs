﻿using System.Collections.Generic;
using System.Linq;
using HK.Framework.EventSystems;
using HK.Nanka.Events;
using UnityEngine;
using UnityEngine.Assertions;

namespace HK.Nanka
{
    public sealed class Inventory
    {
        public Dictionary<int, int> Items { private set; get; }

        public Inventory()
        {
            this.Items = new Dictionary<int, int>();
        }

        public void Add(int itemHash, int number)
        {
            Assert.IsTrue(number >= 1);
            if(!this.Items.ContainsKey(itemHash))
            {
                this.Items.Add(itemHash, 0);
            }

            this.Items[itemHash] += number;

            var itemSpecs = GameController.Instance.ItemSpecs;
            var item = itemSpecs.Get(itemHash);
            if (item.AddedInventoryTask != null)
            {
                item.AddedInventoryTask.Do();                
            }
            UniRxEvent.GlobalBroker.Publish(AddedItem.Get(item));
            Debug.Log(string.Format("{0} x {1}", item.Name, this.Items[itemHash]));
        }

        public void Remove(int itemHash, int number)
        {
            Assert.IsTrue(this.Items.ContainsKey(itemHash));
            this.Items[itemHash] -= number;
            Assert.IsTrue(this.Items[itemHash] >= 0, string.Format("{0}の所持数が{1}になりました", itemHash, this.Items[itemHash]));
        }

        public void Remove(Recipe recipe)
        {
            foreach(var i in recipe.Materials)
            {
                this.Remove(i.ItemName, i.Number);
            }
        }

        public int GetNumber(int itemHash)
        {
            if(!this.Items.ContainsKey(itemHash))
            {
                return 0;
            }

            return this.Items[itemHash];
        }

        public bool IsPossession(int itemHash)
        {
            return this.GetNumber(itemHash) > 0;
        }

        public List<Recipe> GetCraftingList(Recipes recipes)
        {
            return recipes.List.Where(r => r.CanVisibleList(this)).ToList();
        }
    }
}
