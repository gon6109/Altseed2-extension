using Altseed2;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Altseed2Extension.Node
{
    /// <summary>
    /// ロード用デリゲート
    /// </summary>
    /// <param name="loader">ロードするオブジェクト</param>
    /// <returns>タスク</returns>
    public delegate Task LoadFunc(ILoader loader);

    /// <summary>
    /// ローディングシーン
    /// </summary>
    public abstract class LoadingScene : Altseed2.Node, ILoader
    {
        /// <summary>
        /// 次に遷移するシーン
        /// </summary>
        public Altseed2.Node To { get; }

        /// <summary>
        /// ロード用デリゲート
        /// </summary>
        public LoadFunc LoadFunc { get; }

        /// <summary>
        /// 進捗(0.0-1.0)
        /// </summary>
        public float Progress => ProgressInfo.taskCount != 0 ? ProgressInfo.progress / (float)ProgressInfo.taskCount : 0;

        public (int taskCount, int progress) ProgressInfo { get; set; } = (0, 0);
        Task task;
        private readonly TransitionNode transition;
        private IEnumerator<object> coroutine;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="to">次に遷移するシーン</param>
        /// <param name="loadFunc">ロード用デリゲート</param>
        public LoadingScene(Altseed2.Node to, LoadFunc loadFunc, TransitionNode transition = null)
        {
            To = to;
            LoadFunc = loadFunc;
            this.transition = transition;
            coroutine = Update();
        }

        protected override void OnAdded()
        {
            base.OnAdded();
            task = LoadFunc(this);
        }

        protected override void OnUpdate()
        {
            coroutine?.MoveNext();
            base.OnUpdate();
        }

        IEnumerator<object> Update()
        {
            while (!task.IsCanceled && !task.IsCompleted && !task.IsFaulted)
            {
                yield return null;
            }

            if (task.IsCompleted)
            {
                if (transition == null)
                {
                    Engine.RemoveNode(this);
                    Engine.AddNode(To);
                }
                else
                {
                    Engine.AddNode(transition);
                }
            }
            if (task.IsCanceled || task.IsFaulted)
            {
                Engine.Log.Error(LogCategory.User, To.ToString() + "のロードに失敗しました.");
            }
        }
    }
}
