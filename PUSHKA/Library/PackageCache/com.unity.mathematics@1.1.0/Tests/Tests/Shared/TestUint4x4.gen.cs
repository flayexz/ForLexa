using System;
using System.Collections.Generic;

namespace UnityEngine.U2D
{
    // Spline Internal Meta Data.
    internal struct SplinePointMetaData
    {
        public float height;
        public uint spriteIndex;
        public int cornerMode;
    };    
    
    [Serializable]
    public class Spline
    {
        private static readonly string KErrorMessage = "Internal error: Point too close to neighbor";
        private static readonly float KEpsilon = 0.01f;
        [SerializeField]
        private bool m_IsOpenEnded;
        [SerializeField]
        private List<SplineControlPoint> m_ControlPoints = new List<SplineControlPoint>();

        public bool isOpenEnded
        {
            get
            {
                if (GetPointCount() < 3)
                    return true;

                return m_IsOpenEnded;
            }
            set { m_IsOpenEnded = value; }
        }

        private bool IsPositionValid(int index, int next, Vector3 point)
        {
            int prev = (index == 0) ? (m_ControlPoints.Count - 1) : (index - 1);
            next = (next >= m_ControlPoints.Count) ? 0 : next;
            if (prev >= 0)
            {
                Vector3 diff = m_ControlPoints[prev].position - point;
                if (diff.magnitude < KEpsilon)
                    return false;
            }
            if (next < m_ControlPoints.Count)
            {
                Vector3 diff = m_ControlPoints[next].position - point;
                if (diff.magnitude < KEpsilon)
                    return false;
            }
            return true;
        }

        public void Clear()
        {
            m_ControlPoints.Clear();
        }

        public int GetPointCount()
        {
            return m_ControlPoints.Count;
        }

        public void InsertPointAt(int index, Vector3 point)
        {
            if (!IsPositionValid(index, index, point))
                throw new ArgumentException(KErrorMessage);
            m_ControlPoints.Insert(index, new SplineControlPoint { position = point, height = 1.0f, cornerMode = Corner.Automatic });
        }

        public void RemovePointAt(int index)
        {
            if (m_ControlPoints.Count > 2)
                m_ControlPoints.RemoveAt(index);
        }

        public Vector3 GetPosition(int index)
        {
            return m_ControlPoints[index].position;
        }

        public void SetPosition(int index, Vector3 point)
        {
            if (!IsPositionValid(index, index + 1, point))
                throw new ArgumentException(KErrorMessage);
            SplineControlPoint newPoint = m_ControlPoints[index];
            newPoint.position = point;
            m_ControlPoints[index] = newPoint;
        }

        public Vector3 GetLeftTangent(int index)
        {
            ShapeTangentMode mode = GetTangentMode(index);

            if (mode == ShapeTangentMode.Linear)
                return Vector3.zero;

            return m_ControlPoints[index].leftTangent;
        }

        public void SetLeftTangent(int index, Vector3 tangent)
        {
            ShapeTangentMode mode = GetTangentMode(index);

            if (mode == ShapeTangentMode.Linear)
                return;

            SplineControlPoint newPoint = m_ControlPoints[index];
            newPoint.leftTangent = tangent;
            m_ControlPoints[index] = newPoint;
        }

        public Vector3 GetRightTangent(int index)
        {
            ShapeTangentMode mode = GetTangentMode(index);

            if (mode == ShapeTangentMode.Linear)
                return Vector3.zero;

            return m_ControlPoints[index].rightTangent;
        }

        public void SetRightTangent(int index, Vector3 tangent)
        {
            ShapeTangentMode mode = GetTangentMode(index);

            if (mode == ShapeTangentMode.Linear)
                return;

            SplineControlPoint newPoint = m_ControlPoints[index];
            newPoint.rightTangent = tangent;
            m_ControlPoints[index] = newPoint;
        }

        public ShapeTangentMode GetTangentMode(int index)
        {
            return m_ControlPoints[index].mode;
        }

        public void SetTangentMode(int index, ShapeTangentMode mode)
        {
            SplineControlPoint newPoint = m_ControlPoints[index];
            newPoint.mode = mode;
            m_ControlPoints[index] = newPoint;
        }

        public float GetHeight(int index)
        {
            return m_ControlPoints[index].height;
        }

        public void SetHeight(int index, float value)
        {
            m_ControlPoints[index].height = value;
        }

        public int GetSpriteIndex(int index)
        {
            return m_ControlPoints[index].spriteIndex;
        }

        public void SetSpriteIndex(int index, int value)
        {
            m_ControlPoints[index].spriteIndex = value;
        }

        public bool GetCorner(int index)
        {
            return GetCornerMode(index) != Corner.Disable;
        }

        public void SetCorner(int index, bool value)
        {
            m_ControlPoints[index].corner = value;
            m_ControlPoints[index].cornerMode = value ? Corner.Automatic : Corner.Disable;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = (int)2166136261;

                for (int i = 0; i < GetPointCount(); ++i)
                {
                    hashCode = hashCode * 16777619 ^ m_ControlPoints[i].GetHashCode();
                }

                hashCode = hashCode * 16777619 ^ m_IsOpenEnded.GetHashCode();

                return hashCode;
            }
        }
        
        internal void SetCornerMode(int index, Corner value)
        {
            m_ControlPoints[index].corner = (value != Corner.Disable);
            m_ControlPoints[index].cornerMode = value;
        }
        
        internal Corner GetCornerMode(int index)
        {
            if (m_ControlPoints[index].cornerMode == Corner.Disable)
            {
                // For backward compatibility.
                if (m_ControlPoints[index].corner)
                {
                    m_ControlPoints[index].cornerMode = Corner.Automatic;
                    return Corner.Automatic;
                }
            }
            return m_ControlPoints[index].cornerMode;
        }        
    }
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     ﻿using System;
using Unity.Collections;
using System.Runtime.InteropServices;
using Unity.Collections.LowLevel.Unsafe;

namespace UnityEngine.U2D
{
    public class SplineUtility
    {
        public static float SlopeAngle(Vector2 start, Vector2 end)
        {
            Vector2 dir = start - end;
            dir.Normalize();
            Vector2 dvup = new Vector2(0, 1f);
            Vector2 dvrt = new Vector2(1f, 0);

            float dr = Vector2.Dot(dir, dvrt);
            float du = Vector2.Dot(dir, dvup);
            float cu = Mathf.Acos(du);
            float sn = dr >= 0 ? 1.0f : -1.0f;
            float an = cu * Mathf.Rad2Deg * sn;

            // Adjust angles when direction is parallel to Up Axis.
            an = (du != 1f) ? an : 0;
            an = (du != -1f) ? an : -180f;
            return an;
        }
        public static void CalculateTangents(Vector3 point, Vector3 prevPoint, Vector3 nextPoint, Vector3 forward, float scale, out Vector3 rightTangent, out Vector3 leftTangent)
        {
            Vector3 v1 = (prevPoint - point).normalized;
            Vector3 v2 = (nextPoint - point).normalized;
            Vector3 v3 = v1 + v2;
            Vector3 cross = forward;

            if (prevPoint != nextPoint)
            {
                bool colinear = Mathf.Abs(v1.x * v2.y - v1.y * v2.x + v1.x * v2.z - v1.z * v2.x + v1.y * v2.z - v1.z * v2.y) < 0.01f;

                if (colinear)
                {
                    rightTangent = v2 * scale;
                    leftTangent = v1 * scale;
                    return;
                }

                cross = Vector3.Cross(v1, v2);
            }

            rightTangent = Vector3.Cross(cross, v3).normalized * scale;
            leftTangent = -rightTangent;
        }

        public static int NextIndex(int index, int pointCount)
        {
            return Mod(index + 1, pointCount);
        }

        public static int PreviousIndex(int index, int pointCount)
        {
            return Mod(index - 1, pointCount);
        }

        private static int Mod(int x, int m)
        {
            int r = x % m;
            return r < 0 ? r + m : r;
        }
    }

    // Copy utility.
    internal class SpriteShapeCopyUtility<T> where T : struct
    {
        internal static void Copy(NativeSlice<T> dst, T[] src, int length)
        {
            NativeSlice<T> dstSet = new NativeSlice<T>(dst, 0, length);
            dstSet.CopyFrom(src);
        }

        internal static void Copy(T[] dst, NativeSlice<T> src, int length)
        {
            NativeSlice<T> dstSet = new NativeSlice<T>(src, 0, length);
            dstSet.CopyTo(dst);
        }
    }
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                      ﻿using System;
using System.Collections.Generic;

namespace UnityEngine.U2D
{
    public enum ShapeTangentMode
    {
        Linear = 0,
        Continuous = 1,
        Broken = 2,
    };

    public enum CornerType
    {
        OuterTopLeft,
        OuterTopRight,
        OuterBottomLeft,
        OuterBottomRight,
        InnerTopLeft,
        InnerTopRight,
        InnerBottomLeft,
        InnerBottomRight,
    };

    public enum QualityDetail
    {
        High = 16,
        Mid = 8,
        Low = 4
    }

    public enum Corner
    {
        Disable = 0,
        Automatic = 1,
        Stretched = 2,
    }

    [System.Serializable]
    public class SplineControlPoint
    {
        public Vector3 position;
        public Vector3 leftTangent;
        public Vector3 rightTangent;
        public ShapeTangentMode mode;
        public float height = 1f;
        public float bevelCutoff;
        public float bevelSize;
        public int spriteIndex;
        public bool corner;
        [SerializeField]
        Corner m_CornerMode;

        public Corner cornerMode
        {
            get => m_CornerMode;
            set => m_CornerMode = value;
        }

        public override int GetHashCode()
        {
            return  ((int)position.x).GetHashCode() ^ ((int)position.y).GetHashCode() ^ position.GetHashCode() ^
                    (leftTangent.GetHashCode() << 2) ^ (rightTangent.GetHashCode() >> 2) ^  ((int)mode).GetHashCode() ^
                    height.GetHashCode() ^ spriteIndex.GetHashCode() ^ corner.GetHashCode() ^ (m_CornerMode.GetHashCode() << 2);
        }
    }

    [System.Serializable]
    public class AngleRange : ICloneable
    {
        public float start
        {
            get { return m_Start; }
            set { m_Start = value; }
        }

        public float end
        {
            get { return m_End; }
            set { m_End = value; }
        }

        public int order
        {
            get { return m_Order; }
            set { m_Order = value; }
        }

        public List<Sprite> sprites
        {
            get { return m_Sprites; }
            set { m_Sprites = value; }
        }

        [SerializeField]
        float m_Start;
        [SerializeField]
        float m_End;
        [SerializeField]
        int m_Order;
        [SerializeField]
        List<Sprite> m_Sprites = new List<Sprite>();

        public object Clone()
        {
            AngleRange clone = this.MemberwiseClone() as AngleRange;
            clone.sprites = new List<Sprite>(clone.sprites);

            return clone;
        }

        public override bool Equals(object obj)
        {
            var other = obj as AngleRange;

            if (other == null)
                return false;

            bool equals = start.Equals(other.start) && end.Equals(other.end) && order.Equals(other.order);

            if (!equals)
                return false;

            if (sprites.Count != other.sprites.Count)
                return false;

            for (int i = 0; i < sprites.Count; ++i)
                if (sprites[i] != other.sprites[i])
                    return false;

            return true;
        }

        public override int GetHashCode()
        {
            int hashCode = start.GetHashCode() ^ end.GetHashCode() ^ order.GetHashCode();

            if (sprites != null)
            {
                for (int i = 0; i < sprites.Count; i++)
                {
                    Sprite sprite = sprites[i];
                    if (sprite)
                        hashCode = hashCode * 16777619 ^ (sprite.GetHashCode() + i);
                }
            }

            return hashCode;
        }
    }

    [System.Serializable]
    public class CornerSprite : ICloneable
    {
        public CornerType cornerType
        {
            get { return m_CornerType; }
            set { m_CornerType = value; }
        }

        public List<Sprite> sprites
        {
            get { return m_Sprites; }
            set { m_Sprites = value; }
        }

        [SerializeField]
        CornerType m_CornerType;               ///< Set Corner type. enum { OuterTopLeft = 0, OuterTopRight = 1, OuterBottomLeft = 2, OuterBottomRight = 3, InnerTopLeft = 4, InnerTopRight = 5, InnerBottomLeft = 6, InnerBottomRight = 7 }
        [SerializeField]
        List<Sprite> m_Sprites;

