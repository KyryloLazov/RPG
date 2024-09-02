using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class ActionScheduler : MonoBehaviour
    {
        private IAction CurrAction;

        public void StartAction(IAction action)
        {
            if (CurrAction == action)
            {
                return;
            }
            if (CurrAction != null)
            {          
                CurrAction.Cancel();
            }
            CurrAction = action;
            
        }

        public void CancelAction()
        {
            StartAction(null);
        }
    }
}
