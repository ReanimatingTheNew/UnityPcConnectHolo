﻿//
// Copyright (C) 2014 Sixense Entertainment Inc.
// All Rights Reserved
//
// Sixense STEM Test Application
// Version 0.1
//

using UnityEngine;
using System.Collections.Generic;

public class TestGUI : MonoBehaviour
{
    #region Types
    public enum Page
    {
        Tracking,
        Status,
        Settings,
    }
    #endregion

    #region Public Variables
    [Tooltip("Model of the base station in the scene.")]
    public SixenseCore.BaseVisual m_base;

    [Tooltip("Model of the table surface in the scene.")]
    public Transform m_table;

    [Tooltip("Material for position history line plot visualization.")]
    public Material m_material;
    [Tooltip("Material for position history line plot shadow visualization.")]
    public Material m_materialShadow;
    #endregion

    #region Private Variables
    List<int> m_historyID = new List<int>(10000);
    List<Vector3> m_historyPosition = new List<Vector3>(10000);
    List<Quaternion> m_historyRotation = new List<Quaternion>(10000);
    List<int> m_historySequence = new List<int>(10000);
    List<float> m_historyTime = new List<float>(10000);
    int m_recorded = 0;

    Vector2 m_scroll = Vector2.zero;
    Page m_page = Page.Status;
    int m_pnopage = 0;
    bool m_usingNetwork = false;

    int[] m_lastSequence;
    int[] m_percent;
    int[] m_count;
    int[] m_samples;

    float[] m_vibration_magnitude;
    int[] m_vibration_duration;
    uint m_hemi_tracking_init_vec_tracker_id = 0;
    Vector3 m_hemi_tracking_init_vec = new Vector3();
    #endregion

    #region History Recording
    void Start()
    {
        if (!SixenseCore.Device.Initialized)
        {
            Debug.LogWarning("SixenseCore not initialized.");
        }

        m_lastSequence = new int[SixenseCore.Device.MaxNumberTrackers];
        m_percent = new int[SixenseCore.Device.MaxNumberTrackers];
        m_count = new int[SixenseCore.Device.MaxNumberTrackers];
        m_samples = new int[SixenseCore.Device.MaxNumberTrackers];
        m_vibration_magnitude = new float[SixenseCore.Device.MaxNumberTrackers];
        m_vibration_duration = new int[SixenseCore.Device.MaxNumberTrackers];

        for (int i = 0; i < SixenseCore.Device.MaxNumberTrackers; i++)
        {
            m_vibration_magnitude[i] = 0.0f;
            m_vibration_duration[i] = 2000;
        }
    }

    void FixedUpdate()
    {
        if (!SixenseCore.Device.Initialized)
        {
            return;
        }

        for (int i = 0; i < SixenseCore.Device.MaxNumberTrackers; i++)
        {
            var c = SixenseCore.Device.GetTrackerByIndex(i);
            if (!c.Enabled)
                continue;

            // check for out of sequence packets
            if (m_lastSequence[i] == -1)
            {
                m_lastSequence[i] = c.SequenceNumber;
            }
            else if (m_lastSequence[i] == c.SequenceNumber)
            {
            }
            else if ((m_lastSequence[i] == 255 && c.SequenceNumber != 0) ||
                    m_lastSequence[i] != c.SequenceNumber - 1)
            {
                m_count[i]++;
                m_samples[i]++;
            }
            else
            {
                m_samples[i]++;
            }

            if (m_samples[i] >= 240)
            {
                m_percent[i] = 100 * m_count[i] / m_samples[i];
                m_count[i] = 0;
                m_samples[i] = 0;
            }

            m_lastSequence[i] = c.SequenceNumber;

            // record history
            if (i == m_recorded && ((!c.GetButtonDown(SixenseCore.Buttons.BUMPER) && c.GetButton(SixenseCore.Buttons.BUMPER)) ||
                (!Input.GetKeyDown(KeyCode.Space) && Input.GetKey(KeyCode.Space))))
            {
                int index = m_historyPosition.Count;
                m_historyID.Add((int)c.DeviceIndex);

                m_historyPosition.Add(c.Position);
                m_historyRotation.Add(c.Rotation);

                /*m_historyPosition[index] = m_base.m_emitter.TransformPoint(m_historyPosition[index]);
                m_historyRotation[index] = m_base.m_emitter.rotation * c.Rotation;*/

                m_historyTime.Add(Time.deltaTime * 1000f);
                m_historySequence.Add(c.SequenceNumber);
                break;
            }
        }
    }