        public object Clone()
        {
            CornerSprite clone = this.MemberwiseClone() as CornerSprite;
            clone.sprites = new List<Sprite>(clone.sprites);

            return clone;
        }

        public override bool Equals(object obj)
        {
            var other = obj as CornerSprite;

            if (other == null)
                return false;

            if (!cornerType.Equals(other.cornerType))
                return false;

            if (sprites.Count != other.sprites.Count)
                return false;

            for (int i = 0; i < sprites.Count; ++i)
                if (sprites[i] != other.sprites[i])
                    return false;

            return true;
        }

        public override int GetHashCode()
        {
            int hashCode = cornerType.GetHashCode();

            if (sprites != null)
            {
                for (int i = 0; i < sprites.Count; i++)
                {
                    Sprite sprite = sprites[i];
                    if (sprite)
                    {
                        hashCode ^= (i + 1);
                        hashCode ^= sprite.GetHashCode();
                    }
                }
            }

            return hashCode;
        }
    }

    [HelpURLAttribute("https://docs.unity3d.com/Packages/com.unity.2d.spriteshape@latest/index.html?subfolder=/manual/SSProfile.html")]
    public class SpriteShape : ScriptableObject
    {
        public List<AngleRange> angleRanges
        {
            get { return m_Angles; }
            set { m_Angles = value; }
        }

        public Texture2D fillTexture
        {
            get { return m_FillTexture; }
            set { m_FillTexture = value; }
        }

        public List<CornerSprite> cornerSprites
        {
            get { return m_CornerSprites; }
            set { m_CornerSprites = value; }
        }

        public float fillOffset
        {
            get { return m_FillOffset; }
            set { m_FillOffset = value; }
        }

        public bool useSpriteBorders
        {
            get { return m_UseSpriteBorders; }
            set { m_UseSpriteBorders = value; }
        }

        [SerializeField]
        List<AngleRange> m_Angles = new List<AngleRange>();
        [SerializeField]
        Texture2D m_FillTexture;
        [SerializeField]
        List<CornerSprite> m_CornerSprites = new List<CornerSprite>();
        [SerializeField]
        float m_FillOffset;

        [SerializeField]
        bool m_UseSpriteBorders = true;

        private CornerSprite GetCornerSprite(CornerType cornerType)
        {
            var cornerSprite = new CornerSprite();
            cornerSprite.cornerType = cornerType;
            cornerSprite.sprites = new List<Sprite>();
            cornerSprite.sprites.Insert(0, null);
            return cornerSprite;
        }

        void ResetCornerList()
        {
            m_CornerSprites.Clear();
            m_CornerSprites.Insert(0, GetCornerSprite(CornerType.OuterTopLeft));
            m_CornerSprites.Insert(1, GetCornerSprite(CornerType.OuterTopRight));
            m_CornerSprites.Insert(2, GetCornerSprite(CornerType.OuterBottomLeft));
            m_CornerSprites.Insert(3, GetCornerSprite(CornerType.OuterBottomRight));
            m_CornerSprites.Insert(4, GetCornerSprite(CornerType.InnerTopLeft));
            m_CornerSprites.Insert(5, GetCornerSprite(CornerType.InnerTopRight));
            m_CornerSprites.Insert(6, GetCornerSprite(CornerType.InnerBottomLeft));
            m_CornerSprites.Insert(7, GetCornerSprite(CornerType.InnerBottomRight));
        }

        void OnValidate()
        {
            if (m_CornerSprites.Count != 8)
                ResetCornerList();
        }

        void Reset()
        {
            m_Angles.Clear();
            ResetCornerList();
        }

