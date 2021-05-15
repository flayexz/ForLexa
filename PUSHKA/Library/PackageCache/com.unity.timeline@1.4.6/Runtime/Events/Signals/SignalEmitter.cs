        {
                        sb.Append("[");
                        sb.Append(vcam.Name);
                        sb.Append("]");
                    }
                }
                string text = sb.ToString();
                Rect r = CinemachineDebug.GetScreenPos(this, text, GUI.skin.box);
                GUI.Label(r, text, GUI.skin.box);
                GUI.color = color;
                CinemachineDebug.ReturnToPool(sb);
            }
        }

#if UNITY_EDITOR
        private void OnGUI()
        {
            if (CinemachineDebug.OnGUIHandlers != null)
                CinemachineDebug.OnGUIHandlers();
        }
#endif

        WaitForFixedUpdate mWaitForFixedUpdate = new WaitForFixedUpdate();
        private IEnumerator AfterPhysics()
        {
            while (true)
            {
                // FixedUpdate can be called multiple times per frame
                yield return mWaitForFixedUpdate;
                if (m_UpdateMethod == UpdateMethod.FixedUpdate
                    || m_UpdateMethod == UpdateMethod.SmartUpdate)
                {
                    CinemachineCore.UpdateFilter filter = CinemachineCore.UpdateFilter.Fixed;
                    if (m_UpdateMethod == UpdateMethod.SmartUpdate)
                    {
                        // Track the targets
                        UpdateTracker.OnUpdate(UpdateTracker.UpdateClock.Fixed);
                        filter = CinemachineCore.UpdateFilter.SmartFixed;
                    }
                    UpdateVirtualCameras(filter, GetEffectiveDeltaTime(true));
                }
                // Choose the active vcam and apply it to the Unity camera
                if (m_BlendUpdateMethod == BrainUpdateMethod.FixedUpdate)
                {
                    UpdateFrame0(Time.fixedDeltaTime);
                    ProcessActiveCamera(Time.fixedDeltaTime);
                }
            }
        }

        private void LateUpdate()
        {
            if (m_UpdateMethod != UpdateMethod.ManualUpdate)
                ManualUpdate();
        }

        /// <summary>
        /// Call this method explicitly from an external script to update the virtual cameras
        /// and position the main camera, if the Up