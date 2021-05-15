.Lens.OrthographicSize;
#if UNITY_2018_2_OR_NEWER
                    else
                    {
                        cam.usePhysicalProperties = state.Lens.IsPhysicalCamera;
                        cam.lensShift = state.Lens.LensShift;
                    }
    #if CINEMACHINE_HDRP
                    if (state.Lens.IsPhysicalCamera)
                    {
#if UNITY_2019_2_OR_NEWER
                        cam.TryGetComponent<HDAdditionalCameraData>(out var hda);
#else
                        var hda = cam.GetComponent<HDAdditionalCameraData>();
#endif
                        if (hda != null)
                        {
                            hda.physicalParameters.iso = state.Lens.Iso;
                            hda.physicalParameters.shutterSpeed = state.Lens.ShutterSpeed;
                            hda.physicalParameters.aperture = state.Lens.Aperture;
                            hda.physicalParameters.bladeCount = state.Lens.BladeCount;
                            hda.physicalParameters.curvature = state.Lens.Curvature;
                            hda.physicalParameters.barrelClipping = state.Lens.BarrelClipping;
                            hda.physicalParameters.anamorphism = state.Lens.Anamorphism;
                        }
                    }
    #endif
#endif
                }
            }
            if (CinemachineCore.CameraUpdatedEvent != null)
                CinemachineCore.CameraUpdatedEvent.Invoke(this);
        }
    }
}
                                                                                                                                        