        internal static int GetSpriteShapeHashCode(SpriteShape spriteShape)
        {
            // useSpriteBorders, fillOffset and fillTexture are hashChecked elsewhere.

            unchecked
            {
                int hashCode = (int)2166136261;

                hashCode = hashCode * 16777619 ^ spriteShape.angleRanges.Count;

                for (int i = 0; i < spriteShape.angleRanges.Count; ++i)
                {
                    hashCode = hashCode * 16777619 ^ (spriteShape.angleRanges[i].GetHashCode() + i);
                }

                hashCode = hashCode * 16777619 ^ spriteShape.cornerSprites.Count;

                for (int i = 0; i < spriteShape.cornerSprites.Count; ++i)
                {
                    hashCode = hashCode * 16777619 ^ (spriteShape.cornerSprites[i].GetHashCode() + i);
                }

                return hashCode;
            }
        }

    }
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        using System.Collections.Generic;
using Unity.Jobs;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Collections.LowLevel.Unsafe;
#if UNITY_EDITOR
using UnityEditor.U2D;
#endif

namespace UnityEngine.U2D
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(SpriteShapeRenderer))]
    [DisallowMultipleComponent]
    [HelpURLAttribute("https://docs.unity3d.com/Packages/com.unity.2d.spriteshape@latest/index.html?subfolder=/manual/SSController.html")]
    public class SpriteShapeController : MonoBehaviour
    {
        // Internal Dataset.
        const float s_DistanceTolerance = 0.001f;

        // Cached Objects.
        SpriteShape m_ActiveSpriteShape;
        EdgeCollider2D m_EdgeCollider2D;
        PolygonCollider2D m_PolygonCollider2D;
        SpriteShapeRenderer m_SpriteShapeRenderer;
        SpriteShapeGeometryCache m_SpriteShapeGeometryCache;

        Sprite[] m_SpriteArray = new Sprite[0];
        Sprite[] m_EdgeSpriteArray = new Sprite[0];
        Sprite[] m_CornerSpriteArray = new Sprite[0];
        AngleRangeInfo[] m_AngleRangeInfoArray = new AngleRangeInfo[0];

        // Required for Generation.
        NativeArray<float2> m_ColliderData;
        NativeArray<Vector4> m_TangentData;

        // Renderer Stuff.
        bool m_DynamicOcclusionLocal;
        bool m_DynamicOcclusionOverriden;

        // Hash Check.
        int m_ActiveSplineHash = 0;
        int m_ActiveSpriteShapeHash = 0;
        JobHandle m_JobHandle;
        SpriteShapeParameters m_ActiveShapeParameters;

        // Serialized Data.
        [SerializeField]
        Spline m_Spline = new Spline();
        [SerializeField]
        SpriteShape m_SpriteShape;

        [SerializeField]
        float m_FillPixelPerUnit = 100.0f;
        [SerializeField]
        float m_StretchTiling = 1.0f;
        [SerializeField]
        int m_SplineDetail;
        [SerializeField]
        bool m_AdaptiveUV;
        [SerializeField]
        bool m_StretchUV;
        [SerializeField]
        bool m_WorldSpaceUV;
        [SerializeField]
        float m_CornerAngleThreshold = 30.0f;
        [SerializeField]
        int m_ColliderDetail;
        [SerializeField, Range(-0.5f, 0.5f)]
        float m_ColliderOffset;
        [SerializeField]
        bool m_UpdateCollider = true;
        [SerializeField]
        bool m_OptimizeCollider = true;
        [SerializeField]
        bool m_OptimizeGeometry = true;
        [SerializeField]
        bool m_EnableTangents = false;
        [SerializeField]
        [HideInInspector]
        bool m_GeometryCached = false;
        [SerializeField] 
        bool m_UTess2D = false;
        
        #region GetSet

        internal bool geometryCached
        {
            get { return m_GeometryCached; }
            set { m_GeometryCached = value; }
        }

        internal int splineHashCode
        {
            get { return m_ActiveSplineHash; }
        }

        internal Sprite[] spriteArray
        {
            get { return m_SpriteArray; }
        }

        internal SpriteShapeParameters spriteShapeParameters
        {
            get { return m_ActiveShapeParameters; }
        }

        internal SpriteShapeGeometryCache spriteShapeGeometryCache
        {
            get
            {
                if (!m_SpriteShapeGeometryCache)
                    m_SpriteShapeGeometryCache = GetComponent<SpriteShapeGeometryCache>();
                return m_SpriteShapeGeometryCache;
            }
        }

        public int spriteShapeHashCode
        {
            get { return m_ActiveSpriteShapeHash; }
        }

        public bool worldSpaceUVs
        {
            get { return m_WorldSpaceUV; }
            set { m_WorldSpaceUV = value; }
        }

        public float fillPixelsPerUnit
        {
            get { return m_FillPixelPerUnit; }
            set { m_FillPixelPerUnit = value; }
        }

        public bool enableTangents
        {
            get { return m_EnableTangents; }
            set { m_EnableTangents = value; }
        }

        public float stretchTiling
        {
            get { return m_StretchTiling; }
            set { m_StretchTiling = value; }
        }

        public int splineDetail
        {
            get { return m_SplineDetail; }
            set { m_SplineDetail = Mathf.Max(0, value); }
        }

        public int colliderDetail
        {
            get { return m_ColliderDetail; }
            set { m_ColliderDetail = Mathf.Max(0, value); }
        }

        public float colliderOffset
        {
            get { return m_ColliderOffset; }
            set { m_ColliderOffset = value; }
        }

        public float cornerAngleThreshold
        {
            get { return m_CornerAngleThreshold; }
            set { m_CornerAngleThreshold = value; }
        }

        public bool autoUpdateCollider
        {
            get { return m_UpdateCollider; }
            set { m_UpdateCollider = value; }
        }

        public bool optimizeCollider
        {
            get { return m_OptimizeCollider; }
        }

        public bool optimizeGeometry
        {
            get { return m_OptimizeGeometry; }
        }

        public bool hasCollider
        {
            get { return (edgeCollider != null) || (polygonCollider != null); }
        }

        public Spline spline
        {
            get { return m_Spline; }
        }

        public SpriteShape spriteShape
        {
            get { return m_SpriteShape; }
            set { m_SpriteShape = value; }
        }

        public EdgeCollider2D edgeCollider
        {
            get
            {
                if (!m_EdgeCollider2D)
                    m_EdgeCollider2D = GetComponent<EdgeCollider2D>();
                return m_EdgeCollider2D;
            }
        }

        public PolygonCollider2D polygonCollider
        {
            get
            {
                if (!m_PolygonCollider2D)
                    m_PolygonCollider2D = GetComponent<PolygonCollider2D>();
                return m_PolygonCollider2D;
            }
        }

        public SpriteShapeRenderer spriteShapeRenderer
        {
            get
            {
                if (!m_SpriteShapeRenderer)
                    m_SpriteShapeRenderer = GetComponent<SpriteShapeRenderer>();
                return m_SpriteShapeRenderer;
            }
        }

        #endregion

        #region EventHandles.

        void DisposeInternal()
        {
            if (m_ColliderData.IsCreated)
                m_ColliderData.Dispose();
            if (m_TangentData.IsCreated)
                m_TangentData.Dispose();
        }

        void OnApplicationQuit()
        {
            DisposeInternal();
        }

        void OnEnable()
        {
            spriteShapeRenderer.enabled = true;
            m_DynamicOcclusionOverriden = true;
            m_DynamicOcclusionLocal = spriteShapeRenderer.allowOcclusionWhenDynamic;
            spriteShapeRenderer.allowOcclusionWhenDynamic = false;
            UpdateSpriteData();
        }

        void OnDisable()
        {
            spriteShapeRenderer.enabled = false;
            DisposeInternal();
        }

        void OnDestroy()
        {

        }

        void Reset()
        {
            m_SplineDetail = (int)QualityDetail.High;
            m_AdaptiveUV = true;
            m_StretchUV = false;
            m_FillPixelPerUnit = 100f;

            m_ColliderDetail = (int)QualityDetail.High;
            m_StretchTiling = 1.0f;
            m_WorldSpaceUV = false;
            m_CornerAngleThreshold = 30.0f;
            m_ColliderOffset = 0;
            m_UpdateCollider = true;
            m_OptimizeCollider = true;
            m_OptimizeGeometry = true;
            m_EnableTangents = false;

            spline.Clear();
            spline.InsertPointAt(0, Vector2.left + Vector2.down);
            spline.InsertPointAt(1, Vector2.left + Vector2.up);
            spline.InsertPointAt(2, Vector2.right + Vector2.up);
            spline.InsertPointAt(3, Vector2.right + Vector2.down);
        }

        static void SmartDestroy(UnityEngine.Object o)
        {
            if (o == null)
                return;

#if UNITY_EDITOR
            if (!Application.isPlaying)
                DestroyImmediate(o);
            else
#endif
                Destroy(o);
        }

        #endregion

        #region HashAndDataCheck

        public void RefreshSpriteShape()
        {
            m_ActiveSplineHash = 0;
        }

        // Ensure Neighbor points are not too close to each other.
        bool ValidateSpline()
        {
            int pointCount = spline.GetPointCount();
            if (pointCount < 2)
                return false;
            for (int i = 0; i < pointCount - 1; ++i)
            {
                var vec = spline.GetPosition(i) - spline.GetPosition(i + 1);
                if (vec.sqrMagnitude < s_DistanceTolerance)
                {
                    Debug.LogWarningFormat(gameObject, "[SpriteShape] Control points {0} & {1} are too close. SpriteShape will not be generated for < {2} >.", i, i + 1, gameObject.name);
                    return false;
                }
            }
            return true;
        }

        // Ensure SpriteShape is valid if not
        bool ValidateSpriteShapeTexture()
        {
            bool valid = false;
            
            // Check if SpriteShape Profile is valid.
            if (spriteShape != null)
            {
                // Open ended and no valid Sprites set. Check if it has a valid fill texture.
                if (!spline.isOpenEnded)
                {
                    valid = (spriteShape.fillTexture != null);
                }
            }
            else
            {
                // Warn that no SpriteShape is set.
                Debug.LogWarningFormat(gameObject, "[SpriteShape] A valid SpriteShape profile has not been set for gameObject < {0} >.", gameObject.name);
            }
#if UNITY_EDITOR
            // We allow null SpriteShape for rapid prototyping in Editor.
            valid = true;
#endif
            return valid;
        }

        bool HasSpriteShapeChanged()
        {
            bool changed = (m_ActiveSpriteShape != spriteShape);
            if (changed)
                m_ActiveSpriteShape = spriteShape;
            return changed;
        }

        bool HasSpriteShapeDataChanged()
        {
            bool updateSprites = HasSpriteShapeChanged();
            if (spriteShape)
            {
                var hashCode = SpriteShape.GetSpriteShapeHashCode(spriteShape);
                if (spriteShapeHashCode != hashCode)
                {
                    m_ActiveSpriteShapeHash = hashCode;
                    updateSprites = true;
                }
            }
            return updateSprites;
        }

        bool HasSplineDataChanged()
        {
            unchecked
            {
                // Spline.
                int hashCode = (int)2166136261 ^ spline.GetHashCode();

                // Local Stuff.
                hashCode = hashCode * 16777619 ^ (m_UTess2D ? 1 : 0);
                hashCode = hashCode * 16777619 ^ (m_WorldSpaceUV ? 1 : 0); 
                hashCode = hashCode * 16777619 ^ (m_EnableTangents ? 1 : 0);
                hashCode = hashCode * 16777619 ^ (m_GeometryCached ? 1 : 0);
                hashCode = hashCode * 16777619 ^ (m_OptimizeGeometry ? 1 : 0);
                hashCode = hashCode * 16777619 ^ (m_StretchTiling.GetHashCode());
                hashCode = hashCode * 16777619 ^ (m_ColliderOffset.GetHashCode());
                hashCode = hashCode * 16777619 ^ (m_ColliderDetail.GetHashCode());

                if (splineHashCode != hashCode)
                {
                    m_ActiveSplineHash = hashCode;
                    return true;
                }
            }
            return false;
        }

        void LateUpdate()
        {
            BakeCollider();
        }

        void OnWillRenderObject()
        {
            BakeMesh();
        }

        public JobHandle BakeMesh()
        {
            JobHandle jobHandle = default;

#if !UNITY_EDITOR            
            if (spriteShapeGeometryCache)
            {
                // If SpriteShapeGeometry has already been uploaded, don't bother checking further.
                if (0 != m_ActiveSplineHash && 0 != spriteShapeGeometryCache.maxArrayCount)
                    return jobHandle;
            }
#endif                

            bool valid = ValidateSpline();

            if (valid)
            {
                bool splineChanged = HasSplineDataChanged();
                bool spriteShapeChanged = HasSpriteShapeDataChanged();
                bool spriteShapeParametersChanged = UpdateSpriteShapeParameters();

                if (spriteShapeChanged)
                {
                    UpdateSpriteData();
                }

                if (splineChanged || spriteShapeChanged || spriteShapeParametersChanged)
                {
                    jobHandle = ScheduleBake();
                }
            }
            return jobHandle;
        }

#endregion

#region UpdateData

        public bool UpdateSpriteShapeParameters()
        {
            bool carpet = !spline.isOpenEnded;
            bool smartSprite = true;
            bool adaptiveUV = m_AdaptiveUV;
            bool stretchUV = m_StretchUV;
            bool spriteBorders = false;
            uint fillScale = 0;
            uint splineDetail = (uint)m_SplineDetail;
            float borderPivot = 0f;
            float angleThreshold = (m_CornerAngleThreshold >= 0 && m_CornerAngleThreshold < 90) ? m_CornerAngleThreshold : 89.9999f;
            Texture2D fillTexture = null;
            Matrix4x4 transformMatrix = Matrix4x4.identity;

            if (spriteShape)
            {
                if (worldSpaceUVs)
                    transformMatrix = transform.localToWorldMatrix;

                fillTexture = spriteShape.fillTexture;
                fillScale = stretchUV ? (uint)stretchTiling : (uint)fillPixelsPerUnit;
                borderPivot = spriteShape.fillOffset;
                spriteBorders = spriteShape.useSpriteBorders;
                // If Corners are enabled, set smart-sprite to false.
                if (spriteShape.cornerSprites.Count > 0)
                    smartSprite = false;
            }
            else
            {
#if UNITY_EDITOR
                fillTexture = UnityEditor.EditorGUIUtility.whiteTexture;
                fillScale = 100;
#endif
            }

            bool changed = m_ActiveShapeParameters.adaptiveUV != adaptiveUV ||
                m_ActiveShapeParameters.angleThreshold != angleThreshold ||
                m_ActiveShapeParameters.borderPivot != borderPivot ||
                m_ActiveShapeParameters.carpet != carpet ||
                m_ActiveShapeParameters.fillScale != fillScale ||
                m_ActiveShapeParameters.fillTexture != fillTexture ||
                m_ActiveShapeParameters.smartSprite != smartSprite ||
                m_ActiveShapeParameters.splineDetail != splineDetail ||
                m_ActiveShapeParameters.spriteBorders != spriteBorders ||
                m_ActiveShapeParameters.transform != transformMatrix ||
                m_ActiveShapeParameters.stretchUV != stretchUV;

            m_ActiveShapeParameters.adaptiveUV = adaptiveUV;
            m_ActiveShapeParameters.stretchUV = stretchUV;
            m_ActiveShapeParameters.angleThreshold = angleThreshold;
            m_ActiveShapeParameters.borderPivot = borderPivot;
            m_ActiveShapeParameters.carpet = carpet;
            m_ActiveShapeParameters.fillScale = fillScale;
            m_ActiveShapeParameters.fillTexture = fillTexture;
            m_ActiveShapeParameters.smartSprite = smartSprite;
            m_ActiveShapeParameters.splineDetail = splineDetail;
            m_ActiveShapeParameters.spriteBorders = spriteBorders;
            m_ActiveShapeParameters.transform = transformMatrix;

            return changed;
        }

        void UpdateSpriteData()
        {
            if (spriteShape)
            {
                List<Sprite> edgeSpriteList = new List<Sprite>();
                List<Sprite> cornerSpriteList = new List<Sprite>();
                List<AngleRangeInfo> angleRangeInfoList = new List<AngleRangeInfo>();

                List<AngleRange> sortedAngleRanges = new List<AngleRange>(spriteShape.angleRanges);
                sortedAngleRanges.Sort((a, b) => a.order.CompareTo(b.order));

                for (int i = 0; i < sortedAngleRanges.Count; i++)
                {
                    bool validSpritesFound = false;
                    AngleRange angleRange = sortedAngleRanges[i];
                    foreach (Sprite edgeSprite in angleRange.sprites)
                    {
                        if (edgeSprite != null)
                        {
                            validSpritesFound = true;
                            break;
                        }
                    }

                    if (validSpritesFound)
                    {
                        AngleRangeInfo angleRangeInfo = new AngleRangeInfo();
                        angleRangeInfo.start = angleRange.start;
                        angleRangeInfo.end = angleRange.end;
                        angleRangeInfo.order = (uint)angleRange.order;
                        List<int> spriteIndices = new List<int>();
                        foreach (Sprite edgeSprite in angleRange.sprites)
                        {
                            edgeSpriteList.Add(edgeSprite);
                            spriteIndices.Add(edgeSpriteList.Count - 1);
                        }
                        angleRangeInfo.sprites = spriteIndices.ToArray();
                        angleRangeInfoList.Add(angleRangeInfo);
                    }
                }

                bool validCornerSpritesFound = false;
                foreach (CornerSprite cornerSprite in spriteShape.cornerSprites)
                {
                    if (cornerSprite.sprites[0] != null)
                    {
                        validCornerSpritesFound = true;
                        break;
                    }
                }

                if (validCornerSpritesFound)
                {
                    for (int i = 0; i < spriteShape.cornerSprites.Count; i++)
                    {
                        CornerSprite cornerSprite = spriteShape.cornerSprites[i];
                        cornerSpriteList.Add(cornerSprite.sprites[0]);
                    }
                }

                m_EdgeSpriteArray = edgeSpriteList.ToArray();
                m_CornerSpriteArray = cornerSpriteList.ToArray();
                m_AngleRangeInfoArray = angleRangeInfoList.ToArray();

                List<Sprite> spriteList = new List<Sprite>();
                spriteList.AddRange(m_EdgeSpriteArray);
                spriteList.AddRange(m_CornerSpriteArray);
                m_SpriteArray = spriteList.ToArray();
            }
            else
            {
                m_SpriteArray = new Sprite[0];
                m_EdgeSpriteArray = new Sprite[0];
                m_CornerSpriteArray = new Sprite[0];
                m_AngleRangeInfoArray = new AngleRangeInfo[0];
            }
        }

        NativeArray<ShapeControlPoint> GetShapeControlPoints()
        {
            int pointCount = spline.GetPointCount();
            NativeArray<ShapeControlPoint> shapePoints = new NativeArray<ShapeControlPoint>(pointCount, Allocator.Temp);
            for (int i = 0; i < pointCount; ++i)
            {
                ShapeControlPoint shapeControlPoint;
                shapeControlPoint.position = spline.GetPosition(i);
                shapeControlPoint.leftTangent = spline.GetLeftTangent(i);
                shapeControlPoint.rightTangent = spline.GetRightTangent(i);
                shapeControlPoint.mode = (int)spline.GetTangentMode(i);
                shapePoints[i] = shapeControlPoint;
            }
            return shapePoints;
        }

        NativeArray<SplinePointMetaData> GetSplinePointMetaData()
        {
            int pointCount = spline.GetPointCount();
            NativeArray<SplinePointMetaData> shapeMetaData = new NativeArray<SplinePointMetaData>(pointCount, Allocator.Temp);
            for (int i = 0; i < pointCount; ++i)
            {                
                SplinePointMetaData metaData;
                metaData.height = m_Spline.GetHeight(i);
                metaData.spriteIndex = (uint)m_Spline.GetSpriteIndex(i);
                metaData.cornerMode = (int)m_Spline.GetCornerMode(i);
                shapeMetaData[i] = metaData;
            }
            return shapeMetaData;
        }

        int CalculateMaxArrayCount(NativeArray<ShapeControlPoint> shapePoints)
        {
            bool hasSprites = false;
            float smallestWidth = 99999.0f;

            if (null != spriteArray)
            {
                foreach (var sprite in m_SpriteArray)
                {
                    if (sprite != null)
                    {
                        hasSprites = true;
                        float pixelWidth = BezierUtility.GetSpritePixelWidth(sprite);
                        smallestWidth = (smallestWidth > pixelWidth) ? pixelWidth : smallestWidth;
                    }
                }
            }

            // Approximate vertex Array Count.
            float shapeLength = BezierUtility.BezierLength(shapePoints, splineDetail * splineDetail) * 2.0f;
            int adjustWidth = hasSprites ? ((int)(shapeLength / smallestWidth) * 6) + (shapePoints.Length * 6 * splineDetail) : 0;
            int adjustShape = shapePoints.Length * 4 * splineDetail;
            adjustShape = optimizeGeometry ? (adjustShape) : (adjustShape * 2);
            var validFT = ValidateSpriteShapeTexture();
#if !UNITY_EDITOR
                adjustShape = validFT ? adjustShape : 0;
#endif
            int maxArrayCount = adjustShape + adjustWidth;
            return maxArrayCount;
        }

#endregion

        unsafe JobHandle ScheduleBake()
        {
            JobHandle jobHandle = default;

            bool staticUpload = Application.isPlaying;
#if !UNITY_EDITOR
            staticUpload = true;
#endif
            if (staticUpload && geometryCached)
            {
                if (spriteShapeGeometryCache)
                    if (spriteShapeGeometryCache.maxArrayCount != 0)
                        return spriteShapeGeometryCache.Upload(spriteShapeRenderer, this);
            }

            int pointCount = spline.GetPointCount();
            NativeArray<ShapeControlPoint> shapePoints = GetShapeControlPoints();
            NativeArray<SplinePointMetaData> shapeMetaData = GetSplinePointMetaData();
            int maxArrayCount = CalculateMaxArrayCount(shapePoints);

            if (maxArrayCount > 0 && enabled)
            {
                // Collider Data
                if (m_ColliderData.IsCreated)
                    m_ColliderData.Dispose();
                m_ColliderData = new NativeArray<float2>(maxArrayCount, Allocator.Persistent);

                // Tangent Data
                if (!m_TangentData.IsCreated)
                    m_TangentData = new NativeArray<Vector4>(1, Allocator.Persistent);

                NativeArray<ushort> indexArray;
                NativeSlice<Vector3> posArray;
                NativeSlice<Vector2> uv0Array;
                NativeArray<Bounds> bounds = spriteShapeRenderer.GetBounds();
                NativeArray<SpriteShapeSegment> geomArray = spriteShapeRenderer.GetSegments(shapePoints.Length * 8);
                NativeSlice<Vector4> tanArray = new NativeSlice<Vector4>(m_TangentData);

                if (m_EnableTangents)
                { 
                    spriteShapeRenderer.GetChannels(maxArrayCount, out indexArray, out posArray, out uv0Array, out tanArray);
                }
                else
                {
                    spriteShapeRenderer.GetChannels(maxArrayCount, out indexArray, out posArray, out uv0Array);
                }

                var spriteShapeJob = new SpriteShapeGenerator() { m_Bounds = bounds, m_PosArray = posArray, m_Uv0Array = uv0Array, m_TanArray = tanArray, m_GeomArray = geomArray, m_IndexArray = indexArray, m_ColliderPoints = m_ColliderData };
                spriteShapeJob.Prepare(this, m_ActiveShapeParameters, maxArrayCount, shapePoints, shapeMetaData, m_AngleRangeInfoArray, m_EdgeSpriteArray, m_CornerSpriteArray, m_UTess2D);
                // Only update Handle for an actual Job is scheduled.
                m_JobHandle = spriteShapeJob.Schedule();
                spriteShapeRenderer.Prepare(m_JobHandle, m_ActiveShapeParameters, m_SpriteArray);
                jobHandle = m_JobHandle;

#if UNITY_EDITOR
                if (spriteShapeGeometryCache && geometryCached)
                    spriteShapeGeometryCache.SetGeometryCache(maxArrayCount, posArray, uv0Array, tanArray, indexArray, geomArray);
#endif

                JobHandle.ScheduleBatchedJobs();
            }

            if (m_DynamicOcclusionOverriden)
            {
                spriteShapeRenderer.allowOcclusionWhenDynamic = m_DynamicOcclusionLocal;
                m_DynamicOcclusionOverriden = false;
            }
            shapePoints.Dispose();
            shapeMetaData.Dispose();
            return jobHandle;
        }

        public void BakeCollider()
        {
            // Previously this must be explicitly called if using BakeMesh.
            // But now we do it internally. BakeCollider_CanBeCalledMultipleTimesWithoutJobComplete
            m_JobHandle.Complete();
            
            if (m_ColliderData.IsCreated)
            {
                if (autoUpdateCollider)
                {
                    if (hasCollider)
                    {
                        int maxCount = short.MaxValue - 1;
                        float2 last = (float2)0;
                        List<Vector2> m_ColliderSegment = new List<Vector2>();
                        for (int i = 0; i < maxCount; ++i)
                        {
                            float2 now = m_ColliderData[i];
                            if (!math.any(last) && !math.any(now))
                                break;
                            m_ColliderSegment.Add(new Vector2(now.x, now.y));
                        }

                        if (edgeCollider != null)
                            edgeCollider.points = m_ColliderSegment.ToArray();
                        if (polygonCollider != null)
                            polygonCollider.points = m_ColliderSegment.ToArray();
                    }
                }
                // Dispose Collider as its no longer needed.
                m_ColliderData.Dispose();
#if UNITY_EDITOR
                if (UnityEditor.SceneView.lastActiveSceneView != null)
                    UnityEditor.SceneView.lastActiveSceneView.Repaint();
#endif
            }
        }

        internal void BakeMeshForced()
        {
            if (spriteShapeRenderer != null)
            {
                var hasSplineChanged = HasSplineDataChanged();
                if (!spriteShapeRenderer.isVisible && hasSplineChanged)
                {
                    BakeMesh();
                    Rendering.CommandBuffer rc = new Rendering.CommandBuffer();
                    rc.GetTemporaryRT(0, 256, 256, 0);
                    rc.SetRenderTarget(0);
                    rc.DrawRenderer(spriteShapeRenderer, spriteShapeRenderer.sharedMaterial);
                    rc.ReleaseTemporaryRT(0);
                    Graphics.ExecuteCommandBuffer(rc);
                }
            }
        }

        Texture2D GetTextureFromIndex(int index)
        {
            if (index == 0)
                return spriteShape ? spriteShape.fillTexture : null;

            --index;
            if (index < m_EdgeSpriteArray.Length)
                return GetSpriteTexture(m_EdgeSpriteArray[index]);

            index -= m_EdgeSpriteArray.Length;
            return GetSpriteTexture(m_CornerSpriteArray[index]);
        }

        Texture2D GetSpriteTexture(Sprite sprite)
        {
            if (sprite)
            {
#if UNITY_EDITOR
                return UnityEditor.Sprites.SpriteUtility.GetSpriteTexture(sprite, sprite.packed);
#else
                return sprite.texture;
#endif
            }

            return null;
        }
    }
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           using System;
using System.Linq;
using Unity.Jobs;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine.U2D.UTess;
using Unity.Collections.LowLevel.Unsafe;
using Unity.SpriteShape.External.LibTessDotNet;

