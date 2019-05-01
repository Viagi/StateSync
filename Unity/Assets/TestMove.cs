using PF;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ETModel;
using System.Threading.Tasks;

public class TestMove : MonoBehaviour
{
    public TextAsset Graph;
    public float Speed = 5.0f;

    private float deltaTime;
    private Vector3 virtualPos;
    private Vector3 deltaStart;
    private Vector3? deltaEnd;
    private Queue<Vector3> movePath = new Queue<Vector3>();

    // Use this for initialization
    void Start()
    {
        this.virtualPos = this.transform.position;

        AStarConfig config = new AStarConfig();
        config.graphs = DeserializeHelper.Load(this.Graph.bytes);

        MoveUpdate();
    }

    private async void MoveUpdate()
    {
        while (Application.isPlaying)
        {
            Vector2 axis = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            if (axis != Vector2.zero)
            {
                Vector3 move = new Vector3(axis.x, 0, axis.y).normalized * 0.05f * this.Speed;
                NNInfo nearInfo = PathFindHelper.GetNearest(this.virtualPos + move);

                if (nearInfo.position != this.virtualPos)
                {
                    this.virtualPos = nearInfo.position;
                    this.movePath.Enqueue(this.virtualPos);
                }
            }

            await Task.Delay(50);
        }
    }

    private void Update()
    {
        if (this.deltaEnd == null && this.movePath.Count > 0)
        {
            this.deltaTime = 0;
            this.deltaStart = this.transform.position;
            this.deltaEnd = this.movePath.Dequeue();
        }

        if (this.deltaEnd != null)
        {
            this.deltaTime += Time.deltaTime / 0.05f;

            Vector3 deltaPos = Vector3.Lerp(this.deltaStart, this.deltaEnd.Value, this.deltaTime);
            Vector3 direction = (deltaPos - this.transform.position).normalized;
            this.transform.position = deltaPos;

            if (this.deltaTime >= 1)
            {
                this.deltaEnd = null;
            }
        }

        Vector2 axis = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (axis != Vector2.zero)
        {
            this.transform.rotation = Quaternion.LookRotation(new Vector3(axis.x, 0, axis.y), Vector3.up);
        }
    }
}