    void Update()
    {
        if (!SixenseCore.Device.Initialized)
        {
            return;
        }

        for (int i = 0; i < SixenseCore.Device.MaxNumberTrackers; i++)
        {
            var c = SixenseCore.Device.GetTrackerByIndex(i);

            if (!c.Enabled)
                continue;

            if (Input.GetKeyDown(KeyCode.Space) || c.GetButtonDown(SixenseCore.Buttons.BUMPER))
            {
                m_historyID.Clear();
                m_historyPosition.Clear();
                m_historyRotation.Clear();
                m_historySequence.Clear();
                m_historyTime.Clear();
                m_recorded = i;
            }
        }
    }
    #endregion

    #region Visual Output
    void OnPostRender()
    {
        float time = 0;
        var vertices = new Vector3[m_historyPosition.Count];
        var verticesShadow = new Vector3[m_historyPosition.Count];

        for (int h = 0; h < m_historyPosition.Count; h++)
        {
            vertices[h] = m_historyPosition[h];

            Vector3 shadow1 = new Vector3(m_historyPosition[h].x, m_table.position.y + 0.001f, m_historyPosition[h].z);

            verticesShadow[h] = shadow1;

            time -= m_historyTime[h];
        }

        if (Camera.current != null)
        {
            GL.PushMatrix();
            GL.LoadProjectionMatrix(Camera.current.projectionMatrix);
            GL.modelview = Camera.current.worldToCameraMatrix;
            GL.Viewport(Camera.current.pixelRect);

            m_material.SetPass(0);

            GL.Begin(GL.LINES);
            for (int v = 0; v < vertices.Length; v++)
            {
                GL.Vertex(vertices[v]);
            }
            GL.End();

            m_materialShadow.SetPass(0);

            GL.Begin(GL.LINES);
            for (int v = 0; v < verticesShadow.Length; v++)
            {
                GL.Vertex(verticesShadow[v]);
            }
            GL.End();
            GL.PopMatrix();
        }
    }
    #endregion

    #region Textual Output
    Vector3 getPitchYawRoll(Quaternion q)
    {
        var euler = q.eulerAngles;
        if (euler.x > 180)
            euler.x -= 360;
        if (euler.y > 180)
            euler.y -= 360;
        if (euler.z > 180)
            euler.z -= 360;

        euler.x *= -1;
        return euler;
    }

    void OnGUI()
    {
        float scale = Mathf.Max(Screen.height, Screen.width) / 1000.0f;
        int fontsize = Mathf.FloorToInt(13.0f * scale);
        GUI.skin.label.fontSize = fontsize;
        GUI.skin.toggle.fontSize = fontsize;
        GUI.skin.textField.fontSize = fontsize;
        GUI.skin.textArea.fontSize = fontsize;
        GUI.skin.button.fontSize = fontsize;

        float w = Screen.width * 0.3f;

        GUILayout.BeginArea(new Rect(Screen.width * 0.4f, 0, w, Screen.height));
        {
            GUILayout.Label("Core API Version: " + SixenseCore.Device.APIVersion);
            GUILayout.Label("System Type: " + SixenseCore.Device.SystemType.ToString());
        }
        GUILayout.EndArea();

        GUILayout.BeginArea(new Rect(Screen.width * 0.7f, 0, w, Screen.height));
        {
            GUILayout.BeginHorizontal();
            {
                if (GUILayout.Toggle(m_page == Page.Tracking, "Tracking"))
                    m_page = Page.Tracking;
                if (GUILayout.Toggle(m_page == Page.Status, "Status"))
                    m_page = Page.Status;
                if (GUILayout.Toggle(m_page == Page.Settings, "Settings"))
                    m_page = Page.Settings;
            }
            GUILayout.EndHorizontal();

            switch (m_page)
            {
                case Page.Tracking:
                    ShowPnO(w);
                    break;
                case Page.Status:
                    ShowStatus(w);
                    break;
                case Page.Settings:
                    ShowSettings(w);
                    break;
            }
        }
        GUILayout.EndArea();
    }