// We will enable this once Burst gets a verified final version as this attribute keeps changing.
#if ENABLE_SPRITESHAPE_BURST
using Unity.Burst;
#endif

namespace UnityEngine.U2D
{

#if ENABLE_SPRITESHAPE_BURST
    [BurstCompile]
#endif
    public struct SpriteShapeGenerator : IJob
    {

        struct JobParameters
        {
            public int4 shapeData;              // x : ClosedShape (bool) y : AdaptiveUV (bool) z : SpriteBorders (bool) w : Enable Fill Texture.
            public int4 splineData;             // x : StrtechUV. y : splineDetail z : _unused_ w: Collider On/Off
            public float4 curveData;            // x : ColliderPivot y : BorderPivot z : AngleThreshold w : _unused_.
            public float4 fillData;             // x : fillScale  y : fillScale.x W z : fillScale.y H w: 0.
        }

        struct JobSpriteInfo
        {
            public float4 texRect;              // TextureRect.
            public float4 texData;              // x : GPUWidth y : GPUHeight z : TexelWidth w : TexelHeight
            public float4 uvInfo;               // x : x, y : y, z : width, w : height
            public float4 metaInfo;             // x : PPU, y : Pivot Y z : Original Rect Width w : Original Rect Height.
            public float4 border;               // Sprite Border.
        }

        struct JobAngleRange
        {
            public float4 spriteAngles;         // x, y | First Angle & z,w | Second Angle.
            public int4 spriteData;             // Additional Data. x : sorting Order. y : variant Count. z : render Order Max.
        };

        struct JobControlPoint
        {
            public int4 cpData;                 // x : Sprite Index y : Corner Type z : Mode w : Internal Sprite Index.
            public int4 exData;                 // x : Corner Type y: Corner Sprite z : Corner 0(disabled), 1 (stretch), (2, 3)(corner start/end)
            public float2 cpInfo;               // x : Height y : Render Order.
            public float2 position;
            public float2 tangentLt;
            public float2 tangentRt;
        };

        struct JobContourPoint
        {
            public float2 position;             // x, y Position.  
            public float2 ptData;               // x : height. y :source cp.
        }

        struct JobIntersectPoint
        {
            public float2 top;
            public float2 bottom;
        }

        // Tessellation Structures.      
        struct JobSegmentInfo
        {
            public int4 sgInfo;                 // x : Begin y : End. z : Sprite w : First Sprite for that Angle Range.
            public float4 spriteInfo;           // x : width y : height z : Render Order. w : 0 (no) 1 (left stretchy) 2(right)
        };

        struct JobCornerInfo
        {
            public float2 bottom;
            public float2 top;
            public float2 left;
            public float2 right;
            public int2 cornerData;
        }

        struct JobShapeVertex
        {
            public float2 pos;
            public float2 uv;
            public float4 tan;
            public float2 meta;                 // x : height y : -
            public int2 sprite;                 // x : sprite y : is main Point.
        }

        /// <summary>
        /// Native Arrays : Scope : Initialized before and ReadOnly During Job 
        /// </summary>
        [ReadOnly]
        private JobParameters m_ShapeParams;

        [ReadOnly]
        [DeallocateOnJobCompletion]
        private NativeArray<JobSpriteInfo> m_SpriteInfos;

        [ReadOnly]
        [DeallocateOnJobCompletion]
        private NativeArray<JobSpriteInfo> m_CornerSpriteInfos;

        [ReadOnly]
        [DeallocateOnJobCompletion]
        private NativeArray<JobAngleRange> m_AngleRanges;

        /// <summary>
        /// Native Arrays : Scope : Job 
        /// </summary>
        [DeallocateOnJobCompletion]
        private NativeArray<JobSegmentInfo> m_Segments;
        private int m_SegmentCount;

        [DeallocateOnJobCompletion]
        private NativeArray<JobContourPoint> m_ContourPoints;
        private int m_ContourPointCount;

        [DeallocateOnJobCompletion]
        private NativeArray<JobCornerInfo> m_Corners;
        private int m_CornerCount;

        [DeallocateOnJobCompletion]
        private NativeArray<float2> m_TessPoints;
        private int m_TessPointCount;

        [DeallocateOnJobCompletion]
        NativeArray<JobShapeVertex> m_VertexData;

        [DeallocateOnJobCompletion]
        NativeArray<JobShapeVertex> m_OutputVertexData;

        [DeallocateOnJobCompletion]
        private NativeArray<JobControlPoint> m_ControlPoints;
        private int m_ControlPointCount;

        [DeallocateOnJobCompletion]
        private NativeArray<float2> m_CornerCoordinates;

        [DeallocateOnJobCompletion]
        private NativeArray<float2> m_TempPoints;

        [DeallocateOnJobCompletion]
        private NativeArray<JobControlPoint> m_GeneratedControlPoints;

        [DeallocateOnJobCompletion]
        private NativeArray<int2> m_SpriteIndices;

        [DeallocateOnJobCompletion] 
        private NativeArray<JobIntersectPoint> m_Intersectors;
        
        /// <summary>
        /// Output Native Arrays : Scope : SpriteShapeRenderer / SpriteShapeController.
        /// </summary>

        private int m_IndexArrayCount;
        public NativeArray<ushort> m_IndexArray;

        private int m_VertexArrayCount;
        public NativeSlice<Vector3> m_PosArray;
        public NativeSlice<Vector2> m_Uv0Array;
        public NativeSlice<Vector4> m_TanArray;

