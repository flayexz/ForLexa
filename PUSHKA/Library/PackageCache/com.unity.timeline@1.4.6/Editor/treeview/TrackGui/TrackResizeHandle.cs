using System;
using Unity.Collections;
using UnityEngine;

namespace UnityEditor.U2D.PSD
{
    [Serializable]
    class PSDLayer
    {
        [SerializeField]
        string m_Name;
        [SerializeField]
        string m_SpriteName;
        [SerializeField]
        bool m_IsGroup;
        [SerializeField]
        int m_ParentIndex;
        [SerializeField]
        string m_SpriteID;
        [SerializeField]
        int m_LayerID;
        [SerializeField]
        Vector2Int m_MosaicPosition;

        [NonSerialized]
        GameObject m_GameObject;

        public PSDLayer(NativeArray<Color32> tex, int parent, bool group, string layerName, int width, int height, int id)
        {
            isGroup = group;
            parentIndex = parent;
            texture = tex;
            name = layerName;
            this.width = width;
            this.height = height;
            layerID = id;
        }

        publ