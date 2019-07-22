using UnityEngine;
using System.Collections.Generic;

namespace Cb4
{
    sealed class ShadowMaster : MonoBehaviour
    {
        #region Editable attributes

        [SerializeField] SkinnedMeshRenderer _source = null;
        [SerializeField, Range(1, 32)] int _shadowCount = 3;
        [SerializeField, Range(0.01f, 0.1f)] float _interval = 0.05f;
        [SerializeField, Range(0, 10)] float _offsetSpeed = 1;
        [SerializeField] Material _material = null;

        #endregion

        #region Provate members

        Queue<(Mesh mesh, Matrix4x4 matrix, float time)> _shadows;
        MaterialPropertyBlock _props;
        float _time;

        void MakeShadow()
        {
            var mesh = _shadows.Count < _shadowCount ?
                new Mesh() : _shadows.Dequeue().mesh;
            _source.BakeMesh(mesh);

            var matrix = _source.transform.localToWorldMatrix;

            _shadows.Enqueue((mesh, matrix, Time.time));
        }

        float CalculateProgress(float baseTime)
        {
            var p = (Time.time - baseTime) / (_interval * _shadowCount);
            return Mathf.Clamp01(p);
        }

        Matrix4x4 CalculateOffset(float baseTime)
        {
            var offs = _offsetSpeed * (Time.time - baseTime);
            return Matrix4x4.Translate(new Vector3(0, 0, offs));
        }

        #endregion

        void Start()
        {
            _shadows = new Queue<(Mesh, Matrix4x4, float)>();
            _props = new MaterialPropertyBlock();
        }

        void Update()
        {
            _time += Time.deltaTime;

            if (_time > _interval)
            {
                MakeShadow();
                _time -= _interval;
            }

            foreach (var shadow in _shadows)
            {
                var time = shadow.time;

                _props.SetFloat("_Progress", CalculateProgress(time));

                Graphics.DrawMesh(
                    shadow.mesh,
                    CalculateOffset(time) * shadow.matrix,
                    _material, gameObject.layer, null, 0, _props
                );
            }
        }

    }
}