        private int m_GeomArrayCount;
        public NativeArray<SpriteShapeSegment> m_GeomArray;

        private int m_ColliderPointCount;
        public NativeArray<float2> m_ColliderPoints;
        public NativeArray<Bounds> m_Bounds;

        int m_IndexDataCount;
        int m_VertexDataCount;
        int m_ColliderDataCount;
        int m_ActiveIndexCount;
        int m_ActiveVertexCount;

        float2 m_FirstLT;
        float2 m_FirstLB;
        float4x4 m_Transform;

        int kModeLinear;
        int kModeContinous;
        int kModeBroken;
        int kModeUTess;

        int kCornerTypeOuterTopLeft;
        int kCornerTypeOuterTopRight;
        int kCornerTypeOuterBottomLeft;
        int kCornerTypeOuterBottomRight;
        int kCornerTypeInnerTopLeft;
        int kCornerTypeInnerTopRight;
        int kCornerTypeInnerBottomLeft;
        int kCornerTypeInnerBottomRight;
        int kControlPointCount;

        float kEpsilon;
        float kEpsilonOrder;
        float kEpsilonRelaxed;
        float kExtendSegment;
        float kRenderQuality;
        float kOptimizeRender;
        float kColliderQuality;
        float kOptimizeCollider;
        float kLowestQualityTolerance;
        float kHighestQualityTolerance;

        #region Getters.

        // Return Vertex Data Count
        private int vertexDataCount
        {
            get { return m_VertexDataCount; }
        }

        // Return final Vertex Array Count
        private int vertexArrayCount
        {
            get { return m_VertexArrayCount; }
        }
        
        // Return Index Data Count
        private int indexDataCount
        {
            get { return m_IndexDataCount; }
        }

        // Return Sprite Count
        private int spriteCount
        {
            get { return m_SpriteInfos.Length; }
        }

        private int cornerSpriteCount
        {
            get { return m_CornerSpriteInfos.Length; }
        }

        // Return Angle Range Count
        private int angleRangeCount
        {
            get { return m_AngleRanges.Length; }
        }

        // Return the Input Control Point Count.
        private int controlPointCount
        {
            get { return m_ControlPointCount; }
        }

        // Return the Contour Point Count.
        private int contourPointCount
        {
            get { return m_ContourPointCount; }
        }

        // Return Segment Count
        private int segmentCount
        {
            get { return m_SegmentCount; }
        }

        // Needs Collider Generaie.
        private bool hasCollider
        {
            get { return m_ShapeParams.splineData.w == 1; }
        }

        // Collider Pivot
        private float colliderPivot
        {
            get { return m_ShapeParams.curveData.x; }
        }

        // Border Pivot
        private float borderPivot
        {
            get { return m_ShapeParams.curveData.y; }
        }

        // Spline Detail
        private int splineDetail
        {
            get { return m_ShapeParams.splineData.y; }
        }

        // Is this Closed-Loop.
        private bool isCarpet
        {
            get { return m_ShapeParams.shapeData.x == 1; }
        }

        // Is Adaptive UV
        private bool isAdaptive
        {
            get { return m_ShapeParams.shapeData.y == 1; }
        }

        // Has Sprite Border.
        private bool hasSpriteBorder
        {
            get { return m_ShapeParams.shapeData.z == 1; }
        }

        #endregion

        #region Safe Getters.
        JobSpriteInfo GetSpriteInfo(int index)
        {
            if (index >= m_SpriteInfos.Length)
                throw new ArgumentException(string.Format("GetSpriteInfo accessed with invalid Index {0} / {1}", index, m_SpriteInfos.Length));
            return m_SpriteInfos[index];
        }

        JobSpriteInfo GetCornerSpriteInfo(int index)
        {
            int ai = index - 1;
            if (ai >= m_CornerSpriteInfos.Length || index == 0)
                throw new ArgumentException(string.Format("GetCornerSpriteInfo accessed with invalid Index {0} / {1}", index, m_CornerSpriteInfos.Length));
            return m_CornerSpriteInfos[ai];
        }

        JobAngleRange GetAngleRange(int index)
        {
            if (index >= m_AngleRanges.Length)
                throw new ArgumentException(string.Format("GetAngleRange accessed with invalid Index {0} / {1}", index, m_AngleRanges.Length));
            return m_AngleRanges[index];
        }

        JobControlPoint GetControlPoint(int index)
        {
            if (index >= m_ControlPoints.Length)
                throw new ArgumentException(string.Format("GetControlPoint accessed with invalid Index {0} / {1}", index, m_ControlPoints.Length));
            return m_ControlPoints[index];
        }

        JobContourPoint GetContourPoint(int index)
        {
            if (index >= m_ContourPointCount)
                throw new ArgumentException(string.Format("GetContourPoint accessed with invalid Index {0} / {1}", index, m_ContourPointCount));
            return m_ContourPoints[index];
        }

        JobSegmentInfo GetSegmentInfo(int index)
        {
            if (index >= m_SegmentCount)
                throw new ArgumentException(string.Format("GetSegmentInfo accessed with invalid Index {0} / {1}", index, m_SegmentCount));
            return m_Segments[index];
        }

        int GetContourIndex(int index)
        {
            if (index >= m_ControlPoints.Length)
                throw new ArgumentException(string.Format("GetContourIndex accessed with invalid Index {0} / {1}", index, m_ControlPoints.Length));
            return index * m_ShapeParams.splineData.y;
        }

        int GetEndContourIndexOfSegment(JobSegmentInfo isi)
        {
            int contourIndex = GetContourIndex(isi.sgInfo.y) - 1;
            if (isi.sgInfo.y >= m_ControlPoints.Length || isi.sgInfo.y == 0)
                throw new ArgumentException("GetEndContourIndexOfSegment accessed with invalid Index");
            return contourIndex;
        }
        #endregion

        #region Utility
        static void CopyToNativeArray<T>(NativeArray<T> from, int length, ref NativeArray<T> to) where T : struct
        {
            to = new NativeArray<T>(length, Allocator.TempJob);
            for (int i = 0; i < length; ++i)
                to[i] = from[i];
        }

        static void SafeDispose<T>(NativeArray<T> na) where T : struct
        {
            if (na.Length > 0)
                na.Dispose();
        }

        static bool IsPointOnLine(float epsilon, float2 a, float2 b, float2 c)
        {
            float cp = (c.y - a.y) * (b.x - a.x) - (c.x - a.x) * (b.y - a.y);
            if (math.abs(cp) > epsilon)
                return false;

            float dp = (c.x - a.x) * (b.x - a.x) + (c.y - a.y) * (b.y - a.y);
            if (dp < 0)
                return false;

            float ba = (b.x - a.x) * (b.x - a.x) + (b.y - a.y) * (b.y - a.y);
            if (dp > ba)
                return false;
            return true;
        }

        static bool IsPointOnLines(float epsilon, float2 p1, float2 p2, float2 p3, float2 p4, float2 r)
        {
            return IsPointOnLine(epsilon, p1, p2, r) && IsPointOnLine(epsilon, p3, p4, r);
        }

        static bool LineIntersection(float epsilon, float2 p1, float2 p2, float2 p3, float2 p4, ref float2 result)
        {
            float bx = p2.x - p1.x;
            float by = p2.y - p1.y;
            float dx = p4.x - p3.x;
            float dy = p4.y - p3.y;
            float bDotDPerp = bx * dy - by * dx;
            if (math.abs(bDotDPerp) < epsilon)
            {
                return false;
            }
            float cx = p3.x - p1.x;
            float cy = p3.y - p1.y;
            float t = (cx * dy - cy * dx) / bDotDPerp;
            if ((t >= -epsilon) && (t <= 1.0f + epsilon))
            {
                result.x = p1.x + t * bx;
                result.y = p1.y + t * by;
                return true;
            }
            return false;
        }

        static float AngleBetweenVector(float2 a, float2 b)
        {
            float dot = math.dot(a, b);
            float det = (a.x * b.y) - (b.x * a.y);
            return math.atan2(det, dot) * Mathf.Rad2Deg;
        }

        static bool GenerateColumnsBi(float2 a, float2 b, float2 whsize, bool flip, ref float2 rt, ref float2 rb, float cph, float pivot)
        {
            float2 v1 = flip ? (a - b) : (b - a);
            if (math.length(v1) < 1e-30f)
                return false;

            float2 rvxy = new float2(-1f, 1f);
            float2 v2 = v1.yx * rvxy;
            float2 whsizey = new float2(whsize.y * cph);
            v2 = math.normalize(v2);

            float2 v3 = v2 * whsizey;
            rt = a - v3;
            rb = a + v3;
            
            float2 pivotSet = (rb - rt) * pivot;
            rt = rt + pivotSet;
            rb = rb + pivotSet;            
            return true;
        }

        static bool GenerateColumnsTri(float2 a, float2 b, float2 c, float2 whsize, bool flip, ref float2 rt, ref float2 rb, float cph, float pivot)
        {
            float2 rvxy = new float2(-1f, 1f);
            float2 v0 = b - a;
            float2 v1 = c - b;
            v0 = v0.yx * rvxy;
            v1 = v1.yx * rvxy;

            float2 v2 = math.normalize(v0) + math.normalize(v1);
            if (math.length(v2) < 1e-30f)
                return false;
            v2 = math.normalize(v2);
            float2 whsizey = new float2(whsize.y * cph);
            float2 v3 = v2 * whsizey;

            rt = b - v3;
            rb = b + v3;
            
            float2 pivotSet = (rb - rt) * pivot;
            rt = rt + pivotSet;
            rb = rb + pivotSet;            
            return true;
        }
        #endregion

        #region Input Preparation.
        void AppendCornerCoordinates(ref NativeArray<float2> corners, ref int cornerCount, float2 a, float2 b, float2 c, float2 d)
        {
            corners[cornerCount++] = a;
            corners[cornerCount++] = b;
            corners[cornerCount++] = c;
            corners[cornerCount++] = d;
        }

