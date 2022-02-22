﻿using System;
using System.Collections.Generic;
using System.Text;

namespace KobukiCore.KobukiAssets.Skeleton.Data
{
    public class Joint
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
    }

    public enum JointNames
    {
        left_wrist,
        left_thumb_cmc,
        left_thumb_mcp,
        left_thumb_ip,
        left_thumb_tip,
        left_index_cmc,
        left_index_mcp,
        left_index_ip,
        left_index_tip,
        left_middle_cmc,
        left_middle_mcp,
        left_middle_ip,
        left_middle_tip,
        left_ring_cmc,
        left_ring_mcp,
        left_ring_ip,
        left_ringy_tip,
        left_pinky_cmc,
        left_pink_mcp,
        left_pink_ip,
        left_pink_tip,
        right_wrist,
        right_thumb_cmc,
        right_thumb_mcp,
        right_thumb_ip,
        right_thumb_tip,
        right_index_cmc,
        right_index_mcp,
        right_index_ip,
        right_index_tip,
        right_middle_cmc,
        right_middle_mcp,
        right_middle_ip,
        right_middle_tip,
        right_ring_cmc,
        right_ring_mcp,
        right_ring_ip,
        right_ringy_tip,
        right_pinky_cmc,
        right_pink_mcp,
        right_pink_ip,
        right_pink_tip,
        nose, left_eye_in,
        left_eye,
        left_eye_out,
        right_eye_in,
        right_eye,
        right_eye_out,
        left_ear,
        right_ear,
        mounth_left,
        mounth_right,
        left_shoulder,
        right_shoulder,
        left_elbow,
        right_elbow,
        left_wrist_glob,
        right_wrist_glob,
        left_pinky,
        right_pinky,
        left_index,
        right_index,
        left_thumb,
        right_thumb,
        left_hip,
        right_hip,
        left_knee,
        right_knee,
        left_ankle,
        right_ankle,
        left_heel,
        righ_heel,
        left_foot_index,
        right_foot_index
    }
}
