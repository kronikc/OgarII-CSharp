﻿using Ogar_CSharp.worlds;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ogar_CSharp.cells
{
    public enum CellEatResult
    {
        None,
        Rigid,
        Eat,
        EatInvd
    }
    public abstract class Cell
    {
        public Boost boost = default;
        public QuadItem<Cell> item;
        public int id;
        public World world;
        public int birthTick;
        public bool exists;
        public Cell eatenBy;
        public Rect range;
        public bool isBoosting;
        //public Boost boost;
        public Player owner;
        private float x;
        private float y;
        private float size;
        private int color;
        private string name;
        private string skin;
        public bool posChanged, sizeChanged, colorChanged, nameChanged, skinChanged;
        public abstract byte Type { get; }
        public abstract bool IsSpiked { get; }
        public abstract bool IsAgitated { get; }
        public abstract bool AvoidWhenSpawning { get; }
        public virtual bool ShouldUpdate { get => posChanged || sizeChanged || colorChanged || nameChanged || skinChanged; }
        public int Age => (world.handle.tick - birthTick) * world.handle.stepMult;
        public float X { get => x; set { x = value; posChanged = true; } }
        public float Y { get => y; set { y = value; posChanged = true; } }
        public float Size { get => size; set { Misc.ThrowIfBadOrNegativeNumber(value); size = value; sizeChanged = true; } }
        public float SquareSize { get => size * size; set => Size = (short)Math.Sqrt(100 * value); }
        public float Mass { get => size * size / 100; set => Size = (short)Math.Sqrt(100 * value); }
        public int Color { get => color; set { color = value; colorChanged = true; } }
        public string Name { get => name; set { name = value; nameChanged = true; } }
        public string Skin { get => skin; set { skin = value; skinChanged = true; } }
        protected Cell(World world, float x, float y, float size, int color)
        {
            item = new QuadItem<Cell>(this);
            this.world = world;
            id = (int)world._nextCellId;
            birthTick = world.handle.tick;
            this.x = x;
            this.y = y;
            this.size = size;
            this.color = color;
        }
        public abstract CellEatResult GetEatResult(Cell other);
        public virtual void OnSpawned() { }
        public virtual void OnTick() 
        {
            posChanged = sizeChanged = colorChanged = nameChanged = skinChanged = false;
        }
        public virtual void WhenAte(Cell other)
        {
            SquareSize += other.SquareSize;
        }
        public virtual void WhenEatenBy(Cell other)
        {
            eatenBy = other;
        }
        public virtual void OnRemoved() { }
    }
}