        unsafe void PrepareInput(SpriteShapeParameters shapeParams, int maxArrayCount, NativeArray<ShapeControlPoint> shapePoints, bool optimizeGeometry, bool updateCollider, bool optimizeCollider, float colliderPivot, float colliderDetail)
        {
            kModeLinear = 0;
            kModeContinous = 1;
            kModeBroken = 2;

            kCornerTypeOuterTopLeft = 1;
            kCornerTypeOuterTopRight = 2;
            kCornerTypeOuterBottomLeft = 3;
            kCornerTypeOuterBottomRight = 4;
            kCornerTypeInnerTopLeft = 5;
            kCornerTypeInnerTopRight = 6;
            kCornerTypeInnerBottomLeft = 7;
            kCornerTypeInnerBottomRight = 8;

            m_IndexDataCount = 0;
            m_VertexDataCount = 0;
            m_ColliderDataCount = 0;
            m_ActiveIndexCount = 0;
            m_ActiveVertexCount = 0;

            kEpsilon = 0.00001f;
            kEpsilonOrder = -0.0001f; 
            kEpsilonRelaxed = 0.001f;
            kExtendSegment = 10000.0f;

            kLowestQualityTolerance = 4.0f;
            kHighestQualityTolerance = 16.0f;

            kColliderQuality = math.clamp(colliderDetail, kLowestQualityTolerance, kHighestQualityTolerance);
            kOptimizeCollider = optimizeCollider ? 1 : 0;
            kColliderQuality = (kHighestQualityTolerance - kColliderQuality + 2.0f) * 0.002f;
            colliderPivot = (colliderPivot == 0) ? kEpsilonRelaxed : -colliderPivot;

            kOptimizeRender = optimizeGeometry ? 1 : 0;
            kRenderQuality = math.clamp(shapeParams.splineDetail, kLowestQualityTolerance, kHighestQualityTolerance);
            kRenderQuality = (kHighestQualityTolerance - kRenderQuality + 2.0f) * 0.0002f;

            m_ShapeParams.shapeData = new int4(shapeParams.carpet ? 1 : 0, shapeParams.adaptiveUV ? 1 : 0, shapeParams.spriteBorders ? 1 : 0, shapeParams.fillTexture != null ? 1 : 0);
            m_ShapeParams.splineData = new int4(shapeParams.stretchUV ? 1 : 0, (shapeParams.splineDetail > 4) ? (int)shapeParams.splineDetail : 4, 0, updateCollider ? 1 : 0);
            m_ShapeParams.curveData = new float4(colliderPivot, shapeParams.borderPivot, shapeParams.angleThreshold, 0);
            float fx = 0, fy = 0;
            if (shapeParams.fillTexture != null)
            {
                fx = (float)shapeParams.fillTexture.width * (1.0f / (float)shapeParams.fillScale);
                fy = (float)shapeParams.fillTexture.height * (1.0f / (float)shapeParams.fillScale);
            }
            m_ShapeParams.fillData = new float4(shapeParams.fillScale, fx, fy, 0);
            UnsafeUtility.MemClear(m_GeomArray.GetUnsafePtr(), m_GeomArray.Length * UnsafeUtility.SizeOf<SpriteShapeSegment>());

            m_Transform = new float4x4(shapeParams.transform.m00, shapeParams.transform.m01, shapeParams.transform.m02, shapeParams.transform.m03,
                                       shapeParams.transform.m10, shapeParams.transform.m11, shapeParams.transform.m12, shapeParams.transform.m13,
                                       shapeParams.transform.m20, shapeParams.transform.m21, shapeParams.transform.m22, shapeParams.transform.m23,
                                       shapeParams.transform.m30, shapeParams.transform.m31, shapeParams.transform.m32, shapeParams.transform.m33);

            kControlPointCount = shapePoints.Length * (int)shapeParams.splineDetail * 32;
            m_Segments = new NativeArray<JobSegmentInfo>(shapePoints.Length * 2, Allocator.TempJob, NativeArrayOptions.UninitializedMemory);
            m_ContourPoints = new NativeArray<JobContourPoint>(kControlPointCount, Allocator.TempJob, NativeArrayOptions.UninitializedMemory);
            m_TessPoints = new NativeArray<float2>(shapePoints.Length * (int)shapeParams.splineDetail * 128, Allocator.TempJob, NativeArrayOptions.UninitializedMemory);
            m_VertexData = new NativeArray<JobShapeVertex>(maxArrayCount, Allocator.TempJob, NativeArrayOptions.UninitializedMemory);
            m_OutputVertexData = new NativeArray<JobShapeVertex>(maxArrayCount, Allocator.TempJob, NativeArrayOptions.UninitializedMemory);
            m_CornerCoordinates = new NativeArray<float2>(32, Allocator.TempJob, NativeArrayOptions.UninitializedMemory);
            m_Intersectors = new NativeArray<JobIntersectPoint>(kControlPointCount, Allocator.TempJob, NativeArrayOptions.ClearMemory);
            
            m_TempPoints = new NativeArray<float2>(maxArrayCount, Allocator.TempJob, NativeArrayOptions.UninitializedMemory);
            m_GeneratedControlPoints = new NativeArray<JobControlPoint>(kControlPointCount, Allocator.TempJob, NativeArrayOptions.UninitializedMemory);
            m_SpriteIndices = new NativeArray<int2>(kControlPointCount, Allocator.TempJob, NativeArrayOptions.UninitializedMemory);

            int cornerCount = 0;
            AppendCornerCoordinates(ref m_CornerCoordinates, ref cornerCount, new float2(1f, 1f), new float2(0, 1f), new float2(1f, 0), new float2(0, 0));
            AppendCornerCoordinates(ref m_CornerCoordinates, ref cornerCount, new float2(1f, 0), new float2(1f, 1f), new float2(0, 0), new float2(0, 1f));
            AppendCornerCoordinates(ref m_CornerCoordinates, ref cornerCount, new float2(0, 1f), new float2(0, 0), new float2(1f, 1f), new float2(1f, 0));
            AppendCornerCoordinates(ref m_CornerCoordinates, ref cornerCount, new float2(0, 0), new float2(1f, 0), new float2(0, 1f), new float2(1f, 1f));
            AppendCornerCoordinates(ref m_CornerCoordinates, ref cornerCount, new float2(0, 0), new float2(0, 1f), new float2(1f, 0), new float2(1f, 1f));
            AppendCornerCoordinates(ref m_CornerCoordinates, ref cornerCount, new float2(0, 1f), new float2(1f, 1f), new float2(0, 0), new float2(1f, 0));
            AppendCornerCoordinates(ref m_CornerCoordinates, ref cornerCount, new float2(1f, 0), new float2(0, 0), new float2(1f, 1f), new float2(0, 1f));
            AppendCornerCoordinates(ref m_CornerCoordinates, ref cornerCount, new float2(1f, 1f), new float2(1f, 0), new float2(0, 1f), new float2(0, 0));
        }

        void TransferSprites(ref NativeArray<JobSpriteInfo> spriteInfos, Sprite[] sprites, int maxCount)
        {

            for (int i = 0; i < sprites.Length && i < maxCount; ++i)
            {
                JobSpriteInfo spriteInfo = spriteInfos[i];
                Sprite spr = sprites[i];
                if (spr != null)
                {
                    Texture2D tex = spr.texture;
                    spriteInfo.texRect = new float4(spr.textureRect.x, spr.textureRect.y, spr.textureRect.width, spr.textureRect.height);
                    spriteInfo.texData = new float4(tex.width, tex.height, tex.texelSize.x, tex.texelSize.y);
                    spriteInfo.border = new float4(spr.border.x, spr.border.y, spr.border.z, spr.border.w);
                    spriteInfo.uvInfo = new float4(spriteInfo.texRect.x / spriteInfo.texData.x, spriteInfo.texRect.y / spriteInfo.texData.y, spriteInfo.texRect.z / spriteInfo.texData.x, spriteInfo.texRect.w / spriteInfo.texData.y);
                    spriteInfo.metaInfo = new float4(spr.pixelsPerUnit, spr.pivot.y / spr.textureRect.height, spr.rect.width, spr.rect.height);

                    if (!math.any(spriteInfo.texRect))
                    {
                        Cleanup();
                        throw new ArgumentException(string.Format("{0} is packed with Tight packing or mesh type set to Tight. Please check input sprites", spr.name));
                    }
                }
                spriteInfos[i] = spriteInfo;
            }

        }

        void PrepareSprites(Sprite[] edgeSprites, Sprite[] cornerSprites)
        {
            m_SpriteInfos = new NativeArray<JobSpriteInfo>(edgeSprites.Length, Allocator.TempJob);
            TransferSprites(ref m_SpriteInfos, edgeSprites, edgeSprites.Length);

            m_CornerSpriteInfos = new NativeArray<JobSpriteInfo>(kCornerTypeInnerBottomRight, Allocator.TempJob);
            TransferSprites(ref m_CornerSpriteInfos, cornerSprites, cornerSprites.Length);
        }

        void PrepareAngleRanges(AngleRangeInfo[] angleRanges)
        {
            m_AngleRanges = new NativeArray<JobAngleRange>(angleRanges.Length, Allocator.TempJob);

            for (int i = 0; i < angleRanges.Length; ++i)
            {
                JobAngleRange angleRange = m_AngleRanges[i];
                AngleRangeInfo ari = angleRanges[i];
                int[] spr = ari.sprites;
                if (ari.start > ari.end)
                {
                    var sw = ari.start;
                    ari.start = ari.end;
                    ari.end = sw;
                }
                angleRange.spriteAngles = new float4(ari.start + 90f, ari.end + 90f, 0, 0);
                angleRange.spriteData = new int4((int)ari.order, spr.Length, 32, 0);
                m_AngleRanges[i] = angleRange;
            }
        }

        void PrepareControlPoints(NativeArray<ShapeControlPoint> shapePoints, NativeArray<SplinePointMetaData> metaData)
        {
            float2 zero = new float2(0, 0);
            m_ControlPoints = new NativeArray<JobControlPoint>(kControlPointCount, Allocator.TempJob);

            for (int i = 0; i < shapePoints.Length; ++i)
            {
                JobControlPoint shapePoint = m_ControlPoints[i];
                ShapeControlPoint sp = shapePoints[i];
                SplinePointMetaData md = metaData[i];
                shapePoint.position = new float2(sp.position.x, sp.position.y);
                shapePoint.tangentLt = (sp.mode == kModeLinear) ? zero : new float2(sp.leftTangent.x, sp.leftTangent.y);
                shapePoint.tangentRt = (sp.mode == kModeLinear) ? zero : new float2(sp.rightTangent.x, sp.rightTangent.y);
                shapePoint.cpInfo = new float2(md.height, 0);
                shapePoint.cpData = new int4((int)md.spriteIndex, md.cornerMode, sp.mode, 0);
                shapePoint.exData = new int4(-1, 0, 0, 0);
                m_ControlPoints[i] = shapePoint;
            }
            m_ControlPointCount = shapePoints.Length;
            m_Corners = new NativeArray<JobCornerInfo>(shapePoints.Length, Allocator.TempJob);
            GenerateControlPoints();
        }
        #endregion 

        #region Resolve Angles for Points.
        bool WithinRange(JobAngleRange angleRange, float inputAngle)
        {
            float range = angleRange.spriteAngles.y - angleRange.spriteAngles.x;
            float angle = Mathf.Repeat(inputAngle - angleRange.spriteAngles.x, 360f);
            return (angle >= 0f && angle <= range);
        }

        bool AngleWithinRange(float t, float a, float b)
        {
            return (a != 0 && b != 0) && (t >= a && t <= b);
        }

        static float2 BezierPoint(float2 st, float2 sp, float2 ep, float2 et, float t)
        {
            float2 xt = new float2(t);
            float2 nt = new float2(1.0f - t);
            float2 x3 = new float2(3.0f);
            return (sp * nt * nt * nt) + (st * nt * nt * xt * x3) + (et * nt * xt * xt * x3) + (ep * xt * xt * xt);
        }

        static float SlopeAngle(float2 dirNormalized)
        {
            float2 dvup = new float2(0, 1f);
            float2 dvrt = new float2(1f, 0);

            float dr = math.dot(dirNormalized, dvrt);
            float du = math.dot(dirNormalized, dvup);
            float cu = math.acos(du);
            float sn = dr >= 0 ? 1.0f : -1.0f;
            float an = cu * Mathf.Rad2Deg * sn;

            // Adjust angles when direction is parallel to Up Axis.
            an = (du != 1f) ? an : 0;
            an = (du != -1f) ? an : -180f;
            return an;
        }

        static float SlopeAngle(float2 start, float2 end)
        {
            float2 dir = math.normalize(start - end);
            return SlopeAngle(dir);
        }

        bool ResolveAngle(float angle, int activeIndex, ref float renderOrder, ref int spriteIndex, ref int firstSpriteIndex)
        {
            int localRenderOrder = 0;
            int localSpriteIndex = 0;
            for (int i = 0; i < m_AngleRanges.Length; ++i)
            {
                bool withinRange = WithinRange(m_AngleRanges[i], angle);
                if (withinRange)
                {
                    int validIndex = (activeIndex < m_AngleRanges[i].spriteData.y) ? activeIndex : 0;
                    renderOrder = localRenderOrder + validIndex;
                    spriteIndex = localSpriteIndex + validIndex;
                    firstSpriteIndex = localSpriteIndex;
                    return true;
                }
                localRenderOrder += m_AngleRanges[i].spriteData.z;
                localSpriteIndex += m_AngleRanges[i].spriteData.y;
            }
            return false;
        }

        int GetSpriteIndex(int index, int previousIndex, ref int resolved)
        {
            int next = (index + 1) % controlPointCount, spriteIndex = -1, firstSpriteIndex = -1;
            float order = 0;
            var cp = GetControlPoint(index);
            float angle = SlopeAngle(GetControlPoint(next).position, cp.position);
            bool resolve = ResolveAngle(angle, cp.cpData.x, ref order, ref spriteIndex, ref firstSpriteIndex);
            resolved = resolve ? 1 : 0;
            return resolve ? spriteIndex : previousIndex;
        }
        #endregion

