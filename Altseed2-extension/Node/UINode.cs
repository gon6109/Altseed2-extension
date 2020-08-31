using Altseed2;
using Altseed2Extension.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Altseed2Extension.Node
{
    public class UINode : Altseed2.Node
    {
        private UINode focusedUINode;
        private bool _isFocused;
        private bool _isEnable;

        /// <summary>
        /// フォーカスされているか
        /// </summary>
        public virtual bool IsFocused
        {
            get => _isFocused;
            set
            {
                _isFocused = value;
                OnChangedFocus(_isFocused);
            }
        }

        /// <summary>
        /// 有効か否か
        /// </summary>
        public virtual bool IsEnable
        {
            get => _isEnable;
            set
            {
                _isEnable = value;
                OnChangedEnable(_isEnable);
            }
        }

        /// <summary>
        /// 上への参照
        /// </summary>
        public UINode Up { get; set; }

        /// <summary>
        /// 下への参照
        /// </summary>
        public UINode Down { get; set; }

        /// <summary>
        /// 右への参照
        /// </summary>
        public UINode Right { get; set; }

        /// <summary>
        /// 左への参照
        /// </summary>
        public UINode Left { get; set; }

        /// <summary>
        /// 要素のサイズ
        /// </summary>
        public Vector2F Size
        {
            get
            {
                if (Parent is TransformNode transform)
                {
                    var vector3
                        = transform.AbsoluteTransform.Transform3D(new Vector3F(transform.ContentSize.X, transform.ContentSize.Y, 0)) - transform.AbsoluteTransform.Transform3D(new Vector3F());
                    return new Vector2F(vector3.X, vector3.Y);
                }
                return default;
            }
        }

        /// <summary>
        /// フォーカス変更時のイベント
        /// </summary>
        public event Action<bool> OnChangedFocus = delegate { };

        /// <summary>
        /// IsEnable変更時のイベント
        /// </summary>
        public event Action<bool> OnChangedEnable = delegate { };

        /// <summary>
        /// 選択されたか
        /// </summary>
        public bool IsSelected
        {
            get
            {
                if (Manager != null)
                    return Manager.IsMoveFocus && IsEnable && IsFocused && GetIsSelectedFunc();
                return false;
            }
        }

        /// <summary>
        /// UIの基準座標
        /// </summary>
        public Vector2F Position
        {
            get
            {
                if (Parent is TransformNode transform)
                {
                    var vector3 = transform.AbsoluteTransform.Transform3D(new Vector3F());
                    return new Vector2F(vector3.X, vector3.Y);
                }
                return default;
            }
        }

        /// <summary>
        /// 選択条件
        /// </summary>
        public static Func<bool> GetIsSelectedFunc = delegate { return Input.Input.GetInputState(Inputs.A) == 1; };

        /// <summary>
        /// フォーカスされたオブジェクトが変更された時に発火するイベント
        /// </summary>
        public event Action OnChangedFocusedUINode = delegate { };

        /// <summary>
        /// フォーカスされたオブジェクト
        /// </summary>
        public UINode FocusedUINode
        {
            get => focusedUINode;
            set
            {
                if (focusedUINode == value) return;
                if (value != null)
                    value.IsFocused = true;
                if (focusedUINode != null) focusedUINode.IsFocused = false;
                focusedUINode = value;
                OnChangedFocusedUINode();
            }
        }

        public bool IsMoveFocus { get; set; }

        public UINode Manager => EnumerateAncestors().OfType<UINode>().FirstOrDefault();

        public IEnumerable<UINode> UINodeChildren => GetUINodeChildren(this);

        private IEnumerable<UINode> GetUINodeChildren(Altseed2.Node node)
        {
            if (node == null) yield break;

            var uiNode = node.Children.OfType<UINode>().FirstOrDefault();
            if (uiNode != null)
            {
                yield return uiNode;
            }
            else
            {
                foreach (var ui in node.Children.SelectMany(obj => GetUINodeChildren(obj)))
                {
                    yield return ui;
                }
            }
        }

        public UINode()
        {
            IsMoveFocus = true;
            IsEnable = true;
        }

        protected override void OnAdded()
        {
            base.OnAdded();
        }

        /// <summary>
        /// 接続を消去
        /// </summary>
        public void ResetConnection()
        {
            Up = null;
            Down = null;
            Left = null;
            Right = null;
            IsFocused = false;
        }

        /// <summary>
        /// 要素同士の関係を構築
        /// </summary>
        public void ConnectUINodes()
        {
            var uiChildren = UINodeChildren.ToList();

            foreach (var item in uiChildren)
            {
                item.ResetConnection();
            }
            FocusedUINode = null;

            uiChildren.Sort((a, b) => Math.Sign(a.Size.X * a.Size.Y - b.Size.X * b.Size.Y));
            foreach (var item in uiChildren.Where(obj => obj.IsEnable))
            {
                foreach (var item2 in uiChildren.Where(obj => obj.IsEnable && obj != item))
                {
                    var angle = (item2.Position - item.Position).Degree;
                    if (angle >= (-item.Size).Degree && angle < new Vector2F(item.Size.X / 2, -item.Size.Y / 2).Degree)
                    {
                        if (item.Up == null) item.Up = item2;
                        else
                        {
                            if ((item.Up.Position - item.Position).Length >= (item2.Position - item.Position).Length) item.Up = item2;
                        }
                    }
                    else if (angle >= new Vector2F(item.Size.X / 2, -item.Size.Y / 2).Degree && angle < item.Size.Degree)
                    {
                        if (item.Right == null) item.Right = item2;
                        else
                        {
                            if ((item.Right.Position - item.Position).Length >= (item2.Position - item.Position).Length) item.Right = item2;
                        }
                    }
                    else if (angle >= item.Size.Degree && angle < new Vector2F(-item.Size.X / 2, item.Size.Y / 2).Degree)
                    {
                        if (item.Down == null) item.Down = item2;
                        else
                        {
                            if ((item.Down.Position - item.Position).Length >= (item2.Position - item.Position).Length) item.Down = item2;
                        }
                    }
                    else if ((angle >= new Vector2F(-item.Size.X / 2, item.Size.Y / 2).Degree && angle <= 180)
                        || (angle >= -180 && angle < (-item.Size).Degree))
                    {
                        if (item.Left == null) item.Left = item2;
                        else
                        {
                            if ((item.Left.Position - item.Position).Length >= (item2.Position - item.Position).Length) item.Left = item2;
                        }
                    }
                }
            }

            FocusedUINode = uiChildren.Where(obj => obj.IsEnable).LastOrDefault();
        }

        protected override void OnUpdate()
        {
            if (IsMoveFocus && FocusedUINode != null)
            {
                if (GetIsPushedUpFunc()) FocusedUINode = FocusedUINode.Up ?? FocusedUINode;
                if (GetIsPushedRightFunc()) FocusedUINode = FocusedUINode.Right ?? FocusedUINode;
                if (GetIsPushedLeftFunc()) FocusedUINode = FocusedUINode.Left ?? FocusedUINode;
                if (GetIsPushedDownFunc()) FocusedUINode = FocusedUINode.Down ?? FocusedUINode;
            }
            base.OnUpdate();
        }

        /// <summary>
        /// 上押下判定
        /// </summary>
        public static Func<bool> GetIsPushedUpFunc = delegate { return Input.Input.GetInputState(Inputs.Up) == 1; };

        /// <summary>
        /// 下押下判定
        /// </summary>
        public static Func<bool> GetIsPushedDownFunc = delegate { return Input.Input.GetInputState(Inputs.Down) == 1; };

        /// <summary>
        /// 左押下判定
        /// </summary>
        public static Func<bool> GetIsPushedLeftFunc = delegate { return Input.Input.GetInputState(Inputs.Left) == 1; };

        /// <summary>
        /// 右押下判定
        /// </summary>
        public static Func<bool> GetIsPushedRightFunc = delegate { return Input.Input.GetInputState(Inputs.Right) == 1; };
    }
}
