static float3x3 float3x3(uint v) { return new float3x3(v); }

        /// <summary>Return a float3x3 matrix constructed from a uint3x3 matrix by componentwise conversion.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float3x3 float3x3(uint3x3 v) { return new float3x3(v); }

        /// <summary>Returns a float3x3 matrix constructed from a single double value by converting it to float and assigning it to every component.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float3x3 float3x3(double v) { return new float3x3(v); }

        /// <summary>Return a float3x3 matrix constructed from a double3x3 matrix by componentwise conversion.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float3x3 float3x3(double3x3 v) { return new float3x3(v); }

        /// <summary>Return the float3x3 transpose of a float3x3 matrix.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float3x3 transpose(float3x3 v)
        {
            return float3x3(
                v.c0.x, v.c0.y, v.c0.z,
                v.c1.x, v.c1.y, v.c1.z,
                v.c2.x, v.c2.y, v.c2.z);
        }

        /// <summary>Returns the float3x3 full inverse of a float3x3 matrix.</summary>
        public static float3x3 inverse(float3x3 m)
        {
            float3 c0 = m.c0;
            float3 c1 = m.c1;
            float3 c2 = m.c2;

            float3 t0 = float3(c1.x, c2.x, c0.x);
            float3 t1 = float3(c1.y, c2.y, c0.y);
            float3 t2 = float3(c1.z, c2.z, c0.z);

            float3 m0 = t1 * t2.yzx - t1.yzx * t2;
            float3 m1 = t0.yzx * t2 - t0 * t2.yzx;
            float3 m2 = t0 * t1.yzx - t0.yzx * t1;

            float rcpDet = 1.0f / csum(t0.zxy * m0);
            return float3x3(m0, m1, m2) * rcpDet;
        }

        /// <summary>Returns the determinant of a float3x3 matrix.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float determinant(float3x3 m)
        {
            float3 c0 = m.c0;
            float3 c1 = m.c1;
            float3 c2 = m.c2;

            float m00 = c1.y * c2.z - c1.z * c2.y;
            float m01 = c0.y * c2.z - c0.z * c2.y;
            float m02 = c0.y * c1.z - c0.z * c1.y;

            return c0.x * m00 - c1.x * m01 + c2.x * m02;
        }

        /// <summary>Returns a uint hash code of a float3x3 vector.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint hash(float3x3 v)
        {
            return csum(asuint(v.c0) * uint3(0x713BD06Fu, 0x753AD6ADu, 0xD19764C7u) + 
                        asuint(v.c1) * uint3(0xB5D0BF63u, 0xF9102C5Fu, 0x98