    private void ShowStatus(float w)
    {
        if (!SixenseCore.Device.Initialized)
        {
            GUILayout.Label("SixenseCore Device Not Initialized");
            return;
        }

        m_scroll = GUILayout.BeginScrollView(m_scroll);

        uint active = SixenseCore.Device.NumberActiveTrackers;

        if (!SixenseCore.Device.BaseConnected)
        {
            GUILayout.Label("No Connected Base Station");
        }
        else if (active == 0)
        {
            GUILayout.Label("No Connected Trackers");
        }
        else
        {
            GUILayout.Label("Connected: " + active);
        }

#if !WINDOWS_UWP
        if (Application.platform == RuntimePlatform.Android)
        {
            System.Net.IPHostEntry host;
            string localIP = "";
            host = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
            foreach (System.Net.IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    localIP = ip.ToString();
                    break;
                }
            }

            GUILayout.Label("Local IP Address: " + localIP);
        }
#endif

        for (int i = 0; i < SixenseCore.Device.MaxNumberTrackers; i++)
        {
            var con = SixenseCore.Device.GetTrackerByIndex(i);
            if (!con.Connected)
                continue;

            if (con.WiredConnection)
                GUILayout.Label(con.HardwareType.ToString() + " " + con.DeviceIndex + " WIRED");
            else if (!con.Enabled)
                GUILayout.Label(con.HardwareType.ToString() + " " + con.DeviceIndex + " NOT READY");
            else if (con.Docked)
                GUILayout.Label(con.HardwareType.ToString() + " " + con.DeviceIndex + " DOCKED");
            else
                GUILayout.Label(con.HardwareType.ToString() + " " + con.DeviceIndex);

            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("ID: " + con.ID.ToString());
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("HW " + con.HardwareVersion + ",", GUILayout.Width(w / 2 - 1));
                GUILayout.Label("Serial " + con.SerialNumber.ToString(), GUILayout.Width(w / 3 - 1));
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("FW " + con.FirmwareVersion + ",", GUILayout.Width(w / 2 - 1));
                GUILayout.Label("Runtime " + con.RuntimeVersion, GUILayout.Width(w / 3 - 1));
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("Freq " + con.MagneticFrequency.ToString("X2"), GUILayout.Width(w / 2 - 1));
                GUILayout.Label("Gain " + con.Gain.ToString("X2"), GUILayout.Width(w / 3 - 1));
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("Battery: ", GUILayout.Width(w / 5 - 1));

                GUILayout.Label(con.BatteryPercentage + (con.BatteryLow ? "% L" : "%"), GUILayout.Width(w / 5 - 1));

                if (con.ExternalPower)
                    GUILayout.Label("Powered", GUILayout.Width(w / 5 - 1));
                if (con.Charging)
                    GUILayout.Label("Charging", GUILayout.Width(w / 5 - 1));
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("Percent Out of Order: " + m_percent[i]);
            }
            GUILayout.EndHorizontal();

