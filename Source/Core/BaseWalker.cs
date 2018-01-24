#region License
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

using System;
using tainicom.Aether.Elementary;
using System.Collections;
using tainicom.Aether.Elementary.Managers;

namespace tainicom.Aether.Core
{
    public class BaseWalker<T>: IAetherWalker<T>
        where T : IAether
    {
        #region Public Properties
        public T Current { get; protected set; }
        object IEnumerator.Current { get { return this.Current; } }
        public IAetherManager Manager { get; set; }
        #endregion

        #region Private / Protected
        
        #endregion

        #region Constructors
        /// <summary>
        /// Constructs a new instance of the <see cref="BaseWalker" /> class.
        /// </summary>
        public BaseWalker()
        {
        }
        #endregion

        #region Methods

        public virtual void Dispose()
        {
            Current = default(T);
        }
        
        public virtual bool MoveNext()
        {
            throw new NotImplementedException();
        }

        public virtual void Reset()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Events
        #endregion

        #region Event Handlers
        #endregion


        
    }
}
