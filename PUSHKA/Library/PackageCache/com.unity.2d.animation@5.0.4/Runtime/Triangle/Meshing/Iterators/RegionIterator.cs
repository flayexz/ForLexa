               for (int i = 0; i < m_NumPoints; ++i)
                {
                    TessEvent evt = new TessEvent();
                    evt.a = points[i];
                    evt.b = new float2();
                    evt.idx = i;
                    evt.type = (int) TessEventType.EVENT_POINT;
                    events[eventCount++] = evt;
                }

                for (int i = 0; i < numEdges; ++i)
                {
                    TessEdge e = edgesIn[i];
                    float2 a = points[e.a];
                    float2 b = points[e.b];
                    if (a.x < b.x)
                    {
                        TessEvent _s = new TessEvent();
                        _s.a = a;
                        _s.b = b;
                        _s.idx = i;
                        _s.type = (int) TessEventType.EVENT_START;

                        TessEvent _e = new TessEvent();
                        _e.a = b;
                        _e.b = a;
                        _e.idx = i;
                        _e.type = (int) TessEventType.EVENT_END;

                        events[eventCount++] = _s;
                        events[eventCount++] = _e;
                    }
                    else if (a.x > b.x)
                    {
                        TessEvent _s = new TessEvent();
                        _s.a = b;
                        _s.b = a;
                        _s.idx = i;
                        _s.type = (int) TessEventType.EVENT_START;

                        TessEvent _e = new TessEvent();
                        _e.a = a;
                        _e.b = b;
                        _e.idx = i;
                        _e.type = (int) TessEventType.EVENT_END;

                        events[eventCount++] = _s;
                        events[eventCount++] = _e;
                    }
                }

                unsafe
                {
                    TessUtils.InsertionSort<TessEvent, TessEventCompare>(
                        NativeArrayUnsafeUtility.GetUnsafeBufferPointerWithoutChecks(events), 0, eventCount - 1,
                        new TessEventCompare());
                    ;
                }

                float minX = events[0].a.x - (1 + math.abs(events[0].a.x)) * math.pow(2.0f, -16.0f);
                TessHull hull;
                hull.a.x = minX;
                hull.a.y = 1;
                hull.b.x = minX;
                hull.b.y = 0;
                hull.idx = -1;
                hull.ilarray = new ArraySlice<int>(m_ILArray, m_NumPoints * m_NumPoints, m_NumPoints); // Last element
                hull.iuarray = new ArraySlice<int>(m_IUArray, m_NumPoints * m_NumPoints, m_NumPoints);
                hull.ilcount = 0;
                hull.iucount = 0;
                hulls[hullCount++] = hull;

                for (int i = 0, numEvents = eventCount; i < numEvents; ++i)
                {
                    switch (events[i].type)
                    {
                        case (int) TessEventType.EVENT_POINT:
                        {
                            AddPoint(hulls, hullCount, points, events[i].a, events[i].idx);
                        }
                            break;

                        case (int) TessEventType.EVENT_START:
                        {
                            SplitHulls(hulls, ref hullCount, points, events[i]);
                        }
                            break;

                        default:
                        {
                            MergeHulls(hulls, ref hullCount, points, events[i]);
                        }
                            break;
                    }
                }

                hulls.Dispose();
                events.Dispose();
            }


            void Prepare(NativeArray<TessEdge> edgesIn)
            {
                m_Stars = new NativeArray<TessStar>(edgesIn.Length, Allocator.Temp);

                for (int i = 0; i < edgesIn.Length; ++i)
                {
                    TessEdge e = edgesIn[i];
                    e.a = (edgesIn[i].a < edgesIn[i].b) ? edgesIn[i].a : edgesIn[i].b;
                    e.b = (edgesIn[i].a > edgesIn[i].b) ? edgesIn[i].a : edgesIn[i].b;
                    edgesIn[i] = e;
                    TessStar s = m_Stars[i];
                    s.points = new ArraySlice<int>(m_SPArray, i * m_StarCount, m_StarCount);
                    s.pointCount = 0;
                    m_Stars[i] = s;
                }

                unsafe
                {
                    TessUtils.InsertionSort<TessEdge, TessEdgeCompare>(
                        NativeArrayUnsafeUtility.GetUnsafeBufferPointerWithoutChecks(edgesIn), 0, edgesIn.Length - 1,
                        new TessEdgeCompare());
                }

                m_Edges = new NativeArray<TessEdge>(edgesIn.Length, Allocator.Temp);
                m_Edges.CopyFrom(edgesIn);

                // Fill stars.
                for (int i = 0; i < m_CellCount; ++i)
                {
                    int a = m_Cells[i].a;
                    int b = m_Ce