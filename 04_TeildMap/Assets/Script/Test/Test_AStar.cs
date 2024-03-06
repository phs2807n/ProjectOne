using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_AStar : TestBase
{
    private void Start()
    {
        // Node node = new Node();

        Node node1 = new Node(10, 20);
        Node node2 = new Node(10, 20);

        List<Node> nodes = new List<Node>();
        nodes.Sort();

        int i = 0;
    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        List<int> list = new List<int>();
        list.Add(3);
        list.Add(5);
        list.Add(2);
        list.Add(4);
        list.Add(6);
        list.Add(1);

        list.Sort();

        int i = 0;
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        Node node1 = new Node(10, 20);
        node1.G = 10;
        node1.H = 0;
    }
}
