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

        void Start()
        {
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
                var offset = CalculateOffset(shadow.time);
                Graphics.DrawMesh(
                    shadow.mesh, offset * shadow.matrix,
                    _source.sharedMaterial, gameObject.layer
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
