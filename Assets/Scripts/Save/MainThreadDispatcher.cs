using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Save
{
    public class MainThreadDispatcher : MonoBehaviour
    {
        private static readonly Queue<Action> Actions = new Queue<Action>();

        public static void Enqueue(Action action)
        {
            lock (Actions)
            {
                Actions.Enqueue(action);
            }
        }

        void Update()
        {
            while (Actions.Count > 0)
            {
                Action action = null;

                lock (Actions)
                {
                    if (Actions.Count > 0)
                    {
                        action = Actions.Dequeue();
                    }
                }

                action?.Invoke();
            }
        }
    }

}
