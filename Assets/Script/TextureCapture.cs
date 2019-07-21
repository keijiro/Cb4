using UnityEngine;
using System.Collections;
using FFmpegOut;

namespace Cb4
{
    public sealed class TextureCapture : MonoBehaviour
    {
        #region Public properties

        [SerializeField] RenderTexture _source = null;

        public RenderTexture source {
            get { return _source; }
            set { _source = value; }
        }

        [SerializeField] FFmpegPreset _preset;

        public FFmpegPreset preset {
            get { return _preset; }
            set { _preset = value; }
        }

        [SerializeField] float _frameRate = 60;

        public float frameRate {
            get { return _frameRate; }
            set { _frameRate = value; }
        }

        #endregion

        #region Private members

        FFmpegSession _session;

        #endregion

        #region Time-keeping variables

        int _frameCount;
        float _startTime;
        int _frameDropCount;

        float FrameTime {
            get { return _startTime + (_frameCount - 0.5f) / _frameRate; }
        }

        void WarnFrameDrop()
        {
            if (++_frameDropCount != 10) return;

            Debug.LogWarning(
                "Significant frame droppping was detected. This may introduce " +
                "time instability into output video. Decreasing the recording " +
                "frame rate is recommended."
            );
        }

        #endregion

        #region MonoBehaviour implementation

        void OnDisable()
        {
            if (_session != null)
            {
                // Close and dispose the FFmpeg session.
                _session.Close();
                _session.Dispose();
                _session = null;
            }
        }

        IEnumerator Start()
        {
            // Sync with FFmpeg pipe thread at the end of every frame.
            for (var eof = new WaitForEndOfFrame();;)
            {
                yield return eof;
                _session?.CompletePushFrames();
            }
        }

        void Update()
        {
            // Lazy initialization
            if (_session == null)
            {
                // Start an FFmpeg session.
                _session = FFmpegSession.Create(
                    gameObject.name,
                    _source.width,
                    _source.height,
                    _frameRate, preset
                );

                _startTime = Time.time;
                _frameCount = 0;
                _frameDropCount = 0;
            }

            var gap = Time.time - FrameTime;
            var delta = 1 / _frameRate;

            if (gap < 0)
            {
                // Update without frame data.
                _session.PushFrame(null);
            }
            else if (gap < delta)
            {
                // Single-frame behind from the current time:
                // Push the current frame to FFmpeg.
                _session.PushFrame(_source);
                _frameCount++;
            }
            else if (gap < delta * 2)
            {
                // Two-frame behind from the current time:
                // Push the current frame twice to FFmpeg. Actually this is not
                // an efficient way to catch up. We should think about
                // implementing frame duplication in a more proper way. #fixme
                _session.PushFrame(_source);
                _session.PushFrame(_source);
                _frameCount += 2;
            }
            else
            {
                // Show a warning message about the situation.
                WarnFrameDrop();

                // Push the current frame to FFmpeg.
                _session.PushFrame(_source);

                // Compensate the time delay.
                _frameCount += Mathf.FloorToInt(gap * _frameRate);
            }
        }

        #endregion
    }
}
