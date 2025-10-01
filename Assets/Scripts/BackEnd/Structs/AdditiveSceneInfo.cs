using System;
using BackEnd.Project_inspector_Addons;
using UnityEngine.Serialization;

namespace BackEnd.Structs
{
    [Serializable]
    public struct AdditiveSceneInfo
    {
        public SceneReference Scene;
        public int FlagIndex;
        public bool Toggled;

        public AdditiveSceneInfo(SceneReference scene, int flagIndex, bool toggled = true)
        {
            Scene = scene;
            FlagIndex = flagIndex;
            Toggled = toggled;
        }
    }
}