        #region Segments.
        void GenerateSegments()
        {
            int activeSpriteIndex = 0, activeSegmentIndex = 0, firstSpriteIndex = -1;
            JobSegmentInfo activeSegment = m_Segments[0];
            activeSegment.sgInfo = int4.zero;
            activeSegment.spriteInfo = int4.zero;
            float angle = 0;

            // Generate Segments.
            for (int i = 0; i < controlPointCount; ++i)
            {
                int next = (i + 1) % controlPointCount;

                // Check for Last Point and see if we need loop-back.
                bool skipSegmenting = false;
                if (next == 0)
                {
                    if (!isCarpet)
                        continue;
                    next = 1;
                    skipSegmenting = true;
                }

                JobControlPoint iscp = GetControlPoint(i);
                JobControlPoint iscpNext = GetControlPoint(next);

                // If this segment is corner, continue.
                if (iscp.exData.x > 0 && iscp.exData.x == iscpNext.exData.x && iscp.exData.z == 2)
                    continue;

                // Resolve Angle and Order.
                int4 pointData = iscp.cpData;
                float2 pointInfo = iscp.cpInfo;

                // Get Min Max Segment.
                int mn = (i < next) ? i : next;
                int mx = (i > next) ? i : next;
                bool continueStrip = (iscp.cpData.z == kModeContinous), edgeUpdated = false;

                if (false == continueStrip || 0 == activeSegmentIndex)
                    angle = SlopeAngle(iscpNext.position, iscp.position);
                bool resolved = ResolveAngle(angle, pointData.x, ref pointInfo.y, ref pointData.w, ref firstSpriteIndex);
                if (!resolved && !skipSegmenting)
                {
                    // If we do not resolve SpriteIndex (AngleRange) just continue existing segment.
                    pointData.w = activeSpriteIndex;
                    iscp.cpData = pointData;
                    m_ControlPoints[i] = iscp;

                    // Insert Dummy Segment.
                    activeSegment = m_Segments[activeSegmentIndex];
                    activeSegment.sgInfo.x = mn;
                    activeSegment.sgInfo.y = mx;
                    activeSegment.sgInfo.z = -1;
                    m_Segments[activeSegmentIndex] = activeSegment;
                    activeSegmentIndex++;
                    continue;
                }

                // Update current Point.
                activeSpriteIndex = pointData.w;
                iscp.cpData = pointData;
                iscp.cpInfo = pointInfo;
                m_ControlPoints[i] = iscp;
                if (skipSegmenting)
                    continue;

                // Check for Segments. Also check if the Segment Start has been resolved. Otherwise simply start with the next one.
                if (activeSegmentIndex != 0)
                    continueStrip = continueStrip && (m_SpriteIndices[activeSegment.sgInfo.x].y != 0 && activeSpriteIndex == activeSegment.sgInfo.z);

                if (continueStrip && i != (controlPointCount - 1))
                {
                    for (int s = 0; s < activeSegmentIndex; ++s)
                    {
                        activeSegment = m_Segments[s];
                        if (activeSegment.sgInfo.x - mn == 1)
                        {
                            edgeUpdated = true;
                            activeSegment.sgInfo.x = mn;
                            m_Segments[s] = activeSegment;
                            break;
                        }
                        if (mx - activeSegment.sgInfo.y == 1)
                        {
                            edgeUpdated = true;
                            activeSegment.sgInfo.y = mx;
                            m_Segments[s] = activeSegment;
                            break;
                        }
                    }
                }

                if (!edgeUpdated)
                {
                    activeSegment = m_Segments[activeSegmentIndex];
                    JobSpriteInfo sprLt = GetSpriteInfo(iscp.cpData.w);
                    activeSegment.sgInfo.x = mn;
                    activeSegment.sgInfo.y = mx;
                    activeSegment.sgInfo.z = activeSpriteIndex;
                    activeSegment.sgInfo.w = firstSpriteIndex;
                    activeSegment.spriteInfo.x = sprLt.texRect.z;
                    activeSegment.spriteInfo.y = sprLt.texRect.w;
                    activeSegment.spriteInfo.z = pointInfo.y;
                    m_Segments[activeSegmentIndex] = activeSegment;
                    activeSegmentIndex++;
                }
            }

            m_SegmentCount = activeSegmentIndex;

        }

        void UpdateSegments()
        {
            // Determine Distance of Segment.
            for (int i = 0; i < segmentCount; ++i)
            {
                // Calculate Segment Distances.
                JobSegmentInfo isi = GetSegmentInfo(i);
                if (isi.spriteInfo.z >= 0)
                {
                    isi.spriteInfo.w = SegmentDistance(isi);
                    m_Segments[i] = isi;
                }
            }
        }

        bool GetSegmentBoundaryColumn(JobSegmentInfo segment, JobSpriteInfo sprInfo, float2 whsize, float2 startPos, float2 endPos, bool end, ref float2 top, ref float2 bottom)
        {
            bool res = false;
            float pivot = 0.5f - sprInfo.metaInfo.y;
            if (!end)
            {
                JobControlPoint icp = GetControlPoint(segment.sgInfo.x);
                if (math.any(icp.tangentRt))
                    endPos = icp.tangentRt + startPos;
                res = GenerateColumnsBi(startPos, endPos, whsize, end, ref top, ref bottom, icp.cpInfo.x * 0.5f, pivot);
            }
            else
            {
                JobControlPoint jcp = GetControlPoint(segment.sgInfo.y);
                if (math.any(jcp.tangentLt))
                    endPos = jcp.tangentLt + startPos;
                res = GenerateColumnsBi(startPos, endPos, whsize, end, ref top, ref bottom, jcp.cpInfo.x * 0.5f, pivot);
            }
            return res;
        }

        bool GenerateControlPoints()
        {
            // Globals.
            int activePoint = 0, activeIndex = 0;
            int startPoint = 0, endPoint = controlPointCount, lastPoint = (controlPointCount - 1);

            float2 rvxy = new float2(-1f, 1f);
            int2 sprData = new int2(0, 0);
            bool useSlice = true;
            int spriteCount = m_SpriteInfos.Length;

            // Calc and calculate Indices.
            for (int i = 0; i < controlPointCount; ++i)
            {
                var resolved = 0;
                int spriteIndex = GetSpriteIndex(i, activeIndex, ref resolved);
                sprData.x = activeIndex = spriteIndex;
                sprData.y = resolved;
                m_SpriteIndices[i] = sprData;
            }

            // Open-Ended. We simply dont allow Continous mode in End-points.
            if (!isCarpet)
            {
                JobControlPoint cp = GetControlPoint(0);
                cp.cpData.z = (cp.cpData.z == kModeContinous) ? kModeBroken : cp.cpData.z;
                m_GeneratedControlPoints[activePoint++] = cp;
                // If its not carpet, we already pre-insert start and endpoint.
                startPoint = 1;
                endPoint = controlPointCount - 1;
            }

            // Generate Intermediates.
            for (int i = startPoint; i < endPoint; ++i)
            {

                // Check if the Neighbor Points are all in Linear Mode.
                bool cornerCriteriaMet = false;
                bool vc = InsertCorner(i, ref m_SpriteIndices, ref m_GeneratedControlPoints, ref activePoint, ref cornerCriteriaMet);
                if (vc)
                    continue;
                // NO Corners.
                var cp = GetControlPoint(i);
                cp.exData.z = (cornerCriteriaMet && cp.cpData.y == 2) ? 1 : 0;    // Set this to stretched of Corner criteria met but no corner sprites but stretched corner.
                m_GeneratedControlPoints[activePoint++] = cp;
            }

            // Open-Ended.
            if (!isCarpet)
            {
                // Fixup for End-Points and Point-Mode.
                JobControlPoint sp = m_GeneratedControlPoints[0];
                sp.exData.z = 1;
                m_GeneratedControlPoints[0] = sp;
                
                JobControlPoint cp = GetControlPoint(endPoint); 
                cp.cpData.z = (cp.cpData.z == kModeContinous) ? kModeBroken : cp.cpData.z;
                cp.exData.z = 1;
                m_GeneratedControlPoints[activePoint++] = cp;
            }
            // If Closed Shape
            else
            {
                JobControlPoint cp = m_GeneratedControlPoints[0];
                m_GeneratedControlPoints[activePoint++] = cp;
            }

            // Copy from these intermediate Points to main Control Points.
            for (int i = 0; i < activePoint; ++i)
                m_ControlPoints[i] = m_GeneratedControlPoints[i];
            m_ControlPointCount = activePoint;

            // Calc and calculate Indices.
            for (int i = 0; i < controlPointCount; ++i)
            {
                var resolved = 0;
                int spriteIndex = GetSpriteIndex(i, activeIndex, ref resolved);
                sprData.x = activeIndex = spriteIndex;
                sprData.y = resolved;
                m_SpriteIndices[i] = sprData;
            }

            // End
            return useSlice;
        }

        float SegmentDistance(JobSegmentInfo isi)
        {
            float distance = 0;
            int stIx = GetContourIndex(isi.sgInfo.x);
            int enIx = GetEndContourIndexOfSegment(isi);

            for (int i = stIx; i < enIx; ++i)
            {
                int j = i + 1;
                JobContourPoint lt = GetContourPoint(i);
                JobContourPoint rt = GetContourPoint(j);
                distance = distance + math.distance(lt.position, rt.position);
            }

            return distance;
        }

        void GenerateContour()
        {
            int controlPointContour = controlPointCount - 1;
            // Expand the Bezier.
            int ap = 0;
            float fmax = (float)(splineDetail - 1);
            for (int i = 0; i < controlPointContour; ++i)
            {
                int j = i + 1;
                JobControlPoint cp = GetControlPoint(i);
                JobControlPoint pp = GetControlPoint(j);

                float2 p0 = cp.position;
                float2 p1 = pp.position;
                float2 sp = p0;
                float2 rt = p0 + cp.tangentRt;
                float2 lt = p1 + pp.tangentLt;
                int cap = ap;
                float spd = 0, cpd = 0;

                for (int n = 0; n < splineDetail; ++n)
                {
                    JobContourPoint xp = m_ContourPoints[ap];
                    float t = (float) n / fmax;
                    float2 bp = BezierPoint(rt, p0, p1, lt, t);
                    xp.position = bp;
                    spd += math.distance(bp, sp);
                    m_ContourPoints[ap++] = xp;
                    sp = bp;
                }

                sp = p0;
                for (int n = 0; n < splineDetail; ++n)
                {
                    JobContourPoint xp = m_ContourPoints[cap];
                    cpd += math.distance(xp.position, sp);
                    xp.ptData.x = math.lerp(cp.cpInfo.x, pp.cpInfo.x, cpd / spd);
                    m_ContourPoints[cap++] = xp;
                    sp = xp.position;
                }

            }

            // End
            m_ContourPointCount = ap;
            int tessPoints = 0;
            
            // Create Tessallator if required.
            for (int i = 0; i < contourPointCount; ++i)
            {
                if ((i + 1) % splineDetail == 0)
                    continue;
                int h = (i == 0) ? (contourPointCount - 1) : (i - 1);
                int j = (i + 1) % contourPointCount;
                h = (i % splineDetail == 0) ? (h - 1) : h;

                JobContourPoint pp = GetContourPoint(h);
                JobContourPoint cp = GetContourPoint(i);
                JobContourPoint np = GetContourPoint(j);

                float2 cpd = cp.position - pp.position;
                float2 npd = np.position - cp.position;
                if (math.length(cpd) < kEpsilon || math.length(npd) < kEpsilon)
                    continue;

                float2 vl = math.normalize(cpd);
                float2 vr = math.normalize(npd);

                vl = new float2(-vl.y, vl.x);
                vr = new float2(-vr.y, vr.x);

                float2 va = math.normalize(vl) + math.normalize(vr);
                float2 vn = math.normalize(va);

                if (math.any(va) && math.any(vn))
                    m_TessPoints[tessPoints++] = cp.position + (vn * borderPivot);
            }

            m_TessPointCount = tessPoints;            
        }

