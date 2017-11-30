﻿#region License
//   Copyright 2015 Kastellanos Nikolaos
//
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//
//       http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
#endregion

using System.Collections;
using System.Collections.Generic;
using tainicom.Aether.Elementary;

namespace tainicom.Aether.Core.Walkers
{
    public class DepthFirstWalker : BaseWalker
    {
        protected IAether startingElement;

        protected struct Breadcrumb
        {
            public IPlasma Plasma;
            public IEnumerator Enumerator;

            public Breadcrumb(IPlasma plasma, IEnumerator Enumerator)
            {
                this.Plasma = plasma;
                this.Enumerator = Enumerator;
            }
        }

        protected Breadcrumb currentNode;
        protected Queue<Breadcrumb> BreadcrumbQueue;

        public DepthFirstWalker(IAether startingElement)
        {
            this.startingElement = startingElement;
            BreadcrumbQueue = new Queue<Breadcrumb>(16);
        }

        public override void Reset()
        {
            currentNode.Plasma = null;
            currentNode.Enumerator = null;
            Current = null;
        }

        public override bool MoveNext()
        {
            return internalMoveNext();
        }

        //this method is used to break Recursive through the Super class when DepthFirstWalker is inherited
        private bool internalMoveNext()
        {
            if (currentNode.Plasma == null)
            {
                Current = startingElement;
                BreadcrumbQueue.Clear();
                IEnumerator<IAether> enumerator = GetParticles((IPlasma)Current);
                currentNode = new Breadcrumb((IPlasma)Current, enumerator);
                return true;
            }

            if (currentNode.Enumerator.MoveNext())
            {
                Current = (IAether)currentNode.Enumerator.Current;

                IPlasma plasma = Current as IPlasma;
                if (plasma != null)
                {
                    BreadcrumbQueue.Enqueue(currentNode);
                    IEnumerator<IAether> enumerator = GetParticles(plasma);
                    currentNode = new Breadcrumb(plasma, enumerator);
                }
                return true;
            }
            else
            {
                if (BreadcrumbQueue.Count > 0)
                {
                    currentNode = BreadcrumbQueue.Dequeue();
                    internalMoveNext(); //MoveNext();
                    return true;
                }
            }

            return false;
        }

        protected virtual IEnumerator<IAether> GetParticles(IPlasma plasma)
        {
            return plasma.GetEnumerator();
        }

    }
}
