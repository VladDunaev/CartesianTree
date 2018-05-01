﻿using System;

namespace CartesianTree
{
    /// <summary>
    /// Узел декартового дерева
    /// </summary>
    /// <typeparam name="TValue">Информация хранимая в декартовом дереве</typeparam>
    class NodeOfCartesianTree<TValue> where TValue:IComparable
    {
        /// <summary>
        /// Объект для генерации случайных величин
        /// </summary>
        private static Random random = new Random();

        /// <summary>
        /// Информация хранящайся в узле дерева
        /// </summary>
        public NodeInfo<TValue> Info { get; set; }

        /// <summary>
        /// Приоритет узла
        /// </summary>
        public int Priority { get; private set; }

        /// <summary>
        /// Левый сын дерева
        /// </summary>
        public NodeOfCartesianTree<TValue> Left { get; set; }

        /// <summary>
        /// Правый сын дерева
        /// </summary>
        public NodeOfCartesianTree<TValue> Right { get; set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="info">Информация хранящайся в узле дерева</param>
        /// <param name="left">Левый сын</param>
        /// <param name="right">Правый сын</param>
        public NodeOfCartesianTree(NodeInfo<TValue> info = null, NodeOfCartesianTree<TValue> left = null, NodeOfCartesianTree<TValue> right = null)
        {
            Info = info;
            Priority = random.Next(int.MinValue, int.MaxValue);
            Left = left;
            Right = right;
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="info">Информация хранящайся в узле дерева</param>
        /// <param name="prioryty">Приоритет вершины</param>
        /// <param name="left">Левый сын</param>
        /// <param name="right">Правый сын</param>
        public NodeOfCartesianTree(NodeInfo<TValue> info, int prioryty, NodeOfCartesianTree<TValue> left, NodeOfCartesianTree<TValue> right)
        {
            Info = info;
            Priority = prioryty;
            Left = left;
            Right = right;
        }

        /// <summary>
        /// Операция слияния двух декартовых деревеьев
        /// Ключи правого поддерева не меньше, чем ключи левого поддерева
        /// </summary>
        /// <param name="left">Левое поддерево</param>
        /// <param name="right">Правое поддерево</param>
        /// <returns>Результирующее дерево</returns>
        public static NodeOfCartesianTree<TValue> Merge(NodeOfCartesianTree<TValue> left, NodeOfCartesianTree<TValue> right)
        {
            if (left == null) return right;
            if (right == null) return left;
            if (left.Priority > right.Priority)
            {
                var newRight = Merge(left.Right, right);
                left.Right = newRight;
                left.Info.Update(left.Left.Info, left.Right.Info);
                return left;
            }
            else
            {
                var newLeft = Merge(left, right.Left);
                right.Left = newLeft;
                right.Info.Update(right.Left.Info, right.Right.Info);
                return right;
            }
        }

        /// <summary>
        /// Разделяет текущее дерево по ключу x (в левом дереве все что меньше или равно x, в правом остальные)
        /// </summary>
        /// <param name="x">Ключ, по которому происходит разделение</param>
        /// <param name="left">Получивщееся левое поддереов</param>
        /// <param name="right">Получившееся правое поддерево</param>
        public void Split(TValue x, out NodeOfCartesianTree<TValue> left, out NodeOfCartesianTree<TValue> right) 
        {
            NodeOfCartesianTree<TValue> newTree = null;
            if (Info.Value.CompareTo(x) <= 0)
            {
                if (Right == null)
                {
                    right = null;
                }
                else
                {
                    Right.Split(x, out newTree, out right);
                }
                left = new NodeOfCartesianTree<TValue>(Info, Priority, Left, newTree);
                left.Info.Update(left.Left.Info, left.Right.Info);
            }
            else
            {
                if (Left == null)
                {
                    left = null;
                }
                else
                {
                    Left.Split(x, out left, out newTree);
                }
                right = new NodeOfCartesianTree<TValue>(Info, Priority, newTree, Right);
                right.Info.Update(right.Left.Info, right.Right.Info);
            }

        }
    }
}