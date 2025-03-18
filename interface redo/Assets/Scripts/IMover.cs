using UnityEngine;

public interface IMover
{
    Vector3 Position { get; set; }
    void Move();
    void Remove();
}
