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

using Microsoft.Xna.Framework;
using tainicom.Aether.Elementary.Spatial;
using tainicom.Aether.Elementary.Visual;

namespace tainicom.Aether.Elementary.Cameras
{
    public interface ICamera: IReadonlyCameraMatrices, IPosition, IAether, ICameraNode
    {
        //fundamental
        //Matrix View { get; } // Defined in IReadonlyCameraMatrices
        //Matrix Projection { get; } // Defined in IReadonlyCameraMatrices

        //view related
        //Vector3 Position { get; set; } //Defined in IPosition
        Vector3 LookAt { get; set; }
        Vector3 Up { get; set; }
        //view related extensions
        Vector3 Forward { get; set; }
        Vector3 Left { get; }

        //projection related
        float NearPlane { get; set; }
        float FarPlane { get; set; }

        //updates
        void UpdateView();
        void UpdateProjection();
        void SuspendUpdates();
        void ResumeUpdates();
    }
}
