using UnityEngine;
using System.Collections.Generic;

namespace Cb4
{
    sealed class ShadowMaster : MonoBehaviour
    {
        [SerializeField] SkinnedMeshRenderer _source = null;
        [SerializeField, Range(1, 32)] int _shadowCount = 3;
        [SerializeField, Range(0.01f, 1)] float _interval = 0.5f;
        [SerializeField] float _offsetSpeed = 1;

        Queue<(Mesh mesh, Matrix4x4 matrix, float time)> _shadows
            = new Queue<(Mesh, Matrix4x4, float)>();
        float _time;

        MaterialPropertyBlock _props;

        void Start()
        {
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

                var offset = CalculateOffset(time);

                _props.SetFloat("_AlphaCutoff", Mathf.Clamp01((Time.time - time)*2));

                Graphics.DrawMesh(
                    shadow.mesh, offset * shadow.matrix,
                    _source.sharedMaterial, gameObject.layer,
                    null, 0, _props
                );
            }
        }

        void MakeShadow()
        {
            Mesh mesh;

            if (_shadows.Count < _shadowCount)
            {
                mesh = new Mesh();
            }
            else
            {
                mesh = _shadows.Dequeue().mesh;
            }

            _source.BakeMesh(mesh);

            var matrix = _source.transform.localToWorldMatrix;
            _shadows.Enqueue((mesh, matrix, Time.time));
        }

        Matrix4x4 CalculateOffset(float time)
        {
            var offs = _offsetSpeed * (Time.time - time);
            return Matrix4x4.Translate(new Vector3(0, 0, offs));
        }
    }
}