        // Burstable UTess2D Version.
        void TessellateContour()
        {

            // Generate Contour
            GenerateContour();
            
            // Fill Geom. Generate in Native code until we have a reasonably fast enough Tessellation in NativeArray based Jobs.
            SpriteShapeSegment geom = m_GeomArray[0];
            Vector3 pos = m_PosArray[0];

            geom.vertexCount = 0;
            geom.geomIndex = 0;
            geom.indexCount = 0;
            geom.spriteIndex = -1;

            // Fill Geometry. Check if Fill Texture and Fill Scale is Valid.
            if (math.all(m_ShapeParams.shapeData.xw))
            {
                // Fill Geometry. Check if Fill Texture and Fill Scale is Valid.
                if (m_TessPointCount > 0)
                {
                    if (kOptimizeRender > 0)
                        OptimizePoints(kRenderQuality, ref m_TessPoints, ref m_TessPointCount);

                    int dataLength = m_TessPointCount;
                    NativeArray<TessEdge> edges = new NativeArray<TessEdge>(dataLength - 1, Allocator.Temp);
                    NativeArray<float2> points = new NativeArray<float2>(dataLength - 1, Allocator.Temp);

                    for (int i = 0; i < points.Length; ++i)
                        points[i] = m_TessPoints[i];
                    
                    for (int i = 0; i < dataLength - 2; ++i)
                    {
                        TessEdge te = edges[i];
                        te.a = i;
                        te.b = i + 1;
                        edges[i] = te;
                    }
                    TessEdge tee = edges[dataLength - 2];
                    tee.a = dataLength - 2;
                    tee.b = 0;
                    edges[dataLength - 2] = tee;
                    
                    Tessellator st = new Tessellator();
                    st.Triangulate(points, edges);
                    st.ApplyDelaunay(points, edges);
                    NativeArray<TessCell> cellsOut = st.RemoveExterior(ref m_TessPointCount);

                    for (int i = 0; i < m_TessPointCount; ++i)
                    {

                        var a = (UInt16)cellsOut[i].a;
                        var b = (UInt16)cellsOut[i].b;
                        var c = (UInt16)cellsOut[i].c;
                        if ( a != 0 || b != 0 || c != 0)
                        {
                            m_IndexArray[m_ActiveIndexCount++] = a;
                            m_IndexArray[m_ActiveIndexCount++] = c;
                            m_IndexArray[m_ActiveIndexCount++] = b;
                        }
                        
                    }

                    cellsOut.Dispose();
                    points.Dispose();
                    edges.Dispose();
                    st.Cleanup();

                    if (m_ActiveIndexCount > 0)
                    {

                        for (m_ActiveVertexCount = 0; m_ActiveVertexCount < m_TessPointCount + 3; ++m_ActiveVertexCount)
                        {
                            pos = new Vector3(m_TessPoints[m_ActiveVertexCount].x, m_TessPoints[m_ActiveVertexCount].y, 0);
                            m_PosArray[m_ActiveVertexCount] = pos;
                        }

                        m_IndexDataCount = geom.indexCount = m_ActiveIndexCount;
                        m_VertexDataCount = geom.vertexCount = m_ActiveVertexCount;
                        
                    }
                }
            }

            if (m_TanArray.Length > 1)
            {
                for (int i = 0; i < m_ActiveVertexCount; ++i)
                    m_TanArray[i] = new Vector4(1.0f, 0, 0, -1.0f);
            }

            m_GeomArray[0] = geom;

        }        
        
        void TessellateContourMainThread()
        {

            // Generate Contour
            GenerateContour();

            // Fill Geom. Generate in Native code until we have a reasonably fast enough Tessellation in NativeArray based Jobs.
            SpriteShapeSegment geom = m_GeomArray[0];
            Vector3 pos = m_PosArray[0];

            geom.vertexCount = 0;
            geom.geomIndex = 0;
            geom.indexCount = 0;
            geom.spriteIndex = -1;

            // Fill Geometry. Check if Fill Texture and Fill Scale is Valid.
            if (math.all(m_ShapeParams.shapeData.xw))
            {
                // Fill Geometry. Check if Fill Texture and Fill Scale is Valid.
                if (m_TessPointCount > 0)
                {
                    if (kOptimizeRender > 0)
                        OptimizePoints(kRenderQuality, ref m_TessPoints, ref m_TessPointCount);

                    var inputs = new ContourVertex[m_TessPointCount];
                    for (int i = 0; i < m_TessPointCount; ++i)
                        inputs[i] = new ContourVertex() { Position = new Vec3() { X = m_TessPoints[i].x, Y = m_TessPoints[i].y } };

                    Tess tess = new Tess();
                    tess.AddContour(inputs, ContourOrientation.Original);
                    tess.Tessellate(WindingRule.NonZero, ElementType.Polygons, 3);

                    var indices = tess.Elements.Select(i => (UInt16)i).ToArray();
                    var vertices = tess.Vertices.Select(v => new Vector2(v.Position.X, v.Position.Y)).ToArray();
                    m_IndexDataCount = indices.Length;
                    m_VertexDataCount = vertices.Length;

                    if (vertices.Length > 0)
                    {
                        for (m_ActiveIndexCount = 0; m_ActiveIndexCount < m_IndexDataCount; ++m_ActiveIndexCount)
                        {
                            m_IndexArray[m_ActiveIndexCount] = indices[m_ActiveIndexCount];
                        }
                        for (m_ActiveVertexCount = 0; m_ActiveVertexCount < m_VertexDataCount; ++m_ActiveVertexCount)
                        {
                            pos = new Vector3(vertices[m_ActiveVertexCount].x, vertices[m_ActiveVertexCount].y, 0);
                            m_PosArray[m_ActiveVertexCount] = pos;
                        }

                        geom.indexCount = m_ActiveIndexCount;
                        geom.vertexCount = m_ActiveVertexCount;
                    }
                }
            }

            if (m_TanArray.Length > 1)
            {
                for (int i = 0; i < m_ActiveVertexCount; ++i)
                    m_TanArray[i] = new Vector4(1.0f, 0, 0, -1.0f);
            }

            m_GeomArray[0] = geom;

        }

        void CalculateBoundingBox()
        {
            Bounds bounds = new Bounds();
            
            { 
                for (int i = 0; i < vertexArrayCount; ++i)
                {
                    Vector3 pos = m_PosArray[i];
                    bounds.Encapsulate(pos);
                }
            }            
            {
                for (int i = 0; i < contourPointCount; ++i)
                {
                    Vector3 pos = new Vector3(m_ContourPoints[i].position.x, m_ContourPoints[i].position.y, 0);
                    bounds.Encapsulate(pos);
                }
            }

            m_Bounds[0] = bounds;
        }

        void CalculateTexCoords()
        {

            SpriteShapeSegment geom = m_GeomArray[0];
            if (m_ShapeParams.splineData.x > 0)
            {
                float3 ext = m_Bounds[0].extents * 2;
                float3 min = m_Bounds[0].center - m_Bounds[0].extents;
                for (int i = 0; i < geom.vertexCount; ++i)
                {
                    Vector3 pos = m_PosArray[i];
                    Vector2 uv0 = m_Uv0Array[i];
                    float3 uv = ((new float3(pos.x, pos.y, pos.z) - min) / ext) * m_ShapeParams.fillData.x;
                    uv0.x = uv.x;
                    uv0.y = uv.y;
                    m_Uv0Array[i] = uv0;
                }
            }
            else
            {
                for (int i = 0; i < geom.vertexCount; ++i)
                {
                    Vector3 pos = m_PosArray[i];
                    Vector2 uv0 = m_Uv0Array[i];
                    float3 uv = math.transform(m_Transform, new float3(pos.x, pos.y, pos.z));
                    uv0.x = uv.x / m_ShapeParams.fillData.y;
                    uv0.y = uv.y / m_ShapeParams.fillData.z;
                    m_Uv0Array[i] = uv0;
                }
            }

        }

        void CopyVertexData(ref NativeSlice<Vector3> outPos, ref NativeSlice<Vector2> outUV0, ref NativeSlice<Vector4> outTan, int outIndex, NativeArray<JobShapeVertex> inVertices, int inIndex, float sOrder)
        {
            Vector3 iscp = outPos[outIndex];
            Vector2 iscu = outUV0[outIndex];

            float3 v0 = new float3(inVertices[inIndex].pos.x, inVertices[inIndex].pos.y, sOrder);
            float3 v1 = new float3(inVertices[inIndex + 1].pos.x, inVertices[inIndex + 1].pos.y, sOrder);
            float3 v2 = new float3(inVertices[inIndex + 2].pos.x, inVertices[inIndex + 2].pos.y, sOrder);
            float3 v3 = new float3(inVertices[inIndex + 3].pos.x, inVertices[inIndex + 3].pos.y, sOrder);

            outPos[outIndex] = v0;
            outUV0[outIndex] = inVertices[inIndex].uv;
            outPos[outIndex + 1] = v1;
            outUV0[outIndex + 1] = inVertices[inIndex + 1].uv;
            outPos[outIndex + 2] = v2;
            outUV0[outIndex + 2] = inVertices[inIndex + 2].uv;
            outPos[outIndex + 3] = v3;
            outUV0[outIndex + 3] = inVertices[inIndex + 3].uv;

            if (outTan.Length > 1)
            {
                outTan[outIndex] = inVertices[inIndex].tan;
                outTan[outIndex + 1] = inVertices[inIndex + 1].tan;
                outTan[outIndex + 2] = inVertices[inIndex + 2].tan;
                outTan[outIndex + 3] = inVertices[inIndex + 3].tan;
            }
        }

        int CopySegmentRenderData(JobSpriteInfo ispr, ref NativeSlice<Vector3> outPos, ref NativeSlice<Vector2> outUV0, ref NativeSlice<Vector4> outTan, ref int outCount, ref NativeArray<ushort> indexData, ref int indexCount, NativeArray<JobShapeVertex> inVertices, int inCount, float sOrder)
        {
            if (inCount < 4)
                return -1;

            int localVertex = 0;
            int finalCount = indexCount + inCount;
            if (finalCount >= indexData.Length)
                throw new InvalidOperationException(
                    "Mesh data has reached Limits. Please try dividing shape into smaller blocks.");

            for (int i = 0; i < inCount; i = i + 4, outCount = outCount + 4, localVertex = localVertex + 4)
            {
                CopyVertexData(ref outPos, ref outUV0, ref outTan, outCount, inVertices, i, sOrder);
                indexData[indexCount++] = (ushort) (localVertex);
                indexData[indexCount++] = (ushort) (3 + localVertex);
                indexData[indexCount++] = (ushort) (1 + localVertex);
                indexData[indexCount++] = (ushort) (localVertex);
                indexData[indexCount++] = (ushort) (2 + localVertex);
                indexData[indexCount++] = (ushort) (3 + localVertex);
            }

            return outCount;
        }

        void GetLineSegments(JobSpriteInfo sprInfo, JobSegmentInfo segment, float2 whsize, ref float2 vlt,
            ref float2 vlb, ref float2 vrt, ref float2 vrb)
        {
            JobControlPoint scp = GetControlPoint(segment.sgInfo.x);
            JobControlPoint ecp = GetControlPoint(segment.sgInfo.y);
            
            GetSegmentBoundaryColumn(segment, sprInfo, whsize, scp.position, ecp.position, false, ref vlt, ref vlb);
            GetSegmentBoundaryColumn(segment, sprInfo, whsize, ecp.position, scp.position, true, ref vrt, ref vrb);
        }

        void TessellateSegment(int segmentIndex, JobSpriteInfo sprInfo, JobSegmentInfo segment, float2 whsize, float4 border,
            float pxlWidth, bool useClosure, bool validHead, bool validTail, NativeArray<JobShapeVertex> vertices,
            int vertexCount, ref NativeArray<JobShapeVertex> outputVertices, ref int outputCount)
        {
            int outputVertexCount = 0;
            float2 zero = float2.zero;
            float2 lt = zero, lb = zero, rt = zero, rb = zero;
            float4 stretcher = new float4(1.0f, 1.0f, 0, 0);            
            var column0 = new JobShapeVertex();
            var column1 = new JobShapeVertex();
            var column2 = new JobShapeVertex();
            var column3 = new JobShapeVertex();

            int cms = vertexCount - 1;
            int lcm = cms - 1;
            int expectedCount = outputCount + (cms * 4);
            var sprite = vertices[0].sprite;

            if (expectedCount >= outputVertices.Length)
                throw new InvalidOperationException("Mesh data has reached Limits. Please try dividing shape into smaller blocks.");            
            
            float uvDist = 0;
            float uvStart = border.x;
            float uvEnd = whsize.x - border.z;
            float uvTotal = whsize.x;
            float uvInter = uvEnd - uvStart;
            float uvNow = uvStart / uvTotal;
            float dt = uvInter / pxlWidth;
            float pivot = 0.5f - sprInfo.metaInfo.y;
            
            //// //// //// //// Stretch 
            bool stretchCorners = false;
            bool stretchSegment = math.abs(segment.sgInfo.x - segment.