            if (!(con.HardwareType == SixenseCore.Hardware.STEM_PACK))
            {
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label("Analog:", GUILayout.Width(w / 5 - 1));
                    GUILayout.Label("x " + Mathf.RoundToInt(128 * con.JoystickX).ToString() + ",", GUILayout.Width(w / 5 - 1));
                    GUILayout.Label("y " + Mathf.RoundToInt(128 * con.JoystickY).ToString() + ",", GUILayout.Width(w / 5 - 1));
                    GUILayout.Label("t " + Mathf.RoundToInt(255 * con.Trigger).ToString(), GUILayout.Width(w / 5 - 1));
                }
                GUILayout.EndHorizontal();
            }

            GUILayout.BeginHorizontal();
            string buttons = "";
            if (con.GetButton(SixenseCore.Buttons.TRIGGER)) buttons += "Trigger ";
            if (con.GetButton(SixenseCore.Buttons.BUMPER)) buttons += "Bumper ";
            if (con.GetButton(SixenseCore.Buttons.JOYSTICK)) buttons += "Stick ";
            if (con.HardwareType == SixenseCore.Hardware.HYDRA_CONTROLLER)
            {
                if (con.GetButton(SixenseCore.Buttons.A)) buttons += "3 ";
                if (con.GetButton(SixenseCore.Buttons.B)) buttons += "1 ";
                if (con.GetButton(SixenseCore.Buttons.X)) buttons += "2 ";
                if (con.GetButton(SixenseCore.Buttons.Y)) buttons += "4 ";
            } else
            {
                if (con.GetButton(SixenseCore.Buttons.A)) buttons += "A ";
                if (con.GetButton(SixenseCore.Buttons.B)) buttons += "B ";
                if (con.GetButton(SixenseCore.Buttons.X)) buttons += "X ";
                if (con.GetButton(SixenseCore.Buttons.Y)) buttons += "Y ";
            }
            if (con.GetButton(SixenseCore.Buttons.PREV)) buttons += "Prev ";
            if (con.GetButton(SixenseCore.Buttons.NEXT)) buttons += "Next ";
            if (con.GetButton(SixenseCore.Buttons.START)) buttons += "Start ";
            GUILayout.Label("Buttons:   " + buttons);
            GUILayout.EndHorizontal();

            if (Application.platform == RuntimePlatform.Android)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label("Packet DT: " + con.LastPacketDT);
                GUILayout.EndHorizontal();
            }

            GUILayout.Space(10);
        }
        GUILayout.EndScrollView();
    }

    private void ShowSettings(float w)
    {
        if (!SixenseCore.Device.Initialized)
        {
            GUILayout.Label("SixenseCore Device Not Initialized");
            return;
        }

        GUILayout.Label("All Trackers");

        SixenseCore.Device.DistortionCorrectionEnabled = GUILayout.Toggle(SixenseCore.Device.DistortionCorrectionEnabled, "Distortion Correction");

        bool filtering = GUILayout.Toggle(SixenseCore.Device.FilterMovingAverageEnabled, "Moving Average Filter");
        SixenseCore.Device.FilterMovingAverageEnabled = filtering;

        if (filtering)
        {
            var settings = SixenseCore.Device.FilterMovingAverageSettings;

            bool c = GUI.changed;
            if (c) GUI.changed = false;
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label("Ranges");
                string nrs = GUILayout.TextArea(settings.NearRange.ToString());
                GUILayout.Label("-");
                string frs = GUILayout.TextArea(settings.FarRange.ToString());
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Label("Pos");
                string nvp = GUILayout.TextArea(settings.NearPosExp.ToString("0.0##"));
                GUILayout.Label("-");
                string fvp = GUILayout.TextArea(settings.FarPosExp.ToString("0.0##"));
                GUILayout.Label("Rot");
                string nvr = GUILayout.TextArea(settings.NearRotExp.ToString("0.0##"));
                GUILayout.Label("-");
                string fvr = GUILayout.TextArea(settings.FarRotExp.ToString("0.0##"));
                GUILayout.EndHorizontal();

                if (GUI.changed &&
                    float.TryParse(nrs, out settings.NearRange) &&
                    float.TryParse(frs, out settings.FarRange) &&
                    float.TryParse(nvp, out settings.NearPosExp) &&
                    float.TryParse(fvp, out settings.FarPosExp) &&
                    float.TryParse(nvr, out settings.NearRotExp) &&
                    float.TryParse(fvr, out settings.FarRotExp))
                {
                    SixenseCore.Device.FilterMovingAverageSettings = settings;
                }
            }
            GUI.changed |= c;
        }

        bool movingAverageWindowfiltering = GUILayout.Toggle(SixenseCore.Device.FilterMovingAverageWindowEnabled, "Moving Average Window Filter");
        SixenseCore.Device.FilterMovingAverageWindowEnabled = movingAverageWindowfiltering;

        if (movingAverageWindowfiltering)
        {
            var settings = SixenseCore.Device.FilterMovingAverageWindowSettings;

            bool c = GUI.changed;
            if (c) GUI.changed = false;
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label("Ranges");
                string nrs = GUILayout.TextArea(settings.NearRange.ToString());
                GUILayout.Label("-");
                string frs = GUILayout.TextArea(settings.FarRange.ToString());
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Label("# Frames");
                string nws = GUILayout.TextArea(settings.NearWindowSize.ToString(""));
                GUILayout.Label("-");
                string fws = GUILayout.TextArea(settings.FarWindowSize.ToString(""));
                GUILayout.EndHorizontal();

                if (GUI.changed &&
                    float.TryParse(nrs, out settings.NearRange) &&
                    float.TryParse(frs, out settings.FarRange) &&
                    uint.TryParse(nws, out settings.NearWindowSize) &&
                    uint.TryParse(fws, out settings.FarWindowSize))
                {
                    SixenseCore.Device.FilterMovingAverageWindowSettings = settings;
                }
            }
            GUI.changed |= c;
        }

        bool doubleMovingAverageWindowfiltering = GUILayout.Toggle(SixenseCore.Device.FilterDoubleMovingAverageWindowEnabled, "Double Moving Average Window Filter");
        SixenseCore.Device.FilterDoubleMovingAverageWindowEnabled = doubleMovingAverageWindowfiltering;

        if (doubleMovingAverageWindowfiltering)
        {
            var settings = SixenseCore.Device.FilterDoubleMovingAverageWindowSettings;

            bool c = GUI.changed;
            if (c) GUI.changed = false;
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label("Ranges");
                string nrs = GUILayout.TextArea(settings.NearRange.ToString());
                GUILayout.Label("-");
                string frs = GUILayout.TextArea(settings.FarRange.ToString());
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Label("# Frames");
                string nws = GUILayout.TextArea(settings.NearWindowSize.ToString(""));
                GUILayout.Label("-");
                string fws = GUILayout.TextArea(settings.FarWindowSize.ToString(""));
                GUILayout.EndHorizontal();

                if (GUI.changed &&
                    float.TryParse(nrs, out settings.NearRange) &&
                    float.TryParse(frs, out settings.FarRange) &&
                    uint.TryParse(nws, out settings.NearWindowSize) &&
                    uint.TryParse(fws, out settings.FarWindowSize))
                {
                    SixenseCore.Device.FilterDoubleMovingAverageWindowSettings = settings;
                }
            }
            GUI.changed |= c;
        }

        GUILayout.Space(10);

        bool gui_changed_hemi_vec = GUI.changed;
        if (gui_changed_hemi_vec) GUI.changed = false;
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Hemi Tracking Init Vector");
            string h_x = GUILayout.TextArea(m_hemi_tracking_init_vec.x.ToString("0.0##"));
            string h_y = GUILayout.TextArea(m_hemi_tracking_init_vec.y.ToString("0.0##"));
            string h_z = GUILayout.TextArea(m_hemi_tracking_init_vec.z.ToString("0.0##"));
            GUILayout.EndHorizontal();

            if (GUI.changed)
            {
                float.TryParse(h_x, out m_hemi_tracking_init_vec.x);
                float.TryParse(h_y, out m_hemi_tracking_init_vec.y);
                float.TryParse(h_z, out m_hemi_tracking_init_vec.z);
            }
        }
        GUI.changed |= gui_changed_hemi_vec;

        bool gui_changed_hemi_tracker_id = GUI.changed;
        if (gui_changed_hemi_tracker_id) GUI.changed = false;
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Tracker ID");
            string h_id = GUILayout.TextArea(m_hemi_tracking_init_vec_tracker_id.ToString());
            if (GUILayout.Button("Set"))
            {
                SixenseCore.Device.SetHemiTrackingInitVector(m_hemi_tracking_init_vec_tracker_id, m_hemi_tracking_init_vec);
            }
            GUILayout.EndHorizontal();

            if (GUI.changed &&
                uint.TryParse(h_id, out m_hemi_tracking_init_vec_tracker_id))
            {
                m_hemi_tracking_init_vec = SixenseCore.Device.GetHemiTrackingInitVector(m_hemi_tracking_init_vec_tracker_id);
            }
        }
        GUI.changed |= gui_changed_hemi_tracker_id;

        GUILayout.Space(10);

        if (Application.platform != RuntimePlatform.Android)
        {
            if (SixenseCore.Device.NetworkBridgeEnabled)
                m_usingNetwork = true;

            m_usingNetwork = GUILayout.Toggle(m_usingNetwork, "Network Bridge");

            if (!m_usingNetwork)
                SixenseCore.Device.NetworkBridgeEnabled = false;

            if (m_usingNetwork)
            {
                for (int i = 0; i < 4; i++)
                {
                    GUILayout.BeginHorizontal();

                    bool c = GUI.changed;
                    if (c) GUI.changed = false;
                    {
                        GUILayout.Label("IP Address " + i);
                        string address = GUILayout.TextArea(SixenseCore.Device.NetworkBridgeAddresses[i]);

                        if (GUI.changed)
                        {
                            SixenseCore.Device.NetworkBridgeEnabled = false;
                            SixenseCore.Device.NetworkBridgeAddresses[i] = address;
                        }
                    }
                    GUI.changed |= c;

                    GUILayout.EndHorizontal();
                }

                GUILayout.BeginHorizontal();

                if (!SixenseCore.Device.NetworkBridgeEnabled &&
                    GUILayout.Button("Connect"))
                {
                    SixenseCore.Device.NetworkBridgeEnabled = true;
                }

                GUILayout.EndHorizontal();
            }
        }

        m_scroll = GUILayout.BeginScrollView(m_scroll);
        for (int i = 0; i < SixenseCore.Device.MaxNumberTrackers; i++)
        {
            var con = SixenseCore.Device.GetTrackerByIndex(i);
            if (con.Connected)
            {
                GUILayout.Space(10);

                GUILayout.Label(con.ID.ToString() + " (" + con.DeviceIndex + (filtering ? ") Current Range: " + Mathf.RoundToInt(con.Position.magnitude * 1000).ToString() : ")"));

                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Update Info"))
                {
                    con.UpdateInfo();
                }
                if (GUILayout.Button("Sync / Set Hemi"))
                {
                    con.Sync();
                    con.AutoEnableHemisphereTracking();
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Label("Set Power:");
                if (GUILayout.Button("OFF"))
                {
                    con.PowerOff(SixenseCore.PowerState.SLEEP);
                }
                // shipping mode only for STEM hardaware
                if ((con.HardwareType == SixenseCore.Hardware.STEM_CONTROLLER) ||
                    (con.HardwareType == SixenseCore.Hardware.STEM_PACK) ||
                    (con.HardwareType == SixenseCore.Hardware.PROTOTYPE_01_CONTROLLER))
                {
                    // shipping mode only for MCU firmware version vC1.04 or greater
                    string[] firmware_version = con.FirmwareVersion.Split('-');
                    if (firmware_version.Length > 1)
                    {
                        string[] mcu_firmware_version = firmware_version[0].Split('.');
                        if ((mcu_firmware_version.Length > 1) && (int.Parse(mcu_firmware_version[0], System.Globalization.NumberStyles.HexNumber) >= 0xC1) && (int.Parse(mcu_firmware_version[1], System.Globalization.NumberStyles.HexNumber) >= 0x04))
                        {
                            if (GUILayout.Button("Shipping"))
                            {
                                con.PowerOff(SixenseCore.PowerState.SHIPPING_MODE);
                            }
                        }
                    }
                }
                GUILayout.EndHorizontal();

                if (con.Connected)
                {
                    // power off button mode only for STEM hardaware
                    if ((con.HardwareType == SixenseCore.Hardware.STEM_CONTROLLER) ||
                        (con.HardwareType == SixenseCore.Hardware.STEM_PACK) ||
                        (con.HardwareType == SixenseCore.Hardware.PROTOTYPE_01_CONTROLLER))
                    {
                        // power off button mode only for MCU firmware version vC1.04 or greater
                        string[] firmware_version = con.FirmwareVersion.Split('-');
                        if (firmware_version.Length > 1)
                        {
                            string[] mcu_firmware_version = firmware_version[0].Split('.');
                            if ((mcu_firmware_version.Length > 1) && (int.Parse(mcu_firmware_version[0], System.Globalization.NumberStyles.HexNumber) >= 0xC1) && (int.Parse(mcu_firmware_version[1], System.Globalization.NumberStyles.HexNumber) >= 0x04))
                            {
                                GUILayout.BeginHorizontal();
                                if (con.PowerOffButtonMode)
                                {
                                    GUILayout.Label("Power Off Button Mode [Enabled]:");
                                    if (GUILayout.Button("Disable"))
                                    {
                                        con.SetPowerOffButtonMode(false);
                                    }
                                }
                                else
                                {
                                    GUILayout.Label("Power Off Button Mode [Disabled]:");
                                    if (GUILayout.Button("Enable"))
                                    {
                                        con.SetPowerOffButtonMode(true);
                                    }
                                }
                                GUILayout.EndHorizontal();
                            }
                        }
                    }
  
                    // vibration
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Vibration");

                    // magnitude
                    bool c = GUI.changed;
                    if (c) GUI.changed = false;
                    {
                        GUILayout.Label("Magnitude");
                        string magnitude_string = GUILayout.TextArea(m_vibration_magnitude[i].ToString("0.0##"));
						
                        if (GUI.changed)
                        {
                            float magnitude = m_vibration_magnitude[i];
                            if (float.TryParse(magnitude_string, out magnitude))
                            {
                                m_vibration_magnitude[i] = magnitude;
                            }
                        }
                    }
                    GUI.changed |= c;

                    // duration
                    c = GUI.changed;
                    if (c) GUI.changed = false;
                    {
                        GUILayout.Label("Duration");
                        string duration_string = GUILayout.TextArea(m_vibration_duration[i].ToString());

                        if (GUI.changed)
                        {
                            int duration = m_vibration_duration[i];
                            if (int.TryParse(duration_string, out duration))
                            {
                                m_vibration_duration[i] = duration;
                            }
                        }
                    }
                    GUI.changed |= c;

                    if (GUILayout.Button("Set"))
                    {
                        con.Vibrate(m_vibration_duration[i], m_vibration_magnitude[i]);
                    }
                    GUILayout.EndHorizontal();
                }
            }
        }
        GUILayout.EndScrollView();
    }

    private void ShowPnO(float w)
    {
        if (!SixenseCore.Device.Initialized)
        {
            GUILayout.Label("SixenseCore Device Not Initialized");
            return;
        }

        var c = SixenseCore.Device.GetTrackerByIndex(m_recorded);
        if (c != null && (c.GetButton(SixenseCore.Buttons.BUMPER) || Input.GetKey(KeyCode.Space)))
        {
            GUILayout.Label("RECORDING...");
            return;
        }

        m_scroll = GUILayout.BeginScrollView(m_scroll);
        for (int i = 0; i < SixenseCore.Device.MaxNumberTrackers; i++)
        {
            var con = SixenseCore.Device.GetTrackerByIndex(i);
            if (!con.Enabled)
                continue;

            GUILayout.BeginHorizontal();
            {
                GUILayout.Label(con.DeviceIndex + " position (mm):", GUILayout.Width(2 * w / 6 - 1));
                var xyz = con.Position * 1000;
                GUILayout.Label("x " + Mathf.RoundToInt(xyz.x).ToString() + ",", GUILayout.Width(w / 6 - 1));
                GUILayout.Label("y " + Mathf.RoundToInt(xyz.y).ToString() + ",", GUILayout.Width(w / 6 - 1));
                GUILayout.Label("z " + Mathf.RoundToInt(-xyz.z).ToString(), GUILayout.Width(w / 6 - 1));
            }
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("   rotation (deg):", GUILayout.Width(2 * w / 6 - 1));
                var pyr = getPitchYawRoll(con.Rotation);
                GUILayout.Label("ex " + Mathf.RoundToInt(pyr.x).ToString() + ",", GUILayout.Width(w / 6 - 1));
                GUILayout.Label("ey " + Mathf.RoundToInt(pyr.y).ToString() + ",", GUILayout.Width(w / 6 - 1));
                GUILayout.Label("ez " + Mathf.RoundToInt(pyr.z).ToString(), GUILayout.Width(w / 6 - 1));
            }
            GUILayout.EndHorizontal();
            if ((con.HardwareType != SixenseCore.Hardware.PROTOTYPE_02_DAUGHTERBOARD) &&
                (con.HardwareType != SixenseCore.Hardware.PROTOTYPE_02_CONTROLLER) &&
                (con.HardwareType != SixenseCore.Hardware.PROTOTYPE_02_CONTROLLER_P1) &&
                (con.HardwareType != SixenseCore.Hardware.PROTOTYPE_02_CONTROLLER_P2) &&
                (con.HardwareType != SixenseCore.Hardware.PROTOTYPE_02_CONTROLLER_P3) &&
                (con.HardwareType != SixenseCore.Hardware.PROTOTYPE_02_CONTROLLER_P4) &&
                (con.HardwareType != SixenseCore.Hardware.STEM_HANDS_DAUGHTERBOARD) &&
                (con.HardwareType != SixenseCore.Hardware.STEM_HANDS_CONTROLLER))
            {
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label("   gravity (m/s/s):", GUILayout.Width(2 * w / 6 - 1));
                    var xyz = con.LocalGravity;
                    GUILayout.Label("x " + (xyz.x).ToString("0.00") + ",", GUILayout.Width(w / 6 - 1));
                    GUILayout.Label("y " + (xyz.y).ToString("0.00") + ",", GUILayout.Width(w / 6 - 1));
                    GUILayout.Label("z " + (-xyz.z).ToString("0.00"), GUILayout.Width(w / 6 - 1));
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.Space(5);

        }
        if (m_historyPosition.Count == 0)
        {
            uint active = SixenseCore.Device.NumberActiveTrackers;
            if (active == 0)
            {
                GUILayout.Label("Connect a STEM Tracker");
                GUILayout.EndScrollView();
            }
            else
            {
                GUILayout.EndScrollView();
                GUILayout.Label("Hold BUMPER or SPACE to Record");
            }
        }
        else
        {
            GUILayout.Label("History For Controller " + m_recorded);
            GUILayout.BeginHorizontal();
            {
                if (GUILayout.Toggle(m_pnopage == 0, "Position (mm)"))
                    m_pnopage = 0;
                if (GUILayout.Toggle(m_pnopage == 1, "Rotation (deg)"))
                    m_pnopage = 1;
            }
            GUILayout.EndHorizontal();

            float time = 0;
            for (int h = 0; h < m_historyPosition.Count; h++)
            {
                time -= m_historyTime[h];
                if (time > 0)
                    continue;

                time = 100;

                if (m_pnopage == 0)
                    GUILayout.Label(h + ": " + (m_historyPosition[h] * 1000).ToString());
                else if (m_pnopage == 1)
                    GUILayout.Label(h + ": " + getPitchYawRoll(m_historyRotation[h]).ToString());
            }

            GUILayout.EndScrollView();

            GUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("Clear"))
                    m_historyPosition.Clear();

                if (GUILayout.Button("Write Log"))
                    WriteLog();
            }
            GUILayout.EndHorizontal();
        }
    }
    #endregion

    #region Logging
    void WriteLog()
    {
        string[] lines = new string[m_historyPosition.Count];

        for (int h = 0; h < m_historyPosition.Count; h++)
        {
            int id = m_historyID[h];
            Vector3 pos = m_historyPosition[h] * 1000;
            Quaternion rot = m_historyRotation[h];
            float time = m_historyTime[h];
            int sequence = m_historySequence[h];

            lines[h] =
                "controller: " + id +
                " dt: " + time +
                " sequence: " + sequence +
                " pos: " + pos.x + " " + pos.y + " " + -pos.z +
                " quat: " + rot.x + " " + rot.y + " " + -rot.z + " " + -rot.w +
                " euler: " + rot.eulerAngles.x + " " + rot.eulerAngles.y + " " + rot.eulerAngles.z;
        }

#if !WINDOWS_UWP
        System.IO.File.WriteAllLines(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop) +
            "\\SixenseLog.txt", lines);
#endif
    }
    #endregion
}
